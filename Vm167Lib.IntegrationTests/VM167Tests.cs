using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using Moq;
using System.Data;

namespace Vm167Lib.IntegrationTests;

[TestClass]
public class VM167Tests
{
    private readonly Vm167 _vm167;
    private readonly Mock<ILogger<Vm167>> _logger = new();
    private int _active;

    public VM167Tests()
    {
        _vm167 = new(_logger.Object);
    }

    [TestInitialize]
    public async Task TestInitialize()
    {
        var mask = await _vm167.ListDevices();
        _active = await _vm167.OpenDevices(mask);
        Assert.AreEqual(mask, _active);
        Assert.IsFalse(_active == -1, "No VM167 cards found");
        Assert.IsFalse(_active == 0, "Connect problem, disconnect and reconnect USB cable");
    }

    [TestCleanup]
    public async Task TestCleanup()
    {
        await _vm167.CloseDevices();
    }

    [DataTestMethod]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    [DataRow(4)]
    [DataRow(5)]
    public async Task ReadAnalogChannel_GivenChannel_Success(int channel)
    {
        // Arrange
        // Act & Assert
        for (int device = 0; device < Vm167.NumDevices; device++)
        {
            if ((_active & (1 << device)) == 0) continue;
            var value = await _vm167.ReadAnalogChannel(device, channel);
            Logger.LogMessage($"ReadAnalogChannel [{device}:{channel}]: {value}");

            // Assert
            Assert.IsTrue(value >= 0);
        }
    }

    [TestMethod]
    public async Task ReadAllAnalog_Success()
    {
        // Arrange
        int[] values = new int[Vm167.NumAnalogIn];

        // Act & Assert
        for (int device = 0; device < Vm167.NumDevices; device++)
        {
            if ((_active & (1 << device)) == 0) continue;
            await _vm167.ReadAllAnalog(device, values);
            Logger.LogMessage($"ReadAllAnalog [{device}]: {string.Join(", ", values)}");

            foreach (var value in values)
            {
                Assert.IsTrue(value > 0);
            }
        }
    }

    [DataTestMethod]
    [DataRow(1, 127, 1)]
    [DataRow(2, 16, 2)]
    public async Task SetPWM_GivenChannel_Success(int channel, int data, int freq)
    {
        // Arrange
        // Act & Assert
        for (int device = 0; device < Vm167.NumDevices; device++)
        {
            if ((_active & (1 << device)) == 0) continue;
            await _vm167.SetPWM(device, channel, data, freq);
            Logger.LogMessage($"SetPWM [{device}:{channel}]: {data} @ {freq}");
        }
    }

    [DataTestMethod]
    [DataRow(127, 127)]
    [DataRow(255, 255)]
    [DataRow(15, 15)]
    [DataRow(0, 0)]
    public async Task OutputAllPWM_Success(int data1, int data2)
    {
        // Arrange
        // Act & Assert
        for (int device = 0; device < Vm167.NumDevices; device++)
        {
            if ((_active & (1 << device)) == 0) continue;
            await _vm167.OutputAllPWM(device, data1, data2);
            Logger.LogMessage($"OutputAllPWM [{device}]: {data1} & {data2}");
        }
    }

    [DataTestMethod]
    [DataRow(255)]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    [DataRow(8)]
    [DataRow(8)]
    [DataRow(16)]
    [DataRow(32)]
    [DataRow(64)]
    [DataRow(128)]
    public async Task OutputAllDigital_Success(int data)
    {
        // Arrange
        // Act & Assert
        for (int device = 0; device < Vm167.NumDevices; device++)
        {
            if ((_active & (1 << device)) == 0) continue;
            await _vm167.InOutMode(device, 0, 0);
            await _vm167.OutputAllDigital(device, data);
            Logger.LogMessage($"OutputAllDigital [{device}]: {data}");
        }
    }

    [DataTestMethod]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    [DataRow(4)]
    [DataRow(5)]
    [DataRow(6)]
    [DataRow(7)]
    [DataRow(8)]
    public async Task ClearDigitalChannel_Success(int channel)
    {
        // Arrange
        // Act & Assert
        for (int device = 0; device < Vm167.NumDevices; device++)
        {
            if ((_active & (1 << device)) == 0) continue;
            await _vm167.InOutMode(device, 0, 0);
            await _vm167.ClearDigitalChannel(device, channel);
            Logger.LogMessage($"ClearDigitalChannel [{device}]: {channel}");
        }
    }

