using HueDiagnostics.Models;
using KellermanSoftware.CompareNetObjects;
using Microsoft.Extensions.Configuration;
using Q42.HueApi;
using Q42.HueApi.Interfaces;
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
          ILocalHueClient client = new LocalHueClient(bridgeIp!);

          //Get config
          try
          {
            var config = await client.GetConfigAsync();
            diagData.BridgeConfig = config;
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
          ILocalHueClient client = new LocalHueClient(bridge.Ip!, bridge.Key!);

          //Get config
          try
          {
            var bridgeData = await client.GetBridgeAsync();
            diagData.Bridge = bridgeData;

            if (bridgeData?.Config != null)
            {
              Console.WriteLine($"API: {bridgeData.Config.ApiVersion}");
              Console.WriteLine($"LocalTime: {bridgeData.Config.LocalTime}");
              Console.WriteLine($"SoftwareVersion: {bridgeData.Config.SoftwareVersion}");
              Console.WriteLine($"SoftwareUpdate (deprecated): {bridgeData.Config.SoftwareUpdate?.UpdateState}");
              Console.WriteLine($"SoftwareUpdate (deprecated): {bridgeData.Config.SoftwareUpdate?.Text}");

              Console.WriteLine($"SoftwareUpdate:");
              Console.WriteLine($"State: {bridgeData.Config.SoftwareUpdate2.State}");
              Console.WriteLine($"LastChange: {bridgeData.Config.SoftwareUpdate2.LastChange}");
              Console.WriteLine($"AutoInstall: {bridgeData.Config.SoftwareUpdate2.AutoInstall.On}");
              Console.WriteLine($"AutoInstall: {bridgeData.Config.SoftwareUpdate2.AutoInstall.UpdateTime}");
              Console.WriteLine();

            }
          }
          catch (Exception ex)
          {
            Console.WriteLine($"Unable to get bridge for: {bridge.Ip}");
            Console.WriteLine($"Exception: {ex}");
          }

          diagDataList.Add(diagData);

          var command = new LightCommand();
          command.Alert = Alert.Multiple;

          await client.SendGroupCommandAsync(command);
          Console.WriteLine("Sending Alert");
          //Console.ReadLine();

        }

        Console.WriteLine();
        if (diagDataList.Count == 1)
        {
          var json = JsonSerializer.Serialize(diagDataList.First().Bridge?.Config, new JsonSerializerOptions { WriteIndented = true });
          Console.WriteLine("Bridge:");
          Console.WriteLine(json);
        }
        else if (diagDataList.Count > 1)
        {
          Console.WriteLine($"Comparing values...");
          var first = diagDataList.First();

          foreach (var diag in diagDataList.Skip(1))
          {
            ComparisonResult result = compareLogic.Compare(first.Bridge?.Config, diag.Bridge?.Config);

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
