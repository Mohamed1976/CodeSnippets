using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

//https://khalidabuhakmeh.com/creating-random-numbers-with-dotnet-core
//https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.randomnumbergenerator.getint32?view=net-5.0#System_Security_Cryptography_RandomNumberGenerator_GetInt32_System_Int32_
//https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.randomnumbergenerator?view=net-5.0
namespace RandomStringGeneratorLibrary.RandomNumberGenerators
{
    public class CryptoRandomNumberGenerator : IRandomNumberGenerator
    {
        private readonly RandomNumberGenerator _randomNumberGenerator;

        public CryptoRandomNumberGenerator()
        {
            Console.WriteLine("ctor CryptoRandomNumberGenerator()");
            _randomNumberGenerator = RandomNumberGenerator.Create();
            //_randomNumberGenerator.GetBytes()
        }

        private bool disposedValue;

        public int Next()
        {
            return RandomNumberGenerator.GetInt32(int.MaxValue);
        }

        public int Next(int toExclusive)
        {
            return RandomNumberGenerator.GetInt32(toExclusive);
        }

        public int Next(int fromInclusive, int toExclusive)
        {
            return RandomNumberGenerator.GetInt32(fromInclusive, toExclusive);
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
        // ~CryptoRandomNumberGenerator()
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
