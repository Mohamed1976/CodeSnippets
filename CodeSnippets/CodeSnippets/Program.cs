using AbstractClassesNamespace;
using BankNamespace;
using CodeSnippets.Enums;
using CodeSnippets.Implementing_Value_Equality;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using VehicleRegistration;

namespace CodeSnippets
{
    class Program
    {
        static void Main(string[] args)
        {

            #region [ Enums ]

            Console.WriteLine($"\nEnums---------------------------------------------------------------------------------");

            //Enum:         Is an abstract class that includes static helper methods to work with enums.
            //GetNames:     Returns an array of string name of all the constant of specified enum.
            //GetValues:    Returns an array of the values of all the constants of specified enum.
            //GetName:      Returns the name of the constant of the specified value of specified enum.
            //Format:       Converts the specified value of enum type to the specified string format.
            //object Parse(type, string)    Converts the string representation of the name or numeric 
            //                              value of one or more enumerated constants to an equivalent enumerated object. 
            //bool TryParse(string, out TEnum)  Converts the string representation of the name or numeric value 
            //                                  of one or more enumerated constants to an equivalent enumerated object. 
            //                                  The return value indicates whether the conversion succeeded. 

            //Calling GetNames 
            Console.WriteLine($"{nameof(StringComparisonOperators)}: " + Helper.GetEnumNames(typeof(StringComparisonOperators)));
            Console.WriteLine($"{nameof(NumericComparisonOperators)}: " + Helper.GetEnumNames(typeof(NumericComparisonOperators)));
            Console.WriteLine($"{nameof(ListComparisonOperators)}: " + Helper.GetEnumNames(typeof(ListComparisonOperators)));
            Console.WriteLine($"{nameof(OrderStatus)}: " + Helper.GetEnumNames(typeof(OrderStatus)));            
            Console.WriteLine($"{nameof(OrderStatus)}: " + Helper.GetEnumNames<OrderStatus>()); //Using generics to call GetNames
            //Calling GetValues retrieves enum values and corresponding name (using GetName())  
            Dictionary<string, int> enumAndValues = Helper.EnumNamedValues<Moods>();
            foreach(KeyValuePair<string, int> enumAndValue in enumAndValues)
            {
                Console.WriteLine($"{enumAndValue.Key} => {enumAndValue.Value}");
            }

            //Enum DaysOfWeek
            enumAndValues = Helper.EnumNamedValues<WeekDays>();
            foreach (KeyValuePair<string, int> enumAndValue in enumAndValues)
            {
                Console.WriteLine($"{enumAndValue.Key} => {enumAndValue.Value}");
            }

            //Retrieve the description of enum
            Console.WriteLine($"Description of OrderStatus.InProcess: {Helper.GetDescription(OrderStatus.InProcess)} ");
            //The function returns the enum name when no description is found
            Console.WriteLine($"Description of DaysOfWeek.Weekdays: {Helper.GetDescription(WeekDays.Weekdays)} ");
            Console.WriteLine($"Description of DaysOfWeek.Weekdays: {Helper.GetDescription2(OrderStatus.InProcess)} ");
            Console.WriteLine($"Description of DaysOfWeek.Weekdays: {Helper.GetDescription3(OrderStatus.InProcess)} ");
            OrderStatus orderStatus = Helper.GetEnumValueFromDescription<OrderStatus>("In process");
            Console.WriteLine($"Get enum for description In process: {orderStatus.ToString()} ");
            Console.WriteLine($"Get description InProcess: {EnumExtensions.GetDescription<OrderStatus>(OrderStatus.InProcess)} ");
            
            // Create and initialize instance of enum type
            Volume myVolume = Volume.Low;

            // Make decision based on enum value
            switch (myVolume)
            {
                case Volume.Low:
                    Console.WriteLine("The volume has been turned Down.");
                    break;
                case Volume.Medium:
                    Console.WriteLine("The volume is in the middle.");
                    break;
                case Volume.High:
                    Console.WriteLine("The volume has been turned up.");
                    break;
            }

            Console.WriteLine("WeekDays.Friday: {0}", WeekDays.Friday);
            //An explicit cast is necessary to convert from enum type to an integral type. 
            //For example, to get the int value from an enum:
            Console.WriteLine("(int)WeekDays.Friday: {0}", (int)WeekDays.Friday);

            WeekDays wdEnum;
            Enum.TryParse<WeekDays>("1", out wdEnum);
            Console.WriteLine("Enum.TryParse<WeekDays>(\"1\", out myWeekDays): {0}", wdEnum.ToString());
            Enum.TryParse<WeekDays>("Friday", out wdEnum); //case-sensitive match)
            Console.WriteLine("Enum.TryParse<WeekDays>(\"Friday\", out myWeekDays): {0}, iVal: {1}", wdEnum.ToString(), (int)wdEnum);
            bool matchFound = Enum.TryParse("WEDNESDAY", true, out wdEnum); // case-insensitive match
            Console.WriteLine("Enum.TryParse(\"WEDNESDAY\", true, out wdEnum): {0}, iVal: {1}", wdEnum.ToString(), (int)wdEnum);
            DayOfWeek friday = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), "Friday");//Throws exception when error occurs 
            Dictionary<int, string> enumNamedValues = Helper.GetEnumNamedValues<WeekDays>();
            foreach(KeyValuePair<int, string> keyValuePair in enumNamedValues)
            {
                Console.WriteLine("{0}, {1}", keyValuePair.Value, keyValuePair.Key);
            }

            //You could get the underlying type of the enum as follows:
            Console.WriteLine("Enum.GetUnderlyingType(typeof(Volume)): {0}", Enum.GetUnderlyingType(typeof(Volume)));

            var threeFlags = WeekDays.Monday | WeekDays.Tuesday | WeekDays.Friday;
            // This will enumerate all the flags in the variable: "Monday, Tuesday".
            Console.WriteLine("WeekDays.Monday | WeekDays.Tuesday | WeekDays.Friday: {0}", threeFlags);
            //With HasFlag we can check if any of the flags is set
            if (threeFlags.HasFlag(WeekDays.Monday))
                Console.WriteLine("WeekDays.Monday is set.");
            if (threeFlags.HasFlag(WeekDays.Tuesday))
                Console.WriteLine("WeekDays.Tuesday is set.");
            if (threeFlags.HasFlag(WeekDays.Friday))
                Console.WriteLine("WeekDays.Friday is set.");

            //None=0, cannot be used when using flags
            foreach (WeekDays flagToCheck in Enum.GetValues(typeof(WeekDays)))
            {
                if (threeFlags.HasFlag(flagToCheck))
                {
                    Console.WriteLine("Foreach check, flag set: " + flagToCheck);
                }
            }

            var twoFlags = WeekDays.Monday | WeekDays.Tuesday;
            //Check if WeekDays.Monday and WeekDays.Tuesday are both set. 
            if ((threeFlags & twoFlags) == twoFlags)
            {
                Console.WriteLine("(threeFlags & twoFlags) == twoFlags");
            }

