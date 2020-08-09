using System;
using System.Collections.Generic;
using System.Text;
//install System.Runtime.Caching package
using System.Runtime.Caching;
using System.IO;
using System.Threading;
using System.Reflection.Metadata.Ecma335;


//https://docs.microsoft.com/en-us/dotnet/api/system.runtime.caching.memorycache?view=dotnet-plat-ext-3.1
//https://docs.microsoft.com/en-us/dotnet/api/system.runtime.caching.changemonitor?view=dotnet-plat-ext-3.1
//https://www.c-sharpcorner.com/UploadFile/87b416/working-with-caching-in-C-Sharp/
namespace EFCoreDBDemo.Exercises
{
    public class CacheExamples
    {        
        public void Run()
        {
            return;
            int count = 0;
            IEnumerable<string> stocks = default;

            for (; ; )
            {
                stocks = GetAvailableStocks(++count % 3 == 0);

                foreach (string stock in stocks)
                {
                    Console.WriteLine($"{DateTime.Now.ToString()} {stock}");
                }
                Thread.Sleep(10000);
            }

            for (; ; )
            {
                FileCache();
                Thread.Sleep(10000);
            }
        }

        private void FileCache()
        {
            const string file = @"C:\temp\Readme.txt";

            //To put data into the ObjectCache, it first needs to be created. The current constructor is
            //protected, so it isn’t instantiated using the new keyword.Instead, you use the MemoryCache.
            //Default property to get an instance.
            //Conveniently, the ObjectCache class alone provides nearly all the functionality for caching.
            ObjectCache cache = MemoryCache.Default;
            //After you create an instance of the cache and put items in it, you can verify the 
            //existence of those items by using the default indexer on the ObjectCache and the 
            //string value you chose as a key whenever you added the item. Assume, for instance, 
            //that you added a string value that was the PublicKeyValue of an encryption key.
            //The following code tells you whether the item was in the ObjectCache 
            //(using the as keyword enables you to attempt the reference without throwing an 
            //exception if the item is not present). You don’t have to use this technique specifically, 
            //but you will want to verify that something exists in the cache before trying to reference 
            //it just like as would with any other collection.
            string fileContents = cache["filecontents"] as string;
            if (fileContents == null)
            {
                Console.WriteLine($"{DateTime.Now.ToString()} ***cache is refreshed***.");

                CacheItemPolicy policy = new CacheItemPolicy();
                List<string> filePaths = new List<string>();
                filePaths.Add(file);

                /* In addition to understanding the expiration policy, you need to be 
                 * aware of the ChangeMonitor class, which the CacheItemPolicy has a collection of. 
                 * If you refer to my example about the hospital intake application, it’s quite obvious 
                 * that, if the underlying data source changes, the cache needs to know about it. 
                 * If it doesn’t, there will be latency and a potential for very serious problems in many applications.
                 * Although you don’t use the ChangeMonitor class directly, it’s a base class that has 
                 * several derivations (and other ones can certainly be added):
                 * 
                 * ■ CacheEntryChangeMonitor
                 * ■ FileChangeMonitor
                 * ■ HostFileChangeMonitor
                 * ■ SqlChangeMonitor
                 * 
                 * The FileChangeMonitor does exactly what its name implies: It monitors a specific 
                 * file to see whether any changes are made to it, and, if so, the changes will be 
                 * reflected in the cache, provided the CacheItemPolicy dictates it.
                 * 
                 * The HostFileChangeMonitor monitors directories and file paths for changes to them. 
                 * If it detects changes, it responds accordingly.
                 * 
                 * A dependency change is a change in the state of a dependency. 
                 * In a typical cache implementation, after a ChangeMonitor instance 
                 * notifies the cache that a dependency has changed, the cache performs 
                 * the required action, such as invalidating the inserted cache entry. */
                policy.ChangeMonitors.Add(new HostFileChangeMonitor(filePaths));

                // Fetch the file contents.  
                fileContents = File.ReadAllText(file);

                cache.Set("filecontents", fileContents, policy);
            }

            Console.WriteLine($"{DateTime.Now.ToString()} {fileContents}");
        }

        private const string CacheKey = "availableStocks";
        public IEnumerable<string> GetAvailableStocks(bool refresh = false)
        {
            ObjectCache cache = MemoryCache.Default;

            if(refresh)
            {
                //If the item isn’t in cache, this gracefully fails and continues;
                //you can never be guaranteed that an item is in a cache at any moment 
                //in time, even if you just checked for it 3 ms ago.
                cache.Remove(CacheKey);
            }

            if (cache.Contains(CacheKey))
            {
                return (IEnumerable<string>)cache.Get(CacheKey);
            }
            else
            {
                Console.WriteLine($"{DateTime.Now.ToString()} ***cache is refreshed***.");
                IEnumerable<string> availableStocks = this.GetDefaultStocks();
                // Store data in the cache    
                CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
                //set an expiration policy by using either an AbsoluteExpiration or a SlidingExpiration.
                //The difference between the two is apparent to most. With the AbsoluteExpiration, 
                //the CacheItem is purged after a specified amount of time. With the SlidingExpiration, 
                //it is purged only if it has not been accessed after a specified amount of time. 
                //Using either mechanism is simply a matter of determining which one you want to use 
                //and picking an interval to use in conjunction with it.
                //cacheItemPolicy.SlidingExpiration = new TimeSpan(0, 0, 2)
                cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddHours(1.0);

                //If no value is specified for the CacheItemPriority, the default value of Default is used. A
                //value of Default means that there is No Priority. If you’re thinking that the only two options seem to be to set no priority at all or set the
                //priority in a way that ensures it’s never automatically removed, you’re correct.
                //CacheItemPriority has no bearing on whether something can be removed manually; 
                //it simply prevents system optimization operations for purging it when trying to 
                //free up resources.If it stopped such removals from happening, anything put into 
                //cache with a CacheItemPriority would exist in cache for the life of the session, 
                //no matter what else happened.If you do not watch the items you marked as NotRemovable, 
                //you could easily find yourself in a situation in which the application’s resources are 
                //completely consumed and it has no means by which to free up enough of them to keep 
                //functioning correctly.                
                cacheItemPolicy.Priority = CacheItemPriority.Default;
                cache.Add(CacheKey, availableStocks, cacheItemPolicy);
                return availableStocks;
            }
        }

        public IEnumerable<string> GetDefaultStocks()
        {
            return new List<string>() { "Pen", "Pencil", "Eraser" } as IEnumerable<string>;
        }
    }
}
