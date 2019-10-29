using System;
using System.Collections.Generic;
using System.Text;

namespace DelegateExamples
{
    public class TestObject
    {
        public TestObject()
        {
        }

        private DateTime? startDate;

        public DateTime? StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        private DateTime? endDate;

        public DateTime? EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        private Func<TestObject, bool> DateValidater = (TestObject testObject) =>
        {
            bool isValid = false;

            if(testObject.StartDate.HasValue && 
            testObject.EndDate.HasValue &&
            testObject.EndDate > testObject.StartDate)
            {
                isValid = true;
            }

            return isValid;
        };

        public void ProcessData()
        {
            if(DateValidater(this))
            {
                Console.WriteLine("TestObject has VALID date fields."); 
            }
            else
            {
                Console.WriteLine("TestObject has INVALID date fields.");
            }
        }

    }
}
