using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Q42.HueApi;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.ColorConverters.OriginalWithModel;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models.Bridge;
using Q42.WinRT.Data;
using Q42.WinRT.Portable.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Q42.HueApi.WinRT.Sample.ViewModel
{
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



			IBridgeLocator httpLocator = new HttpBridgeLocator();
			IBridgeLocator ssdpLocator = new SSDPBridgeLocator();

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
				set { _registerSuccess = value;
				RaisePropertyChanged("RegisterSuccess");
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
					var result = await RegisterDataLoader.LoadAsync(() => _hueClient.RegisterAsync(p, p));

					RegisterSuccess = !string.IsNullOrEmpty(result);
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
		}
}