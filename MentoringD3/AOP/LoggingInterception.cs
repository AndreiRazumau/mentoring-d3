using AOP.Log;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AOP
{
    class LoggingInterception : IInterceptionBehavior
    {
        private ILogger _logger;

        public LoggingInterception()
        {
            this._logger = Dependencies.GetContainer().Resolve<ILogger>();
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            var parameters = new List<string>();
            foreach (var param in input.MethodBase.GetParameters())
            {
                parameters.Add($"Parameter '{param.Name}': ");
            }
            for (int i = 0; i < input.Inputs.Count; i++)
            {
                try
                {
                    parameters[i] += JsonConvert.SerializeObject(input.Inputs[i]);
                }
                catch
                {
                    parameters[i] += "Not serializable";
                }
            }

            this._logger.WriteLog($"{DateTime.Now.ToLongTimeString()}: Invoking method '{input.MethodBase.Name}'", parameters);

            var result = getNext()(input, getNext);

            if (result.Exception != null)
            {
                this._logger.WriteLog($"{DateTime.Now.ToLongTimeString()}: Method '{input.MethodBase.Name}' threw exception '{result.Exception.Message}'\n" + result.Exception.ToString());
            }
            else if (result.ReturnValue != null)
            {
                this._logger.WriteLog($"{DateTime.Now.ToLongTimeString()}: Method '{input.MethodBase.Name}' returned '{result.ReturnValue}'");
            }
            else
            {
                this._logger.WriteLog($"{DateTime.Now.ToLongTimeString()}: Method '{input.MethodBase.Name}' invoked without result ");
            }

            return result;
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public bool WillExecute
        {
            get { return true; }
        }
    }
}
