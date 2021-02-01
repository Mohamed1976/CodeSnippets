using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleWithDI.Exercises
{
    public class GeneralExercises
    {
        public async Task Run()
        {
            await Task.Delay(100);
            //CSharp9Features();
            RecordExample();
        }
        
        private void RecordExample()
        {
            //You can inherit from Records 
            PersonRecord rec1 = new PersonRecord(FirstName:"Mark", "Spenser");
            PersonRecord rec2 = new PersonRecord(FirstName: "Donald", "Duck");
            PersonRecord rec3 = new PersonRecord(FirstName: "Mickey", "Mouse");
            PersonRecord rec4 = new PersonRecord(FirstName: "Mickey", "Mouse");

            Person person1 = new Person(lastName:"Connery_1", firstName:"John_1");
            Person person2 = new Person(lastName: "Connery_2", firstName: "John_2");
            Person person3 = new Person(lastName: "Connery_3", firstName: "John_3");
            Person person4 = new Person(lastName: "Connery_3", firstName: "John_3");

            Console.WriteLine("Record Type: ");
            Console.WriteLine($"rec1 ToString(): {rec1}");
            //The == Operator compares the reference identity while the Equals() method compares only contents.
            Console.WriteLine($"Equals(rec3, rec4): { Equals(rec3, rec4) }");
            Console.WriteLine($"ReferenceEquals(rec3, rec4): { ReferenceEquals(rec3, rec4) }");
            Console.WriteLine($"rec3 == rec4: { rec3 == rec4 }");
            Console.WriteLine($"rec3 != rec4: { rec3 != rec4 }");
            Console.WriteLine($"HashCode of rec3: {rec3.GetHashCode()}");
            Console.WriteLine($"HashCode of rec4: {rec4.GetHashCode()}");
            Console.WriteLine($"HashCode of rec2: {rec2.GetHashCode()}");

            //You can use a anonymous tuple to retrieve the record values.
            var (FirstName, LastName) = rec2;
            Console.WriteLine($"Tuple values: FirstName:{FirstName}, LastName:{LastName}");
            //You can copy a record and for example modify just one property  
            PersonRecord rec5 = rec4 with
            {
                LastName = "Kleine Maus"
            };

            Console.WriteLine($"rec5.ToString(): {rec5}");

            EmployeeRecord bill = new EmployeeRecord("Bill", "Gates", (decimal)1.99);
            Console.WriteLine($"bill.ToString(): {bill}");
            Console.WriteLine($"bill.FullName(): {bill.FullName}, ${bill.GetYearSalary()}");
            ProgrammerRecord peter = new ProgrammerRecord("Peter","Deer",11,(decimal)2.99);
            Console.WriteLine($"peter.FullName(): {peter.FullName}, ${peter.GetYearSalary()} {peter.EvaluateExperience()}");


            Console.WriteLine("");
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("");

            Console.WriteLine("Class Type: ");
            Console.WriteLine($"person1 ToString(): {person1}");
            Console.WriteLine($"Equals(person3, person4): { Equals(person3, person4) }");
            Console.WriteLine($"ReferenceEquals(person3, person4): { ReferenceEquals(person3, person4) }");
            Console.WriteLine($"person3 == person4: { person3 == person4 }");
            Console.WriteLine($"person3 != person4: { person3 != person4 }");
            Console.WriteLine($"HashCode of person3: {person3.GetHashCode()}");
            Console.WriteLine($"HashCode of person4: {person4.GetHashCode()}");
            Console.WriteLine($"HashCode of person2: {person2.GetHashCode()}");

            var (PersonFirstName, PersonLastName) = person3;
            Console.WriteLine($"Tuple values: FirstName:{PersonFirstName}, LastName:{PersonLastName}");

        }

        //Intro to Records in C# 9 - How To Use Records And When To Use Them
        //https://www.youtube.com/watch?v=9Byvwa9yF-I
        //Record is a fancy class with extra stuff, it is a reference type but acts as a value type.
        //Is immutable
        public record PersonRecord(string FirstName, string LastName);

        public record EmployeeRecord(string FirstName, string LastName, decimal Salary)
        {
            public string FullName 
            {
                get
                {
                    return $"{FirstName} {LastName}";
                }
            }

            public decimal GetYearSalary()
            {
                return Salary * 12;
            }
        }

        public record ProgrammerRecord(string FirstName, string LastName, int YearsOfExperience, decimal Salary) :
            EmployeeRecord(FirstName, LastName, Salary)
        {
            public string EvaluateExperience()
            {
                if(YearsOfExperience > 10)
                {
                    return "Great";
                }
                else
                {
                    return "Good";
                }
            }
        } 

        //Records can also be contained in properties of classes
        public class AuditEmployee
        {
            public AuditEmployee(EmployeeRecord employee)
            {
                Employee = employee;
                References = new List<PersonRecord>();
            }

            public EmployeeRecord Employee { get; set; }
            public List<PersonRecord> References { get; set; }
        }

        //Declaration of Record1 is simillar to Class1.   
        public class Person
        {
            public Person(string firstName, string lastName)
            {
                this.FirstName = firstName;
                this.LastName = lastName;
            }

            public string FirstName { get; init; }
            public string LastName { get; init; }

            public void Deconstruct(out string firstName, out string lastName)
            {
                firstName = this.FirstName;
                lastName = this.LastName;
            }
        }


        //.NET 5 released in November 2020, .NET 6 will be released in November 2021 ... etc
        //Note: even .NET framework versions have long time support 3 years (.NET 4, .NET 6 .. etc)
        //Big Changes in .NET 5, C# 9, and Visual Studio 2019 (v16.8)
        //Records value types, makes a shallow copy  
        //https://www.youtube.com/watch?v=zjVgQNfAEOs
        //
        private void CSharp9Features()
        {
            Console.WriteLine("CSharp9Features()");
            Exercises.Person person1 = new Exercises.Person(1,"John", "O'Connor");
            //person1.Id = 2; prohibited
            Console.WriteLine(person1);
            Exercises.Person person2 = new Exercises.Person() { Id = 2, FirstName = "Sarah", LastName = "Connor" };
            //person1.Id = 2; prohibited
            Console.WriteLine(person2);

            Exercises.Person person3 = default;
            if(person3 is not null)
                Console.WriteLine("person3  is not null");
            else if(person3 is null)
                Console.WriteLine("person3  is null");

            Exercises.Person[] persons = new Exercises.Person[]
            {
                //Instead of using: new Person(1,"John", "O'Connor")  
                //We can use simplified syntax 
                new (1, "Linda", "Hamilton"),
                new (2, "Kyle", "Reese"),
                new (3, "John", "Connor"),
            };

            foreach (Exercises.Person person in persons)
            {
                Console.WriteLine(person);
            }

            Console.WriteLine($"-20 => {TempInterpreter(-20)}");
            Console.WriteLine($"10 => {TempInterpreter(10)}");
            Console.WriteLine($"100 => {TempInterpreter(100)}");
            Console.WriteLine($"-101 => {TempInterpreter(-101)}");
        }

        private string TempInterpreter(int tempDegCelsius)
        {
            string interpretation = tempDegCelsius switch
            {
                /* You can use the and, or operators  */
                >= -100 and < 0 => "Cold, but liveable",
                >= 0  and < 10 => "Cold",
                >= 10 and < 80 => "Warm, but liveable",
                _ => "Unlivable"
            };

            return interpretation;
        }
    }

    public class Person
    {
        public Person()
        {
        }

        public Person(int id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        //Init means that property can only be set once. 
        public int Id { get; init; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public override string ToString()
        {
            return $"{Id},  {FirstName}, {LastName}";
        }
    }


}
