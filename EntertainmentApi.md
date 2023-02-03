HueApi.Entertainment
=========

Hue Entertainment API client to stream messages to the Hue Bridge.

## How to use?
Some basic usage examples.  
Please check the included sample app for a working demo (`Q42.HueApi.Streaming.Sample`).  
**Run `HueApi.Entertainment.ConsoleSample` for a demo using the V2 API.**

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
var all = await client.LocalHueClient.GetEntertainmentGroups();
var group = all.FirstOrDefault();

//Create a streaming group
var entGroup = new StreamingGroup(group.Locations);

//Connect to the streaming group
await client.Connect(group.Id);

//Start auto updating this entertainment group
client.AutoUpdate(entGroup, 50);
```

### Layers
When you're connected and auto-updating, it's possible to change the color and brightness or the lights in a StreamingGroup. We do this by first creating a layer.
There are two types of layers. Base layers and effect layers. You should always have 1 base layer which you can use for all operations. If you want to run a new effect but keep the current effect running on the base layer, you can start the second effect on a new effect layer.
Base and effect layers are the same, the only difference is that when the brightness of a light on an effect layer is 0, the effect layer will be ignored and the state of the light on the base layer will be used. 
```cs
//Create new base layer
var entLayer = stream.GetNewLayer(isBaseLayer: true);
```

### CancellationTokens
CancellationTokens are used on almost every method call. This makes sure long running background tasks can be cancelled if needed.

### Basics
```cs
//Change brightness and color
entLayer.SetBrightness(CancellationToken.None, 1);
entLayer.SetColor(CancellationToken.None, new RGBColor("FF0000"));

//Change them both at the same time with a transition time of 1 second
entLayer.SetState(CancellationToken.None, new RGBColor("FF0000"), 1, TimeSpan.FromSeconds(1));
```

### Basics for a single light
```cs
//The EntertainmentLayer contains all the lights, you can make subselections
var light1 = entLayer.Where(x => x.Id == 1).FirstOrDefault();
var lightsOnRightSide = entLayer.GetRight();

//Change the color of the single light
light1.SetColor(CancellationToken.None, new RGBColor("FF0000"));

//Change the subgroup of lights on the right side
lightsOnRightSide.SetColor(CancellationToken.None, new RGBColor("FF0000"));
```

### Effects
Philips Hue defines different types of effects. Read about them here:
https://developers.meethue.com/documentation/hue-edk-effect-creation

#### Area Effects
Supported by extension methods on the EntertainmentLayer
```
GetLeft()
GetRight()
GetFront()
GetBack()
GetCenter() //X > -0.1 && X < 0.1
```

Example usage:
```cs
entLayer.GetLeft().GetBack().SetBrightness(CancellationToken.None, 0);
```

#### LightIteratorEffect
Loops through all the lights to apply a (custom) effect.

There is an `IteratorEffect` extension available on the StreamingGroup or any list of StreamingLights.
There IteratorEffect supports different `IteratorEffectMode`s:
- Cycle: keeps looping through the list of lights to apply the effect
- Bouce: will apply the effect to each light in the list and goes backwards at the end of the list 
- Single: will apply the effect to each light once
- Random: will apply the effect to a random light
- All: apply the effect to all lights in sync (recommended over AllIndividual)
- AllIndividual: apply the effect to all lights and give each light a custom transition. Only needed when lights have different starting colors/brightness.

An IteratorEffect can run on a list of EntertainmentLights, or a two dimensional list of EntertainmentLights for more advanced scenario's.
Check the sample app for some usage.

Example to set a random color:
```cs
entLayer.IteratorEffect(CancellationToken.None, async (current, cancel, t) => {
  var r = new Random();
  var color = new RGBColor(r.NextDouble(), r.NextDouble(), r.NextDouble());
  current.SetState(cancel, color, 1, TimeSpan.FromMilliseconds(500));
}, IteratorEffectMode.Cycle, waitTime: TimeSpan.FromMilliseconds(500), duration: TimeSpan.FromSeconds(30));
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
 entLayer.AutoCalculateEffectUpdate(new CancellationToken());
```

You can then place effects on the XY grid. See Q42.HueApi.Streaming.Sample for a moving RedLight example
```cs
var redLightEffect = new RedLightEffect();
redLightEffect.Radius = 0.5;
redLightEffect.Y = -1;
entLayer.PlaceEffect(redLightEffect);
redLightEffect.Start();
```

#### MultiChannelEffect
Not supported.


### Open Source Project Credits

* BouncyCastle is used the DTLS Handshake procedure and TLS_PSK_WITH_AES_128_GCM_SHA256 Cipher

