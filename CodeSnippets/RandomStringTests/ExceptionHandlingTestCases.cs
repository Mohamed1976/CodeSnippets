using System;
using Xunit;
using RandomString;
using RandomString.Charset;
using RandomString.RandomGenerators;

namespace RandomStringTests
{
    public class ExceptionHandlingTestCases
    {
        private readonly IRandomStringGenerator _randomStringGenerator = null;
        private readonly IRandomGenerator _pseudoRandomGenerator = null;
        private readonly IRandomGenerator _cryptoRandomGenerator = null;

        public ExceptionHandlingTestCases()
        {
            _randomStringGenerator = new RandomStringGenerator();
            _pseudoRandomGenerator = new PseudoRandomGenerator();
            _cryptoRandomGenerator = new CryptoRandomGenerator();
        }

        [Fact]
        public void InvalidMaxValuePseudoRandomGeneratorNextMethod()
        {
            const string paramName = "maxValue";
            ArgumentOutOfRangeException expectedException = new ArgumentOutOfRangeException(paramName, "'maxValue' must be greater than zero.");

            ArgumentOutOfRangeException ex = 
                Assert.Throws<ArgumentOutOfRangeException>(paramName, () =>
                {
                    _pseudoRandomGenerator.Next(-1);
                });

            Assert.Equal(expectedException.Message, ex.Message);
        }

        [Fact]
        public void InvalidMaxValueInRangePseudoRandomGeneratorNextMethod()
        {
            const string paramName = "maxValue";
            ArgumentOutOfRangeException expectedException =
                new ArgumentOutOfRangeException(paramName, "'maxValue' must be greater than zero.");

            ArgumentOutOfRangeException ex =
                Assert.Throws<ArgumentOutOfRangeException>(paramName, () =>
                {
                    _pseudoRandomGenerator.Next(0, -1);
                });

            Assert.Equal(expectedException.Message, ex.Message);
        }

        [Fact]
        public void InvalidMinValueInRangePseudoRandomGeneratorNextMethod()
        {
            const string paramName = "minValue";
            ArgumentOutOfRangeException expectedException =
                new ArgumentOutOfRangeException(paramName, "'minValue' must be greater than zero.");

            ArgumentOutOfRangeException ex =
                Assert.Throws<ArgumentOutOfRangeException>(paramName, () =>
                {
                    _pseudoRandomGenerator.Next(-1, 1);
                });

            Assert.Equal(expectedException.Message, ex.Message);
        }

        [Fact]
        public void MinValueGreaterThenMaxValuePseudoRandomGeneratorNextMethod()
        {
            const string paramName = "minValue";
            ArgumentOutOfRangeException expectedException =
                new ArgumentOutOfRangeException(paramName, "'minValue' cannot be greater than maxValue.");

            ArgumentOutOfRangeException ex =
                Assert.Throws<ArgumentOutOfRangeException>(paramName, () =>
                {
                    _pseudoRandomGenerator.Next(11, 10);
                });

            Assert.Equal(expectedException.Message, ex.Message);
        }

        [Fact]
        public void InvalidMaxValueCryptoRandomGeneratorNextMethod()
        {
            const string paramName = "maxValue";
            ArgumentOutOfRangeException expectedException = new ArgumentOutOfRangeException(paramName, "'maxValue' must be greater than zero.");

            ArgumentOutOfRangeException ex =
                Assert.Throws<ArgumentOutOfRangeException>(paramName, () =>
                {
                    _cryptoRandomGenerator.Next(-1);
                });

            Assert.Equal(expectedException.Message, ex.Message);
        }

        [Fact]
        public void InvalidMaxValueInRangeCryptoRandomGeneratorNextMethod()
        {
            const string paramName = "maxValue";
            ArgumentOutOfRangeException expectedException = 
                new ArgumentOutOfRangeException(paramName, "'maxValue' must be greater than zero.");

            ArgumentOutOfRangeException ex =
                Assert.Throws<ArgumentOutOfRangeException>(paramName, () =>
                {
                    _cryptoRandomGenerator.Next(0, -1);
                });

            Assert.Equal(expectedException.Message, ex.Message);
        }

        [Fact]
        public void InvalidMinValueInRangeCryptoRandomGeneratorNextMethod()
        {
            const string paramName = "minValue";
            ArgumentOutOfRangeException expectedException =
                new ArgumentOutOfRangeException(paramName, "'minValue' must be greater than zero.");

            ArgumentOutOfRangeException ex =
                Assert.Throws<ArgumentOutOfRangeException>(paramName, () =>
                {
                    _cryptoRandomGenerator.Next(-1, 1);
                });

            Assert.Equal(expectedException.Message, ex.Message);
        }

