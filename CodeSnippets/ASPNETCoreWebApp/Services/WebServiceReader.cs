using ASPNETCoreWebApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Services
{
    public class WebServiceReader : IWebServiceReader
    {
        public WebServiceReader()
        {
        }

        private const string ImageOfDayUrl = "https://api.nasa.gov/planetary/apod?api_key=DEMO_KEY";

        public async Task<ImageOfDay> GetImageOfDayAsync()
        {
            Debug.WriteLine("Entering: private async Task<ImageOfDay> GetImageOfDay(string imageURL)");
            Debug.Indent();
            WebClient webClient = new WebClient();
            string url = ImageOfDayUrl + "&date=" + DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd");
            Debug.WriteLine("Download url: " + url);
            string JSONContent = await webClient.DownloadStringTaskAsync(url);
            Debug.WriteLine("JSON response: " + JSONContent);
            ImageOfDay imageOfDay = JsonConvert.DeserializeObject<ImageOfDay>(JSONContent);
            Debug.Unindent();
            Debug.WriteLine("Exiting: private async Task<ImageOfDay> GetImageOfDay(string imageURL)");
            return imageOfDay;
        }
    }
}
