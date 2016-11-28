using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
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

        private BusStops[] busStopData;
      

        public MainPage()
        {
            this.InitializeComponent();
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {

            makeMap();
            await populateMap();

        }


        private async void makeMap()
        {


            // Set users current location
            var accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:


                // Get users current location
                Geolocator geolocator = new Geolocator();
                Geoposition pos = await geolocator.GetGeopositionAsync();
                Geopoint myLocation = pos.Coordinate.Point;

                // Create an icon for user
                MapIcon userIcon = new MapIcon();
                userIcon.Location = myLocation;
                userIcon.NormalizedAnchorPoint = new Point(0.5, 1.0);
                userIcon.Title = "You are here";
                userIcon.ZIndex = 0;
                userIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/userIcon.png"));


                // Add the icon to the map
                MyMap.MapElements.Add(userIcon);

                // Set the map's location
                MyMap.Center = myLocation;
                MyMap.ZoomLevel = 16; //14
                MyMap.LandmarksVisible = true;

                break;

            case GeolocationAccessStatus.Denied: //++++++++++++++++++++++++++++++THIS NEEDS HANDLED!! 

                //Ask user to change location settings.
                bool result = await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-location"));
                break;
            }
        }



        //private async void populateMap()
        private async Task<BusStops[]> populateMap()
        {

            //find bus stops
            busStopData = await GetBusStops.API_Call();

            //loop through busStopData
            for (int i = 0; i < busStopData.Length; i++)
            {


                //create map icon
                MapIcon tempMapIcon = new MapIcon();
                

                //find location for icon
                BasicGeoposition iconPosition = new BasicGeoposition()
                {
                    Latitude = busStopData[i].latitude,
                    Longitude = busStopData[i].longitude
                };

 
                //define icon
                Geopoint iconPoint = new Geopoint(iconPosition);
                tempMapIcon.Location = iconPoint;
                tempMapIcon.NormalizedAnchorPoint = new Point(0.5, 1.0);
                tempMapIcon.ZIndex = 0;
                tempMapIcon.Title = busStopData[i].long_name;
                //tempMapIcon.Title = busStopData[i].stop_ref;
                /*
                MapIconObj tempMapIconObj = new MapIconObj();
                tempMapIconObj.mapIcon = tempMapIcon;
                tempMapIconObj.stop_id = busStopData[i].stop_id;
                */

                //add icon to map
                MyMap.MapElements.Add(tempMapIcon);

            }

            return busStopData;

        }



        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++DELETE!!
        private void test1_Click(object sender, RoutedEventArgs e)
        {

            //BusStops[] test = await GetBusStops.API_Call();
            // textBoxTest1.Text = test[0].stop_ref.ToString();

            //var data = GetBusStops.GetBusStopData();
            //textBoxTest1.Text = data.ToString();
        }
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++DELETE!!



        private void mapIcon_Click(MapControl sender, MapElementClickEventArgs args)
        {

            //find which icon element has been clicked 
            MapIcon clickedIcon = args.MapElements.FirstOrDefault(x => x is MapIcon) as MapIcon;
            //Debug.WriteLine("Icon clicked with ID: " + clickedIcon.Title); //test 
            Debug.WriteLine("Icon clicked with Long Name: " + clickedIcon.Title); //test 

            string long_name = clickedIcon.Title;

            /*
            switch (long_name)
            {
                case "You are here":
                    showMessage(long_name);
                    break;
                default:
                    string stop_ref = "";

                    //loop through busStopData
                    for (int i = 0; i < busStopData.Length; i++)
                    {
                        //find stop_ref
                        if (long_name == busStopData[i].long_name)
                        {
                            stop_ref = busStopData[i].stop_ref;
                            break;
                        }
                    }

                    makeDepartureTimes(stop_ref);
                    break;
            }*/
           

            
            if (!long_name.Contains("You are here"))
            {
                string stop_ref = "";

                //loop through busStopData
                for (int i = 0; i < busStopData.Length; i++)
                {
                    //find stop_ref
                    if (long_name == busStopData[i].long_name)
                    {
                        stop_ref = busStopData[i].stop_ref;
                        break;
                    }
                }

                makeDepartureTimes(stop_ref);
            }
            else
            {
                showMessage(long_name);
            }

        }

        //private async void makeDepartureTimes(string stopName, string stop_ref)
        private async void makeDepartureTimes(string stop_ref)
        {

            //ProgressRing progRing = new ProgressRing();

           // progRing.IsActive = true; //++++++++++++++++++++++++++++++++++++++++++++++++++++++NOT WORKING?? 
            DepartureTimes departureTimeData = await GetDepartureTimes.API_Call(stop_ref);
           // progRing.IsActive = false;

            string message = string.Format("Route" + " | " + "Destination" +  " | " + "Time" + "\n\n");
            ///string depart_timestamp = "";
            DateTime nextBus;
            DateTime currentTime;
            TimeSpan timeUntillNextBus;
            

            //loop through list of departure times
            for (int i = 0; i < departureTimeData.times.Count; i++)
            {
                //find due time
                //depart_timestamp = departureTimeData.times[i].depart_timestamp;
                nextBus = DateTime.Parse(departureTimeData.times[i].depart_timestamp);
                currentTime = DateTime.UtcNow;
                timeUntillNextBus = nextBus.Subtract(currentTime);

                 //append string
                 message += string.Format("{0}" + " | " + "{1}" + " | " + "{2} hours" + " {3} minutes" + "\n\n", departureTimeData.times[i].timetable_id, departureTimeData.times[i].display_name, timeUntillNextBus.Hours, timeUntillNextBus.Minutes);
            }

            showMessage(message);

        }




        private async void showMessage(string message)
        {
            MessageDialog dialog = new MessageDialog(message);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await dialog.ShowAsync());
        }

    }

}
