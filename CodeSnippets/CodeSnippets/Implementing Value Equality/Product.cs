using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSnippets.Implementing_Value_Equality
{
    public class Product : IEquatable<Product>
    {
        public string Name { get; set; }
        public int Code { get; set; }

        public bool Equals(Product product)
        {
            if (product is null)
                return false;

            return this.Name == product.Name && this.Code == product.Code;
        }

        public override bool Equals(object obj) => Equals(obj as Product);
        //TODO check which HashCode is returned.
        public override int GetHashCode() => (Name, Code).GetHashCode();
    }
}
