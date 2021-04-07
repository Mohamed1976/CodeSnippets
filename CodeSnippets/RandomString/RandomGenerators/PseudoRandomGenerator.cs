using System;
using System.Collections.Generic;
using System.Text;

namespace RandomString.RandomGenerators
{
    //internal 
    public class PseudoRandomGenerator : IRandomGenerator
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

        public PseudoRandomGenerator(int seed)
        {
            Console.WriteLine($"Constructor is called PseudoRandomGenerator(int seed): {seed}");
            _pseudoRandomGenerator = new Random(seed);
        }

        #endregion

        #region [ IRandomGenerator Methods ]

        public int Next()
        {
            return _pseudoRandomGenerator.Next();
        }

        public int Next(int maxValue)
        {
            if (maxValue < 0)
                throw new ArgumentOutOfRangeException("maxValue", "'maxValue' must be greater than zero.");

            return _pseudoRandomGenerator.Next(maxValue);
        }

        public int Next(int minValue, int maxValue)
        {
            if (minValue < 0)
                throw new ArgumentOutOfRangeException("minValue", "'minValue' must be greater than zero.");

            if (maxValue < 0)
                throw new ArgumentOutOfRangeException("maxValue", "'maxValue' must be greater than zero.");

            if (minValue > maxValue)
                throw new ArgumentOutOfRangeException("minValue", "'minValue' cannot be greater than maxValue.");

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
