using System;
using System.Collections.Generic;
using System.Threading;

namespace Multithreading
{
    public class ArrayThreadProcessor : IProcessor
    {
        private List<Thread> _threads;

        public ArrayThreadProcessor()
        {
            this._threads = new List<Thread>();
        }

        public IList<int> Process(IList<int> array)
        {
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {

                this._threads.Add(new Thread(MultiplyValue));
            }

            var index = 0;
            foreach (var thread in this._threads)
            {
                if (index >= array.Count)
                {
                    break;
                }
                var elCount = (int)Math.Ceiling((double)(array.Count - index) / Environment.ProcessorCount);
                var threadInfo = new ThreadInfo
                {
                    StartIndex = index,
                    EndIndex = index + elCount - 1,
                    SourceArray = array
                };
                thread.Start(threadInfo);
                index += elCount;
            }

            foreach (var thread in this._threads)
            {
                if (thread.ThreadState == ThreadState.Running)
                {
                    thread.Join();
                }
            }

            this._threads = null;
            return array;
        }

        private void MultiplyValue(object value)
        {
            var info = (ThreadInfo)value;

            for (int i = info.StartIndex; i <= info.EndIndex; i++)
            {
                info.SourceArray[i] *= 2;
            }
        }
    }
}

