using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Generates random non-negative random integer.
/// </summary>
/// <param name="random">The source of random numbers.</param>
/// <returns>A 32-bit signed integer that is in range [0, <see cref="int.MaxValue"/>].</returns>

//https://github.com/dienomb/RandomString/blob/master/src/RandomEx.cs
//https://github.com/dmarciano/RandomStringGenerator/blob/master/src/RSBLib/Random%20Generators/CryptoRandomGenerator.cs
namespace ConsoleWithDI.RandomString
{
    public class RandomGenerator
    {
        private readonly RandomNumberGenerator _randomNumberGenerator = null;
        // Create a byte array to hold the random value.
        private readonly byte[] _uintBuffer = null;
        private bool disposed = false;

        public RandomGenerator()
        {
            _randomNumberGenerator = RandomNumberGenerator.Create();
            _uintBuffer = new byte[sizeof(int)];
        }

        public int Next()
        {
            return Next(0, int.MaxValue);
        }

        public int Next(int maxValue)
        {
            if (maxValue < 0)
                throw new ArgumentOutOfRangeException("maxValue");

            return Next(0, maxValue);
        }
        /*
            Note however that this has a flaw, 62 valid characters is equal to 5,9541963103868752088061235991756 bits (log(62) / log(2)), so it won't divide evenly on a 32 bit number (uint).
            What consequences does this have? As a result, the random output won't be uniform. Characters which are lower in value will occur more likely (just by a small fraction, but still it happens).
            To be more precise, the first 4 characters of a valid array are 0,00000144354999199840239435286 % more likely to occur.
        */
        // returns a random number in the range [minValue, maxValue)
        public int Next(int minValue, int maxValue)
        {
            int randomVal = -1; 

            if(minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException();
            }
            else if(maxValue > minValue)
            {
                long diff = maxValue - minValue;

                do
                {
                    _randomNumberGenerator.GetBytes(_uintBuffer);
                    uint rand = BitConverter.ToUInt32(_uintBuffer, 0);
                    long max = 1 + (long)uint.MaxValue;
                    long remainder = max % diff;
                    if (rand < max - remainder)
                    {
                        randomVal = (int)(minValue + (rand % diff));
                    }
                } while (randomVal == -1) ;
            }
            else /* maxValue == minValue */
            {
                randomVal = minValue;
            }

            return randomVal;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (_randomNumberGenerator != null)
                {
                    _randomNumberGenerator.Dispose();
                }

                disposed = true;
            }            
        }
    }
}
