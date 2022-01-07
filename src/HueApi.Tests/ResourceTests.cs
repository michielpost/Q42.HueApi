using HueApi.BridgeLocator;
using HueApi.Models.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HueApi.Tests
{
  [TestClass]
  public class ResourceTests
  {
    private readonly LocalHueClient localHueClient;

    public ResourceTests()
    {
      var builder = new ConfigurationBuilder().AddUserSecrets<ResourceTests>();
      var config = builder.Build();

      localHueClient = new LocalHueClient(config["ip"], key: config["key"]);
    }

    [TestMethod]
    public async Task Get()
    {
      var result = await localHueClient.GetResources();

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);
    }
  
  }
}
