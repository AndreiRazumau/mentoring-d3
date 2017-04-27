using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Multithreading
{
    public class Program
    {
        static void Main(string[] args)
        {
            Stopwatch watch;
            IList<int> resultArray;

            Console.Write("Please, insert the array size: ");
            var arraySize = int.Parse(Console.ReadLine());
            Console.WriteLine("Choose the process mechanism: 1 - Threads; 2 - Parallel library");
            var mechanism = int.Parse(Console.ReadLine());

            var sourceArray = new List<int>(arraySize);
            var randomizer = new Random(6);

            Console.WriteLine("\nSource array:");
            for (int i = 0; i < arraySize; i++)
            {
                var value = randomizer.Next(0, 100000);
                sourceArray.Add(value);
                Console.Write($"{value} ");
            }

            switch (mechanism)
            {
                case 1:
                    var threadProcessor = new ArrayThreadProcessor();
                    watch = Stopwatch.StartNew();
                    resultArray = threadProcessor.Process(sourceArray);
                    watch.Stop();
                    break;
                case 2:
                    var parallelProcessor = new ArrayParallelProcessor();
                    watch = Stopwatch.StartNew();
                    resultArray = parallelProcessor.Process(sourceArray);
                    watch.Stop();
                    break;
                default:
                    return;
            }

            Console.WriteLine("\n\nResult array:");
            foreach (var item in resultArray)
            {
                Console.Write($"{item} ");
            }

            Console.WriteLine($"\nExecution time: {watch.ElapsedMilliseconds} ms");
            Console.ReadKey();
        }
    }
}
