using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Q42.HueApi;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.ColorConverters.HSB;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models.Bridge;
using Q42.HueApi.Models.Groups;
using Q42.WinRT.Portable.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Q42.HueApi.UniversalWindows.Sample.ViewModel
{

  public class LightPositionVm : ViewModelBase
  {
    public string Id { get; set; }
    public LightLocation LightLocation { get; set; }

    int halfGridSize = 200;

    public LightPositionVm(KeyValuePair<string, LightLocation> original)
    {
      this.Id = original.Key;
      this.LightLocation = original.Value;
    }

    public double X
    {
      get { return (LightLocation.X * halfGridSize) + halfGridSize; }
      set
      {
        LightLocation.X = (value - halfGridSize) / halfGridSize;
        RaisePropertyChanged("X");
      }
    }

    public double Y
    {
      get { return (LightLocation.Y * -halfGridSize) + halfGridSize; }
      set
      {
        LightLocation.Y = (value - halfGridSize) / -halfGridSize;
        RaisePropertyChanged("Y");
      }
    }

  }

  /// <summary>
  /// This class contains properties that the main View can data bind to.
  /// <para>
  /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
  /// </para>
  /// <para>
  /// You can also use Blend to data bind with the tool's support.
  /// </para>
  /// <para>
  /// See http://www.galasoft.ch/mvvm
  /// </para>
  /// </summary>
  public class MainViewModel : ViewModelBase
  {
    public DataLoader LocateBridgeDataLoader { get; set; }
    public RelayCommand LocateBridgeCommand { get; set; }

    public DataLoader SsdpLocateBridgeDataLoader { get; set; }
    public RelayCommand SsdpLocateBridgeCommand { get; set; }

    public DataLoader RegisterDataLoader { get; set; }


    public RelayCommand GetLightsCommand { get; set; }

    public RelayCommand TurnOnCommand { get; set; }
    public RelayCommand TurnOffCommand { get; set; }
    public RelayCommand GreenCommand { get; set; }
    public RelayCommand RedCommand { get; set; }
    public RelayCommand ColorloopCommand { get; set; }
    public RelayCommand FlashCommand { get; set; }


    private List<LightPositionVm> _groupForPositioning;

    public List<LightPositionVm> GroupForPositioning
    {
      get { return _groupForPositioning; }
      set
      {
        _groupForPositioning = value;
        RaisePropertyChanged("GroupForPositioning");
      }
    }


    IBridgeLocator httpLocator = new HttpBridgeLocator();

    internal void SendAlert(string id)
    {
      _hueClient.SendCommandAsync(new LightCommand() { Alert = Alert.Once }, new[] { id });
    }

    IBridgeLocator ssdpLocator = new SsdpBridgeLocator();

    LocalHueClient _hueClient;


    private string _httpBridges;

    public string HttpBridges
    {
      get { return _httpBridges; }
      set
      {
        _httpBridges = value;
        RaisePropertyChanged("HttpBridges");
      }
    }

    private string _ssdpBridges;

    public string SsdpBridges
    {
      get { return _ssdpBridges; }
      set
      {
        _ssdpBridges = value;
        RaisePropertyChanged("SsdpBridges");
      }
    }

    private bool _registerSuccess;

    public bool RegisterSuccess
    {
      get { return _registerSuccess; }
      set
      {
        _registerSuccess = value;
        RaisePropertyChanged("RegisterSuccess");
      }
    }

    private RegisterEntertainmentResult _registerEntertainmentResult;

    public RegisterEntertainmentResult RegisterEntertainmentResult
    {
      get { return _registerEntertainmentResult; }
      set
      {
        _registerEntertainmentResult = value;
        RaisePropertyChanged("RegisterEntertainmentResult");
      }
    }

    private string _getLights;

    public string GetLights
    {
      get { return _getLights; }
      set
      {
        _getLights = value;
        RaisePropertyChanged("GetLights");
      }
    }




    /// <summary>
    /// Initializes a new instance of the MainViewModel class.
    /// </summary>
    public MainViewModel()
    {
      LocateBridgeDataLoader = new DataLoader();
      SsdpLocateBridgeDataLoader = new DataLoader();

      RegisterDataLoader = new DataLoader();


      LocateBridgeCommand = new RelayCommand(LocateBridgeAction);
      SsdpLocateBridgeCommand = new RelayCommand(SsdpLocateBridgeAction);

      GetLightsCommand = new RelayCommand(GetLightsAction);

      TurnOnCommand = new RelayCommand(TurnOnAction);
      TurnOffCommand = new RelayCommand(TurnOffAction);
      GreenCommand = new RelayCommand(GreenAction);
      RedCommand = new RelayCommand(RedAction);
      ColorloopCommand = new RelayCommand(ColorloopAction);
      FlashCommand = new RelayCommand(FlashAction);

      //GroupForPositioning = new Dictionary<string, LightLocation>()
      //  {
      //    { "1", new LightLocation { 60, 50,0 } },
      //    { "2", new LightLocation { 0.5, 0.1,0 } },
      //    { "3", new LightLocation { 0.7, 0.1,0 } },
      //    { "4", new LightLocation { 0.1, 0.7,0 } },
      //  }.Select(x => new LightPositionVm(x)).ToList();


    }


    private async void LocateBridgeAction()
    {





      var result = await LocateBridgeDataLoader.LoadAsync(() => httpLocator.LocateBridgesAsync(TimeSpan.FromSeconds(5)));

      HttpBridges = string.Join(", ", result.Select(x => x.IpAddress).ToArray());

      if (result.Count() > 0)
        _hueClient = new LocalHueClient(result.Select(x => x.IpAddress).First());
    }

    private async void SsdpLocateBridgeAction()
    {
      var result = await SsdpLocateBridgeDataLoader.LoadAsync(() => ssdpLocator.LocateBridgesAsync(TimeSpan.FromSeconds(5)));

      if (result == null)
        result = new List<LocatedBridge>();

      SsdpBridges = string.Join(", ", result.Select(x => x.IpAddress).ToArray());

      if (result.Count() > 0)
        _hueClient = new LocalHueClient(result.Select(x => x.IpAddress).First());
    }

    internal async void Register(string p)
    {
      var result = await RegisterDataLoader.LoadAsync(() => _hueClient.RegisterAsync(p, p, generateClientKey: true));

      RegisterEntertainmentResult = result;
      RegisterSuccess = !string.IsNullOrEmpty(result?.Username);
    }

    internal void Initialize(string p)
    {
      _hueClient.Initialize(p);
    }


    private async void GetLightsAction()
    {
      var result = await _hueClient.GetLightsAsync();

      GetLights = string.Format("Found {0} lights", result.Count());
    }


    private void RedAction()
    {
      LightCommand command = new LightCommand();
      command.TurnOn().SetColor(new RGBColor("FF0000"));

      _hueClient.SendCommandAsync(command);
    }

    private void GreenAction()
    {
      LightCommand command = new LightCommand();
      command.TurnOn().SetColor(new RGBColor("00FF00"));

      _hueClient.SendCommandAsync(command);
    }

    private void TurnOffAction()
    {
      LightCommand command = new LightCommand();
      command.TurnOff();

      _hueClient.SendCommandAsync(command);
    }

    private void TurnOnAction()
    {
      LightCommand command = new LightCommand();
      command.TurnOn();

      _hueClient.SendCommandAsync(command);
    }

    private void ColorloopAction()
    {
      LightCommand command = new LightCommand();
      command.TurnOn();
      command.Effect = Effect.ColorLoop;

      _hueClient.SendCommandAsync(command);
    }

    private void FlashAction()
    {
      LightCommand command = new LightCommand();
      command.TurnOn();
      command.Alert = Alert.Once;

      _hueClient.SendCommandAsync(command);
    }

    public void SetColor(int r, int g, int b)
    {
      LightCommand command = new LightCommand();
      command.TurnOn();
      command.SetColor(new RGBColor(r, g, b));

      _hueClient.SendCommandAsync(command);
    }



    internal void ManualRegister(string ip)
    {
      _hueClient = new LocalHueClient(ip);
    }

    internal async Task GetEntertainmentGroup(string groupId)
    {
      var bridge = await _hueClient.GetBridgeAsync();
      var group = bridge.Groups.Where(x => x.Id == groupId && x.Type == GroupType.Entertainment).FirstOrDefault();

      if (group == null)
        throw new Exception("No entertainment group found with id: " + groupId);

      GroupForPositioning = group.Locations.Select(x => new LightPositionVm(x)).ToList();

    }

    public async Task SaveGroupLocationAction(string groupId)
    {
      var locations = GroupForPositioning.ToDictionary(x => x.Id, x => x.LightLocation);

      await _hueClient.UpdateGroupLocationsAsync(groupId, locations);
    }

    public async Task<string> CreateNewEntertainmentGroup()
    {
      var allLights = await _hueClient.GetLightsAsync();

      var newGroupId = await _hueClient.CreateGroupAsync(allLights.Take(10).Select(x => x.Id), "New Entertainment Group", RoomClass.TV, GroupType.Entertainment);

      return newGroupId;
    }
  }
}
