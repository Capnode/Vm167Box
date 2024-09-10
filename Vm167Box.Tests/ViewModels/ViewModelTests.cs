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
        public async Task Open_TwoDevices_Success()
        {
            // Arrange
            var settingsMock = new Mock<ISettingsService>();
            var vm167Mock = new Mock<IVm167Service>();
            vm167Mock.Setup(service => service.ListDevices()).ReturnsAsync(3);
            var vm = new PanelViewModel(_logger.Object, settingsMock.Object, vm167Mock.Object);

            // Act
            await vm.Open();
            Logger.LogMessage($"Open device");

            // Assert
            vm167Mock.Verify(service => service.ListDevices(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public async Task Open_NoDevices_ThrowException()
        {
            // Arrange
            var settingsMock = new Mock<ISettingsService>();
            var vm167Mock = new Mock<IVm167Service>();
            vm167Mock.Setup(service => service.ListDevices()).Throws(new ApplicationException());
            var vm = new PanelViewModel(_logger.Object, settingsMock.Object, vm167Mock.Object);

            // Act
            await vm.Open();

            // Assert
            vm167Mock.Verify(service => service.OpenDevice(It.IsAny<int>()), Times.Never);
        }
    }
}
