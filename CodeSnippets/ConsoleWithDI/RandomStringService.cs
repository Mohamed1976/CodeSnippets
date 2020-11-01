using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleWithDI
{
    public class RandomStringService : IRandomStringService
    {
        private readonly ILogger<RandomStringService> logger;
        private readonly IConfiguration config;

        public RandomStringService(ILogger<RandomStringService> logger, IConfiguration config)
        {
            this.logger = logger;
            this.config = config;
        }

        private const string ALPHA = "ABCDEFGHIJ";
        private const string alpha = "abcdefghij";
        private const string digits = "1234567890";
        private const string beta = "@#!~&*()?/";

        public string GenerateRandomString(int stringSize, CharSet charSet)
        {
            //logger.LogInformation("Entering GenerateRandomString().");
            int val = config.GetValue<int>("StringLength");

            string result = default;
            string charCollection = ALPHA + alpha + digits + beta;
            int size = charCollection.Length;
            Random _random = new Random();

            for (int i = 0; i < 20; i++)
            {
                result += charCollection[_random.Next(size)];
            }

            return result;
        }
    }
}
