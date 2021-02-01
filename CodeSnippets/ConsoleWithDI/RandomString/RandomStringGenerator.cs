/*
   Copyright 2019 Raphael Beck
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


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

/// <summary>
/// Generates random string of the given length.
/// </summary>
/// <param name="random">The source of random numbers.</param>
/// <param name="allowedChars">The allowed characters for the random string.</param>
/// <param name="length">The length of the random string.</param>
/// <returns>Randomly generated string.</returns>
/// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is less than zero.</exception>


/// <summary>
/// Generates a random <c>string</c> from a specific set of legal characters.<para> </para>
/// If the passed <paramref name="size"/> argument is 0, or if <paramref name="legalChars"/> is <c>null</c> or empty, <c>string.Empty</c> is returned.<para> </para>
/// If <paramref name="size"/> is &lt;0, the absolute value of that number will be used.<para> </para>
/// On success, a random <c>string</c> is returned.
/// </summary>
/// <param name="legalChars">The legal characters to be used for the <c>string</c> generation.</param>
/// <param name="size">The desired output <c>string</c>'s length.</param>
/// <returns>If the passed <paramref name="size"/> argument is 0, or if <paramref name="legalChars"/> is <c>null</c> or empty, <c>string.Empty</c> is returned. If <paramref name="size"/> is &lt;0, the absolute value of that number will be used. On success, a random <c>string</c> is returned.</returns>




namespace ConsoleWithDI.RandomString
{
    public enum RandomGeneratorType
    {
        PseudoRandomGenerator,
        SecureRandomGenerator
    }

    /// <summary>
    /// This class can be used to create random strings.
    /// </summary>
    public class RandomStringGenerator //: IRandomStringGenerator, IDisposable
    {
        private const int DefaultLength = 20;
        private const int MaxLength = 1000;

        private bool disposed = false;
        private readonly RandomGenerator randomGenerator = null;
        //private const bool defaultExcludeSimilarLookingCharacters = false;
        //private const int defaultLength = 20;
        //private const AllowedCharacters defaultAllowedCharacters =
        //    AllowedCharacters.LowerCaseLetters | AllowedCharacters.UpperCaseLetters |
        //    AllowedCharacters.Digits | AllowedCharacters.SpecialChars;


        /// <summary>
        /// Gets a singleton instance that uses the <see cref="Random"/> class to create a random string.
        /// This is a fast generator but should not be used to generate passwords.
        /// Use <see cref="CryptographicRandom"/> for that purpose.
        /// </summary>
        /// <summary>
        /// Gets a singleton instance that uses the <see cref="System.Security.Cryptography.RNGCryptoServiceProvider"/> to create a random string.
        /// This is a slower generator but can be used for generating passwords.
        /// </summary>
        public RandomStringGenerator(RandomGeneratorType randomGeneratorType =
            RandomGeneratorType.SecureRandomGenerator)
        {
            if (randomGeneratorType == RandomGeneratorType.SecureRandomGenerator)
            {
                //randomGenerator = new RandomGenerator();
            }
            else
            {
                //randomGenerator = new RandomGenerator();
            }


            randomGenerator = new RandomGenerator();
        }



        //The list of characters that is to be used when creating the random string
        //Creates a random string generator that will be based on the characters supplied
        //Creates a random string generator of the size supplied that will be based on the characters supplied

        /// <summary>
        /// Generates a random string based on specified options.
        /// Sets the collection of allowed characters to choose from during string generation.
        /// </summary>
        /// <param name="length">The desired length of the generated string.</param>
        /// <param name="allowedCharacters">The set of allowed characters used for string generation.</param>
        /// <param name="eachCharacterMustOccurAtLeastOnce">Specifies whether each character in the <paramref name="allowedCharacters"/>
        /// array must occur at least once in the generated random string.</param>
        /// <returns>The generated string.</returns>
        public string GenerateString(char[] allowedCharacters, int length)
        {
            if (length <= 0 || allowedCharacters is null || allowedCharacters.Length == 0)
            {
                throw new ArgumentNullException(nameof(allowedCharacters));
                // throw new ArgumentOutOfRangeException(nameof(length), "The length of the random string must be a positive non-zero integer.");
                // throw new ArgumentOutOfRangeException(nameof(allowedCharacters), "There must be at least one allowed character to create a random string.");
                //throw new InvalidOperationException("When the flag for 'each character must occur at least once' is used the desired length of the " +
                //$"random string must be at least as long as the number of allowed characters (requested length: {length} - minimum required length: {allowedCharacters.Length}).");


            }

            if (length > MaxLength)
            {
                throw new ArgumentOutOfRangeException(nameof(length), $"To prevent memory issues the maximum length for random strings is {MaxLength}.");
            }



            //throw new InvalidOperationException("When the flag for 'each character must occur at least once' is used the desired length of the " +
            //$"random string must be at least as long as the number of allowed characters (requested length: {this.stringLength} - minimum required length: {this.allowedCharacters.Count}).");

            //this.AllowedCharacters = allowedCharacters ?? throw new ArgumentNullException(nameof(allowedCharacters));
            if (allowedCharacters is null)
            {
                throw new ArgumentNullException(nameof(allowedCharacters));
            }

            //Validate char array, size
            StringBuilder sb = new StringBuilder();
            int size = allowedCharacters.Length;

            for (int i = 0; i < length; i++)
            {
                sb.Append(allowedCharacters[randomGenerator.Next(size)]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Exclude similar looking characters from the use during the random string generation.
        /// Similar looking characters are: 
        /// 'l' (lower-case L), '1' (one), '|' (pipe), 'I' (upper-case i),
        /// '0' (zero), 'O' (upper-case o), 'o' (lower-case O).
        /// </summary>
        /// <returns>The builder.</returns>

        /// <summary>
        /// Generates a random string based on specified options.
        /// Sets the collection of allowed characters to choose from during string generation by using pre-defined character groups.
        /// </summary>
        /// <param name="length">The desired length of the generated string.</param>
        /// <param name="allowedCharacters">The set of allowed characters used for string generation.</param>
        /// <param name="eachCharacterMustOccurAtLeastOnce">Specifies whether each character in the <paramref name="allowedCharacters"/>
        /// array must occur at least once in the generated random string.</param>
        /// <returns>The generated string.</returns>
        public string Generate(AllowedCharacters allowedCharacters = AllowedCharacters.LowerCaseLetters | 
            AllowedCharacters.UpperCaseLetters | AllowedCharacters.Digits | AllowedCharacters.SpecialChars,
            int length = DefaultLength, bool excludeSimilarLookingCharacters = false)
        {
            StringBuilder sb = new StringBuilder();

            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length),
                  "The length of the random string must be a positive non-zero integer.");
            }

            // Now run through and create the string
            //for (var i = 0; i < size; i++)
            //{
            //    code.Append(_characters[_random.Next(charListCount)]);
            //}

            return null;
        }

        //should be NextString( 
        public string GenerateString(int minUpperCaseLetters = 0, 
            int minLowerCaseLetters = 0, int minDigits = 0, 
            int minSpecialChars = 0, int length = DefaultLength,
            bool excludeSimilarLookingCharacters = true)
        {
            int sum = minUpperCaseLetters + minLowerCaseLetters + minDigits + minSpecialChars;

            if (length < sum)
                throw new ArgumentException("length parameter must be valid!");

            //if (this.eachCharacterMustOccurAtLeastOnce
            //  && (this.allowedCharacters.Count > this.stringLength))
            //{
            //    throw new InvalidOperationException("When the flag for 'each character must occur at least once' is used the desired length of the " +
            //      $"random string must be at least as long as the number of allowed characters (requested length: {this.stringLength} - minimum required length: {this.allowedCharacters.Count}).");
            //}



            if (minUpperCaseLetters > 0)
            {
                //chars.AddRange(GetUpperCaseChars(this.MinUpperCaseChars));
            }

            if (minLowerCaseLetters > 0)
            {
                //chars.AddRange(GetLowerCaseChars(this.MinLowerCaseChars));
            }

            if (minDigits > 0)
            {
                //chars.AddRange(GetNumericChars(this.MinNumericChars));
            }

            if (minSpecialChars > 0)
            {
                //chars.AddRange(GetSpecialChars(this.MinSpecialChars));
            }

            


            return null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (randomGenerator != null)
                {
                    randomGenerator.Dispose();
                }

                disposed = true;
            }
        }
    }
}
