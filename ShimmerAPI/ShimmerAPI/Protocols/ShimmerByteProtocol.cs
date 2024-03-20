using ShimmerAPI.Radios;
using ShimmerAPI.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using static ShimmerAPI.ShimmerBluetooth;

namespace ShimmerAPI.Protocols
{
    public class ShimmerByteProtocol
    {
        AbstractRadio Radio;
        ConcurrentQueue<byte> cq = new ConcurrentQueue<byte>();
        bool ProcessData = false;
        public ShimmerByteProtocol(AbstractRadio radio)
        {
            Radio = radio;
            Radio.BytesReceived += Radio_BytesReceived;
        }

        private void Radio_BytesReceived(object sender, byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                cq.Enqueue(buffer[i]);
            }

        }
        public void Connect()
        {
            Thread thread = new Thread(ProcessDataPackets);
            // Start the thread
            thread.Start();
            Radio.Connect();
        }

        private void ProcessDataPackets()
        {
            ProcessData = true;
            while (ProcessData)
            {
                int qSize = cq.Count();
                if (qSize > 0)
                {
                    byte[] buffer = ProgrammerUtilities.DequeueBytes(cq, qSize);
                    Debug.WriteLine(ProgrammerUtilities.ByteArrayToHexString(buffer));
                }
                Thread.Sleep(5);
            }
        }

        public void Disconnect()
        {
            Radio.Disconnect();
        }

        public void Inquiry()
        {
            Radio.WriteBytes(new byte[1] { (byte)PacketTypeShimmer2.INQUIRY_COMMAND });
            System.Threading.Thread.Sleep(200);
        }

        public virtual void StartStreaming()
        {  
            Radio.WriteBytes(new byte[1] { (byte)PacketTypeShimmer2.START_STREAMING_COMMAND });
        }

        public virtual void StopStreaming()
        {
            Radio.WriteBytes(new byte[1] { (byte)PacketTypeShimmer2.STOP_STREAMING_COMMAND });
        }

    }
}
