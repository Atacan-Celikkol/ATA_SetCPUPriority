using System.Reflection;

namespace ATA_SetCPUPriority.Services;

public class ShortcutService : IShortcutService
{
    public void CreateUninstallShortcut()
    {
        string shortcutPath = Path.Combine(Environment.CurrentDirectory, "Uninstall.lnk");

        if (!File.Exists(shortcutPath))
        {
            var shell = new IWshRuntimeLibrary.WshShell();

            var shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutPath);

            shortcut.TargetPath = Path.GetFullPath(AppDomain.CurrentDomain.FriendlyName) + ".exe";
            shortcut.Arguments = "uninstall";
            shortcut.Description = "Uninstall ATA_SetCPUPriority";

            shortcut.Save();

            Console.WriteLine("Uninstall shortcut created.");
        }
    }
}
