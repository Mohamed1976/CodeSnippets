using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleWithDI.RandomString.RandomGenerators
{
    /// <summary>
    /// A random string generator that uses the <see cref="System.Security.Cryptography.RNGCryptoServiceProvider"/> to create a random string.
    /// This is a slower generator but can be used for generating passwords.
    /// </summary>

    /// <summary>
    /// Gets a singleton instance that uses the <see cref="System.Security.Cryptography.RNGCryptoServiceProvider"/> to create a random string.
    /// This is a slower generator but can be used for generating passwords.
    /// </summary>
    public class CryptoRandomGenerator
    {

        /// <summary>
        /// Creates a new instance of the <see cref="CryptographicRandomStringGenerator"/> class.
        /// </summary>
        public CryptoRandomGenerator()
        {

        }
    }
}
