using RandomStringGeneratorLibrary.RandomNumberGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomStringGeneratorLibrary
{
    public class RandomStringGenerator
    {
        private readonly IRandomNumberGenerator _randomNumberGenerator = null;
        public RandomStringGenerator()
        {
            //_randomNumberGenerator = new CryptoRandomNumberGenerator();
            _randomNumberGenerator = new PseudoRandomNumberGenerator();
        }

        public int Next()
        {
            return _randomNumberGenerator.Next();
            //throw new NotImplementedException();
        }

        public int Next(int fromInclusive, int toExclusive)
        {
            return _randomNumberGenerator.Next(fromInclusive, toExclusive);
            //throw new NotImplementedException();
        }
    }
}
