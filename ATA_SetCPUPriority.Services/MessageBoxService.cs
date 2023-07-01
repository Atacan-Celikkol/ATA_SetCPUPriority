using System.Runtime.InteropServices;

namespace ATA_SetCPUPriority.Services;

public class MessageBoxService : IMessageBoxService
{
    [DllImport("user32.dll")]
    static extern int MessageBox(nint hWnd, string lpText, string lpCaption, uint uType);

    public MessageBoxResult Show(string text, string? caption = null, MessageBoxButtons? buttons = null, MessageBoxIcon? icon = null, MessageBoxDefaultButton? defaultButton = null, MessageBoxModal? modal = null)
    {
        uint uType = (uint)(buttons ?? MessageBoxButtons.Ok) | (uint)(icon ?? MessageBoxIcon.None) | (uint)(defaultButton ?? MessageBoxDefaultButton.Button1) | (uint)(modal ?? MessageBoxModal.Application);
        return (MessageBoxResult)MessageBox(nint.Zero, text, caption ?? "\0", uType);
    }
}

public enum MessageBoxButtons : uint
{
    Ok = 0x00000000,
    OkCancel = 0x00000001,
    YesNo = 0x00000004,
    YesNoCancel = 0x00000003
}

public enum MessageBoxResult
{
    Ok = 1,
    Cancel = 2,
    Abort = 3,
    Retry = 4,
    Ignore = 5,
    Yes = 6,
    No = 7,
    TryAgain = 10,
    Continue = 11
}

public enum MessageBoxDefaultButton : uint
{
    Button1 = 0x00000000,
    Button2 = 0x00000100,
    Button3 = 0x00000200,
    Button4 = 0x00000300
}

public enum MessageBoxModal : uint
{
    Application = 0x00000000,
    System = 0x00001000,
    Task = 0x00002000
}

public enum MessageBoxIcon : uint
{
    None = 0x00000000,
    Warning = 0x00000030,
    Information = 0x00000040,
    Error = 0x00000010
}
