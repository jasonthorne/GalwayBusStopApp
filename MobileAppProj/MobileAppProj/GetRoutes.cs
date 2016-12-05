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

    //adapted from: https://www.youtube.com/watch?v=rFyc9PSpPug&index=72&list=PLi2hbezQRVS0cPMeW3uDlUHnO_rPvJCV9

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
    public class _401
    {
        [DataMember]
        public int timetable_id { get; set; }
        [DataMember]
        public string long_name { get; set; }
        [DataMember]
        public string short_name { get; set; }
    }


    [DataContract]
    public class _402
    {
        [DataMember]
        public int timetable_id { get; set; }
        [DataMember]
        public string long_name { get; set; }
        [DataMember]
        public string short_name { get; set; }
    }

    [DataContract]
    public class _403
    {
        [DataMember]
        public int timetable_id { get; set; }
        [DataMember]
        public string long_name { get; set; }
        [DataMember]
        public string short_name { get; set; }
    }

    [DataContract]
    public class _404
    {
        [DataMember]
        public int timetable_id { get; set; }
        [DataMember]
        public string long_name { get; set; }
        [DataMember]
        public string short_name { get; set; }
    }

    [DataContract]
    public class _405
    {
        [DataMember]
        public int timetable_id { get; set; }
        [DataMember]
        public string long_name { get; set; }
        [DataMember]
        public string short_name { get; set; }
    }

    [DataContract]
    public class _407
    {
        [DataMember]
        public int timetable_id { get; set; }
        [DataMember]
        public string long_name { get; set; }
        [DataMember]
        public string short_name { get; set; }
    }

    [DataContract]
    public class _409
    {
        [DataMember]
        public int timetable_id { get; set; }
        [DataMember]
        public string long_name { get; set; }
        [DataMember]
        public string short_name { get; set; }
    }

    [DataContract]
    public class _410
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
        public _401 _401 { get; set; }
        [DataMember]
        public _402 _402 { get; set; }
        [DataMember]
        public _403 _403 { get; set; }
        [DataMember]
        public _404 _404 { get; set; }
        [DataMember]
        public _405 _405 { get; set; }
        [DataMember]
        public _407 _407 { get; set; }
        [DataMember]
        public _409 _409 { get; set; }
        [DataMember]
        public _410 _410 { get; set; }   
          
    }
      
}

