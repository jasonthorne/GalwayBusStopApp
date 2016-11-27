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
       // private DepartureTimes departureTimeData;
        //private List<MapIconObj> mapIconList = new List<MapIconObj>();


        //ArrayList tempArray = new ArrayList(); //DELETE++++++++++++++++++


        public MainPage()
        {
            this.InitializeComponent();
        }

        /*
        public class MapIconObj
        {
            public MapIcon mapIcon { get; set; }
            public int stop_id { get; set; }
        }
        */



        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {

            makeMap();
            await populateMap();

            /*
            string stop_ref = "";

            //collect stop_refs 
            for (int i = 0; i < busStopData.Length; i++)
            {
                stop_ref = busStopData[i].stop_ref;
                tempArray.Add(stop_ref);
                textBoxTest1.Text += " " + tempArray[i]; //test
            }
            */

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
                    userIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/userIcon.png"));


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
                //var tempButton = new Button();
                //tempButton.Content = "TEST"; //////////////////////////REMOVE LATER!! (& make button invisible)
                //tempButton.Visibility = System.Windows.Visibility.Hidden; //---------------NOT WORKING! 
                //this.MyMap.Children.Add(tempButton);


                //find location for icon
                BasicGeoposition iconPosition = new BasicGeoposition()
                {
                    Latitude = busStopData[i].latitude,
                    Longitude = busStopData[i].longitude
                };

                /*
                //find location for button
                var buttonPosition = new Geopoint(new BasicGeoposition()
                {
                    Latitude = busStopData[i].latitude,
                    Longitude = busStopData[i].longitude
                });
                //MapControl.SetLocation(tempButton, buttonPosition);
                */
                //add button to map
                //MapControl.SetNormalizedAnchorPoint(tempButton, new Point(0.5, 1.0));

                //add values to obj

                //add obj to list
                ///mapIconList.Add(tempMapIconObj);


                //define icon
                Geopoint iconPoint = new Geopoint(iconPosition);
                tempMapIcon.Location = iconPoint;
                tempMapIcon.NormalizedAnchorPoint = new Point(0.5, 1.0);
                tempMapIcon.ZIndex = 0;
                //tempMapIcon.Title = busStopData[i].stop_id.ToString();
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



            /*
            //find departure time data using stop_refs 
            for (int j=0; j<tempArray.Count; j++)
            { 
                departureTimeData = await GetDepartureTimes.API_Call(tempArray[j].ToString());
               textBoxTest2.Text += " " + departureTimeData.stop.stop_ref;
               // textBoxTest2.Text += " " + departureTimeData.times[j].depart_timestamp; //++index out of bounds
            }
          */

            return busStopData;

        }




        private void test1_Click(object sender, RoutedEventArgs e)
        {

            //BusStops[] test = await GetBusStops.API_Call();
            // textBoxTest1.Text = test[0].stop_ref.ToString();

            //var data = GetBusStops.GetBusStopData();
            //textBoxTest1.Text = data.ToString();
        }


        private void mapIcon_Click(MapControl sender, MapElementClickEventArgs args)
        {

            //find which icon element has been clicked 
            MapIcon clickedIcon = args.MapElements.FirstOrDefault(x => x is MapIcon) as MapIcon;
            //Debug.WriteLine("Icon clicked with ID: " + clickedIcon.Title); //test 
            Debug.WriteLine("Icon clicked with Long Name: " + clickedIcon.Title); //test 

            if (clickedIcon.Title == "You are here")
            {
                showMessage(clickedIcon.Title);
            }
            else
            {

                string stop_ref = "";

                //loop through busStopData
                for (int i = 0; i < busStopData.Length; i++)
                {
                    //find stop_ref
                    if (clickedIcon.Title == busStopData[i].long_name)
                    {
                        stop_ref = busStopData[i].stop_ref;
                        break;
                    }
                }


                //makeDepartureTimes(clickedIcon.Title, stop_ref);
                //makeDepartureTimes(clickedIcon.Title);

                //showDepartureTimes(clickedIcon.Title); ///test
                makeDepartureTimes(stop_ref);

            }



        }

        //private async void makeDepartureTimes(string stopName, string stop_ref)
        private async void makeDepartureTimes(string stop_ref)
        {

            DepartureTimes departureTimeData = await GetDepartureTimes.API_Call(stop_ref);

           string message = string.Format("Route" + " " + "Destination" + " " + "Time" + "\n\n");
           

            //loop through list of departure times
            for (int i = 0; i < departureTimeData.times.Count; i++)
            {

                message += departureTimeData.times[i].depart_timestamp + "\n\n";
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