    [TestMethod]
    public async Task ClearAllDigital_Success()
    {
        // Arrange
        // Act & Assert
        for (int device = 0; device < Vm167.NumDevices; device++)
        {
            if ((_active & (1 << device)) == 0) continue;
            await _vm167.InOutMode(device, 0, 0);
            await _vm167.ClearAllDigital(device);
            Logger.LogMessage($"ClearAllDigital [{device}]");
        }
    }

    [DataTestMethod]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    [DataRow(4)]
    [DataRow(5)]
    [DataRow(6)]
    [DataRow(7)]
    [DataRow(8)]
    public async Task SetDigitalChannel_Success(int channel)
    {
        // Arrange
        // Act & Assert
        for (int device = 0; device < Vm167.NumDevices; device++)
        {
            if ((_active & (1 << device)) == 0) continue;
            await _vm167.InOutMode(device, 0, 0);
            await _vm167.SetDigitalChannel(device, channel);
            Logger.LogMessage($"SetDigitalChannel [{device}]: {channel}");
        }
    }

    [TestMethod]
    public async Task SetAllDigital_Success()
    {
        // Arrange
        // Act & Assert
        for (int device = 0; device < Vm167.NumDevices; device++)
        {
            if ((_active & (1 << device)) == 0) continue;
            await _vm167.InOutMode(device, 0, 0);
            await _vm167.SetAllDigital(device);
            Logger.LogMessage($"SetAllDigital [{device}]");
        }
   }

    [DataTestMethod]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    [DataRow(4)]
    [DataRow(5)]
    [DataRow(6)]
    [DataRow(7)]
    [DataRow(8)]
    public async Task ReadDigitalChannel_Success(int channel)
    {
        // Arrange
        // Act & Assert
        for (int device = 0; device < Vm167.NumDevices; device++)
        {
            if ((_active & (1 << device)) == 0) continue;
            var value = await _vm167.ReadDigitalChannel(device, channel);
            Logger.LogMessage($"ReadDigitalChannel [{device}:{channel}]: {value}");
        }
    }

    [TestMethod]
    public async Task ReadAllDigital_Success()
    {
        // Arrange
        // Act & Assert
        for (int device = 0; device < Vm167.NumDevices; device++)
        {
            if ((_active & (1 << device)) == 0) continue;
            var value = await _vm167.ReadAllDigital(device);
            Logger.LogMessage($"ReadAllDigital [{device}]: {value}");
        }
    }

    [TestMethod]
    public async Task ReadCounter()
    {
        // Arrange
        // Act & Assert
        for (int device = 0; device < Vm167.NumDevices; device++)
        {
            if ((_active & (1 << device)) == 0) continue;
            var value = await _vm167.ReadCounter(device);
            Logger.LogMessage($"ReadCounter [{device}]: {value}");
        }
    }

    [TestMethod]
    public async Task ResetCounter()
    {
        // Arrange
        // Act & Assert
        for (int device = 0; device < Vm167.NumDevices; device++)
        {
            if ((_active & (1 << device)) == 0) continue;
            await _vm167.ResetCounter(device);
        }
    }

    [TestMethod]
    public async Task Connected()
    {
        // Arrange
        // Act
        var connected = await _vm167.Connected();
        Logger.LogMessage($"Connected: {connected}");

        // Assert
        Assert.IsTrue(connected > 0);
    }

    [TestMethod]
    public async Task VersionFirmware()
    {
        // Arrange
        // Act & Assert
        for (int device = 0; device < Vm167.NumDevices; device++)
        {
            if ((_active & (1 << device)) == 0) continue;
            var version = await _vm167.VersionFirmware(device);
            Logger.LogMessage($"VersionFirmware [{device}]: {version:X}");
            Assert.IsTrue(version > 0);
        }
    }

    [TestMethod]
    public async Task ReadBackPWMOut()
    {
        // Arrange
        int[] buffer = new int[Vm167.NumPwmOut];

        // Act & Assert
        for (int device = 0; device < Vm167.NumDevices; device++)
        {
            if ((_active & (1 << device)) == 0) continue;
            await _vm167.ReadBackPWMOut(device, buffer);
            Logger.LogMessage($"ReadBackPWMOut [{device}]: {string.Join(", ", buffer)}");
        }
    }

    [TestMethod]
    public async Task ReadBackInOutMode()
    {
        // Arrange

        // Act & Assert
        for (int device = 0; device < Vm167.NumDevices; device++)
        {
            if ((_active & (1 << device)) == 0) continue;
            var mode = await _vm167.ReadBackInOutMode(device);
            Logger.LogMessage($"ReadBackInOutMode [{device}]: {mode}");
        }
    }
}