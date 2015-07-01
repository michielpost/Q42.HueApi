using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Tests.Remote
{
	[TestClass]
	public class RemoteLightsTests : LightsTests
	{
		[TestInitialize]
		public new void Initialize()
		{
			var remoteBridge = new RemoteHueClient("test");
			//remoteBridge.Initialize("bridgeId");

			_client = remoteBridge;
		}

		[TestMethod]
		public async Task SetLightNameTest()
		{
			await _client.SetLightNameAsync("1", "test");
			var result = await _client.GetLightAsync("1");
			Assert.AreEqual("test", result.Name);

			await _client.SetLightNameAsync("1", "test change");
			result = await _client.GetLightAsync("1");
			Assert.AreEqual("test change", result.Name);


		}
	}
}
