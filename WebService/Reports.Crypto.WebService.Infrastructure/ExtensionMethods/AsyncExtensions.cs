using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reports.Crypto.WebService.Infrastructure.ExtensionMethods
{
    public static class AsyncExtensions
    {
        // Nesting await in Parallel.ForEach: https://stackoverflow.com/a/25877042/1961386
        public static Task ForEachAsync<T>(this IEnumerable<T> source, int dop, Func<T, Task> body) 
        { 
            return Task.WhenAll( 
                from partition in Partitioner.Create(source).GetPartitions(dop) 
                select Task.Run(async delegate { 
                    using (partition) 
                        while (partition.MoveNext()) 
                            await body(partition.Current).ContinueWith(t => 
                            {
                                //observe exceptions
                            });
                })); 
        }
    }
}