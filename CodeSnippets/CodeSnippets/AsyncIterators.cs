using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeSnippets
{
    public class AsyncIterators
    {
        public async Task AsyncIterator()
        {
            await foreach (int i in GenerateSequence())
            {
                Console.WriteLine($"The time is {DateTime.Now:hh:mm:ss}, retrieved number {i}"); 
            }
        }

        internal async IAsyncEnumerable<int> GenerateSequence()
        {
            for(int i = 0; i < 50; i++)
            {
                //Every tenth element, wait 2 seconds
                if (i % 10 == 0)
                    await Task.Delay(2000);
                yield return i;
            }
        }
    }
}
