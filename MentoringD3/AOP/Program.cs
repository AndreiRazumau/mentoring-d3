using AOP.Demo;
using Microsoft.Practices.Unity;
using System;

namespace AOP
{
    class Program
    {
        static void Main(string[] args)
        {
            Dependencies.Register();
            var container = Dependencies.GetContainer();
            var demoComponent = container.Resolve<IDemoComponent>();

            try
            {
                demoComponent.RunWithException(42);
            }
            catch
            {
            }
            var context = new DemoContext("First", DateTime.Now, 2349);
            demoComponent.RunWithoutResult(context);
            context = new DemoContext("Second", DateTime.Now.AddDays(-5), 3456354);
            demoComponent.RunWithResult(context);

            Console.WriteLine("\nRun PostSharpDemoComponent:\n");
            demoComponent = new PostSharpDemoComponent();
            context = new DemoContext("Third", DateTime.Now, 68796);
            demoComponent.RunWithoutResult(context);
            context = new DemoContext("Forth", DateTime.Now.AddDays(3), 3254345);
            demoComponent.RunWithResult(context);
            try
            {
                demoComponent.RunWithException(42);
            }
            catch
            {
            }

            Console.ReadLine();
        }
    }
}
