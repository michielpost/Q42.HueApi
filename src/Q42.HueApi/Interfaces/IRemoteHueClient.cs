using System.Threading.Tasks;

namespace Q42.HueApi.Interfaces
{
	/// <summary>
	/// Remote Hue Client responsible for interacting with the bridge using the remote API
	/// </summary>
	public interface IRemoteHueClient : IHueClient
	{
		/// <summary>
		/// Untested
		/// </summary>
		/// <param name="clientId"></param>
		/// <param name="state"></param>
		/// <param name="deviceId"></param>
		/// <param name="appId"></param>
		/// <param name="deviceName"></param>
		/// <param name="responseType"></param>
		/// <returns></returns>
		Task<string> Authorize(string clientId, string state, string deviceId, string appId, string deviceName = null, string responseType = "code");

		/// <summary>
		/// Initialize the client with a bridgeId and appKey (whitelist identifier)
		/// </summary>
		/// <param name="bridgeId"></param>
		/// <param name="appKey"></param>
		void Initialize(string bridgeId, string appKey);

		/// <summary>
		/// Registers bridge for remote communication. Returns appKey and Initialized the client with this appkey
		/// </summary>
		/// <param name="bridgeId"></param>
		/// <returns></returns>
		Task<string> RegisterAsync(string bridgeId, string appId);

		/// <summary>
		/// Set the accessToken for the RemoteHueClient
		/// </summary>
		/// <param name="accessToken"></param>
		void SetRemoteAccessToken(string accessToken);

		/// <summary>
		/// Untested
		/// </summary>
		/// <param name="refreshToken"></param>
		/// <param name="clientId"></param>
		/// <param name="clientSecret"></param>
		/// <returns></returns>
		Task<string> RefreshToken(string refreshToken, string clientId, string clientSecret);
	}
}