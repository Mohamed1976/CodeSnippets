using System;
using Xunit;
using RandomString;
using RandomString.Charset;

namespace RandomStringTests
{
    public class ExceptionHandlingTestCases
    {
        private readonly IRandomStringGenerator _randomStringGenerator = null;

        public ExceptionHandlingTestCases()
        {
            _randomStringGenerator = new RandomStringGenerator();
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
                    _randomStringGenerator.Next(0, 0, 0, 0, Common.MaxStringLength + 1, false);
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
                    _randomStringGenerator.Next(0, 0, 0, 0, Common.MinStringLength - 1, false);
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
                    _randomStringGenerator.Next(-1, 0, 0, 0, 20, false);
                });

            Assert.Equal(expectedException.Message, ex.Message);

            ex = Assert.Throws<ArgumentException>(
                () =>
                {
                    _randomStringGenerator.Next(0, -1, 0, 0, 20, false);
                });

            Assert.Equal(expectedException.Message, ex.Message);

            ex = Assert.Throws<ArgumentException>(
                () =>
                {
                    _randomStringGenerator.Next(0, 0, -1, 0, 20, false);
                });

            Assert.Equal(expectedException.Message, ex.Message);

            ex = Assert.Throws<ArgumentException>(
                () =>
                {
                    _randomStringGenerator.Next(0, 0, 0, -1, 20, false);
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
                    _randomStringGenerator.Next(5, 5, 5, 6, 20, false);
                });

            Assert.Equal(expectedException.Message, ex.Message);

            ex = Assert.Throws<ArgumentException>(
                () =>
                {
                    _randomStringGenerator.Next(5, 5, 6, 5, 20, false);
                });

            Assert.Equal(expectedException.Message, ex.Message);

            ex = Assert.Throws<ArgumentException>(
                () =>
                {
                    _randomStringGenerator.Next(5, 6, 5, 5, 20, false);
                });

            Assert.Equal(expectedException.Message, ex.Message);

            ex = Assert.Throws<ArgumentException>(
                () =>
                {
                    _randomStringGenerator.Next(6, 5, 5, 5, 20, false);
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