using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSnippets
{
    class ShoppingCart : IShoppingCart
    {
        public void CalculateTotal()
        {
            Console.WriteLine("CalculateTotal() from ShoppingCart : IShoppingCart");
        }
    }
}
