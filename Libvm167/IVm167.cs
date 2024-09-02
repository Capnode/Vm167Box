namespace Libvm167
{
    public interface IVm167 : IDisposable
    {
        /// <summary>
        /// Open devices found on system
        /// </summary>
        /// <returns>
        ///   -1 no devices found,
        ///    0 driver problem, try reconnecting,
        ///    1 device address 0 found,
        ///    2 device address 1 found,
        ///    3 device address 0 and 1 found
        /// </returns>
        Task<int> OpenDevices();

        /// <summary>
        /// Close devices
        /// </summary>
        Task CloseDevices();

        /// <summary>
        /// Read analog channel
        /// </summary>
        /// <param name="CardAddress">address of device 0 or 1</param>
        /// <param name="Channel">channel to read, 1-8</param>
        /// <returns>value of channel 0..1023</returns>
        Task<int> ReadAnalogChannel(int CardAddress, int Channel);

        /// <summary>
        /// Read all analog channels
        /// </summary>
        /// <param name="CardAddress">address of device 0 or 1</param>
        /// <param name="Buffer">array of 5 ints to be populated with values</param>
        Task ReadAllAnalog(int CardAddress, int[] Buffer);

        /// <summary>
        /// Set PWM output
        /// </summary>
        /// <param name="CardAddress">address of device 0 or 1</param>
        /// <param name="Channel">channel on card 1 or 2 </param>
        /// <param name="Data">value to output 0..255</param>
        /// <param name="Freq">PWM frequency 1..3, 1:  2929.68 Hz, 2: 11718.75 Hz, 3: 46875 Hz</param>
        Task SetPWM(int CardAddress, int Channel, int Data, int Freq);

        /// <summary>
        /// Set both PWM outputs
        /// </summary>
        /// <param name="CardAddress">address of device 0 or 1</param>
        /// <param name="Data1">Channel 1 value 0..255</param>
        /// <param name="Data2">Channel 2 value 0..255</param>
        Task OutputAllPWM(int CardAddress, int Data1, int Data2);

        /// <summary>
        /// Set digital outputs 
        /// </summary>
        /// <param name="CardAddress">address of device 0 or 1</param>
        /// <param name="Data">bit pattern for outputs</param>
        Task OutputAllDigital(int CardAddress, int Data);

        /// <summary>
        /// Clears digital channel (set to 0)
        /// </summary>
        /// <param name="CardAddress">address of device 0 or 1</param>
        /// <param name="Channel">channel to clear 1..8</param>
        Task ClearDigitalChannel(int CardAddress, int Channel);

        /// <summary>
        /// Clears all digital channels (set to 0)
        /// </summary>
        /// <param name="CardAddress">address of device 0 or 1</param>
        Task ClearAllDigital(int CardAddress);

        /// <summary>
        /// Sets digital channel (set to 1)
        /// </summary>
        /// <param name="CardAddress">address of device 0 or 1</param>
        /// <param name="Channel">channel to set 1..8</param>
        Task SetDigitalChannel(int CardAddress, int Channel);

        /// <summary>
        /// Sets all digital channels (set to 1)
        /// </summary>
        /// <param name="CardAddress">address of device 0 or 1</param>
        Task SetAllDigital(int CardAddress);

        /// <summary>
        /// Reads digital channel
        /// </summary>
        /// <param name="CardAddress">address of device 0 or 1</param>
        /// <param name="Channel">channel to read 1..8</param>
        /// <returns>value of channel true/false</returns>
        Task<bool> ReadDigitalChannel(int CardAddress, int Channel);

        /// <summary>
        /// Reads all digital channels
        /// </summary>
        /// <param name="CardAddress">address of device 0 or 1</param>
        /// <returns>bitpattern of digital channels</returns>
        Task<int> ReadAllDigital(int CardAddress);

        /// <summary>
        /// Sets the input/output mode of digital channels
        /// </summary>
        /// <param name="CardAddress">address of device 0 or 1</param>
        /// <param name="HighNibble">mode of channels 5-8, 0 output, 1 input</param>
        /// <param name="LowNibble">mode of channels 1-4, 0 output, 1 input</param>
        Task InOutMode(int CardAddress, int HighNibble, int LowNibble);

        /// <summary>
        /// Read pulse counter on digital 1
        /// </summary>
        /// <param name="CardAddress">address of device 0 or 1</param>
        /// <returns>value of pulse counter</returns>
        Task<uint> ReadCounter(int CardAddress);

        /// <summary>
        /// Reset pulse counter on digital 1 to 0
        /// </summary>
        /// <param name="CardAddress">address of device 0 or 1</param>
        Task ResetCounter(int CardAddress);

        /// <summary>
        /// Check connection status
        /// </summary>
        /// <returns>0: No devices connected, 1: Device address 0 connected, 2: Device address 1 connected, 3: Deivces 0 and 1 connected</returns>
        Task<int> Connected();

        /// <summary>
        /// Check firmware version for card
        /// </summary>
        /// <param name="CardAddress">address of device 0 or 1</param>
        /// <returns>is 4 byte sized fields each representing one digit of the firmware version</returns>
        Task<int> VersionFirmware(int CardAddress);

        /// <summary>
        /// Check dll version
        /// </summary>
        /// <returns>is 4 byte sized fields each representing one digit of the dll version</returns>
        int VersionDLL();

        /// <summary>
        /// Read the values of the PWM outputs
        /// </summary>
        /// <param name="CardAddress">address of device 0 or 1</param>
        /// <param name="Buffer">array of 2 integers to be populated</param>
        Task ReadBackPWMOut(int CardAddress, int[] Buffer);

        /// <summary>
        /// Read the Input/Output mode of digital pins
        /// </summary>
        /// <param name="CardAddress">address of device 0 or 1</param>
        /// <returns>0: All outputs, 1: 1-4 input, 5-8 output, 2: 1-4 output, 5-8 input, 3: All inputs</returns>
        Task<int> ReadBackInOutMode(int CardAddress);
    }
}
