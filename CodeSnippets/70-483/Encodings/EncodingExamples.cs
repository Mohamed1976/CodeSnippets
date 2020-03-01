using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

//https://docs.microsoft.com/en-us/dotnet/api/system.text.encoding.getencoding?view=netframework-4.8
//UTF-8 vs UTF-16, UTF-32
//https://www.johndcook.com/blog/2019/09/09/how-utf-8-works/
namespace _70_483.Encodings
{
    public class EncodingExamples
    {
        public EncodingExamples()
        {

        }

        const string pdfFile = @"C:\Users\moham\Desktop\70-483\[Tiberiu_Covaci,_Rod_Stephens,_Vincent_Varallo,_Ge(z-lib.org).pdf";
        public void Run()
        {
            byte[] _bytes = ConvertStringToByte("Test string with UTF16 encoding.", Encoding.Unicode);
            DumpBytes("UTF-16 to bytes: ", _bytes);
            string str = ConvertByteArrayToString(_bytes, Encoding.Unicode);
            Console.WriteLine("{0}", str);

            Encoding __encoding = GetEncodingV2(pdfFile);
            Console.WriteLine("GetEncodingV2: {0}", __encoding);

            Encoding encoding = GetEncoding(pdfFile);
            Console.WriteLine($"Encoding: {encoding}");
            //FileMode.Open Open an existing file. An exception is thrown if the file
            //does not exist. This mode can be used for reading or writing.
            using (FileStream inputStream = new FileStream(pdfFile, FileMode.Open, FileAccess.Read))
            {
                long fileLength = inputStream.Length;
                byte[] readBytes = new byte[fileLength];
                inputStream.Read(readBytes, 0, (int)fileLength);
                //Calculate the SHA256 HASH
                HashAlgorithm hasher = SHA256.Create();
                //For example if you want to use SHA1 hash algorithm 
                //HashAlgorithm hasher = new SHA1Managed();
                byte[] hash = hasher.ComputeHash(readBytes);
                Console.WriteLine($"File size: {fileLength} (bytes)");
                DumpBytes("File Hash: ", hash);
            }

            HashAlgorithm _hasher = SHA256.Create();
            byte[] bytes = File.ReadAllBytes(pdfFile);
            byte[] _hash = _hasher.ComputeHash(bytes);
            Console.WriteLine($"File size: {bytes.Length} (bytes)");
            DumpBytes("File Hash: ", _hash);

            // and save back - System.IO.File.WriteAll* makes sure all bytes are written properly.
            //File.WriteAllBytes(pdfFilePath, bytes);
        }

        //Method to Convert Byte[] to string
        private static string ConvertByteArrayToString(Byte[] ByteOutput, Encoding encoding)
        {
            string StringOutput = encoding.GetString(ByteOutput);
            return StringOutput;
        }

        //Method to Convert String to Byte[]
        public static byte[] ConvertStringToByte(string Input, Encoding encoding)
        {
            return encoding.GetBytes(Input);
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

        /// <summary>
        /// Determines a text file's encoding by analyzing its byte order mark (BOM).
        /// Defaults to ASCII when detection of the text file's endianness fails.
        /// </summary>
        /// <param name="filename">The text file to analyze.</param>
        /// <returns>The detected encoding.</returns>
        /// https://stackoverflow.com/questions/3825390/effective-way-to-find-any-files-encoding
        public static Encoding GetEncodingV2(string filename)
        {
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
            }

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;
            return Encoding.ASCII;
        }

        /// <summary
        /// Get File's Encoding
        /// </summary>
        /// <param name="filename">The path to the file
        private static Encoding GetEncoding(string filename)
        {
            //Encoding for the OS's current ANSI code page is returned by default.
            Encoding encoding = Encoding.Default;

            try
            {
                // This is a direct quote from MSDN:  
                // The CurrentEncoding value can be different after the first
                // call to any Read method of StreamReader, since encoding
                // autodetection is not done until the first call to a Read method.
                //https://docs.microsoft.com/en-us/dotnet/api/system.io.streamreader.currentencoding?view=netframework-4.8

                using (StreamReader reader = new StreamReader(filename, Encoding.Default, true))
                {
                    if (reader.Peek() >= 0) // you need this!
                        reader.Read();

                    encoding = reader.CurrentEncoding;
                }                
            }
            catch (Exception ex)
            {
                encoding = Encoding.Default;
                Console.WriteLine("GetEncoding failed: {0}", ex.ToString());
            }
            finally
            {
                encoding = Encoding.Default;
            }

            return encoding;
        }
    }
}
