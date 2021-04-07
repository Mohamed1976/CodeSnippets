using RandomString;
using Xunit;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace RandomStringTests
{
    public class RandomStringGeneratorValidationTestCases
    {
        //private readonly IRandomStringGenerator _pseudoRandomGenerator = null;
        private readonly IRandomStringGenerator _cryptoRandomGenerator = null;

        public RandomStringGeneratorValidationTestCases()
        {
            //_pseudoRandomGenerator = new RandomStringGenerator(RandomGeneratorType.PseudoRandomGenerator);
            //Assert.NotNull(_pseudoRandomGenerator);

            _cryptoRandomGenerator = new RandomStringGenerator(RandomGeneratorType.SecureRandomGenerator);
            Assert.NotNull(_cryptoRandomGenerator);
        }

        [Theory]
        [InlineData(TestCasesCommon.MinStringLength, false)]
        [InlineData(TestCasesCommon.MinStringLength, true)]
        [InlineData(TestCasesCommon.MinStringLength + 1, false)]
        [InlineData(TestCasesCommon.MinStringLength + 1, true)]
        [InlineData(TestCasesCommon.MaxStringLength, false)]
        [InlineData(TestCasesCommon.MaxStringLength, true)]
        [InlineData(TestCasesCommon.MaxStringLength - 1, false)]
        [InlineData(TestCasesCommon.MaxStringLength - 1, true)]
        public void GenerateStringUsingUpperCaseLettersAndCryptoRandomGenerator(int length, 
            bool excludeSimilarLookingCharacters)
        {
            string randomString = _cryptoRandomGenerator.Next(AllowedCharacters.UpperCaseLetters, length,
                excludeSimilarLookingCharacters);

            Assert.True(randomString.Length == length);
            bool isValid = randomString.All((ch) => TestCasesCommon.UpperCaseLetters.Contains(ch));
            Assert.True(isValid);

            if(excludeSimilarLookingCharacters)
            {
                isValid = randomString.All((ch) => !TestCasesCommon.SimilarLookingCharacters.Contains(ch));
                Assert.True(isValid);
            }                
        }

        [Theory]
        [InlineData(TestCasesCommon.MinStringLength, false)]
        [InlineData(TestCasesCommon.MinStringLength, true)]
        [InlineData(TestCasesCommon.MinStringLength + 1, false)]
        [InlineData(TestCasesCommon.MinStringLength + 1, true)]
        [InlineData(TestCasesCommon.MaxStringLength, false)]
        [InlineData(TestCasesCommon.MaxStringLength, true)]
        [InlineData(TestCasesCommon.MaxStringLength - 1, false)]
        [InlineData(TestCasesCommon.MaxStringLength - 1, true)]
        public void GenerateStringUsingLowerCaseLettersAndCryptoRandomGenerator(int length,
            bool excludeSimilarLookingCharacters)
        {
            string randomString = _cryptoRandomGenerator.Next(AllowedCharacters.LowerCaseLetters, length,
                excludeSimilarLookingCharacters);

            Assert.True(randomString.Length == length);
            bool isValid = randomString.All((ch) => TestCasesCommon.LowerCaseLetters.Contains(ch));
            Assert.True(isValid);

            if (excludeSimilarLookingCharacters)
            {
                isValid = randomString.All((ch) => !TestCasesCommon.SimilarLookingCharacters.Contains(ch));
                Assert.True(isValid);
            }
        }

        [Theory]
        [InlineData(TestCasesCommon.MinStringLength, false)]
        [InlineData(TestCasesCommon.MinStringLength, true)]
        [InlineData(TestCasesCommon.MinStringLength + 1, false)]
        [InlineData(TestCasesCommon.MinStringLength + 1, true)]
        [InlineData(TestCasesCommon.MaxStringLength, false)]
        [InlineData(TestCasesCommon.MaxStringLength, true)]
        [InlineData(TestCasesCommon.MaxStringLength - 1, false)]
        [InlineData(TestCasesCommon.MaxStringLength - 1, true)]
        public void GenerateStringUsingDigitsAndCryptoRandomGenerator(int length,
            bool excludeSimilarLookingCharacters)
        {
            string randomString = _cryptoRandomGenerator.Next(AllowedCharacters.Digits, length,
                excludeSimilarLookingCharacters);

            Assert.True(randomString.Length == length);
            bool isValid = randomString.All((ch) => TestCasesCommon.Digits.Contains(ch));
            Assert.True(isValid);

            if (excludeSimilarLookingCharacters)
            {
                isValid = randomString.All((ch) => !TestCasesCommon.SimilarLookingCharacters.Contains(ch));
                Assert.True(isValid);
            }
        }

        [Theory]
        [InlineData(TestCasesCommon.MinStringLength, false)]
        [InlineData(TestCasesCommon.MinStringLength, true)]
        [InlineData(TestCasesCommon.MinStringLength + 1, false)]
        [InlineData(TestCasesCommon.MinStringLength + 1, true)]
        [InlineData(TestCasesCommon.MaxStringLength, false)]
        [InlineData(TestCasesCommon.MaxStringLength, true)]
        [InlineData(TestCasesCommon.MaxStringLength - 1, false)]
        [InlineData(TestCasesCommon.MaxStringLength - 1, true)]
        public void GenerateStringUsingSpecialCharsAndCryptoRandomGenerator(int length,
            bool excludeSimilarLookingCharacters)
        {
            string randomString = _cryptoRandomGenerator.Next(AllowedCharacters.SpecialChars, length,
                excludeSimilarLookingCharacters);

            Assert.True(randomString.Length == length);
            bool isValid = randomString.All((ch) => TestCasesCommon.SpecialReadableAsciiLetters.Contains(ch));
            Assert.True(isValid);

            if (excludeSimilarLookingCharacters)
            {
                isValid = randomString.All((ch) => !TestCasesCommon.SimilarLookingCharacters.Contains(ch));
                Assert.True(isValid);
            }
        }

        [Theory]
        [InlineData(TestCasesCommon.MinStringLength, false)]
        [InlineData(TestCasesCommon.MinStringLength, true)]
        [InlineData(TestCasesCommon.MinStringLength + 1, false)]
        [InlineData(TestCasesCommon.MinStringLength + 1, true)]
        [InlineData(TestCasesCommon.MaxStringLength, false)]
        [InlineData(TestCasesCommon.MaxStringLength, true)]
        [InlineData(TestCasesCommon.MaxStringLength - 1, false)]
        [InlineData(TestCasesCommon.MaxStringLength - 1, true)]
        public void GenerateStringUsingBracketsAndCryptoRandomGenerator(int length,
            bool excludeSimilarLookingCharacters)
        {
            string randomString = _cryptoRandomGenerator.Next(AllowedCharacters.Brackets, length,
                excludeSimilarLookingCharacters);

            Assert.True(randomString.Length == length);
            bool isValid = randomString.All((ch) => TestCasesCommon.Brackets.Contains(ch));
            Assert.True(isValid);

            if (excludeSimilarLookingCharacters)
            {
                isValid = randomString.All((ch) => !TestCasesCommon.SimilarLookingCharacters.Contains(ch));
                Assert.True(isValid);
            }
        }

        [Theory]
        [InlineData(TestCasesCommon.MinStringLength, false)]
        [InlineData(TestCasesCommon.MinStringLength, true)]
        [InlineData(TestCasesCommon.MinStringLength + 1, false)]
        [InlineData(TestCasesCommon.MinStringLength + 1, true)]
        [InlineData(TestCasesCommon.MaxStringLength, false)]
        [InlineData(TestCasesCommon.MaxStringLength, true)]
        [InlineData(TestCasesCommon.MaxStringLength - 1, false)]
        [InlineData(TestCasesCommon.MaxStringLength - 1, true)]
        public void GenerateStringUsingAllCharsAndCryptoRandomGenerator(int length,
            bool excludeSimilarLookingCharacters)
        {
            string randomString = _cryptoRandomGenerator.Next(AllowedCharacters.All, length,
                excludeSimilarLookingCharacters);

            Assert.True(randomString.Length == length);
            bool isValid = randomString.All((ch) =>
            {
                return TestCasesCommon.UpperCaseLetters.Contains(ch) || TestCasesCommon.LowerCaseLetters.Contains(ch) || 
                    TestCasesCommon.Digits.Contains(ch) || TestCasesCommon.SpecialReadableAsciiLetters.Contains(ch) ||
                    TestCasesCommon.Minus.Contains(ch) || TestCasesCommon.Space.Contains(ch) ||
                    TestCasesCommon.Underscore.Contains(ch) || TestCasesCommon.Brackets.Contains(ch);
            });
            Assert.True(isValid);

            if (excludeSimilarLookingCharacters)
            {
                isValid = randomString.All((ch) => !TestCasesCommon.SimilarLookingCharacters.Contains(ch));
                Assert.True(isValid);
            }
        }
    }
}