            //Add and remove values from flagged enum
            twoFlags |= WeekDays.Saturday;
            //Remove flag
            twoFlags &= ~WeekDays.Saturday;

            //Using << notation for flags makes enums more readable
            //The left-shift operator (<<) can be used in flag enum declarations to ensure that each flag has exactly one 
            //1 in binary representation, as flags should.
            /*[Flags]
            public enum MyEnum
            {
                None = 0,
                Flag1 = 1 << 0,
                Flag2 = 1 << 1,
                Flag3 = 1 << 2,
                Flag4 = 1 << 3,
                Flag5 = 1 << 4,
                ...
                Flag31 = 1 << 30
            }*/
            int resultLeftShiftOperator = 1 << 0;
            Console.WriteLine("1 << 0: " + resultLeftShiftOperator);
            resultLeftShiftOperator = 1 << 1;
            Console.WriteLine("1 << 1: " + resultLeftShiftOperator);
            resultLeftShiftOperator = 1 << 2;
            Console.WriteLine("1 << 2: " + resultLeftShiftOperator);
            resultLeftShiftOperator = 1 << 3;
            Console.WriteLine("1 << 3: " + resultLeftShiftOperator);
            resultLeftShiftOperator = 1 << 4;
            Console.WriteLine("1 << 4: " + resultLeftShiftOperator);

            //Since an enum can be cast to and from its underlying integral type, the value may fall outside the range of values
            //given in the definition of the enum type. Although the below enum type DaysOfWeek only has 7 defined values, it can still hold any int value.
            //However, undefined enum values can be detected by using the method Enum.IsDefined. For example,
            OrderStatus orderStatus1 = (OrderStatus)30; //Although 30 is not defined we still can assign it this value, because of its underlying type is int 
            Console.WriteLine("(OrderStatus)30: " + orderStatus1 + ", Enum.IsDefined(typeof(OrderStatus),(OrderStatus)30): " + Enum.IsDefined(typeof(OrderStatus), orderStatus1));
            //The default value for an enum is zero.If an enum does not define an item with a value of zero, its default value will be zero.
            Console.WriteLine("default(OrderStatus): " + default(OrderStatus).ToString());
            Console.WriteLine("default(WeekDays): " + default(WeekDays).ToString());
            Console.WriteLine("OrderStatus.InProcess.GetDescription(): " + OrderStatus.InProcess.GetDescription());

            #endregion

            #region [ Implementing Value Equality ]

            Console.WriteLine($"\nImplementing Value Equality-----------------------------------------------------------");
            object objectX = new object();
            //objectY references objectX 
            object objectY = objectX;
            ReferenceEquals(objectX, objectY);
            Console.WriteLine("ReferenceEquals(objectX, objectY): {0} == true (Objects reference the same memory)", ReferenceEquals(objectX, objectY));

            //String is a reference type, the equal operator compares the values not the memory addresses 
            // Define some strings:
            string stringX = "hello";
            string stringY = "hello";
            // Compare string values of a constant and an instance: True
            Console.WriteLine("stringX == stringY: {0} == true (Values are compared)", stringX == stringY);
            Console.WriteLine("ReferenceEquals(stringX, stringY): {0} == false (Different references)", ReferenceEquals(stringX, stringY));

            //https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.sequenceequal?view=netframework-4.8
            Pet pet1 = new Pet { Name = "Turbo", Age = 2 };
            Pet pet2 = new Pet { Name = "Peanut", Age = 8 };

            // Create two lists of pets.
            List<Pet> pets1 = new List<Pet> { pet1, pet2 };
            List<Pet> pets2 = new List<Pet> { pet1, pet2 };

            //true if the two source sequences are of equal length and their corresponding elements are equal according to the 
            //default equality comparer for their type; otherwise, false.
            bool equal = pets1.SequenceEqual(pets2);
            //Lists reference the same memory objects, therefore are equal  
            Console.WriteLine("The lists {0} equal.", equal ? "are" : "are not");
            List<Pet> pets3 = new List<Pet> { new Pet { Name = "Turbo", Age = 2 }, new Pet { Name = "Peanut", Age = 8 } };
            //Values are equal, but the two lists reference different memories "are not equal"
            equal = pets1.SequenceEqual(pets3);
            Console.WriteLine("The lists {0} equal.", equal ? "are" : "are not");

            //Product class has implemented custom comparator, value comparison instead of reference comparison  
            Product[] storeA = { new Product { Name = "apple", Code = 9 }, new Product { Name = "orange", Code = 4 } };
            Product[] storeB = { new Product { Name = "apple", Code = 9 }, new Product { Name = "orange", Code = 4 } };
            bool equalAB = storeA.SequenceEqual(storeB);
            Console.WriteLine("storeA.SequenceEqual(storeB) Equal? " + equalAB);

            //https://www.codeproject.com/Articles/5251448/Implementing-Value-Equality-in-Csharp
            //Reference equality and value equality are two different ways to determine the equality of an object.
            //With reference equality, two objects are compared by memory address.If both objects point to the same
            //memory address, they are equivalent. Otherwise, they are not.Using reference equality, the data the object 
            //holds is not considered. The only time two objects are equal is if they actually refer to the same instance.

            //Often, we would prefer to use value equality.With value equality, two objects are considered equal if all of 
            //their fields have the same data, whether or not they point to the same memory location. That means multiple 
            //instances can be equal to each other, unlike with reference equality.

            //A class that implements IEqualityComparer<T> compare class instances using using value semantics like Dictionary<TKey, TValue>.
            //Normal instance comparisons will use reference equality.
            Implementing_Value_Equality.Employee employeeX = new Implementing_Value_Equality.Employee(id: 1,
                firstName: "John",
                lastName: "Smith",
                birthday: new DateTime(1981, 11, 19));
            
            Implementing_Value_Equality.Employee employeeY = new Implementing_Value_Equality.Employee(id: 1,
                firstName: "John",
                lastName: "Smith",
                birthday: new DateTime(1981, 11, 19));

