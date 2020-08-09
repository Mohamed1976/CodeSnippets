using System;
using System.Collections.Generic;

namespace DataLibrary.Models
{
    public partial class Region
    {
        public Region()
        {
            Country = new HashSet<Country>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Country> Country { get; set; }
    }
}
