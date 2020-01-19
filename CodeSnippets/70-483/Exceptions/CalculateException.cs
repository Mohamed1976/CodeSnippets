using System;
using System.Collections.Generic;
using System.Text;

namespace _70_483.Exceptions
{
    class CalculateException : Exception
    {
        public enum CalcErrorCodes
        {
            InvalidNumberText,
            DivideByZero
        }
        public CalcErrorCodes Error { get; private set; }
        public CalculateException(string message, CalcErrorCodes error) : base(message)
        {
            Error = error;
        }

    }
}
