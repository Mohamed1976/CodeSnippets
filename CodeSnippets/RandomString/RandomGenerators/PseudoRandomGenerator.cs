using System;
using System.Collections.Generic;
using System.Text;

namespace RandomString.RandomGenerators
{
    internal class PseudoRandomGenerator : IRandomGenerator
    {
        #region [ Fields ]

        private readonly Random _pseudoRandomGenerator = null;
        private bool disposedValue = false;

        #endregion

        #region [ Constructor ]

        public PseudoRandomGenerator()
        {
            _pseudoRandomGenerator = new Random();
        }

        #endregion

        #region [ IRandomGenerator Methods ]

        public int Next()
        {
            return _pseudoRandomGenerator.Next();
        }

        public int Next(int maxValue)
        {
            return _pseudoRandomGenerator.Next(maxValue);
        }

        public int Next(int minValue, int maxValue)
        {
            return _pseudoRandomGenerator.Next(minValue, maxValue);
        }

        #endregion

        #region[ IDisposable ]

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects)
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
