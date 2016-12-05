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
using Windows.UI;
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
        private RouteStops routeStopData;
        private bool madeRoute = false;
        List<RouteStops> routeStopsList = new List<RouteStops>();
        

        public MainPage()
        {
            this.InitializeComponent();
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {

            makeMap();

            progressRing.IsActive = true;
            await populateMap();
            progressRing.IsActive = false;

            makeRoutes();

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

                case GeolocationAccessStatus.Denied:

                //Ask user to change location settings, and exit program. 
                bool result = await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-location"));
                Application.Current.Exit();
                break;
            }
        }



        //populate map with bus stops
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

                //add icon to map
                MyMap.MapElements.Add(tempMapIcon);

            }

            return busStopData;

        }



        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++DELETE!!
        private void test1_Click(object sender, RoutedEventArgs e)
        {
            ///RoutesListBox.Items.Add("fff");

            //BusStops[] test = await GetBusStops.API_Call();
            // textBoxTest1.Text = test[0].stop_ref.ToString();

            //var data = GetBusStops.GetBusStopData();
            //textBoxTest1.Text = data.ToString();
        }
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++DELETE!!


        //listener for icon clicks
        private void mapIcon_Click(MapControl sender, MapElementClickEventArgs args)
        {

            //find which icon element has been clicked 
            MapIcon clickedIcon = args.MapElements.FirstOrDefault(x => x is MapIcon) as MapIcon;
            Debug.WriteLine("Icon clicked with Long Name: " + clickedIcon.Title); //test ================================REMOVE!! 

            string long_name = clickedIcon.Title;
           

            //if user hasnt clicked his location icon
            if (!long_name.Contains("You are here"))
            {

                //loop through busStopData
                for (int i = 0; i < busStopData.Length; i++)
                {
                    //find stop_ref
                    if (long_name == busStopData[i].long_name)
                    {
                        //stop_ref = busStopData[i].stop_ref;
                        makeDepartureTimes(busStopData[i].stop_ref);
                        break;
                    }  
                }

           } 
           else //user location was clicked
           {
                showMessage(clickedIcon.Title);       
           }

        }

        //find and format departure times
        private async void makeDepartureTimes(string stop_ref)
        {
            
            progressRing.IsActive = true; 
            DepartureTimes departureTimeData = await GetDepartureTimes.API_Call(stop_ref); 
            progressRing.IsActive = false; 

            DateTime nextBus;
            DateTime currentTime;
            TimeSpan timeUntillNextBus;

            string message = string.Format("Route" + " | " + "Destination" + " | " + "Time" + "\n\n");
            string hourValue = "";
            string minuteValue = "";
            bool showMinutes; 
            bool showHours; 
         

            //loop through list of departure times
            for (int i = 0; i < departureTimeData.times.Count; i++)
            {
                //find due time
                nextBus = DateTime.Parse(departureTimeData.times[i].depart_timestamp);
                currentTime = DateTime.UtcNow;
                timeUntillNextBus = nextBus.Subtract(currentTime);

                //(re)set bools
                showMinutes = true;
                showHours = true;


                //set minutes
                if (timeUntillNextBus.Minutes > 1)
                {
                      minuteValue = "minutes";
                }
                else if (timeUntillNextBus.Minutes == 1)
                {
                     minuteValue = "minute";
                }
                else if (timeUntillNextBus.Minutes == 0 || timeUntillNextBus.Minutes < 0)
                {
                    //dont print minutes
                    showMinutes = false;
                }

                //set hours
                if (timeUntillNextBus.Hours > 1)
                {
                    hourValue = "hours";
                    
                }
                else if (timeUntillNextBus.Hours == 1)
                {
                    hourValue = "hour";

                }
                else if (timeUntillNextBus.Hours == 0 || timeUntillNextBus.Hours < 0)
                {
                    //dont print hours 
                    showHours = false;
                }

               
                //append string 
                if (showHours && showMinutes)
                {
                    message += string.Format(departureTimeData.times[i].timetable_id + " | " + departureTimeData.times[i].display_name + " | " + timeUntillNextBus.Hours + " " + hourValue + " & " + timeUntillNextBus.Minutes + " " + minuteValue + " from now \n\n");
                }
                else if (showHours && !showMinutes)
                {
                    message += string.Format(departureTimeData.times[i].timetable_id + " | " + departureTimeData.times[i].display_name + " | " + timeUntillNextBus.Hours + " " + hourValue + " from now \n\n");
                }
                else if (!showHours && showMinutes)
                {
                    message += string.Format(departureTimeData.times[i].timetable_id + " | " + departureTimeData.times[i].display_name + " | " + timeUntillNextBus.Minutes + " " + minuteValue + " from now \n\n");
                }
                else if (!showHours && !showMinutes)
                {
                    message += string.Format(departureTimeData.times[i].timetable_id + " | " + departureTimeData.times[i].display_name + " | " + "due now\n\n");
                }

            }

            showMessage(message);

        }


        //show message to user
        private async void showMessage(string message)
        {
            MessageDialog dialog = new MessageDialog(message);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await dialog.ShowAsync());
        }


        //show routes to user
        private void ShowRoutesButton_Click(object sender, RoutedEventArgs e)
        {
            MySpiltView.IsPaneOpen = !MySpiltView.IsPaneOpen;
        }


        //list box event listener
        private void RoutesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            RouteStops foundRoute = new RouteStops();

            //loop through routeStops list
            for (int i = 0; i < routeStopsList.Count; i++)
            {

                //find selected route
                if ((string)RoutesListBox.SelectedItem == routeStopsList[i].route.long_name)
                {
                    Debug.WriteLine(routeStopsList[i].route.long_name); //////////////////////////REMOVE 
                    /////RoutesListBox.Items.Add("Delete route"); //add a delete route button
                    foundRoute = routeStopsList[i];
                    break;
                }

            }


            //pass route stops into makePolyLine
            makePolyLine(foundRoute);


            /*
            if ((string)RoutesListBox.SelectedItem == routeStopData.route.long_name)
            {
                Debug.WriteLine((string)RoutesListBox.SelectedItem); //+++++++++++++++++++++++++++++++++++++++++++++++++
                RoutesListBox.Items.Add("Delete route");

                makePolyLine();

            }
            */

            if ((string)RoutesListBox.SelectedItem == "Delete route")
            {
                //remove polyline
            }

        }


        //private async void makeRoutes()
        private void makeRoutes()
        {

            progressRing.IsActive = true;
            ///Routes routeData = await GetRoutes.API_Call(); ////NOT WORKING 
            progressRing.IsActive = false;


            /*This is a work around to the above API call not working.
             * Initial plan was to loop through routeData to extract timetable_ids, making routeStopData call at each ith position,
             * then add received long_name to listBox.*/

            addRoute(401);
            addRoute(402);
            addRoute(403);
            addRoute(404);
            addRoute(405);
            addRoute(407);
            addRoute(409);
              
        }


        //add route to listBox
        private async void addRoute(int timetable_id)
        {
            RouteStops tempRouteStopData = await GetRouteStops.API_Call(timetable_id.ToString());
            RoutesListBox.Items.Add(tempRouteStopData.route.long_name);
            routeStopsList.Add(tempRouteStopData);
        }


        private void makePolyLine(RouteStops routeStops)
        {

            
            for (int i = 0; i < routeStops.stops.Count; i++)
            {
               // Debug.Write();
            }

        }

    }

}
