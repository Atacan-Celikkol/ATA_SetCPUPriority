using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ATA_SetCPUPriority.Services.Tests
{
    [TestClass()]
    public class MessageBoxServiceTests
    {
        private readonly MessageBoxService _service = new();
        [TestMethod()]
        public void ShowTest()
        {
            try
            {
                _service.Show(text: "Hello", caption: "Test", icon: MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                Assert.Fail($"An exception was thrown: {ex.Message}");
            }
        }
    }
}