using _70_483_USING_NET_FRAMEWORK.Exercises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _70_483_USING_NET_FRAMEWORK
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
            
            try
            {
                RandomStringExamples randomString = new RandomStringExamples();
                randomString.Run().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }

            Console.ReadLine();
            return;

            try
            {
                DataBaseExamples dataBaseExamples = new DataBaseExamples();
                dataBaseExamples.Run();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                //throw;
            }

            try
            {
                WcfExamples wcfExamples = new WcfExamples();
                wcfExamples.Run();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                //throw;
            }

            Console.ReadLine();
            return;

            #region [ Linq Exercises ]

            try
            {
                LinqExercises linqExercises = new LinqExercises();
                //linqExercises.Run();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured in LinqExercises.Run: {0}", ex.Message);
                throw;
            }
            #endregion

            #region [ General Exercises ]

            try
            {
                GeneralExercises generalExercises = new GeneralExercises();
                generalExercises.Run().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured in GeneralExercises.Run: {0}", ex.Message);
            }

            #endregion

            #region [ PerformanceCounterType AverageBase Example ]

            try
            {
                //PerformanceCounterTypeAverageBaseExample performanceCounterTypeAverageBaseExample = new
                //    PerformanceCounterTypeAverageBaseExample();
                //performanceCounterTypeAverageBaseExample.Run();

                //PerformanceCounterTypeAverageBaseExample2 performanceCounterTypeAverageBaseExample2 =
                //    new PerformanceCounterTypeAverageBaseExample2();
                //performanceCounterTypeAverageBaseExample2.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured in PerformanceCounterTypeAverageBaseExample.Run: {0}", ex.Message);
            }
            #endregion

            #region [ Performancecounter Exercises ]

            try
            {
                PerformancecounterExercises performancecounterExercises = new PerformancecounterExercises();
                //performancecounterExercises.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured in PerformancecounterExercises.Run: {0}", ex.Message);
            }
        
            #endregion

            #region [ Exam Notes 002 ]

            try
            {
                ExamNotes002 examNotes002 = new ExamNotes002();
                //examNotes002.Run();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured in ExamNotes002.Run: {0}", ex.Message);
            }

            #endregion

            #region [ Diagnostics ]

            try
            {
                Diagnostics diagnostics = new Diagnostics();
                //diagnostics.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured in Diagnostics.Run: {0}", ex.Message);
            }

            #endregion

            #region [ Exam Notes 001 ]

            try
            {
                ExamNotes001 examNotes001 = new ExamNotes001();
                //examNotes001.Run().Wait();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured in ExamNotes001.Run: {0}", ex.Message);
            }
            #endregion

            #region [ Serialization Exercises ]

            try
            {
                SerializationExercises serializationExercises = new SerializationExercises();
                //serializationExercises.Run().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured in SerializationExercises.Run: {0}", ex.Message);
            }

            #endregion

            Console.ReadLine();
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("An Unhandled Exception occurred: {0}", e.ExceptionObject.ToString());
        }
    }
}
