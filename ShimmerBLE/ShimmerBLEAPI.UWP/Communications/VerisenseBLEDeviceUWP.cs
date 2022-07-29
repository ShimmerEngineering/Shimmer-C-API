using ShimmerBLEAPI.Devices;
using System;
using System.Collections.Generic;
using System.Text;
using shimmer.Communications;

namespace ShimmerBLEAPI.UWP.Communications
{
    public class VerisenseBLEDeviceUWP : VerisenseBLEDevice
    {
        public VerisenseBLEDeviceUWP(string uuid, string name, string comport, CommunicationType commtype) : base(uuid, name)
        {
            ComPort = comport;
            CommType = commtype;
        }
        public VerisenseBLEDeviceUWP(string id, string name) : base(id, name)
        {

        }
        protected override void InitializeRadio()
        {
            if (BLERadio != null)
                BLERadio.CommunicationEvent -= UartRX_ValueUpdated;
            if (CommType == CommunicationType.BLE)
            {
                BLERadio = new RadioPluginBLE();
            }
            else if (CommType == CommunicationType.SerialPort)
            {
                BLERadio = new SerialPortByteCommunicationUWP();
                ((SerialPortByteCommunicationUWP)BLERadio).ComPort = ComPort;
            }
        }
    }
}
