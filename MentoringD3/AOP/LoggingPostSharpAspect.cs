using AOP.Log;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System;
using System.Collections.Generic;

namespace AOP
{
    [PSerializable]
    public class LoggingPostSharpAspect : OnMethodBoundaryAspect
    {
        private ILogger _logger;

        public override void OnEntry(MethodExecutionArgs args)
        {
            this._logger = Dependencies.GetContainer().Resolve<ILogger>();
            var key = args.Arguments;
            var parameters = new List<string>();

            foreach (var param in args.Arguments)
            {
                try
                {
                    parameters.Add(JsonConvert.SerializeObject(param));
                }
                catch
                {
                    parameters.Add("Not serializable");
                }
            }

            this._logger.WriteLog($"{DateTime.Now.ToLongTimeString()}: Invoking method '{args.Method.Name}'", parameters);

            base.OnEntry(args);
        }

        public override void OnException(MethodExecutionArgs args)
        {
            this._logger = Dependencies.GetContainer().Resolve<ILogger>();
            this._logger.WriteLog($"{DateTime.Now.ToLongTimeString()}: Method '{args.Method.Name}' threw exception '{args.Exception.Message}'\n" + args.Exception.ToString());
            base.OnException(args);
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            this._logger = Dependencies.GetContainer().Resolve<ILogger>();
            if (args.ReturnValue != null)
            {
                this._logger.WriteLog($"{DateTime.Now.ToLongTimeString()}: Method '{args.Method.Name}' returned '{args.ReturnValue}'");
            }
            else
            {
                this._logger.WriteLog($"{DateTime.Now.ToLongTimeString()}: Method '{args.Method.Name}' invoked without result ");
            }
            base.OnSuccess(args);
        }
    }
}
