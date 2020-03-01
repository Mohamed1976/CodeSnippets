using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace _70_483.Encryption
{
    //All of the encryption classes, including Aes, are extensions of the base class
    //SymmetricAlgorithm, which is in the
    //System.Security.Cryptography namespace.There are a number of
    //other encryption implementations that you can use in your program including the
    //Data Encryption Standard(DES), RC2, Rijndael, and Triple DES.
    //The Data Encryption Standard(DES) was developed in the 1970s.Advances
    //in computer power and cryptoanalysis(code cracking) mean that this standard is
    //now regarded as insecure.There are libraries available for the use of DES in
    //.NET and the encryption process is performed in exactly the same way as with
    //the AES standard described next. You should only use DES encryption for
    //compatibility with legacy systems. Any new systems requiring data encryption should use AES.
    //RC2 is another encryption technology that is supported by.NET, but is now
    //regarded as insecure.Again, you should only use this in your applications if you
    //are working with an existing system that uses this encryption.
    //Rijndael is the cypher on which AES was based.AES is implemented as a
    //subset of Rijndael. If you want to access all the features of Rijndael you can use
    //this class. TripleDES improves on the security of the DES standard by encrypting the
    //incoming data three times in succession using three different keys.The
    //electronic payment industry makes use of TripleDES.

    public class EncryptionExamples
    {
        public EncryptionExamples()
        {

        }

        //Create a new pair of keys and store them in user container
        private void GetRSAKeysFromMachineKeyStore(string containerName,
            out string publicKey,
            out string privateKey,
            bool deleteKeyStore = false)
        {
            CspParameters cspParams = new CspParameters();
            cspParams.KeyContainerName = containerName;

            // Specify that the key is to be stored in the machine level key store
            cspParams.Flags = CspProviderFlags.UseMachineKeyStore;

            // Create a crypto service provider
            RSACryptoServiceProvider rsaStore = new RSACryptoServiceProvider(cspParams);
            publicKey = rsaStore.ToXmlString(includePrivateParameters: false);
            privateKey = rsaStore.ToXmlString(includePrivateParameters: true);

            //If you want to delete a stored key you can set the PersistKeyInCsP
            //property of the RSACryptoServiceProvider instance to False and then clear the service
            if (deleteKeyStore)
            {
                rsaStore.PersistKeyInCsp = false;
            }
            else
            {
                // Make sure that it is persisting keys
                rsaStore.PersistKeyInCsp = true;
            }

            // Clear the provider to make sure it saves the key
            rsaStore.Clear();
        }

        //Create a new pair of keys and store them in user container
        private void GetRSAKeysFromContainers(string containerName, 
        out string publicKey, 
        out string privateKey,
        bool deleteKeyStore = false)
        {
            CspParameters csp = new CspParameters();
            csp.KeyContainerName = containerName;

            // Create a new RSA to encrypt the data
            RSACryptoServiceProvider rsaStore = new RSACryptoServiceProvider(csp);
            publicKey = rsaStore.ToXmlString(includePrivateParameters: false);
            privateKey = rsaStore.ToXmlString(includePrivateParameters: true);

            //If you want to delete a stored key you can set the PersistKeyInCsP
            //property of the RSACryptoServiceProvider instance to False and then clear the service
            if(deleteKeyStore)
            {
                rsaStore.PersistKeyInCsp = false;
                rsaStore.Clear();
            }
        }

        //The RSACryptoServiceProvider class provides methods that can be used
        //to extract the public and private keys from an instance.These can then be sent to
        //a recipient who can use them to encrypt and decrypt messages. The
        //ToXmlString method will return either the public or the public and private
        //keys, depending on the value of a Boolean argument.
        private void GetRSAKeys(out string _publicKey, out string _privateKey)
        {
            // Create a new RSA to encrypt the data
            // should be wrapped in using for production code
            RSACryptoServiceProvider rsaEncrypt = new RSACryptoServiceProvider();

            string publicKey = rsaEncrypt.ToXmlString(includePrivateParameters: false); //Contains only public key
            string privateKey = rsaEncrypt.ToXmlString(includePrivateParameters: true); //Contains public and private key

            _publicKey = publicKey;
            _privateKey = privateKey;
        }

        private void RSADecrypt(byte[] cipherText, string privateKey, out string plainText)
        {
            // Create a new RSA to decrypt the data
            // should be wrapped in using for production code
            RSACryptoServiceProvider rsaDecrypt = new RSACryptoServiceProvider();
            //Configure the decryptor from the XML in the private key
            //Once the XML has been sent to the recipient it can be used to configure an
            //RSACryptoServiceProvider instance by using the FromXmlString method.
            rsaDecrypt.FromXmlString(privateKey);

            byte[] decryptedBytes = rsaDecrypt.Decrypt(cipherText, fOAEP: false);

            // This will convert our input string into bytes and back
            ASCIIEncoding converter = new ASCIIEncoding();
            plainText = converter.GetString(decryptedBytes);
        }

        private void RSAEncrypt(string plainText, string publicKey, out byte[] cipherText)
        {
            byte[] encryptedBytes;
            // This will convert our input string into bytes and back
            ASCIIEncoding converter = new ASCIIEncoding();
            // Convert the plain text into a byte array
            byte[] plainBytes = converter.GetBytes(plainText);

            // Create a new RSA to encrypt the data
            // should be wrapped in using for production code
            RSACryptoServiceProvider rsaEncrypt = new RSACryptoServiceProvider();
            // Now tell the encyryptor to use the public key to encrypt the data
            rsaEncrypt.FromXmlString(publicKey);

            // Use the encryptor to encrypt the data. The fOAEP parameter
            // specifies how the output is "padded" with extra bytes
            // For maximum compatibility with receiving systems, set this as
            // false
            encryptedBytes = rsaEncrypt.Encrypt(plainBytes, fOAEP: false);
            cipherText = encryptedBytes;
        }

        private static void showHash(object source)
        {
            Console.WriteLine("Hash for {0} is: {1:X}", source, source.GetHashCode());
        }

        private static byte[] calculateHash(string source)
        {
            // This will convert our input string into bytes and back
            ASCIIEncoding converter = new ASCIIEncoding();
            byte[] sourceBytes = converter.GetBytes(source);
            HashAlgorithm hasher = SHA256.Create();
            //For example if you want to use SHA1 hash algorithm 
            //HashAlgorithm hasher = new SHA1Managed();
            byte[] hash = hasher.ComputeHash(sourceBytes);
            return hash;
        }

        private byte[] GetSHA512Hash(string msg)
        {
            //SHA2 family is save, md5 sha1 is not considerd save  
            SHA512 hashSvc = SHA512.Create();

            //SHA512 returns 512 bits (8 bits/byte, 64 bytes) for the hash
            byte[] hash = hashSvc.ComputeHash(Encoding.UTF8.GetBytes(msg));

            return hash;
        }

        private void SymmetricEncryptionAes(string plainText,
            out byte[] cipherText,
            out byte[] initializationVector,
            out byte[] key)
        {
            byte[] _cipherText = null;
            byte[] _key = null;
            byte[] _initializationVector = null;

            //Defaults - Keysize 256, Mode CBC, Padding PKC27
            //Aes cipher = new AesManaged();
            //Aes cipher = new AesCryptoServiceProvider();
            using (Aes cipher = Aes.Create())
            {
                //If you use PaddingMode.Zeros and CipherMode.ECB the resulting
                //cipher text will always be the same. In contrast to the 
                //PaddingMode.ISO10126 and CBC, each time you encrypt the same string
                //a different cipher text will be created. 
                //cipher.Padding = PaddingMode.Zeros; 
                //cipher.Mode = CipherMode.ECB;

                //PaddingMode.ISO10126 you take the original string and break it into blocks
                //the last block is not going to be the right size (128 bits),
                //the alogorithms needs to pad out the rest of the last block
                //By using this PaddingMode.ISO10126 it is going to put random data
                //to fill the last block to 128 bits (so padding with random data) 
                cipher.Padding = PaddingMode.ISO10126;

                //Create the encryptor, convert to bytes, and encrypt
                ICryptoTransform cryptTransform = cipher.CreateEncryptor();
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                _cipherText = cryptTransform.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);                
                _initializationVector = cipher.IV;
                _key = cipher.Key;
            }

            //Set the out parameters
            cipherText = _cipherText;
            initializationVector = _initializationVector;
            key = _key;
        }

        private void SymmetricEncryptionAes(out string plainText,
            byte[] cipherText,
            byte[] initializationVector,
            byte[] key)
        {
            //Defaults - Keysize 256, Mode CBC, Padding PKC27
            //Aes cipher = new AesManaged();
            //Aes cipher = new AesCryptoServiceProvider();
            using (Aes cipher = Aes.Create())
            {
                //cipher.Padding = PaddingMode.Zeros;
                //cipher.Mode = CipherMode.ECB;
                cipher.Padding = PaddingMode.ISO10126;
                cipher.Key = key;
                cipher.IV = initializationVector;

                ICryptoTransform cryptTransform = cipher.CreateDecryptor();
                byte[] plainTextBytes = cryptTransform.TransformFinalBlock(cipherText, 0, cipherText.Length);
                plainText = Encoding.UTF8.GetString(plainTextBytes);
            }
        }

        private const string RJB_RSA_KEYS = "<RSAKeyValue><Modulus>6/7PGT9AO1ADPdnBZT3ZSImsuXfcuX9UvCjB1Zu0UliZha4bHZNc" +
            "Bj4VtuPJLF6ERpgJDfBqVTrT7yOMkVn4orfTlOExPedK8AWj9gTYBumGrFTDZwko1iQ5YZQ2kZGxg3QGpJhqeiEs8beFW672kXNyj5+Uye" +
            "Yp6R7su+fuiz8=</Modulus><Exponent>AQAB</Exponent><P>/AY3OtnrdVp6t5zVVpTqFVVHd88xgRgaO+Y4KIjzJKua1I0B8PEfIo" +
            "hUUai8vRbRTJZGHxwmfryQ8bkpmKXgEw==</P><Q>77fchIo2EP8jwwil//ZS+oD78eN6luKlGRljs/8wXMqaK3/x0qQHfjcBvVu0jLk1M" +
            "zdNZZPiNMaxZczbJeBFpQ==</Q><DP>hZ3eBkunNE7GJTb3PLIy8SCHhZPKEUFwFzXVrFf/YP/CVNJ1pwKPmUViPvERL8c7LDm376KDHkp" +
            "nJmEfFplLFQ==</DP><DQ>bniy3Tm8dNS/rE++AFmKH/t1ICIPCp3kK87xja/an8iWh9lsngANm/LJkHREnl1z0Oh5eIhQRLYUZq+jhq72" +
            "KQ==</DQ><InverseQ>cXngkJq+LxEl4GzRLyk1nVcHBsKUKmqSkEgWm1qD+PtEtHMW25RR8Am2AYWU0YGa35T/hbRVG9WQhl3ZOB3pCw=" +
            "=</InverseQ><D>oxVWNoNANvzHELHvdLA1/GuvogeTz9iPTOv5b00HYrR5eyji8iBIQsQaq2VkOzYhwMsFzs0qHjXmCWcOl8+OAkimP7O" +
            "FE6xmd0xPKaZTyFWYFBkFWbqbeAXCTcDuWH79DO0WSr35oZeppxzd4Zz+8p0GkXVgSaZgBfbcI40I6Mk=</D></RSAKeyValue>";

        private void EncryptUsingRSA(string plainText, out byte [] cipherText)
        {
            using (RSA cipher = RSA.Create())
            {
                //Read from a previously exported RSA set of keys in XML (public and private), does not use the key container...        
                cipher.FromXmlString(RJB_RSA_KEYS);
                byte[] data = Encoding.UTF8.GetBytes(plainText);
                cipherText = cipher.Encrypt(data, RSAEncryptionPadding.Pkcs1);
            }
        }

        private void DecryptUsingRSA(out string plainText, byte[] cipherText)
        {
            using (RSA cipher = RSA.Create())
            {
                cipher.FromXmlString(RJB_RSA_KEYS);
                //Decrypt the data
                byte[] original = cipher.Decrypt(cipherText, RSAEncryptionPadding.Pkcs1);
                plainText = Encoding.UTF8.GetString(original);
            }
        }

        private const string __msgToEncrypt = "This message will be encrypted.";

        public void Run()
        {
            byte[] cipherTextRSA = null;
            string originalMessage = null;
            //Encrypt using RSA
            EncryptUsingRSA(__msgToEncrypt, out cipherTextRSA);
            DecryptUsingRSA(out originalMessage, cipherTextRSA);
            string __cipherRSATextStr = BitConverter.ToString(cipherTextRSA).Replace("-", " ");
            Console.WriteLine("Message: {0}\n", originalMessage);
            Console.WriteLine("Encrypted Message: {0}\n", __cipherRSATextStr);
            
            //https://www.youtube.com/watch?v=rLEJLuA3hd0
            //Cryptography is the science of keeping messages secure, advantages are: 
            //1) Confidentiality – protect data from being read
            //2) Integrity – verify that data was not modified
            //3) Authentication – identify and validate a user
            //4) Non - repudiation – sender cannot deny later that he sent a message
            byte [] hash = GetSHA512Hash("This is a simple demonstration of hashing");
            //This converts the 64 byte hash into the string hex representation of byte values 
            // (shown by default as 2 hex characters per byte) that looks like 
            // "FB-2F-85-C8-85-67-F3-C8-CE-9B-79-9C-7C-54-64-2D-0C-7B-41-F6...", each pair represents
            // the byte value of 0-255.  Removing the "-" separator creates a 128 character string 
            // representation in hex
            string hexStr = BitConverter.ToString(hash).Replace("-", " ");
            Console.WriteLine("Hash value: {0}", hexStr);

            //Symmetric Algorithms 
            byte[] __cipherText = null;
            byte[] __initializationVector = null;
            byte[] __key = null;

            SymmetricEncryptionAes(__msgToEncrypt,
                out __cipherText,
                out __initializationVector,
                out __key);

            string __cipherTextStr = BitConverter.ToString(__cipherText).Replace("-", " ");
            string __initializationVectorStr = BitConverter.ToString(__initializationVector).Replace("-", " ");
            string __keyStr = BitConverter.ToString(__key).Replace("-", " ");
            //Convert byte arrays to base64 string
            string __cipherTextStrBase64 = Convert.ToBase64String(__cipherText);
            string __initializationVectorStrBase64 = Convert.ToBase64String(__initializationVector);
            string __keyStrBase64 = Convert.ToBase64String(__key);
            //After base64 encryption we can decrypt using FromBase64String 
            byte [] initVector = Convert.FromBase64String(__initializationVectorStrBase64);
            Console.WriteLine("Message: {0}\n", __msgToEncrypt);
            Console.WriteLine("Encrypted Message: {0}\n", __cipherTextStr);
            Console.WriteLine("Initialization Vector: {0}\n", __initializationVectorStr);
            Console.WriteLine("Encryption Key: {0}\n", __keyStr);
            //Decryption using key above 
            string outPlainText;
            SymmetricEncryptionAes(out outPlainText,
                __cipherText,
                __initializationVector,
                __key);
            Console.WriteLine("Decrypted Message: {0}\n", outPlainText);

            //Identity validation can be accomplished using Certification Authorities
            //Certification authorities produce digital certificates for use when signing messages.

            //MD5 hashing
            //The MD(Message Digest) 5 algorithm was created in 1991 to replace MD4.It
            //produces a hash code that is 16 bytes(128) bits in size.It has been shown that it
            //is possible to create different documents that both have the same MD5 hash
            //code, which means that the algorithm should not be used for cryptographic
            //purposes such as document signing.
            //MD5 can, however, be used to detect data corruption.Corruption due to
            //failure of a storage medium or bit errors during transmission of data is an
            //example. The main advantage MD5 has over other hashing algorithms is that it is fast.

            //SHA1 hashing Secure Hash Algorithm(SHA) 1 is a hash algorithm that produces a 20 byte
            //(160 bit) hash code value.While it is better than MD5
            //it has been shown to be vulnerable to brute force attack(where an attacker
            //tries to replicate a given hash code by trying many different data files).

            //SHA2 hashing
            //SHA2 improves on SHA1 and is actually a family of six hash functions that can
            //produce outputs that are 224, 256, 384 or 512 bits in size.
            byte[] str1 = calculateHash("Hello world");
            DumpBytes("SHA256, Hello world: ", str1);
            str1 = calculateHash("world Hello");
            DumpBytes("SHA256, world Hello: ", str1);
            str1 = calculateHash("Hemmm world");
            DumpBytes("SHA256, Hemmm world: ", str1);
            //The SHA2 algorithm is vulnerable to “length extension attacks,” where a
            //malicious person can add to the end of an existing document without changing
            //the hash code. The SHA3 standard addresses this, but it is not presently available
            //from the Security.Cryptography namespace.If you need to use SHA3,
            //there are implementations available on GitHub.

            //All C# objects provide a GetHash() method that will return an integer hash value for that object.
            //The object type provides a GetHash() method that returns a hash value
            //based on the location of that object in memory.When you create a new class you
            //should consider adding a GetHash() method that returns a hash value based
            //on the contents of that object. The string type provides a GetHash() method
            //that uses the contents of the string to calculate a hash value.
            showHash("Hello world");
            showHash("world Hello");
            showHash("Hemmm world");

            //Machine level key storage
            //User level key storage is fine if a machine only has one user, but if keys are to be
            //shared among many users, they should be stored at a machine level. Windows
            //implements machine level key storage in a folder that contains a file for each
            //key.The path to this folder on Windows 10 is usually
            //C:\ProgramData\Microsoft\Crypto\RSA\MachineKeys.
            //Once the keys have been saved, they will be loaded from the store the next
            //time the program runs. A machine level key is removed in just the same way as a
            //user key, by setting the PersistKeyInCsp property to false.

            //Note You need administrator permissions to store keys on machine level.   
            //string _containerName = "Machine Level Key";
            //string ____publicKey;
            //string ____privateKey;

            //GetRSAKeysFromMachineKeyStore(_containerName,
            //    out ____publicKey,
            //    out ____privateKey);
            //Console.WriteLine("Public key: {0}", ____publicKey);
            //Console.WriteLine("Private key: {0}", ____privateKey);

            //GetRSAKeysFromMachineKeyStore(_containerName,
            //    out ____publicKey,
            //    out ____privateKey);
            //Console.WriteLine("Public key: {0}", ____publicKey);
            //Console.WriteLine("Private key: {0}", ____privateKey);

            string containerName = "MyKeyStore";
            string ___publicKey;
            string ___privateKey;

            //User level key storage
            GetRSAKeysFromContainers(containerName,
                out ___publicKey,
                out ___privateKey);
            Console.WriteLine("Public key: {0}", ___publicKey);
            Console.WriteLine("Private key: {0}", ___privateKey);

            GetRSAKeysFromContainers(containerName,
                out ___publicKey,
                out ___privateKey);
            Console.WriteLine("Public key: {0}", ___publicKey);
            Console.WriteLine("Private key: {0}", ___privateKey);

            //RSA asymmetric encryption to create public and private keys
            //The RSACryptoServiceProvider class in the System.Security.Cryptography namespace will perform encryption
            //and decryption of data using this standard.
            //The RSACryptoServiceProvider instance provides Encrypt and Decrypt methods to encrypt and decrypt byte arrays.
            string publicKey, privateKey;
            GetRSAKeys(out publicKey, out privateKey);
            Console.WriteLine("Public key: {0}", publicKey);
            Console.WriteLine("Private key: {0}", privateKey);

            byte[] _cipherText;
            string _plainText = "This is my super secret data";
            RSAEncrypt(_plainText, publicKey, out _cipherText);
            Console.WriteLine("String to encrypt: {0}", _plainText);
            DumpBytes("Encrypted: ", _cipherText);

            string __plainText;
            RSADecrypt(_cipherText, privateKey, out __plainText);
            Console.WriteLine("Decrypted message: {0}", __plainText);

            //AES(Advanced Encryption Standard) symmetric encryption, successor of DES (Data Encryption Standard) 
            //The Advanced Encryption Standard(AES) is used worldwide to encrypt data.It
            //supersedes the Data Encryption Standard(DES). It is a symmetric encryption
            //system.You use the same key to encrypt and decrypt the data.
            //The Aes class can be found in the System.Security.Cryptography namespace.
            //The Aes class provides options that allow you to set the length of the key to
            //use.If you want greater security you can use longer keys, but this will slow
            //down the encryption and decryption process.
            string plainText = "This is my super secret data";
            byte[] initializationVector;
            byte[] cipherText;
            byte[] key;

            AESEncryption(plainText, 
                out cipherText, 
                out initializationVector,
                out key);

            // Dump out our data
            Console.WriteLine("String to encrypt: {0}", plainText);
            DumpBytes("Key: ", key);
            DumpBytes("Initialization Vector: ", initializationVector);
            DumpBytes("Encrypted: ", cipherText);

            //The decryption process
            string decryptedText;
            AESDecryption(out decryptedText, cipherText, initializationVector, key);
            Console.WriteLine("String after decryption: {0}", decryptedText);
        }

        private void AESDecryption(out string _decryptedText,
            byte[] _cipherText,
            byte[] _initializationVector,
            byte[] _key)
        {
            string decryptedText;

            using (Aes aes = Aes.Create())
            {
                // Configure the aes instances with the key and
                // initialization vector to use for the decryption
                aes.Key = _key;
                aes.IV = _initializationVector;
                // Create a decryptor from aes
                // should be wrapped in using for production code
                ICryptoTransform decryptor = aes.CreateDecryptor();

                using (MemoryStream decryptStream = new MemoryStream(_cipherText))
                {
                    using (CryptoStream decryptCryptoStream =
                    new CryptoStream(decryptStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(decryptCryptoStream))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            decryptedText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            
            _decryptedText = decryptedText;
        }

        private void AESEncryption(string plainText, 
            out byte[] _cipherText,
            out byte[] _initializationVector,
            out byte[] _key)
        {
            //string plainText = "This is my super secret data";
            // byte array to hold the encrypted message
            byte[] cipherText;
            // byte array to hold the key that was used for encryption
            byte[] key;
            // byte array to hold the initialization vector that was used for encryption
            byte[] initializationVector;

            // Create an Aes instance
            // This creates a random key and initialization vector
            using (Aes aes = Aes.Create())
            {
                //The initialization vector adds security to a particular key value by specifying a
                //random start point in the stream of encryption values that will be produced to
                //encrypt the input.If every encryption stars at the beginning of the encryption
                //stream, there is a chance that the repeated use of a particular encryption key
                //provides a large enough set of encrypted messages for an eavesdropper to break
                //the code. By using a different initialization vector for each message, you can
                //remove this possibility.The receiver of the message will need both the key and
                //the initialization vector value to decrypt the code.
                // copy the key and the initialization vector
                key = aes.Key;
                initializationVector = aes.IV;
                // create an encryptor to encrypt some data
                // should be wrapped in using for production code
                ICryptoTransform encryptor = aes.CreateEncryptor();

                // Create a new memory stream to receive the
                // encrypted data.
                using (MemoryStream encryptMemoryStream = new MemoryStream())
                {
                    // create a CryptoStream, tell it the stream to write to
                    // and the encryptor to use. Also set the mode
                    using (CryptoStream encryptCryptoStream = new CryptoStream(
                        encryptMemoryStream,
                        encryptor, 
                        CryptoStreamMode.Write))
                    {
                        // make a stream writer from the cryptostream
                        using (StreamWriter swEncrypt = new StreamWriter(encryptCryptoStream))
                        {
                            //Write the secret message to the stream.
                            swEncrypt.Write(plainText);
                        }
                        // get the encrypted message from the stream
                        cipherText = encryptMemoryStream.ToArray();
                    }
                }
            }

            _cipherText = cipherText;
            _initializationVector = initializationVector;
            _key = key;
        }

        private static void DumpBytes(string title, byte[] bytes)
        {
            Console.Write(title);
            foreach (byte b in bytes)
            {
                Console.Write("{0:X} ", b);
            }
            Console.WriteLine();
        }
    }
}
