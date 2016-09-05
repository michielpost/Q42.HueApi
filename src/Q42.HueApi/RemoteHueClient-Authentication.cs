using Newtonsoft.Json.Linq;
using Q42.HueApi.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi
{
	/// <summary>
	/// http://www.developers.meethue.com/documentation/remote-api-authentication
	/// </summary>
	public partial class RemoteHueClient
	{

		/// <summary>
		/// Authorization request
		/// </summary>
		/// <param name="clientId">Identifies the client that is making the request. The value passed in this parameter must exactly match the value you receive from hue. Note that the underscore is not used in the clientid name of this parameter.</param>
		/// <param name="state">Provides any state that might be useful to your application upon receipt of the response. The Hue Authorization Server roundtrips this parameter, so your application receives the same value it sent. To mitigate against cross-site request forgery (CSRF), it is strongly recommended to include an anti-forgery token in the state, and confirm it in the response. One good choice for a state token is a string of 30 or so characters constructed using a high-quality random-number generator.</param>
		/// <param name="deviceId">The device identifier must be a unique identifier for the app or device accessing the Hue Remote API.</param>
		/// <param name="appId">Identifies the app that is making the request. The value passed in this parameter must exactly match the value you receive from hue.</param>
		/// <param name="deviceName">The device name should be the name of the app or device accessing the remote API. The devicename is used in the user's "My Apps" overview in the Hue Account (visualized as: "<app name> on <devicename>"). If not present, deviceid is also used for devicename. The <app name> is the application name you provided to us the moment you requested access to the remote API.</param>
		/// <param name="responseType">The response_type value must be "code".</param>
		/// <returns></returns>
		public static Uri BuildAuthorizeUri(string clientId, string state, string deviceId, string appId, string deviceName = null, string responseType = "code")
		{
			if (string.IsNullOrEmpty(clientId))
				throw new ArgumentNullException(nameof(clientId));
			if (string.IsNullOrEmpty(state))
				throw new ArgumentNullException(nameof(state));
			if (string.IsNullOrEmpty(deviceId))
				throw new ArgumentNullException(nameof(deviceId));
			if (string.IsNullOrEmpty(appId))
				throw new ArgumentNullException(nameof(appId));
			if (string.IsNullOrEmpty(responseType))
				throw new ArgumentNullException(nameof(responseType));

			string url = string.Format("https://api.meethue.com/oauth2/auth?clientid={0}&response_type={5}&state={1}&appid={3}&deviceid={2}&devicename={4}", clientId, state, deviceId, appId, deviceName, responseType);

			return new Uri(url);
		}

		public static RemoteAuthorizeResponse ProcessAuthorizeResponse(string responseData)
		{
			string url = responseData;
			string[] parts = url.Split(new char[] { '?', '&' });

			RemoteAuthorizeResponse result = new RemoteAuthorizeResponse();

			foreach (var part in parts)
			{
				string[] nv = part.Split(new char[] { '=' });
				if(nv.Length == 2)
				{
					if (nv[0].ToLower() == "code")
						result.Code = nv[1];
					if (nv[0].ToLower() == "state")
						result.State = nv[1];
				}
			}

			return result;
		}

		public async Task<string> GetToken(string code)
		{
			HttpClient client = HueClient.GetHttpClient();
			var result = await client.PostAsync(new Uri($"https://api.meethue.com/oauth2/token?code={code}&grant_type=authorization_code"), null).ConfigureAwait(false);

			//TODO: Do something with the result

			return string.Empty;
		}


		public async Task<string> RefreshToken(string refreshToken, string clientId, string clientSecret)
		{
			var stringContent = new StringContent("refresh_token=" + refreshToken);

			//base64(clientid:clientsecret)
			var basicAuthHeaderInput = clientId + ":" + clientSecret;
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(basicAuthHeaderInput);
			var basicAuthHeaderString = System.Convert.ToBase64String(plainTextBytes);


			HttpClient client = HueClient.GetHttpClient();
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", basicAuthHeaderString);
			var result = await client.PostAsync(new Uri("https://api.meethue.com/oauth2/refresh?grant_type=refresh_token"), stringContent).ConfigureAwait(false);

			//TODO: Do something with the result

			return string.Empty;
		}


		public async Task<string> RegisterAsync(string bridgeId, string appId)
		{
			if (string.IsNullOrEmpty(bridgeId))
				throw new ArgumentNullException(nameof(bridgeId));
			if (string.IsNullOrEmpty(appId))
					throw new ArgumentNullException(nameof(appId));

			JObject obj = new JObject();
			obj["linkbutton"] = true;

			HttpClient client = HueClient.GetHttpClient();
			var configResponse = await client.PutAsync(new Uri($"{_apiBase}{bridgeId}/0/config"), new StringContent(obj.ToString())).ConfigureAwait(false);

			JObject bridge = new JObject();
			bridge["devicetype"] = appId;

			var response = await client.PostAsync(new Uri($"{_apiBase}{bridgeId}/"), new StringContent(bridge.ToString())).ConfigureAwait(false);
			var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);


			JObject result;
			try
			{
				JArray jresponse = JArray.Parse(stringResponse);
				result = (JObject)jresponse.First;
			}
			catch
			{
				//Not an expected response. Return response as exception
				throw new Exception(stringResponse);
			}

			JToken error;
			if (result.TryGetValue("error", out error))
			{
				if (error["type"].Value<int>() == 101) // link button not pressed
					throw new Exception("Link button not pressed");
				else
					throw new Exception(error["description"].Value<string>());
			}

			var key = result["success"]["username"].Value<string>();
			Initialize(key);

			return key;
		}
	}
}
