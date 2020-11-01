using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

//https://github.com/dienomb/RandomString/blob/master/src/RandomEx.cs
//https://github.com/dmarciano/RandomStringGenerator/blob/master/src/RSBLib/Random%20Generators/CryptoRandomGenerator.cs
namespace ConsoleWithDI.RandomString
{
    public class RandomGenerator
    {
        private readonly RandomNumberGenerator _randomNumberGenerator = null;
        // Create a byte array to hold the random value.
        private readonly byte[] _uintBuffer = null;

        public RandomGenerator()
        {
            _randomNumberGenerator = RandomNumberGenerator.Create();
            _uintBuffer = new byte[4];
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

        public int Next(int minValue, int maxValue)
        {
            int randomVal = default; 

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
    }
}
