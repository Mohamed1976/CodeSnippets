﻿using System;
using System.Collections.Generic;
using System.Text;

namespace _70_483.GarbageCollector
{
    public abstract class AbstractClass : IDisposable
    {
        protected AbstractClass(string name)
        {
            Console.WriteLine("protected AbstractClass(string name): " + name);
        }

        #region [ IDisposable Support ]
        
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Console.WriteLine("AbstractClass.protected virtual void Dispose(bool disposing), managed objects");
                    // TODO: dispose managed state (managed objects).
                }

                Console.WriteLine("AbstractClass.protected virtual void Dispose(bool disposing), unmanaged objects");
                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~AbstractClass()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
