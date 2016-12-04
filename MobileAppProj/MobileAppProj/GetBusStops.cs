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
    public class GetBusStops
    {

        //link-http://galwaybus.herokuapp.com/stops.json

        //make call to API to get bus stop data.
        public static async Task<BusStops[]> API_Call()
        {

            string url = String.Format("http://galwaybus.herokuapp.com/stops.json"); //make url

            HttpClient http = new HttpClient(); //set up client

            var response = await http.GetAsync(url); //get response

            var jsonMsg = await response.Content.ReadAsStringAsync(); //read in string

            var serializer = new DataContractJsonSerializer(typeof(BusStops[])); //deserialize json into c# class

            var memStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonMsg)); //memory stream of json bytes for deserializer
            var result = (BusStops[])serializer.ReadObject(memStream);

            return result; //return result object
            
        }

    }



    //C# class from Json 
    [DataContract]
    public class BusStops
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


}
