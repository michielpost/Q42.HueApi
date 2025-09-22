using HueApi.ColorConverters.Original.Extensions;
using HueApi.Models;
using HueApi.Models.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueApi.Tests
{
  [TestClass]
  public class LightTests
  {
    private readonly LocalHueApi localHueClient;

    public LightTests()
    {
      var builder = new ConfigurationBuilder().AddUserSecrets<LightTests>();
      var config = builder.Build();

      localHueClient = new LocalHueApi(config["ip"], key: config["key"]);
    }

    [TestMethod]
    public async Task Get()
    {
      var result = await localHueClient.Light.GetAllAsync();

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);
    }

    [TestMethod]
    public async Task GetById()
    {
      var all = await localHueClient.Light.GetAllAsync();
      var id = all.Data.Last().Id;

      var result = await localHueClient.Light.GetByIdAsync(id);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);

    }


    [TestMethod]
    public async Task PutById()
    {
      var all = await localHueClient.Light.GetAllAsync();
      var id = all.Data.Last().Id;

      UpdateLight req = new UpdateLight()
      {
        Signaling = new SignalingUpdate
        {
          Signal = Signal.alternating,
          Duration = 60000,
          Colors = new List<Color>
          {
            new Color { Xy = new XyPosition { X = 0.456, Y = 0.123, }},
            new Color { Xy = new XyPosition { X = 0.333, Y = 0.712, }}
          }
        }
      };
      var result = await localHueClient.Light.UpdateAsync(id, req);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      var resultRead = await localHueClient.Light.GetByIdAsync(id);

      Assert.IsTrue(result.Data.Count == 1);
      Assert.AreEqual(id, result.Data.First().Rid);

    }

    [TestMethod]
    public async Task ChangeLightColor()
    {
      var all = await localHueClient.Light.GetAllAsync();
      var id = all.Data.Last().Id;

      //Turn red
      var req = new UpdateLight()
        .TurnOn()
        .SetColor(new ColorConverters.RGBColor("FF0000"));

      var result = await localHueClient.Light.UpdateAsync(id, req);

      await Task.Delay(TimeSpan.FromSeconds(5));

      //Turn blue
      req = new UpdateLight()
        .SetColor(new ColorConverters.RGBColor("0000FF"));
      result = await localHueClient.Light.UpdateAsync(id, req);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
      Assert.AreEqual(id, result.Data.First().Rid);

    }

    [TestMethod]
    public async Task TestChangeLightColor()
    {
      var all = await localHueClient.Light.GetAllAsync();
      var id = all.Data.First().Id;
      var rgbColorHue = new HueApi.ColorConverters.RGBColor(10, 10, 10);

      // Create the light update command
      var req = new UpdateLight()
      .TurnOn()
      .SetColor(rgbColorHue);

      var result = localHueClient.Light.UpdateAsync(id, req).Result;

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
      Assert.AreEqual(id, result.Data.First().Rid);

    }

    [TestMethod]
    public async Task ChangeGradientLightColor()
    {
      var all = await localHueClient.Light.GetAllAsync();

      //Get gradient light
      var gradientLight = all.Data.Where(x => x.Gradient != null).FirstOrDefault();

      if (gradientLight == null)
        throw new Exception("No Gradient light found.");

      var id = gradientLight.Id;

      //Turn on
      var req = new UpdateLight().TurnOn();

      req.Gradient = new Gradient();
      req.Gradient.Mode = GradientMode.interpolated_palette;
      req.Gradient.Points = new System.Collections.Generic.List<GradientPoint>()
      {
        new GradientPoint().SetColor(new ColorConverters.RGBColor("FF0000")), //red
        new GradientPoint().SetColor(new ColorConverters.RGBColor("00FF00")), //green
        new GradientPoint().SetColor(new ColorConverters.RGBColor("0000FF")), //blue
        new GradientPoint().SetColor(new ColorConverters.RGBColor("FFA500")), //orange
        new GradientPoint().SetColor(new ColorConverters.RGBColor("A020F0")), //purple
      };

      //req.Effects = new Effects() { Effect = Effect.fire };

      var result = await localHueClient.Light.UpdateAsync(id, req);


      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
      Assert.AreEqual(id, result.Data.First().Rid);

    }

    [TestMethod]
    public async Task ChangePlayStripLightColor()
    {
      var all = await localHueClient.Light.GetAllAsync();

      //Get gradient light
      var officeStrip = all.Data
        .Where(x => x.Metadata != null)
        .Where(x => x.Metadata!.Archetype  == "hue_lightstrip")
        .Where(x => x.Metadata!.Name.Contains("office", StringComparison.InvariantCultureIgnoreCase))
        .FirstOrDefault();

      if (officeStrip == null)
        throw new Exception("No Gradient Led Strip found.");

      var id = officeStrip.Id;

      //Latest update also supports gradient on a play ledstrip
      Assert.IsNotNull(officeStrip.Gradient);

      //Turn red
      var req = new UpdateLight()
        .TurnOn();
      //.SetColor(new ColorConverters.RGBColor("FF0000"));

      //req.Gradient = new Gradient();
      //req.Gradient.Mode = GradientMode.interpolated_palette;
      //req.Gradient.Points = new System.Collections.Generic.List<GradientPoint>()
      //{
      //  new GradientPoint().SetColor(new ColorConverters.RGBColor("FF0000")), //red
      //  new GradientPoint().SetColor(new ColorConverters.RGBColor("00FF00")), //green
      //  new GradientPoint().SetColor(new ColorConverters.RGBColor("0000FF")), //blue
      //  new GradientPoint().SetColor(new ColorConverters.RGBColor("FFA500")), //orange
      //  new GradientPoint().SetColor(new ColorConverters.RGBColor("A020F0")), //purple
      //};

      req.EffectsV2 = new EffectsV2()
      {
         Action = new EffectAction
         {
            Effect = Effect.prism,
         }
      };

      var result = await localHueClient.Light.UpdateAsync(id, req);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
      Assert.AreEqual(id, result.Data.First().Rid);

    }

    [TestMethod]
    public void ColorTest()
    {
      var request = new UpdateLight().SetColor(new HueApi.ColorConverters.RGBColor("FF0000"));

      Assert.IsNotNull(request);

    }

    [TestMethod]
    public async Task SetBrightnessDeltaDown()
    {
      var all = await localHueClient.Light.GetAllAsync();
      var id = all.Data.First().Id;

      var req = new UpdateLight().SetBrightnessDelta(DeltaAction.down, 10);
      var result = await localHueClient.Light.UpdateAsync(id, req);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
      Assert.AreEqual(id, result.Data.First().Rid);
    }

    [TestMethod]
    public async Task SetBrightnessDeltaUp()
    {
      var all = await localHueClient.Light.GetAllAsync();
      var id = all.Data.First().Id;

      var req = new UpdateLight().SetBrightnessDelta(DeltaAction.up, 10);
      var result = await localHueClient.Light.UpdateAsync(id, req);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
      Assert.AreEqual(id, result.Data.First().Rid);
    }

    [TestMethod]
    public async Task SetBrightness()
    {
      var all = await localHueClient.Light.GetAllAsync();
      var id = all.Data.First().Id;

      var req = new UpdateLight().SetBrightness(50);
      var result = await localHueClient.Light.UpdateAsync(id, req);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
      Assert.AreEqual(id, result.Data.First().Rid);
    }


    [TestMethod]
    public async Task TurnOnTurnOffTest()
    {
      var all = await localHueClient.Light.GetAllAsync();
      var id = all.Data.First().Id;

      var turnOnreq = new UpdateLight().TurnOn();
      var turnOnResult = await localHueClient.Light.UpdateAsync(id, turnOnreq);

      Assert.IsNotNull(turnOnResult);
      Assert.IsFalse(turnOnResult.HasErrors);
      Assert.AreEqual(id, turnOnResult.Data.First().Rid);


      var turnOffreq = new UpdateLight().TurnOff();
      var turnOffResult = await localHueClient.Light.UpdateAsync(id, turnOffreq);

      Assert.IsNotNull(turnOffResult);
      Assert.IsFalse(turnOffResult.HasErrors);
      Assert.AreEqual(id, turnOffResult.Data.First().Rid);

    }
  }
}
