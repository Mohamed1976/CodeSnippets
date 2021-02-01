using RandomString;
using RandomString.Charset;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Text.RegularExpressions;

namespace RandomStringTests
{
    public class CharsetValidationTestCases
    {
        [Fact]
        public void GetUpperCaseLetters()
        {
            const int nrOfUpperCaseLetters = 26;
            char[] charset = CharsetComposer.GetCharacters(AllowedCharacters.UpperCaseLetters, false);
            Assert.Equal(nrOfUpperCaseLetters, charset.Distinct().Count());
            Assert.Equal(CharsetComposer.UpperCaseLetters.Length, charset.Length);            
            bool isValid = charset.All((ch) => CharsetComposer.UpperCaseLetters.Contains(ch));
            Assert.True(isValid);
            isValid = charset.All((ch) => char.IsUpper(ch));
            Assert.True(isValid);
        }

        [Fact]
        public void GetLowerCaseLetters()
        {
            const int nrOfLowerCaseLetters = 26;
            char[] charset = CharsetComposer.GetCharacters(AllowedCharacters.LowerCaseLetters, false);
            Assert.Equal(nrOfLowerCaseLetters, charset.Distinct().Count());
            Assert.Equal(CharsetComposer.LowerCaseLetters.Length, charset.Length);
            bool isValid = charset.All((ch) => CharsetComposer.LowerCaseLetters.Contains(ch));            
            Assert.True(isValid);
            isValid = charset.All((ch) => char.IsLetter(ch));
            Assert.True(isValid);
        }

        [Fact]
        public void GetDigits()
        {
            const int nrOfDigits = 10;            
            char[] charset = CharsetComposer.GetCharacters(AllowedCharacters.Digits, false);            
            Assert.Equal(nrOfDigits, charset.Distinct().Count());
            Assert.Equal(CharsetComposer.Digits.Length, charset.Length);
            bool isValid = charset.All((ch) => CharsetComposer.Digits.Contains(ch));
            Assert.True(isValid);
            isValid = charset.All((ch) => char.IsDigit(ch));
            Assert.True(isValid);
        }

        [Fact]
        public void GetBrackets()
        {
            const int nrOfBrackets = 8;
            char[] IsBracket = { '<', '>', '{', '}', '[', ']', '(', ')' };
            
            char[] charset = CharsetComposer.GetCharacters(AllowedCharacters.Brackets, false);
            Assert.Equal(nrOfBrackets, charset.Distinct().Count());
            Assert.Equal(CharsetComposer.Brackets.Length, charset.Length);
            bool isValid = charset.All((ch) => CharsetComposer.Brackets.Contains(ch));
            Assert.True(isValid);
            isValid = charset.All((ch) => IsBracket.Contains(ch));
            Assert.True(isValid);
        }

        [Fact]
        public void GetSpace()
        {
            const int nrOfSpaces = 1;
            char[] charset = CharsetComposer.GetCharacters(AllowedCharacters.Space, false);
            Assert.True(nrOfSpaces == charset.Distinct().Count());
            Assert.Equal(CharsetComposer.Space.Length, charset.Length);
            bool isValid = charset.All((ch) => CharsetComposer.Space.Contains(ch));
            Assert.True(isValid);
            isValid = charset.All((ch) => char.IsWhiteSpace(ch));
            Assert.True(isValid);
        }

        [Fact]
        public void GetMinus()
        {
            char[] IsMinus = { '-' };
            const int nrOfMinusus = 1;

            char[] charset = CharsetComposer.GetCharacters(AllowedCharacters.Minus, false);
            Assert.True(nrOfMinusus == charset.Distinct().Count());
            Assert.Equal(CharsetComposer.Minus.Length, charset.Length);            
            bool isValid = charset.All((ch) => CharsetComposer.Minus.Contains(ch));
            Assert.True(isValid);
            isValid = charset.All((ch) => IsMinus.Contains(ch));
            Assert.True(isValid);
        }

        [Fact]
        public void GetUnderscore()
        {
            char[] IsUnderscore = { '_' };
            const int nrOfUnderscores = 1;

            char[] charset = CharsetComposer.GetCharacters(AllowedCharacters.Underscore, false);
            
            Assert.True(nrOfUnderscores == charset.Distinct().Count());
            Assert.Equal(CharsetComposer.Underscore.Length, charset.Length);
            bool isValid = charset.All((ch) => CharsetComposer.Underscore.Contains(ch));
            Assert.True(isValid);
            isValid = charset.All((ch) => IsUnderscore.Contains(ch));
            Assert.True(isValid);

        }

