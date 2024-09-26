using Vm167Box.ViewModels;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using Vm167Box.Services;

namespace Vm167Box.Tests.ViewModels
{
    [TestClass]
    public class ViewModelTests
    {
        private readonly Mock<ILogger<PanelViewModel>> _logger = new();

        [TestMethod]
        public void Open_TwoDevices_Success()
        {
            // Arrange
            var settingsMock = new Mock<ISettingsService>();
            var vm167Mock = new Mock<IVm167Service>();
            vm167Mock.Setup(service => service.ListDevices()).ReturnsAsync(3);

            // Act
            var vm = new PanelViewModel(_logger.Object, settingsMock.Object, vm167Mock.Object);
            Logger.LogMessage($"Open device");

            // Assert
            vm167Mock.Verify(service => service.ListDevices(), Times.Once);
        }
    }
}
