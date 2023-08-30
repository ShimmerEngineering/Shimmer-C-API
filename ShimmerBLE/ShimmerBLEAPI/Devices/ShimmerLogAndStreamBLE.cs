using shimmer.Communications;
using ShimmerAPI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShimmerBLEAPI.Devices
{
    public class ShimmerLogAndStreamBLE : ShimmerLogAndStream
    {
        protected IVerisenseByteCommunication BLERadio;
        BlockingCollection<int> Buffer = new BlockingCollection<int>(2048);
        public Guid Asm_uuid { get; set; }
        public ShimmerLogAndStreamBLE(string devID) : base(devID)
        {
            Asm_uuid = Guid.Parse(devID);
        }

        public override string GetShimmerAddress()
        {
            throw new NotImplementedException();
        }

        public override void SetShimmerAddress(string address)
        {
            throw new NotImplementedException();
        }

        protected override void CloseConnection()
        {
            throw new NotImplementedException();
        }

        protected override void FlushConnection()
        {
            throw new NotImplementedException();
        }

        protected override void FlushInputConnection()
        {
            throw new NotImplementedException();
        }

        protected override bool IsConnectionOpen()
        {
            if (BLERadio.GetConnectivityState().Equals(ConnectivityState.Connected))
            {
                return true;
            }
            return false;
        }

        protected void UartRX_ValueUpdated(object sender, ByteLevelCommunicationEvent comEvent)
        {
            if (comEvent.Event == ByteLevelCommunicationEvent.CommEvent.NewBytes)
            {
                byte[] bytes = comEvent.Bytes;
                for (int i = 0; i < bytes.Length; i++)
                {
                    Buffer.Add(bytes[i]);
                }
            }
        }

        protected override void OpenConnection()
        {
            if (BLERadio != null) { 
                BLERadio.CommunicationEvent -= UartRX_ValueUpdated;      
            }

            BLERadio = new RadioPluginBLE();

            BLERadio.Asm_uuid = Asm_uuid;
            BLERadio.CommunicationEvent += UartRX_ValueUpdated;

            Task<ConnectivityState> taskconnect = BLERadio.Connect();
            taskconnect.Wait();
        }

        protected override int ReadByte()
        {
            return (int)Buffer.Take();
        }

        protected override void WriteBytes(byte[] b, int index, int length)
        {
            BLERadio.WriteBytes(b);
        }
    }
}
