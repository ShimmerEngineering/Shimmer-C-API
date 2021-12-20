using System;
using System.Collections.Generic;
using System.Text;

namespace ShimmerBLEAPI.Models
{
    public class VerisenseBLEScannedDevice
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public int RSSI { get; set; }
        public Guid Uuid { get; set; }
        public bool IsPaired { get; set; }
        public bool IsConnected { get; set; }
        public bool IsConnectable { get; set; }
        public Object DeviceInfo { get; set; }

    }
}
