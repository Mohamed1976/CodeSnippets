using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleWithDI.RandomString
{
    public class RandomStringGenerator
    {
        private RandomGenerator randomGenerator = null;

        public RandomStringGenerator()
        {
            randomGenerator = new RandomGenerator();
        }


        public string GenerateString(char[] chars, int length)
        {
            //Validate char array, size

            StringBuilder sb = new StringBuilder();
            int size = chars.Length;

            for (int i = 0; i < length; i++)
            {
                sb.Append(chars[randomGenerator.Next(size)]);
            }

            return sb.ToString();
        }
    }
}
