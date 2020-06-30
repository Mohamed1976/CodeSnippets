using ASPNETCoreWebApp.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Helpers
{
    public class ReservationModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            ValueProviderResult raw = bindingContext.ValueProvider.GetValue("loc");
            dynamic data = raw.FirstValue.Split(",");
            var result = new ReservationLocation()
            {
                City = data[0],
                State = data[1]
            };

            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }
    }
}
