using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RandomString.RandomGenerators
{
    public class PseudoRandomGeneratorV2 : IRandomGenerator
    {
        private static int seed = Environment.TickCount;
        private readonly Random _pseudoRandomGenerator = null;


        public PseudoRandomGeneratorV2()
        {
            seed = Interlocked.Increment(ref seed);
            Console.WriteLine($"Constructor PseudoRandomGeneratorV2() called, seed: {seed}");
            _pseudoRandomGenerator = new Random(seed);
        }

        static PseudoRandomGeneratorV2()
        {
            seed = Environment.TickCount;
            Console.WriteLine($"Static constructor PseudoRandomGeneratorV2() called, seed: {seed}");
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int Next()
        {
            return _pseudoRandomGenerator.Next();
            //throw new NotImplementedException();
        }

        public int Next(int maxValue)
        {
            throw new NotImplementedException();
        }

        public int Next(int minValue, int maxValue)
        {
            throw new NotImplementedException();
        }
    }
}
