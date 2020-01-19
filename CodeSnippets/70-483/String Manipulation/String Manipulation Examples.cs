using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace _70_483.String_Manipulation
{
    //The C# type string (with a lower-case s) is mapped onto the .NET type String(with an upper-case S).
    //The theoretical limit on the maximum size of a string is around 2GB.
    //Strings in C# are managed by reference, but string values are immutable, which
    //means that the contents of a string can’t be changed once the string has been
    //created.If you want to change the contents of a string you have to make a new
    //string. All string editing functions return a new string with the edited content.
    //The downside of this approach is that if you perform a lot of string editing you
    //will end up creating a lot of string objects. (StringBuilder provides a mutable string)

    //When a program is compiled the compiler uses a process called string interning
    //to improve the efficiency of string storage. Consider we set two string variables, 
    //s1 and s2, to the text “hello.” The compiler will save program memory by making both 
    //s1 and s2 refer to the same string object with the content “hello.”
    //The process of string interning makes string comparison quicker.If two string
    //references are equal it means that they both contain the same string, there is no
    //need to compare the text in the two strings to see if it is the same.Note that if the
    //string references are different; this doesn’t mean that the strings are definitely
    //not different.It means that the program must compare each of the characters in
    //the strings to determine if they are the same. String interning only happens when the program is compiled.
    //Interning slows the program down. However, you can force a given string to be interned at run time by using the
    //Intern method provided by the string type. The statement here makes the string s3 refer to an interned version of the string.    
    //string s3 = string.Intern("Hello");
    class String_Manipulation_Examples
    {
        private sealed class StringComparer
        {
            public StringComparer(CultureInfo selectedCultureInfo, 
                StringComparison selectedStringComparison,
                string strX,
                string strY)
            {
                SelectedCultureInfo = selectedCultureInfo;
                SelectedStringComparison = selectedStringComparison;
                stringX = strX;
                stringY = strY;
            }

            public CultureInfo SelectedCultureInfo { get; }

            public StringComparison SelectedStringComparison { get; }

            public string stringX { get; }
            
            public string stringY { get; }
        }


        public String_Manipulation_Examples()
        {
        }

        //https://docs.microsoft.com/en-us/dotnet/api/system.stringcomparison?view=netframework-4.8
        private void StringComparisonCulture(object stringComparer)
        {
            StringComparer stringComparerObj = stringComparer as StringComparer;
            //As operator return null when unable to cast to requested type, 
            //In contrast the cast operator throws an exception if unable to cast    
            if(stringComparerObj == null)
                throw new ArgumentException($"StringComparisonCulture(object stringComparer), wrong argument type: [{stringComparer.GetType().ToString()}].");

            Thread.CurrentThread.CurrentCulture = stringComparerObj.SelectedCultureInfo;
            bool equal = stringComparerObj.stringX.Equals(stringComparerObj.stringY, stringComparerObj.SelectedStringComparison);
            Console.WriteLine($"{stringComparerObj.stringX} = {stringComparerObj.stringY} ({stringComparerObj.SelectedStringComparison}): {equal}");
        }

        public void Run()
        {
            DateTime dt = DateTime.Now;
            //Display only the date portion of a DateTime according to the French culture
            Console.WriteLine("French date portion {0}.", dt.ToString("d", new CultureInfo("fr-FR")));

            //You have seen how to use format strings that incorporate placeholders in them.
            //Each placeholder is enclosed in braces { and }
            //and a placeholder relates to a
            //particular value which is identified by its position in the arguments that follow
            //the format string.
            string __name = "Rob";
            int __age = 21;
            Console.WriteLine("Your name is {0} and your age is {1,-5:D}", __name, __age);
            //String interpolation allows you to put the values to be converted directly into
            //the string text. An interpolated string is identified by a leading dollar($) sign at
            //the start of the string literal. The statement below shows how this works.The
            //WriteLine method now only has one parameter.
            Console.WriteLine($"Your name is {__name} and your age is {__age,-5:D}");
            //Note that when this program is compiled the compiler will convert the
            //interpolated string into a format string, extract the values from the string, and
            //build a call of WriteLine that looks exactly like the first call of WriteLine.
            //So you cannot dynamically use string interpolation when application is running.   

            //The act of assigning a string interpolation literal in a program produces a result
            //of type FormattedString. This provides a ToString method that accepts
            //a FormatProvider.This is useful because it allows you to use interpolated
            //strings with culture providers.
            decimal _bankBalance = 123.45M;
            FormattableString balanceMessage = $"US balance : {_bankBalance:C}";
            CultureInfo _usProvider = new CultureInfo("en-US");
            Console.WriteLine(balanceMessage.ToString(_usProvider));
            CultureInfo _ukProvider = new CultureInfo("en-GB");
            Console.WriteLine(balanceMessage.ToString(_ukProvider));

            string _name = "Rob";
            int _age = 21;
            Console.WriteLine("Your name is {0} and your age is {1,-5:D}", _name, _age);
            //String interpolation allows you to put the values to be converted directly into
            //the string text. An interpolated string is identified by a leading dollar($) sign at
            //the start of the string literal.
            Console.WriteLine($"Your name is {_name} and your age is {_age,-5:D}");
            //Note that when this program is compiled the compiler will convert the
            //interpolated string into a format string, extract the values from the string, and
            //build a call of WriteLine that looks exactly like the first call of WriteLine.
            Console.WriteLine(String.Format($"Your name is {_name} and your age is {_age,-5:D}"));

            //You can add behaviors to classes that you create
            //to allow them to be given formatting commands in the same way.Any type that
            //implements the IFormattable interface will contain a ToString method
            //that can be used to request formatted conversion of values into a string.
            MusicTrack song = new MusicTrack(artist: "Rob Miles", title: "My Way");
            Console.WriteLine("MusicTrack: {0}", song.ToString());
            Console.WriteLine("Track: {0:F}", song);
            Console.WriteLine("Artist: {0:A}", song);
            Console.WriteLine("Title: {0:T}", song);

            //The second parameter to the ToString method is an object that
            //implements the IFormatter interface. This parameter can be used by the
            //ToString method to determine any culture specific behaviors that may be
            //required in the string conversion process.For example, you might add date of
            //recording and price information to a music track, in which case the display of the
            //date and price information could be customized for different regions.
            //By default (i.e.unless you specify otherwise) the IFormatter reference
            //that is passed into the ToString call is the current culture.
            //Several types implement a ToString method that does make use of culture information.
            decimal bankBalance = 123.45M;
            CultureInfo usProvider = new CultureInfo("en-US");
            Console.WriteLine("US balance: {0}", bankBalance.ToString("C", usProvider));
            CultureInfo ukProvider = new CultureInfo("en-GB");
            Console.WriteLine("UK balance: {0}", bankBalance.ToString("C", ukProvider));
            //MusicTrack class does not implement cultural specific formating yet
            //song.ToString("F", usProvider);

            //Format strings
            //You can use format strings to request that output be formatted in a particular way when it is printed.
            //The WriteLine method is given a string that contains placeholders which start and end with braces.
            //Within the placeholder is first the number of the item in the WriteLine parameters to
            //be printed at that placeholder, the number of characters the item should occupy
            //(negative if the item is to be left justified), a colon(:) and then formatting
            //information for the item. The formatting information for an integer string
            //conversion can comprise the character ‘D’ meaning decimal string and ‘X’
            //meaning hexadecimal string. The formatting information for a floating-point
            //number can comprise the character ‘N,’ which is followed by the number of
            //decimal places to be printed.
            //This works because the int and double types can accept formatting
            //commands to specify the string they are to return.A program can give formatting
            //commands to many.NET types. The DateTime structure provides a wide
            //range of formatting commands.
            //A program can give formatting commands to many.NET types. The DateTime structure provides a wide
            //range of formatting commands.
            int i = 99;
            double pi = 3.141592654;
            //https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-pad-a-number-with-leading-zeros
            //https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings
            //https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/formatting-numeric-results-table
            //https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-numeric-format-strings
            Console.WriteLine(" {0,-10:D}{0, -10:X}{1,5:N2}", i, pi);
            //99        63         3.14 //Console.WriteLine(" {0,-10:D}{0, -10:X}{1,5:N2}", i, pi); 
            //99                  63         3.14 //Console.WriteLine(" {0,-20:D}{0, -10:X}{1,5:N2}", i, pi);
            //         9963         3.14 //Console.WriteLine(" {0,10:D}{0, -10:X}{1,5:N2}", i, pi);

            //Enumerate string methods
            foreach (char ch in "Hello world")
                Console.WriteLine(ch);
            
            //String comparison and cultures
            //Strings in C# are made up of Unicode characters encoded into 16 bit values the
            //using UTF-16 encoding format. The Unicode standard provides a range of
            //characters that can lead to culture specific behaviors when strings are being compared.
            //For example there are situations where the words “encyclopaedia” and “encyclopædia” should be regarded as equal.
            //You can use a StringComparision value to request a range of useful behaviors.
            //You can request that strings be compared according to the culture of the currently executing thread.
            // Default comparison fails because the strings are different
            if (!"encyclopædia".Equals("encyclopaedia"))
                Console.WriteLine("Unicode (encyclopædia = encyclopaedia) are not equal");

            ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(StringComparisonCulture);

            StringComparer stringComparer = new StringComparer(CultureInfo.CreateSpecificCulture("en-US"),
                StringComparison.CurrentCulture,
                "encyclopaedia",
                "encyclopædia");
            Thread thread = new Thread(parameterizedThreadStart);
            thread.IsBackground = true;
            thread.Start(stringComparer);
            thread.Join();

            stringComparer = new StringComparer(CultureInfo.CreateSpecificCulture("en-US"),
                StringComparison.CurrentCultureIgnoreCase,
                "ENCYCLOPAEDIA",
                "encyclopædia");
            thread = new Thread(parameterizedThreadStart);
            thread.IsBackground = true;
            thread.Start(stringComparer);
            thread.Join();

            stringComparer = new StringComparer(CultureInfo.CreateSpecificCulture("en-US"),
                StringComparison.OrdinalIgnoreCase,
                "ENCYCLOPAEDIA",
                "encyclopædia");
            thread = new Thread(parameterizedThreadStart);
            thread.IsBackground = true;
            thread.Start(stringComparer);
            thread.Join();
            
            //The string type provides a range of methods that can be used to find the position
            //of substrings inside a string.
            //Contains The method Contains can be used to test of a string contains another string. It
            //returns true if the source string contains the search string.
            string input = " Rob Miles";
            if (input.Contains("Rob"))
            {
                Console.WriteLine("Input contains Rob");
            }

            //The methods StartsWith and EndsWith can be used to test if a string starts
            //or ends with a particular string.Note that if the string starts or ends with one or
            //more whitespace characters these methods will not work.A whitespace character
            //is a space, tab, linefeed, carriage -return, formfeed, vertical - tab or newline
            //character.There are methods you can use to trim a string.TrimStart creates a
            //new string with whitespace removed from the start, TrimEnd removes
            //whitespace from the end of the string and Trim removes whitespace from both ends.
            string trimmedString = input.TrimStart();
            if (trimmedString.StartsWith("Rob"))
            {
                Console.WriteLine("Starts with Rob");
            }

            //IndexOf and SubString
            //The method IndexOf returns an integer which gives the position of the first
            //occurrence of a character or string in a string.There is also a LastIndexOf
            //method that will give the position of the last occurrence of a string.There are
            //overloads for these methods that let you specify the start position for the search.
            //You can use these position values with the SubString method to extract a
            //particular substring from a string.
            int nameStart = input.IndexOf("Rob");
            string name = input.Substring(nameStart, 3);
            Console.WriteLine(name);

            //Split
            //The Split method can be used to split a string into a number of substrings. The
            //split action returns an array of strings, it is given one or more separator strings
            //that will be used to split the string.
            //Note that if the source sentence held a number of consecutive spaces, each of
            //these would be resolved into a separate line in the output.
            string sentence = "The cat sat on the mat.";
            string[] words = sentence.Split(' ');
            foreach (string word in words)
            {
                Console.WriteLine(word);
            }

            string dataString = "Rob Miles 21\nAnother line\n34";
            StringReader dataStringReader = new StringReader(dataString);
            string firstLine = dataStringReader.ReadLine();
            string secondLine = dataStringReader.ReadLine();
            string thirdLine = dataStringReader.ReadLine();
            Console.WriteLine(firstLine);
            Console.WriteLine(secondLine);
            Console.WriteLine(thirdLine);
            int age;
            int.TryParse(thirdLine, out age);
            Console.WriteLine($"Age of rob: {age}");
            
            //StringBuilder can improve the speed of a program while at the same time making less work for the garbage collector.
            //The StringBuilder type provides methods that let a program treat a string
            //as a mutable object.A StringBuilder is implemented by a character array.It
            //provides methods that can be used to append strings to the StringBuilder,
            //remove strings from the StringBuilder and a property, Capacity, which
            //can be used to set the maximum number of characters that can be placed in a StringBuilder instance.
            string firstName = "Mohamed";
            string secondName = "Kalmoua";
            StringBuilder fullNameBuilder = new StringBuilder();
            fullNameBuilder.Append(firstName);
            fullNameBuilder.Append(" ");
            fullNameBuilder.Append(secondName);
            Console.WriteLine(fullNameBuilder.ToString());

            //If you want to assemble strings out of other string you can also use format
            //strings and string interpolation instead of string addition.
            DateTime dat = new DateTime(2012, 1, 17, 9, 30, 0);
            string city = "Chicago";
            int temp = -16;
            string output = String.Format("At {0} in {1}, the temperature was {2} degrees.", dat, city, temp);
            string fullName = string.Format("Welcome {0} {1}.", firstName, secondName);
            //string interpolation
            string fullName2 = $"{firstName} {secondName}";
            Console.WriteLine(output);
            Console.WriteLine(fullName);
            Console.WriteLine(fullName2);

            //StringWriter
            //The StringWriter class is based on the StringBuilder class. It
            //implements the TextWriter interface, so programs can send their output into a string.
            StringWriter writer = new StringWriter();
            writer.WriteLine("Hello World");
            writer.Close();
            Console.Write(writer.ToString());

            StringBuilder strBuilder = new StringBuilder("file path characters are: ");
            StringWriter strWriter = new StringWriter(strBuilder);
            strWriter.Write(Path.GetInvalidPathChars(), 0, Path.GetInvalidPathChars().Length);
            strWriter.Close();
            // Since the StringWriter is closed, an exception will 
            // be thrown if the Write method is called. However, 
            // the StringBuilder can still manipulate the string.
            strBuilder.Insert(0, "Invalid ");
            Console.WriteLine(strWriter.ToString());
        }
    }
}
