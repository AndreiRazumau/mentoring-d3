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

            if (array.Count < Environment.ProcessorCount)
            {
                for (var i = 0; i < array.Count; ++i)
                {
                    var threadInfo = new ThreadInfo
                    {
                        StartIndex = i,
                        EndIndex = i,
                        SourceArray = array
                    };
                    this._threads[i].Start(threadInfo);
                }
            }
            else
            {
                var chunkSize = array.Count / this._threads.Count; // 33 chunk_size
                var chunkCount = array.Count / chunkSize;  // 3 chunk_count
                var reminder = array.Count % chunkSize; // 1 reminder;

                var thread_info_list = new List<ThreadInfo>();
                var threads = this._threads;

                for (var i = 0; i < chunkCount - 1; ++i)
                {
                    thread_info_list.Add(new ThreadInfo
                    {
                        StartIndex = i * chunkSize,
                        EndIndex = i * chunkSize + chunkSize,
                        SourceArray = array
                    });
                }
                thread_info_list.Add(new ThreadInfo
                {
                    StartIndex = (chunkCount - 1) * chunkSize,
                    EndIndex = (chunkCount - 1) * chunkSize + chunkSize + reminder,
                    SourceArray = array
                });

                for (int i = 0; i < this._threads.Count; i++)
                {
                    this._threads[i].Start(thread_info_list[i]);
                }
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

            for (int i = info.StartIndex; i < info.EndIndex; i++)
            {
                info.SourceArray[i] *= 2;
            }
        }
    }
}

