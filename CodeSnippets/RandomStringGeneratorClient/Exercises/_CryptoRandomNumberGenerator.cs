using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RandomStringGeneratorClient.Exercises
{
    public class _CryptoRandomNumberGenerator
    {
        private readonly RandomNumberGenerator _randomNumberGenerator;
        private readonly byte[] _uintBuffer = new byte[sizeof(uint)];
        public _CryptoRandomNumberGenerator()
        {
            _randomNumberGenerator = RandomNumberGenerator.Create();
            //_randomNumberGenerator.GetBytes()
            // GetBytes(new Span<byte>(data, offset, count));
        }

        public int GetInt32(int fromInclusive, int toExclusive)
        {
            if (fromInclusive >= toExclusive)
                throw new ArgumentException("InvalidRandomRange");//SR.Argument_InvalidRandomRange);

            // The total possible range is [0, 4,294,967,295).
            // Subtract one to account for zero being an actual possibility.
            uint range = (uint)toExclusive - (uint)fromInclusive - 1;

            // If there is only one possible choice, nothing random will actually happen, so return
            // the only possibility.
            if (range == 0)
            {
                return fromInclusive;
            }

            // Create a mask for the bits that we care about for the range. The other bits will be
            // masked away.
            uint mask = range;
            mask |= mask >> 1;
            mask |= mask >> 2;
            mask |= mask >> 4;
            mask |= mask >> 8;
            mask |= mask >> 16;

            //Span<uint> resultSpan = stackalloc uint[1];
            uint result;

            do
            {
                _randomNumberGenerator.GetBytes(_uintBuffer);
                uint rand = BitConverter.ToUInt32(_uintBuffer, 0);

                //RandomNumberGeneratorImplementation.FillSpan(MemoryMarshal.AsBytes(resultSpan));
                result = mask & rand; // resultSpan[0];
            }
            while (result > range);

            return (int)result + fromInclusive;
        }

        private unsafe void FillSpan(Span<byte> data)
        {
            if (data.Length > 0)
            {
                fixed (byte* ptr = data) GetBytes(ptr, data.Length);
            }
        }

        private unsafe void GetBytes(byte* pbBuffer, int count)
        {
            //_randomNumberGenerator.GetBytes(pbBuffer, count);

            //Debug.Assert(count > 0);

            //if (!Interop.Crypto.GetRandomBytes(pbBuffer, count))
            //{
            //    throw Interop.Crypto.CreateOpenSslCryptographicException();
            //}
        }

    }
}
