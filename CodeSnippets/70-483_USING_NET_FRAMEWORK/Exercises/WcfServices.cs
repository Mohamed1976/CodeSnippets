using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace _70_483_USING_NET_FRAMEWORK.Exercises
{
    public class WcfServices
    {
        [ServiceContract]
        public interface ICalculator
        {
            [OperationContract]
            long Add(long x, long y);

            [OperationContract]
            long Subtract(long x, long y);

            [OperationContract]
            long Multiply(long x, long y);

            [OperationContract]
            long Divide(long x, long y);

            [OperationContract]
            long Mod(long x, long y);

            [OperationContract]
            string Echo(string input);
        }

        public class CalculatorService : ICalculator
        {
            public long Add(long x, long y)
            {
                return x + y;
            }

            public long Divide(long x, long y)
            {
                throw new NotImplementedException();
            }

            public long Mod(long x, long y)
            {
                throw new NotImplementedException();
            }

            public long Multiply(long x, long y)
            {
                throw new NotImplementedException();
            }

            public long Subtract(long x, long y)
            {
                throw new NotImplementedException();
            }

            public string Echo(string input)
            {
                return input;
            }
        }
    }
}
