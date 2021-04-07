using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace RandomString.RandomGenerators
{
    //internal 
    public class CryptoRandomGenerator : IRandomGenerator
    {
        #region [ Fields ]

        private readonly RandomNumberGenerator _cryptoRandomGenerator = null;
        private readonly byte[] _uintBuffer = null;
        private bool disposedValue = false;

        #endregion

        #region [ Constructor ]

        public CryptoRandomGenerator()
        {
            _cryptoRandomGenerator = RandomNumberGenerator.Create();
            _uintBuffer = new byte[sizeof(uint)];
            Console.WriteLine("Constructor CryptoRandomGenerator");
        }

        #endregion

        #region [ IRandomGenerator Methods ]

        public int Next()
        {
            return Next(0, int.MaxValue);
        }

        public int Next(int maxValue)
        {
            if (maxValue < 0)
                throw new ArgumentOutOfRangeException("maxValue", "'maxValue' must be greater than zero.");

            return Next(0, maxValue);
        }

        public int Next(int minValue, int maxValue)
        {
            int randomVal = -1;

            if (minValue < 0)
                throw new ArgumentOutOfRangeException("minValue", "'minValue' must be greater than zero.");

            if (maxValue < 0)
                throw new ArgumentOutOfRangeException("maxValue", "'maxValue' must be greater than zero.");

            if(minValue > maxValue)
                throw new ArgumentOutOfRangeException("minValue", "'minValue' cannot be greater than maxValue.");
            
            if (maxValue > minValue)
            {
                long diff = maxValue - minValue;

                do
                {
                    _cryptoRandomGenerator.GetBytes(_uintBuffer);
                    uint rand = BitConverter.ToUInt32(_uintBuffer, 0);
                    long max = 1 + (long)uint.MaxValue;
                    long remainder = max % diff;
                    if (rand < max - remainder)
                    {
                        randomVal = (int)(minValue + (rand % diff));
                    }
                } while (randomVal == -1);
            }
            else /* else if maxValue == minValue */
            {
                randomVal = minValue;
            }

            return randomVal;
        }

        #endregion

        #region[ IDisposable ]

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_cryptoRandomGenerator != null)
                    {
                        _cryptoRandomGenerator.Dispose();
                    }
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
