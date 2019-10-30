using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSnippets
{
    public interface IShoppingCart
    {
        //Note you can implement static methods in the interface, not recommended  

        public void CalculateTotal();
        //Optional method, default behaviour is implemented in interface.  
        public void CalculateTax()
        {
            throw new NotImplementedException("CalculateTax method is not implemented.");
        }
    }
}
