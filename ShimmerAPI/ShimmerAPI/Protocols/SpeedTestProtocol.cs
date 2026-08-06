using ShimmerAPI.Radios;
using ShimmerAPI.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using static ShimmerAPI.Radios.AbstractRadio;

namespace ShimmerAPI.Protocols
{
    public class SpeedTestProtocol
    {
        public EventHandler<String> ResultUpdate;
        AbstractRadio Radio;
        protected byte[] OldTestData = new byte[0];
        protected bool TestFirstByteReceived = false;
        protected long TestSignalTotalNumberOfBytes = 0;
        protected long TestSignalTotalEffectiveNumberOfBytes = 0;
        protected long NumberofBytesDropped = 0;
        protected long NumberofNumbersSkipped = 0;
        protected double TestSignalTSStart = 0;
        protected bool TestSignalEnabled = false;
        protected bool ProcessData = false;

        ConcurrentQueue<byte> cq = new ConcurrentQueue<byte>();

        readonly byte[] ShimmerStopTestSignalCommand = new byte[2] { (byte)0xA4, (byte)0x00 };
        readonly byte[] ShimmerStartTestSignalCommand = new byte[2] { (byte)0xA4, (byte)0x01 };
        readonly byte[] VerisenseStopTestSignalCommand = new byte[5] { (byte)0x29, (byte)0x02, (byte)0x00, (byte)0x15, (byte)0x00};
        readonly byte[] VerisenseStartTestSignalCommand = new byte[5] { (byte)0x29, (byte)0x02, (byte)0x00, (byte)0x15, (byte)0x01};
        protected byte[] StopTestSignalCommand;
        protected byte[] StartTestSignalCommand;

        public SpeedTestProtocol(AbstractRadio radio)
        {
            Radio = radio;
            if (Radio.getDeviceType().Equals(DeviceType.Verisense)){
                StartTestSignalCommand = VerisenseStartTestSignalCommand;
                StopTestSignalCommand = VerisenseStopTestSignalCommand;
            } else
            {
                StopTestSignalCommand = ShimmerStopTestSignalCommand;
                StartTestSignalCommand = ShimmerStartTestSignalCommand;
            }
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
            TestSignalTotalEffectiveNumberOfBytes = 0;
            NumberofBytesDropped = 0;
            System.Console.WriteLine("Stop Test Signal");
            TestSignalTSStart = (DateTime.UtcNow - ShimmerBluetooth.UnixEpoch).TotalMilliseconds;
            if (Radio.WriteBytes(StopTestSignalCommand))
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
            if (Radio.WriteBytes(StartTestSignalCommand))
            {
                TestSignalEnabled = true;
            }
        }

        private void ProcessDataPackets()
        {
            ProcessData = true;
            int lengthOfPacket = 5;
            int keepValue = 0;
            while (ProcessData)
            {

                if (TestSignalEnabled)
                {
                    int qSize = cq.Count();
                    if (qSize > 0)
                    {
                        byte[] buffer = ProgrammerUtilities.DequeueBytes(cq, qSize);
                        if (!TestFirstByteReceived)
                        {
                            Console.WriteLine("DISCARD BYTE");
                            TestFirstByteReceived = true;
                            ProgrammerUtilities.CopyAndRemoveBytes(ref buffer, 1);

                        }

                        TestSignalTotalNumberOfBytes += buffer.Length;
                        /*
                        Console.WriteLine();
                        Debug.WriteLine(ProgrammerUtilities.ByteArrayToHexString(buffer));
                        */

                        byte[] data = OldTestData.Concat(buffer).ToArray();
                        //byte[] data = newdata;
                        double testSignalCurrentTime = (DateTime.UtcNow - ShimmerBluetooth.UnixEpoch).TotalMilliseconds;
                        double duration = (testSignalCurrentTime - TestSignalTSStart) / 1000.0; //make it seconds
                        //Console.WriteLine("Throughput (bytes per second): " + (TestSignalTotalNumberOfBytes / duration));
                        //Console.WriteLine("RXB OTD:" + BitConverter.ToString(OldTestData).Replace("-", ""));
                        //Console.WriteLine("RXB:" + BitConverter.ToString(data).Replace("-", ""));
                        int charPrintCount = 0;
                        while(data.Length >= lengthOfPacket+1)
                        {
                            if (data[0] == 0XA5 && data[5] == 0XA5)
                            {
                                byte[] bytesFullPacket = new byte[lengthOfPacket];
                                System.Array.Copy(data, 0, bytesFullPacket, 0, lengthOfPacket);
                                data = ProgrammerUtilities.RemoveBytesFromArray(data, lengthOfPacket);
                                if (bytesFullPacket[0] == 0xA5)
                                {
                                    TestSignalTotalEffectiveNumberOfBytes += 5;
                                    //Array.Reverse(bytes);
                                    byte[] bytes = new byte[lengthOfPacket - 1];
                                    System.Array.Copy(bytesFullPacket, 1, bytes, 0, bytes.Length);
                                    int intValue = BitConverter.ToInt32(bytes, 0);

                                    if (keepValue != 0)
                                    {
                                        var difference = intValue - keepValue;
                                        if ((difference) != 1)
                                        {
                                            NumberofNumbersSkipped += difference;
                                        }
                                    }

                                    keepValue = intValue;
                                   
                                    var intValueString = intValue.ToString();
                                    Console.Write(intValueString + " , ");
                                    charPrintCount+= intValueString.Length;
                                    if (charPrintCount % 120 == 0)
                                    {
                                        Console.WriteLine();
                                    }
                                }
                            } else
                            {
                                data = ProgrammerUtilities.RemoveBytesFromArray(data, 1);
                                NumberofBytesDropped++;
                            }
                        }
                        testSignalCurrentTime = (DateTime.UtcNow - ShimmerBluetooth.UnixEpoch).TotalMilliseconds;
                        duration = (testSignalCurrentTime - TestSignalTSStart) / 1000.0; //make it seconds
                        Console.WriteLine();
                        String result = "Effective Throughput (bytes per second): " + (TestSignalTotalEffectiveNumberOfBytes / duration) + ", Number of Bytes Dropped: " + NumberofBytesDropped + ", Numbers Skipped: " + NumberofNumbersSkipped + ", (Duration S): " + duration + "";
                        Console.WriteLine(result);
                        ResultUpdate?.Invoke(this, result);
                        OldTestData = data;
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

        

    }
}
