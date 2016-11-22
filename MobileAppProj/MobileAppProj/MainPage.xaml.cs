using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
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

                    /*
                    var geolocator = new Geolocator { DesiredAccuracyInMeters = 0 };

                    var position = await geolocator.GetGeopositionAsync();

                    textBoxLat.Text = position.Coordinate.Latitude.ToString();
                    textBoxLong.Text = position.Coordinate.Longitude.ToString();

                    */

                   
                    // Get users current location.
                    Geolocator geolocator = new Geolocator();
                    Geoposition pos = await geolocator.GetGeopositionAsync();
                    Geopoint myLocation = pos.Coordinate.Point;


                    // Create an icon for user
                    MapIcon userIcon = new MapIcon();
                    userIcon.Location = myLocation;
                    userIcon.NormalizedAnchorPoint = new Point(0.5, 1.0);
                    userIcon.Title = "You are here";
                    userIcon.ZIndex = 0;

                    // Add the icon to the map.
                    MyMap.MapElements.Add(userIcon);

                   
                    // Set the map's location.
                    MyMap.Center = myLocation;
                    MyMap.ZoomLevel = 14;
                    MyMap.LandmarksVisible = true;

                    break;

                    case GeolocationAccessStatus.Denied:

                        //Ask user to change location settings.
                       bool result = await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-location"));
                       break;
                }

        }


       



    }
}
