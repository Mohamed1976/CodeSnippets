using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace _70_483.Multithreading
{
    public static class TasksHelper
    {

        public static Task LoopMethod()
        {
            return Task.Run(() =>
            {
                for (int x = 0; x < 100; x++)
                {
                    Console.Write("*");
                }
            });
        }

        public static Task<int> ReturnsTaskInt()
        {
            return Task<int>.Run(() =>
            {
                return 1976;
            });
        }

        public static async Task<int> ReturnsTaskInt2()
        {
            Task<int> task = Task<int>.Run(() =>
            {
                return 2019;
            }).ContinueWith((i) =>
            {
                return i.Result + 1;
            });

            return await task;
        }
    }
}