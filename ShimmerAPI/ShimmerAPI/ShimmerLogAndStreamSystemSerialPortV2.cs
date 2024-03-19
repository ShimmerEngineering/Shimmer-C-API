using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace ShimmerAPI
{
    public class ShimmerLogAndStreamSystemSerialPortV2 : ShimmerLogAndStreamSystemSerialPort, IDisposable
    {
        private MemoryStream RXMemoryStream = null;
        private BinaryReader RXBinaryReader = null;
        private int NumberofBytesToRead;
        private int IndexPosition;
        private bool ReadRequired = true;
        private bool Terminate = false;
        Stopwatch ReadStopWatch = new Stopwatch();

        public ShimmerLogAndStreamSystemSerialPortV2(string devID, string bComPort) : base(devID, bComPort)
        {

        }

        
        protected override int ReadByte()
        {
            if (GetState() != SHIMMER_STATE_NONE)
            {
                if (ReadRequired)
                {
                    while (true)
                    {
                        NumberofBytesToRead = SerialPort.BytesToRead;
                        if (NumberofBytesToRead > 0)
                        {
                            ReadStopWatch.Restart();
                            if (RXMemoryStream != null)
                                RXMemoryStream.Dispose();
                            if (RXBinaryReader != null)
                                RXBinaryReader.Dispose();

                            byte[] buffer = new byte[NumberofBytesToRead];
                            SerialPort.Read(buffer, 0, NumberofBytesToRead);
                            ReadRequired = false;
                            RXMemoryStream = new MemoryStream(buffer);
                            RXBinaryReader = new BinaryReader(RXMemoryStream);
                            IndexPosition = 0;
                            break;
                        }
                        else
                        {
                            if (ReadStopWatch.ElapsedMilliseconds > ReadTimeout) //If no bytes have been read throw an exception
                            {
                                ReadStopWatch.Restart();
                                throw new System.TimeoutException();
                            }
                        }
                        Thread.Sleep(10);
                        if (Terminate) Dispose();
                    }
                }
                byte ret = RXBinaryReader.ReadByte();
                IndexPosition++;
                if (IndexPosition == NumberofBytesToRead)
                    ReadRequired = true;
                return ret;
            }
            throw new InvalidOperationException();
        }

        public void Dispose()
        {
            if (RXMemoryStream != null)
                RXMemoryStream.Dispose();
            if (RXBinaryReader != null)
                RXBinaryReader.Dispose();
            if (SerialPort.IsOpen)
                SerialPort.Close();
        }

        public void CloseHandle()
        {
            this.Terminate = true;
        }

        protected override void OpenConnection()
        {
            RXMemoryStream = null;
            RXBinaryReader = null;
            NumberofBytesToRead = 0;
            IndexPosition = 0 ;
            ReadRequired = true;
            Terminate = false;
            ReadStopWatch = new Stopwatch();

            base.OpenConnection();
        }

    }

}
