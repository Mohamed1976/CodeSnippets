using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;


//internalsvisibleto strong named assembly
//https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.internalsvisibletoattribute?view=net-5.0
//https://stackoverflow.com/questions/30943342/how-to-use-internalsvisibleto-attribute-with-strongly-named-assembly
//TODO [assembly: InternalsVisibleTo("RandomStringTests")]
/*
C:\>"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\"sn -p sgKey.snk public.key

Microsoft (R) .NET Framework Strong Name Utility  Version 4.0.30319.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Public key written to public.key

C:\Users\moham\Desktop\Temp>sn -tp public.key
'sn' is not recognized as an internal or external command,
operable program or batch file.

C:\>"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\"sn -tp public.key

Microsoft (R) .NET Framework Strong Name Utility  Version 4.0.30319.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Public key (hash algorithm: sha1):
00240000048000009400000006020000002400005253413100040000010001005d365e38746725
c75fa05e7d079568082b2641ba9ab209bb41dee8ea27344ee658b3cfd29218942302cf6b80c3f8
20718e6c1ff49b6b1e1bba4e77c87e32637b535671145e1c53f53dc57b5b91be25474f0dd03489
0a2f084b051ea7d7f3f0a32120d7191502d7d383d384b46c1aee7f9e653b60cf8a024985aa8bbe
4db392cb

Public key token is 5841b1cd22da1f03

*/

//[assembly: InternalsVisibleTo("RandomStringTests")]
[assembly: InternalsVisibleTo("RandomStringTests, PublicKey=0024000004800000940000000602000000240000" +
                                                           "5253413100040000010001005d365e38746725" +
                                                            "c75fa05e7d079568082b2641ba9ab209bb41dee8ea27344ee658b3cfd29218942302cf6b80c3f8" +
                                                            "20718e6c1ff49b6b1e1bba4e77c87e32637b535671145e1c53f53dc57b5b91be25474f0dd03489" +
                                                            "0a2f084b051ea7d7f3f0a32120d7191502d7d383d384b46c1aee7f9e653b60cf8a024985aa8bbe" +
                                                            "4db392cb")]
namespace RandomString
{
    #region [ Enums ]

    public enum RandomGeneratorType
    {
        PseudoRandomGenerator,
        SecureRandomGenerator
    }

    /// <summary>
    /// Charsets can be composed of the following characters:  
    /// Charsets are named by strings composed of the following characters:
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

    #endregion

    //google: xunit access internal class
    //internal static class Common
    internal static class Common
    {
        #region [ Constants ]

        public const int MinStringLength = 1;
        public const int MaxStringLength = 1000;
        public const int MinNrOfChars = 2;

        #endregion

        #region [ Error Codes ]

        /* String length parameter outside of expected range. */
        public static readonly string STRING_LENGTH_EXCEEDS_RANGE = $"Please enter a valid string length between " +
            $"{MinStringLength} and {MaxStringLength}.";

        /* In order to generate a random string, the character array must contain at least two 
         * distinct characters. */
        public const string NO_CHARS_SPECIFIED = "Please provide at least two distinct characters " +
            "in the allowedCharacters array parameter.";

        /* In order to generate a random string, at least one charset must be specified in the 
         * allowedCharacters parameter. */
        public const string NO_CHARSET_SPECIFIED = "Please specify at least one character " +
            "set in the allowedCharacters parameter.";

        /* All minimum required character parameters must be zero or greater than zero. */
        public const string MINIMUM_PARAMETER_IS_NEGATIVE = "At least one of the following parameters must " +
            "be zero or greater than zero: minUpperCaseLetters, minLowerCaseLetters, minDigits, minSpecialChars.";

        /* The minimum number of requested characters exceeds the string length parameter. */
        public const string MINIMUM_PARAMETER_EXCEEDS_LENGTH = "The minimum number of characters " +
            "requested must be equal or smaller than the specified string length parameter.";

        #endregion
    }
}
