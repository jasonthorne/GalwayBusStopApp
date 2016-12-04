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
    public class GetRoutes
    {

        //make call to API to get routes data.
        public static async Task<Routes> API_Call()
        {

            string url = String.Format("http://galwaybus.herokuapp.com/routes.json"); //make url

            HttpClient http = new HttpClient(); //set up client

            var response = await http.GetAsync(url); //get response

            var jsonMsg = await response.Content.ReadAsStringAsync(); //read in string

            var serializer = new DataContractJsonSerializer(typeof(Routes)); //deserialize json into c# class

            var memStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonMsg)); //memory stream of json bytes for deserializer
            var result = (Routes)serializer.ReadObject(memStream);

            return result; //return result object

        }
    }



    //C# class from Json 
    [DataContract]
    public class Route_401
    {
        [DataMember]
        public int timetable_id { get; set; }
        [DataMember]
        public string long_name { get; set; }
        [DataMember]
        public string short_name { get; set; }
    }

    [DataContract]
    public class Route_402
    {
        [DataMember]
        public int timetable_id { get; set; }
        [DataMember]
        public string long_name { get; set; }
        [DataMember]
        public string short_name { get; set; }
    }

    [DataContract]
    public class Route_403
    {
        [DataMember]
        public int timetable_id { get; set; }
        [DataMember]
        public string long_name { get; set; }
        [DataMember]
        public string short_name { get; set; }
    }

    [DataContract]
    public class Route_404
    {
        [DataMember]
        public int timetable_id { get; set; }
        [DataMember]
        public string long_name { get; set; }
        [DataMember]
        public string short_name { get; set; }
    }

    [DataContract]
    public class Route_405
    {
        [DataMember]
        public int timetable_id { get; set; }
        [DataMember]
        public string long_name { get; set; }
        [DataMember]
        public string short_name { get; set; }
    }

    [DataContract]
    public class Route_407
    {
        [DataMember]
        public int timetable_id { get; set; }
        [DataMember]
        public string long_name { get; set; }
        [DataMember]
        public string short_name { get; set; }
    }

    [DataContract]
    public class Route_409
    {
        [DataMember]
        public int timetable_id { get; set; }
        [DataMember]
        public string long_name { get; set; }
        [DataMember]
        public string short_name { get; set; }
    }

    [DataContract]
    public class Route_410
    {
        [DataMember]
        public int timetable_id { get; set; }
        [DataMember]
        public string long_name { get; set; }
        [DataMember]
        public string short_name { get; set; }
    }

    [DataContract]
    public class Routes
    {
        [DataMember]
        public Route_401 Route_401 { get; set; }
        [DataMember]
        public Route_402 Route_402 { get; set; }
        [DataMember]
        public Route_403 Route_403 { get; set; }
        [DataMember]
        public Route_404 Route_404 { get; set; }
        [DataMember]
        public Route_405 Route_405 { get; set; }
        [DataMember]
        public Route_407 Route_407 { get; set; }
        [DataMember]
        public Route_409 Route_409 { get; set; }
        [DataMember]
        public Route_410 Route_410 { get; set; }
    }

}
