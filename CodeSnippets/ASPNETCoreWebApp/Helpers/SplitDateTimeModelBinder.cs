using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

//Customized model binders can be very useful when you are dealing with disconnects 
//between the items that are displayed on a user interface and their real type.
//It can also handle situations when you want to change the names of items in a form, 
//such as when a form submission might come from a different site and their form names 
//do not match your model names.You don't ever have to change your data structure 
//just to support your UI needs; instead, write a model binder between the two.

//There are other ways to bind a browser request to an object. Some additional model
//binders are listed below.
//1) DefaultModelBinder: Maps a browser request to a standard data object
//2) LinqBinaryModelBinder: Maps a browser request to a Language-Integrated Query(LINQ) object
//3) ModelBinderAttribute:  An attribute that associates a model type to a model-builder type
//4) ModelBinderDictionary: Represents a class that contains

//One of the biggest advantages of using custom model binding is the potential for reuse.
//For example, suppose that you are working on a human resources application. There are 
//multiple online forms in which users enter personal information, such as birthday, 
//health insurance, dental insurance, and so on.Each area of the application that 
//needs a date has three entry boxes for the date value: month, day, and year.
//Traditional mapping returns those three values as discrete model properties. 
//Somewhere in your code, you have to parse them into a DateTime object. 
//You could use a helper method to return a DateTime based on the three objects, 
//but wouldn’t it be simpler if that were already done for you by the time the 
//data got back to the server? Especially if it was already available for the 
//next form that you have to create? That is one of the benefits of custom model binders.

//Model binders: These provide a mechanism for tokenizing data in an HTTP request
//and converting it into a Common Language Runtime(CLR) type.For example, you
//may have a Controller action that expects a Person object and a View that shows
//a form for entering the properties of that object. A model binder automatically
//transforms the form data into a Person object. Without the model binder, you would
//be responsible for creating an instance of a Person object and then pulling the
//name-value pairs from the Request object and writing the data to the appropriate
//properties of your object.

//As you can see, customized model binders can be very useful when you are dealing with 
//disconnects between the items that are displayed on a user interface and their real type.
//It can also handle situations when you want to change the names of items in a form, such 
//as when a form submission might come from a different site and their form names do not 
//match your model names.You don't ever have to change your data structure just to support 
//your UI needs; instead, write a model binder between the two.

//https://www.dotnetcurry.com/aspnet-mvc/1368/aspnet-core-mvc-custom-model-binding
//https://github.com/DaniJG/AspCoreCustomModelBinder/blob/master/ASPCoreCustomModelBinder/ModelBinding/SplitDateTimeModelBinder.cs
//https://metanit.com/sharp/aspnet5/8.6.php
namespace ASPNETCoreWebApp.Helpers
{
    //Class needs to implement the System.Web.Mvc.IModelBinder interface,
    //which has the single BindModel method. In that method, you create the 
    //object, manage the binding, and return the object after binding is completed.
    public class SplitDateTimeModelBinder : IModelBinder
    {
        private readonly IModelBinder fallbackBinder;
        public SplitDateTimeModelBinder(IModelBinder fallbackBinder)
        {
            this.fallbackBinder = fallbackBinder;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            Debug.WriteLine("BindModelAsync(ModelBindingContext bindingContext)");
            // Make sure both Date and Time values are found in the Value Providers
            // NOTE: You might not want to enforce both parts
            var datePartName = $"{bindingContext.ModelName}.Date";
            var timePartName = $"{bindingContext.ModelName}.Time";
            Debug.WriteLine($"datePartName: {datePartName}, timePartName: {timePartName}");
            var datePartValues = bindingContext.ValueProvider.GetValue(datePartName);
            var timePartValues = bindingContext.ValueProvider.GetValue(timePartName);
            Debug.WriteLine($"timePartValues: {timePartValues.FirstValue}, " +
                $"datePartValues: {datePartValues.FirstValue}");

            Debug.WriteLine($"datePartValues.Length: {datePartValues.Length}, " +
                $"timePartValues.Length: {timePartValues.Length}");
            // Fallback to the default binder when a part is missing
            if (datePartValues.Length == 0 || timePartValues.Length == 0)
            {
                return fallbackBinder.BindModelAsync(bindingContext);
                //return Task.CompletedTask;
            }

            // When not wrapping the SimpleTypeModelBinder, you want to return an empty result here
            //if (datePartValues.Length == 0 || timePartValues.Length == 0) return Task.CompletedTask;

            // Parse Date and Time
            // TODO: You might want a stronger/smarter handling of locales, formats and cultures
            //DateTime.TryParseExact(
            //    datePartValues.FirstValue,
            //    "d",
            //    CultureInfo.InvariantCulture,
            //    DateTimeStyles.None,
            //    out var parsedDateValue);

            //DateTime.TryParseExact(
            //    timePartValues.FirstValue,
            //    "t",
            //    CultureInfo.InvariantCulture,
            //    DateTimeStyles.AdjustToUniversal,
            //    out var parsedTimeValue);

            bool validDate = DateTime.TryParse(
                datePartValues.FirstValue,
                CultureInfo.CurrentCulture, 
                DateTimeStyles.AssumeLocal, //| DateTimeStyles.AdjustToUniversal
                out var parsedDateValue);

            Debug.WriteLine($"validDate: {validDate}, parsedDateValue: {parsedDateValue}");

            validDate = DateTime.TryParse(
                timePartValues.FirstValue,
                CultureInfo.CurrentCulture,
                DateTimeStyles.AssumeLocal, //| DateTimeStyles.AdjustToUniversal
                out var parsedTimeValue);

            Debug.WriteLine($"validDate: {validDate}, parsedDateValue: {parsedTimeValue}");

            // Combine into single DateTime which is the end result
            var result = new DateTime(parsedDateValue.Year,
                            parsedDateValue.Month,
                            parsedDateValue.Day,
                            parsedTimeValue.Hour,
                            parsedTimeValue.Minute,
                            parsedTimeValue.Second);

            Debug.WriteLine($"Resulting DateTime: {result}");
            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, result, $"{datePartValues.FirstValue} {timePartValues.FirstValue}");
            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }
    }
}
