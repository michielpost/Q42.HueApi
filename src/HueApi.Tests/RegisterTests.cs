using HueApi.Models.Exceptions;
using HueApi.Models.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HueApi.Tests
{
  [TestClass]
  public class RegisterTests
  {
    private readonly string ip;

    public RegisterTests()
    {
      var builder = new ConfigurationBuilder().AddUserSecrets<BehaviorInstanceTests>();
      var config = builder.Build();

      ip = config["ip"];
    }

    [TestMethod]
    public async Task RegisterAppWithBridgeTest()
    {
      try
      {
        var result = await HueApi.LocalHueApi.RegisterAsync(ip, "test", "test", false);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Ip);
        Assert.IsNotNull(result.Username);
      }
      catch(LinkButtonNotPressedException ex)
      {
        Assert.IsNotNull(ex);
      }
      
    }
  }
}
