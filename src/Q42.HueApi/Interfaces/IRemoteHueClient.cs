using System.Threading.Tasks;

namespace Q42.HueApi.Interfaces
{
	public interface IRemoteHueClient : IHueClient
	{
		Task<string> Authorize(string clientId, string state, string deviceId, string appId, string deviceName = null, string responseType = "code");
		void Initialize(string bridgeId);
		Task<string> RefreshToken(string refreshToken, string clientId, string clientSecret);
		void SetRemoteAccessToken(string accessToken);
	}
}