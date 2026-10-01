using HueApi.Models.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HueApi.Tests
{
  [TestClass]
  public class BehaviorScriptFormulaTests
  {
    private readonly LocalHueApi localHueClient;

    public BehaviorScriptFormulaTests()
    {
      var builder = new ConfigurationBuilder().AddUserSecrets<BehaviorScriptFormulaTests>();
      var config = builder.Build();

      localHueClient = new LocalHueApi(config["ip"], key: config["key"]);
    }

    [TestMethod]
    public async Task Get()
    {
      var result = await localHueClient.BehaviorScriptFormula.GetAllAsync();

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);
    }

    [TestMethod]
    public async Task GetById()
    {
      var all = await localHueClient.BehaviorScriptFormula.GetAllAsync();
      var id = all.Data.First().Id;

      var result = await localHueClient.BehaviorScriptFormula.GetByIdAsync(id);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
    }
  }
}
