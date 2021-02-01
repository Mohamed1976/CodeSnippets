using RandomString.Charset;
using System;
using System.Collections.Generic;
using System.Text;

namespace RandomString
{
    public interface IRandomStringGenerator
    {
        string Next(AllowedCharacters allowedCharacters, int length, bool excludeSimilarLookingCharacters);

        string Next(int minUpperCaseLetters, int minLowerCaseLetters, int minDigits, int minSpecialChars, 
            int length, bool excludeSimilarLookingCharacters);

        string Next(char[] allowedCharacters, int length);
    }
}
