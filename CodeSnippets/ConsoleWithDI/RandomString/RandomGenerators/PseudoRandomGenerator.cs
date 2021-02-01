using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleWithDI.RandomString.RandomGenerators
{

    /// <summary>
    /// A random string generator that uses the <see cref="Random"/> class to create a random string.
    /// This is a fast generator but should not be used to generate passwords.
    /// Use <see cref="CryptographicRandomStringGenerator"/> for that purpose.
    /// </summary>
    /// 
    /// <summary>
    /// Gets a singleton instance that uses the <see cref="Random"/> class to create a random string.
    /// This is a fast generator but should not be used to generate passwords.
    /// Use <see cref="CryptographicRandom"/> for that purpose.
    /// </summary>
    public class PseudoRandomGenerator
    {

        private readonly Random random;

        /// <summary>
        /// Creates new instance of the <see cref="PseudoRandomStringGenerator"/> class.
        /// </summary>
        public PseudoRandomGenerator()
        {
            this.random = new Random();
        }

        public int GetNextRandomNumber(int maxValue)
        {
            return this.random.Next(maxValue);
        }

    }
}
