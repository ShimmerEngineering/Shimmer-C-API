using ShimmerAPI.Radios;
using ShimmerAPI.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace ShimmerAPI.Protocols
{
    public class SpeedTestProtocol
    {
        AbstractRadio Radio;
        protected byte[] OldTestData = new byte[0];
        protected bool TestFirstByteReceived = false;
        protected long TestSignalTotalNumberOfBytes = 0;
        protected double TestSignalTSStart = 0;
        protected bool TestSignalEnabled = false;
        protected bool ProcessData = false;

        ConcurrentQueue<byte> cq = new ConcurrentQueue<byte>();

        public SpeedTestProtocol(AbstractRadio radio)
        {
            Radio = radio;
            Radio.BytesReceived += Radio_BytesReceived;
        }

        public void Connect()
        {
            Radio.Connect();
        }

        public void Disconnect()
        {
            Radio.Disconnect();
        }

        public void StopTestSignal()
        {
            OldTestData = new byte[0];
            TestFirstByteReceived = false;
            TestSignalTotalNumberOfBytes = 0;
            System.Console.WriteLine("Stop Test Signal");
            TestSignalTSStart = (DateTime.UtcNow - ShimmerBluetooth.UnixEpoch).TotalMilliseconds;
            if (Radio.WriteBytes(new byte[2] { (byte)0xA4, (byte)0x00 }))
            {
                TestSignalEnabled = false;
            }
        }

        public void StartTestSignal()
        {
            Thread thread = new Thread(ProcessDataPackets);
            // Start the thread
            thread.Start();
            OldTestData = new byte[0];
            TestFirstByteReceived = false;
            TestSignalTotalNumberOfBytes = 0;
            System.Console.WriteLine("Start Test Signal");
            TestSignalTSStart = (DateTime.UtcNow - ShimmerBluetooth.UnixEpoch).TotalMilliseconds;
            if (Radio.WriteBytes(new byte[2] { (byte)0xA4, (byte)0x01 }))
            {
                TestSignalEnabled = true;
            }
        }

        private void ProcessDataPackets()
        {
            ProcessData = true;
            while (ProcessData)
            {

                if (TestSignalEnabled)
                {
                    int qSize = cq.Count();
                    if (qSize > 0)
                    {
                        byte[] buffer = DequeueBytes(cq, qSize);
                        if (!TestFirstByteReceived)
                        {
                            Console.WriteLine("DISCARD BYTE");
                            TestFirstByteReceived = true;
                            ProgrammerUtilities.CopyAndRemoveBytes(ref buffer, 1);

                        }

                        TestSignalTotalNumberOfBytes += buffer.Length;
                        Console.WriteLine();
                        Debug.WriteLine(ProgrammerUtilities.ByteArrayToHexString(buffer));
                        byte[] data = OldTestData.Concat(buffer).ToArray();
                        //byte[] data = newdata;
                        double testSignalCurrentTime = (DateTime.UtcNow - ShimmerBluetooth.UnixEpoch).TotalMilliseconds;
                        double duration = (testSignalCurrentTime - TestSignalTSStart) / 1000.0; //make it seconds
                        Console.WriteLine("Throughput (bytes per second): " + (TestSignalTotalNumberOfBytes / duration));
                        //Console.WriteLine("RXB OTD:" + BitConverter.ToString(OldTestData).Replace("-", ""));
                        //Console.WriteLine("RXB:" + BitConverter.ToString(data).Replace("-", ""));
                        for (int i = 0; i < (data.Length / 4); i++)
                        {
                            byte[] bytes = new byte[4];
                            System.Array.Copy(data, i * 4, bytes, 0, 4);
                            //Array.Reverse(bytes);
                            int intValue = BitConverter.ToInt32(bytes, 0);
                            Console.Write(intValue + " , ");
                        }
                        Console.WriteLine();

                        int remainder = data.Length % 4;
                        if (remainder != 0)
                        {
                            OldTestData = new byte[remainder];
                            System.Array.Copy(data, data.Length - remainder, OldTestData, 0, remainder);
                        }
                        else
                        {
                            OldTestData = new byte[0];
                        }
                    }
                }
            }
        }

        private void Radio_BytesReceived(object sender, byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                cq.Enqueue(buffer[i]);
            }

        }

        static byte[] DequeueBytes(ConcurrentQueue<byte> queue, int count)
        {
            byte[] result = new byte[count];
            for (int i = 0; i < count; i++)
            {
                if (queue.TryDequeue(out byte dequeuedByte))
                {
                    result[i] = dequeuedByte;
                }
                else
                {
                    // Queue is empty before dequeuing the desired count of bytes
                    // You can handle this case based on your requirements
                    Array.Resize(ref result, i);
                    break;
                }
            }
            return result;
        }


    }
}
