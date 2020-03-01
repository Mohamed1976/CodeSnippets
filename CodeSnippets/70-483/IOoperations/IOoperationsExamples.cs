using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

//File access is a very slow activity when compared with the speed of modern processors
//A stream is a software object that represents a stream of data.The.NET
//framework provides a Stream class that serves as the parent type for a range of
//classes that can be used to read and write data.There are three ways that a
//program can interact with a stream:
//1) Write a sequence of bytes to a stream
//2) Read a sequence of bytes from a stream
//3) Position the “file pointer” in a stream
//The Stream class is abstract and serves as a template for streams that connect to actual storage resources.
//The file pointer is the position in a stream where the next read or write
//operation will take place. A program can use the Seek method provided by the stream to set this position.
//System.IO.Stream      Abstract class, inherited by the following types  
//      System.IO.BufferedStream
//      System.IO.FileStream
//      System.IO.MemoryStream
//      System.IO.PipeStream
//      System.IO.NetworkStream

//The child classes all contain the stream behaviors that allow data to be
//transferred, for example the Stream method Write can be used on any of
//them to write bytes to that stream.However, how each type of stream created is
//dependent on that stream type.For example, to create a FileStream a
//program must specify the path to the file and how the file is going to be used. To
//create a MemoryStream a program must specify the buffer in memory to be used.

//Unicode is a mapping of character symbols to numeric values.The UTF8
//encoding maps Unicode characters onto 8-bit values that can be stored in arrays
//of bytes.Most text files are encoded using UTF8. The Encoding class also
//provides support for other encoding standards including UTF32(Unicode
//encoding to 32-bit values) and ASCII. The GetBytes encoding method takes 
//a C# string and returns the bytes that represent that string in the specified 
//encoding.The GetString decoding method takes an array of bytes and returns the 
//string that a buffer full of bytes represents.

//The Stream class implements the IDisposable interface. This means that any 
//objects derived from the Stream type must also implement the interface. 
//This means that we can use the C# using construction to ensure
//that files are closed when they are no longer required.

//Using the Directory and DirectoryInfo class you can manipulate directories.
//Note that if a program attempts to delete a directory that is not
//empty an exception will be thrown.

//You can have a relative path or absolute path.
//The absolute path contains drive letter and identifies 
//all the sub-directories in the path to the file.
//DirectoryInfo startDir = new DirectoryInfo(@"..\..\..\..");

//Overview of I/O classes 


namespace _70_483.IOoperations
{
    public class IOoperationsExamples
    {
        public IOoperationsExamples()
        {

        }

        static void FindFiles(DirectoryInfo dir, string searchPattern, ref int fileCount)
        {
            foreach (DirectoryInfo directory in dir.GetDirectories())
            {
                FindFiles(directory, searchPattern, ref fileCount);
            }
            FileInfo[] matchingFiles = dir.GetFiles(searchPattern);
            foreach (FileInfo fileInfo in matchingFiles)
            {
                Console.WriteLine(fileInfo.FullName);
                fileCount++;
            }
        }

        static void FindFilesV2(string path, string searchPattern, ref int fileCount)
        {
            foreach(string dir in Directory.EnumerateDirectories(path))
            {
                FindFilesV2(dir, searchPattern, ref fileCount);
            }

            foreach(string filename in Directory.EnumerateFiles(path, searchPattern))
            {
                Console.WriteLine(filename);
                fileCount++;
            }
        }

        private Task<string> GetWebpageContent(string Url)
        {
            WebClient client = new WebClient();
            return client.DownloadStringTaskAsync(Url);
        }

        private Task<string> readWebpage(string uri)
        {
            HttpClient client = new HttpClient();
            return client.GetStringAsync(uri);
        }

        private Task WriteBytesAsync(string filename, byte[] items)
        {
            using (FileStream outStream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write))
            {
                return outStream.WriteAsync(items, 0, items.Length);
            }
        }

        //Path.GetTempFileName() can be used to generate filenames
        private readonly string fileName2 = "Test#@@#.dat";

        public void Run()
        {
            using (StreamReaderEnumerator streamReaderEnumerator =
                new StreamReaderEnumerator(@"C:\Users\moham\Desktop\Notes\Notes001.txt"))
            {
                while (streamReaderEnumerator.MoveNext())
                {
                    Console.WriteLine(streamReaderEnumerator.Current);
                }
            }

            return;
            //The MemoryStream creates a stream whose backing store is memory.
            //MemoryStream to FileStream
            string memString = "Memory test string !!";
            // convert string to stream
            byte[] buffer = Encoding.UTF8.GetBytes(memString);
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                using (FileStream file = new FileStream(fileName2, FileMode.Truncate, FileAccess.Write))
                {
                    ms.WriteTo(file);
                }
            }

