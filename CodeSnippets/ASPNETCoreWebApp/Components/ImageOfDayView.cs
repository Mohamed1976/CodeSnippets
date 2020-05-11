using ASPNETCoreWebApp.Models;
using ASPNETCoreWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Components
{
    public class ImageOfDayView : ViewComponent
    {
        private readonly IWebServiceReader webServiceReader;
        private readonly ILogger<ImageOfDayView> logger;

        public ImageOfDayView(IWebServiceReader webServiceReader, 
            ILogger<ImageOfDayView> logger)
        {
            this.webServiceReader = webServiceReader;
            this.logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ImageOfDay imageOfDay = await webServiceReader.GetImageOfDayAsync();
            logger.LogInformation(imageOfDay.ToString());
            return View(imageOfDay);
        }
    }
}
