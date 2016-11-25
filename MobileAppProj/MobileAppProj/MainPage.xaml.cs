using System;
using System.Collections;
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



        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            makeMap();
            populateMap();


        }


        private async void makeMap()
        {


            // Set users current location.
            var accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:


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

                case GeolocationAccessStatus.Denied: //++++++++++++++++++++++++++++++THIS NEEDS HANDLED!! 

                    //Ask user to change location settings.
                    bool result = await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-location"));
                    break;
            }
        }


       
        private async void populateMap()
        {

            //find bus stops
            BusStops[] busStopData = await GetBusStops.API_Call();



            DepartureTimes departureTimeData;
            ArrayList tempArray = new ArrayList();
            string stop_ref = "";


            //collect stop_refs 
            for (int i=0; i<busStopData.Length; i++)
            {
                stop_ref = busStopData[i].stop_ref;
                tempArray.Add(stop_ref);
                textBoxTest1.Text += " " + tempArray[i]; //test
            }

            //find departure time data using stop_refs 
            for (int j=0; j<tempArray.Count; j++)
            { 
                departureTimeData = await GetDepartureTimes.API_Call(tempArray[j].ToString());
                //textBoxTest2.Text += " " + departureTimeData.stop.stop_ref;
                /////////////////////textBoxTest2.Text += " " + departureTimeData.times[j].depart_timestamp;++index out of bounds
                //textBoxTest2.Text += " " + departureTimeData.Length;
            }




            //  string num = busStopData[0].stop_ref; 

            // textBoxTest1.Text = num; 

            //DepartureTimes[] test = await GetDepartureTimes.API_Call(num);


            // textBoxTest2.Text = test[0].times[0].depart_timestamp;

            //textBoxTest2.Text = test[0].stop.stop_ref;





        }

       


        private async void test1_Click(object sender, RoutedEventArgs e)
        {
            
            BusStops[] test = await GetBusStops.API_Call();
            textBoxTest1.Text = test[0].stop_ref.ToString();

            //var data = GetBusStops.GetBusStopData();
            //textBoxTest1.Text = data.ToString();
        }
    }
}
