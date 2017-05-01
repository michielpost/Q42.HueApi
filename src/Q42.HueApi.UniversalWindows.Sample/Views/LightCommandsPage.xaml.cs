using Q42.HueApi.UniversalWindows.Sample.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Q42.HueApi.UniversalWindows.Sample.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class LightCommandsPage : Page
	{
		public LightCommandsPage()
		{
			this.InitializeComponent();
		}

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.  The Parameter
		/// property is typically used to configure the page.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
		}

		private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
		{
			byte R, G, B, A;

			A = 255;
			R = Convert.ToByte(RSlider.Value);
			G = Convert.ToByte(GSlider.Value);
			B = Convert.ToByte(BSlider.Value);

			Color myColor = new Color();
			myColor = Color.FromArgb(A, R, G, B);

			showColor.Fill = new SolidColorBrush(myColor);
		}

		private void ColorButton_Click(object sender, RoutedEventArgs e)
		{
			((MainViewModel)this.DataContext).SetColor((int)RSlider.Value, (int)GSlider.Value, (int)BSlider.Value);

		}
	}
}
