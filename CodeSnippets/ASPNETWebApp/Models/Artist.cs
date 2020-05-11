using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETWebApp.Models
{

    public class Artist
    {
        public Artist(string firstName, string lastName, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime BirthDate { get; private set; }
    }
}