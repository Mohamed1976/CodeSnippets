using RandomString.RandomGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace RandomStringTests
{
    public class RandomNumberGeneratorValidationTestCases
    {
        private readonly IRandomGenerator _pseudoRandomGenerator = null;
        private readonly IRandomGenerator _cryptoRandomGenerator = null;

        public RandomNumberGeneratorValidationTestCases()
        {
            _pseudoRandomGenerator = new PseudoRandomGenerator();
            _cryptoRandomGenerator = new CryptoRandomGenerator();
        }
    }
}
