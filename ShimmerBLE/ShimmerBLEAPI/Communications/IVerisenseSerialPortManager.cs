using ShimmerBLEAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShimmerBLEAPI.Devices;

namespace shimmer.Communications
{
    public interface IVerisenseSerialPortManager
    {
        List<VerisenseSerialDevice> GetListOfSerialDevices();
        Task<bool> StartScanForSerialPorts();
        void StopScanForSerialPorts();
        VerisenseBLEDevice CreateVerisenseSerialDevice(string uuid, string serialId);
    }
}
