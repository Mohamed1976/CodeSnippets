using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleWithDI.RandomString
{
    /// <summary>
    /// A shortcut for selecting specific groups of allowed characters.
    /// </summary>
    [Flags]
    public enum AllowedCharacters : byte
    {
        /// <summary>
        /// No character
        /// </summary>
        None = 0x00,
        /// <summary>
        /// Latin upper-case characters A,B,C ... (Count: 26)
        /// </summary>
        UpperCaseLetters = 0x01,
        /// <summary>
        /// Latin lower-case characters a,b,c ... (Count: 26)
        /// </summary>
        LowerCaseLetters = 0x02,
        /// <summary>
        /// Digits 0,1,2,3 ... (Count: 10)
        /// </summary>
        Digits = 0x04,
        /// <summary>
        /// All readable special ascii characters - '!', '&quot;', '#', '$', '%', '&amp;', ''', '*', '+', ',', '.', '/', ':', ';', '=', '?', '@', '\', '^', '´', '`', '|', '~' (Count: 23)
        /// </summary>
        SpecialChars = 0x08,
        /// <summary>
        /// The minus ('-') character (Count: 1)
        /// </summary>
        Minus = 0x10,
        /// <summary>
        /// The underscore ('_') character (Count: 1)
        /// </summary>        
        Underscore = 0x20,
        /// <summary>
        /// The space (' ') character (Count: 1)
        /// </summary>
        Space = 0x40,
        /// <summary>
        /// All bracket characters '&lt;', '&gt;', '{', '}', '[', ']', '(', ')' (Count: 8)
        /// </summary>
        Brackets = 0x80,
        /// <summary>
        /// All characters specified in AllowedCharacters (Count: xx)
        /// </summary>
        All = 0xFF
    }

    public static class Charset
    {
        public static readonly char[] UpperCaseLetters =
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H',
            'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
            'Q', 'R', 'S', 'T', 'U', 'W', 'X', 'Y', 'Z'
        };

        public static readonly char[] LowerCaseLetters =
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h',
            'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
            'q', 'r', 's', 't', 'u', 'w', 'x', 'y', 'z'
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

        public static char[] GetCharacters(AllowedCharacters allowedChars, bool ExcludeSimilarChars = false)
        {
            List<char> allowedCharslist = new List<char>();

            if (allowedChars == AllowedCharacters.None)
            {
                throw new ArgumentOutOfRangeException(nameof(allowedChars),
                    "Please make sure you select at least one character set.");
            }

            if (allowedChars.HasFlag(AllowedCharacters.UpperCaseLetters))
            {
                allowedCharslist.AddRange(UpperCaseLetters);
            }

            if (allowedChars.HasFlag(AllowedCharacters.LowerCaseLetters))
            {
                allowedCharslist.AddRange(LowerCaseLetters);
            }

            if (allowedChars.HasFlag(AllowedCharacters.Digits))
            {
                allowedCharslist.AddRange(Digits);
            }

            if (allowedChars.HasFlag(AllowedCharacters.SpecialChars))
            {
                allowedCharslist.AddRange(SpecialReadableAsciiLetters);
            }

            if (allowedChars.HasFlag(AllowedCharacters.Minus))
            {
                allowedCharslist.AddRange(Minus);
            }

            if (allowedChars.HasFlag(AllowedCharacters.Underscore))
            {
                allowedCharslist.AddRange(Underscore);
            }

            if (allowedChars.HasFlag(AllowedCharacters.Space))
            {
                allowedCharslist.AddRange(Space);
            }

            if (allowedChars.HasFlag(AllowedCharacters.Brackets))
            {
                allowedCharslist.AddRange(Brackets);
            }

            if(ExcludeSimilarChars)
            {
                allowedCharslist.RemoveAll(ch => SimilarLookingCharacters.Contains(ch));
            }

            return allowedCharslist.ToArray();
        }
    }
}
