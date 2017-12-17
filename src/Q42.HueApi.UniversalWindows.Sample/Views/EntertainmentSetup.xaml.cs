using Q42.HueApi.Models.Groups;
using Q42.HueApi.UniversalWindows.Sample.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Q42.HueApi.UniversalWindows.Sample.Views
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class EntertainmentSetup : Page
  {
    //TranslateTransform dragTranslation1;

    public EntertainmentSetup()
    {
      this.InitializeComponent();

      //dragTranslation1 = new TranslateTransform();
      //Elipse1.RenderTransform = this.dragTranslation1;

    }

    private void Ellipse_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
    {
      var max = EntGrid.Width / 2;
      var min = max * -1;

      if (sender is FrameworkElement uiElement)
      {
        if (uiElement.DataContext is LightPositionVm location)
        {
          location.X += e.Delta.Translation.X;
          location.Y += e.Delta.Translation.Y;
          return;
        }

        var dragTranslation1 = uiElement.RenderTransform as TranslateTransform;
        if(dragTranslation1 == null)
        {
          dragTranslation1 = new TranslateTransform();
          uiElement.RenderTransform = dragTranslation1;
        }

        if (dragTranslation1.X > max)
        {
          dragTranslation1.X = max;
          e.Complete();
          return;
        }
        else if (dragTranslation1.X < min)
        {
          dragTranslation1.X = min;
          e.Complete();
          return;
        }

        if (dragTranslation1.Y > max)
        {
          dragTranslation1.Y = max;
          e.Complete();
          return;
        }
        else if (dragTranslation1.Y < min)
        {
          dragTranslation1.Y = min;
          e.Complete();
          return;
        }

        dragTranslation1.X += e.Delta.Translation.X;
        dragTranslation1.Y += e.Delta.Translation.Y;
      }


    }


    private void Ellipse_Tapped(object sender, TappedRoutedEventArgs e)
    {
      //Get datacontext and sent alert to light
      if (sender is FrameworkElement fe)
      {
        if (fe.DataContext is LightPositionVm location)
        {
          ((MainViewModel)this.DataContext).SendAlert(location.Id);

        }

      }
    }

    private void LoadButton_Click(object sender, RoutedEventArgs e)
    {
      string groupId = EntGroupId.Text;
      ((MainViewModel)this.DataContext).GetEntertainmentGroup(groupId);
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
      string groupId = EntGroupId.Text;
      ((MainViewModel)this.DataContext).SaveGroupLocationAction(groupId);
    }

    private async void CreateEntGroup_Click(object sender, RoutedEventArgs e)
    {
      var result = await ((MainViewModel)this.DataContext).CreateNewEntertainmentGroup();
      EntGroupId.Text = result;

      //Load new group
      await ((MainViewModel)this.DataContext).GetEntertainmentGroup(result);

      //Random position new group
      Random r = new Random();
      var positions = ((MainViewModel)this.DataContext).GroupForPositioning;
      foreach(var pos in positions)
      {
        pos.X = r.NextDouble() * 400;
        pos.Y = r.NextDouble() * 400;
      }

      //Save new group
      await ((MainViewModel)this.DataContext).SaveGroupLocationAction(result);


    }
  }
}
