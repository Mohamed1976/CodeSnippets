using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace _70_483.IOoperations
{
    public class StreamReaderEnumerator : IEnumerator<string>
    {
        private bool disposedValue = false;
        private StreamReader _sr = null;

        public StreamReaderEnumerator(string path)
        {
            _sr = new StreamReader(path);
        }

        ~StreamReaderEnumerator()
        {
            Dispose(false);
        }

        private string _current;
        // Implement the IEnumerator(T).Current publicly, but implement 
        // IEnumerator.Current, which is also required, privately.
        public string Current
        {

            get
            {
                if (_sr == null || _current == null)
                {
                    throw new InvalidOperationException();
                }

                return _current;
            }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Dispose of managed resources.
                }

                // Dispose of unmanaged resources.
                _current = null;
                if (_sr != null)
                {
                    _sr.Close();
                    _sr.Dispose();
                }

                disposedValue = true;
            }            
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool MoveNext()
        {
            _current = _sr.ReadLine();
            if (_current == null)
                return false;
            return true;
        }

        public void Reset()
        {
            _sr.DiscardBufferedData();
            _sr.BaseStream.Seek(0, SeekOrigin.Begin);
            _current = null;
        }
    }
}
