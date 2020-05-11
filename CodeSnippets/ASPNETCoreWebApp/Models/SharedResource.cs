using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//https://andrewlock.net/adding-localisation-to-an-asp-net-core-application/
namespace ASPNETCoreWebApp.Models
{
    public class SharedResource
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public SharedResource(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        public string GetLocalizedString()
        {
            return _localizer["My localized string"];
        }
    }
}
