using System;
using System.Collections.Generic;
using System.Text;

namespace _70_483.ProgramFlow
{
    class ProgramFlowExamples
    {
        public ProgramFlowExamples()
        {

        }

        public void Run()
        {
            //C# provides a number of constructions that can implement looping behaviors:
            //the while loop, the do loop, and the for loop and the foreach loop.
            //LoopExamples();
            //LoopBreakExamples();
            //GotoExamples();
            //SwitchExamples();
            EvaluatingExpressions();
        }

        private void EvaluatingExpressions()
        {
            //Logical OR operator |
            uint a = 0b_1010_0000;
            uint b = 0b_1001_0001;
            uint c = a | b;
            Console.WriteLine(Convert.ToString(c, toBase: 2)); // Output: 10110001

            //Logical exclusive OR operator ^
            uint aa = 0b_1111_1000;
            uint bb = 0b_0001_1100;
            uint cc = aa ^ bb;
            Console.WriteLine(Convert.ToString(cc, toBase: 2)); // Output: 11100100

            //Logical AND operator &
            uint aaa = 0b_1111_1000;
            uint bbb = 0b_1001_1101;
            uint ccc = aaa & bbb;
            Console.WriteLine(Convert.ToString(ccc, toBase: 2)); // 10011000

            string name = null;
            string result = name ?? "Name was empty.";
            Console.WriteLine(result);
            //Available in C# 8.0 and later, the null-coalescing assignment operator ??= assigns the value of its right-hand 
            //operand to its left-hand operand only if the left-hand operand evaluates to null. The ??= operator doesn't evaluate 
            //its right-hand operand if the left-hand operand evaluates to non-null.
            name ??= new string("New created string.");
            Console.WriteLine(name);

            //An operator can work on one operand, and such operands are called unary or monadic.
            //Unary or monadic operators are either prefix (given before the operand) or postfix (given after the operand).
            //Alternatively, an operand can work on two (binary), or in the case of the conditional operator ?: three(ternary) operands.
            int counter = 0;
            // Prefix monadic operator - perform before value given
            Console.WriteLine(++counter); //Prints 1, increments and prints 
            // Postfix monadic operator -perform after value given
            Console.WriteLine(counter++); //Prints 1, prints afterwhich it increments the variable  

            //The context of the use of an operator determines the actual behavior that the operator will exhibit.
            //For example, the addition operator can be used to add two
            //numeric operands together, or concatenate two strings together.The use of an
            //incorrect context(for example adding a number to a string) will be detected by
            //the compiler and cause a compilation error.
            int number = 8 + 9;
            string str = "Alfa" + "Beta";

            //Each operator has a priority or precedence that determines when it is
            //performed during expression evaluation. This precedence can be overridden by
            //the use of parenthesis; elements enclosed in parenthesis are evaluated first.
            //Operators also have an associability, which gives the order(left to right or right
            //to left) in which they are evaluated if a number of them appear together.
            // Binary operators - two operands
            int i = 1 + 1; // sets i to 2
            //In an expression with multiple operators, the operators with higher precedence are evaluated before the operators 
            //with lower precedence. In the following example, the multiplication is performed first because it has higher  precedence than addition:
            i = 1 + 2 * 3; // sets i to 7 because * performed first
            i = (1 + 2) * 3; // sets i to 9 because + performed first

            // ternary operators - three operands
            i = i > 0 ? 0 : 1; // sets i to 0 because condition is true;
        }

        private void SwitchExamples()
        {
            int number = 3;
            //Each clause must be explicitly ended with a break, a return, or by the program throwing an exception.
            //The switch construction will switch on character, string and enumerated values, and it is possible to group cases.
            switch (number)
            {
                case 1:
                case 2:
                case 3:
                    Console.WriteLine($"Switched value [1, 2, 3] == {number}");
                    break;
                case 4:
                    Console.WriteLine($"Switched value 4 == {number}");
                    break;
                default:
                    Console.WriteLine($"Unknown value, default switch statement  ");
                    break;
            }
        }

        //The goto statement transfers the program control directly to a labeled statement.
        //A common use of goto is to transfer control to a specific switch-case label or the default label in a switch statement.
        //The goto statement is also useful to get out of deeply nested loops.
            private void GotoExamples()
        {
            int n = 3;
            decimal cost = 0;

            switch (n)
            {
                case 1:
                    cost += 25;
                    break;
                case 2:
                    cost += 25;
                    goto case 1;
                case 3:
                    cost += 50;
                    goto case 1;
                default:
                    Console.WriteLine("Invalid selection.");
                    break;
            }

            Console.WriteLine($"Total costs {cost} == 75 cents.");
            
            int[,] array2D = new int[4, 3] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 }, { 10, 11, 12 } };
            int searchNumber = 13;

