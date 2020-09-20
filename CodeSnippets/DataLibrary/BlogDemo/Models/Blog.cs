using DataLibrary.BlogDemo.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

//Create simple Blog model 
//https://docs.microsoft.com/en-us/ef/core/?WT.mc_id+vstoolbox-c9-niner
namespace DataLibrary.BlogDemo.Models
{
    public class Blog : BaseEntity
    {
        private string _url;
        private string _name;

        //public int BlogId { get; set; }

        public string Url
        {
            get { return _url; }
            set
            {
                if (_url != value)
                {
                    _url = value;
                    PropertyChanged();
                }
            }
        }

        //this will be used by EF when constituting an instance
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    PropertyChanged();
                }
            }
        }

        public override string ToString()
        {
            return $"#Blog: {Name} {Url} {Id}";
        }


        [NotMapped]
        public bool IsDirty { get; set; } //= false; default value is already false
        public void PropertyChanged()
        {
            IsDirty = true;
            Console.WriteLine("Property Changed");
        }
    }
}
