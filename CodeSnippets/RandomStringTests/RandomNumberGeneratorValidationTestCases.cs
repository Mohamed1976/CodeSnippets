using RandomString.RandomGenerators;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace RandomStringTests
{
    public class RandomNumberGeneratorValidationTestCases
    {
        private readonly IRandomGenerator _pseudoRandomGenerator = null;
        private readonly IRandomGenerator _cryptoRandomGenerator = null;
        private readonly ITestOutputHelper _output;

        public RandomNumberGeneratorValidationTestCases(ITestOutputHelper output)
        {
            _pseudoRandomGenerator = new PseudoRandomGenerator();
            _cryptoRandomGenerator = new CryptoRandomGenerator();
            _output = output;
        }

        [Fact]
        public void ValidateMaxValueCryptoRandomGenerator()
        {
            //The exclusive upper bound of the random number to be generated. 
            //maxValue must be greater than or equal to 0.
            int maxValue = 35; //Should be length of all charsets combined 
            int value;

            //A 32 - bit signed integer that is greater than or equal to 0, and less than maxValue; 
            //that is, the range of return values ordinarily includes 0 but not maxValue.
            //However, if maxValue equals 0, maxValue is returned.
            for(int i = 0; i < 1000; i++)
            {
                value = _cryptoRandomGenerator.Next(maxValue);
                Assert.True(value >= 0 && value < maxValue);
            }

            maxValue = 0;
            value = _cryptoRandomGenerator.Next(maxValue);
            Assert.True(value == maxValue);
        }

        [Fact]
        public void ValidateMaxAndMinValueCryptoRandomGenerator()
        {
            //Returns a random integer that is within a specified range.
            //The inclusive lower bound of the random number returned.
            //The exclusive upper bound of the random number returned. 
            //maxValue must be greater than or equal to minValue.
            //A 32-bit signed integer greater than or equal to minValue 
            //and less than maxValue; that is, the range of return values 
            //includes minValue but not maxValue. If minValue equals 
            //maxValue, minValue is returned.
            int maxValue = 35; //Should be length of all charsets combined 
            int minValue = 7;
            int value;

            for (int i = 0; i < 1000; i++)
            {
                value = _cryptoRandomGenerator.Next(minValue, maxValue);
                Assert.True(value >= minValue && value < maxValue);
            }

            minValue = 0;
            maxValue = 0;
            value = _cryptoRandomGenerator.Next(minValue, maxValue);
            Assert.True(value == minValue);

            minValue = 35;
            maxValue = 35;
            value = _cryptoRandomGenerator.Next(minValue, maxValue);
            Assert.True(value == minValue);

        }

        [Fact]
        public void ValidateMaxValuePseudoRandomGenerator()
        {
            int maxValue = 35;
            int value;

            for (int i = 0; i < 1000; i++)
            {
                value = _pseudoRandomGenerator.Next(maxValue);
                Assert.True(value >= 0 && value < maxValue);
            }

            maxValue = 0;
            value = _pseudoRandomGenerator.Next(maxValue);
            Assert.True(value == maxValue);
        }

        [Fact]
        public void ValidateMaxAndMinValuePseudoRandomGenerator()
        {
            int maxValue = 35; //Should be length of all charsets combined 
            int minValue = 7;
            int value;

            for (int i = 0; i < 1000; i++)
            {
                value = _pseudoRandomGenerator.Next(minValue, maxValue);
                Assert.True(value >= minValue && value < maxValue);
            }

            minValue = 0;
            maxValue = 0;
            value = _pseudoRandomGenerator.Next(minValue, maxValue);
            Assert.True(value == minValue);

            minValue = 35;
            maxValue = 35;
            value = _pseudoRandomGenerator.Next(minValue, maxValue);
            Assert.True(value == minValue);
        }

        [Fact]
        public void ValidateRandomnessCryptoRandomGenerator()
        {
            int failure = 0;
            const int nrOfIterations = 10000;

            Func<double> randomGenerator = () => (double)_cryptoRandomGenerator.Next() / int.MaxValue;
            for (int i = 0; i < nrOfIterations; i++)
            {
                if (!KSTest(randomGenerator))
                {
                    failure++;
                }
            }

            //Kolmogorov - Smirnov test of CryptoRandomGenerator: success[9988], failure[12]
            //Kolmogorov - Smirnov test of CryptoRandomGenerator: success[9990], failure[10]
            //Kolmogorov - Smirnov test of CryptoRandomGenerator: success[9990], failure[10]
            //Kolmogorov - Smirnov test of CryptoRandomGenerator: success[9991], failure[9]
            //Kolmogorov - Smirnov test of CryptoRandomGenerator: success[9992], failure[8]
            _output.WriteLine($"Kolmogorov-Smirnov test of CryptoRandomGenerator: success[{nrOfIterations - failure}], failure[{failure}]");
            //If the test were to fail more frequently, the most likely explanation would be a bug in the code.
            Assert.True(failure < 20);
        }

        [Fact]
        public void ValidateRandomnessPseudoRandomGenerator()
        {
            int failure = 0;
            const int nrOfIterations = 10000;

            Func<double> randomGenerator = () => (double)_pseudoRandomGenerator.Next() / int.MaxValue;
            for (int i = 0; i < nrOfIterations; i++)
            {
                if (!KSTest(randomGenerator))
                {
                    failure++;
                }
            }

            //Kolmogorov - Smirnov test of PseudoRandomGenerator: success[9988], failure[12]
            //Kolmogorov - Smirnov test of PseudoRandomGenerator: success[9995], failure[5]
            //Kolmogorov - Smirnov test of PseudoRandomGenerator: success[9991], failure[9]
            //Kolmogorov - Smirnov test of PseudoRandomGenerator: success[9987], failure[13]
            //Kolmogorov - Smirnov test of PseudoRandomGenerator: success[9988], failure[12]
            //Kolmogorov - Smirnov test of PseudoRandomGenerator: success[9994], failure[6]
            //Kolmogorov - Smirnov test of PseudoRandomGenerator: success[9990], failure[10]
            //Kolmogorov - Smirnov test of PseudoRandomGenerator: success[9987], failure[13]
            //Kolmogorov - Smirnov test of PseudoRandomGenerator: success[9994], failure[6]
            //Kolmogorov - Smirnov test of PseudoRandomGenerator: success[9993], failure[7]
            _output.WriteLine($"Kolmogorov-Smirnov test of PseudoRandomGenerator: success[{nrOfIterations - failure}], failure[{failure}]");
            //If the test were to fail more frequently, the most likely explanation would be a bug in the code.
            Assert.True(failure < 20);
        }

        //https://www.codeproject.com/Articles/25172/Simple-Random-Number-Generation
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
    }
}
