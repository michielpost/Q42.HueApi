using HueApi.Extensions.cs;
using HueApi.Models.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HueApi.Tests
{
  [TestClass]
  public class ServiceGroupTests
  {
    private readonly LocalHueApi localHueClient;

    public ServiceGroupTests()
    {
      var builder = new ConfigurationBuilder().AddUserSecrets<ServiceGroupTests>();
      var config = builder.Build();

      localHueClient = new LocalHueApi(config["ip"], key: config["key"]);
    }

    [TestMethod]
    public async Task Get()
    {
      var result = await localHueClient.ServiceGroup.GetAllAsync();

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);
    }

    [TestMethod]
    public async Task GetById()
    {
      var all = await localHueClient.ServiceGroup.GetAllAsync();
      var id = all.Data.First().Id;

      var result = await localHueClient.ServiceGroup.GetByIdAsync(id);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);

    }

    [TestMethod]
    public async Task Create()
    {
      var resources = await localHueClient.Motion.GetAllAsync();
      var res1 = resources.Data.First();
      var res2 = resources.Data.Last();

      var req = new CreateUpdateServiceGroup()
      {
        Children = new System.Collections.Generic.List<Models.ResourceIdentifier>()
         {
          res1.ToResourceIdentifier(),
          res2.ToResourceIdentifier(),
         },
        Metadata = new Models.Metadata()
        {
          Name = "Test Service Group",
        }
      };

      var result = await localHueClient.ServiceGroup.CreateAsync(req);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);

    }


    [TestMethod]
    public async Task PutById()
    {
      var all = await localHueClient.ServiceGroup.GetAllAsync();
      var id = all.Data.Last().Id;

      var req = new CreateUpdateServiceGroup();
      var result = await localHueClient.ServiceGroup.UpdateAsync(id, req);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
      Assert.AreEqual(id, result.Data.First().Rid);

    }
  }
}
