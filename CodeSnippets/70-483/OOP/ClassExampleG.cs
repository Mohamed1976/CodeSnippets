using System;
using System.Collections.Generic;
using System.Text;

namespace _70_483.OOP
{
    public abstract class ClassExampleG
    {
        public abstract void MethodA();
        protected abstract void MethodB();
        public void MethodC()
        {
            Console.WriteLine("abstract class ClassExampleG, public void MethodC()");
        }
    }

    public class ClassExampleH : ClassExampleG
    {
        public ClassExampleH() : base()
        {

        }

        public override void MethodA()
        {
            Console.WriteLine("public class ClassExampleH : ClassExampleG, public override void MethodA()");
        }

        protected override void MethodB()
        {
            Console.WriteLine("public class ClassExampleH : ClassExampleG, public override void MethodB()");
        }
    }
}
