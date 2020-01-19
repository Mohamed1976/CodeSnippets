using System;
using System.Collections.Generic;
using System.Text;

namespace _70_483.GarbageCollector
{
    class DerivedDerivedClass : DerivedClass
    {
        public DerivedDerivedClass()
        {

        }

        ~DerivedDerivedClass()
        {
            Dispose(false);
        }

        private bool disposedValue = false; // To detect redundant calls
        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                //Release managed resources 
                if (disposing)
                {
                    Console.WriteLine("DerivedDerivedClass: Disposing managed resources.");
                }

                //Release unmanaged resources
                Console.WriteLine("DerivedDerivedClass: Disposing unmanaged resources.");
                disposedValue = true;
            }

            base.Dispose(disposing);
        }
    }
}
