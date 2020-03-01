//Conditional compilation using attributes
//#define TERSE
#define VERBOSE

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Reflection;
using static _70_483.OOP.ClassHierarchyExamples;
using System.Linq;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;

namespace _70_483.Reflection
{
    class ReflectionExamples
    {
        public ReflectionExamples()
        {
                
        }

        public void Run()
        {
            //Generate code at runtime by using CodeDOM
            //Using Code Document Object Model (CodeDOM) applications can create code at runtime.
            //There are document object models for XML,
            //JSON and HTML documents, and there is also one that is used to represent the
            //structure of a class. This is called a CodeDOM object.
            //A CodeDOM object can be parsed to create a source file or an executable assembly.
            //You can create either Visual Basic .NET or C# source files from a given
            //CodeDOM object and you can create CodeDOM objects using either language.
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            // Create a namespace to hold the types we are going to create
            CodeNamespace personnelNameSpace = new CodeNamespace("Personnel");
            // Import the system namespace
            personnelNameSpace.Imports.Add(new CodeNamespaceImport("System"));
            // Create a Person class
            CodeTypeDeclaration personClass = new CodeTypeDeclaration("Person");
            personClass.IsClass = true;
            personClass.TypeAttributes = System.Reflection.TypeAttributes.Public;
            // Add the Person class to personnelNamespace
            personnelNameSpace.Types.Add(personClass);
            // Create a field to hold the name of a person
            CodeMemberField nameField = new CodeMemberField("String", "name");
            nameField.Attributes = MemberAttributes.Private;
            // Add the name field to the Person class
            personClass.Members.Add(nameField);
            // Add the namespace to the document
            compileUnit.Namespaces.Add(personnelNameSpace);
            //Once the CodeDOM object has been created you can create a
            //CodeDomProvider to parse the code document and produce the program code from it.
            // Now we need to send our document somewhere
            // Create a provider to parse the document
            //You can generate code for c#, VB, JavaScript 
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            // Give the provider somewhere to send the parsed output
            StringWriter strW = new StringWriter();
            // Set some options for the parse - we can use the defaults
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            // Generate the C# source from the CodeDOM
            provider.GenerateCodeFromCompileUnit(compileUnit, strW, options);
            // Close the output stream
            strW.Close();
            // Print the C# output
            Console.WriteLine(strW.ToString());

            //Finding components in assemblies
            //Search an assembly for a particular type of component class.
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            List<Type> AccountTypes = new List<Type>();
            foreach (Type t in thisAssembly.GetTypes())
            {
                if (t.IsInterface)
                    continue;
                if (typeof(IAccount).IsAssignableFrom(t))
                {
                    AccountTypes.Add(t);
                }
            }

            foreach(Type AccountType in AccountTypes)
            {
                Console.WriteLine("IAccount type: {0}", AccountType);
            }

            //You can simplify the identification of the types by using a LINQ query as shown below.
            //var _AccountTypes = from __type in thisAssembly.GetTypes()
            //          where typeof(IAccount).IsAssignableFrom(__type) && !__type.IsInterface
            //          select __type;

            //Using reflection
            //The Type of an object contains all the fields of an object, along with all the
            //metadata describing the object.You can use methods and objects in the
            //System.Reflection namespace to work with Type objects.
            Manager manager = new Manager("Mohamed", 40);
            Type type = manager.GetType();
            Console.WriteLine("Manager type: {0}", type.ToString());
            //Note that the Name property has been implemented by the compiler as a pair    
            //of get and set methods(set_Name and get_Name), and the class contains
            //all the methods that are exposed by the parent object, including ToString and, of course, the GetType method.
            foreach (MemberInfo member in type.GetMembers())
            {
                Console.WriteLine(member.ToString());
            }

            //It is possible to load an assembly from a file by using the Assembly.Load
            //method.The statement below would load all the types in a file called
            //BankTypes.dll.This means that at its start an application could search a
            //particular folder for assembly files, load them and then search for classes that
            //can be used in the application.
            //Assembly bankTypes = Assembly.Load("BankTypes.dll");

            //Note using reflextion slows down the program.
            //You can use the information provided by a type to create a call to a method in that type.
            //We can do this by finding the MethodInfo for this method and then calling the Invoke method on this reference.
            //A program can now obtain a reference to an object, find out what
            //behaviors that object exposes, and then make use of the behaviors that it needs.             
            MethodInfo setMethod = type.GetMethod("set_Name");
            setMethod.Invoke(manager, new object[] { "Fred" });
            Console.WriteLine("Manager name after invoke: " + manager.Name);
            
            //Conditional compilation using attributes
            //You can use the Conditional attribute to activate and de-activate the
            //contents of methods. This attribute is declared in the System.Diagnostics namespace.
            //The body of the reportHeader method will be obeyed if either the TERSE or the
            //VERBOSE symbols are defined because two attributes are combined before that method definition.
            Manager.reportHeader(); //Body is executed because VERBOSE condition is defined
            Manager.terseReport(); //Body is not executed because Terse condition is not met
            Manager.verboseReport(); //Body is executed because VERBOSE is defined.

            //Testing for attributes
            //C# allows you to add metadata to an application in the form of attributes that
            //are attached to classes and class members. An attribute is an instance of a class
            //that extends the Attribute class.
            //A program can check that a given class has a particular attribute class attached to
            //it by using the IsDefined method, which is a static member of the
            //Attribute class. The IsDefined method accepts two parameters; the first
            //is the type of the class being tested and the second type of the attribute class that
            //the test is looking for. This test just tells us that a given class has an attribute of a particular type.
            if (Attribute.IsDefined(typeof(Manager), typeof(SerializableAttribute)))
                Console.WriteLine("Manager can be serialized");

            //You can create your own attribute classes to help manage elements of your application.
            //The data values stored in an attribute instances are set from the
            //class metadata when the attribute is loaded.A program can change them as it
            //runs, but these changes will be lost when the program ends.
            //The manager class has the programmer attribute set, see code below 
            //If the attribute is not defined on the class, GetCustomAttribute returns null.
            Attribute attribute = Attribute.GetCustomAttribute(typeof(Manager), typeof(ProgrammerAttribute));
            if (attribute != null)
                Console.WriteLine("Manager class programmer: " + ((ProgrammerAttribute)attribute).Programmer);
        }
    }

