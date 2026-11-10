using Microsoft.Win32;
using System.Runtime.Versioning;

namespace IdeapadToolkit.Core.Services
{
    [SupportedOSPlatform("windows")]
    public class RunOnStartupService : IRunOnStartupService
    {
        private static string _assemblyPath
        {
            get
            {
                return Environment.ProcessPath ?? string.Empty;
            }
        }
        public bool IsRunOnStartupEnabled()
        {
            bool result;
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run"))
            {
                bool flag = key == null;
                if (flag)
                {
                    result = false;
                }
                else
                {
                    object? o = key!.GetValue("IdeapadToolkit");
                    result = o != null;
                }
            }
            return result;
        }

        public void ToggleRunOnStartup()
        {
            if (!IsRunOnStartupEnabled())
            {
                EnableRunAtStartup();
            }
            else
            {
                DisableRunAtStartup();
            }
        }

        private static void DisableRunAtStartup()
        {
            using RegistryKey? key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            bool flag = key != null;
            if (flag)
            {
                try
                {
                    key!.DeleteValue("IdeapadToolkit");
                }
                catch (ArgumentException)
                {
                }
            }
        }

        private static void EnableRunAtStartup()
        {
            using RegistryKey? key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            key?.SetValue("IdeapadToolkit", _assemblyPath + " nogui");
        }
    }
}
