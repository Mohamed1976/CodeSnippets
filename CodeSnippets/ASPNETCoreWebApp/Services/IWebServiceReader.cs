using ASPNETCoreWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Services
{
    public interface IWebServiceReader
    {
        Task<ImageOfDay> GetImageOfDayAsync();
    }
}
