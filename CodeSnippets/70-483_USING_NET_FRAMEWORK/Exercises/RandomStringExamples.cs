using RandomString;
using RandomString.RandomGenerators;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _70_483_USING_NET_FRAMEWORK.Exercises
{
    public class RandomStringExamples
    {
        public async Task Run()
        {
            Console.WriteLine("Entering RandomStringExamples Run()");
            //UInt32.MaxValue;
            //UInt32.MinValue;
            //Int32.MaxValue;
            //Int32.MinValue;
            
            //GenerateRandomValues();
            for (int i = 0; i < 100; i++)
            {
                GenerateRandomValuesV3();
                //GenerateRandomValuesV2();
                //RandomNumberInMultiThreading();
            }
            //GenerateString();
            //ExceptionHandling();
            //ValidationExample();
            //ValidateRandomGenerator();
            await Task.Delay(100);
        }

        private static ThreadSafeRandom _threadSafeRandom = new ThreadSafeRandom();

        public void GenerateRandomValuesV3()
        {
            int cryptoSampleSize = 1000000;
            ConcurrentQueue<int> cryptoSamples = new ConcurrentQueue<int>();
            //ThreadSafeRandom _threadSafeRandom = new ThreadSafeRandom();

            Parallel.For(0, cryptoSampleSize, (i, loop) =>
            {
                int val = _threadSafeRandom.Next();
                cryptoSamples.Enqueue(val);
            });

            int _sampleCount = cryptoSamples.Count;
            int _distinctValues = cryptoSamples.Distinct().Count();
            Console.WriteLine($"\n#Value Count: {_sampleCount}");
            Console.WriteLine($"#Distinct Value Count: {_distinctValues}");
            Console.WriteLine($"#Duplicates numbers: {_sampleCount - _distinctValues}\n");

            return;
            const int sampleSize = 1000000;
            List<int> samples = new List<int>();

            ThreadSafeRandom threadSafeRandom = new ThreadSafeRandom();
            for (int i = 0; i < sampleSize; i++)
            {
                int val = threadSafeRandom.Next();
                samples.Add(val);
            }

            int sampleCount = samples.Count;
            int distinctValues = samples.Distinct().Count();
            Console.WriteLine($"\nValue Count: {sampleCount}");
            Console.WriteLine($"Distinct Value Count: {distinctValues}");
            Console.WriteLine($"Duplicates numbers: {sampleCount - distinctValues}\n");
        }

        public void GenerateRandomValuesV2()
        {
            //List<int> samples = new List<int>();
            List<uint> samples = new List<uint>();
            const int sampleSize = 1000000;
            RNGCryptoServiceProvider random = new RNGCryptoServiceProvider("testo");

            byte[] randomBytes = new byte[sampleSize * sizeof(int)];
            random.GetBytes(randomBytes);

            //BitConverter.ToInt32 gets the next four bytes in the array and returns a 32 bit integer. 
            //The next line of code just makes sure that the number is positive.If you don't mind 
            //getting negative numbers, then you can skip that. Or, you can get unsigned integers 
            //by calling BitConverter.ToUInt32.
            for (int i = 0; i < sampleSize; i++)
            {
                uint val = BitConverter.ToUInt32(randomBytes, i * 4);
                //int val = BitConverter.ToInt32(randomBytes, i * 4);
                //val &= 0x7fffffff;
                samples.Add(val);
            }
            
            int sampleCount = samples.Count;
            int distinctValues = samples.Distinct().Count();
            Console.WriteLine($"\nValue Count: {sampleCount}");
            Console.WriteLine($"Distinct Value Count: {distinctValues}");
            Console.WriteLine($"Duplicates numbers: {sampleCount - distinctValues}\n");
            
            return;
            Console.WriteLine($"Press key to view all data.");
            Console.ReadKey();
            for (int i = 0; i < sampleSize; i++)
            {
                Console.WriteLine("{0} => {1}", i, samples[i]);
                if(i % 256 == 0)
                {
                    Console.WriteLine($"Press key to view all data.");
                    Console.ReadKey();
                }
            }
        }

        public void GenerateRandomValues()
        {
            RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[1024];

            random.GetBytes(randomBytes);
            foreach (var b in randomBytes)
            {
                Console.Write("{0:X2} ", b);
            }
        }

        object lockobj = new object();
        //https://www.codeproject.com/Articles/877343/Random-Number-in-MultiThreading
        public void RandomNumberInMultiThreading()
        {
            //Seeding problems    
            //ThreadLocal<PseudoRandomGeneratorV2> randomNumberGenerator6 =
            //    new ThreadLocal<PseudoRandomGeneratorV2>(() =>
            //        new PseudoRandomGeneratorV2());


            //ThreadLocal<CryptoRandomGenerator> randomNumberGenerator6 =
            //    new ThreadLocal<CryptoRandomGenerator>(() =>
            //        new CryptoRandomGenerator());

            //Random class is not thread safe the solution below is perfect and fits as a thread safe solution.
            //In code, random instance created using ThreadLocal<T> class means each task receives its own local instance of Random class.
            ThreadLocal<PseudoRandomGenerator> randomNumberGenerator6 =
                new ThreadLocal<PseudoRandomGenerator>(() =>
                    new PseudoRandomGenerator(Guid.NewGuid().GetHashCode()));

            int cryptoSampleSize = 1000000;
            ConcurrentQueue<int> cryptoSamples = new ConcurrentQueue<int>();

            Parallel.For(0, cryptoSampleSize, (i, loop) =>
            {
                int val = randomNumberGenerator6.Value.Next();
                cryptoSamples.Enqueue(val);
            });

            int sampleCount = cryptoSamples.Count;
            int distinctValues = cryptoSamples.Distinct().Count();
            Console.WriteLine($"\nValue Count: {sampleCount}");
            Console.WriteLine($"Distinct Value Count: {distinctValues}");
            Console.WriteLine($"Duplicates numbers: {sampleCount - distinctValues}\n");
            
            return;
            Console.WriteLine($"Press key to view all data.");
            Console.ReadKey();

            //Random number class in .NET Framework is used for generating random numbers.Random 
            //number class allows creating random number instance by following two constructors:
            //Random() – It makes use of system clock as seed value and creates instance.
            //As random makes use of System clock as input when creating two instances as below:
            //It use the same system clock input so output of the above when doing below code...
            //...generates the same random number. Example both lines of code write 10 on console.
            IRandomGenerator randomNumberGenerator1 = new PseudoRandomGenerator();
            IRandomGenerator randomNumberGenerator2 = new PseudoRandomGenerator();

            for(int i = 0; i < 10; i++)
            {
                int valueGen1 = randomNumberGenerator1.Next();
                int valueGen2 = randomNumberGenerator2.Next();
                string msg = valueGen1 == valueGen2 ? "Equal" : "Not Equal";

                Console.WriteLine($"Generator1: {valueGen1}, " +
                    $"Generator2: {valueGen2}, {msg}");
            }

            Console.WriteLine($"Press key to continue.");
            Console.ReadKey();

            //So the above two ways of creating random instance show that seed value 
            //plays an important role in creating random number instance
            IRandomGenerator randomNumberGenerator3 = new PseudoRandomGenerator(Guid.NewGuid().GetHashCode());
            IRandomGenerator randomNumberGenerator4 = new PseudoRandomGenerator(Guid.NewGuid().GetHashCode());
            Console.WriteLine($"\n\nThe same using different seeds.");
            for (int i = 0; i < 10; i++)
            {
                int valueGen1 = randomNumberGenerator3.Next();
                int valueGen2 = randomNumberGenerator4.Next();
                string msg = valueGen1 == valueGen2 ? "Equal" : "Not Equal";

                Console.WriteLine($"Generator1: {valueGen1}, " +
                    $"Generator2: {valueGen2}, {msg}");
            }

            Console.WriteLine($"Press key to continue.");
            Console.ReadKey();

            IRandomGenerator randomNumberGenerator = new PseudoRandomGenerator();
            int value = 0;
            List<int> valueList = new List<int>();

            Parallel.For(0, 1000, (i, loop) =>
            {
                lock (lockobj)
                {
                    value = randomNumberGenerator.Next();
                    valueList.Add(value);
                }
                //if (randomNumberGenerator.Next() == 0) loop.Stop();
            });

            Console.WriteLine($"Value Count: {valueList.Count}");
            Console.WriteLine($"Distinct Value Count: {valueList.Distinct().Count()}");
            Console.WriteLine($"Press key to view all data.");
            Console.ReadKey();
            for(int i = 0; i < valueList.Count; i++)
            {
                Console.WriteLine($"{i}, {valueList[i]}");
            }
        }

        //double num3 = (double)num1/num2;
        //double num3 = (double)num1/(double)num2;
        private void ValidateRandomGenerator()
        {
            Console.WriteLine("Random Exception handling.");
            try
            {
                Random _rnd = new Random();
                for(int i = 0; i < 100; i++)
                {
                    int value = _rnd.Next(-100, 10);
                    Console.WriteLine("value: {0}", value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} {1}", ex.Message, ex.GetType());
                //throw;
            }

            return;

            for(int j = 0; j < 100; j++)
            { 
                int success = 0;
                int failure = 0;

                Random rnd = new Random();
                Func<double> randomGenerator = () => (double)rnd.Next()/int.MaxValue;
                Console.WriteLine("Start processing, ValidateRandomGenerator()");
                for (int i = 0; i < 10000; i++)
                {
                    if(KSTest(randomGenerator))
                    {
                        success++;
                    }
                    else
                    {
                        failure++;
                    }
                }

                Console.WriteLine($"success: {success}, failure: {failure}");
            }
            //for (int i = 0; i < 10; i++)
            //{
            //    Console.WriteLine($"{i} {randomGenerator()}");
            //}
        }

        private static bool KSTest(Func<double> randomGenerator)
        {
            /// Kolmogorov-Smirnov test for distributions.  See Knuth volume 2, page 48-51 (third edition).
            /// This test should *fail* on average one time in 1000 runs.
            /// That's life with random number generators: if the test passed all the time, 
            /// the source wouldn't be random enough!  If the test were to fail more frequently,
            /// the most likely explanation would be a bug in the code.

            bool passed = false;
            int numReps = 1000;
            double failureProbability = 0.001; // probability of test failing with normal input
            int j;
            double[] samples = new double[numReps];

            for (j = 0; j != numReps; ++j)
                samples[j] = randomGenerator();

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

            passed = cutoff_low <= K_plus && K_plus <= cutoff_high && 
                cutoff_low <= K_minus && K_minus <= cutoff_high;

            return passed;
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
            char[] charset = { 'Å', 'Ä', 'Ö', 'å', 'ä', 'ö','A', 'A', 'b', '*', '-', 'c', 'F', 'G', 'h', 'k', 'K', '&', '$', '@', '!', '4', '7' };
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
                pwd = randomStringGenerator.Next(5, 5, 10, 0, 20, AllowedCharacters.All, false);
                Console.WriteLine(pwd);
            }
        }
    }

    public class ThreadSafeRandom : Random
    {
        private static readonly RNGCryptoServiceProvider _global = new RNGCryptoServiceProvider();

        private ThreadLocal<Random> _local = new ThreadLocal<Random>(() =>
        {

            var buffer = new byte[4];
            _global.GetBytes(buffer);         
            // RNGCryptoServiceProvider is
            // thread-safe for use in this manner
            return new Random(BitConverter.ToInt32(buffer, 0));
        });

        public override int Next()
        {
            return _local.Value.Next();
        }

        public override int Next(int maxValue)
        {
            return _local.Value.Next(maxValue);
        }

        public override int Next(int minValue, int maxValue)
        {
            return _local.Value.Next(minValue, maxValue);
        }

        public override double NextDouble()
        {

            return _local.Value.NextDouble();
        }

        public override void NextBytes(byte[] buffer)
        {
            _local.Value.NextBytes(buffer);
        }
    }
}

/*
References and Notes:
https://www.c-sharpcorner.com/uploadfile/logisimo/random-number-generation-and-windows-forms-encryption-via-C-Sharp-parallel-programming/

*/

