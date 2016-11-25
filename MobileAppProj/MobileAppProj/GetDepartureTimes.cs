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
    class GetDepartureTimes
    {
        
        //make call to API to get departure times
        public static async Task<DepartureTimes[]> API_Call(string stop_ref)
        {

            string url = String.Format("http://galwaybus.herokuapp.com/{0}.json", stop_ref); //make url

            HttpClient http = new HttpClient(); //set up client

            var response = await http.GetAsync(url); //get response

            var jsonMsg = await response.Content.ReadAsStringAsync(); //read in string

            var serializer = new DataContractJsonSerializer(typeof(DepartureTimes[])); //deserialize json into c# class

            var memStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonMsg)); //memory stream of json bytes for deserializer
            var result = (DepartureTimes[])serializer.ReadObject(memStream);

            return result; //return result object

        }

    }



    //C# class from Json 
    [DataContract]
    public class Stop
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
    public class Time
    {
        [DataMember]
        public string timetable_id { get; set; }
        [DataMember]
        public string display_name { get; set; }
        [DataMember]
        public string depart_timestamp { get; set; }
        [DataMember]
        public bool low_floor { get; set; }
    }

    [DataContract]
    public class DepartureTimes
    {
        [DataMember]
        public Stop stop { get; set; }
        [DataMember]
        public List<Time> times { get; set; }
    }



}

