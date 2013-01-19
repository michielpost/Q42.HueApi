Q42.HueApi
=========

Open source library for communication with the Philips Hue bridge.

Download directly from NuGet [Q42.HueApi on NuGet](https://nuget.org/packages/Q42.HueApi).

## How to use?
Some basic usage examples

### Bridge
Before you can communicate with the Philips Hue Bridge, you need to register your application.
	
	HueClient client = new HueClient("ip");
	client.Register("mypersonalappname");
	
If you already registered an appname, you can initialize the HueClient with the appname	

	client.Initialize("mypersonalappname");

### Control the lamps
Main usage of this library is to be able to control your lamps. We use a LampCommand for that. A LampCommand can be send to one or more / multiple lamps. A LampCommand can hold a color, effect, on/off etc.

	var command = new LampCommand();
	command.on = true;
	
There are some helpers to set a color on a command:
	
	//Turn the lamp on and set a Hex color for the command
	command.TurnOn().SetColor("FF00AA")
	
LampCommands also support Effects and Alerts

	//Blink once
	command.alert = Alerts.select;
	
	//Or start a colorloop
	command.effect = Effects.colorloop
	
Once you have composed your command, send it to one or more lamps

	client.SendCommand(command, new List<string> { "1" });
	
Or send it to all lamps

	client.SendCommand(command);

## How To install?
Download the source from GitHub or get the compiled assembly from NuGet [Q42.WinRT on NuGet](https://nuget.org/packages/Q42.WinRT).

## Credits
* [Q42](http://www.q42.nl) ([@q42](http://twitter.com/q42))

### Open Source Project Credits

* Newtonsoft.Json is used for object serialization

## License

Q42.HueApi is licensed under [MIT](http://www.opensource.org/licenses/mit-license.php "Read more about the MIT license form"). Refer to [license.txt](https://github.com/Q42/Q42.HueApi/blob/master/LICENSE.txt) for more information.

## Contributions

Contributions are welcome. Fork this repository and send a pull request if you have something useful to add.


## Related Projects
Other useful Philips Hue projects.

* [cDima Hue library](https://github.com/cDima/Hue) C# library
