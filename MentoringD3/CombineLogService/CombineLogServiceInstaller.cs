using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace CombineLogService
{
    [RunInstaller(true)]
    public class CombineLogServiceInstaller : Installer
    {
        public CombineLogServiceInstaller()
        {
            var serviceInstaller = new ServiceInstaller()
            {
                ServiceName = "CombineLogService",
                DisplayName = "CombineLogService",
                DelayedAutoStart = true,
                StartType = ServiceStartMode.Automatic
            };
            this.Installers.Add(serviceInstaller);

            var serviceProcessInstaller = new ServiceProcessInstaller()
            {
                Account = ServiceAccount.LocalService
            };
            this.Installers.Add(serviceProcessInstaller);
        }
    }
}