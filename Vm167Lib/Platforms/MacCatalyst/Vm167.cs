using Microsoft.Extensions.Logging;
using System.Data;
using System.Runtime.InteropServices;

namespace Vm167Lib;

partial class Vm167
{
    private readonly IntPtr[] _devices = new IntPtr[IVm167.NumDevices];

    [DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
    private static extern IntPtr IOServiceGetMatchingService(IntPtr masterPort, IntPtr matchingDictionary);

    [DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
    private static extern IntPtr IOServiceMatching(string name);

    [DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
    private static extern IntPtr IOServiceOpen(IntPtr service, uint owningTask, uint type, ref IntPtr connect);

    [DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
    private static extern int IOConnectCallMethod(IntPtr connect, uint selector, ulong[] input, uint inputCnt, IntPtr inputStruct, uint inputStructCnt, out ulong output, out uint outputCnt, IntPtr outputStruct, ref uint outputStructCnt);

    [DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
    private static extern int IOServiceClose(IntPtr connect);

    [DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
    private static extern IntPtr CFNumberCreate(IntPtr allocator, int theType, ref uint value);

    [DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
    private static extern void CFRelease(IntPtr cf);

    [DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
    private static extern void CFDictionarySetValue(IntPtr theDict, IntPtr key, IntPtr value);

    [DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
    private static extern IntPtr CFStringCreateWithCString(IntPtr alloc, string cStr, uint encoding);

    private Task<int> Scan()
    {
        throw new NotImplementedException();
    }

    private async Task<int> Open(int mask)
    {
        if ((mask & 1) > 0)
        {
            _devices[IVm167.Device0] = OpenDevice(Vid, Pid0);
        }

        if ((mask & 2) > 0)
        {
            _devices[IVm167.Device1] = OpenDevice(Vid, Pid1);
        }

        int found = 0;
        for (var i = 0; i < IVm167.NumDevices; i++)
        {
            if (_devices[i] != IntPtr.Zero)
            {
                found |= 1 << i;
            }
        }

        var result = found == 0 ? -1 : found;
        return await Task.FromResult(result);
    }

    private void Close()
    {
        if (_devices == null) throw new NoNullAllowedException(nameof(_devices));

        for (var i = 0; i < IVm167.NumDevices; i++)
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
        IntPtr vendorIdKey = CFStringCreateWithCString(IntPtr.Zero, "idVendor", 0);
        IntPtr productIdKey = CFStringCreateWithCString(IntPtr.Zero, "idProduct", 0);

        IntPtr vendorIdValue = CFNumberCreate(IntPtr.Zero, 9, ref vendorId); // kCFNumberSInt32Type = 9
        IntPtr productIdValue = CFNumberCreate(IntPtr.Zero, 9, ref productId); // kCFNumberSInt32Type = 9

        CFDictionarySetValue(matchingDict, vendorIdKey, vendorIdValue);
        CFDictionarySetValue(matchingDict, productIdKey, productIdValue);

        CFRelease(vendorIdKey);
        CFRelease(productIdKey);
        CFRelease(vendorIdValue);
        CFRelease(productIdValue);

        IntPtr service = IOServiceGetMatchingService(IntPtr.Zero, matchingDict);
        if (service == IntPtr.Zero)
        {
            _logger.LogInformation($"Device: 0x{productId:x} not found");
            return IntPtr.Zero;
        }

        IntPtr connect = IntPtr.Zero;
        uint owningTask = 0; // Use appropriate task
        uint type = 0; // Use appropriate type
        var result = IOServiceOpen(service, owningTask, type, ref connect);
        if (result != 0)
        {
            _logger.LogInformation($"Failed to open device: 0x{productId:x}, Error: 0x{result:x}");
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

        int result = IOConnectCallMethod(device, 1, input, inputCnt, inputStruct, inputStructCnt, out output, out outputCnt, outputStruct, ref outputStructCnt);

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

        int result = IOConnectCallMethod(device, 0, input, inputCnt, inputStruct, inputStructCnt, out output, out outputCnt, outputStruct, ref outputStructCnt);
        if (result == 0)
        {
            Marshal.Copy(outputStruct, buffer, 0, buffer.Length);
        }

        Marshal.FreeHGlobal(outputStruct);
        var size = result == 0 ? (int)outputStructCnt : -1;
        return await Task.FromResult(size);
    }
}
