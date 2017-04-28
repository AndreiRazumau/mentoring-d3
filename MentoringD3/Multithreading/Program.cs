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

            Console.Write("Please, insert the array size: ");
            var arraySize = int.Parse(Console.ReadLine());
            Console.WriteLine("Choose the process mechanism: 1 - Threads; 2 - Parallel library");
            var mechanism = int.Parse(Console.ReadLine());

            var sourceArray = new List<int>(arraySize);
            var randomizer = new Random();

            Console.WriteLine("\nSource array:");
            for (int i = 0; i < arraySize; i++)
            {
                var value = randomizer.Next(0, 10000000);
                sourceArray.Add(value);
            }

            IProcessor processor;

            switch (mechanism)
            {
                case 1:
                    Console.WriteLine("\nUse Threads:");
                    processor = new ArrayThreadProcessor();

                    break;
                case 2:
                    Console.WriteLine("\nUse TPL:");
                    processor = new ArrayParallelProcessor();
                    break;
                default:
                    return;
            }

            watch = Stopwatch.StartNew();
            var resultArray = processor.Process(sourceArray);
            watch.Stop();

            Console.WriteLine("Result array:");
            foreach (var item in resultArray)
            {
                //Console.Write($"{item} ");
            }

            Console.WriteLine($"\nExecution time: {watch.ElapsedMilliseconds} ms");
            Console.ReadKey();
        }
    }
}
