using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Q42.HueApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Tests
{
  [TestClass]
	public class Schedule_GenericScheduleCommandTests
	{
		[TestMethod]
		public void CanConvertToSceneCommand()
		{
			SceneCommand sceneCommand = new SceneCommand();
			sceneCommand.Scene = "test123";

			var json = JsonConvert.SerializeObject(sceneCommand);

			GenericScheduleCommand genericCommand = new GenericScheduleCommand(json);

			Assert.IsTrue(genericCommand.IsSceneCommand());
			Assert.IsNotNull(genericCommand.AsSceneCommand());

			var scene = genericCommand.AsSceneCommand();
			Assert.AreEqual(sceneCommand.Scene, scene.Scene);
		}

		[TestMethod]
		public void CanConvertToLightCommand()
		{
			LightCommand lightCommand = new LightCommand();
			lightCommand.Alert = Alert.Multiple;
			lightCommand.On = true;

			var json = JsonConvert.SerializeObject(lightCommand);

			GenericScheduleCommand genericCommand = new GenericScheduleCommand(json);

			Assert.IsFalse(genericCommand.IsSceneCommand());
			Assert.IsNotNull(genericCommand.AsLightCommand());

			var light = genericCommand.AsLightCommand();
			Assert.AreEqual(lightCommand.Alert, light.Alert);
		}
	}
}
