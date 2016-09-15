## Remote API
There is also a Philips Hue Remote API. It allows you to send commands to a bridge over the internet. You can request access here: http://www.developers.meethue.com/content/remote-api  
Q42.HueApi is compatible with the remote API.

**Check out the sample code:
https://github.com/Q42/Q42.HueApi/blob/master/src/Q42.HueApi.RemoteApi.Sample/MainPage.xaml.cs**

How to use the Remote API with the Q42.HueApi library?

You'll need an appId, clientId and clientSecret provided by Philips Hue. You can request them on the Hue Developer Portal. Before we can use the RemoteHueClient, we need to get an access token. We use the RemoteAuthenticationClient for that.

```cs 
IRemoteAuthenticationClient authClient = new RemoteAuthenticationClient(clientId, clientSecret, appId);
```

The user needs to link their bridge to your app by going to a special authorize URL. We can build this URL like this:

```cs 
authClient.BuildAuthorizeUri("sample", "consoleapp");
```

Your application is responsible for showing this URL to the user and handle the response. In a Windows 10 UWP app it's easy to present a browser windows to authorize the app, with just one line of code:

```cs 
var webAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, authorizeUri, callbackUri);
```

This authentication request results in a code, we can parse that from the result:

```cs 
var result = authClient.ProcessAuthorizeResponse(webAuthenticationResult.ResponseData);
```

With that code we can get an access token:

```cs 
var accessToken = await authClient.GetToken(result.Code);
```

Now you can create an RemoteHueClient and give it the helper function that is able to get the access token and automatically refreshes this token when needed:

```cs 
IRemoteHueClient client = new RemoteHueClient(authClient.GetValidToken);
var bridges = await client.GetBridgesAsync();
```

The last step is to register our app with the user's Bridge

```cs 
//Register app
var key = await client.RegisterAsync(bridges.First().Id, "Sample App");

//Or initialize with saved key:
client.Initialize(bridges.First().Id, "saved_key");

//Turn all lights on
var lightResult = await client.SendCommandAsync(new LightCommand().TurnOn());
```

            
