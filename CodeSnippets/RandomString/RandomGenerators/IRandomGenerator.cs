using System;
using System.Collections.Generic;
using System.Text;

namespace RandomString.RandomGenerators
{
    //internal 
    public interface IRandomGenerator: IDisposable
    {
        int Next();
        int Next(int maxValue);
        int Next(int minValue, int maxValue);
    }
}
