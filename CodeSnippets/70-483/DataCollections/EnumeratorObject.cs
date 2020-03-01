using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace _70_483.DataCollections
{
    public class EnumeratorObject : IEnumerable, IEnumerator<int>//, IEnumerable<int>
    {
        public EnumeratorObject(int limit)
        {
            _count = 0;
            _limit = limit;
        }
        
        private int _count;
        private int _limit;

        public int Current => _count;

        object IEnumerator.Current => _count;

        public void Dispose()
        {
        }

        public IEnumerator GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            if (++_count < _limit)
                return true;
            else
                return false;
        }

        public void Reset()
        {
            _count = 0;
        }

        //IEnumerator<int> IEnumerable<int>.GetEnumerator()
        //{
        //    return this;
        //}
    }
}
