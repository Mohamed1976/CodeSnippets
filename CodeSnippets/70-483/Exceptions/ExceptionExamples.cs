using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _70_483.Exceptions
{
    class ExceptionExamples
    {
        //You should make sure that your code deals with any exceptions that may be
        //thrown by methods, propagating the exception onward if necessary to ensure that
        //exception events are not hidden from different parts of your application.
        //Your methods should also throw exceptions in situations where it is not
        //meaningful to continue with an action.If a method throws an exception the
        //caller must deal with that exception if the program is to continue running.
        //However, if a method returns an error code when something goes wrong, this
        //error code could be ignored by the caller.
        //It is important that you consider how to manage error conditions during the
        //design of an application.It is very hard to add error management to an
        //application once it has been created.


        //Exceptions are, as their name implies, errors that occur in exceptional circumstances.
        //I don’t consider something like an invalid user input as a situation that should be managed by 
        //the use of exceptions.User error is to be expected and dealt with in the normal running of a solution.
        //A program should use an exception to manage an error if it is not meaningful
        //for the program to continue execution at the point the error occurs. For example
        //if a network connection fails or a storage device becomes full. Handing an exception may involve such
        //actions as alerting the user(if any), creating a log entry, releasing resources, and
        //perhaps even shutting down the application in a well-managed way.
        //If an exception is not caught by an exception handler within the program it
        //will be caught by the.NET environment and will cause the thread or task to
        //terminate.Uncaught exceptions may cause threads and tasks to fail silently with
        //no message to the user. An uncaught exception thrown in the foreground thread
        //of an application will cause the application to terminate. Exceptions can be
        //nested.When an exception is thrown, the.NET runtime searches up the call
        //stack to find the “closest” exception handler to deal with the exception.This is a
        //comparatively time-consuming process, which means that exceptions should not
        //be used for “run of the mill” errors, but only invoked in exceptional circumstances.
        public async Task Run()
        {
            //ExceptionExample1();
            //ExceptionExample2();
            //ExceptionExample3();
            try
            {
               // ExceptionExample4();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ExceptionExample4 after re throwing exception.");
                Console.WriteLine("\tMessage: " + ex.Message);
                Console.WriteLine("\tStacktrace: " + ex.StackTrace);
            }

            try
            {
                //ExceptionExample5();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ExceptionExample5 after re throwing exception.");
                Console.WriteLine("\tMessage: " + ex.Message);
                Console.WriteLine("\tStacktrace: " + ex.StackTrace);
                Console.WriteLine("\tInnerException Message: " + ex.InnerException.Message);
                Console.WriteLine("\tInnerException Stacktrace: " + ex.InnerException.StackTrace);
            }

            //ExceptionExample6();

            try
            {
                //ExceptionExample7();
            }
            catch (CalculateException cex)
            {
                Console.WriteLine("ExceptionExample7 passed exception using when.");
                Console.WriteLine("\tMessage: " + cex.Message);
                Console.WriteLine("\tStacktrace: " + cex.StackTrace);
                Console.WriteLine("\tError: " + cex.Error);
            }

            //https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/exception-handling-task-parallel-library
            //await ExceptionExample8("https://www.microsoft.com");
            //await ExceptionExample8("invalid uri");

            //Handling exceptions in eventhandlers. A number of programs can subscribe to an event. 
            //They do this by binding a delegate to the event. The delegate serves as a
            //reference to a piece of C# code which the subscriber wants to run when the event
            //occurs. This piece of code is called an event handler.
            //But what happens if one of the event handlers fails by
            //throwing an exception? If code in one of the subscribers throws an uncaught
            //exception the exception handling process ends at that point and no further
            //subscribers will be notified.This would mean that some subscribers would not be informed of the event
            //Exceptions are catched in the RaiseAlarm() and aggregated to an AggregateException 
            AlarmEventHandler alarmEventHandler = new AlarmEventHandler();
            alarmEventHandler.OnAlarmRaised += AlarmListener1;
            alarmEventHandler.OnAlarmRaised += AlarmListener2;
            //Raising the alarm event and catching the AggregateException that is generated RaiseAlarm()   
            try
            {
                //alarmEventHandler.RaiseAlarm();
            }
            catch (AggregateException agg)
            {
                Console.WriteLine($"AggregateException after calling alarmEventHandler.RaiseAlarm(), type[{agg.GetType().Name}].");
                foreach (Exception ex in agg.InnerExceptions)
                {
                    Console.WriteLine("\tMessage: " + ex.Message);
                    Console.WriteLine("\tStacktrace: " + ex.StackTrace);
                }
            }
        }

        private void AlarmListener1(object source, AlarmEventArgs args)
        {
            Console.WriteLine("Alarm listener 1 called");
            Console.WriteLine("Alarm in {0}", args.MyMessage);
            throw new Exception("Bang");
        }
        private void AlarmListener2(object source, AlarmEventArgs args)
        {
            Console.WriteLine("Alarm listener 2 called");
            Console.WriteLine("Alarm in {0}", args.MyMessage);
            throw new Exception("Boom");
        }


        private void ExceptionExample1()
        {
            try
            {
                throw new Exception("Generated exception.");
            }
            //The program does not use the exception object that is produced when the exception is thrown.
            catch
            {
                Console.WriteLine("Something wrong happend in ExceptionExample1.");
            }
        }

        private void ExceptionExample2()
        {
            try
            {
                string numberText = "A";
                int result;
                result = int.Parse(numberText);
                Console.WriteLine("You entered {0}", result);
            }
            //Method uses the Message and StackTrace properties of the exception to generate an error message.
            //The first line of the output gives the error message and the StackTrace gives the position in the program at which
            //the error occurred. The HelpLink property can be set to give further
            //information about the exception.The TargetSite property gives the name of
            //the method that causes the exception, and the Source property gives the name
            //of the application that caused the error, or the name of the assembly if the
            //application name has not been set.
            catch (Exception ex)
            {
                Console.WriteLine("Message: " + ex.Message);
                Console.WriteLine("Stacktrace: " + ex.StackTrace);
                Console.WriteLine("HelpLink: " + ex.HelpLink);
                Console.WriteLine("TargetSite: " + ex.TargetSite);
                Console.WriteLine("Source:" + ex.Source);
            }
        }
        private void ExceptionExample3()
        {
            try
            {
                string numberText = "0";
                int result = int.Parse(numberText);
                Console.WriteLine("You entered {0}", result);
                //Note that not all arithmetic errors will throw an exception at this point in the
                //code; if the same division is performed using the floating point or double
                //precision type, the result will be evaluated as “infinity.”
                int sum = 1 / result;
                Console.WriteLine("Sum is {0}", sum);
            }
            //The order of the catch elements is important.If the first catch element caught the Exception type the
            //compiler would produce the error “A previous catch clause already catches all
            //exceptions of this or of a super type(‘Exception’)”. You must put the most
            //abstract exception type last in the sequence.
            catch (NotFiniteNumberException nx)
            {
                Console.WriteLine($"Invalid number: {nx.Message}");
            }
            catch (DivideByZeroException zx)
            {
                Console.WriteLine($"Divide by zero: {zx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected exception: {ex.Message}");
            }
            //The try construction can contain a finally element that identifies code that
            //will be executed irrespective of whatever happens in the try construction.
            //Displays the thank you message, whether or not any exceptions are thrown when the program runs.
            //Note that code in this block is guaranteed to run irrespective of what happens
            //during the try construction.This includes situations where the code in the
            //construction returns to a calling method or code in the exception handlers cause
            //other exceptions to be thrown.The finally block is where a program can
            //release any resources that it may be using.
            //The only situation in which a finally block will not be executed are:
            //1) If preceding code (in either the try block or an exception handler) enters an infinite loop.
            //2) If the programmer uses the Environment.FailFast method in the code protected by the try construction 
            //to explicitly request that any finally elements are ignored.
            finally
            {
                Console.WriteLine("Thanks for using my program.");
            }
        }

    //One of the fundamental principles of exception design is that catching an
    //exception should not lead to errors being hidden from other parts of an
    //application.Sometimes an exception will be caught that needs to be “passed up”
    //to an enclosing exception handler.This might be because the low-level handler
    //doesn’t recognize the exception, or it might be because a handler at a higher
    //level must also be alerted to the occurring exception.An exception can be rethrown
    //by using the keyword throw with no parameter: throw;
        private void ExceptionExample4()
        {
            try
            {
                throw new Exception("Generated exception in ExceptionExample4.");
            }
            catch(Exception ex)
            {
                Console.WriteLine("ExceptionExample4 before re throwing exception.");
                Console.WriteLine("\tMessage: " + ex.Message);
                Console.WriteLine("\tStacktrace: " + ex.StackTrace);
                throw;
                //You might think that when re-throwing an exception, you should give the
                //exception object to be re-thrown, as shown below. 
                //This is bad practice because it will remove the stack trace information that is
                //part of the original exception and replace it with stack trace information that
                //describes the position reached in the exception handler code. This will make it
                //harder to work out what is going on when the error occurs, because the location
                //of the error will be reported as being in your handler, rather than at the point at
                //which the original exception was generated.

                //throw ex; // this will not preserve the original stack
            }
        }

        private void ExceptionExample5()
        {
            try
            {
                throw new Exception("Generated exception in ExceptionExample5.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ExceptionExample5 before re throwing exception.");
                Console.WriteLine("\tMessage: " + ex.Message);
                Console.WriteLine("\tStacktrace: " + ex.StackTrace);
                throw new Exception("Exception added in the catch, re throwing.", ex);
            }
        }

        private void ExceptionExample6()
        {
            try
            {
                throw new CalculateException("Calculation failed", CalculateException.CalcErrorCodes.DivideByZero);
            }
            catch (CalculateException cex)
            {
                Console.WriteLine("ExceptionExample6 custom exception.");
                Console.WriteLine("\tMessage: " + cex.Message);
                Console.WriteLine("\tStacktrace: " + cex.StackTrace);
                Console.WriteLine("\tError: " + cex.Error);
            }
        }

        private void ExceptionExample7()
        {
            try
            {
                throw new CalculateException("Calculation failed", CalculateException.CalcErrorCodes.DivideByZero);
            }
            //An exception handler can re - throw an exception if it is not in a position to deal
            //with the exception. An alternative to re-throwing an exception is to create a handler that
            //only catches exceptions that contain particular data values.
            //The when keyword is followed by a conditional clause that performs a test on the exception object.
            //The exception handler will only trigger in the event of an exception being
            //thrown that has an Error property set to InvalidNumberText. An exception with
            //any other error code is ignored, and will be handled in the calling method.   
            catch (CalculateException cex) when (cex.Error == CalculateException.CalcErrorCodes.InvalidNumberText) 
            {
                Console.WriteLine("ExceptionExample7 will not be called because it is not InvalidNumberText.");
            }
        }

        private async Task ExceptionExample8(string uri)
        {
            try
            {
                var content = await FetchWebPage(uri);
                Console.WriteLine($"FetchWebPage: {uri}, length: {content.Length}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ExceptionExample8 task throwing exception, exception type: {ex.GetType()}");
                Console.WriteLine("\tMessage: " + ex.Message);
                Console.WriteLine("\tStacktrace: " + ex.StackTrace);
            }
        }

        async static Task<string> FetchWebPage(string uri)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(uri).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return content;
        }
    }
}