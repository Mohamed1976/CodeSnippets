using RandomStringGeneratorLibrary;
using RandomStringGeneratorLibrary.RandomNumberGenerators;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RandomStringGeneratorClient.Exercises
{
    public class RandomStringExamples
    {
        public async Task Run()
        {
            //IRandomNumberGenerator _cryptoRandomNumberGenerator = new CryptoRandomNumberGenerator();
            //IRandomNumberGenerator _pseudoRandomNumberGenerator = new PseudoRandomNumberGenerator();

            //Func<int, int, int> cryptoRandomNumberGenerator = (fromInclusive, toExclusive) =>
            //{
            //    return _cryptoRandomNumberGenerator.Next(fromInclusive, toExclusive);
            //};

            //Func<int, int, int> pseudoRandomNumberGenerator = (fromInclusive, toExclusive) =>
            //{
            //    return _pseudoRandomNumberGenerator.Next(fromInclusive, toExclusive);
            //};

            //Check reproducibility
            for (int i = 0; i < 100; i++)
            {
                RandomCheck();
                //ThreadSafeRandomCheck();
                //CheckThreadSafety();
                //CheckUniformity(cryptoRandomNumberGenerator);
                //CheckNumberOfDuplicates(cryptoRandomNumberGenerator);

                //pseudoRandomNumberGeneratorCheckThreadSafety();
                ///CheckUniformity(pseudoRandomNumberGenerator);
                //CheckNumberOfDuplicates(pseudoRandomNumberGenerator);

                //RandomString class
                //_pseudoRandomNumberGeneratorCheckThreadSafety();

                //CheckNumberDistribution(10000000, 0, 100);
                //CheckNumberDistribution(10000000, 1, 7);
                //CheckNumberDistribution(10000000, 7, 13);
            }

            
            //MultipleThreads();
            await Task.Delay(100);
        }

        private int[] _maxValue = new int[6];
        private int[] _minValue = new int[6];
        private void RandomCheck()
        {
            CheckNumberDistributionV2(10000000, 1, 7);
            //_CryptoRandomNumberGenerator numberGenerator = new _CryptoRandomNumberGenerator();
            //int value = numberGenerator.GetInt32(0, 6);
            //Console.WriteLine($"numberGenerator.GetInt32(0, 6) => {value}");
            Console.WriteLine($"\nMax distribution:");
            for (int i = 0; i < _maxValue.Length; i++)
            {
                Console.WriteLine($"{i} => {_maxValue[i]}");
            }

            Console.WriteLine($"\nMin distribution:");
            for (int i = 0; i < _minValue.Length; i++)
            {
                Console.WriteLine($"{i} => {_minValue[i]}");
            }

        }

        private void CheckNumberDistributionV2(int nrOfNumbers, int fromInclusive, int toExclusive)
        {
            Stopwatch stopwatch = new Stopwatch();
            int[] numbers = new int[nrOfNumbers];
            _CryptoRandomNumberGenerator randomNumberGenerator = new _CryptoRandomNumberGenerator();

            stopwatch.Start();
            for (int i = 0; i < nrOfNumbers; i++)
                numbers[i] = randomNumberGenerator.GetInt32(fromInclusive, toExclusive);
            stopwatch.Stop();

            int minValue = numbers.Min();
            int maxValue = numbers.Max();

            int[] frequencyArray = new int[maxValue - minValue + 1];

            for (int i = 0; i < numbers.Length; i++)
            {
                frequencyArray[numbers[i] - minValue]++;
            }

            int maxFrequency = frequencyArray.Max();
            int minFrequency = frequencyArray.Min();

            Console.WriteLine($"nrOfNumbers[{nrOfNumbers}], " +
                $"fromInclusive[{fromInclusive}], toExclusive[{toExclusive}]");
            Console.WriteLine($"maxFrequency[{maxFrequency}], minFrequency[{minFrequency}]");
            Console.WriteLine($"Delta time: {stopwatch.ElapsedMilliseconds}ms");
            for (int i = 0; i < frequencyArray.Length; i++)
            {
                if(frequencyArray[i] == maxFrequency)
                {
                    _maxValue[i]++;
                }

                if (frequencyArray[i] == minFrequency)
                {
                    _minValue[i]++;
                }

                Console.WriteLine($"{minValue + i} => {frequencyArray[i]}, ({(double)frequencyArray[i] * 100 / numbers.Length}%)");
            }
        }

        private void ThreadSafeRandomCheck()
        {
            int sampleSize = 1000000;
            ConcurrentQueue<int> samples = new ConcurrentQueue<int>();
            //RandomStringGenerator randomNumberGenerator = new RandomStringGenerator();
            ThreadLocal<RandomStringGenerator> randomNumberGenerator =
                new ThreadLocal<RandomStringGenerator>(() =>
                    new RandomStringGenerator());

            Parallel.For(0, sampleSize, (i, loop) =>
            {
                int val = randomNumberGenerator.Value.Next();
                samples.Enqueue(val);
            });

            int _sampleCount = samples.Count;
            int _distinctValues = samples.Distinct().Count();
            Console.WriteLine($"\n#Value Count: {_sampleCount}");
            Console.WriteLine($"#Distinct Value Count: {_distinctValues}");
            Console.WriteLine($"#Duplicates numbers: {_sampleCount - _distinctValues}\n");
        }

        private void MultipleThreads()
        {
            Thread t1 = new Thread(() =>
            {
                RandomStringGenerator randomNumberGenerator = new RandomStringGenerator();

                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine($"t1 {i}, {randomNumberGenerator.Next()}");
                    Thread.Sleep(500);
                }
            });

            Thread t2 = new Thread(() =>
            {
                RandomStringGenerator randomNumberGenerator = new RandomStringGenerator();

                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine($"t2 {i}, {randomNumberGenerator.Next()}");
                    Thread.Sleep(500);
                }
            });

            t1.Start();
            t2.Start();
        }

        private void CheckNumberDistribution(int nrOfNumbers, int fromInclusive, int toExclusive)
        {
            Stopwatch stopwatch = new Stopwatch();
            int[] numbers = new int[nrOfNumbers];
            RandomStringGenerator randomNumberGenerator = new RandomStringGenerator();

            stopwatch.Start();
            for (int i = 0; i < nrOfNumbers; i++)
                numbers[i] = randomNumberGenerator.Next(fromInclusive, toExclusive);
            stopwatch.Stop();

            int minValue = numbers.Min();
            int maxValue = numbers.Max();

            int[] frequencyArray = new int[maxValue - minValue + 1];

            for (int i = 0; i < numbers.Length; i++)
            {
                frequencyArray[numbers[i] - minValue]++;
            }

            int maxFrequency = frequencyArray.Max();
            int minFrequency = frequencyArray.Min();

            Console.WriteLine($"nrOfNumbers[{nrOfNumbers}], " +
                $"fromInclusive[{fromInclusive}], toExclusive[{toExclusive}]");
            Console.WriteLine($"maxFrequency[{maxFrequency}], minFrequency[{minFrequency}]");

            for(int i = 0; i < frequencyArray.Length; i++)
            {
                Console.WriteLine($"{minValue + i} => {frequencyArray[i]}, ({(double)frequencyArray[i]*100 / numbers.Length}%)");
            }
        }

        private void pseudoRandomNumberGeneratorCheckThreadSafety()
        {
            int cryptoSampleSize = 1000000;
            ConcurrentQueue<int> cryptoSamples = new ConcurrentQueue<int>();
            IRandomNumberGenerator randomNumberGenerator = new PseudoRandomNumberGenerator();
            var watch = new Stopwatch();

            watch.Start();
            Parallel.For(0, cryptoSampleSize, (i, loop) =>
            {
                int val = randomNumberGenerator.Next();
                cryptoSamples.Enqueue(val);
            });
            watch.Stop();

            int sampleCount = cryptoSamples.Count;
            int distinctValues = cryptoSamples.Distinct().Count();
            double average = cryptoSamples.Average();
            Console.WriteLine($"\nValue Count: {sampleCount}");
            Console.WriteLine($"Distinct Value Count: {distinctValues}");
            Console.WriteLine($"Duplicates numbers: {sampleCount - distinctValues}");
            Console.WriteLine($"Average values: {average}");
            Console.WriteLine($"RandomNumberGenerator took {watch.Elapsed.TotalMilliseconds} ms\n");
        }

        //Average 400ms, _randomNumberGenerator = new CryptoRandomNumberGenerator();
        //Average 300ms, _randomNumberGenerator = new PseudoRandomNumberGenerator();
        private void _pseudoRandomNumberGeneratorCheckThreadSafety()
        {
            int cryptoSampleSize = 1000000;
            ConcurrentQueue<int> cryptoSamples = new ConcurrentQueue<int>();
            //IRandomNumberGenerator randomNumberGenerator = new PseudoRandomNumberGenerator();
            RandomStringGenerator randomNumberGenerator = new RandomStringGenerator(); 
            var watch = new Stopwatch();

            watch.Start();
            Parallel.For(0, cryptoSampleSize, (i, loop) =>
            {
                int val = randomNumberGenerator.Next();
                cryptoSamples.Enqueue(val);
            });
            watch.Stop();

            int sampleCount = cryptoSamples.Count;
            int distinctValues = cryptoSamples.Distinct().Count();
            double average = cryptoSamples.Average();
            Console.WriteLine($"\nValue Count: {sampleCount}");
            Console.WriteLine($"Distinct Value Count: {distinctValues}");
            Console.WriteLine($"Duplicates numbers: {sampleCount - distinctValues}");
            Console.WriteLine($"Average values: {average}");
            Console.WriteLine($"RandomNumberGenerator took {watch.Elapsed.TotalMilliseconds} ms\n");
        }

        private void CheckThreadSafety()
        {
            int cryptoSampleSize = 1000000;
            ConcurrentQueue<int> cryptoSamples = new ConcurrentQueue<int>();
            IRandomNumberGenerator randomNumberGenerator = new CryptoRandomNumberGenerator();
            var watch = new Stopwatch();

            watch.Start();
            Parallel.For(0, cryptoSampleSize, (i, loop) =>
            {
                int val = randomNumberGenerator.Next();
                cryptoSamples.Enqueue(val);
            });
            watch.Stop();

            int sampleCount = cryptoSamples.Count;
            int distinctValues = cryptoSamples.Distinct().Count();
            double average = cryptoSamples.Average();
            Console.WriteLine($"\nValue Count: {sampleCount}");
            Console.WriteLine($"Distinct Value Count: {distinctValues}");
            Console.WriteLine($"Duplicates numbers: {sampleCount - distinctValues}");
            Console.WriteLine($"Average values: {average}");
            Console.WriteLine($"RandomNumberGenerator took {watch.Elapsed.TotalMilliseconds} ms\n");
        }

        //Given a range of possible values, they would all have an equal probability if being generated.
        //The experiment I ran generated 1000 000 random values between 1 and 10,000. If the series averages 5,000, then we know we have a uniform distribution.
        private void CheckUniformity(Func<int,int,int> randomNumberGenerator)
        {
            int min = 1, max = 10000;
            int sampleSize = 1000000, value = 0;
            List<int> samples = new List<int>();
            //IRandomNumberGenerator randomNumberGenerator = new CryptoRandomNumberGenerator();
            var watch = new Stopwatch();
            
            watch.Start();
            for (int i = 0; i < sampleSize; i++)
            {
                value = randomNumberGenerator(min, max);
                //value = randomNumberGenerator.Next(1,10000);
                samples.Add(value);
            }
            watch.Stop();

            /* Show summary */
            int sampleCount = samples.Count;
            int distinctValues = samples.Distinct().Count();

            Console.WriteLine($"\nGenerating {sampleSize} random values between {min} and {max}.");
            Console.WriteLine($"Sample Count: {sampleCount}");
            Console.WriteLine($"Distinct sample Count: {distinctValues}");
            Console.WriteLine($"Duplicates numbers: {sampleCount - distinctValues}");
            Console.WriteLine($"Average sample: {samples.Average()}");
            Console.WriteLine($"RandomNumberGenerator took {watch.Elapsed.TotalMilliseconds} ms\n");
        }

        private void CheckNumberOfDuplicates(Func<int, int, int> randomNumberGenerator)
        {
            int min = 0, max = int.MaxValue;
            int sampleSize = 1000000, value = 0;
            List<int> samples = new List<int>();
            //IRandomNumberGenerator randomNumberGenerator = new CryptoRandomNumberGenerator();
            var watch = new Stopwatch();

            watch.Start();
            for (int i = 0; i < sampleSize; i++)
            {
                //value = randomNumberGenerator.Next();
                value = randomNumberGenerator(min, max);
                samples.Add(value);
            }
            watch.Stop();

            /* Show summary */
            int sampleCount = samples.Count;
            int distinctValues = samples.Distinct().Count();
            Console.WriteLine($"\nGenerating {sampleSize} random values between {min} and {max}.");
            Console.WriteLine($"Sample Count: {sampleCount}");
            Console.WriteLine($"Distinct sample Count: {distinctValues}");
            Console.WriteLine($"Duplicates numbers: {sampleCount - distinctValues}");
            Console.WriteLine($"RandomNumberGenerator took {watch.Elapsed.TotalMilliseconds} ms\n");

        }
    }
}
