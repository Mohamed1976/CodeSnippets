using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace _70_483_USING_NET_FRAMEWORK.Exercises
{
    public class ExamNotes001
    {
        public ExamNotes001()
        {

        }

        public static Name ConvertToName(string json)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            return javaScriptSerializer.Deserialize<Name>(json);
        }

        private string WriteObject(Location location, XmlObjectSerializer serializer)
        {
            string json = default(string);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamReader streamReader = new StreamReader(memoryStream))
                {
                    serializer.WriteObject(memoryStream, location);
                    memoryStream.Position = 0;
                    json = streamReader.ReadToEnd();
                }
            }

            return json;
        }

        private void SerializeDictionary<TKey,TValue>(Dictionary<TKey, TValue> dictionary, out string Json)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string serResult = javaScriptSerializer.Serialize(dictionary);
            Json = serResult;
        }
        private void DeserializeDictionary<TKey, TValue>(out Dictionary<TKey, TValue> dictionary, string Json)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            Dictionary<TKey, TValue> _dictionary = javaScriptSerializer.Deserialize<Dictionary<TKey, TValue>>(Json);
            dictionary = _dictionary;
        }

        public void ProcessReports(List<decimal> values, CancellationToken ct)
        {
            for(int i = 0; i < 20; i++)
            {
                Thread.Sleep(250);
                Console.WriteLine("Waiting and anticipating: {0}",i);
                if (ct.IsCancellationRequested)
                    break;
            }

            if (ct.IsCancellationRequested)
            {
                Console.WriteLine("Task was cancelled.");
                ct.ThrowIfCancellationRequested();
            }
        }

        private async Task WriteData(string fileName, string buffer)
        {
            await Task.Run(() =>
            {
                StreamWriter streamWriter = null;
                //StreamWriter Constructor (String) Initializes a new instance of the StreamWriter 
                //class for the specified file by using the default encoding and buffer size.
                //The StreamWriter class has no method Open.
                streamWriter = new StreamWriter(fileName);
                streamWriter.Write(buffer);
                streamWriter.Close();
            });
        }

        private Task<string> Authorization(int statusCode)
        {
            return Task.Factory.StartNew<string>(() =>
            {
                string statusText = "Undefined";
                Thread.Sleep(500);

                switch(statusCode)
                {
                    case 0:
                        statusText = "Error";
                        break;
                    case 1:
                        statusText = "Success";
                        break;
                    default:
                        statusText = "Unauthorized";
                        break;
                }

                return statusText;
            });
        }

        private Task<string> SerializeObject<TObject>(TObject obj, XmlObjectSerializer ser) 
        {
            return Task.Factory.StartNew<string>(() =>
            {
                string serContent = default;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using(StreamReader streamReader = new StreamReader(memoryStream))
                    {
                        ser.WriteObject(memoryStream, obj);
                        memoryStream.Position = 0;
                        serContent = streamReader.ReadToEnd();
                    }
                }

                return serContent;
            });
        }

        public async Task Run()
        {
            //-----------------------------------------------------------------------------------
            // NetDataContractSerializer, DataContractSerializer
            //
            //The NetDataContractSerializer differs from the DataContractSerializer in one important way: 
            //the NetDataContractSerializer includes CLR type information in the serialized XML, whereas the 
            //DataContractSerializer does not.Therefore, the NetDataContractSerializer can be used only 
            //if both the serializing and deserializing ends share the same CLR types.
            //The serializer can serialize types to which either the DataContractAttribute or SerializableAttribute 
            //attribute has been applied. It also serializes types that implement ISerializable.
            //https://docs.microsoft.com/en-us/dotnet/api/system.runtime.serialization.netdatacontractserializer?view=netframework-4.8
            //-----------------------------------------------------------------------------------
            School school = new School(newfName: "Zighetti", newLName: "Barbara", newID: 101);
            NetDataContractSerializer serializer = new NetDataContractSerializer();
            string xml = await SerializeObject<School>(school, serializer);
            Console.WriteLine(xml+'\n');

            DataContractSerializer dataContractSerializer = new DataContractSerializer(school.GetType());
            xml = await SerializeObject<School>(school, dataContractSerializer);
            Console.WriteLine(xml + '\n');

            DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(school.GetType());
            xml = await SerializeObject<School>(school, dataContractJsonSerializer);
            Console.WriteLine(xml + '\n');
            
            //-----------------------------------------------------------------------------------
            //You are adding a method to an existing application.The method uses an integer named statusCode as an
            //input parameter and returns the status code as a string.
            //The method must meet the following requirements:
            //• Return "Error" if the statusCode is 0.
            //• Return "Success" if the statusCode is 1.
            //• Return "Unauthorized" if the statusCode is any value other than 0 or l.            
            //-----------------------------------------------------------------------------------
            string code = await Authorization(1);
            Console.WriteLine(code);

            await WriteData("message.txt", "The letter for the king.");
            Console.WriteLine("WriteData complete.");
            
            string player = "Johnny";
            int reward = 1000;
            string formatted = string.Format("Player {0}, collected {1} coins", player, reward.ToString("###0"));
            Console.WriteLine(formatted);
            //Formatted to three digits 099
            formatted = string.Format("Player {0}, collected {1:D3} coins", player, reward); 
            Console.WriteLine(formatted);
            formatted = string.Format("Player {0}, collected {1:000#} coins", player, reward);
            Console.WriteLine(formatted);
            double doubleValue = 2.897;
            decimal decimalValue = 2.897m;
            Console.WriteLine("Formatted double: {0:N2}", doubleValue);
            Console.WriteLine("Formatted decimal: {0:N2}", decimalValue);

            //-----------------------------------------------------------------------------------
            // DataContractJsonSerializer can also be used without DataContract, DataMember attributes  
            //-----------------------------------------------------------------------------------
            List<Patient> patients = new List<Patient>()
            {
                new Patient() { Id=1, IsActive=true, Name="Joe" },
                new Patient() { Id=2, IsActive=true, Name="Mark" }
            };

            //DataContractJsonSerializer dataContractJson = new DataContractJsonSerializer(typeof(List<Patient>));
            //The same as
            DataContractJsonSerializer dataContractJson = new DataContractJsonSerializer(patients.GetType());
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamReader streamReader = new StreamReader(memoryStream))
                {
                    //Serialize
                    dataContractJson.WriteObject(memoryStream, patients);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    string json2 = streamReader.ReadToEnd();
                    Console.WriteLine(json2);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    //Deserialize
                    List<Patient> patients2 = dataContractJson.ReadObject(memoryStream) as List<Patient>;
                    if(patients2 != null)
                    {
                        foreach (Patient patient in patients2)
                            Console.WriteLine(patient);
                    }
                }
            }

            //-----------------------------------------------------------------------------------
            // Object casting 
            //-----------------------------------------------------------------------------------
            Widget widget = new Widget();
            widget.DoWork((object)new Widget());
            widget.DoWork((ItemBase)new Widget());
            widget.DoWork(new Widget());

            //-----------------------------------------------------------------------------------
            // You are implementing a method named ProcessReports that performs a long-running task.
            // ProcessReports() method has the following method signature:
            // public void ProcessReports(List<decimal> values, CancellationToken ct)
            // If the calling code requests cancellation, the method must perform the following actions:
            // • Cancel the long-running task.
            // • Set the task status to TaskStatus.Canceled.
            //-----------------------------------------------------------------------------------
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Task task = Task.Factory.StartNew(() =>
            {
                ProcessReports(new List<decimal>() { 1.00m, 2.20m }, cancellationTokenSource.Token);
            }, TaskCreationOptions.DenyChildAttach);

