Q42.HueApi.Streaming
=========

Hue Entertainment API client to stream messages to the Hue Bridge.

## How to use?
Some basic usage examples.  
Please check the included sample app for a working demo (`Q42.HueApi.Streaming.Sample`).

### Register your app with the bridge
Make sure to request access to the entertainment API by giving generateClientKey = true to the register method
```cs 
LocalHueClient.RegisterAsync("ipAddress", "applicationName", "deviceName", true);
```

### Connect to an entertainment group
```cs 
//Initialize streaming client
StreamingHueClient client = new StreamingHueClient(ip, key, entertainmentKey);

//Get the entertainment group
var all = await client.LocalHueClient.GetBridgeAsync();
var group = all.Groups.Where(x => x.Type == HueApi.Models.Groups.GroupType.Entertainment).FirstOrDefault();

//Create a streaming group
var entGroup = new StreamingGroup(group.Locations);

//Connect to the streaming group
await client.Connect(group.Id);

//Start auto updating this entertainment group
client.AutoUpdate(entGroup, 50);
```

When you're connected and auto-updating, it's possible to change the color and brightness or the lights in a StreamingGroup

### Basics
```cs
//Change brightness and color
entGroup.SetBrightness(1);
entGroup.SetColor(new RGBColor("FF0000"));

//Change them both at the same time with a transition time of 1 second
entGroup.SetState(new RGBColor("FF0000"), 1, TimeSpan.FromSeconds(1));
```

### Effects
Philips Hue defines different types of effects. Read about them here:
https://developers.meethue.com/documentation/hue-edk-effect-creation

#### Area Effects
Supported by extension methods on the StreamingGroup
```
GetLeft()
GetRight()
GetFront()
GetBack()
GetCenter() //X > -0.1 && X < 0.1
```

Example usage:
```cs
entGroup.GetLeft().GetBack().SetBrightness(0);
```

#### LightIteratorEffect
Loops through all the lights to apply a (custom) effect.

There is an `IteratorEffect` extension available on the StreamingGroup or any list of StreamingLights.
There IteratorEffect supports different `IteratorEffectMode`s:
- Cycle: keeps looping through the list of lights to apply the effect
- Bouce: will apply the effect to each light in the list and goes backwards at the end of the list 
- Single: will apply the effect to each light once
- Random: will apply the effect to a random light
- All: apply the effect to all lights in sync (recommanded over AllIndividual)
- AllIndividual: apply the effect to all lights and give each light a custom transition. Only needed when lights have different starting colors/brightness.

Example to set a random color:
```cs
return entGroup.IteratorEffect(async (current, t) => {
  var r = new Random();
  var color = new RGBColor(r.NextDouble(), r.NextDouble(), r.NextDouble());
  current.SetState(color, 1, transitionTime.Value.Value);
}, mode, waitTime, duration, cancellationToken);
```

You can combine the Area and LightIteratorEffect:
```cs
entGroup.GetLeft().IteratorEffect(
```

#### Example Effects
A few example effects are included:
- SetRandomColor
- SetRandomColorFromList
- Flash
- FlashQuick
- KnightRider
- Christmas


#### LightSourceEffect
Make sure to enable this optional feature on the StreamingHueClient:
```cs
 //Optional: calculated effects that are placed in the room
client.AutoCalculateEffectUpdate(entGroup);
```

You can then place effects on the XY grid. See Q42.HueApi.Streaming.Sample for a moving RedLight example
```cs
var redLightEffect = new RedLightEffect();
redLightEffect.Radius = 0.5;
redLightEffect.Y = -1;
entGroup.PlaceEffect(redLightEffect);
redLightEffect.Start();
```

#### MultiChannelEffect
Not supported.


### Open Source Project Credits

* BouncyCastle is used the DTLS Handshake procedure and TLS_PSK_WITH_AES_128_GCM_SHA256 Cipher

