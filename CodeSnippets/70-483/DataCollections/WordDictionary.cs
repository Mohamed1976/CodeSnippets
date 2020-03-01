using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace _70_483.DataCollections
{
    public class WordDictionary : IEnumerable<string>
    {
        private List<string> _elements;

        public WordDictionary(string[] array)
        {
            _elements = new List<string>(array);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
