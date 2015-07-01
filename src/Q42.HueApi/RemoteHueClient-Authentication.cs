using System;
using System.Collections.Generic;
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
		public async Task<string> Authorize(string clientId, string state, string deviceId, string appId, string deviceName = null, string responseType = "code")
		{
			if (string.IsNullOrEmpty(clientId))
				throw new ArgumentNullException("clientId");
			if (string.IsNullOrEmpty(state))
				throw new ArgumentNullException("state");
			if (string.IsNullOrEmpty(deviceId))
				throw new ArgumentNullException("deviceId");
			if (string.IsNullOrEmpty(appId))
				throw new ArgumentNullException("appId");
			if (string.IsNullOrEmpty(responseType))
				throw new ArgumentNullException("responseType");

			string url = string.Format("https://api.meethue.com/oauth2/auth?clientid={0}&response_type={5}&state={1}&appid={3}&deviceid={2}&devicename={4}", clientId, state, deviceId, appId, deviceName, responseType);

			HttpClient client = HueClient.GetHttpClient();
			var stringResult = await client.GetStringAsync(new Uri(url)).ConfigureAwait(false);

			//TODO: get code parameter from returned redirect url?

			return stringResult;
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
	}
}
