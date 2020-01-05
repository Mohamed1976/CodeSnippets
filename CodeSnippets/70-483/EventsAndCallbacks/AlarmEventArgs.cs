using System;
using System.Collections.Generic;
using System.Text;

namespace _70_483.EventsAndCallbacks
{
    class AlarmEventArgs
    {
        public DateTime CreationDate { get; private set; }
        public string Location { get; private set; }
        public AlarmEventArgs(string location, DateTime creationDate)
        {
            Location = location;
            CreationDate = creationDate;
        }
    }
}
