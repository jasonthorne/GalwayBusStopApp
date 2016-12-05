using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace MobileAppProj
{
    public class GetRouteStops
    {
        //make call to API to get list of route stops
        public static async Task<RouteStops> API_Call(string timetable_id)
        {

            string url = String.Format("http://galwaybus.herokuapp.com/routes/{0}.json", timetable_id); //make url

            HttpClient http = new HttpClient(); //set up client

            var response = await http.GetAsync(url); //get response

            var jsonMsg = await response.Content.ReadAsStringAsync(); //read in string

            var serializer = new DataContractJsonSerializer(typeof(RouteStops)); //deserialize json into c# class

            var memStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonMsg)); //memory stream of json bytes for deserializer
            var result = (RouteStops)serializer.ReadObject(memStream);

            return result; //return result object

        }

    }


    //C# class from Json 
    [DataContract]
    public class Route
    {
        [DataMember]
        public int timetable_id { get; set; }
        [DataMember]
        public string long_name { get; set; }
        [DataMember]
        public string short_name { get; set; }
    }


    [DataContract]
    public class Stops
    {
        [DataMember]
        public string long_name { get; set; }
        [DataMember]
        public int stop_id { get; set; }
        [DataMember]
        public string stop_ref { get; set; }
        [DataMember]
        public string irish_long_name { get; set; }
        [DataMember]
        public double latitude { get; set; }
        [DataMember]
        public double longitude { get; set; }
    }

    [DataContract]
    public class RouteStops
    {
        [DataMember]
        public Route route { get; set; }
        [DataMember]
        public List<List<Stops>> stops { get; set; } //Close!! This splits the lis into two lists!
        //public List<Stops> stops { get; set; } //Close!! THis splits the lis into two lists!
    }


}