    //When you create an attribute class, the proper
    //practice is to add attribute usage information to the declaration of the attribute
    //class, so that the compiler can make sure that the attribute is only used in meaningful situations.
    //Perhaps you don’t want to be able to assign Programmer attributes to the methods in a class. 
    //You only want to assign a Programmer attribute to the class itself.
    //The AttributeUseage control how the attribute is used.
    //The AttributeUseage settings below only allow the Programmer attribute to be applied to class declarations.
    //If you try to use the ProgrammerAttribute on anything other than a class you will find that the compiler will generate errors.
    [AttributeUsage(AttributeTargets.Class)]
    //[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    class ProgrammerAttribute : Attribute
    {
        private string programmerValue;
        public ProgrammerAttribute(string programmer)
        {
            programmerValue = programmer;
        }

      //the programmer name is stored as a read-only
      //property of the attribute.We could have made the programmer name a writable
      //property(added a set behavior to the Programmer property), but this is not
      //sensible, because changes to the programmer name are not persisted when the program ends.
        public string Programmer
        {
            get
            {
                return programmerValue;
            }
        }
    }

    //You can add the Programmer attribute to elements in your program in the
    //same way as when adding the Serializable attribute, although in this case
    //the attribute constructor must be called to set the programmer name.    
    [ProgrammerAttribute(programmer: "Mohamed")]
    //The Serializable attribute doesn’t actually hold any data, it is the
    //fact that a class has a Serializable attribute instance attached to it means
    //that the class is may be opened and read by a serializer.
    //A serializer takes the entire contents of a class and sends it into a stream.
    //Note that some serializers, notably the XMLSerializer and the
    //JSONSerializer, don’t need classes to be marked as serializable before they can work with them.
    [Serializable]
    public class Manager
    {
        public int Age;
        //The Manager class also contains a
        //NonSerialized attribute that is applied to the screenPosition member
        //variable.This member of the class is only used to manage the display of a
        //Person object and should not be saved when it is serialized.This tells the
        //serializer not to save the value of screenPosition.
        [NonSerialized]
        // No need to save this
        private int screenPosition;
        public Manager(string name, int age)
        {
            _name = name;
            Age = age;
            screenPosition = 0;
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        //Note that the Conditional attribute controls whether or not the body of a
        //given method is obeyed when the method is called, it does not control whether
        //or not the method itself is passed to the compiler.The Conditional attribute
        //does not perform the same function as conditional compilation in languages such
        //as C and C++, it does not prevent code from being passed to the compiler, rather
        //it controls whether code is executed when it runs.
        [Conditional("TERSE")]
        public static void terseReport()
        {
            Console.WriteLine("This is output from the terse report.");
        }

        [Conditional("VERBOSE")]
        public static void verboseReport()
        {
            Console.WriteLine("This is output from the verbose report.");
        }

        [Conditional("VERBOSE"), Conditional("TERSE")]
        public static void reportHeader()
        {
            Console.WriteLine("This is the header for the report");
        }
    }

}
