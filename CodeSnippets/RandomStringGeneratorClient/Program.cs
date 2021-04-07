using RandomStringGeneratorClient.Exercises;
using System;
using System.Threading.Tasks;

namespace RandomStringGeneratorClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            RandomStringExamples randomStringExamples = new RandomStringExamples();
            await randomStringExamples.Run();
            Console.WriteLine("Hello World!");
        }
    }
}
