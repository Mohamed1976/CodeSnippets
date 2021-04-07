using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RandomStringGeneratorLibrary.RandomNumberGenerators
{
    public class PseudoRandomNumberGenerator : IRandomNumberGenerator
    {
        //static int SeedCount = 0;
        //static int GenerateSeed()
        //{
        //    return (int)((DateTime.Now.Ticks << 4) +
        //                   (Interlocked.Increment(ref SeedCount)));
        //}

        //private readonly ThreadLocal<Random> _pseudoRandomNumberGenerator;
        //ThreadLocal<PseudoRandomGenerator>

        private readonly Random _pseudoRandomNumberGenerator;
        public PseudoRandomNumberGenerator()
        {
            Console.WriteLine("##ctor PseudoRandomNumberGenerator()");
            _pseudoRandomNumberGenerator = new Random(Guid.NewGuid().GetHashCode());
            //_pseudoRandomNumberGenerator = new ThreadLocal<Random>(() =>
            //{
            //    return new Random(Guid.NewGuid().GetHashCode());
            //});

            //_pseudoRandomNumberGenerator = new Random(seed);
        }

        //public PseudoRandomNumberGenerator(int seed)
        //{
        //    _pseudoRandomNumberGenerator = new ThreadLocal<Random>(() =>
        //    {
        //        return new Random(Guid.NewGuid().GetHashCode());
        //    });

        //    //_pseudoRandomNumberGenerator = new Random(seed);
        //}

        private bool disposedValue;

        public int Next()
        {
            //Console.WriteLine("##ctor PseudoRandomNumberGenerator() Next() is called");
            return _pseudoRandomNumberGenerator.Next();
            //return _pseudoRandomNumberGenerator.Value.Next();
            //throw new NotImplementedException();
        }

        public int Next(int toExclusive)
        {
            return _pseudoRandomNumberGenerator.Next(toExclusive);
            //return _pseudoRandomNumberGenerator.Value.Next(toExclusive);
            //throw new NotImplementedException();
        }

        public int Next(int fromInclusive, int toExclusive)
        {
            return _pseudoRandomNumberGenerator.Next(fromInclusive, toExclusive);
            //return _pseudoRandomNumberGenerator.Value.Next(fromInclusive, toExclusive);
            //throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~PseudoRandomNumberGenerator()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
