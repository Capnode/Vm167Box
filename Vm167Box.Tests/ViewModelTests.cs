using Vm167Box.ViewModels;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using Libvm167;

namespace Vm167Box.Tests
{
    [TestClass]
    public class ViewModelTests
    {
        private readonly Mock<ILogger<MainViewModel>> _logger = new();

        [TestMethod]
        public async Task Open_TwoDevices_Success()
        {

            // Arrange
            var vm167Mock = new Mock<IVm167>();
            vm167Mock.Setup(service => service.OpenDevices()).ReturnsAsync(3);
            var vm = new MainViewModel(_logger.Object, vm167Mock.Object);

            // Act
            await vm.Open();
            Logger.LogMessage($"Open device");

            // Assert
            vm167Mock.Verify(service => service.OpenDevices(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException), "No VM167 card found")]
        public async Task Open_NoDevices_ThrowException()
        {

            // Arrange
            var vm167Mock = new Mock<IVm167>();
            vm167Mock.Setup(service => service.OpenDevices()).ReturnsAsync(-1);
            var vm = new MainViewModel(_logger.Object, vm167Mock.Object);

            // Act
            await vm.Open();

            // Assert
            vm167Mock.Verify(service => service.OpenDevices(), Times.Once);
        }
    }
}
