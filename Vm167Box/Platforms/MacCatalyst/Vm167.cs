using Microsoft.Extensions.Logging;
using System.Data;
using System.Runtime.InteropServices;

namespace Vm167Box;

partial class Vm167
{
    private IntPtr _deviceHandle;
    private readonly IntPtr[] _devices = new IntPtr[NumDevices];

    [DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
    private static extern IntPtr IOServiceGetMatchingService(IntPtr masterPort, IntPtr matchingDictionary);

    [DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
    private static extern IntPtr IOServiceMatching(string name);

    [DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
    private static extern IntPtr IOServiceOpen(IntPtr service, uint owningTask, uint type, ref IntPtr connect);

    [DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
    private static extern int IOConnectCallMethod(IntPtr connect, uint selector, ulong[] input, uint inputCnt, IntPtr inputStruct, uint inputStructCnt, out ulong output, out uint outputCnt, IntPtr outputStruct, ref uint outputStructCnt);

    [DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
    private static extern IntPtr IOServiceAddMatchingValue(IntPtr matchingDictionary, string key, IntPtr value);

    [DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
    private static extern int IOServiceClose(IntPtr connect);

    private async Task<int> Open()
    {
        _devices[Device0] = OpenDevice(Vid, Pid0);
        _devices[Device1] = OpenDevice(Vid, Pid1);

        int found = 0;
        for (var i = 0; i < NumDevices; i++)
        {
            if (_devices[i] != IntPtr.Zero)
            {
                found |= 1 << i;
            }
        }

        var mask = found == 0 ? -1 : found;
        return await Task.FromResult(mask);
    }

    private void Close()
    {
        if (_devices == null) throw new NoNullAllowedException(nameof(_devices));

        for (var i = 0; i < NumDevices; i++)
        {
            if (_devices[i] != IntPtr.Zero)
            {
                IOServiceClose(_devices[i]);
                _devices[i] = IntPtr.Zero;
            }
        }
    }

    private IntPtr OpenDevice(uint vendorId, uint productId)
    {
        IntPtr matchingDict = IOServiceMatching("IOUSBDevice");

        // Add vendorId and productId to the matching dictionary
        IOServiceAddMatchingValue(matchingDict, "idVendor", (IntPtr)vendorId);
        IOServiceAddMatchingValue(matchingDict, "idProduct", (IntPtr)productId);

        IntPtr service = IOServiceGetMatchingService(IntPtr.Zero, matchingDict);
        if (service == IntPtr.Zero)
        {
            _logger.LogInformation($"Device: 0x{productId:x} not found");
            return IntPtr.Zero;
        }

        IntPtr connect = IntPtr.Zero;
        uint owningTask = 0; // Use appropriate task
        uint type = 0; // Use appropriate type
        if (IOServiceOpen(service, owningTask, type, ref connect) != 0)
        {
            _logger.LogInformation($"Failed to open device: 0x{productId:x}");
            return IntPtr.Zero;
        }

        return connect;
    }

    private async Task<int> Write(int cardAddress, int bytes)
    {
        var device = _devices[cardAddress];
        if (device == IntPtr.Zero) return 0;
        var buffer = _deviceBuffer[cardAddress];

        ulong[] input = new ulong[0];
        uint inputCnt = 0;
        IntPtr inputStruct = Marshal.AllocHGlobal(bytes);
        Marshal.Copy(buffer, 0, inputStruct, bytes);
        uint inputStructCnt = (uint)bytes;
        ulong output;
        uint outputCnt = 1;
        IntPtr outputStruct = IntPtr.Zero;
        uint outputStructCnt = 0;

        int result = IOConnectCallMethod(_deviceHandle, 1, input, inputCnt, inputStruct, inputStructCnt, out output, out outputCnt, outputStruct, ref outputStructCnt);

        Marshal.FreeHGlobal(inputStruct);
        var size = result == 0 ? (int)output : -1;
        return await Task.FromResult(size);
    }

    private async Task<int> Read(int cardAddress)
    {
        var device = _devices[cardAddress];
        if (device == IntPtr.Zero) return 0;
        var buffer = _deviceBuffer[cardAddress];

        ulong[] input = new ulong[0];
        uint inputCnt = 0;
        IntPtr inputStruct = IntPtr.Zero;
        uint inputStructCnt = 0;
        ulong output;
        uint outputCnt = 1;
        IntPtr outputStruct = Marshal.AllocHGlobal(buffer.Length);
        uint outputStructCnt = (uint)buffer.Length;

        int result = IOConnectCallMethod(_deviceHandle, 0, input, inputCnt, inputStruct, inputStructCnt, out output, out outputCnt, outputStruct, ref outputStructCnt);
        if (result == 0)
        {
            Marshal.Copy(outputStruct, buffer, 0, buffer.Length);
        }

        Marshal.FreeHGlobal(outputStruct);
        var size = result == 0 ? (int)outputStructCnt : -1;
        return await Task.FromResult(size);
    }
}
