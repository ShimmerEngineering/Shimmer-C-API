using ShimmerAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace ShimmerAPI.Radios
{

    public class SerialPortRadio : AbstractRadio
    {
        public System.IO.Ports.SerialPort SerialPort = new System.IO.Ports.SerialPort();
        protected String ComPort;
        public int ReadTimeout = 1000; //ms
        public int WriteTimeout = 1000; //ms
        public SerialPortRadio(String comPort){
            ComPort = comPort;
        }

        public override bool Connect()
        {
            SerialPort.BaudRate = 115200;
            SerialPort.PortName = ComPort;
            SerialPort.ReadTimeout = this.ReadTimeout;
            SerialPort.WriteTimeout = this.WriteTimeout;
            
            try
            {
                SerialPort.Open();
            }
            catch (Exception ex)
            {
                return false;
            }
            SerialPort.DiscardInBuffer();
            SerialPort.DiscardOutBuffer();
            Thread thread = new Thread(ReadData);
            // Start the thread
            thread.Start();
            return true;
        }

        public override bool Disconnect()
        {
            throw new NotImplementedException();
        }

        public override bool WriteBytes(byte[] bytes)
        {
            try
            {
                SerialPort.Write(bytes, 0, bytes.Length);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        protected byte[] OldTestData = new byte[0];
        protected bool TestFirstByteReceived = false;
        protected long TestSignalTotalNumberOfBytes = 0;
        protected double TestSignalTSStart = 0;
        protected bool TestSignalEnabled = false;

        public void StartTestSignal()
        {
            OldTestData = new byte[0];
            TestFirstByteReceived = false;
            TestSignalTotalNumberOfBytes = 0;
            System.Console.WriteLine("Start Test Signal");
            TestSignalTSStart = (DateTime.UtcNow - ShimmerBluetooth.UnixEpoch).TotalMilliseconds;
            if (WriteBytes(new byte[2] { (byte)0xA4, (byte)0x01 }))
            {
                TestSignalEnabled = true;
            }
        }

        protected void ReadData()
        {
            while(true)
            {
                int NumberofBytesToRead = SerialPort.BytesToRead;
                if (NumberofBytesToRead > 0)
                {
                    byte[] buffer = new byte[NumberofBytesToRead];
                    SerialPort.Read(buffer, 0, NumberofBytesToRead);
                    if (TestSignalEnabled)
                    {
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



                    Thread.Sleep(1); // Simulate some work
                }
            }
        }
    }
}