            List<Implementing_Value_Equality.Employee> employees = new List<Implementing_Value_Equality.Employee>();
            employees.Add(employeeX);
            employees.Add(employeeY);
            //The default comparison is reference, therefore the two object above are seen as different;    
            int count = employees.Distinct<Implementing_Value_Equality.Employee>().ToList().Count;
            Console.WriteLine($"employees.Distinct using default reference comparison, 2 == {count}.");
            //Comparison using value equality, therefore the two object above are seen having the same values;    
            count = employees.Distinct<Implementing_Value_Equality.Employee>(EmployeeEqualityComparer.Default).ToList().Count;
            Console.WriteLine($"employees.Distinct using value equality comparison, 1 == {count}.");
            // These will return false, since the 2 instances have different memory addresses
            // this is reference equality:
            Console.WriteLine("employeeX.Equals(employeeY): {0}", employeeX.Equals(employeeY));
            Console.WriteLine("employeeX==employeeY: {0}", employeeX == employeeY);
            Console.WriteLine("ReferenceEquals(employeeX, employeeY): {0}", ReferenceEquals(employeeX, employeeY));
            //EmployeeEqualityComparer employeeEqualityComparer = new EmployeeEqualityComparer();
            //Alternatively to object creation as above, we can use the singleton instance Default  
            // this will return true since this class is designed to compare the data in the fields:
            Console.WriteLine("employeeEqualityComparer.Equals(employeeX, employeeY): {0}", EmployeeEqualityComparer.Default.Equals(employeeX, employeeY));            
            // Create dictionary and add two employee instances/objects:
            var d1 = new Dictionary<Implementing_Value_Equality.Employee, int>();            
            d1.Add(employeeX, 0);
            //Key matching uses reference matching therefore d1.ContainsKey(employeeX) will return true, same instance   
            Console.WriteLine("Dictionary.ContainsKey(employeeX): {0} == true (Same memory address)", d1.ContainsKey(employeeX));
            //Two object with same properties but different memory addresses will return false. 
            Console.WriteLine("Dictionary.ContainsKey(employeeY): {0} == false (Different memory addresses)", d1.ContainsKey(employeeY));
            //Create dictionary that uses the custom comparer instead of reference comparison. 
            Dictionary<Implementing_Value_Equality.Employee, int>  d2 = 
                new Dictionary<Implementing_Value_Equality.Employee, int>(EmployeeEqualityComparer.Default);
            d2.Add(employeeX, 0);
            //Key matching uses custom comparer matching therefore d1.ContainsKey(employeeX) will return true, same value   
            Console.WriteLine("Dictionary.ContainsKey(employeeX): {0} == true (Same memory address)", d2.ContainsKey(employeeX));
            //Two object with same properties but different memory addresses will return true in case of custom comparer. 
            Console.WriteLine("Dictionary.ContainsKey(employeeY): {0} == true (Different memory address, same value properties)", d2.ContainsKey(employeeY));

            //Create two Customer object with the same property values
            Implementing_Value_Equality.Customer customerX = new Implementing_Value_Equality.Customer(1, "John Smith",
                "Software Design Engineer in Test",
                new DateTime(1981, 11, 19));

            Implementing_Value_Equality.Customer customerY = new Implementing_Value_Equality.Customer(1, "John Smith",
                "Software Design Engineer in Test",
                new DateTime(1981, 11, 19));

            // these will return true because they are overloaded, in Customer to compare the fields
            Console.WriteLine("customerX.Equals(customerY): {0} == true (property values are compared)", customerX.Equals(customerY));
            Console.WriteLine("customerX == customerY: {0} == true (property values are compared)", customerX == customerY);
            Console.WriteLine("customerX != customerY: {0} == false (property values are compared)", customerX != customerY);

            // Create a dictionary:
            var customersDict = new Dictionary<Implementing_Value_Equality.Customer, int>();
            customersDict.Add(customerX, 0);
            // These will return true, since Customer implements Equals():
            Console.WriteLine("Dictionary.ContainsKey(customerX): {0} == true", customersDict.ContainsKey(customerX));
            Console.WriteLine("Dictionary.ContainsKey(customerY): {0} == true", customersDict.ContainsKey(customerY));
            //Create list and add two customers to it.
            List<Implementing_Value_Equality.Customer> customers = new List<Implementing_Value_Equality.Customer>();
            customers.Add(customerX);
            customers.Add(customerY);
            Console.WriteLine("customers.Count: {0} == 2 (Two objects were added)", customers.Count);
            Console.WriteLine("customers.Distinct().ToList().Count: {0} == 1 (One distinct object)", customers.Distinct().ToList().Count);

            //Structs do a kind of value equality semantics by default.They compare each field.This works until the fields 
            //themselves use reference semantics, so you may find yourself implementing value semantics on a struct anyway 
            //if you need to compare those fields themselves by value.

            #endregion

            #region [ What is new in C# 8.0 ]

            //https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8
            //readonly method
            Rectangle rectangle = new Rectangle(10, 10);
            Console.WriteLine("Rectangle area: [{0}]", rectangle.Area());
            //Indices and ranges
            string[] digits = new string[]
            {                   //Index from start      //Index from end
                "The",          //0                     //^10    
                "quick",        //1                     //^9
                "brown",        //2                     //^8
                "fox",          //3                     //^7
                "jumped",       //4                     //^6
                "over",         //5                     //^5
                "the",          //6                     //^4
                "lazy",         //7                     //^3
                "dog",          //8                     //^2
                "Ten"           //9                     //^1
            };
            var allWords = digits[..]; //Contains all the content
            var firstPhrases = digits[..4]; //Contains digits[0]..digits[3]
            var lastPhrases = digits[6..]; //Contains digits[6]..digits[9]
            Index index = ^4;
            Console.WriteLine($"{index}: {digits[index]}");
            //The last element is not included in the range
            Range range = 1..4;
            var list = digits[range];
            var _numbers = Enumerable.Range(0, 100).ToArray();
            int x = 12;
            int y = 25;
            int z = 36;
            Console.WriteLine($"{_numbers[^x]} is the same as {_numbers[_numbers.Length - x]}");
            Console.WriteLine($"{_numbers[x..y].Length} is the same as {y - x}");
            Span<int> spanXY = _numbers[x..y];
            Span<int> spanYZ = _numbers[y..z];
            Console.WriteLine($"spanXY and spanYZ are consecutive.");
            foreach (int i in spanXY)
            {
                Console.WriteLine($"spanXY: {i}"); 
            }

            foreach (int i in spanYZ)
            {
                Console.WriteLine($"spanYZ: {i}");
            }
            
            Console.WriteLine($"_numbers[x..^x] removes x elements from both ends.");
            Span<int> spanXX = _numbers[x..^x];
            Console.WriteLine($"spanXX[0] {spanXX[0]}, spanXX[^1] {spanXX[^1]}.");

            Console.WriteLine($"_numbers[..x] means _numbers[0..x] and means _numbers[x..] means _numbers[x..^0]");
            Span<int> spanX = _numbers[..x];
            Span<int> span0X = _numbers[0..x];
            Console.WriteLine($"\tspanX vs span0X: {spanX[0]}..{spanX[^1]} is the same as {span0X[0]}..{span0X[^1]}");
            Span<int> spanZ = _numbers[z..];
            Span<int> spanZEnd = _numbers[z..^0];
            Console.WriteLine($"\tspanZ vs spanZEnd: {spanZ[0]}..{spanZ[^1]} is the same as {spanZEnd[0]}..{spanZEnd[^1]}");

            //New range indices allow you to easily iterate through arrays.     
            IndicesExample();
            //You can use a static function within a static function.
            StaticLocalFunc();

