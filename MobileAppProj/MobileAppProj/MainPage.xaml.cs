using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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



        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {

            /*
             // Specify Galway location.
             BasicGeoposition GalwayPosition = new BasicGeoposition() { Latitude = 53.270962, Longitude = -9.062691};
             Geopoint cityCenter = new Geopoint(GalwayPosition);

             // Set the map to Galway location.
             MyMap.Center = cityCenter;
             MyMap.ZoomLevel = 14;
             MyMap.LandmarksVisible = true;
             */


          
            // Set users current location.
            var accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:

                    // Get users current location.
                    Geolocator geolocator = new Geolocator();
                    Geoposition pos = await geolocator.GetGeopositionAsync();
                    Geopoint myLocation = pos.Coordinate.Point;

                    // Set the map's location.
                    MyMap.Center = myLocation;
                    MyMap.ZoomLevel = 14;
                    MyMap.LandmarksVisible = true;
                    break;

                case GeolocationAccessStatus.Denied:

                    //Ask user to change location settings if access is denied
                    bool result = await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-location"));
                    break;
            }


        }
    }
}
