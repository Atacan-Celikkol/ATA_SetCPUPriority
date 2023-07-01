namespace ATA_SetCPUPriority.Services;

public interface IMessageBoxService
{
    MessageBoxResult Show(string text, string? caption = null, MessageBoxButtons? buttons = null, MessageBoxIcon? icon = null, MessageBoxDefaultButton? defaultButton = null, MessageBoxModal? modal = null);
}
