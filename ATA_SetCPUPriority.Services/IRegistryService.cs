namespace ATA_SetCPUPriority.Services;

public interface IRegistryService
{
    void InstallContextMenu();
    void SetPriority(string programName, string priority);
    void UninstallContextMenu();
}
