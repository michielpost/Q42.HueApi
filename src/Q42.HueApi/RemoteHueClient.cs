using Q42.HueApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi
{
	public partial class RemoteHueClient : HueClient, IRemoteHueClient
	{
		private readonly string _apiBase = "https://api.meethue.com/v2/bridges/";
		private static string _remoteAccessToken;
		private string _bridgeId;


		public RemoteHueClient(string accessToken)
		{
			SetRemoteAccessToken(accessToken);
		}

		public void SetRemoteAccessToken(string accessToken)
		{
			_remoteAccessToken = accessToken;

			var client = GetHttpClient();
			if (!string.IsNullOrEmpty(_remoteAccessToken))
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _remoteAccessToken);
			else
				_httpClient.DefaultRequestHeaders.Authorization = null;
		}

		/// <summary>
		/// Initialize client with your app key
		/// </summary>
		/// <param name="appKey"></param>
		public void Initialize(string bridgeId, string appKey)
		{
			if (bridgeId == null)
				throw new ArgumentNullException(nameof(bridgeId));

			_bridgeId = bridgeId;

			Initialize(appKey);
		}


		public new static HttpClient GetHttpClient()
		{
			// return per-thread HttpClient
			if (_httpClient == null)
			{
				_httpClient = new HttpClient();

				if (!string.IsNullOrEmpty(_remoteAccessToken))
					_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _remoteAccessToken);
			}

			return _httpClient;
		}


		/// <summary>
		/// Base URL for the API
		/// </summary>
		protected override string ApiBase
		{
			get
			{
				return $"{_apiBase}{_bridgeId}/{_appKey}/";
			}
		}
	}
}
