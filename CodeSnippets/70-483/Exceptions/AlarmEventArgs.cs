using System;
using System.Collections.Generic;
using System.Text;

namespace _70_483.Exceptions
{
    class AlarmEventArgs : EventArgs
    {
        public AlarmEventArgs(string msg)
        {
            MyMessage = msg;
        }

        private string myMessage;

        public string MyMessage
        {
            get { return myMessage; }
            private set { myMessage = value; }
        }

    }
}
