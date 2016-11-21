using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409




namespace MobileAppProj
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }


       
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Specify Galway location.
            BasicGeoposition GalwayPosition = new BasicGeoposition() { Latitude = 53.270962, Longitude = -9.062691};
            Geopoint cityCenter = new Geopoint(GalwayPosition);

            // Set the map to Galway location.
            MyMap.Center = cityCenter;
            MyMap.ZoomLevel = 14;
            MyMap.LandmarksVisible = true;
        }


    }
}
