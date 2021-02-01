using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomString.Charset
{
    internal static class CharsetComposer
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

        #endregion

        #region [ Methods ] 

        public static char[] GetCharacters(AllowedCharacters allowedCharacters, bool ExcludeSimilarChars = false)
        {
            List<char> allowedCharslist = new List<char>();

            if (allowedCharacters == AllowedCharacters.None)
            {
                throw new ArgumentException(Common.NO_CHARSET_SPECIFIED, nameof(allowedCharacters));
            }

            if (allowedCharacters.HasFlag(AllowedCharacters.UpperCaseLetters))
            {
                allowedCharslist.AddRange(UpperCaseLetters);
            }

            if (allowedCharacters.HasFlag(AllowedCharacters.LowerCaseLetters))
            {
                allowedCharslist.AddRange(LowerCaseLetters);
            }

            if (allowedCharacters.HasFlag(AllowedCharacters.Digits))
            {
                allowedCharslist.AddRange(Digits);
            }

            if (allowedCharacters.HasFlag(AllowedCharacters.SpecialChars))
            {
                allowedCharslist.AddRange(SpecialReadableAsciiLetters);
            }

            if (allowedCharacters.HasFlag(AllowedCharacters.Minus))
            {
                allowedCharslist.AddRange(Minus);
            }

            if (allowedCharacters.HasFlag(AllowedCharacters.Underscore))
            {
                allowedCharslist.AddRange(Underscore);
            }

            if (allowedCharacters.HasFlag(AllowedCharacters.Space))
            {
                allowedCharslist.AddRange(Space);
            }

            if (allowedCharacters.HasFlag(AllowedCharacters.Brackets))
            {
                allowedCharslist.AddRange(Brackets);
            }

            if (ExcludeSimilarChars)
            {
                allowedCharslist.RemoveAll(ch => SimilarLookingCharacters.Contains(ch));
            }

            return allowedCharslist.ToArray();
        }

        #endregion
    }
}
