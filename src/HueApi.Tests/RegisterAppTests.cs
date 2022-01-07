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
  public class RegisterAppTests
  {
    private readonly LocalHueClient localHueClient;

    public RegisterAppTests()
    {
      var builder = new ConfigurationBuilder().AddUserSecrets<RegisterAppTests>();
      var config = builder.Build();

      localHueClient = new LocalHueClient(config["ip"], key: config["key"]);
    }

    [TestMethod]
    public async Task RegisterApp()
    {
      var result = await localHueClient.Register(new RegisterRequest("unittest"));

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);
    }
  }
}
