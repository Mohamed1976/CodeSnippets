using System;

namespace ConsoleWithDI
{
    [Flags]
    public enum CharSet : byte
    {
        None = 0x00,
        LowerCase = 0x01,
        UpperCase = 0x02,
        Digits = 0x04,
        Special = 0x08,
        All = LowerCase | UpperCase | Digits | Special
        //All = 0xFF
    }

    public enum CharType
    {
        LowerCase = 0,
        UpperCase = 1,
        Numeric = 2,
        Special = 3
    }

    public interface IRandomStringService
    {
        string GenerateRandomString(int stringSize = 20, CharSet charSet = CharSet.All);

        string GenerateRandomString(int length,
            int minUpperCaseChars,
            int minLowerCaseChars,
            int minNumericChars,
            int minSpecialChars,
            CharSet fillRest)
        {
            throw new NotImplementedException("");
        }
    }
}