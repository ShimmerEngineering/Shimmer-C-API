using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ShimmerAPI.Radios
{
    public abstract class AbstractRadio
    {
        public enum RadioStatus
        {
            Connected,
            Disconnected,
            Connecting
        }

        protected RadioStatus CurrentRadioStatus = RadioStatus.Disconnected;

        public EventHandler<byte[]> BytesReceived;
        public EventHandler<RadioStatus> RadioStatusChanged;
        public abstract bool Connect();
        public abstract bool Disconnect();
        public abstract bool WriteBytes(byte[] bytes);

        protected long TestSignalTotalNumberOfBytes = 0;
        protected double TestSignalTSStart = 0;
        public bool TrackThroughput = true;
        protected double lastPrintDuration =0;
        protected double printEveryXSecond = 0;
        protected int count = 0;
        protected void SendBytesReceived(byte[] buffer)
        {
            if (TrackThroughput)
            {
                
                count++;
                TestSignalTotalNumberOfBytes += buffer.Length;
                if (TestSignalTSStart == 0)
                {
                    TestSignalTSStart = Environment.TickCount;
                }

                if (count % 100 == 0)
                {
                    double durationInS = (Environment.TickCount - TestSignalTSStart) / 1000;
                    if ((durationInS - lastPrintDuration) >= printEveryXSecond) //print every x second
                    {
                        Debug.WriteLine("Radio RX Throughput: " + TestSignalTotalNumberOfBytes / durationInS);
                        lastPrintDuration = durationInS;
                    }
                }
                

            }

            BytesReceived?.Invoke(this, buffer);
        }

    }
}
