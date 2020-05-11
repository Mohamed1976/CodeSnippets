using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Models
{
    /* If you go to this website: https://api.nasa.gov/planetary/apod?api_key=DEMO_KEY&date=2020-02-15 
     * You will get the the JSON respons shown below. When you enter this JSON respons in the website     
     * http://json2csharp.com/, it generates the ImageOfDay class that corresponds to the JSON respons shown below.
     * 
     * {"date":"2020-02-15","explanation":"A jewel of the southern sky, the Great Carina Nebula, also known as NGC 3372, 
     * spans over 300 light-years, one of our galaxy's largest star forming regions. Like the smaller, more northerly 
     * Great Orion Nebula, the Carina Nebula is easily visible to the unaided eye, though at a distance of 7,500 light-years 
     * it is some 5 times farther away. This gorgeous telescopic close-up reveals remarkable details of the region's 
     * central glowing filaments of interstellar gas and obscuring cosmic dust clouds in a field of view nearly 20 
     * light-years across. The Carina Nebula is home to young, extremely massive stars, including the still enigmatic 
     * and violently variable Eta Carinae, a star system with well over 100 times the mass of the Sun. 
     * In the processed composite of space and ground-based image data a dusty, two-lobed Homunculus Nebula 
     * appears to surround Eta Carinae itself just below and left of center. While Eta Carinae is likely on the 
     * verge of a supernova explosion, X-ray images indicate that the Great Carina Nebula has been a veritable 
     * supernova factory.","hdurl":"https://apod.nasa.gov/apod/image/2002/Eta-HST-ESO-New-LL.jpg","media_type":"image",
     * "service_version":"v1","title":"Carina Nebula Close Up","url":"https://apod.nasa.gov/apod/image/2002/Eta-HST-ESO-New-LL1024.jpg"}
    */
    public class ImageOfDay
    {
        public string date { get; set; }
        public string explanation { get; set; }
        public string hdurl { get; set; }
        public string media_type { get; set; }
        public string service_version { get; set; }
        public string title { get; set; }
        public string url { get; set; }

        public override string ToString()
        {
            return string.Format("Date: {0}\nExplanation: {1}\nHdurl: {2}\nMedia_type: {3}\nService_version: {4}\n" +
                "Title: {5}\nUrl: {6}", date, explanation, hdurl, media_type, service_version, title, url);
        }
    }
}
