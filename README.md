Q42.HueApi
=========

Open source library for communication with the Philips Hue bridge.
This library covers all the Philips hue API calls! You can set the state of your lights, update the Bridge configuration, create groups, schedules etc.

This Portable Library is compatible with: .Net45, Windows 8, Windows Phone 8, Windows 10 Universal Apps and the Xamarin Platform!
Download directly from NuGet [Q42.HueApi on NuGet](https://nuget.org/packages/Q42.HueApi).

## How to use?
Some basic usage examples

### Bridge
Before you can communicate with the Philips Hue Bridge, you need to find the bridge and register your application:

	IBridgeLocator locator = new HttpBridgeLocator();
	
	//For Windows 8 and .NET45 projects you can use the SSDPBridgeLocator which actually scans your network. 
	//See the included BridgeDiscoveryTests and the specific .NET and .WinRT projects
    IEnumerable<string> bridgeIPs = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5));
	
Register your application
	
	ILocalHueClient client = new LocalHueClient("ip");
	var appKey = await client.RegisterAsync("mypersonalappname", "mydevicename");
	//Save the app key for later use
	
If you already registered an appname, you can initialize the HueClient with the app's key:	

	client.Initialize("mypersonalappkey");

### Control the lights
Main usage of this library is to be able to control your lights. We use a LightCommand for that. A LightCommand can be send to one or more / multiple lights. A LightCommand can hold a color, effect, on/off etc.

	var command = new LightCommand();
	command.On = true;
	
There are some helpers to set a color on a command:
	
	//Turn the light on and set a Hex color for the command (see the section about Color Converters)
    command.TurnOn().SetColor(new RGBColor("FF00AA"))
	
LightCommands also support Effects and Alerts

	//Blink once
	command.Alert = Alerts.Once;
	
	//Or start a colorloop
	command.Effect = Effects.ColorLoop;
	
Once you have composed your command, send it to one or more lights

	client.SendCommandAsync(command, new List<string> { "1" });
	
Or send it to all lights

	client.SendCommandAsync(command);

### Color Conversion
The Philips Hue lights work with Brightness, Saturation, Hue and X, Y properties. More info can be found in the Philips Hue Developer documentation: http://www.developers.meethue.com/documentation/core-concepts#color_gets_more_complicated
It's not trivial to convert the light colors to a color system developers like to work with, like RGB or HEX. Q42.HueApi has 3 different color converters out of the box. They are in a seperate package and it's easy to create your own color converter.
 - *Original*:  The original converter based on a large XY array.
 - *OriginalWithModel*: Does not use the large XY array, but should produce the same result and uses the lamp model.
 - *HSB*: Converts based on Hue, Brightness and Saturation.

 How to use a color converter?
 Add one of the following usings:  
 `using Q42.HueApi.ColorConverters.Original`  
 `using Q42.HueApi.ColorConverters.OriginalWithModel`  
 `using Q42.HueApi.ColorConverters.HSB`  

 This will add extension methods to `Light`, `State` and `LightCommand`. So you can set the color using `new RGBColor()` and convert the `State` back to `RGBColor`

 Pull Requests with improvements to the color conversion are always welcome! 
 
	
## Remote API
There is also a Philips Hue Remote API. It allows you to send commands to a bridge over the internet. You can request access here: http://www.developers.meethue.com/content/remote-api  
Q42.HueApi is compatible with the remote API.  
You need an Access Token and a Bridge Id. Please refer to the Philips Hue API documentation on how to obtain them. This library does not have support for it yet. Pull Requests are welcome!

	IRemoteHueClient remoteHueClient =  new RemoteHueClient("access token");
	remoteHueClient.Initialize("bridge id");
	
After the setup, you can send normal commands to the remote API:

	remoteHueClient.SendCommandAsync(command, new List<string> { "1" });

## How To install?
Download the source from GitHub or get the compiled assembly from NuGet [Q42.HueApi on NuGet](https://nuget.org/packages/Q42.HueApi).

## Credits
This library is made possible by contributions from:
* [Michiel Post](http://www.michielpost.nl) ([@michielpostnl](http://twitter.com/michielpostnl)) - core contributor
* [Q42](http://www.q42.nl) ([@q42](http://twitter.com/q42))
* [@ermau](https://github.com/ermau)
* [@koenvanderlinden](https://github.com/koenvanderlinden)

### Open Source Project Credits

* Newtonsoft.Json is used for object serialization

## License

Q42.HueApi is licensed under [MIT](http://www.opensource.org/licenses/mit-license.php "Read more about the MIT license form"). Refer to [license.txt](https://github.com/Q42/Q42.HueApi/blob/master/LICENSE.txt) for more information.

## Contributions

Contributions are welcome. Fork this repository and send a pull request if you have something useful to add.

[![Build status](https://ci.appveyor.com/api/projects/status/k12ortcvy3t5bmy7)](https://ci.appveyor.com/project/michielpost/q42-hueapi)


## Related Projects

* [Lists of hue libraries](https://github.com/Q42/hue-libs)
* [Official Philips hue API documentation](http://developers.meethue.com)


## Apps that use Q42.HueAPI
Are you using Q42.HueAPI? Get your app listed here! Edit this page and send a pull request.

Windows Phone
* [My Hue Light Switch](http://www.windowsphone.com/s?appid=669c9e16-b417-43c6-b0cc-724e8dfd5866)
* [iControlHue](http://www.windowsphone.com/s?appid=f1b2bcb5-82e4-4a04-9894-c9e08b85a55d)
* [OnHue](http://www.windowsphone.com/s?appid=37d7f4dc-8520-4fa8-9b27-46531c34dd60)
* [Huetro for Hue](http://www.windowsphone.com/s?appid=f14faa22-179d-42e4-99ca-88b44d10449b)

Windows
* [My Hue Light Switch](http://apps.microsoft.com/windows/app/my-hue-light-switch/1193bff8-dec8-4997-82e3-a0f9aedacbb2)
* [DarkLights](http://apps.microsoft.com/windows/app/09fb8d8b-cefc-4215-b3b2-a87a483d6690)
* [Huetro for Hue](http://apps.microsoft.com/windows/app/33553060-d57c-467d-8348-5e88071360c5)

Windows Desktop
* [CastleOS](http://www.CastleOS.com/)

MONO
* [Command Line Tool](http://www.everyhue.com/vanilla/discussion/1061)