            //Display last digit "Ten"
            Console.WriteLine($"digits[^1]: {digits[^1]}");
            //Display "Nine"
            Console.WriteLine($"digits[^2]: {digits[^2]}");
            //Displays the complete list similar to foreach(string digit in digits) 
            foreach (string digit in digits[0..^0])
            {
                Console.WriteLine($"String digit in digits[0..^0]: {digit}");
            }
            //Displays "Two" .. "Nine"
            foreach (string digit in digits[1..^1])
            {
                Console.WriteLine($"String digit in digits[1..^1]: {digit}");
            }
            //Displays Index=2, 3, 4(not included) "Three", "Four"
            foreach (string digit in digits[2..4])
            {
                Console.WriteLine($"String digit in digits[2..4]: {digit}");
            }
            //Using declarations, no brackets ({}) needed to define scope of using statement
            //The dispose method is called when using statement gets out of scope
            /*
            using var inputFile = new StreamReader(@"c:\inputFile.txt");
            using var outputFile = new StreamWriter(@"c:\outputFile.txt");
            string line; 
            while((line = inputFile.ReadLine()) != null)
            {
                outputFile.WriteLine(line);
            }*/

            //using switch expressions instead of switch statement
            Console.WriteLine($"7 + 3 = {Calculator(7, 3, MathOperation.Add)}");
            //The IShoppingCart has an optional method that is not mandatory to implement.
            IShoppingCart shoppingCart = new ShoppingCart();
            shoppingCart.CalculateTotal();
            //null coalescing assignment operator
            //https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-coalescing-operator
            List<int> numbers = null;
            //If the list is not allocated, allocate new list 
            numbers ??= new List<int>();
            int? intNumber = null; 
            numbers.AddRange(new int[] { intNumber ??= 5, intNumber ??= 9, 11, 17 });
            Console.WriteLine($"After new list is allocated, items [{string.Join(" ", numbers)}]");
            int indexOfSetToSum = 1;
            double sumResult = 0;
            List<double[]> numberArrays = null;
            sumResult = numberArrays?[indexOfSetToSum]?.Sum() ?? double.NaN;
            Console.WriteLine($"Should be NaN: {sumResult}");
            numberArrays = new List<double[]>();
            numberArrays.Add(new double[] { 6, 8, 9, 11, 13 });
            numberArrays.Add(new double[] { 16, 28, 39, 311, 613 });
            numberArrays.Add(new double[] { 45, 85, 32, 324, 97 });
            sumResult = numberArrays?[indexOfSetToSum]?.Sum() ?? double.NaN;
            Console.WriteLine($"Should be {16 + 28 + 39 + 311 + 613}: {sumResult}");

