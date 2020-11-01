using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace ConsoleWithDI
{
    public class RandomStringServiceV2 : IRandomStringService
    {
        private readonly ILogger<RandomStringServiceV2> logger;
        private readonly IConfiguration config;

        public RandomStringServiceV2(ILogger<RandomStringServiceV2> logger, IConfiguration config)
        {
            this.logger = logger;
            this.config = config;
        }

        public string GenerateRandomString(int stringSize, CharSet charSet)
        {
            Random Rand = new Random();

            const string lowers = "abcdefghijklmnopqrstuvwxyz";
            const string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            const string specials = @"~!@#$%^&*():;[]{}<>,.?/\|";

            // Make a string containing all allowed characters.
            string allowed = default(string);
            string randomString = default(string);

            if ((charSet & CharSet.LowerCase) == CharSet.LowerCase)
            {
                allowed += lowers;
            }

            if ((charSet & CharSet.UpperCase) == CharSet.UpperCase)
            {
                allowed += uppers;
            }

            if ((charSet & CharSet.Digits) == CharSet.Digits)
            {
                allowed += digits;
            }

            if ((charSet & CharSet.Special) == CharSet.Special)
            {
                allowed += specials;
            }

            int arrSize = allowed.Length;
            logger.LogInformation("arrSize : {arrSize}", arrSize);
            for (int i = 0; i < stringSize; i++)
            {
                randomString += allowed[Rand.Next(arrSize)];
            }

            return randomString;
        }
    }
}