        [Fact]
        public void MinValueGreaterThenMaxValueCryptoRandomGeneratorNextMethod()
        {
            const string paramName = "minValue";
            ArgumentOutOfRangeException expectedException =
                new ArgumentOutOfRangeException(paramName, "'minValue' cannot be greater than maxValue.");

            ArgumentOutOfRangeException ex =
                Assert.Throws<ArgumentOutOfRangeException>(paramName, () =>
                {
                    _cryptoRandomGenerator.Next(11, 10);
                });

            Assert.Equal(expectedException.Message, ex.Message);
        }

        [Fact]
        /* public string Next(AllowedCharacters allowedCharacters, int length, bool excludeSimilarLookingCharacters) */
        public void InvalidCharsetNextMethod()
        {
            const string paramName = "allowedCharacters";
            ArgumentException expectedException = new ArgumentException(Common.NO_CHARSET_SPECIFIED, paramName);

            ArgumentException ex = Assert.Throws<ArgumentException>(paramName,
                () =>
                {
                    _randomStringGenerator.Next(AllowedCharacters.None, 20, false);
                });

            Assert.Equal(expectedException.Message, ex.Message);
        }

        [Fact]
        /* public string Next(AllowedCharacters allowedCharacters, int length, bool excludeSimilarLookingCharacters) */
        public void StringLengthParameterTooLargeNextMethod()
        {
            const string paramName = "length";
            ArgumentException expectedException = new ArgumentException(Common.STRING_LENGTH_EXCEEDS_RANGE, paramName);

            ArgumentException ex = Assert.Throws<ArgumentException>(paramName,
                () =>
                {
                    _randomStringGenerator.Next(AllowedCharacters.All, Common.MaxStringLength + 1, false);
                });

            Assert.Equal(expectedException.Message, ex.Message);
        }

        [Fact]
        /* public string Next(AllowedCharacters allowedCharacters, int length, bool excludeSimilarLookingCharacters) */
        public void StringLengthParameterTooSmallNextMethod()
        {
            const string paramName = "length";
            ArgumentException expectedException = new ArgumentException(Common.STRING_LENGTH_EXCEEDS_RANGE, paramName);

            ArgumentException ex = Assert.Throws<ArgumentException>(paramName,
                () =>
                {
                    _randomStringGenerator.Next(AllowedCharacters.All, Common.MinStringLength - 1, false);
                });

            Assert.Equal(expectedException.Message, ex.Message);
        }

        [Fact]
        /* public string Next(int minUpperCaseLetters, int minLowerCaseLetters, int minDigits, 
            int minSpecialChars, int length, bool excludeSimilarLookingCharacters)*/
        public void StringLengthParameterTooLargeNextMethodOverloadOne()
        {
            const string paramName = "length";
            ArgumentException expectedException = new ArgumentException(Common.STRING_LENGTH_EXCEEDS_RANGE, paramName);

            ArgumentException ex = Assert.Throws<ArgumentException>(paramName,
                () =>
                {
                    _randomStringGenerator.Next(0, 0, 0, 0, Common.MaxStringLength + 1, AllowedCharacters.All, false);
                });

            Assert.Equal(expectedException.Message, ex.Message);
        }

        [Fact]
        /* public string Next(int minUpperCaseLetters, int minLowerCaseLetters, int minDigits, 
            int minSpecialChars, int length, bool excludeSimilarLookingCharacters)*/
        public void StringLengthParameterTooSmallNextMethodOverloadOne()
        {
            const string paramName = "length";
            ArgumentException expectedException = new ArgumentException(Common.STRING_LENGTH_EXCEEDS_RANGE, paramName);

            ArgumentException ex = Assert.Throws<ArgumentException>(paramName,
                () =>
                {
                    _randomStringGenerator.Next(0, 0, 0, 0, Common.MinStringLength - 1, AllowedCharacters.All, false);
                });

            Assert.Equal(expectedException.Message, ex.Message);
        }

        [Fact]
        /* public string Next(int minUpperCaseLetters, int minLowerCaseLetters, int minDigits, 
            int minSpecialChars, int length, bool excludeSimilarLookingCharacters)*/
        public void MinOccurrenceIsNegativeNextMethodOverloadOne()
        {
            ArgumentException expectedException = new ArgumentException(Common.MINIMUM_PARAMETER_IS_NEGATIVE);

            ArgumentException ex = Assert.Throws<ArgumentException>(
                () =>
                {
                    _randomStringGenerator.Next(-1, 0, 0, 0, 20, AllowedCharacters.All, false);
                });

            Assert.Equal(expectedException.Message, ex.Message);

            ex = Assert.Throws<ArgumentException>(
                () =>
                {
                    _randomStringGenerator.Next(0, -1, 0, 0, 20, AllowedCharacters.All, false);
                });

            Assert.Equal(expectedException.Message, ex.Message);

            ex = Assert.Throws<ArgumentException>(
                () =>
                {
                    _randomStringGenerator.Next(0, 0, -1, 0, 20, AllowedCharacters.All, false);
                });

            Assert.Equal(expectedException.Message, ex.Message);

            ex = Assert.Throws<ArgumentException>(
                () =>
                {
                    _randomStringGenerator.Next(0, 0, 0, -1, 20, AllowedCharacters.All, false);
                });

            Assert.Equal(expectedException.Message, ex.Message);
        }

