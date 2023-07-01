using System.Runtime.Versioning;
using Microsoft.Win32;

namespace ATA_SetCPUPriority.Services
{
    [SupportedOSPlatform("windows")]
    public class RegistryService : IRegistryService
    {
        public readonly Dictionary<string, string> PriorityLevels = new()
        {
            {"High", "3"},
            {"Above Normal", "6"},
            {"Normal", "2"},
            {"Below Normal", "5"},
            {"Low", "1"}
        };

        public void InstallContextMenu()
        {
            for (int i = 0; i < PriorityLevels.Count; i++)
            {
                var priority = PriorityLevels.ElementAt(i);

                using RegistryKey key = Registry.ClassesRoot.CreateSubKey($@"exefile\shell\Set CPU Priority\shell\{i + 1}. {priority.Key}");
                key.SetValue("MUIVerb", priority.Key);
                using RegistryKey commandKey = key.CreateSubKey("command");
                commandKey.SetValue("", $@"""{AppDomain.CurrentDomain.BaseDirectory}{AppDomain.CurrentDomain.FriendlyName}"" ""%1"" {priority.Value}");
            }

            using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(@"exefile\shell\Set CPU Priority"))
            {
                key.SetValue("Icon", @"%windir%\System32\imageres.dll,-150");
                key.SetValue("MUIVerb", "Set CPU Priority");
                key.SetValue("SubCommands", "");
            }
        }

        public void SetPriority(string programName, string priority)
        {
            var key = Registry.LocalMachine.CreateSubKey($@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\{programName}.exe\PerfOptions");
            key.SetValue("CpuPriorityClass", priority, RegistryValueKind.DWord);
        }

        public void UninstallContextMenu()
        {
            Registry.ClassesRoot.DeleteSubKeyTree(@"exefile\shell\Set CPU Priority", false);
        }
    }
}
