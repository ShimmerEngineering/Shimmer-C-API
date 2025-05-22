using shimmer.Communications;
using ShimmerBLEAPI.Devices;
using ShimmerBLEMauiAPI.Communications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShimmerBLEMauiAPI.Devices
{
    public class VerisensePluginBLEDevice : VerisenseBLEDevice
    {
        public static string path;
        public VerisensePluginBLEDevice(string uuid, string name, string comport, CommunicationType commtype) : base(uuid, name)
        {
            ComPort = comport;
            CommType = commtype;
        }

        public VerisensePluginBLEDevice(string uuid, string name) : base(uuid, name)
        {

        }

        protected override void InitializeRadio()
        {
            if (BLERadio != null)
                BLERadio.CommunicationEvent -= UartRX_ValueUpdated;
            if (CommType == CommunicationType.BLE)
            {
                BLERadio = new RadioPluginBLEv3();
            }

        }
    }
}