            // Search.
            for (int i = 0; i < array2D.GetLength(0); i++)
            {
                for (int j = 0; j < array2D.GetLength(1); j++)
                {
                    if(array2D[i, j] == searchNumber)
                    {
                        Console.WriteLine($"The number {array2D[i, j]}, i[{i}], j[{j}] is found.");
                        goto Found;
                    }
                }
            }

            Console.WriteLine($"The number {searchNumber} was not found.");
            goto Finish;

        Found:
            Console.WriteLine($"The number {searchNumber} was found.");

        Finish:
            Console.WriteLine("End of search.");

        }

        private void LoopBreakExamples()
        {
            //The break statement
            //When the break statement is reached, the program immediately exits the loop.
            //A loop can many break statements, but from a design point of view this is to
            //be discouraged because it can make it much harder to discern the flow through the program.
            for (int index = 0; index < names.Length; index++)
            {
                Console.WriteLine(names[index]);
                if (names[index] == "David")
                {
                    Console.WriteLine("Breaking on David.");
                    break;
                }                    
            }

            //The continue statement
            //The continue statement does not cause a loop to end. Instead, it ends the
            //current pass through the code controlled by the loop. The terminating condition
            //is then tested to determine if the loop should continue.
            for (int index = 0; index < names.Length; index++)
            {
                if (names[index] == "David")
                {
                    Console.WriteLine("Continue on David, means skipping rest of loop.");
                    continue;
                }
                Console.WriteLine(names[index]);
            }
        }

        private string[] names = { "Rob", "Mary", "David", "Jenny", "Chris", "Imogen" };

        private void LoopExamples()
        {
            int count = 0;

            while (count < 2)
            {
                Console.WriteLine($"While loop @work, count[{count}]");
                count++;
            }

            do
            {
                Console.WriteLine($"Do while loop @work, count[{count}]");
                count--;
            } while (count > 0);

            //When for loop runs, Initalize() is called once, then Test is called, for loop body is executed, and the Update() is executed.            
            //Initialize called
            //Test called
            //For loop @work 0
            //Update called
            //Test called
            //For loop @work 1
            //Update called
            //Test called
            //This output shows that a test is performed immediately after initialization, so
            //it is possible that the statement controlled by the loop may never be performed.
            //This illustrates a very important aspect of the for loop construction, in that the
            //initialize, test, and update behaviors can be anything that you wish.
            for (Initalize(); Test(); Update())
            {
                Console.WriteLine("For loop @work {0}", counter);
            }

            //You can leave out any of the elements of a for loop.You can also perform
            //multiple statements for the initialize, update, and test elements.The statements
            //are separated by a comma.
            //A simple for loop repeat 3 times is show below 
            for (int counter = 0; counter < 3; counter++)
            {
                Console.WriteLine("Simple For loop @work {0}", counter);
            }

            //Iterate through a collection using for loop
            for (int index = 0; index < names.Length; index++)
            {
                Console.WriteLine(names[index]);
            }

            //The foreach construction makes iterating through a collection much easier.
            //It isn’t possible for code in a foreach construction to modify the iterating value, only possible for reference types.
            foreach (string name in names)
            {
                Console.WriteLine(name);
            }

            //If the foreach loop is working on a list of references to objects, the objects
            //on the ends of those references can be changed. The code below works
            // through a list of Person objects, changing the Name property of each person in
            //the list to upper case.
            Person[] persons =
            {
                new Person("John", "Connor"),
                new Person("James", "Bond"),                
                new Person("Mohamed", "Kalmoua"),
            };
            //Set FirstName to upper
            foreach(Person p in persons)
            {
                p.FirstName = p.FirstName.ToUpper();
            }

            //The foreach construction can iterate through any object which implements
            //the IEnumerable interface. These objects expose a method called
            //GetEnumerator(). This method must return an object that implements the
            //System.Collections.IEnumerator interface. This interface exposes
            //methods that the foreach construction can use to get the next item from the
            //enumerator and determine if there any more items in the collection.Many
            //collection classes, including lists and dictionaries, implement the IEnumerable interface.
            //Note that the iteration can be implemented in a “lazy” way; the next item to be
            //iterated only needs to be fetched when requested.The results of database queries
            //can be returned as objects that implement the IEnumerable interface and then
            //only fetch the actual data items when needed.It is important that the item being
            //iterated is not changed during iteration, if the iterating code tried to remove
            //items from the list it was iterating through this would cause the program to throw
            //an exception when it ran.            
            foreach (Person p in persons)
            {
                Console.WriteLine(p.FirstName +", "+ p.LastName);
            }
        }

        private static int counter = 0;
        static void Initalize()
        {
            Console.WriteLine("Initialize called");
            counter = 0;
        }
        static void Update()
        {
            Console.WriteLine("Update called");
            counter = counter + 1;
        }
        static bool Test()
        {
            Console.WriteLine("Test called");
            return counter < 2;
        }
    }
}
