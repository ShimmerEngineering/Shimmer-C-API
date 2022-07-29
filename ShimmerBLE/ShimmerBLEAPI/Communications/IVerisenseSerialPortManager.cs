using ShimmerBLEAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace shimmer.Communications
{
    public interface IVerisenseSerialPortManager
    {
        List<VerisenseSerialDevice> GetListOfSerialDevices();
        Task<bool> StartScanForSerialPorts();
        void StopScanForSerialPorts();
    }
}
