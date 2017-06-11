using System;

namespace AOP.Demo
{
    public class DemoComponent : IDemoComponent
    {
        public int RunWithResult(DemoContext context)
        {
            context.SetCompleted();
            return 5;
        }

        public void RunWithoutResult(DemoContext context)
        {
            context.SetCompleted();
        }

        public void RunWithException(int input)
        {
            throw new NotImplementedException();
        }
    }
}