            //Read back using FileStream MemoryStream
            using (MemoryStream ms = new MemoryStream())
            {
                using (FileStream file = new FileStream(fileName2, FileMode.Open, FileAccess.Read))
                {
                    file.CopyTo(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    byte[] buff = new byte[ms.Length];
                    int nrOfBytesRead = ms.Read(buff, 0, (int)ms.Length);
                    //We can also use for loop to fill buffer 
                    //for(int cnt = 0; cnt < ms.Length; cnt++ )
                    //{
                    //    //ReadByte() reads byte and casts it to int 
                    //    buff[cnt] = Convert.ToByte(ms.ReadByte());
                    //}

                    Console.WriteLine("Text from file: {0}, {1}", ms.Length, Encoding.UTF8.GetString(buff));                    
                }
            }

            //https://docs.microsoft.com/en-us/dotnet/api/system.io.filestream.seek?view=netframework-4.8
            //The Stream abstract class allows reading, writing, seeking 
            Random random = new Random();
            // Create random data to write to the file.
            byte[] dataArray = new byte[100];
            random.NextBytes(dataArray);
            using (FileStream fileStream2 = new FileStream(fileName2, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                foreach(byte b in dataArray)
                {
                    fileStream2.WriteByte(b);
                }

                //Now we use the fileseek to rewind file pointer and read all back. 
                fileStream2.Seek(0, SeekOrigin.Begin);

                for(int cnt = 0; cnt < fileStream2.Length; cnt++)
                {

                    byte read = (byte)fileStream2.ReadByte();
                    //The 2 in X2 mean 2 digits. Eg 0A. Where as X4 would show 000A.
                    //string.Format("{0:X2}", myByte), and since C# 6, $"{myByte:X2}
                    //The case of the X in the format specifier will affect the case of the resulting hex digits. ie.
                    //255.ToString("X2") returns FF, whereas 255.ToString("x2") returns ff.
                    Console.WriteLine("Byte[{0}], {1:X2}=={2:X2}, {3}", 
                        cnt,
                        read,
                        dataArray[cnt],
                        read == dataArray[cnt] ? 
                        "The data was written to " + fileStream2.Name + " and verified."
                        : "Error writing data.");                   
                }

                //Retrieve the last 10 bytes in reverse order;
                for (int offset = 1; offset <= fileStream2.Length; offset++)
                {
                    fileStream2.Seek(-offset, SeekOrigin.End);
                    Console.WriteLine("Bytes in reversed order: [{0}]=={1:x2}", offset, (byte)fileStream2.ReadByte());
                    //Only last 10 bytes are printed in reverse order, break out of loop
                    if (offset == 10)
                    {
                        break;
                    }
                }

                //Print last 10 bytes before EOF 
                fileStream2.Seek(90, SeekOrigin.Begin);
                int nextByte = 0x00;
                //ReadByte(): The byte, cast to an System.Int32, or -1 if the end of the stream has been reached.
                while ((nextByte = fileStream2.ReadByte()) != -1 )
                {
                    Console.WriteLine($"[{fileStream2.Position}]=={Convert.ToByte(nextByte).ToString("x2")}");
                }
            }

            //The file operations provided by the File class do not have any asynchronous
            //versions, so the FileStream class should be used instead. Code below show shows a function
            //that writes an array of bytes to a specified file using asynchronous writing.
            //
            //try
            //{
            //      await WriteBytesAsync(filename, bytes);
            //}
            //catch(Exception ex)
            //{
            //      Console.WriteLine("WriteBytesAsync exception: {0}", ex.Message);
            //}

            //C# programs can create network socket objects that can
            //communicate over the network by sending unacknowledged datagrams using
            //UDP (User Datagram Protocol) or creating managed connections using TCP/IP
            //(Transmission Control Protocol).
            //Read and write from the network by using classes in the System.Net namespace
            //The classes in the System.Net namespace that allow a program to communicate with servers using the HTTP
            //(HyperText Transport Protocol). This protocol operates on top of a TCP/IP
            //connection.In other words, TCP/IP provides the connection between the server
            //and client systems and HTTP defines the format of the messages that are exchanged over that connection.            
            //An HTTP client, for example a web browser, creates a TCP/IP connection to a
            //server and makes a request for data by sending the HTTP GET command.The
            //server will then respond with a page of information.After the response has been
            //returned to the client the TCP/IP connection is closed.
            //The information returned by the server is formatted using HTML (HyperText
            //Markup Language) and rendered by the browser.In the case of an ASP(Active
            //Server Pages) application the HTML document may be produced dynamically by software,
            //rather than being loaded from a file stored on the server.
            //HTTP was originally used for the sharing of human - readable web pages.
            //However, now an HTTP request may return an XML or JSON formatted
            //document that describes data in an application.
            //The REST(REpresentational State Transfer) architecture uses the GET, PUT,
            //POST and DELETE operations of HTTP to allow a client to request a server to
            //perform functions in a client-server application.The fundamental operation that
            //is used to communicate with these and other servers is the sending of a “web
            //request” to a server to perform an HTML command on the server            
            //Three different ways to interact with web servers WebRequest, WebClient, and HttpClient.

            //HttpClient
            //The HTTPClient is important because it is the way in which a Windows
            //Universal Application can download the contents of a website. Unlike the
            //WebRequest and the WebClient classes, an HTTPClient only provides asynchronous methods.
            //As with file handling, loading information from the Internet is prone to error.
            //Network connections may be broken or servers may be unavailable. This means
            //that web request code should be enclosed in appropriate exception handlers.
            //
            //try
            //{
            //      await readWebpage(uri);
            //}
            //catch(Exception ex)
            //{
            //      Console.WriteLine("Exception calling readWebpage: {0}.", ex.Message);
            //}

            //WebClient
            //The WebClient class provides a simpler and quicker way of reading the text from a web server.
            WebClient client = new WebClient();
            string webContent = client.DownloadString("https://www.microsoft.com");
            Console.WriteLine("https://www.microsoft.com size: {0}", webContent.Length);
            //The WebClient class also provides methods that can be used to read from the server asynchronously.
            //private Task<string> GetWebpageContent(string Url)

            //WebRequest
            //The WebRequest class is an abstract base class that specifies the behaviors of a
            //web request.It exposes a static factory method called Create, which is given a
            //universal resource identifier(URI) string that specifies the resource that is to be
            //used.The Create method inspects the URI it is given and returns a child of the
            //WebRequest class that matches that resource.The Create method can create
            //HttpWebRequest, FtpWebRequest, and FileWebRequest objects.
            //In the case of a web site, the URI string will start with “http” or “https” and the Create method
            //will return an HttpWebRequest instance.
            //The GetResponse method on an HttpWebRequest returns a WebResponse
            //instance that describes the response from the server.Note that this response is
            //not the web page itself, but an object that describes the response from the server.
            //To actually read the text from the webpage a program must use the
            //GetResponseStream method on the response to get a stream from which the webpage text can be read.
            WebRequest webRequest = WebRequest.Create("https://www.microsoft.com");
            WebResponse webResponse = webRequest.GetResponse();
            //Note that the use of using around the StreamReader ensures that the input
            //stream is closed when the web page response has been read.It is important that
            //either this stream or the WebResponse instance are explicitly closed after use, as
            //otherwise the connection will not be reused and a program might run out of web connections.
            using (StreamReader responseReader = new StreamReader(webResponse.GetResponseStream()))
            {
                string siteText = responseReader.ReadToEnd();
                Console.WriteLine("https://www.microsoft.com size: {0}",siteText.Length);
            }
            
            //Using WebRequest instances to read web pages works, but it is rather
            //complicated. It does, however, have the advantage that a program can set a wide
            //range of properties on the web and request to tailor it to particular server
            //requirements.This flexibility is not available on some of the other methods we are going to consider.
            //The above is synchronous, in that the program will wait for the
            //web page response to be generated and the response to be read.It is possible to
            //use the WebRequest in an asynchronous manner so that a program is not paused
            //in this way.However, the programmer has to create event handlers to be called
            //when actions are completed.

            //paths
            //A path defines the location of a file on a storage device.
            //Paths can be relative or absolute. A relative path specifies the location of a file
            //relative to the folder in which the program is presently running.
            //When expressing paths, the character “.” (period)has a special significance.A
            //single period “.” means the current directory. A double period “..” means the
            //directory above the present one.
            //The @ character at the start of the string literal marks the
            //string as a verbatim string.This means that any escape characters in the string
            //will be ignored. This is useful because otherwise the backslash characters in the
            //string might be interpreted as escape characters.
            //The path to a file contains two elements: the directories in the path and the
            //name of the file in the directory. The Path class provides a lot of very helpful
            //methods that can be used to work with paths in programs.It provides methods to
            //remove filenames from full paths, change the extension on a filename, and
            //combine filenames and directory paths.
            //The Path class is very useful and should always be used in preference to
            //manually working with the path strings.The Path class also provides methods
            //that can generate temporary filenames.
            //An absolute path includes the drive letter and identifies all the sub-directories in the path to the file.
            string fullName = @"c:\users\rob\Documents\test.txt";
            string dirName = Path.GetDirectoryName(fullName);
            string __fileName = Path.GetFileName(fullName);
            string fileExtension = Path.GetExtension(fullName);
            string lisName = Path.ChangeExtension(fullName, ".lis");
            string newTest = Path.Combine(dirName, "newtest.txt");
            Console.WriteLine("Full name: {0}", fullName);
            Console.WriteLine("File directory: {0}", dirName);
            Console.WriteLine("File name: {0}", __fileName);
            Console.WriteLine("File extension: {0}", fileExtension);
            Console.WriteLine("File with lis extension: {0}", lisName);
            Console.WriteLine("New test: {0}", newTest);
            //The DirectoryInfo class provides a method called GetFiles that can be
            //used to get a collection of FileInfo items that describe the files in a directory.
            //One overload of GetFiles can accept a search string. Within the search string
            //the character* can represent any number of characters and the character ? can
            //represent a single character.
            DirectoryInfo startDir = new DirectoryInfo(@"..\..\..\..");
            string searchString = "*.cs";
            int fileCount = 0;
            FindFiles(startDir, searchString, ref fileCount);
            Console.WriteLine("Count files: {0}", fileCount);
            //The Directory class provides a method called EnumerateFiles that can also be
            //used to enumerate files in this way.
            fileCount = 0;
            FindFilesV2(@"..\..\..\..", searchString, ref fileCount);
            Console.WriteLine("Count files: {0}", fileCount);
            
            const string dir = "TestDir";
            //An instance of the DirectoryInfo class describes the contents of one
            //directory.The class also provides methods that can be used to create and manipulate directories.
            DirectoryInfo localDir = new DirectoryInfo(dir);
            localDir.Create();
            if (localDir.Exists)
            {
                Console.WriteLine("Directory created successfully");
                localDir.Delete();
            }

            if (!localDir.Exists)
            {
                Console.WriteLine("Directory deleted successfully");
            }

            //Directory and DirectoryInfo classes
            //A file system can create files that contain collections of file information items.
            //These are called directories or folders. Directories can contain directory
            //information about directories, which allows a user to nest directories to create tree structures.
            //As with files, there are two ways to work with directories: the Directory
            //class and the DirectoryInfo class. The Directory class is like the File class.
            //It is a static class that provides methods that can enumerate the contents of
            //directories and create and manipulate directories.
            Directory.CreateDirectory(dir);
            if (Directory.Exists(dir))
            {
                Console.WriteLine("Directory created successfully");
                Directory.Delete(dir);
            }

            if (!Directory.Exists(dir))
            {
                Console.WriteLine("Directory deleted successfully");
            }

            //FileInfo
            //A file system maintains information about each file it stores. This includes the
            //name of the file, permissions associated with the file, dates associated with the
            //creation, modification of the file, and the physical location of the file on the
            //storage device. The filesystem also maintains attribute information about each
            //file.The attribute information is held as a single value with different bits in the
            //value indicating different attributes.We can use logical operators to work with
            //these values and assign different attributes to a file.The available attributes are as follows:
            //1) FileAttributes.Archive The file has not been backed up yet. The attribute
            //   will be cleared when/if the file is backed up.
            //2) FileAttributes.Compressed The file is compressed.This is not something
            //   that our program should change.
            //3) FileAttributes.Directory The file is a directory. This is not something our
            //   program should change.
            //4) FileAttributes.Hidden The file will not appear in an ordinary directory listing.
            //5) FileAttributes.Normal This is a normal file with no special attributes.This
            //   attribute is only valid when there are no other attributes assigned to the file.
            //6) FileAttributes.ReadOnly The file cannot be written.
            //7) FileAttributes.System The file is part of the operating system and is used by it.
            //8) FileAttributes.Temporary The file is a temporary file that will not be
            //   required when the application has finished.The file system will attempt to
            //   keep this file in memory to improve performance.
            //You can use a FileInfo instance to open a file for reading and writing,
            //moving a file, renaming a file, and also modifying the security settings on a file.
            //Some of the functions provided by a FileInfo instance duplicate those
            //provided by the File class. The File class is most useful when you want to
            //perform an action on a single file.The FileInfo class is most useful when
            //you want to work with a large number of files.
            const string filePath = "TextFileInfo.txt";
            File.WriteAllText(path: filePath, contents: "This text goes in the file");
            FileInfo info = new FileInfo(filePath);
            Console.WriteLine("Name: {0}", info.Name);
            Console.WriteLine("Full Path: {0}", info.FullName);
            Console.WriteLine("Last Access: {0}", info.LastAccessTime);
            Console.WriteLine("Creation Time: {0}", info.CreationTime);
            Console.WriteLine("Last WriteTime: {0}", info.LastWriteTime);
            Console.WriteLine("Length: {0}", info.Length);
            Console.WriteLine("Attributes: {0}", info.Attributes);
            Console.WriteLine("Make the file read only");
            info.Attributes |= FileAttributes.ReadOnly;
            Console.WriteLine("Attributes: {0}", info.Attributes);
            Console.WriteLine("Remove the read only attribute");
            info.Attributes &= ~FileAttributes.ReadOnly;
            Console.WriteLine("Attributes: {0}", info.Attributes);

            //the DriveInfo class in the System.IO namespace can be
            //used to obtain information about the drives attached to a system.
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                Console.Write("Name:{0} ", drive.Name);
                if (drive.IsReady)
                {
                    Console.Write(" Type:{0}", drive.DriveType);
                    Console.Write(" Format:{0}", drive.DriveFormat);
                    Console.Write(" Free space:{0}", drive.TotalFreeSpace);
                }
                else
                {
                    Console.Write(" Drive not ready");
                }
                Console.WriteLine();
            }

            //Handle stream exceptions, Our application may try to open a file that does not
            //exist, or a given storage device may become full during writing.It is also
            //possible that threads in a multi-threaded application can “fight” over files. If one
            //thread attempts to access a file already in use by another, this will lead to exceptions being thrown.
            //With this in mind you should ensure that production code that opens and
            //interacts with streams is protected by try–catch constructions.There are a set
            //of file exceptions that are used to indicate different error conditions.
            try
            {
                //Use the File class
                //The File class is a “helper” class that makes it easier to work with files. It
                //contains a set of static methods that can be used to append text to a file, copy a
                //file, create a file, delete a file, move a file, open a file, read a file, and manage file security.
                const string _fileName = "TextFile.txt";
                const string _destFileName = "CopyTextFile.txt";

                File.WriteAllText(path: _fileName, contents: "This text goes in the file");
                File.AppendAllText(path: _fileName, contents: " - This goes on the end");
                //Read content of file.
                if (File.Exists(_fileName))
                {
                    Console.WriteLine("Text File exists");
                    byte[] fileContent = File.ReadAllBytes(path: _fileName);
                    string _fileContent = Encoding.UTF8.GetString(fileContent);
                    Console.WriteLine(_fileContent);
                    string contents = File.ReadAllText(path: _fileName);
                    Console.WriteLine("File contents: {0}", contents);
                    File.Copy(sourceFileName: _fileName, destFileName: _destFileName, overwrite: true);
                }

                //using (TextReader reader = File.OpenText(path: _destFileName))
                using (StreamReader reader = File.OpenText(path: _destFileName))
                {
                    string text = reader.ReadToEnd();
                    Console.WriteLine("Copied text: {0}", text);
                }
            }
            catch(FileNotFoundException ex)
            {
                Console.WriteLine("FileNotFoundException occured while writing to file: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured while writing to file: {0}", ex.Message);
            }

            const string fileName = "OutputText.txt";
            //Control file use with FileMode and FileAccess
            //The base Stream class provides properties that a program can use to determine the
            //abilities of a given stream instance(whether a program can read, write, or seek
            //on this stream). The FileMode enumeration is used in the constructor of a FileStream to
            //indicate how the file is to be opened. 
            //The following FileMode modes are available:
            //1) FileMode.Append Open a file for appending to the end. If the file exists,
            //   move the seek position to the end of this file.If the file does not exist;
            //   create it. This mode can only be used if the file is being opened for writing.
            //2) FileMode.Truncate Open a file for writing and remove any existing contents.
            //3) FileMode.Create Create a file for writing. If the file already exists, it is
            //   overwritten. Note that this means the existing contents of the file are lost.
            //4) FileMode.CreateNew Create a file for writing. If the file already exists, an
            //   exception is thrown.
            //5) FileMode.Open Open an existing file. An exception is thrown if the file
            //   does not exist.This mode can be used for reading or writing.
            //6) FileMode.OpenOrCreate Open a file for reading or writing. If the file does
            //   not exist, an empty file is created.This mode can be used for reading or writing.
            //The following FileAccess modes are available: 
            //1) FileAccess.Read Open a file for reading.
            //2) FileAccess.ReadWrite Open a file for reading or writing.
            //3) FileAccess.Write Open a file for writing.

            //If a file stream is used in a manner that
            //is incompatible with how it is opened, the action will fail with an exception.

            //The FileStream object provides a stream instance connected to a file.
            //Writing to a file and auto dispose FileStream
            //A stream can only transfer arrays of bytes to and from the storage device, so the
            //program below uses the Encoding class from the System.Text
            //namespace.The UTF8 property of this class provides methods that will encode and decode Unicode text.
            //Unicode is a mapping of character symbols to numeric values. The UTF8
            //encoding maps Unicode characters onto 8 - bit values that can be stored in arrays
            //of bytes.Most text files are encoded using UTF8.The Encoding class also
            //provides support for other encoding standards including UTF32(Unicode
            //encoding to 32-bit values) and ASCII.
            //The GetBytes encoding method takes a C# string and returns the bytes that
            //represent that string in the specified encoding.The GetString decoding
            //method takes an array of bytes and returns the string that a buffer full of bytes represents.
            using (FileStream outputStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                string outputMessageString = "Hello world";
                byte[] outputMessageBytes = Encoding.UTF8.GetBytes(outputMessageString);
                outputStream.Write(outputMessageBytes, 0, outputMessageBytes.Length);
                //outputStream.Close(); , because of using dispose will close the filestream
            }

            //The Stream class implements the IDisposable interface shown in Skill 2.4.
            //This means that any objects derived from the Stream type must also implement
            //the interface. This means that we can use the C# using construction to ensure
            //that files are closed when they are no longer required.
            using (FileStream inputStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                long fileLength = inputStream.Length;
                byte[] readBytes = new byte[fileLength];
                inputStream.Read(readBytes, 0, (int)fileLength);
                string readString = Encoding.UTF8.GetString(readBytes);
                //inputStream.Close();, because of using dispose will close the filestream
                Console.WriteLine("Read message: {0}", readString);
            }

            //TextWriter, TextReader, StreamWriter and StreamReader 
            //C# language provides stream classes that make it much easier to
            //work with text.The TextWriter and TextReader classes are abstract
            //classes that define a set of methods that can be used with text.
            //The StreamWriter class extends the TextWriter class to provide a
            //class that we can us to write text into streams.
            using (StreamWriter streamWriter = new StreamWriter(fileName))
            {
                streamWriter.WriteLine("Hello world");
            }

            using (StreamReader streamReader = new StreamReader(fileName))
            {
                string read = streamReader.ReadToEnd();
                Console.WriteLine(read);
            }

            const string compressedFile = "CompText.zip";

            //Chain streams together
            //The Stream class has a constructor that will accept another stream as a
            //parameter, allowing the creation of chains of streams.
            //The code below shows how to use the GZipStream from the System.IO.Compression namespace in a
            //chain of streams that will save and load compressed text.
            using (FileStream writeFile = new FileStream(compressedFile, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (GZipStream writeFileZip = new GZipStream(writeFile, CompressionMode.Compress))
                {
                    using (StreamWriter writeFileText = new StreamWriter(writeFileZip))
                    {
                        writeFileText.Write("This text will be compressed.");
                    }
                }
            }

            using (FileStream readFile = new FileStream(compressedFile, FileMode.Open, FileAccess.Read))
            {
                using (GZipStream readFileZip = new GZipStream(readFile, CompressionMode.Decompress))
                {
                    using (StreamReader readFileText = new StreamReader(readFileZip))
                    {
                        string _read = readFileText.ReadToEnd();
                        Console.WriteLine(_read);
                    }
                }
            }
        }
    }
}
