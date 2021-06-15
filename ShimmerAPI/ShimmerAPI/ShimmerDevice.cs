using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ShimmerAPI.ShimmerBluetooth;

namespace ShimmerAPI
{
    public abstract class ShimmerDevice
    {
        protected double LastReceivedTimeStamp = 0;
        protected double CurrentTimeStampCycle = 0;
        protected double LastReceivedCalibratedTimeStamp = -1;
        protected double CalTimeStart;
        protected double SamplingRate;
        protected int TimeStampPacketByteSize = 2;
        protected int TimeStampPacketRawMaxValue = 65536;// 16777216 or 65536 
        protected int ADCRawSamplingRateValue;
        protected int HardwareVersion = 0;
        public Boolean FirstTimeCalTime = true;
        public long PacketLossCount = 0;
        public double PacketReceptionRate = 100;
        public EventHandler UICallback; //this is to be used by other classes to communicate with the c# API
        public enum ShimmerVersion
        {
            SHIMMER1 = 0,
            SHIMMER2 = 1,
            SHIMMER2R = 2,
            SHIMMER3 = 3
        }
        protected double CalibrateTimeStamp(double timeStamp)
        {
            //first convert to continuous time stamp
            double calibratedTimeStamp = 0;
            if (LastReceivedTimeStamp > (timeStamp + (TimeStampPacketRawMaxValue * CurrentTimeStampCycle)))
            {
                CurrentTimeStampCycle = CurrentTimeStampCycle + 1;
            }

            LastReceivedTimeStamp = (timeStamp + (TimeStampPacketRawMaxValue * CurrentTimeStampCycle));

            double clockConstant = 1024;
            if (HardwareVersion == (int)ShimmerVersion.SHIMMER2R || HardwareVersion == (int)ShimmerVersion.SHIMMER2)
            {
                clockConstant = 1024;
            }
            else if (HardwareVersion == (int)ShimmerVersion.SHIMMER3)
            {
                clockConstant = 32768;
            }

            calibratedTimeStamp = LastReceivedTimeStamp / clockConstant * 1000;   // to convert into mS
            if (FirstTimeCalTime)
            {
                FirstTimeCalTime = false;
                CalTimeStart = calibratedTimeStamp;
            }
            if (LastReceivedCalibratedTimeStamp != -1)
            {
                double timeDifference = calibratedTimeStamp - LastReceivedCalibratedTimeStamp;
                double expectedTimeDifference = (1 / SamplingRate) * 1000; //in ms
                double adjustedETD = expectedTimeDifference + (expectedTimeDifference * 0.1);

                //if (timeDifference > (1 / ((clockConstant / ADCRawSamplingRateValue) - 1)) * 1000)
                if (timeDifference > adjustedETD)
                {
                    //calculate the estimated packet loss within that time period
                    int numberOfLostPackets = ((int)Math.Ceiling(timeDifference / expectedTimeDifference)) - 1;
                    PacketLossCount = PacketLossCount + numberOfLostPackets;
                    //PacketLossCount = PacketLossCount + 1;
                    long mTotalNumberofPackets = (long)((calibratedTimeStamp - CalTimeStart) / (1 / (clockConstant / ADCRawSamplingRateValue) * 1000));
                    mTotalNumberofPackets = (long)((calibratedTimeStamp - CalTimeStart) / expectedTimeDifference);

                    PacketReceptionRate = (double)((mTotalNumberofPackets - PacketLossCount) / (double)mTotalNumberofPackets) * 100;

                    if (PacketReceptionRate < 99)
                    {
                        //System.Console.WriteLine("PRR: " + PacketReceptionRate);
                    }
                }
            }

            EventHandler handler = UICallback;
            if (handler != null)
            {
                CustomEventArgs newEventArgs = new CustomEventArgs((int)ShimmerIdentifier.MSG_IDENTIFIER_PACKET_RECEPTION_RATE, (object)GetPacketReceptionRate());
                handler(this, newEventArgs);
            }

            LastReceivedCalibratedTimeStamp = calibratedTimeStamp;
            return calibratedTimeStamp - CalTimeStart; // make it start at zero
        }
        public double GetPacketReceptionRate()
        {
            return PacketReceptionRate;
        }

    }
}
