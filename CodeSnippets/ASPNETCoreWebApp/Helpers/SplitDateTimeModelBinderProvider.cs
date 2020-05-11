using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ASPNETCoreWebApp.Helpers
{
    public class SplitDateTimeModelBinderProvider : IModelBinderProvider
    {
        //private readonly IModelBinder binder =
        //    new SplitDateTimeModelBinder(
        //        new SimpleTypeModelBinder(typeof(DateTime)));

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-3.1
            var loggerFactory = context.Services.GetRequiredService<ILoggerFactory>();
            IModelBinder binder = new SplitDateTimeModelBinder(
                new SimpleTypeModelBinder(typeof(DateTime), loggerFactory));

            return context.Metadata.ModelType == typeof(DateTime) ? binder : null;
        }
    }
}
