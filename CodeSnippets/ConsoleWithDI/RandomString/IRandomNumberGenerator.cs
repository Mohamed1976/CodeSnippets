using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleWithDI.RandomString
{
    public interface  IRandomNumberGenerator
    {
        int GetNextRandomNumber();
        int GetNextRandomNumber(int maxValue);
        int GetNextRandomNumber(int minxValue, int maxValue);
    }
}
