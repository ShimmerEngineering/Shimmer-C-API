using shimmer.Communications;
using shimmer.Models;
using static shimmer.Models.ShimmerBLEEventData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace JointCorpWatch
{
    public class JCWatch
    {
        #region Comm Props
        public event EventHandler<ShimmerBLEEventData> ShimmerBLEEvent;
        protected IVerisenseByteCommunication BLERadio;
        public int GallCallBackErrorCount = 0;

        public Guid Asm_uuid { get; set; }
        protected const string LogObject = "JCWatch";
        protected const string BadCRC = "BadCRC";
        ShimmerDeviceBluetoothState CurrentBluetoothState = ShimmerDeviceBluetoothState.None;
        #endregion

        #region Read Request props

        public static byte[] readHistory = new byte[] { 0x71, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x71 };
        public static byte[] openecg = new byte[] { 0x99, 0x04, 0x00, 0x5a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xf7 };
        public static byte[] getDeviceInfo = new byte[] { 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04 };
        public static byte[] ecgppgRealTime = new byte[] { 0x99, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x9A };
        public static byte[] ppgRealTime = new byte[] { 0x39, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3A };
        public static byte[] getMacAddress = new byte[] { 0x22, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 0x22 };

        #endregion

        public JCWatch(String id, bool reconnect = false)
        {
            Asm_uuid = Guid.Parse(id);
        }

        protected async Task<bool> GetKnownDevice()
        {
            BLERadio.Asm_uuid = Asm_uuid;
            BLERadio.CommunicationEvent += UartRX_ValueUpdated;

            var result = await BLERadio.Connect();
            if (result.Equals(ConnectivityState.Connected))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual void InitializeRadio()
        {
            if (BLERadio != null)
                BLERadio.CommunicationEvent -= UartRX_ValueUpdated;

            BLERadio = new JCWatchRadio(Asm_uuid.ToString());
        }

        public async Task<bool> Connect(bool autoconnect=false)
        {
            InitializeRadio();
            StateChange(ShimmerDeviceBluetoothState.Connecting);
            var result = await GetKnownDevice();
            if (!result)
            {
                StateChange(ShimmerDeviceBluetoothState.Disconnected);
                return false;
            }
            StateChange(ShimmerDeviceBluetoothState.Connected);
            return true;
        }

        public void CancelConnect()
        {
            ((JCWatchRadio)BLERadio).CancelConnect();
        }

        public async Task<bool> WriteBytes(byte[] value)
        {
            await BLERadio.WriteBytes(value);
            return true;
        }

        public async Task<bool> Disconnect()
        {
            var result = await BLERadio.Disconnect();
            if (result.Equals(ConnectivityState.Disconnected))
            {
                BLERadio.CommunicationEvent -= UartRX_ValueUpdated;
                StateChange(ShimmerDeviceBluetoothState.Disconnected);
                return true;
            }
            else if (result.Equals(ConnectivityState.Limited))
            {
                StateChange(ShimmerDeviceBluetoothState.Limited);
                return true;
            }
            return true;
        }

        protected void StateChange(ShimmerDeviceBluetoothState state)
        {
            if (!CurrentBluetoothState.Equals(state))
            {
                CurrentBluetoothState = state;
                if (ShimmerBLEEvent != null)
                    ShimmerBLEEvent.Invoke(null, new ShimmerBLEEventData { ASMID = Asm_uuid.ToString(), CurrentEvent = VerisenseBLEEvent.StateChange });
            }
        }

        protected void UartRX_ValueUpdated(object sender, ByteLevelCommunicationEvent comEvent)
        {
            if (comEvent.Event == ByteLevelCommunicationEvent.CommEvent.NewBytes)
            {
                byte[] bytes = comEvent.Bytes;
                var JCWatchEvent = JCWatchDataParser.DataParsingWithData(bytes);
                if (JCWatchEvent != null)
                {
                    if (ShimmerBLEEvent != null)
                    {
                        ShimmerBLEEvent.Invoke(null, new ShimmerBLEEventData { ASMID = Asm_uuid.ToString(), CurrentEvent = VerisenseBLEEvent.NewDataPacket, ObjMsg = JCWatchEvent });
                    }
                }
            }
            else if (comEvent.Event == ByteLevelCommunicationEvent.CommEvent.Disconnected)
            {
                StateChange(ShimmerDeviceBluetoothState.Disconnected);
            }
        }

        public ShimmerDeviceBluetoothState GetVerisenseBLEState()
        {
            return CurrentBluetoothState;
        }

        public static string MyDictionaryToJson(IDictionary<string, string> dict)
        {
            var x = dict.Select(d =>
                string.Format("\"{0}\": {1}", d.Key, string.Join(",", d.Value)));
            return "{" + string.Join(",", x) + "}";
        }
    }
}
