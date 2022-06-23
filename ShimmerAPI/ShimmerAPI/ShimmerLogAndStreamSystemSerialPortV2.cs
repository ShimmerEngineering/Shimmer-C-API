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
        private MemoryStream ms = null;
        private BinaryReader br = null;
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
                            if (ms != null)
                                ms.Dispose();
                            if (br != null)
                                br.Dispose();

                            byte[] buffer = new byte[NumberofBytesToRead];
                            SerialPort.Read(buffer, 0, NumberofBytesToRead);
                            ReadRequired = false;
                            ms = new MemoryStream(buffer);
                            br = new BinaryReader(ms);
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
                byte ret = br.ReadByte();
                IndexPosition++;
                if (IndexPosition == NumberofBytesToRead)
                    ReadRequired = true;
                return ret;
            }
            throw new InvalidOperationException();
        }

        public void Dispose()
        {
            if (ms != null)
                ms.Dispose();
            if (br != null)
                br.Dispose();
            if (SerialPort.IsOpen)
                SerialPort.Close();
        }

        public void CloseHandle()
        {
            this.Terminate = true;
        }

        protected override void OpenConnection()
        {
            MemoryStream ms = null;
            BinaryReader br = null;
            NumberofBytesToRead = 0;
            IndexPosition = 0 ;
            ReadRequired = true;
            Terminate = false;
            ReadStopWatch = new Stopwatch();

            SerialPort.BaudRate = 115200;
            SerialPort.PortName = ComPort;
            SerialPort.ReadTimeout = this.ReadTimeout;
            SerialPort.WriteTimeout = this.WriteTimeout;
            SetState(SHIMMER_STATE_CONNECTING);
            try
            {
                SerialPort.Open();
            }
            catch
            {
            }
            SerialPort.DiscardInBuffer();
            SerialPort.DiscardOutBuffer();
        }

    }

}
