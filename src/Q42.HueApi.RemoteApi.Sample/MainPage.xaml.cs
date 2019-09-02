using Q42.HueApi.Interfaces;
using Q42.HueApi.Models;
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

      //Fill with values provided on https://developers.meethue.com/my-apps/
      string appId = ""; //q42-hueapi-test
      string clientId = ""; 
      string clientSecret = "";
      var callbackUri = new Uri(""); //https://localhost/q42hueapitest

      IRemoteAuthenticationClient authClient = new RemoteAuthenticationClient(clientId, clientSecret, appId);

      //If you already have an accessToken, call:
      //AccessTokenResponse storedAccessToken = SomehwereFrom.Storage();
      //authClient.Initialize(storedAccessToken);
      //IRemoteHueClient client = new RemoteHueClient(authClient.GetValidToken);

      //Else, reinitialize:

      var authorizeUri = authClient.BuildAuthorizeUri("sample", "consoleapp");

      var webAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, authorizeUri, callbackUri);

      if (webAuthenticationResult != null)
      {
        var result = authClient.ProcessAuthorizeResponse(webAuthenticationResult.ResponseData);

        if (!string.IsNullOrEmpty(result.Code))
        {
          //You can store the accessToken for later use
          var accessToken = await authClient.GetToken(result.Code);

          IRemoteHueClient client = new RemoteHueClient(authClient.GetValidToken);
          var bridges = await client.GetBridgesAsync();

          if (bridges != null)
          {
            //Register app
            //var key = await client.RegisterAsync(bridges.First().Id, "Sample App");

            //Or initialize with saved key:
            client.Initialize(bridges.First().Id, "C95sK6Cchq2LfbkbVkfpRKSBlns2CylN-VxxDD8F");

            //Turn all lights on
            var lightResult = await client.SendCommandAsync(new LightCommand().TurnOn());

          }
        }
      }
    }

    private static string CalculateHash(string clientId, string clientSecret, string nonce)
    {
      var HASH1 = MD5($"{clientId}:oauth2_client@api.meethue.com:{clientSecret}");
      var HASH2 = MD5("POST:/oauth2/token");
      var response = MD5(HASH1 + ":" + nonce + ":" + HASH2);

      return response;
    }

    private static string MD5(string str)
    {
      var alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
      IBuffer buff = CryptographicBuffer.ConvertStringToBinary(str, BinaryStringEncoding.Utf8);
      var hashed = alg.HashData(buff);
      var res = CryptographicBuffer.EncodeToHexString(hashed);
      return res;
    }

  }
}
