using HueApi;
using HueApi.BridgeLocator;
using HueApi.Models;
using HueApi.Models.Requests;
using HueDiagnostics.Models;
using KellermanSoftware.CompareNetObjects;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace HueDiagnostics
{
  internal class Program
  {
    static async Task Main(string[] args)
    {
      var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json");

      var configuration = builder.Build();

      List<BridgeAppConfig> bridgesConfig = configuration.GetSection("bridges").Get<List<BridgeAppConfig>>()?.Where(x => !string.IsNullOrWhiteSpace(x.Ip)).ToList() ?? new();

      Console.WriteLine("Running Hue Diagnostics...");

      Console.WriteLine("Getting bridge IPs from config");
      Console.WriteLine($"Found {bridgesConfig.Count()}");
      foreach (var bridge in bridgesConfig)
      {
        Console.WriteLine($"{bridge.Ip}");
      }
      Console.WriteLine();

      Console.WriteLine("Getting bridge IPs from HTTP discovery...");
      IBridgeLocator locator = new HttpBridgeLocator(); //Or: LocalNetworkScanBridgeLocator, MdnsBridgeLocator, MUdpBasedBridgeLocator
      var bridges = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5));

      Console.WriteLine($"Found {bridges.Count()}");
      foreach(var bridge in bridges)
      {
        Console.WriteLine($"{bridge.IpAddress} | {bridge.BridgeId}");
      }
      Console.WriteLine();

      var missingConfig = bridges.Select(x => x.IpAddress).Except(bridgesConfig.Select(x => x.Ip));
      var missingHttp = bridgesConfig.Select(x => x.Ip).Except(bridges.Select(x => x.IpAddress));

      foreach(var missing in missingConfig)
      {
        Console.WriteLine($"{missing} is missing in appsettings.json");
      }
      foreach (var missing in missingHttp)
      {
        Console.WriteLine($"{missing} is not detected by HTTP discovery");
      }
      Console.WriteLine();

      CompareLogic compareLogic = new CompareLogic(new ComparisonConfig()
      {
        MaxDifferences = int.MaxValue,
        MembersToIgnore = new List<string>
            {
              "WhiteList",
              "Lights"
            }
      });

      var allBridgeIps = bridges.Select(x => x.IpAddress).Union(bridgesConfig.Select(x => x.Ip)).Where(x => !string.IsNullOrEmpty(x)); ;
      if (!allBridgeIps.Any())
        Console.WriteLine("No bridge IPs to check");
      else
      {
        List<DiagnosticsData> diagDataList = new();
        foreach (var bridgeIp in allBridgeIps)
        {
          Console.WriteLine($"Getting bridge config for: {bridgeIp}");

          DiagnosticsData diagData = new DiagnosticsData();
          diagData.BridgeIp = bridgeIp;

          //Get local client
          LocalHueApi client = new LocalHueApi(bridgeIp!, key: null);

          //Get config
          try
          {
            var deviceDataResult = await client.Device.GetAllAsync();

            var deviceData = deviceDataResult.Data.FirstOrDefault();
            diagData.Device = deviceData;
          }
          catch(Exception ex)
          {
            Console.WriteLine($"Unable to get bridge config for: {bridgeIp}");
            Console.WriteLine($"Exception: {ex}");
          }

          diagDataList.Add(diagData);
        }

        Console.WriteLine();
        if (diagDataList.Count == 1)
        {
          var json = JsonSerializer.Serialize(diagDataList.First(), new JsonSerializerOptions { WriteIndented = true });
          Console.WriteLine("Bridge config:");
          Console.WriteLine(json);
        }
        else if (diagDataList.Count > 1)
        {
          Console.WriteLine($"Comparing values...");
       
          var first = diagDataList.First();

          foreach (var diag in diagDataList.Skip(1))
          {
            ComparisonResult result = compareLogic.Compare(first, diag);

            if (!result.AreEqual)
              Console.WriteLine(result.DifferencesString);
            else
              Console.WriteLine("No differences found");
          }

        }

      }

      var bridgesWithKey = bridgesConfig.Where(x => !string.IsNullOrEmpty(x.Key));
      if (!bridgesWithKey.Any())
        Console.WriteLine("Please add key to bridge config in appsettings.json to run additional checks");
      else
      {
        List<DiagnosticsData> diagDataList = new();
        foreach (var bridge in bridgesWithKey)
        {
          Console.WriteLine($"Getting bridge config for: {bridge.Ip}");

          DiagnosticsData diagData = new DiagnosticsData();
          diagData.BridgeIp = bridge.Ip;

          //Get local client
          LocalHueApi client = new LocalHueApi(bridge.Ip!, bridge.Key!);

          //Get config
          try
          {
            var deviceDataResult = await client.Device.GetAllAsync();
            var bridgeDataResult = await client.Bridge.GetAllAsync();

            var bridgeData = bridgeDataResult.Data.FirstOrDefault();
            diagData.Bridge = bridgeData;

            var deviceData = deviceDataResult.Data.FirstOrDefault();
            diagData.Device = deviceData;

            if (deviceData != null)
            {
              Console.WriteLine($"SoftwareVersion: {deviceData.ProductData.SoftwareVersion}");
              Console.WriteLine();

            }

            if (bridgeData != null)
            {
              Console.WriteLine($"TimeZone: {bridgeData.TimeZone}");
            }
          }
          catch (Exception ex)
          {
            Console.WriteLine($"Unable to get bridge for: {bridge.Ip}");
            Console.WriteLine($"Exception: {ex}");
          }

          diagDataList.Add(diagData);

          var command = new UpdateLight();
          command.Alert = new UpdateAlert();

          await client.Light.UpdateAsync(Guid.Empty, command);
          Console.WriteLine("Sending Alert");
          //Console.ReadLine();

        }

        Console.WriteLine();
        if (diagDataList.Count == 1)
        {
          var json = JsonSerializer.Serialize(diagDataList.First().Bridge, new JsonSerializerOptions { WriteIndented = true });
          Console.WriteLine("Bridge:");
          Console.WriteLine(json);
        }
        else if (diagDataList.Count > 1)
        {
          Console.WriteLine($"Comparing values...");
          var first = diagDataList.First();

          foreach (var diag in diagDataList.Skip(1))
          {
            ComparisonResult result = compareLogic.Compare(first.Bridge, diag.Bridge);

            if (!result.AreEqual)
              Console.WriteLine(result.DifferencesString);
            else
              Console.WriteLine("No differences found");
          }
        }
      }



      Console.WriteLine();
      Console.WriteLine("Finished");
      Console.ReadLine();
    }
  }
}
