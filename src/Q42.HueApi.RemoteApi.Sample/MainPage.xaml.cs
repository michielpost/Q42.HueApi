using Q42.HueApi.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Q42.HueApi.RemoteApi.Sample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);


			string appId = "";
			string clientId = "";
			string clientSecret = "";

			//IRemoteHueClient client = new RemoteHueClient(clientSecret);
			var authorizeUri = RemoteHueClient.BuildAuthorizeUri(clientId, "sample", "consoleapp", appId);
			var callbackUri = new Uri("https://localhost/q42hueapi");

			var webAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, authorizeUri, callbackUri);

			if (webAuthenticationResult != null)
			{
				var result = RemoteHueClient.ProcessAuthorizeResponse(webAuthenticationResult.ResponseData);

				if (!string.IsNullOrEmpty(result.Code))
				{

					var requestUri = new Uri($"https://api.meethue.com/oauth2/token?code={result.Code}&grant_type=authorization_code");

					using (var httpClient = new HttpClient())
					{
							var responseTask = await httpClient.PostAsync(requestUri, null);
						var r = responseTask.Headers.WwwAuthenticate.ToString();
						r = r.Replace("Digest ", string.Empty);

						int startNonce = r.IndexOf("nonce=") + 7;
						int endNonce = r.IndexOf("\"", startNonce);
						string nonce = r.Substring(startNonce, endNonce - startNonce);

						if (!string.IsNullOrEmpty(r))
						{
							//Get token
							var request = new HttpRequestMessage()
							{
								RequestUri = requestUri,
								Method = HttpMethod.Post,
								 
							};

							string response = CalculateHash(clientId, clientSecret, nonce);
							string param = $"username=\"{clientId}\", {r}, uri=\"/oauth2/token\", response=\"{response}\"";

							request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Digest", param);

							var respon = await httpClient.SendAsync(request);
							var s = await respon.Content.ReadAsStringAsync();
						}

					}

					IRemoteHueClient client = new RemoteHueClient(null);
					var token = await client.GetToken(result.Code);

					var bridges = await client.GetBridgeAsync();

					if (bridges != null)
					{ }
				}
			}
		}

		private string CalculateHash(string clientId, string clientSecret, string nonce)
		{
			var HASH1 = MD5($"{clientId}:oauth2_client@api.meethue.com:{clientSecret}");
			var HASH2 = MD5("POST:/oauth2/token");
			var response = MD5(HASH1 + ":" + nonce + ":" + HASH2);

			return response;
		}

		public string MD5(string input)
		{

			String strAlgName = HashAlgorithmNames.Md5;
			return this.SampleHashMsg(strAlgName, input);

		}

		public String SampleHashMsg(String strAlgName, String strMsg)
		{
			// Convert the message string to binary data.
			IBuffer buffUtf8Msg = CryptographicBuffer.ConvertStringToBinary(strMsg, BinaryStringEncoding.Utf8);

			// Create a HashAlgorithmProvider object.
			HashAlgorithmProvider objAlgProv = HashAlgorithmProvider.OpenAlgorithm(strAlgName);

			// Hash the message.
			IBuffer buffHash = objAlgProv.HashData(buffUtf8Msg);

			// Verify that the hash length equals the length specified for the algorithm.
			if (buffHash.Length != objAlgProv.HashLength)
			{
				throw new Exception("There was an error creating the hash");
			}

			// Convert the hash to a string (for display).
			String strHashBase64 = CryptographicBuffer.EncodeToBase64String(buffHash);

			// Return the encoded string
			return strHashBase64;
		}


	}
}