            //In past do asynchronous call and wait for all the data to be available.
            //Using IAsyncEnumerable, As the data is available you yield it and return it. 
            //Gets Data and returns it immediately, then waits for data await async Task.. etc
            //Async iterators are useful when you have to get some data, process the data and get some more. 
            /* Uncomment to see behaviour
            AsyncIterator();
            */
            try
            {
                decimal toll = CalculateToll(new Car { Passengers = 3 }, DateTime.Now.AddDays(3), inbound:true);
                Console.WriteLine($"CalculateToll {toll}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //Interpolated verbatim strings, $@"" == @$"", the @ and $ order doesn't matter  

            //Using interfaces
            LoggerFactory loggerFactory = new LoggerFactory();
            var logger = loggerFactory.GetLogger(LoggerType.Database);
            logger.Log("Log message");

            #endregion

            #region [ Yield keyword ]

            //When using the Yield method, after each yield, control is returned to the calling method
            List<string> stringList = new List<string>(new string[] { "Alfa", "Beta", "Gamma", "Een", "Delta", "Ywe" });
            foreach (string str in Filter(stringList))
            {
                Console.WriteLine($"Yield keyword calling method: {str}");
            }

            List<string> results = Filter(stringList).ToList();
            foreach (string str in results)
            {
                Console.WriteLine($"Yield keyword after results received: {str}");
            }

            //The Yield method variables retain their value between function calls
            foreach (int i in RunningTotal())
            {
                Console.WriteLine($"Yield keyword RunningTotal(): {i}");
            }

            #endregion

            #region [ Tuples ]

            //https://docs.microsoft.com/en-us/dotnet/csharp/tuples
            TupleExample();

            #endregion

            #region [ ConfigurationManager ]

            //A copy of App.config is maintained to store changed values <SolutionName>.dll.config 
            //There are different ways to maintain the Configuration file.
            //1) Read Configuration using ConnectionStrings from App.Config
            //2) Read Configuration using AppSettings from App.Config
            //3) Read Configuration from External Config file
            //https://docs.microsoft.com/en-us/dotnet/api/system.configuration.configurationmanager.appsettings?view=netframework-4.8
            //You need the nuget System.Configuration.ConfigurationManager
            //Add Application Configuration File (App.config)
            Console.WriteLine($"\nConfigurationManager------------------------------------------------------------------");
            ReadAllSettings();
            ReadSetting("Username");
            ReadSetting("NoneExisting");
            AddUpdateAppSettings("DefaultColor", "Red");
            //You can encrypt sensitive information in the connection Strings and Configuration Files
            //https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/connection-strings-and-configuration-files
            GetConnectionStrings();
            ReadConnectionString("NoneExistingDB");
            ReadConnectionString("DBConnection");
            bool IsActive = true;
            AddUpdateAppSettings(nameof(IsActive), IsActive.ToString());
            bool result = GetConfigKeyValue(nameof(IsActive), false);
            Console.WriteLine($"{nameof(IsActive)}: {result} == {IsActive}");
            //You can also read the setting as string and convert it using Boolean.Parse
            //Illustrated below
            //string boolStr = ReadSetting(nameof(IsActive));
            //bool result = Boolean.Parse(boolStr);
            //Int32.Parse() .. etc

            #endregion

            #region[ Singleton ]

            //Singleton is registered in autofac as SingleInstance
            //builder.RegisterType<ApplicationStatus>().As<IApplicationStatus>().SingleInstance();
            Console.WriteLine($"\nSingleton-----------------------------------------------------------------------------");
            IApplicationStatus applicationStatus = new ApplicationStatus();
            applicationStatus.EnterBusy();
            applicationStatus.SetMessage(SeverityType.Error, "Error message set.");
            Console.WriteLine($"IApplicationStatus.Busy: {applicationStatus.IsBusy}, Message: {applicationStatus.Message}");
            //Statically Initialized Singleton 
            Singleton singleton = Singleton.Instance;
            Console.WriteLine($"singleton.GetState(): {singleton.GetState()}");
            Singleton singleton2 = Singleton.Instance;
            Console.WriteLine($"singleton2.GetDetails(): {singleton2.GetDetails()}");
            Singleton singleton3 = Singleton.Instance;
            Console.WriteLine($"singleton3.GetDetails(): {singleton3.GetDetails()}");
            Singleton singleton4 = Singleton.Instance;
            Console.WriteLine($"singleton4.AreaOfCircle(2): {singleton4.AreaOfCircle(2)}");

            #endregion

            #region [ Abstract classes, Partial classes, inheritance and virtual methods ]

            //Difference and similarity between Virtual and Abstract keywords.
            //The main and most important difference between Virtual and Abstract Keywords is that Virtual method / property 
            //may or may not be overriden in the derived class. Whereas, in case of abstract keyword, you have to override 
            //the method or property, or else the compiler will throw error.
            //Virtual and Abstract are the only methods/properties that can be overriden in the derived class.
            //Access modifiers can only be: public, protected, internal or protected internal. Private modifier is not allowed 
            //because you can not access private methods/properties from derived classes.    
            //Virtual methods have an implementation and provide the derived classes with the option of overriding it. 
            //Abstract methods do not provide an implementation and force the derived classes to override the method.
            //If an abstract method is defined in a class, then the class must be declared as an abstract class.
            //Virtual Method can reside in an abstract and non-abstract class. It provides the derived classes 
            //with the option of overriding it. By default, methods defined in the base class are not overridable.
            //You can apply the sealed keyword to indicate that a class cannot serve as a base class for any additional classes.
            Console.WriteLine($"\nAbstract and Virtual methods----------------------------------------------------------");
            UKEUBankCustomer uKEUBankCustomer = new UKEUBankCustomer("MyName");
            Console.WriteLine($"UKEUBankCustomer.GetName() {uKEUBankCustomer.GetName()}");
            Console.WriteLine($"UKEUBankCustomer.GetCustomerType() {uKEUBankCustomer.GetCustomerType()}");
            Console.WriteLine($"UKEUBankCustomer.GetCustomerTypeName() {uKEUBankCustomer.GetCustomerTypeName()}");
            //The virtual method is called because there is not override  
            Console.WriteLine($"UKEUBankCustomer.GetCountryRegion() {uKEUBankCustomer.GetCountryRegion()}");

            NonUKEUBankCustomer nonUKEUBankCustomer = new NonUKEUBankCustomer("Another name");
            Console.WriteLine($"nonUKEUBankCustomer.GetName() {nonUKEUBankCustomer.GetName()}");
            Console.WriteLine($"nonUKEUBankCustomer.GetCustomerType() {nonUKEUBankCustomer.GetCustomerType()}");
            Console.WriteLine($"nonUKEUBankCustomer.GetCustomerTypeName() {nonUKEUBankCustomer.GetCustomerTypeName()}");
            Console.WriteLine($"nonUKEUBankCustomer.GetCountryRegion() {nonUKEUBankCustomer.GetCountryRegion()}");

            //Interfaces imply a "can do" relationship
            //In C#, A class can implement one or more interfaces. But a class can inherit only one abstract class.
            //In C#, An interface cannot have the constructor declaration. An abstract class can have the constructor declaration.
            //An abstract class can be fully, partially or not implemented (methods, fields, constructors). Interfaces should be fully implemented.
            FulltimeEmployee fulltimeEmployee = new FulltimeEmployee("FG45","MyFirstName", "MyLastName");
            Console.WriteLine($"FulltimeEmployee.Add(), {fulltimeEmployee.Add()}");
            Console.WriteLine($"FulltimeEmployee.Delete(), {fulltimeEmployee.Delete()}");
            fulltimeEmployee.FirstName = "MyFirstName2";
            fulltimeEmployee.LastName = "MyLastName2";
            fulltimeEmployee.ID = "AS65";
            Console.WriteLine($"FulltimeEmployee.Search() after modification, {fulltimeEmployee.Search()}");
            Console.WriteLine($"FulltimeEmployee.CalculateWage(), {fulltimeEmployee.CalculateWage()}");
            //abstract Shape class and  derived classes
            Shape[] shapes = { new AbstractClassesNamespace.Rectangle(10, 12), new Square(5), new Circle(3) };
            foreach (var shape in shapes)
            {
                Console.WriteLine($"{shape}: area, {Shape.GetArea(shape)}; " + $"perimeter, {Shape.GetPerimeter(shape)}");
                Console.WriteLine($"{shape}: area, {shape.Area}; " + $"perimeter, {shape.Perimeter}");

                var rect = shape as AbstractClassesNamespace.Rectangle;
                if (rect != null)
                {
                    Console.WriteLine($"   Is Square: {rect.IsSquare()}, Diagonal: {rect.Diagonal}");
                }

                var sq = shape as Square;
                if (sq != null)
                {
                    Console.WriteLine($"   Diagonal: {sq.Diagonal}");
                }

                var circle = shape as Circle;
                if (circle != null)
                {
                    Console.WriteLine($"   Circumference: {circle.Circumference}, Radius: {circle.Radius}");
                }
            }

            //https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/inheritance
            //https://docs.microsoft.com/en-us/dotnet/api/system.object?view=netframework-4.8
            //Inheritance applies only to classes and interfaces.Other type categories(structs, delegates, and enums) do not support inheritance.
            //Inheritance is a feature of object-oriented programming languages that allows you to define a base class 
            //that provides specific functionality(data and behavior) and to define derived classes that either inherit 
            //or override that functionality. Inheritance is one of the fundamental attributes of object-oriented programming.
            //It allows you to define a child class that reuses(inherits), extends, or modifies the behavior of a parent class. 
            //The class whose members are inherited is called the base class. The class that inherits the members of the base 
            //class is called the derived class.C# and .NET support single inheritance only. That is, a class can only inherit 
            //from a single class. However, inheritance is transitive, which allows you to define an inheritance hierarchy for 
            //a set of types. In other words, type D can inherit from type C, which inherits from type B, which inherits from 
            //the base class type A. Because inheritance is transitive, the members of type A are available to type D.
            //GetValue() get private value from base class, possible because of nested class
            var b = new A.B();
            Console.WriteLine(b.GetValue());

            //Protected members are visible only in derived classes.
            //Internal members are visible only in derived classes that are located in the same assembly as the base class. 
            //They are not visible in derived classes located in a different assembly from the base class.
            //Public members are visible in derived classes and are part of the derived class' public interface. 
            //Public inherited members can be called just as if they are defined in the derived class.
            //Derived classes can also override inherited members by providing an alternate implementation.
            //In order to be able to override a member, the member in the base class must be marked with the virtual keyword.
            //You can override inherited member only when marked as virtual, abstract, or override. 
            //All types in the .NET type system implicitly inherit from Object or a type derived from it. 
            //For example SimpleClass does not have any members, but when using reflection, we find nine members
            //One of these members is a parameterless (or default) constructor that is automatically supplied for 
            //the SimpleClass type by the C# compiler. The remaining eight are members of Object, the type from 
            //which all classes and interfaces in the .NET type system ultimately implicitly inherit.  
            Type t = typeof(SimpleClass);
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                                 BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
            MemberInfo[] members = t.GetMembers(flags);
            Console.WriteLine($"Type {t.Name} has {members.Length} members: ");
            foreach (var member in members)
            {
                string access = "";
                string stat = "";
                var method = member as MethodBase;
                if (method != null)
                {
                    if (method.IsPublic)
                        access = " Public";
                    else if (method.IsPrivate)
                        access = " Private";
                    else if (method.IsFamily)
                        access = " Protected";
                    else if (method.IsAssembly)
                        access = " Internal";
                    else if (method.IsFamilyOrAssembly)
                        access = " Protected Internal ";
                    if (method.IsStatic)
                        stat = " Static";
                }
                var output = $"{member.Name} ({member.MemberType}): {access}{stat}, Declared by {member.DeclaringType}";
                Console.WriteLine(output);
            }

            //Abstract Publication class and derived book class 
            var book = new Book("The Tempest", "0971655819", "Shakespeare, William", "Public Domain Press");
            Console.WriteLine($"{book.Title}, " +
                $"{(book.GetPublicationDate() == "NYP" ? "Not Yet Published" : "published on " + book.GetPublicationDate()):d} by {book.Publisher}");
            book.Publish(new DateTime(2016, 8, 18));
            Console.WriteLine($"{book.Title}, " +
                $"{(book.GetPublicationDate() == "NYP" ? "Not Yet Published" : "published on " + book.GetPublicationDate()):d} by {book.Publisher}");
            var book2 = new Book("The Tempest", "Classic Works Press", "Shakespeare, William");
            Console.WriteLine($"{book.Title} and {book2.Title} are the same publication: " +
                  $"{book.Equals(book2)}");

            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/partial-classes-and-methods
            //It is possible to split the definition of a class, a struct, an interface or a method over two or 
            //more source files.Each source file contains a section of the type or method definition, and all parts 
            //are combined when the application is compiled. 
            //There are several situations when splitting a class definition is desirable:
            //1) When working on large projects, spreading a class over separate files enables multiple 
            //programmers to work on it at the same time.
            //2) When working with automatically generated source, code can be added to the class without having to 
            //recreate the source file. Visual Studio uses this approach when it creates Windows Forms, 
            //Web service wrapper code, and so on. You can create code that uses these classes without 
            //having to modify the file created by Visual Studio.
            Customer customer = new Customer("MyFirstName", "MyLastName");
            customer.DoWork();
            customer.GoToLunch();
            //Partial struct 
            PartialStruct partialStruct;
            partialStruct.Struct_Test();
            partialStruct.Struct_Test2();

            //In C#, a method in a derived class can have the same name as a method in the base class. 
            //You can specify how the methods interact by using the new and override keywords. The override 
            //modifier extends the base class virtual method, and the new modifier hides an accessible base 
            //class method. The difference is illustrated in the examples in this topic.
            //New Keyword to hide implementation of base class
            BaseClass baseClass = new BaseClass();
            //DerivedClass is casted to base class 
            BaseClass DerivedCastedToBase = new DerivedClass();
            DerivedClass derivedClass = new DerivedClass();
            baseClass.Method1();
            baseClass.Method2();
            baseClass.Method3();
            baseClass.Method4();
            DerivedCastedToBase.Method1();
            DerivedCastedToBase.Method2();
            DerivedCastedToBase.Method3();
            DerivedCastedToBase.Method4();
            derivedClass.Method1();
            derivedClass.Method2();
            derivedClass.Method3();
            derivedClass.Method4();
            //Output
            //BaseClass - Method1
            //BaseClass - Method2
            //BaseClass - Method3
            //BaseClass - Method4
            //DerivedCastedToBase, when casting derived class to base class the overridde method in the 
            //derived class is called instead of the method in the base class (DerivedClass - Method3).  
            //BaseClass - Method1
            //BaseClass - Method2
            //DerivedClass - Method3
            //BaseClass - Method4
            //BaseClass - Method1
            //DerivedClass - Method2
            //DerivedClass - Method3
            //DerivedClass - Method4


            //TestCars1 produces the following output.Notice especially the results for car2, which probably are not 
            //what you expected.The type of the object is ConvertibleCar, but DescribeCar does not access the version 
            //of ShowDetails that is defined in the ConvertibleCar class because that method is declared with the new 
            //modifier, not the override modifier.As a result, a ConvertibleCar object displays the same description 
            //as a Car object. Contrast the results for car3, which is a Minivan object. In this case, the ShowDetails 
            //method that is declared in the Minivan class overrides the ShowDetails method that is declared in the 
            //Car class, and the description that is displayed describes a minivan.
            StandardCar car1 = new StandardCar();
            car1.DescribeCar();

            //Notice the output from this test case. The new modifier is  
            //used in the definition of ShowDetails in the ConvertibleCar class.    
            //ShowDetails() of the base class is called because of the "public new void ShowDetails()" and not override keyword     
            ConvertibleCar car2 = new ConvertibleCar();
            car2.DescribeCar();

            Minivan car3 = new Minivan();
            car3.DescribeCar();

            //Output is the same as above   
            var cars = new List<StandardCar> { new StandardCar(), new ConvertibleCar(), new Minivan() };
            foreach (var car in cars)
            {
                car.DescribeCar();
            }

            #endregion

            Console.ReadLine();
        }

        #region [ Tuple methods ]

        //To access tuple elements use Item1-Item8 properties.
        //Tuples are created using generic types Tuple<T1>-Tuple<T1, T2, T3, T4, T5, T6, T7, T8>. 
        //Each of the types represents a tuple containing 1 to 8 elements.Elements can be of different types.
        public static void TupleExample()
        {
            //Unnamed tuples, you can access the properties using [Item1..ItemX]
            var tuple = new Tuple<string, int, bool, Car>("foo", 123, true, new Car() { Passengers=6 });
            string firstProp = tuple.Item1;
            int secondProp = tuple.Item2;
            bool thirdProp = tuple.Item3;
            Car fourthProp = tuple.Item4;
            Console.WriteLine($"Tuple content: {firstProp}, {secondProp}, {thirdProp}, {fourthProp.GetType()}, {fourthProp.Passengers}   ");
            //Tuples can also be created using static Tuple.Create methods. In this case, the types of the elements 
            //are inferred by the C# Compiler.
            var tuple1 = Tuple.Create<string, int, bool, Car>("foo", 123, true, new Car() { Passengers = 6 });
            Console.WriteLine($"Tuple1 content: {tuple1.Item1}, {tuple1.Item2}, {tuple1.Item3}, {tuple1.Item4.GetType()}, {tuple1.Item4.Passengers}   ");
            //Since C# 7.0, Tuples can be easily created using ValueTuple.
            var tuple2 = ("foo", 123, true, new Car() { Passengers = 6 });
            Console.WriteLine($"Tuple2 content: {tuple2.Item1}, {tuple2.Item2}, {tuple2.Item3}, {tuple2.Item4.GetType()}, {tuple2.Item4.Passengers}   ");
            //Elements can be named for easier decomposition, named tuples
            (string str, int number, bool flag, Car instance) tuple3 = ("foo", 123, true, new Car() { Passengers = 6 });
            Console.WriteLine($"Tuple3 content: {tuple3.str}, {tuple3.number}, {tuple3.flag}, {tuple3.instance.GetType()}, {tuple3.instance.Passengers}   ");
            //Tuples can be compared based on their elements.
            //As an example, an enumerable whose elements are of type Tuple can be sorted based on comparisons operators
            //defined on a specified element:
            List<Tuple<int, string>> list = new List<Tuple<int, string>>();
            list.Add(new Tuple<int, string>(6, "foo"));
            list.Add(new Tuple<int, string>(5, "bar"));
            list.Add(new Tuple<int, string>(2, "qux"));
            list.Sort((x, y) => x.Item1.CompareTo(y.Item1));
            foreach (var element in list)
            {
                Console.WriteLine($"Sorted tuple ascending {element}");
            }

            list.Sort((x, y) => y.Item1.CompareTo(x.Item1));
            foreach (var element in list)
            {
                Console.WriteLine($"Sorted tuple descending {element}");
            }
            //Return multiple values from a method
            var result = AddMultiply(25, 28);
            Console.WriteLine($"Tuple x + y = {result.Item1}");
            Console.WriteLine($"Tuple x * y = {result.Item2}");
            //Equality and tuples
            //Beginning with C# 7.3, tuple types support the == and != operators. These operators work by
            //comparing each member of the left argument to each member of the right argument in order.
            var left = (a: 5, b: 10);
            var right = (a: 5, b: 10);
            Console.WriteLine($"Tuples are equal {left == right}");
            double stdev = StandardDeviation(new double[] { 12.34, 6.78, 5.67, 7.89, 11.14 });
            Console.WriteLine($"Calculation stdev {string.Format("{0:0.00}", stdev)}");
        }

        //Delegate as alternative for function specification
        public static Tuple<int, int> AddMultiply(int x, int y) =>
            new Tuple<int, int>(x + y, x * y);

        //For one, the Tuple classes named their properties Item1, Item2, and so on.Those names carry no semantic information.Using these Tuple types does not enable communicating the meaning of each of the properties.The new language features enable you to declare and use semantically meaningful names for the elements in a tuple.
        //The Tuple classes cause more performance concerns because they are reference types.
        public static double StandardDeviation(IEnumerable<double> sequence)
        {
            var computation = ComputeSumAndSumOfSquares(sequence);
            //Equal calls
            //(int count, double sum, double sumOfSquares) = ComputeSumAndSumOfSquares(sequence);
            //var (sum, sumOfSquares, count) = ComputeSumAndSumOfSquares(sequence);
            //(double sum, var sumOfSquares, var count) = ComputeSumAndSumOfSquares(sequence);
            var variance = computation.SumOfSquares - computation.Sum * computation.Sum / computation.Count;
            return Math.Sqrt(variance / computation.Count);
        }

        //You can remove the field names from the return value declaration and return an unnamed tuple.
        //The fields of this return tuple are named Item1, Item2, and Item3. 
        //It's recommended that you provide semantic names to the elements of tuples returned from methods.
        //private static (int, double, double) ComputeSumAndSumOfSquares(IEnumerable<double> sequence)
        private static (int Count, double Sum, double SumOfSquares) ComputeSumAndSumOfSquares(IEnumerable<double> sequence)
        {
            double sum = 0;
            double sumOfSquares = 0;
            int count = 0;

            foreach (var item in sequence)
            {
                count++;
                sum += item;
                sumOfSquares += item * item;
            }

            return (count, sum, sumOfSquares);
        }

        #endregion

        #region [ What is new in C# 8.0 ]

        public static void IndicesExample()
        {
            int[] sequence = Enumerable.Range(0, 1000).Select(x => (int)(Math.Sqrt(x) * 100)).ToArray(); 
            
            for(int i = 0; i < sequence.Length; i += 100)
            {
                Range range = i..(i + 10);
                var (min, max, average) = MovingAverage(sequence, range);
                Console.WriteLine($"Forward: Range.Start[{range.Start}], " +
                    $"Range.End[{range.End}] Calculated (min[{min}], max[{max}], average[{average}])  ");
            }

            for (int i = 0; i < sequence.Length; i += 100)
            {
                Range range = ^(i + 10)..^i;
                var (min, max, average) = MovingAverage(sequence, range);
                Console.WriteLine($"Backward: Range.Start[{range.Start}], " +
                    $"Range.End[{range.End}] Calculated (min[{min}], max[{max}], average[{average}])  ");
            }

            //Local lambda function to calculate moving average, returning Tuple. 
            (int min, int max, double average) MovingAverage(int[] sequence, Range range) =>
                (
                    sequence[range].Min(),
                    sequence[range].Max(),
                    sequence[range].Average()
                );
        }

        public static void StaticLocalFunc()
        {
            foreach(int i in Counter(0, 10))
            {
                Console.WriteLine($"StaticLocalFunc() i:[{i}]");
            }
        }

        //Local functions were added in c# 7.0
        //Static local function were added in c# 8.0
        //By using a static local function, you know that the local static function 
        //is not using variables in its outer scope, the Just-In-Time (JIT) compiler can then make some optimizations     
        public static IEnumerable<int> Counter(int start, int end)
        {
            if (start > end)
                throw new ArgumentOutOfRangeException(nameof(start), "Start index should be less than end index.");

            return LocalCounter(start, end);

            static IEnumerable<int> LocalCounter(int start, int end)
            {
                for (int i = start; i < end; i++)
                {
                    Console.WriteLine($"yield return i:[{i}]");
                    yield return i;
                }                
            }
        }


        public static double Calculator(double x, double y, MathOperation operation)
        {
            double result = 0;

            //Switch Expressions
            result = operation switch
            {
                MathOperation.Add => x + y,
                MathOperation.Subtract => x - y,
                MathOperation.Multiply => x * y,
                MathOperation.Divide => x / y,
                _ => throw new NotImplementedException("Math operation not implemented.")
            };

            return result;

            //The switch statement
            /*switch (operation)
            {
                case MathOperation.Add:
                    result = x + y;
                    break;
                case MathOperation.Subtract:
                    result = x - y;
                    break;
                case MathOperation.Multiply:
                    result = x * y;
                    break;
                case MathOperation.Divide:
                    result = x / y;
                    break;
                default:
                    throw new NotImplementedException("Math operation not implemented.");
            }

            return result;*/
        }

        public enum MathOperation
        {
            Add,
            Subtract,
            Multiply,
            Divide
        }

        public struct Rectangle
        {
            public Rectangle(double height, double width) : this()
            {
                Height = height;
                Width = width;
            }

            private double _height;

            public double Height
            {
                readonly get { return _height; }
                private set { _height = value; }
            }

            private double _width;

            public double Width
            {
                readonly get { return _width; }
                private set { _width = value; }
            }

            //By marking the method as readonly, no defensive copy of the method is made, improves performance.
            //Note the get properties used by the method must also be readonly.  
            //A struct is a value type, its memory will be allocated on the stack.
            //When you pass a struct to a method,  a copy of the struct will be passed.
            //A class is a reference type, its memory will be allocated on the heap.
            public readonly double Area()
            {
                return Width * Height;
            }

        }

        //Note async methods should always return Task or use FireAndForget extension method 
        public static async void AsyncIterator()
        {
            AsyncIterators asyncIterators = new AsyncIterators();
            await asyncIterators.AsyncIterator();
        }

        private enum TimeBand
        {
            MorningRush,
            Daytime,
            EveningRush,
            Overnight
        }

        public static decimal CalculateToll(object vehicle, DateTime timeOfToll, bool inbound)
        {
            bool isWeekDay = timeOfToll.DayOfWeek switch
            {
                /*
                DayOfWeek.Monday => true,
                DayOfWeek.Tuesday => true,
                DayOfWeek.Wednesday => true,
                DayOfWeek.Thursday => true,
                DayOfWeek.Friday => true,
                DayOfWeek.Saturday => false,
                DayOfWeek.Sunday => false,
                _ => throw new ArgumentException(message: "Not known DayOfWeek", paramName: nameof(timeOfToll))
                */

                //Above can be simplified to this 
                DayOfWeek.Saturday => false,
                DayOfWeek.Sunday => false,
                _ => true 
            };

            TimeBand timeBand = TimeBand.Overnight;
            int hours = timeOfToll.Hour;
            if(hours < 6)
            {
                timeBand = TimeBand.Overnight;
            }
            else if(hours < 10)
            {
                timeBand = TimeBand.MorningRush;
            }
            else if (hours < 16)
            {
                timeBand = TimeBand.Daytime;
            }
            else if (hours < 20)
            {
                timeBand = TimeBand.EveningRush;
            }

            var peakTimePremium = (isWeekDay, timeBand, inbound) switch
            {
                (true, TimeBand.Overnight, _) => 0.75m,
                (true, TimeBand.Daytime, _) => 1.50m,
                (true, TimeBand.MorningRush, true) => 2.50m,
                (true, TimeBand.EveningRush, false) => 2.50m,
                //All other possibilities
                (_, _, _) => 1.00m,
            };

            //Switch on vehicle type  
            var toll = vehicle switch
            {
                Car { Passengers:3 } => 1.00m,
                Car { Passengers:2  } => 1.50m,
                Car _ => 2.00m,
                Taxi _ => 4.50m,
                Bus _ => 5.50m,
                Truck _ => 7.50m,
                //If some other type {}
                { } => throw new ArgumentException(message: "Not known vehicle type", paramName: nameof(vehicle)),
                null => throw new ArgumentNullException(nameof(vehicle))
            };

            return toll + peakTimePremium;
        }

        #endregion

        #region [ Yield keyword ]

        static IEnumerable<string> Filter(List<string> stringList)
        {
            foreach(string str in stringList)
            {
                if(str.Length > 3)
                {
                    Console.WriteLine($"Yield keyword method CALLED: {str}");
                    yield return str;
                }
            }
        }

        static IEnumerable<int> RunningTotal()
        {
            int total = 0;

            for(int i = 0; i < 3; i++)
            {
                total++;
                yield return total;
            }
        }

        #endregion

        #region [ ConfigurationManager ]

        /// </summary>
        /// <typeparam name="T">typeparam is the type in which value will be returned, it could be 
        /// any type eg. int, string, bool, decimal etc.</typeparam>
        /// <param name="strKey">key to find value from AppSettings</param>
        /// <param name="defaultValue">defaultValue will be returned in case of value is null or any
        /// exception occures</param>
        /// <returns>AppSettings value against key is returned in Type of default value or given as typeparam T</returns>
        public static T GetConfigKeyValue<T>(string strKey, T defaultValue)
        {
            var result = defaultValue;
            try
            {
                if (ConfigurationManager.AppSettings[strKey] != null)
                    result = (T)Convert.ChangeType(ConfigurationManager.AppSettings[strKey],
                    typeof(T));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception GetConfigKeyValue: {ex.Message}.");
            }
            return result;
        }

        /// <summary>
        /// Get value from AppSettings by key, convert to Type of default value or typeparam T and return
        /// </summary>
        /// <typeparam name="T">typeparam is the type in which value will be returned, it could be 
        /// any type eg. int, string, bool, decimal etc.</typeparam>
        /// <param name="strKey">key to find value from AppSettings</param>
        /// <returns>AppSettings value against key is returned in Type given as typeparam T</returns>
        public static T GetConfigKeyValue<T>(string strKey)
        {
            return GetConfigKeyValue(strKey, default(T));
        }

        static void ReadAllSettings()
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings.Count == 0)
                {
                    Console.WriteLine("AppSettings is empty.");
                }
                else
                {
                    foreach (var key in appSettings.AllKeys)
                    {
                        Console.WriteLine("Key: {0} Value: {1}", key, appSettings[key]);
                    }
                }
            }
            catch (ConfigurationErrorsException ex)
            {
                Console.WriteLine($"Error reading app settings {ex.Message}");
            }
        }

        static void GetConnectionStrings()
        {
            ConnectionStringSettingsCollection connectionSettings = ConfigurationManager.ConnectionStrings;

            if(connectionSettings == null)
            {
                Console.WriteLine("ConnectionSettings is empty.");
            }
            else
            {
                foreach (ConnectionStringSettings cs in connectionSettings)
                {
                    Console.WriteLine("Key: {0}, Provider: {1}, Value: {2}", cs.Name, cs.ProviderName, cs.ConnectionString);
                }
            }
        }

        static void ReadConnectionString(string key)
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings;
                if (connectionString == null)
                {
                    Console.WriteLine("ConnectionSettings is empty.");
                }
                else
                {
                    if(connectionString[key] == null)
                    {
                        Console.WriteLine($"Key {key}, Not Found");
                    }
                    else
                    {
                        Console.WriteLine("Key: {0}, Provider: {1}, Value: {2}", connectionString[key].Name, connectionString[key].ProviderName, connectionString[key].ConnectionString);
                    }
                }
            }
            catch (ConfigurationErrorsException ex)
            {
                Console.WriteLine($"Error reading connection string {ex.Message}");
            }
        }

        static void ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "Not Found";
                Console.WriteLine("Key: {0} Value: {1}", key, result);
            }
            catch (ConfigurationErrorsException ex)
            {
                Console.WriteLine($"Error reading app settings {ex.Message}");
            }
        }

        static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException ex)
            {
                Console.WriteLine($"Error reading app settings {ex.Message}");
            }
        }

        #endregion

    }
}

namespace VehicleRegistration
{
    public class Car
    {
        public int Passengers { get; set; }
    }

    public class Taxi
    {
        public int Fares { get; set; }
    }

    public class Bus
    {
        public int Capacity { get; set; }
        public int Riders { get; set; }
    }

    public class Truck
    {
        public int Weight { get; set; }
    }
}


