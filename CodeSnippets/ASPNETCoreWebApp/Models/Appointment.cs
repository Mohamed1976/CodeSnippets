using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//There are other ways to bind a browser request to an object. 
//DefaultModelBinder Maps a browser request to a standard data object
//LinqBinaryModelBinder Maps a browser request to a Language-Integrated Query(LINQ) object
//ModelBinderAttribute An attribute that associates a model type to a model-builder type
//ModelBinderDictionary Represents a class that contains all model binders for the application, listed by binder type

namespace ASPNETCoreWebApp.Models
{
    public class Appointment
    {
        public string Id { get; set; }
        public string Name { get; set; }

        // No need to do this when registering the model binder for all DateTime properties
        //[ModelBinder(BinderType = typeof(SplitDateTimeModelBinder))]
        public DateTime AppointmentDate { get; set; }
    }
}
