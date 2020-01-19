using System;
using System.Collections.Generic;
using System.Text;

namespace _70_483.EventsAndCallbacks
{
    class HockeyTeam
    {
        private string _name;
        private int _founded;

        public HockeyTeam(string name, int year)
        {
            _name = name;
            _founded = year;
        }

        public string Name
        {
            get { return _name; }
        }

        public int Founded
        {
            get { return _founded; }
        }
    }
}
