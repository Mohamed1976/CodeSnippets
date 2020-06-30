using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Helpers
{
    public class CustomEncryptor
    {
        public string Encrypt(string plainText)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(plainText);
            string base64 = Convert.ToBase64String(bytes);
            return base64;
        }
    }
}
