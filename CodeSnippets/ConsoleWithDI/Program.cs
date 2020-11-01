using ConsoleWithDI.RandomGenerator;
using ConsoleWithDI.RandomString;
using ConsoleWithDI.RandomValidator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

//https://www.youtube.com/watch?v=GAOCe-2nXqc
namespace ConsoleWithDI
{
    class Program
    {
        private static IHost _host = null;
        static void Main(string[] args)
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            BuildConfig(builder);

            //Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger.Information("Application starting.");

            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    //services.AddTransient<IRandomStringService, RandomStringService>();
                    //services.AddTransient<IRandomStringService, RandomStringServiceV2>();
                    //services.AddTransient<IRandomStringService, RandomStringServiceV3>();
                    //services.AddTransient<IRandomStringService, RandomStringServiceV4>();                    
                })
                .UseSerilog()
                .Build();

            string pwd = default;
            char[] charset = { 'A', 'A', 'b', '*', '-', 'c', 'F', 'G', 'h', 'k', 'K', '&', '$', '@', '!', '4', '7' }; 
            RandomStringGenerator randomStringGenerator = new RandomStringGenerator();
            for(int i = 0; i < 100; i++)
            {
                pwd = randomStringGenerator.GenerateString(charset, 20);
                Console.WriteLine(pwd);
            }


            //Console.WriteLine($" {charset.Length} == {(charset.Distinct()).Count()}") ;

            Console.ReadLine();
            return;
            /*chars explained 
            https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/char 
            https://docs.microsoft.com/en-us/dotnet/api/system.char?view=netcore-3.1
            https://docs.microsoft.com/en-us/dotnet/standard/base-types/character-encoding-introduction

            //Change the code pages of the console 
            https://stackoverflow.com/questions/38533903/set-c-sharp-console-application-to-unicode-output
            https://docs.microsoft.com/en-us/windows-server/administration/windows-commands/chcp
            //Char glyphes
            http://www.fileformat.info/info/charset/UTF-16/list.htm
            If your keyboard has a number pad on the right hand side (or your laptop keyboard has a number lock) 
            you can use Alt codes to get characters you wouldn't find normally. Hold down Alt and type 0128 to 
            get a Euro key to appear.

            $	dollar	United States (USD), Canada (CAD), Australia (AUD), etc.
            €	euro	Eurozone (EUR)
            ¢	cent	1/100 of a dollar or euro
            £	pound	United Kingdom (GBP)
            ¥	yen	Japan (JPY)
            ₿	bitcoin	cryptocurrency (XBT, BTC)
            ฿	baht	Thailand (THB)
            ₫	dong	Vietnam (VND)
            ₴	hryvnia	Ukraine (UAH)
            ₪	shekel	Israel (ILS)
            ₽	ruble	Russia (RUB)
            ₹	rupee	India (INR)
            ₩	won	South Korea (KRW)
            */
            //UTF-16 <BOM>
            int SmallestCodePoint = 0x00000000;
            int LargestCodePoint  = 0x0000FFFF;
            char[] chars = { '\u0061', '\u0308', 'A', 'a', '1', '$', '€', '¢', '£', '¥', '₿', '฿', '₫', '₴', '₪', '₽', '₹', '₩' };
            /*
            Glyphs, which may consist of a single character or of a base character followed by one or more 
            combining characters. For example, the character ä is represented by a Char object whose code 
            unit is U+0061 followed by a Char object whose code unit is U+0308. (The character ä can also 
            be defined by a single Char object that has a code unit of U+00E4.) The following example 
            illustrates that the character ä consists of two Char objects.
            */
            string combining = "\u0061\u0308"; //prints ä
            Console.WriteLine($"{combining} == ä");
            for (int i = 0; i < chars.Length; i++)
                Console.Write($"{chars[i]} ");
            
            //Prints all UTF-16 characters 
            for (int i = SmallestCodePoint ; i < LargestCodePoint; i++)
            {
                if (Convert.ToChar(i) != '?')
                {
                    Console.Write("U+{0:X4} == {1}", Convert.ToUInt16(i), Convert.ToChar(i));
                    if (i % 10 == 0)
                        Console.WriteLine();
                    else
                        Console.Write(" ");
                }
            }

            //Nuget project to generate random strings
            //RandomString.RandomGenerator randomGenerator = new RandomString.RandomGenerator();


            Console.ReadLine();
            return;


            //GenerateRandomStrings();
            //GenerateRandomStringsV2();
            //GenerateRandomStringsV3();

            //ClosureExample();
            //ClosureExampleV2();

            /*Validation of Random values 
             * 
             * Test the derivted distributions by looking at their means and variances. TestDistributions();
            If this test were applied repeatedly with ideal random input, the test would fail on average once 
            in every thousand applications. This is highly unusual in software testing: the test should fail 
            occasionally! That's statistics for you. Don't be alarmed if the test fails. Try again with another 
            seed and it will most likely pass. The test is good enough to catch most coding errors since a bug 
            would likely result in the test failing far more often. The test code also uses RunningStat, a class 
            for accurately computing sample mean and variance as values accumulate. */
            for (int i = 0; i < 10000; i++)
            {
                KSTest();
            }

            /*random.NextDouble();                                  => passedCounter: 9994, failedCounter: 6
                                                                    => passedCounter: 9991, failedCounter: 9
                                                                    => passedCounter: 9991, failedCounter: 9            
                                                                    => passedCounter: 9989, failedCounter: 11
                                                                    => passedCounter: 9994, failedCounter: 6
            
            SimpleRNG.GetUniform();                                 => passedCounter: 9995, failedCounter: 5
                                                                    => passedCounter: 9991, failedCounter: 9
                                                                    => passedCounter: 9991, failedCounter: 9
                                                                    => passedCounter: 9997, failedCounter: 3
                                                                    => passedCounter: 9991, failedCounter: 9

            ((double)SimpleRNG.GetUint()) / uint.MaxValue;          => passedCounter: 9992, failedCounter: 8
                                                                    => passedCounter: 9994, failedCounter: 6
                                                                    => passedCounter: 9995, failedCounter: 5
                                                                    => passedCounter: 9991, failedCounter: 9
                                                                    => passedCounter: 9991, failedCounter: 9
            MaxValue == 74
            ((double)(SimpleRNG.GetUint() % MaxValue)) / MaxValue;  => passedCounter: 9835, failedCounter: 165
                                                                    => passedCounter: 9824, failedCounter: 176
                                                                    => passedCounter: 9802, failedCounter: 198
                                                                    => passedCounter: 9828, failedCounter: 172
                                                                    => passedCounter: 9825, failedCounter: 175
            (MaxValue == 100)
            ((double)random.Next(MaxValue)) / MaxValue;             => passedCounter: 9826, failedCounter: 174  
                                                                    => passedCounter: 9852, failedCounter: 148
                                                                    => passedCounter: 9832, failedCounter: 168
                                                                    => passedCounter: 9850, failedCounter: 150
                                                                    => passedCounter: 9855, failedCounter: 145
            (MaxValue == 150)
                                                                    => passedCounter: 9925, failedCounter: 75
                                                                    => passedCounter: 9908, failedCounter: 92
                                                                    => passedCounter: 9920, failedCounter: 80
                                                                    => passedCounter: 9933, failedCounter: 67
                                                                    => passedCounter: 9932, failedCounter: 68
            (MaxValue == 100)
            ((double)randomGenerator.Next(MaxValue)) / MaxValue;    => passedCounter: 9858, failedCounter: 142
                                                                    => passedCounter: 9853, failedCounter: 147        
                                                                    => passedCounter: 9848, failedCounter: 152
                                                                    => passedCounter: 9835, failedCounter: 165
                                                                    => passedCounter: 9852, failedCounter: 148
            (MaxValue == 150)
                                                                    => passedCounter: 9930, failedCounter: 70
                                                                    => passedCounter: 9915, failedCounter: 85
                                                                    => passedCounter: 9931, failedCounter: 69
                                                                    => passedCounter: 9919, failedCounter: 81
                                                                    => passedCounter: 9922, failedCounter: 78
            (MaxValue == 74)
            ((double)randomGenerator.Next(MaxValue)) / MaxValue;
                                                                    => passedCounter: 9824, failedCounter: 176                                                                          
                                                                    => passedCounter: 9815, failedCounter: 185
                                                                    => passedCounter: 9819, failedCounter: 181
                                                                    => passedCounter: 9810, failedCounter: 190
                                                                    => passedCounter: 9821, failedCounter: 179
             
            ((double)randomGenerator.Next(int.MaxValue)) / int.MaxValue;
                                                                    => passedCounter: 9993, failedCounter: 7      
                                                                    => passedCounter: 9992, failedCounter: 8      
                                                                    => passedCounter: 9998, failedCounter: 2
                                                                    => passedCounter: 9992, failedCounter: 8
                                                                    => passedCounter: 9987, failedCounter: 13     

             */

            Console.WriteLine($"passedCounter: {passedCounter}, failedCounter: {failedCounter}");         

            Console.ReadLine();
        }

        static int passedCounter = 0;
        static int failedCounter = 0;

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true);
        }

        private static void GenerateRandomStrings()
        {            
            IRandomStringService service = _host.Services.GetRequiredService<IRandomStringService>();
            //ActivatorUtilities.CreateInstance, Will allow for more flexibility in the constructor call (you can pass in additional arguments, etc)
            //RandomStringService service = ActivatorUtilities.CreateInstance<RandomStringService>(host.Services);
            for (int i = 0; i < 1000; i++)
            {
                string rand = service.GenerateRandomString(charSet: CharSet.All, stringSize: 100);
                Console.WriteLine(rand);
            }
        }

        private static void GenerateRandomStringsV2()
        {
            IRandomStringService service = _host.Services.GetRequiredService<IRandomStringService>();
            //ActivatorUtilities.CreateInstance, Will allow for more flexibility in the constructor call (you can pass in additional arguments, etc)
            //RandomStringService service = ActivatorUtilities.CreateInstance<RandomStringService>(host.Services);
            for (int i = 0; i < 1000; i++)
            {
                string rand = service.GenerateRandomString(20, 5, 5, 5, 5, CharSet.All);
                Console.WriteLine(rand);
            }
        }

        private static void GenerateRandomStringsV3()
        {
            RandomString.RandomGenerator randomGenerator = new RandomString.RandomGenerator();
            List<int> _randomInts = new List<int>();
            for (int i = 0; i < 10000000; i++)
            {
                int val = randomGenerator.Next(0, 100);
                _randomInts.Add(val);
                //Console.WriteLine($"{i} => {val}");
            }

            CheckDistribution(_randomInts);

            return;
            List<int> randomInts = new List<int>();
            for (int i = 0; i < 1000; i++)
            {
                int val = randomGenerator.Next(); // (0, 100);
                randomInts.Add(val);
                Console.WriteLine($"{i} => {val}");
            }

            BitArray bitArray = new BitArray(randomInts.ToArray());
            ShowBitArray(bitArray, 4, 52);
            Console.WriteLine("1. Testing input frequencies");
            double pFreq = FrequencyTest(bitArray);
            Console.WriteLine($"Frequency: {pFreq:N6}");
            if (pFreq < 0.01)
                Console.WriteLine("There is evidence that sequence is NOT random");
            else
                Console.WriteLine("Sequence passes NIST frequency test for randomness");

            string bitString = "1100 0011 1010 1110 0000 1111 0000 1111 0000 1111 0000 1111 0000";
            BitArray _bitArray = MakeBitArray(bitString);
            Console.WriteLine("Input sequence to test for randomness: \n");
            ShowBitArray(_bitArray, 4, 52);
            Console.WriteLine("1. Testing input frequencies");
            pFreq = FrequencyTest(_bitArray);
            Console.WriteLine("pValue for Frequency test = " + pFreq.ToString("F4"));
            if (pFreq < 0.01)
                Console.WriteLine("There is evidence that sequence is NOT random");
            else
                Console.WriteLine("Sequence passes NIST frequency test for randomness");

            return;
            char[] chars = Charset.GetCharacters(AllowedCharacters.UpperCaseLetters |
                AllowedCharacters.LowerCaseLetters | AllowedCharacters.Digits);
            Console.WriteLine(new string(chars));

            chars = Charset.GetCharacters(AllowedCharacters.UpperCaseLetters |
                AllowedCharacters.LowerCaseLetters | AllowedCharacters.Digits, ExcludeSimilarChars:true);
            Console.WriteLine(new string(chars));

            chars = Charset.GetCharacters(AllowedCharacters.LowerCaseLetters);
            Console.WriteLine(new string(chars));

            chars = Charset.GetCharacters(AllowedCharacters.Digits);
            Console.WriteLine(new string(chars));

            chars = Charset.GetCharacters(AllowedCharacters.Brackets);
            Console.WriteLine(new string(chars));

            chars = Charset.GetCharacters(AllowedCharacters.Space);
            Console.WriteLine(new string(chars));
            
            chars = Charset.GetCharacters(AllowedCharacters.Specials);
            Console.WriteLine(new string(chars));
            
            chars = Charset.GetCharacters(AllowedCharacters.Minus);
            Console.WriteLine(new string(chars));

            chars = Charset.GetCharacters(AllowedCharacters.Underscore);
            Console.WriteLine(new string(chars));
        }

        private static void CheckDistribution(List<int> randomInts)
        {
            Dictionary<int, int> intDistribution = new Dictionary<int, int>();
            int minFrequency = -1;
            int maxFrequency = -1;


            for (int i = 0; i < randomInts.Count; i++)
            {
                if(intDistribution.ContainsKey(randomInts[i]))
                {
                    intDistribution[randomInts[i]]++;
                }
                else
                {
                    intDistribution.Add(randomInts[i], 1);
                }
            }

            foreach(var pair in intDistribution.OrderBy(pair => pair.Key))
            {
                if (minFrequency == -1 || maxFrequency == -1)
                {
                    minFrequency = pair.Value;
                    maxFrequency = pair.Value;
                }
                else
                {
                    if (minFrequency > pair.Value)
                        minFrequency = pair.Value;

                    if (maxFrequency < pair.Value)
                        maxFrequency = pair.Value;
                }

                Console.WriteLine($"{pair.Key} => {pair.Value}({(((double)pair.Value / randomInts.Count) * 100).ToString("F6")})");
            }
            
            Console.WriteLine($"maxFrequency: {maxFrequency} ");
            Console.WriteLine($"minFrequency: {minFrequency} ");
        }

        //Test Run - Implementing the National Institute of Standards and Technology Tests of Randomness Using C#
        //https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/government-special-issue/test-run-implementing-the-national-institute-of-standards-and-technology-tests-of-randomness-using-csharp
        //https://docs.microsoft.com/en-us/dotnet/api/system.collections.bitarray?view=netcore-3.1
        //https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.rngcryptoserviceprovider?view=netcore-3.1
        static void ShowBitArray(BitArray bitArray, int blockSize, int lineSize)
        {
            for (int i = 0; i < bitArray.Length; ++i)
            {
                if (i > 0 && i % blockSize == 0) Console.Write(" ");
                if (i > 0 && i % lineSize == 0) Console.WriteLine("");
                if (bitArray[i] == false) Console.Write("0");
                else Console.Write("1");
            }
            Console.WriteLine("");
        }

        static double FrequencyTest(BitArray bitArray)
        {
            double sum = 0;
            for (int i = 0; i < bitArray.Length; ++i)
            {
                if (bitArray[i] == false) sum = sum - 1;
                else sum = sum + 1;
            }
            double testStat = Math.Abs(sum) / Math.Sqrt(bitArray.Length);
            double rootTwo = 1.414213562373095;
            double pValue = ErrorFunctionComplement(testStat / rootTwo);
            return pValue;
        }

        static double ErrorFunctionComplement(double x)
        {
            return 1 - ErrorFunction(x);
        }

        static double ErrorFunction(double x)
        {
            double p = 0.3275911;
            double a1 = 0.254829592;
            double a2 = -0.284496736;
            double a3 = 1.421413741;
            double a4 = -1.453152027;
            double a5 = 1.061405429;
            double t = 1.0 / (1.0 + p * x);
            double err = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);
            return err;
        }

        static BitArray MakeBitArray(string bitString)
        {
            int size = 0;
            for (int i = 0; i < bitString.Length; ++i)
                if (bitString[i] != ' ') ++size;
            BitArray result = new BitArray(size);
            int k = 0; // ptr into result
            for (int i = 0; i < bitString.Length; ++i)
            {
                if (bitString[i] == ' ') continue;
                if (bitString[i] == '1') result[k] = true;
                else result[k] = false;
                ++k;
            }
            return result;
        }

        private static void ClosureExample()
        {
            Console.WriteLine("Linear Function Plot.");
            Exercises.Math math = new Exercises.Math();
            Exercises.Function aLinearFunction = math.GetLinearFunction(2, 4);
            math.PlotFunction(aLinearFunction, -25, 25, 1);

            Console.WriteLine("Quadratic Function Plot.");
            Exercises.Function aQuadraticFunction = math.GetQuadraticFunction(3, 0, 0);
            math.PlotFunction(aQuadraticFunction, -25, 25, 1);
        }

        private static void ClosureExampleV2()
        {
            Console.WriteLine("Linear Function Plot V2.");
            Exercises.Math math = new Exercises.Math();
            Func<double,double> func = math.GetLinearFunctionV2(2, 4);
            math.PlotFunctionV2(func, -25, 25, 1);

            Console.WriteLine("Quadratic Function Plot V2.");
            func = math.GetQuadraticFunctionV2(3, 0, 0);
            math.PlotFunctionV2(func, -25, 25, 1);
        }

        // Verify that distributions have the correct mean and variance.
        // Note that sample mean and sample variance will not exactly match the expected mean and variance.
        static void TestDistributions()
        {
            const int numSamples = 100000;
            double mean, variance, stdev, shape, scale, degreesOfFreedom;
            NumberValidator rs = new NumberValidator();

            // Gamma distribution
            rs.Clear();
            shape = 10; scale = 2;
            for (int i = 0; i < numSamples; ++i)
                rs.Push(SimpleRNG.GetGamma(shape, scale));
            PrintResults("gamma", shape * scale, shape * scale * scale, rs.Mean(), rs.Variance());

            // Normal distribution
            rs.Clear();
            mean = 2; stdev = 5;
            for (int i = 0; i < numSamples; ++i)
                rs.Push(SimpleRNG.GetNormal(2, 5));
            PrintResults("normal", mean, stdev * stdev, rs.Mean(), rs.Variance());

            // Student t distribution
            rs.Clear();
            degreesOfFreedom = 6;
            for (int i = 0; i < numSamples; ++i)
                rs.Push(SimpleRNG.GetStudentT(6));
            PrintResults("Student t", 0, degreesOfFreedom / (degreesOfFreedom - 2.0), rs.Mean(), rs.Variance());

            // Weibull distribution
            rs.Clear();
            shape = 2; scale = 3;
            mean = 3 * Math.Sqrt(Math.PI) / 2;
            variance = 9 * (1 - Math.PI / 4);
            for (int i = 0; i < numSamples; ++i)
                rs.Push(SimpleRNG.GetWeibull(shape, scale));
            PrintResults("Weibull", mean, variance, rs.Mean(), rs.Variance());

            // Beta distribution
            rs.Clear();
            double a = 7, b = 2;
            mean = a / (a + b);
            variance = mean * (1 - mean) / (a + b + 1);
            for (int i = 0; i < numSamples; ++i)
                rs.Push(SimpleRNG.GetBeta(a, b));
            PrintResults("Beta", mean, variance, rs.Mean(), rs.Variance());
        }

        // Convenience function for TestDistributions()
        static void PrintResults
        (
            string name,
            double expectedMean,
            double expectedVariance,
            double computedMean,
            double computedVariance
        )
        {
            Console.WriteLine("Testing {0}", name);
            Console.WriteLine("Expected mean:     {0}, computed mean:     {1}", expectedMean, computedMean);
            Console.WriteLine("Expected variance: {0}, computed variance: {1}", expectedVariance, computedVariance);
            Console.WriteLine("");
        }

        /// <summary>
        /// Test application for the SimpleRNG random number generator.
        /// This verifies that the random numbers have the expected 
        /// distribution using a standard statistical test.
        /// Unfortunately the test is more complicated than the generator itself.
        /// 
        /// For more information on testing random number generators, see
        /// chapter 10 of Beautiful Testing by Tim Riley and Adam Goucher.
        /// </summary>
        static void KSTest()
        {
            RandomString.RandomGenerator randomGenerator = new RandomString.RandomGenerator();
            const int MaxValue = 74;

            /// Kolmogorov-Smirnov test for distributions.  See Knuth volume 2, page 48-51 (third edition).
            /// This test should *fail* on average one time in 1000 runs.
            /// That's life with random number generators: if the test passed all the time, 
            /// the source wouldn't be random enough!  If the test were to fail more frequently,
            /// the most likely explanation would be a bug in the code.

            SimpleRNG.SetSeedFromSystemTime();
            Random random = new Random();

            int numReps = 1000;
            double failureProbability = 0.001; // probability of test failing with normal input
            int j;
            double[] samples = new double[numReps];

            for (j = 0; j != numReps; ++j)
                samples[j] = ((double)randomGenerator.Next(int.MaxValue)) / int.MaxValue;


            //((double)randomGenerator.Next(MaxValue)) / MaxValue;
            //((double)(SimpleRNG.GetUint() % MaxValue)) / MaxValue;
            //((double)SimpleRNG.GetUint()) / uint.MaxValue;
            //random.NextDouble();
            //SimpleRNG.GetUniform();
            //((double)random.Next(MaxValue)) / MaxValue;
            //((double)randomGenerator.Next(MaxValue)) / MaxValue;

            System.Array.Sort(samples);

            double CDF;
            double temp;
            int j_minus = 0, j_plus = 0;
            double K_plus = -double.MaxValue;
            double K_minus = -double.MaxValue;

            for (j = 0; j != numReps; ++j)
            {
                CDF = samples[j];
                temp = (j + 1.0) / numReps - CDF;
                if (K_plus < temp)
                {
                    K_plus = temp;
                    j_plus = j;
                }
                temp = CDF - (j + 0.0) / numReps;
                if (K_minus < temp)
                {
                    K_minus = temp;
                    j_minus = j;
                }
            }

            double sqrtNumReps = Math.Sqrt((double)numReps);
            K_plus *= sqrtNumReps;
            K_minus *= sqrtNumReps;

            // We divide the failure probability by four because we have four tests:
            // left and right tests for K+ and K-.
            double p_low = 0.25 * failureProbability;
            double p_high = 1.0 - 0.25 * failureProbability;
            double cutoff_low = Math.Sqrt(0.5 * Math.Log(1.0 / (1.0 - p_low))) - 1.0 / (6.0 * sqrtNumReps);
            double cutoff_high = Math.Sqrt(0.5 * Math.Log(1.0 / (1.0 - p_high))) - 1.0 / (6.0 * sqrtNumReps);

            Console.WriteLine("\n\nTesting the random number distribution");
            Console.WriteLine("using the Kolmogorov-Smirnov (KS) test.\n");

            Console.WriteLine("K+ statistic: {0}", K_plus);
            Console.WriteLine("K+ statistic: {0}", K_minus);
            Console.WriteLine("Acceptable interval: [{0}, {1}]", cutoff_low, cutoff_high);
            Console.WriteLine("K+ max at {0} {1}", j_plus, samples[j_plus]);
            Console.WriteLine("K- max at {0} {1}", j_minus, samples[j_minus]);

            if (cutoff_low <= K_plus && K_plus <= cutoff_high && cutoff_low <= K_minus && K_minus <= cutoff_high)
            {
                Console.WriteLine("\nKS test passed\n");
                passedCounter++;
            }
                
            else
            {
                Console.WriteLine("\nKS test failed\n");
                failedCounter++;
            }
        }
    }
}
