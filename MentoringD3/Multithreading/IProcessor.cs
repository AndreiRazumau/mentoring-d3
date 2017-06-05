using System.Collections.Generic;

namespace Multithreading
{
    public interface IProcessor
    {
        IList<int> Process(IList<int> array);
    }
}
