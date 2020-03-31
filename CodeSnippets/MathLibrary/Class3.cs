using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLibrary
{

    //You need to ensure that the Method2 method for the Class3 class can be executed only when instances of
    //the class are accessed through the Interface1 interface. The solution must ensure that calls to the Method1
    //method can be made either through the interface or through an instance of the class.
    //Which signature should you use for each method? (To answer, select the appropriate signature for each
    //method in the answer area.)
    class Class3 : Interface1
    {
        public void Method1(decimal amount)
        {
            throw new NotImplementedException();
        }

        void Interface1.Method2(decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