        [Fact]
        public void GetSpecialReadableAsciiLetters()
        {
            char[] IsSpecialReadableAsciiLetters =
            {
                        '!', '"', '#', '$', '%', '&', '\'', '*',
                        '+', ',', '.', '/', ':', ';', '=', '?',
                        '@', '\\', '^', '´', '`', '|', '~'
            };

            char[] charset = CharsetComposer.GetCharacters(AllowedCharacters.SpecialChars, false);

            Assert.True(IsSpecialReadableAsciiLetters.Length == charset.Distinct().Count());
            Assert.Equal(CharsetComposer.SpecialReadableAsciiLetters.Length, charset.Length);
            bool isValid = charset.All((ch) => CharsetComposer.SpecialReadableAsciiLetters.Contains(ch));
            Assert.True(isValid);
            isValid = charset.All((ch) => IsSpecialReadableAsciiLetters.Contains(ch));
            Assert.True(isValid);
        }

        [Fact]
        public void GetAllChars()
        {
            char[] UpperCaseLetters =
            {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H',
                'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
                'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
            };

            char[] LowerCaseLetters =
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h',
                'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
                'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
            };

            char[] Digits =
            {
                '1','2','3','4','5','6','7','8','9','0'
            };
            
            char[] SpecialReadableAsciiLetters =
            {
                '!', '"', '#', '$', '%', '&', '\'', '*',
                '+', ',', '.', '/', ':', ';', '=', '?',
                '@', '\\', '^', '´', '`', '|', '~'
            };

            char[] Minus = { '-' };

            char[] Underscore = { '_' };

            char[] Space = { ' ' };

            char[] Brackets = { '<', '>', '{', '}', '[', ']', '(', ')' };

            char[] SimilarLookingCharacters = { '1', 'l', 'I', '|', 'o', 'O', '0' };

            char[] allChars = UpperCaseLetters
                .Concat(LowerCaseLetters)
                .Concat(Digits)
                .Concat(SpecialReadableAsciiLetters)
                .Concat(Minus)
                .Concat(Underscore)
                .Concat(Space)
                .Concat(Brackets)
                .ToArray();

            char[] charset = CharsetComposer.GetCharacters(AllowedCharacters.All, false);
            Assert.True(charset.Count() == charset.Distinct().Count());
            Assert.True(allChars.Count() == charset.Count());
            bool isValid = allChars.All((ch) => charset.Contains(ch));
            Assert.True(isValid);
            /* Charset also contains similar looking characters. */
            isValid = SimilarLookingCharacters.All((ch) => charset.Contains(ch));
            Assert.True(isValid);

            charset = CharsetComposer.GetCharacters(AllowedCharacters.UpperCaseLetters |
                AllowedCharacters.LowerCaseLetters |
                AllowedCharacters.Digits |
                AllowedCharacters.Brackets |
                AllowedCharacters.Minus |
                AllowedCharacters.Space |
                AllowedCharacters.SpecialChars |
                AllowedCharacters.Underscore,
                false);

            Assert.True(charset.Count() == charset.Distinct().Count());
            Assert.True(allChars.Count() == charset.Count());
            isValid = allChars.All((ch) => charset.Contains(ch));
            Assert.True(isValid);
            /* Charset also contains similar looking characters. */
            //isValid = SimilarLookingCharacters.All((ch) => charset.Contains(ch));
            //Assert.True(isValid);
        }

        [Fact]
        public void GetAllCharsAndExcludeSimilarLookingCharacters()
        {
            char[] UpperCaseLetters =
            {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H',
                'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
                'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
            };

            char[] LowerCaseLetters =
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h',
                'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
                'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
            };

            char[] Digits =
            {
                '1','2','3','4','5','6','7','8','9','0'
            };

            char[] SpecialReadableAsciiLetters =
            {
                '!', '"', '#', '$', '%', '&', '\'', '*',
                '+', ',', '.', '/', ':', ';', '=', '?',
                '@', '\\', '^', '´', '`', '|', '~'
            };

            char[] Minus = { '-' };

            char[] Underscore = { '_' };

            char[] Space = { ' ' };

            char[] Brackets = { '<', '>', '{', '}', '[', ']', '(', ')' };

            char[] SimilarLookingCharacters = { '1', 'l', 'I', '|', 'o', 'O', '0' };

            List<char> allChars = UpperCaseLetters
                .Concat(LowerCaseLetters)
                .Concat(Digits)
                .Concat(SpecialReadableAsciiLetters)
                .Concat(Minus)
                .Concat(Underscore)
                .Concat(Space)
                .Concat(Brackets)
                .ToList();

            int removed = allChars.RemoveAll((ch) => SimilarLookingCharacters.Contains(ch));
            Assert.True(SimilarLookingCharacters.Length == removed);
            char[] charset = CharsetComposer.GetCharacters(AllowedCharacters.All, true);
            Assert.True(charset.Count() == charset.Distinct().Count());
            Assert.True(allChars.Count() == charset.Count());
            bool isValid = allChars.All((ch) => charset.Contains(ch));
            Assert.True(isValid);
        }
    }
}
