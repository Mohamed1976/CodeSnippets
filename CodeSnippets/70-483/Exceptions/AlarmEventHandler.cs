using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace _70_483.Exceptions
{
    class AlarmEventHandler
    {
        public event EventHandler<AlarmEventArgs> OnAlarmRaised;


        public void RaiseAlarm()
        {
            List<Exception> exceptionList = new List<Exception>();

            //Each event handler can be called individually and then a
            //single aggregate exception created which contains all the details of any
            //exceptions that were thrown by event handlers.
            //The GetInvocationList method is used on the delegate to obtain a
            //list of subscribers to the event. This list is then iterated and the
            //DynamicInvoke method called for each subscriber. Any exceptions thrown
            //by subscribers are caught and added to a list of exceptions. Note that the
            //exception thrown by the subscriber is delivered by a
            //TypeInvocationException, and it is the inner exception from this that must be saved.
            foreach (Delegate handler in OnAlarmRaised.GetInvocationList())
            {
                try
                {
                    handler.DynamicInvoke(this, new AlarmEventArgs("Message from AlarmEventHandler class."));
                }
                catch (TargetInvocationException e)
                {
                    exceptionList.Add(e.InnerException);
                }
            }

            if (exceptionList.Count > 0)
                throw new AggregateException(exceptionList);
        }
    }
}
