using System;
using System.Collections.Generic;
using System.Text;

//https://www.nuget.org/packages/RandomStringGenerator4DotNet/
//https://github.com/keyvan/RandomStringGenerator4DotNet/tree/master/RandomStringGenerator

namespace ConsoleWithDI
{
    public class RandomStringServiceV4 : IRandomStringService
    {
        public string GenerateRandomString(int stringSize = 20, CharSet charSet = CharSet.All)
        {
            throw new NotImplementedException();
        }

        public string GenerateRandomString(int length,
            int minUpperCaseChars,
            int minLowerCaseChars,
            int minNumericChars,
            int minSpecialChars,
            CharSet fillRest)
        {
            int sum = minUpperCaseChars + minLowerCaseChars + minNumericChars + minSpecialChars;
            if (length < sum)
                throw new ArgumentException("length parameter must be valid!");

            const string DIGITS = "0123456789";
            const string LOWERCASE = "abcdefghijklmnopqrstuvwxyz";
            const string SPECIAL = @"!@#$%^&*()+=~[:'<>?,.|";
            const string UPPERCASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            List<char> charsRequired = new List<char>();
            //Should be better random generator
            Random random = new Random();

            if (minUpperCaseChars > 0)
            {
                for (int index = 0; index < minUpperCaseChars; index++)
                {
                    charsRequired.Add(Char.ToUpper(Convert.ToChar(random.Next(65, 90))));
                }
            }

            if (minLowerCaseChars > 0)
            {
                for (int index = 0; index < minLowerCaseChars; index++)
                {
                    charsRequired.Add(Char.ToLower(Convert.ToChar(random.Next(97, 122))));
                }
            }

            if(minNumericChars > 0)
            {
                for (int index = 0; index < minNumericChars; index++)
                {
                    charsRequired.Add(Convert.ToChar(random.Next(0, 9).ToString()));
                }
            }

            if(minSpecialChars > 0)
            {
                for (int index = 0; index < minSpecialChars; index++)
                {
                    charsRequired.Add(Char.ToLower(Convert.ToChar(random.Next(35, 38))));
                }
            }

            int restLength = length - charsRequired.Count;
            //Use CharSet fillRest to generate more random chars.   

            string result = string.Empty;
            while (charsRequired.Count > 0)
            {
                int randomIndex = random.Next(0, charsRequired.Count);
                result += charsRequired[randomIndex];
                charsRequired.RemoveAt(randomIndex);
            }

            return result;
        }
    }
}
