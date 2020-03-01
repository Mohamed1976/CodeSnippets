using System;
using System.Collections.Generic;
using System.Text;

namespace _70_483.GarbageCollector
{
    public class DerivedFromAbstractClass : AbstractClass
    {
        public DerivedFromAbstractClass(string name) : base(name)
        {
            Console.WriteLine("DerivedFromAbstractClass(string name): " + name);
        }

        ~DerivedFromAbstractClass()
        {
            Dispose(false);
        }

        private bool disposedValue = false; // To detect redundant calls

        protected override void Dispose(bool disposing)
        {
            if(!disposedValue)
            {
                if (disposing)
                {
                    Console.WriteLine("DerivedFromAbstractClass.protected virtual void Dispose(bool disposing), managed objects");
                    // Dispose managed resources.
                }
                Console.WriteLine("DerivedFromAbstractClass.protected virtual void Dispose(bool disposing), unmanaged objects");
                // Dispose unmanaged resources here.
                disposedValue = true;
            }

            base.Dispose(disposing);
        }
    }
}
