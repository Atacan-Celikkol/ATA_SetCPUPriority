// ... [your existing import statements] ...

using System.Reflection;

namespace ATA_SetCPUPriority.Services;

public class ShortcutService : IShortcutService
{
    // Add your existing shortcut-related code here, 
    // refactoring it into the method defined in IShortcutService.
    // Make sure to add try/catch blocks for error handling.

    public void CreateUninstallShortcut()
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
}
