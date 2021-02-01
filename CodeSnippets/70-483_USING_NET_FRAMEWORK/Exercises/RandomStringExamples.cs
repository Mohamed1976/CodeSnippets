using RandomString;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _70_483_USING_NET_FRAMEWORK.Exercises
{
    public class RandomStringExamples
    {

        public async Task Run()
        {
            Console.WriteLine("Entering RandomStringExamples Run()");
            //GenerateString();
            //ExceptionHandling();
            ValidationExample();

            await Task.Delay(100);
        }

        public static readonly char[] Digits =
        {
            '1','2','3','4','5','6','7','8','9','0'
        };

        private void ValidationExample()
        {
            bool valid = Digits.All((ch) =>
            {
                Console.WriteLine(ch);
                return Digits.Contains(ch);
            });

            Console.WriteLine("valid: " + valid);
        }

        private void ExceptionHandling()
        {
            try
            {
                RandomStringGenerator randomStringGenerator = new RandomStringGenerator(RandomGeneratorType.SecureRandomGenerator);
                randomStringGenerator.Next(AllowedCharacters.None, 20, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}.");
            }

            try
            {
                RandomStringGenerator randomStringGenerator = new RandomStringGenerator(RandomGeneratorType.SecureRandomGenerator);
                randomStringGenerator.Next(AllowedCharacters.All, 0, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}.");
            }

            try
            {
                RandomStringGenerator randomStringGenerator = new RandomStringGenerator(RandomGeneratorType.SecureRandomGenerator);
                randomStringGenerator.Next(AllowedCharacters.All, 1001, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}.");
            }
        }

        private void GenerateString()
        {
            string pwd = default;
            char[] charset = { 'A', 'A', 'b', '*', '-', 'c', 'F', 'G', 'h', 'k', 'K', '&', '$', '@', '!', '4', '7' };
            RandomStringGenerator randomStringGenerator = new RandomStringGenerator();
            for (int i = 0; i < 100; i++)
            {
                pwd = randomStringGenerator.Next(charset, 20);
                Console.WriteLine(pwd);
            }

            Console.WriteLine("\n\n");
            for (int i = 0; i < 100; i++)
            {
                pwd = randomStringGenerator.Next(AllowedCharacters.All, 20, false);
                Console.WriteLine(pwd);
            }

            Console.WriteLine("\n\n");
            for (int i = 0; i < 100; i++)
            {
                pwd = randomStringGenerator.Next(AllowedCharacters.LowerCaseLetters |
                    AllowedCharacters.UpperCaseLetters |
                    AllowedCharacters.Digits, 20, false);
                Console.WriteLine(pwd);
            }

            Console.WriteLine("\n\n");
            for (int i = 0; i < 100; i++)
            {
                pwd = randomStringGenerator.Next(5, 5, 10, 0, 20, false);
                Console.WriteLine(pwd);
            }
        }
    }
}
