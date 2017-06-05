using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multithreading
{
    public class ArrayParallelProcessor : IProcessor
    {
        public IList<int> Process(IList<int> array)
        {
            Parallel.For(0, array.Count, i => array[i] *= 2);
            return array;
        }
    }
}
