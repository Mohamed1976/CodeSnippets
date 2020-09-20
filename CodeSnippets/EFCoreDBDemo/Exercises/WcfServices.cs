using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace EFCoreDBDemo.Exercises
{
    public class WcfServices
    {
        static readonly string ServiceBaseAddress = "http://" + Environment.MachineName + ":8000/Service";

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
        }
    }
}
