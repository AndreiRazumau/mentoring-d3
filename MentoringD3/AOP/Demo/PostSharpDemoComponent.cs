using System;

namespace AOP.Demo
{
    public class PostSharpDemoComponent : IDemoComponent
    {
        [LoggingPostSharpAspect]
        public int RunWithResult(DemoContext context)
        {
            context.SetCompleted();
            return 5;
        }

        [LoggingPostSharpAspect]
        public void RunWithoutResult(DemoContext context)
        {
            context.SetCompleted();
        }

        [LoggingPostSharpAspect]
        public void RunWithException(int input)
        {
            throw new NotImplementedException();
        }
    }
}
