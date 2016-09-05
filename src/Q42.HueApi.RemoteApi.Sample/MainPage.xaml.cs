using Q42.HueApi.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web;
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


			string appId = "yourAppId";
			string clientId = "clientId";
			string clientSecret = "clientSecret";

			//IRemoteHueClient client = new RemoteHueClient(clientSecret);
			var authorizeUri = RemoteHueClient.BuildAuthorizeUri(clientId, "sample", "consoleapp", appId);
			var callbackUri = new Uri("https://localhost/q42hueapi");

			var webAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, authorizeUri, callbackUri);

			if (webAuthenticationResult != null)
			{
				var result = RemoteHueClient.ProcessAuthorizeResponse(webAuthenticationResult.ResponseData);

				if (!string.IsNullOrEmpty(result.Code))
				{
					IRemoteHueClient client = new RemoteHueClient(null);
					var token = await client.GetToken(result.Code);

					var bridges = await client.GetBridgeAsync();

					if (bridges != null)
					{ }
				}
			}
		}
	}
}
