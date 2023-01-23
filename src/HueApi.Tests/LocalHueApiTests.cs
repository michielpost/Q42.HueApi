using HueApi.BridgeLocator;
using HueApi.Models.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HueApi.Tests
{
  [TestClass]
  public class LocalHueApiTests
  {

    private string ip;
    private string key;

    public LocalHueApiTests()
    {
      var builder = new ConfigurationBuilder().AddUserSecrets<BridgeHomeTests>();
      var config = builder.Build();

      ip = config["ip"];
      key= config["key"];
    }

    [TestMethod]
    public async Task CreateHueClientWithUsedHttpClient()
    {
      HttpClient httpClient = new HttpClient();

      //Use the existing httpClient
      var httpResult = await httpClient.GetAsync("https://www.google.com");

      var localHueClient = new LocalHueApi(ip, key, httpClient);
      var result = await localHueClient.GetBridgeHomesAsync();

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);
    }

    [TestMethod]
    public async Task StartEventStreamWithUsedHttpClient()
    {
      HttpClient httpClient = new HttpClient();

      //Use the existing httpClient
      var httpResult = await httpClient.GetAsync("https://www.google.com");

      var localHueClient = new LocalHueApi(ip, key);
      localHueClient.StartEventStream(httpClient);

      await Task.Delay(TimeSpan.FromSeconds(5));
    }

  }
}
