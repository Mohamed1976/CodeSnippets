using System;
using System.Collections.Generic;
using System.Text;

namespace _70_483.OOP
{
    public class ClassExampleE
    {
        public virtual void DisplayInfo()
        {
            Console.WriteLine("public virtual void DisplayInfo() from class ClassExampleE.");
        }
    }

    public class ClassExampleF : ClassExampleE
    {
        public override void DisplayInfo()
        {
            Console.WriteLine("public override void DisplayInfo() from class ClassExampleF.");
        }
    }
}
