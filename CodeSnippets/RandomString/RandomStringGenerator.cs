/*
   Copyright 2021 Mohamed Kalmoua
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using RandomString.Charset;
using RandomString.RandomGenerators;
using System.Text;
using System.Linq;
using System;

namespace RandomString
{
    /// <summary>
    /// The main Math class.
    /// Contains all methods for performing basic math functions.
    /// </summary>
    /// <remarks>
    /// This class can add, subtract, multiply and divide.
    /// These operations can be performed on both integers and doubles
    /// </remarks>
    /// <exception cref="System.OverflowException">Thrown when one parameter is max
    /// and the other is greater than 0.</exception>
    /// <exception cref="System.OverflowException">Thrown when one parameter is max
    /// and the other is greater than 0.</exception>
    /// See <see cref="Math.Add(double, double)"/> to add doubles.

    /// <summary>
    /// A generator that creates random strings restricted to the character set supplied.
    /// Simple utility class for generating random <c>string</c>s of any arbitrary size.
    /// </summary

    /// <summary>
    /// Gets the default legal characters for <c>string</c> generation that is used
    /// when no specific legal <c>char[]</c> array is passed as an argument to the <see cref="GenerateRandomString(int)"/> method.
    /// </summary>
    /// <returns>Returns a copy of <see cref="DEFAULT_LEGAL_CHARS"/>.</returns>

    /// <summary>
    /// Generates a random <c>string</c> using only alpha-numeric characters.<para> </para>
    /// If the passed <paramref name="size"/> argument is 0, <c>string.Empty</c> is returned.<para> </para>
    /// If <paramref name="size"/> is &lt;0, the absolute value of that number will be used.<para> </para>
    /// A random <c>string</c> is returned.
    /// </summary>
    /// <param name="size">How long should the generated <c>string</c> be?</param>
    /// <returns>If the passed <paramref name="size"/> argument is 0, <c>string.Empty</c> is returned. If <paramref name="size"/> is &lt;0, the absolute value of that number will be used. A random <c>string</c> is returned.</returns>

    /// <summary>
    /// Generates a random <c>string</c> from a specific set of legal characters.<para> </para>
    /// If the passed <paramref name="size"/> argument is 0, or if <paramref name="legalChars"/> is <c>null</c> or empty, <c>string.Empty</c> is returned.<para> </para>
    /// If <paramref name="size"/> is &lt;0, the absolute value of that number will be used.<para> </para>
    /// On success, a random <c>string</c> is returned.
    /// </summary>
    /// <param name="legalChars">The legal characters to be used for the <c>string</c> generation.</param>
    /// <param name="size">The desired output <c>string</c>'s length.</param>
    /// <returns>If the passed <paramref name="size"/> argument is 0, or if <paramref name="legalChars"/> is <c>null</c> or empty, <c>string.Empty</c> is returned. If <paramref name="size"/> is &lt;0, the absolute value of that number will be used. On success, a random <c>string</c> is returned.</returns>

    /// <summary>
    /// Generates a random <c>string</c> from a specific set of legal characters.<para> </para>
    /// If the passed <paramref name="size"/> argument is 0, or if <paramref name="legalChars"/> is <c>null</c> or empty, <c>string.Empty</c> is returned.<para> </para>
    /// If <paramref name="size"/> is &lt;0, the absolute value of that number will be used.<para> </para>
    /// On success, a random <c>string</c> is returned.
    /// </summary>
    /// <param name="legalChars">The legal chars to be used for the <c>string</c> generation.</param>
    /// <param name="size">The desired output <c>string</c>'s length.</param>
    /// <returns>If the passed <paramref name="size"/> argument is 0, or if <paramref name="legalChars"/> is <c>null</c> or empty, <c>string.Empty</c> is returned. If <paramref name="size"/> is &lt;0, the absolute value of that number will be used. On success, a random <c>string</c> is returned.</returns>
    /// <returns>If the passed <paramref name="size"/> argument is 0, or if <paramref name="legalChars"/> is <c>null</c> or empty, <c>string.Empty</c> is returned. If <paramref name="size"/> is &lt;0, the absolute value of that number will be used. On success, a random <c>string</c> is returned.</returns>
    public class RandomStringGenerator : IRandomStringGenerator
    {
        #region [ private members]

        private readonly IRandomGenerator _randomGenerator = null;

        #endregion

        #region [ Constructor ]

        /// <summary>
        /// Represents a pseudo-random number generator, a device that produces a sequence
        /// of numbers that meet certain statistical requirements for randomness.
        /// </summary>
        /// <remarks>
        /// Use the RNGCryptoServiceProvider class if you need a strong random number generator.
        /// </remarks>
        public RandomStringGenerator(RandomGeneratorType randomGeneratorType =
            RandomGeneratorType.SecureRandomGenerator)
        {
            if(randomGeneratorType == RandomGeneratorType.SecureRandomGenerator) {
                _randomGenerator = new CryptoRandomGenerator();
            }
            else {
                _randomGenerator = new PseudoRandomGenerator();  
            }
        }

        #endregion

        #region [ IRandomStringGenerator Methods ]

        /// <summary>
        /// Creates a random string whose length is the number of characters specified.
        /// Characters will be chosen from the charsets specified.
        /// Min = <see cref="Common.MinStringLength"/> and Max = <see cref="Common.MaxStringLength"/> 
        /// 
        /// </summary>
        ///<value cref="Common.MinStringLength"></value> 
        /// <param name="allowedCharacters">The enum specifies the charsets to use.</param>
        /// <param name="length">The length of the random string to create.</param>
        /// <param name="excludeSimilarLookingCharacters">Specifies whether to exclude similar looking characters in the generated random string.</param>
        /// <returns>A random generated string</returns>
        /// <exception cref="ArgumentException">If length is larger </exception>
        /// <exception cref="System.ArgumentException">Thrown when age is set to be less than 18 years.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when age is set to be more than 70 years.</exception>
        public string Next(AllowedCharacters allowedCharacters, int length, bool excludeSimilarLookingCharacters)
        {            
            string randomString;
            char[] charset;

            if (allowedCharacters == AllowedCharacters.None)
                throw new ArgumentException(Common.NO_CHARSET_SPECIFIED, nameof(allowedCharacters));

            if (length < Common.MinStringLength || length > Common.MaxStringLength)
                throw new ArgumentException(Common.STRING_LENGTH_EXCEEDS_RANGE, nameof(length));

            /* Get chosen charset. */
            charset = CharsetComposer.GetCharacters(allowedCharacters, excludeSimilarLookingCharacters);

            /* Generate random string. */
            randomString = GetRandomString(charset, length);
            
            return randomString;
        }

        //Add FillRest = CharType.LowerCase | UpperCase | Digits;
        public string Next(int minUpperCaseLetters, int minLowerCaseLetters, int minDigits, 
            int minSpecialChars, int length, bool excludeSimilarLookingCharacters)
        {
            int nrOfPreSpecifiedChars;
            string randomString = string.Empty;
            char[] charset;

            if (length < Common.MinStringLength || length > Common.MaxStringLength)
                throw new ArgumentException(Common.STRING_LENGTH_EXCEEDS_RANGE, nameof(length));

            if(minUpperCaseLetters < 0 || minLowerCaseLetters < 0 || minDigits < 0 || minSpecialChars < 0)
                throw new ArgumentException(Common.MINIMUM_PARAMETER_IS_NEGATIVE);

            nrOfPreSpecifiedChars = minUpperCaseLetters + minLowerCaseLetters + minDigits + minSpecialChars;

            if(nrOfPreSpecifiedChars > length)
                throw new ArgumentException(Common.MINIMUM_PARAMETER_EXCEEDS_LENGTH);

            if(minUpperCaseLetters > 0)
            {
                charset = CharsetComposer.GetCharacters(AllowedCharacters.UpperCaseLetters, 
                    excludeSimilarLookingCharacters);
                randomString += GetRandomString(charset, minUpperCaseLetters);
            }

            if (minLowerCaseLetters > 0)
            {
                charset = CharsetComposer.GetCharacters(AllowedCharacters.LowerCaseLetters,
                    excludeSimilarLookingCharacters);
                randomString += GetRandomString(charset, minLowerCaseLetters);
            }

            if (minDigits > 0)
            {
                charset = CharsetComposer.GetCharacters(AllowedCharacters.Digits,
                    excludeSimilarLookingCharacters);
                randomString += GetRandomString(charset, minDigits);
            }

            if (minSpecialChars > 0)
            {
                charset = CharsetComposer.GetCharacters(AllowedCharacters.SpecialChars,
                    excludeSimilarLookingCharacters);
                randomString += GetRandomString(charset, minSpecialChars);
            }

            /* The rest of the characters can be chosen from the complete set, Letters, digits and special symbols. */
            if(length > nrOfPreSpecifiedChars)
            {
                charset = CharsetComposer.GetCharacters(AllowedCharacters.UpperCaseLetters | 
                    AllowedCharacters.LowerCaseLetters | AllowedCharacters.Digits | 
                    AllowedCharacters.SpecialChars, excludeSimilarLookingCharacters);

                randomString += GetRandomString(charset, length - nrOfPreSpecifiedChars);
            }

            /* Randomly shuffle chars in string. */
            randomString = ShuffleString(randomString);

            return randomString;
        }

        /// <summary>
        /// Creates a random string whose length is the number of characters specified.
        /// Characters will be chosen from the char array specified.
        /// </summary>
        /// <param name="allowedCharacters">The characters to select from when </param>
        /// <param name="length">The size of the random string being generated.</param>
        /// The list of characters that is to be used when creating the random string
        public string Next(char[] allowedCharacters, int length)
        {
            string randomString;
            int nrOfUniqueChars;

            if (allowedCharacters is null)
                throw new ArgumentNullException(nameof(allowedCharacters));

            nrOfUniqueChars = allowedCharacters.Distinct().Count();
            if(nrOfUniqueChars < Common.MinNrOfChars)
                throw new ArgumentException(Common.NO_CHARS_SPECIFIED, nameof(allowedCharacters));

            if (length < Common.MinStringLength || length > Common.MaxStringLength)
                throw new ArgumentException(Common.STRING_LENGTH_EXCEEDS_RANGE, nameof(length));

            randomString = GetRandomString(allowedCharacters, length);

            return randomString;
        }

        #endregion

        #region [ Private Methods]

        private string GetRandomString(char[] allowedCharacters, int length)
        {
            int randVal;
            StringBuilder sb = new StringBuilder();

            for (int index = 0; index < length; index++)
            {
                randVal = _randomGenerator.Next(allowedCharacters.Length);
                sb.Append(allowedCharacters[randVal]);
            }

            return sb.ToString();
        }

        /* The Fisher-Yates algorithms orders the array in place with a cost of of O(n). */
        private string ShuffleString(string inputString)
        {
            int randomIndex;
            char temp;

            /* Copies the characters in this instance to a Unicode character array. */
            char[] inputCharArray = inputString.ToCharArray();
            for (int i = inputCharArray.Length - 1; i > 0; i--)
            {
                randomIndex = _randomGenerator.Next(0, i + 1);
                temp = inputCharArray[i];
                inputCharArray[i] = inputCharArray[randomIndex];
                inputCharArray[randomIndex] = temp;
            }

            return new string(inputCharArray);
        }

        #endregion
    }
}

/*
Sign assembly (Strong name):

https://docs.microsoft.com/en-us/dotnet/standard/assembly/create-public-private-key-pair 
C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools>sn -k sgKey.snk   (Admin rights) 

Properties -> Signing select key.
*/