#pragma warning disable CS4014
            //No need to wait for the Task to finish
            Task.Run(() =>
            {
                Thread.Sleep(300);
                cancellationTokenSource.Cancel();
            });
#pragma warning restore CS4014

            await task.ContinueWith((finishedTask) =>
            {
                Console.WriteLine("Task result: {0}", finishedTask.Status);
                /*if (task.Status == TaskStatus.RanToCompletion)
                {
                    Console.WriteLine("Task completed successfully, result: {0}", task.Result);
                }
                else if (task.Status == TaskStatus.Faulted)
                {
                    Console.WriteLine("Task has faulted.");
                }
                else if (task.Status == TaskStatus.Canceled)
                {
                    Console.WriteLine("Task has been canceled.");
                } */
            });

            //-----------------------------------------------------------------------------------
            //Typecast
            //-----------------------------------------------------------------------------------
            ArrayList arrayList = new ArrayList();
            int var1 = 10, var2;
            arrayList.Add(var1);
            var2 = (int)arrayList[0];
            Console.WriteLine(var2);

            //-----------------------------------------------------------------------------------
            // Dictionary JSON Serializer/Deserializer
            //
            //The JavaScriptSerializer Class Provides serialization and deserialization functionality for AJAX - enabled
            //applications.
            //The JavaScriptSerializer class is used internally by the asynchronous communication layer to serialize and
            //deserialize the data that is passed between the browser and the Web server.You cannot access that
            //instance of the serializer.However, this class exposes a public API.Therefore, you can use the class when
            //you want to work with JavaScript Object Notation(JSON) in managed code.
            //-----------------------------------------------------------------------------------
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "Alfa", 2020},
                { "Beta", 2030},
                { "Gamma", 2050}
            };

            string Json = default;
            SerializeDictionary<string, object>(dictionary, out Json);
            Console.WriteLine(Json);

            Dictionary<string, object> dictionary1 = default;
            DeserializeDictionary<string, object>(out dictionary1, Json);
            foreach (KeyValuePair<string, object> keyValuePair in dictionary1)
                Console.WriteLine("Key: {0}, Value: {1}", keyValuePair.Key, (int)keyValuePair.Value);

            //-----------------------------------------------------------------------------------
            // DataContractJsonSerializer
            //-----------------------------------------------------------------------------------
            Location location = new Location() { Direction = Compass.North, Label = "New world" };
            string json3 = WriteObject(location, new DataContractJsonSerializer(typeof(Location)));
            Console.WriteLine("Json serialization: " + json3);

            //XML serialization
            string xml3 = WriteObject(location, new DataContractSerializer(location.GetType()));
            Console.WriteLine("Xml serialization: " + xml3);

            //-----------------------------------------------------------------------------------
            // Delegate usage
            //-----------------------------------------------------------------------------------
            UserTracker userTracker = new UserTracker();
            userTracker.AddUser("Bobby", delegate (int count)
            {
                Console.WriteLine("Nr of users added: {0}", count);
            });

            //We could also use lambda expression instead of delegate
            userTracker.AddUser("Suzanne", (int count) => //(count) =>, datatype infered by CLR
            {
                Console.WriteLine("Nr of users added: {0}", count);
            });

            userTracker.Print();
            
            //-----------------------------------------------------------------------------------
            // JavaScriptSerializer
            //-----------------------------------------------------------------------------------
            string json = "{ \"FirstName\" : \"David\", \"LastName\" : \"Jones\", \"Values\" : [0, 1, 2] }";
            Name name = ConvertToName(json);
            Console.WriteLine("{0:f}", name);
            Console.WriteLine("{0:s}", name);
            
            //-----------------------------------------------------------------------------------
            //Queues are useful for storing messages in the order they were received for sequential processing.
            //Objects stored in a Queue<T> are inserted at one end and removed from the other.
            //You can iterate a Queue object.  
            //-----------------------------------------------------------------------------------
            Queue<string> numbers = new Queue<string>();
            numbers.Enqueue("one");
            numbers.Enqueue("two");
            numbers.Enqueue("three");
            numbers.Enqueue("four");
            numbers.Enqueue("five");

            // A queue can be enumerated without disturbing its contents.
            foreach (string number in numbers)
                Console.WriteLine(number);
        }
    }

    public class Name : IFormattable
    {
        public int[] Values { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            string name = FirstName;

            if(format.Equals("f",StringComparison.OrdinalIgnoreCase))
            {
                name = FirstName + ", " + LastName;
                
                for(int i = 0; i < Values.Length; i++)
                {
                    name += ", " + Values[i];
                }
            }
            else if(format.Equals("s", StringComparison.OrdinalIgnoreCase))
            {
                name = FirstName + ", " + LastName;
            }

            return name;
        }
    }

    public enum Compass
    {
        North,
        South,
        East,
        West
    }

    [DataContract]
    public class Location
    {
        [DataMember]
        public string Label { get; set; }

        [DataMember]
        public Compass Direction {  get; set; }
    }

    public delegate void AddUserCallback(int i);
    public class UserTracker
    {
        private List<User> users = new List<User>();
        public void AddUser(string name, AddUserCallback callback)
        {
            users.Add(new User(name));
            callback(users.Count);
        }

        public void Print()
        {
            foreach (User user in users)
                Console.WriteLine(user.Name);
        }
    }

    public class User
    {
        public User(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }

    public class ItemBase
    {
    }

    public class Widget : ItemBase
    {
        public void DoWork(object obj)
        {
            Console.WriteLine("In DoWork(object obj)");
        }

        public void DoWork(Widget widget)
        {
            Console.WriteLine("In DoWork(Widget widget)");
        }

        public void DoWork(ItemBase itemBase)
        {
            Console.WriteLine("In DoWork(ItemBase itemBase)");
        }
    }

    public class Patient
    {
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", Name, Id, IsActive);
        }
    }

    // You must apply a DataContractAttribute or SerializableAttribute
    // to a class to have it serialized by the NetDataContractSerializer.
    [DataContract(Name = "Customer", Namespace = "http://www.contoso.com")]
    public class School
    {
        public School(string newfName, string newLName, int newID)
        {
            FirstName = newfName;
            LastName = newLName;
            ID = newID;
        }

        //Order, determines the position of the property in the XML file 
        [DataMember(Name = "SureName", Order = 3)]
        public string FirstName;
        [DataMember(Name = "_LastName", Order = 2)]
        public string LastName;
        [DataMember(Name = "SchoolID", Order = 1)]
        public int ID;
    }
}
