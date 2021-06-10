using System.Collections.Generic;
using System.Windows;
using Microsoft.Maps.MapControl.WPF;
using System.Windows.Media;
using CPTS451_Milestone1;

namespace BingMaps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(List<Business> coordinates, bool userIDSelected)
        {
            InitializeComponent();


            for (int i = 0; i < coordinates.Count; i++)
            {
                Pushpin pushpin = new Pushpin();

                if (userIDSelected && i == coordinates.Count - 1)
                {
                    pushpin.Background = new SolidColorBrush(Colors.Red);
                } 
                else
                {
                    pushpin.Background = new SolidColorBrush(Colors.Blue);
                    pushpin.ToolTip = coordinates[i].Name + "\n" + "Stars: " + coordinates[i].Stars.ToString();
                }

                pushpin.Location = new Microsoft.Maps.MapControl.WPF.Location(coordinates[i].Latitude, coordinates[i].Longitude);
                this.bingMap.Children.Add(pushpin);
            }
           
        }
    }
}
