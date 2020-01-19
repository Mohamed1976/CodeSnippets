using System;
using System.Collections.Generic;
using System.Text;

namespace _70_483.EventsAndCallbacks
{
    class Alarm
    {
        // Delegate for the alarm event
        public Action OnAlarmRaised { get; set; }

        //C# provides an event construction that allows a delegate to be specified as
        //an event. This is shown in Listing 1-67. The keyword event is added before the
        //definition of the delegate. The member OnAlarmRaised is now created as a
        //data field in the Alarm class, rather than a property.OnAlarmRaised no
        //longer has get or set behaviors.However, it is now not possible for code
        //external to the Alarm class to assign values to OnAlarmRaised, and the
        //OnAlarmRaised delegate can only be called from within the class where it is
        //declared.In other words, adding the event keyword turns a delegate into a
        //properly useful event. The code below has one other improvement over previous
        //versions.It creates a delegate instance and assigns it when OnAlarmRaised is
        //created, so there is now no need to check whether or not the delegate has a value
        //before calling it. This simplifies the RaiseAlarm method.
        public event Action OnAlarmRaisedV2 = delegate { };

        //Create events with built-in delegate type EventHandler
        //Programs that use events should use the
        //EventHandler class instead of Action.This is because the
        //EventHandler class is the part of.NET designed to allow subscribers to be
        //given data about an event. EventHandler is used throughout the .NET
        //framework to manage events. An EventHandler can deliver data, or it can
        //just signal that an event has taken place.
        //public delegate void EventHandler([NullableAttribute(2)] object? sender, EventArgs e);
        public event EventHandler OnAlarmRaisedV3 = delegate { };

        //C# provides an event construction that allows a delegate to be specified as
        //an event. This is below. The keyword event is added before the
        //definition of the delegate. The member OnAlarmRaised is now created as a
        //data field in the Alarm class, rather than a property.OnAlarmRaised no
        //longer has get or set behaviors.However, it is now not possible for code
        //external to the Alarm class to assign values to OnAlarmRaised, and the
        //OnAlarmRaised delegate can only be called from within the class where it is
        //declared.In other words, adding the event keyword turns a delegate into a
        //properly useful event.
        //The code below has one other improvement over previous
        //versions.It creates a delegate instance and assigns it when OnAlarmRaised is
        //created, so there is now no need to check whether or not the delegate has a value
        //before calling it. This simplifies the RaiseAlarm method.
        //public delegate void EventHandler<[NullableAttribute(2)] TEventArgs>([NullableAttribute(2)] object? sender, TEventArgs e);
        public event EventHandler<AlarmEventArgs> OnAlarmRaisedV4 = delegate { };

        // Called to raise an alarm
        //Delegates added to a published event are called on the same thread as the
        //thread publishing the event. If a delegate blocks this thread, the entire
        //publication mechanism is blocked.This means that a malicious or badly written
        //subscriber has the ability to block the publication of events. This is addressed by
        //the publisher starting an individual task to run each of the event subscribers.
        public void RaiseAlarm()
        {
            //Invoke callbacks using GetInvocationList()  
            //foreach (Delegate del in OnAlarmRaised.GetInvocationList())
            //{
            //    del.DynamicInvoke();
            //}

            // Only raise the alarm if someone has, subscribed.
            if (OnAlarmRaised != null)
            {
                OnAlarmRaised();
            }

            OnAlarmRaisedV2();

            // Raises the alarm. The event handler receivers a reference to the alarm that is raising this event
            OnAlarmRaisedV3(this, EventArgs.Empty);
            
            try
            {
                OnAlarmRaisedV4(this, new AlarmEventArgs("Europe", DateTime.Now));
            }
            catch(AggregateException agg)
            {
                foreach (Exception ex in agg.InnerExceptions)
                    Console.WriteLine($"Exception: {ex.Message}");
            }

            //The null conditional operator “.?” means, “only access this member of the
            //class if the reference is not null.” A delegate exposes an Invoke method to
            //invoke the methods bound to the delegate.
            //Alternatively you can use 
            //OnAlarmRaised?.Invoke();
        }

        // Example below
        // You have a private method in your class and you want to make invocation of the method possible by certain callers. What do you do?

        public delegate void delegateDisplayUserCredentials(string userName);

        public delegateDisplayUserCredentials AuthorizeRequest(string password)
        {
            if(password == "Welcome123")
            {
                return new delegateDisplayUserCredentials(DisplayUserCredentials);
            }
            else
            {
                return null;
            }            
        }

        private void DisplayUserCredentials(string userName)
        {
            Console.WriteLine($"DisplayUserCredentials: {userName}");
        }
    }
}
