using System.Collections.Generic;

namespace Multithreading
{
    public class ThreadInfo
    {
        public int StartIndex { get; set; }

        public int EndIndex { get; set; }

        public IList<int> SourceArray { get; set; }
    }
}
