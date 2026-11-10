using System.Diagnostics;
using System.Security.Principal;
using System.Runtime.Versioning;

namespace IdeapadToolkit.Core.Services
{
    [SupportedOSPlatform("windows")]
    public class AdministratorPermissionService : IAdministratorPermissionService
    {
        public bool IsAdministrator
        {
            get
            {
                var identity = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        public void RelaunchAsAdmin()
        {
            var proc = new Process
            {
                StartInfo =
                    {FileName = Environment.ProcessPath, UseShellExecute = true, Verb = "runas", Arguments="ignoreRunning"}
            };
            proc.Start();
            Environment.Exit(0);
        }
    }
}
