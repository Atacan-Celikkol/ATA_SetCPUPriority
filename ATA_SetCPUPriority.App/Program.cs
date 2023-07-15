using ATA.Windows.MessageBox;
using ATA_SetCPUPriority.Services;
using System.Runtime.Versioning;

namespace ATA_SetCPUPriority;

[SupportedOSPlatform("windows")]
class Program
{
    private static readonly RegistryService registryService = new();
    private static readonly ShortcutService shortcutService = new();

    private static readonly string programName = "ATA - Set CPU Priority";

    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            HandleInstall();
        }
        else if (args.Length == 2)
        {
            var filePath = args[0];
            var priority = args[1];

            HandleSetPriority(filePath, priority);
        }
        else if (args.Length == 1 && args[0].ToLower() == "uninstall")
        {
            HandleUninstall();
        }
        else
        {
            throw new ArgumentException();
        }
    }

    private static void HandleInstall()
    {
        var result = MessageBox.Show(
            text: "Right-click menu will be created; do you accept it?",
            caption: "Welcome!",
            buttons: MessageBoxButtons.YesNo);

        if (result == MessageBoxResult.Yes)
        {
            try
            {
                registryService.InstallContextMenu();
                shortcutService.CreateUninstallShortcut();

                MessageBox.Show(
                    text: $"{programName} successfully installed!",
                    caption: "Success!",
                    icon: MessageBoxIcon.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    text: e.Message,
                    caption: "Error!",
                    icon: MessageBoxIcon.Error);
            }
        }
    }

    private static void HandleUninstall()
    {
        var result = MessageBox.Show(
            text: $"Are you sure to uninstall {programName} from the context menu?",
            caption: "Uninstall",
            buttons: MessageBoxButtons.YesNo);

        if (result == MessageBoxResult.Yes)
        {
            try
            {
                registryService.UninstallContextMenu();
                MessageBox.Show(
                    text: $"{programName} successfully uninstalled!",
                    caption: "Success!",
                    icon: MessageBoxIcon.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    text: e.Message,
                    caption: "Error!",
                    icon: MessageBoxIcon.Error);
            }
        }
    }
    private static void HandleSetPriority(string filePath, string priority)
    {
        var programName = Path.GetFileNameWithoutExtension(filePath);
        var priorityName = registryService.PriorityLevels.First(x => x.Value == priority).Key;

        registryService.SetPriority(programName, priority);

        MessageBox.Show(
            text: $"CPU priority successfully set to {priorityName} for {programName}",
            caption: "Success!",
            icon: MessageBoxIcon.Information);
    }

}
