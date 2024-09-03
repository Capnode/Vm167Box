using Microsoft.Extensions.Logging;
using System.Data;
using Windows.Devices.Usb;
using Windows.Storage.Streams;

namespace Vm167Lib;

partial class Vm167
{
    private readonly UsbDevice?[] _devices = new UsbDevice[NumDevices];

    private async Task<int> Open()
    {
        _devices[Device0] = await ScanPort(Pid0);
        _devices[Device1] = await ScanPort(Pid1);

        int found = 0;
        for (var i = 0; i < NumDevices; i++)
        {
            if (_devices[i] != null)
            {
                found |= 1 << i;
            }
        }

        return found == 0 ? -1 : found;
    }

    private void Close()
    {
        if (_devices == null) throw new NoNullAllowedException(nameof(_devices));

        for (var i = 0; i < NumDevices; i++)
        {
            _devices[i]?.Dispose();
            _devices[i] = null;
        }
    }

    private async Task<UsbDevice?> ScanPort(uint pid)
    {
        var aqs = UsbDevice.GetDeviceSelector(Vid, pid);
        var devices = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(aqs);
        UsbDevice? device = null;
        for (var i = 0; i < devices.Count; i++)
        {
            try
            {
                device = await UsbDevice.FromIdAsync(devices[i].Id);
                _logger.LogInformation($"open device: 0x{pid:x}:{i}");
                return device;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"open device: 0x{pid:x}:{i} failed: {ex.Message}");
            }
        }

        return device;
    }

    private async Task<int> Write(int cardAddress, int bytes)
    {
        var device = _devices[cardAddress];
        if (device == null) return 0;
        var buffer = _deviceBuffer[cardAddress];
        UsbBulkOutPipe writePipe = device.DefaultInterface.BulkOutPipes[0];
        writePipe.WriteOptions |= UsbWriteOptions.ShortPacketTerminate;

        var stream = writePipe.OutputStream;
        DataWriter writer = new(stream);
        writer.WriteBytes(buffer.AsSpan(0, bytes).ToArray());

        uint bytesWritten = 0;
        try
        {
            bytesWritten = await writer.StoreAsync();
        }
        catch (Exception exception)
        {
            _logger.LogError("Write error: {}", exception.Message.ToString());
        }
        finally
        {
            _logger.LogTrace("Data written: {} bytes.", bytesWritten);
        }

        return (int)bytesWritten;
    }

    private async Task<int> Read(int cardAddress)
    {
        var device = _devices[cardAddress];
        if (device == null) return 0;
        var buffer = _deviceBuffer[cardAddress];
        UsbBulkInPipe readPipe = device.DefaultInterface.BulkInPipes[0];
        readPipe.ReadOptions |= UsbReadOptions.AutoClearStall;

        // Read data from the input pipe and store it in the buffer
        var stream = readPipe.InputStream;

        uint bytesRead;
        DataReader reader = new(stream);
        while (true)
        {
            try
            {
                bytesRead = await reader.LoadAsync((uint)buffer.Length);
                if (bytesRead > 0)
                {
                    reader.ReadBytes(buffer);
                    break;
                }
            }
            catch (Exception exception)
            {
                _logger.LogError("Read error: {}", exception.Message);
            }
        }

        return (int)bytesRead;
    }
}
