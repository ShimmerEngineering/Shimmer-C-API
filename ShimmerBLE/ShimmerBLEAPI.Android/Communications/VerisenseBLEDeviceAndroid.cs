using ShimmerBLEAPI.Devices;
using shimmer.Communications;

namespace ShimmerBLEAPI.Android.Communications
{
    public class VerisenseBLEDeviceAndroid : VerisenseBLEDevice
    {
        public VerisenseBLEDeviceAndroid(string uuid, string name, string comport, CommunicationType commtype) : base(uuid, name)
        {
            ComPort = comport;
            CommType = commtype;
        }
        public VerisenseBLEDeviceAndroid(string id, string name) : base(id, name)
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
                BLERadio = new SerialPortByteCommunicationAndroid();
                ((SerialPortByteCommunicationAndroid)BLERadio).ComPort = ComPort;
            }
        }
    }
}
