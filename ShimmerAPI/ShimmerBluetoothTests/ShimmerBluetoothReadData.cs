using ShimmerAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShimmerBluetoothTests
{
    class ObjectClusterByteArray: ObjectCluster
    {
        public List<byte> packet;
        public ObjectClusterByteArray(String comPort, String shimmerId):base(comPort, shimmerId)
        {
            
        }
    }

    class ShimmerBluetoothReadData : ShimmerBluetooth
    {
        private bool throwException = false;
        public int byteDataIndex = -1;
        public byte[] data = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        int numberOfPackets = 0;
        public void enableReadTimeoutException(bool exception )
        {
            throwException = exception;
        }

        public ShimmerBluetoothReadData(String name) : base(name)
        {
          
        }

        public void start()
        {
            StopReading = false; //read data thread continue
            IsFilled = true; //inquiry done
            SetState(ShimmerBluetooth.SHIMMER_STATE_STREAMING);
            PacketSize = 10;
            ReadThread = new Thread(new ThreadStart(ReadData));
            ReadThread.Name = "Read Thread for Device: " + DeviceName;
            ReadThread.Start();
        }

        public void stop()
        {
            StopReading = true;
        }

        protected override ObjectCluster BuildMsg(List<byte> packet)
        {
            ObjectClusterByteArray ojc = new ObjectClusterByteArray("","");
            ojc.packet = packet;
            return ojc;
        }

        public override string GetShimmerAddress()
        {
            return "testDeviceAddress";
        }

        public override void SetShimmerAddress(string address)
        {
        }

        protected override void CloseConnection()
        {
            
        }

        protected override void FlushConnection()
        {
            
        }

        protected override void FlushInputConnection()
        {

        }

        protected override bool IsConnectionOpen()
        {
            return true;
        }

        protected override void OpenConnection()
        {
        }

        protected override int ReadByte()
        {
            byteDataIndex++;
            if (data.Length == byteDataIndex)
            {
                byteDataIndex = 0;
                numberOfPackets++;
            }
            if (throwException)
            {
                if (numberOfPackets==5 && byteDataIndex==4)
                {
                    throw new TimeoutException();
                }
            }
            return data[byteDataIndex];
        }

        protected override void WriteBytes(byte[] b, int index, int length)
        {
        }
        
    }
}
