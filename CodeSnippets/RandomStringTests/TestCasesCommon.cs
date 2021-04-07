using System;
using System.Collections.Generic;
using System.Text;

namespace RandomStringTests
{
    public static class TestCasesCommon
    {
        #region [ Fields ]

        public static readonly char[] UpperCaseLetters =
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H',
            'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
            'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        public static readonly char[] LowerCaseLetters =
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h',
            'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
            'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        };

        public static readonly char[] Digits =
        {
            '1','2','3','4','5','6','7','8','9','0'
        };

        public static readonly char[] SpecialReadableAsciiLetters =
        {
            '!', '"', '#', '$', '%', '&', '\'', '*',
            '+', ',', '.', '/', ':', ';', '=', '?',
            '@', '\\', '^', '´', '`', '|', '~'
        };

        public static readonly char[] Minus = { '-' };

        public static readonly char[] Underscore = { '_' };

        public static readonly char[] Space = { ' ' };

        public static readonly char[] Brackets = { '<', '>', '{', '}', '[', ']', '(', ')' };

        public static readonly char[] SimilarLookingCharacters = { '1', 'l', 'I', '|', 'o', 'O', '0' };

        public const int MinStringLength = 1;

        public const int MaxStringLength = 1000;

        #endregion
    }
}
