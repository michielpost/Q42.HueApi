HueApi
=========

Open source library for communication with the Philips Hue bridge.

Over **400k downloads** on NuGet (Q42.Hue + HueApi)

This library covers all the Philips hue API calls! You can set the state of your lights, update the Bridge configuration, create groups, schedules etc.

This library targets `.net 8` and `.net 9`!  
Download directly from NuGet:
- Clip API v2: [HueApi on NuGet](https://nuget.org/packages/HueApi) (works with Hue Bridge Pro!)

**New for v3**
The new Fluent API allows for easy access to all resources through their respective methods. For example:
```cs
//Light resource
localHueApi.Light.GetAllAsync()
localHueApi.Light.UpdateAsync(id, ...)

//Room Resource
localHueApi.Room.GetAllAsync()
```

Features:
- Support for Hue Entertainment API
- Support for the Hue Remote API
- Support for the Hue Bridge Pro APIs
- Multiple Color Converters

Make sure to install the **HueApi** packages:
- [HueApi from NuGet](https://nuget.org/packages/HueApi)
- [HueApi.ColorConverters from NuGet](https://nuget.org/packages/HueApi.ColorConverters)
- [HueApi.Entertainment from NuGet](https://nuget.org/packages/HueApi.Entertainment)

Do **not** use the `Q42` prefixed packages anymore. They target old APIs which will be removed in the future.

## How to use?
Some basic usage examples

Use the LocalHueApi:
```cs
var localHueApi = new LocalHueApi("BRIDGE_IP", "KEY");
```

Register your application
	
```cs
//Make sure the user has pressed the button on the bridge before calling RegisterAsync
//It will throw an LinkButtonNotPressedException if the user did not press the button
var regResult = await LocalHueClient.RegisterAsync("BRIDGE_IP", "mypersonalappname", "mydevicename");

//Save the app key for later use and use it to initialize LocalHueApi
var appKey = regResult.Username;
```

Change the lights:
```cs
var lights = await localHueApi.Light.GetAllAsync();
var id = all.Data.Last().Id; //Pick a light

var req = new UpdateLight()
	.TurnOn()
	.SetColor(new ColorConverters.RGBColor("FF0000"));
	
var result = await localHueApi.Light.UpdateAsync(id, req);
```

## API Reference
Use the API reference provided by Hue to discover the capabilities of the API.
https://developers.meethue.com/develop/hue-api-v2/api-reference/

## EventStream
Listen to the new EventStream to get notified by the Hue Bridge when new events occur.

```cs
localHueApi.OnEventStreamMessage += EventStreamMessage;
localHueApi.StartEventStream();

void EventStreamMessage(List<EventStreamResponse> events)
{
  Console.WriteLine($"{events.Count} new events");

  foreach(var hueEvent in events)
  {
    foreach(var data in hueEvent.Data)
    {
      Console.WriteLine($"Data: {data.Metadata?.Name} / {data.IdV1}");
    }
  }
}

//localHueApi.StopEventStream();
```

Sample usage can be found in the included Console Sample App: `HueApi.ConsoleSample`

## Clip V2 API
Philips Hue has developed a new Clip V2 API. This library has support for the new Clip V2 APIs. Philips Hue is still developing these APIs and new functionality is added regularly. Please create an issue or PR if you need something that is not supported yet.


## Support for Hue Entertainment.  
Check out the [HueApi.Entertainment documentation](https://github.com/michielpost/Q42.HueApi/blob/master/EntertainmentApi.md)   
Read about the [Philips Entertainment API](https://developers.meethue.com/entertainment-blog)

	
## Remote API
There is also a Philips Hue Remote API. It allows you to send commands to a bridge over the internet. You can request access here: http://www.developers.meethue.com/content/remote-api  
Q42.HueApi is compatible with the remote API.  There's a sample app and documentation can be found here:
https://github.com/michielpost/Q42.HueApi/blob/master/RemoteApi.md

For remote usage with the new CLIP v2 API, use the `new RemoteHueApi("KEY", "token")`


### Color Conversion
The Philips Hue lights work with Brightness, Saturation, Hue and X, Y properties. More info can be found in the Philips Hue Developer documentation: http://www.developers.meethue.com/documentation/core-concepts#color_gets_more_complicated
It's not trivial to convert the light colors to a color system developers like to work with, like RGB or HEX. HueApi has 2 different color converters out of the box. They are in a seperate package and it's easy to create your own color converter.

The `HueApi.ColorConverters` NuGet package contains:
 - *Original*:  The original converter based on a large XY array.
 - *HSB*: Converts based on Hue, Brightness and Saturation.

 How to use a color converter?
 Add one of the following usings:  
 `using HueApi.ColorConverters.Original`  
 `using HueApi.ColorConverters.HSB`  

 This will add extension methods to `Light`, `State` and `LightCommand`. So you can set the color using `new RGBColor()` and convert the `State` back to `RGBColor`

 Pull Requests with improvements to the color conversion are always welcome! 
 

## How To install?
Download the source from GitHub or get the compiled assembly from NuGet
- [HueApi on NuGet](https://nuget.org/packages/HueApi)


## Credits
This library is made possible by contributions from:
* [Michiel Post](http://www.michielpost.nl) ([@michielpostnl](https://twitter.com/michielpostnl)) - core contributor
* [Q42](https://www.q42.nl) ([@q42](http://twitter.com/q42))
* [@ermau](https://github.com/ermau)
* [@koenvanderlinden](https://github.com/koenvanderlinden)
* [@Indigo744](https://github.com/Indigo744)
* [and others](https://github.com/michielpost/Q42.HueApi/graphs/contributors)

## License

HueApi is licensed under [MIT](http://www.opensource.org/licenses/mit-license.php "Read more about the MIT license form"). Refer to [license.txt](https://github.com/michielpost/Q42.HueApi/blob/master/LICENSE.txt) for more information.

## Contributions

Contributions are welcome. Fork this repository and send a pull request if you have something useful to add.

[![Build and publish](https://github.com/michielpost/Q42.HueApi/actions/workflows/nuget_publish.yml/badge.svg)](https://github.com/michielpost/Q42.HueApi/actions/workflows/nuget_publish.yml)


## Related Projects

* [Lists of hue libraries](https://github.com/Q42/hue-libs)
* [Official Philips hue API documentation](http://developers.meethue.com)


## Apps that use Q42.HueAPI
Are you using Q42.HueAPI? Get your app listed here! Edit this page and send a pull request.

Open Source
* [Chromatics](https://github.com/logicallysynced/Chromatics)
* [Eurovision Hue](https://github.com/martincostello/eurovision-hue)
* [glimmr](https://github.com/d8ahazard/glimmr)
* [HomeBlaze](https://github.com/RicoSuter/HomeBlaze)

Windows
* [DarkLights](https://apps.microsoft.com/detail/9WZDNCRDKM21)
* [Huetro for Hue](https://apps.microsoft.com/detail/9wzdncrfjj3t)
* [hueDynamic](https://www.microsoft.com/store/apps/9nblggh42jgb)
* [PresenceLight](https://github.com/isaacrlevin/PresenceLight)

Other
* [Hue Light DJ](https://github.com/michielpost/HueLightDJ)

No longer available or maintained
* [My Hue Light Switch](http://apps.microsoft.com/windows/app/my-hue-light-switch/1193bff8-dec8-4997-82e3-a0f9aedacbb2)
* [CastleOS](http://www.CastleOS.com/)
* [My Hue Light Switch (WP)](http://www.windowsphone.com/s?appid=669c9e16-b417-43c6-b0cc-724e8dfd5866)
* [iControlHue (WP)](http://www.windowsphone.com/s?appid=f1b2bcb5-82e4-4a04-9894-c9e08b85a55d)
* [OnHue (WP)](http://www.windowsphone.com/s?appid=37d7f4dc-8520-4fa8-9b27-46531c34dd60)
* [Huetro for Hue (WP)](http://www.windowsphone.com/s?appid=f14faa22-179d-42e4-99ca-88b44d10449b)
* [hueReact (Android)](https://play.google.com/store/apps/details?id=com.hallidev.HueReact)
* [Command Line Tools](https://github.com/DigitalNut/HueCmdNetCore)  -- Control your Hue from your command line
* [C# Script Command Line Tool](https://github.com/DigitalNut/HueScript)  -- Use C# as your scripting language
* [Andriks.HueApiDemo (Winforms)](https://github.com/andriks2/Andriks.HueApiDemo)