        [Fact]
        /* public string Next(int minUpperCaseLetters, int minLowerCaseLetters, int minDigits, 
            int minSpecialChars, int length, bool excludeSimilarLookingCharacters)*/
        public void MinOccurrenceExceedsLengthNextMethodOverloadOne()
        {
            ArgumentException expectedException = new ArgumentException(Common.MINIMUM_PARAMETER_EXCEEDS_LENGTH);

            ArgumentException ex = Assert.Throws<ArgumentException>(
                () =>
                {
                    _randomStringGenerator.Next(5, 5, 5, 6, 20, AllowedCharacters.All, false);
                });

            Assert.Equal(expectedException.Message, ex.Message);

            ex = Assert.Throws<ArgumentException>(
                () =>
                {
                    _randomStringGenerator.Next(5, 5, 6, 5, 20, AllowedCharacters.All, false);
                });

            Assert.Equal(expectedException.Message, ex.Message);

            ex = Assert.Throws<ArgumentException>(
                () =>
                {
                    _randomStringGenerator.Next(5, 6, 5, 5, 20, AllowedCharacters.All, false);
                });

            Assert.Equal(expectedException.Message, ex.Message);

            ex = Assert.Throws<ArgumentException>(
                () =>
                {
                    _randomStringGenerator.Next(6, 5, 5, 5, 20, AllowedCharacters.All, false);
                });

            Assert.Equal(expectedException.Message, ex.Message);
        }

        [Fact]
        /* public string Next(char[] allowedCharacters, int length) */
        public void CharsIsNullNextMethodOverloadTwo()
        {
            ArgumentNullException expectedException = new ArgumentNullException("allowedCharacters");

            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(
                () =>
                {
                    _randomStringGenerator.Next(null, 20);
                });

            Assert.Equal(expectedException.Message, ex.Message);
        }

        [Fact]
        /* public string Next(char[] allowedCharacters, int length) */
        public void NoCharsSpecifiedNextMethodOverloadTwo()
        {
            ArgumentException expectedException = new ArgumentException(Common.NO_CHARS_SPECIFIED, "allowedCharacters");

            ArgumentException ex = Assert.Throws<ArgumentException>(
                () =>
                {
                    _randomStringGenerator.Next(new char[] { 'm' }, 20);
                });

            Assert.Equal(expectedException.Message, ex.Message);

            ex = Assert.Throws<ArgumentException>(
                () =>
                {
                    _randomStringGenerator.Next(new char[] { 'm', 'm', 'm' }, 20);
                });

            Assert.Equal(expectedException.Message, ex.Message);
        }

        [Fact]
        /* public string Next(char[] allowedCharacters, int length) */
        public void StringLengthParameterTooSmallNextMethodOverloadTwo()
        {
            const string paramName = "length";
            ArgumentException expectedException = new ArgumentException(Common.STRING_LENGTH_EXCEEDS_RANGE, paramName);

            ArgumentException ex = Assert.Throws<ArgumentException>(paramName,
                () =>
                {
                    _randomStringGenerator.Next(new char[] { 'a','b' }, Common.MinStringLength - 1);
                });

            Assert.Equal(expectedException.Message, ex.Message);
        }

        [Fact]
        /* public string Next(char[] allowedCharacters, int length) */
        public void StringLengthParameterTooLargeNextMethodOverloadTwo()
        {
            const string paramName = "length";
            ArgumentException expectedException = new ArgumentException(Common.STRING_LENGTH_EXCEEDS_RANGE, paramName);

            ArgumentException ex = Assert.Throws<ArgumentException>(paramName,
                () =>
                {
                    _randomStringGenerator.Next(new char[] { 'a', 'b' }, Common.MaxStringLength + 1);
                });

            Assert.Equal(expectedException.Message, ex.Message);
        }

        [Fact]
        public void InvalidCharsetGetCharactersMethod()
        {
            const string paramName = "allowedCharacters";
            ArgumentException expectedException = new ArgumentException(Common.NO_CHARSET_SPECIFIED, paramName);

            ArgumentException ex = Assert.Throws<ArgumentException>(paramName,
                () =>
                {
                    char[] charset = CharsetComposer.GetCharacters(AllowedCharacters.None, false);
                });

            Assert.Equal(expectedException.Message, ex.Message);
        }
    }
}