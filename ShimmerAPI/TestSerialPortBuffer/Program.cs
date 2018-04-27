using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ShimmerAPI.ShimmerBluetooth;

namespace TestSerialPortBuffer
{
    class Program
    {
        protected static double LastReceivedTimeStamp = 0;
        protected static double CurrentTimeStampCycle = 0;
        protected static double LastReceivedCalibratedTimeStamp = -1;
        protected static double TimeStampPacketRawMaxValue = 16777216;// 16777216 or 65536 
        protected static bool FirstTimeCalTime = true;
        protected static double CalTimeStart = 0;
        protected static double SamplingRate = 1024;
        protected static double lastKnownTS = 0;

        static ConcurrentQueue<byte[]> conque  = new ConcurrentQueue<byte[]>();
        static double packettslastknown = 0;
        static void Main(string[] args)
        {


            System.IO.Ports.SerialPort SerialPort = new System.IO.Ports.SerialPort();
            int packetSize = 23; //LN Accel + EXG TEST SIGNAL Using L&S0.11
            SerialPort.BaudRate = 115200;
            SerialPort.PortName = "COM11";
            SerialPort.ReadTimeout = 2000;
            SerialPort.WriteTimeout = 2000;
            SerialPort.ReadBufferSize = 2147483647;
            System.Console.WriteLine(SerialPort.ReadBufferSize);
            try
            {
                SerialPort.Open();
            }
            catch
            {

            }

            SerialPort.Write(new byte[1] { (byte)PacketTypeShimmer2.START_STREAMING_COMMAND }, 0, 1);
            SerialPort.ReadByte();
            int i = 0;
            System.Threading.Thread myThread;
            myThread = new System.Threading.Thread(new
             System.Threading.ThreadStart(processData));
            myThread.Start();
            while (true)
            {
                byte[] packet = new byte[23];
                if (SerialPort.BytesToRead >= 24)
                {
                    int b = SerialPort.ReadByte();
                    i++;
                    SerialPort.Read(packet, 0, 23);
                    conque.Enqueue(packet);

                   
                }
            }
        }

        public static void processData()
        {
            int i = 0;
            while (true)
            {   
                if (conque.IsEmpty)
                {
                    Thread.Sleep(1);
                } else
                {
                    byte[] packet = new byte[23];
                    conque.TryDequeue(out packet);
                    i++;
                    double packetts = CalibrateTimeStamp(parseTimeStamps(packet));
                    if (i % 1024 == 0)
                    {
                        //System.Console.WriteLine(packet[0] + "," + packet[1] + "," + packet[2] + "," + packet[3] + "," + SerialPort.BytesToRead);
                        System.Console.WriteLine(packetts + "," + (packetts - packettslastknown));
                    }
                    
                    packettslastknown = packetts;
                }
            }
        }

        public static double parseTimeStamps(byte[] data)
        {
            long xmsb = ((long)(data[0 + 2] & 0xFF) << 16);
            long msb = ((long)(data[0 + 1] & 0xFF) << 8);
            long lsb = ((long)(data[0 + 0] & 0xFF));
            double parsedts = xmsb + msb + lsb;
            return parsedts;
        }

        protected static double CalibrateTimeStamp(double timeStamp)
        {
            //first convert to continuous time stamp
            double calibratedTimeStamp = 0;
            if (LastReceivedTimeStamp > (timeStamp + (TimeStampPacketRawMaxValue * CurrentTimeStampCycle)))
            {
                CurrentTimeStampCycle = CurrentTimeStampCycle + 1;
            }

            LastReceivedTimeStamp = (timeStamp + (TimeStampPacketRawMaxValue * CurrentTimeStampCycle));
            calibratedTimeStamp = LastReceivedTimeStamp / 32768 * 1000;   // to convert into mS
            if (FirstTimeCalTime)
            {
                FirstTimeCalTime = false;
                CalTimeStart = calibratedTimeStamp;
            }
            if (LastReceivedCalibratedTimeStamp != -1)
            {
                double timeDifference = calibratedTimeStamp - LastReceivedCalibratedTimeStamp;
                double clockConstant = 32768;

                double expectedTimeDifference = (1 / SamplingRate) * 1000; //in ms
                double adjustedETD = expectedTimeDifference + (expectedTimeDifference * 0.1);

            }

            LastReceivedCalibratedTimeStamp = calibratedTimeStamp;
            return calibratedTimeStamp - CalTimeStart; // make it start at zero
        }
    }
}
