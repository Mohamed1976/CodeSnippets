using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace _70_483.DataCollections
{
    public class EnumeratorObjectV2 : IEnumerable<int>
    {
        public EnumeratorObjectV2(int limit)
        {
            _limit = limit;
        }

        private int _limit;

        public IEnumerator<int> GetEnumerator()
        {
            for(int index = 0; index < _limit; index++)
            {
                yield return index;
            }

            //yield return 1;
            //yield return 2;
            //yield return 3;
            //yield return 4;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
