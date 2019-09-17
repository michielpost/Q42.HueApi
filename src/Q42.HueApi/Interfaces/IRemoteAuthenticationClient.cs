using Q42.HueApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Interfaces
{
  public interface IRemoteAuthenticationClient
  {
    Uri BuildAuthorizeUri(string state, string deviceId, string? deviceName = null, string responseType = "code");

    RemoteAuthorizeResponse ProcessAuthorizeResponse(string responseData);

    /// <summary>
    /// Initialize with existing AccessTokenResponse
    /// </summary>
    /// <param name="storedAccessToken"></param>
    void Initialize(AccessTokenResponse storedAccessToken);

    Task<AccessTokenResponse?> GetToken(string code);

    Task<AccessTokenResponse?> RefreshToken(string refreshToken);

    /// <summary>
    /// Gets a valid access token
    /// </summary>
    /// <returns></returns>
    Task<string?> GetValidToken();
  }
}
