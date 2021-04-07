using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomStringGeneratorLibrary.RandomNumberGenerators
{
    public interface IRandomNumberGenerator : IDisposable
    {
        int Next();
        int Next(int toExclusive);
        int Next(int fromInclusive, int toExclusive);
    }
}
