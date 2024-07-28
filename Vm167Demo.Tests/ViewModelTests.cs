using Vm167Demo.ViewModels;
using Moq;
using Vm167Box;

namespace Vm167Demo.Tests
{
    [TestClass]
    public class ViewModelTests
    {
        [TestMethod]
        public async Task OpenDevice_Success()
        {
            // Arrange
            var vm167Mock = new Mock<IVm167>();
            var vm = new MainViewModel(vm167Mock.Object);

            // Act
            await vm.OpenDevices();

            // Assert
            vm167Mock.Verify(service => service.OpenDevices(), Times.Once);
        }
    }
}
