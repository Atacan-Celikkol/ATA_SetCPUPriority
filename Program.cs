using Microsoft.Win32;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace ATA_SetCPUPriority;

[SupportedOSPlatform("windows")]
class Program
{
    static void Main(string[] args)
    {
        MessageBox(0, "Context menu uninstalled successfully!", "Success!", 0);

        if (args.Length == 0)
        {
            InstallContextMenu();
            CreateUninstallShortcut();
        }
        else if (args.Length == 2)
        {
            string filePath = args[0];
            string priority = args[1];
            SetPriority(filePath, priority);
        }
        else if (args.Length == 1 && args[0].ToLower() == "uninstall")
        {
            UninstallContextMenu();
        }
    }

    private static void InstallContextMenu()
    {
        string[] priorityLevels = { "High", "Above Normal", "Normal", "Below Normal", "Low" };
        string[] priorityValues = { "3", "6", "2", "5", "1" };

        using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(@"exefile\shell\Set CPU Priority"))
        {
            key.SetValue("Icon", @"C:\Windows\System32\imageres.dll,-150");
            key.SetValue("MUIVerb", "Set CPU Priority");
            key.SetValue("SubCommands", "");
        }

        for (int i = 0; i < priorityLevels.Length; i++)
        {
            using (RegistryKey key = Registry.ClassesRoot.CreateSubKey($@"exefile\shell\Set CPU Priority\shell\{i + 1}. {priorityLevels[i]}"))
            {
                key.SetValue("MUIVerb", priorityLevels[i]);
                using (RegistryKey commandKey = key.CreateSubKey("command"))
                {
                    commandKey.SetValue("", $@"""{AppDomain.CurrentDomain.BaseDirectory}{AppDomain.CurrentDomain.FriendlyName}"" ""%1"" {priorityValues[i]}");
                }
            }
        }
    }

    static void CreateUninstallShortcut()
    {
        string shortcutPath = Path.Combine(Environment.CurrentDirectory, "Uninstall.lnk");

        if (!File.Exists(shortcutPath))
        {
            var shell = new IWshRuntimeLibrary.WshShell();

            var shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutPath);

            shortcut.TargetPath = Assembly.GetExecutingAssembly().Location.Replace(".dll", ".exe");
            shortcut.Arguments = "uninstall";
            shortcut.Description = "Uninstall ATA_SetCPUPriority";

            shortcut.Save();

            Console.WriteLine("Uninstall shortcut created.");
        }
    }

    static void SetPriority(string filePath, string priority)
    {
        var programName = Path.GetFileNameWithoutExtension(filePath);
        var key = Registry.LocalMachine.CreateSubKey($@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\{programName}.exe\PerfOptions");
        key.SetValue("CpuPriorityClass", priority, RegistryValueKind.DWord);
        Console.WriteLine($"Priority of {programName} set to {priority}.");
    }

    static void UninstallContextMenu()
    {
        Registry.ClassesRoot.DeleteSubKeyTree(@"exefile\shell\Set CPU Priority", false);
        MessageBox(0, "Context menu uninstalled successfully!", "Success!", 0);
    }

    [DllImport("User32.dll", CharSet = CharSet.Unicode)]
    static extern void MessageBox(IntPtr h, string m, string c, int type);
}
