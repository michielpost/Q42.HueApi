Q42.HueApi
=========

Open source library for communication with the Philips Hue bridge.

Download directly from NuGet [Q42.HueApi on NuGet](https://nuget.org/packages/Q42.HueApi).

## How to use?
Some basic usage examples

### Bridge
Before you can communicate with the Philips Hue Bridge, you need to register your application.
	
	BridgeService.Register("mypersonalappname");
	
Now you can create an instance of the LampService with the IP of the bridge and your appname	

	new LampService("192.168.1.10", "mypersonalappname")

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

	LampService.SendCommand(command, new List<string> { "1" });
	
Or send it to all lamps

	LampService.SendCommand(command);

## How To install?
Download the source from GitHub or get the compiled assembly from NuGet [Q42.WinRT on NuGet](https://nuget.org/packages/Q42.WinRT).

## Credits
* [Q42](http://www.q42.nl) ([@q42](http://twitter.com/q42))

### Open Source Project Credits

* Newtonsoft.Json is used for object serialization


## Related Projects
Other useful Philips Hue projects.

* [cDima Hue library](https://github.com/cDima/Hue) C# library
