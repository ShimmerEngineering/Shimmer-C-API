using ShimmerBLEAPI.Communications;
using ShimmerBLEAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace shimmer.Communications
{
    public interface IVerisenseBLEManager
    {
        List<VerisenseBLEScannedDevice> GetListOfScannedDevices();
        Task<bool> StartScanForDevices();
        void StopScanForDevices();
        Task<bool> PairVerisenseDevice(Object Device, IBLEPairingKeyGenerator generator);

        event EventHandler<BLEManagerEvent> BLEManagerEvent;
        EventHandler<BLEManagerEvent> GetBLEManagerEvent();
        
    }

    /// <summary>
    /// Store the event data
    /// </summary>
    public class BLEManagerEvent
    {
        public enum BLEAdapterEvent
        {
            DeviceDiscovered = 1,
            DeviceConnectionLost = 2,
            DeviceDisconnected = 3,
            AdapterScanTimeout = 4,
            ScanCompleted = 5,
            DevicePaired = 6
        }
        public BLEAdapterEvent CurrentEvent;
        public string message;
        public Object objMsg;
    }
}
