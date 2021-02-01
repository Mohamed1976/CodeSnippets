using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleWithDI.RandomString
{
    public class RandomGeneratorV2
    {
		/// <summary>
		/// Represents a pseudo-random number generator, a device that produces a sequence
		/// of numbers that meet certain statistical requirements for randomness.
		/// </summary>
		/// <remarks>
		/// Use the RNGCryptoServiceProvider class if you need a strong random number generator.
		/// </remarks>
		private readonly Random rand = null; //new Random();

        public RandomGeneratorV2()
        {
			rand = new Random();
		}


	}
}
