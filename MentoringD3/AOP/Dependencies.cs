using AOP.Demo;
using AOP.Log;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace AOP
{
    public class Dependencies
    {
        private static UnityContainer _container;

        static Dependencies()
        {
            _container = new UnityContainer();
            _container.AddNewExtension<Interception>();
        }

        public static void Register()
        {
            _container.RegisterType<IDemoComponent, DemoComponent>(new Interceptor<InterfaceInterceptor>(), new InterceptionBehavior<LoggingInterception>());
            _container.RegisterType<ILogger, Logger>();
        }

        public static UnityContainer GetContainer()
        {
            return _container;
        }
    }
}
