using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSnippets
{
    public partial struct PartialStruct
    {
        public void Struct_Test() 
        {
            Console.WriteLine($"From partial struct S1, Struct_Test()");
        }
    }

    public partial struct PartialStruct
    {
        public void Struct_Test2() 
        {
            Console.WriteLine($"From partial struct S1, Struct_Test2()");
        }
    }

    //Interfaces can also be defined as partial interface 
    public partial interface ITest
    {
        void DoWork();
    }

    public partial interface ITest
    {
        void GoToLunch();
    }

    public partial class Customer : ITest
    {

        public Customer(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
        public void DoWork()
        {
            Console.WriteLine($"From partial class Customer, FirstName: {FirstName}");
        }

        private string FirstName { get; set; }
        private string LastName { get; set; }
    }

    public partial class Customer
    {
        public void GoToLunch()
        {
            Console.WriteLine($"From partial class Customer, LastName: {LastName}");
        }
    }

    class BaseClass
    {
        public void Method1()
        {
            Console.WriteLine("BaseClass - Method1");
        }

        public void Method2()
        {
            Console.WriteLine("BaseClass - Method2");
        }

        public virtual void Method3()
        {
            Console.WriteLine("BaseClass - Method3");
        }

        public virtual void Method4()
        {
            Console.WriteLine("BaseClass - Method4");
        }
    }

    class DerivedClass : BaseClass
    {
        //When you build the project, you see that the addition of the Method2 method in BaseClass causes 
        //a warning.The warning says that the Method2 method in DerivedClass hides the Method2 method in
        //BaseClass.You are advised to use the new keyword in the Method2 definition if you intend to cause 
        //that result.Alternatively, you could rename one of the Method2 methods to resolve the warning, 
        //but that is not always practical.
        //public void Method2()
        //{
        //    Console.WriteLine("DerivedClass - Method2");
        //}

        //The new keyword preserves the relationships that produce that output, but it suppresses the warning. 
        //The variables that have type BaseClass continue to access the members of BaseClass, and the variable 
        //that has type DerivedClass continues to access members in DerivedClass first, and then to consider 
        //members inherited from BaseClass. To suppress the warning, add the new modifier to the definition of 
        //Method2 in DerivedClass, as shown in the following code.The modifier can be added before or after public.
        public new void Method2()
        {
            Console.WriteLine("DerivedClass - Method2");
        }

        //The use of the override modifier enables to access the Method3 method that is defined in DerivedClass.
        //Typically, that is the desired behavior in inheritance hierarchies.You want objects that have values 
        //that are created from the derived class to use the methods that are defined in the derived class. 
        //You achieve that behavior by using override to extend the base class method.
        public override void Method3()
        {
            Console.WriteLine("DerivedClass - Method3");
        }

        public new void Method4()
        {
            Console.WriteLine("DerivedClass - Method4");
        }
    }

    //The following example illustrates similar behavior in a different context. The example defines three classes: 
    //a base class named Car and two classes that are derived from it, ConvertibleCar and Minivan. The base class 
    //contains a DescribeCar method. The method displays a basic description of a car, and then calls ShowDetails 
    //to provide additional information. Each of the three classes defines a ShowDetails method. The new modifier 
    //is used to define ShowDetails in the ConvertibleCar class. The override modifier is used to define ShowDetails 
    //in the Minivan class. Define the base class, Car. The class defines two methods,  
    // DescribeCar and ShowDetails. DescribeCar calls ShowDetails, and each derived  
    // class also defines a ShowDetails method. The example tests which version of  
    // ShowDetails is selected, the base class method or the derived class method.  
    class StandardCar
    {
        public void DescribeCar()
        {
            System.Console.WriteLine("Four wheels and an engine.");
            ShowDetails();
        }

        public virtual void ShowDetails()
        {
            System.Console.WriteLine("Class StandardCar.ShowDetails(), Standard transportation.");
        }
    }

    // Class ConvertibleCar uses the new modifier to acknowledge that ShowDetails  
    // hides the base class method.  
    class ConvertibleCar : StandardCar
    {
        public new void ShowDetails()
        {
            System.Console.WriteLine("Class ConvertibleCar.ShowDetails(), A roof that opens up.");
        }
    }

    // Class Minivan uses the override modifier to specify that ShowDetails  
    // extends the base class method.  
    class Minivan : StandardCar
    {
        public override void ShowDetails()
        {
            System.Console.WriteLine("Class Minivan.ShowDetails(), Carries seven people.");
        }
    }
}
