using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

//References:
//https://www.codeproject.com/Articles/5246285/WinForm-Generate-Password-Tool
//https://stackoverflow.com/questions/54991/generating-random-passwords

//Notes
//Use the RNGCryptoServiceProvider class if you need a strong random number generator.

namespace ConsoleWithDI
{
    public class RandomStringServiceV3 : IRandomStringService
    {
        private readonly ILogger<RandomStringServiceV3> logger;
        private readonly IConfiguration config;

        public RandomStringServiceV3(ILogger<RandomStringServiceV3> logger, IConfiguration config)
        {
            this.logger = logger;
            this.config = config;
        }

        public string GenerateRandomString(int stringSize = 20, CharSet charSet = CharSet.All)
        {
            const string DIGITS = "0123456789";
            const string LOWERCASE = "abcdefghijklmnopqrstuvwxyz";
            const string SPECIAL = @"!@#$%^&*()+=~[:'<>?,.|";
            const string UPPERCASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            // Make a string containing all allowed characters.
            string allowed = default;
            StringBuilder sb = new StringBuilder();
            byte[] bytes = new byte[stringSize];

            if ((charSet & CharSet.LowerCase) == CharSet.LowerCase)
            {
                allowed += LOWERCASE;
            }

            if ((charSet & CharSet.UpperCase) == CharSet.UpperCase)
            {
                allowed += UPPERCASE;
            }

            if ((charSet & CharSet.Digits) == CharSet.Digits)
            {
                allowed += DIGITS;
            }

            if ((charSet & CharSet.Special) == CharSet.Special)
            {
                allowed += SPECIAL;
            }

            new RNGCryptoServiceProvider().GetBytes(bytes);

            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(allowed[bytes[i] % allowed.Length]);
            }

            return sb.ToString();
        }
    }
}
