using System;
using System.IO.Ports;

namespace ShimmerAPI
{

    public class ShimmerLogAndStreamSystemSerialPort : ShimmerLogAndStream
    {
        protected String ComPort;
        public System.IO.Ports.SerialPort SerialPort = new System.IO.Ports.SerialPort();

        public ShimmerLogAndStreamSystemSerialPort(String devID, String bComPort)
            : base(devID)
        {
            ComPort = bComPort;
        }

        public ShimmerLogAndStreamSystemSerialPort(String devName, String bComPort, double samplingRate, int setEnabledSensors, byte[] exg1configuration, byte[] exg2configuration)
            : base(devName,samplingRate, setEnabledSensors, exg1configuration, exg2configuration)
        {
            ComPort = bComPort;
        }

        public ShimmerLogAndStreamSystemSerialPort(String devName, String bComPort, double samplingRate, int accelRange, int gsrRange, int setEnabledSensors, bool enableLowPowerAccel, bool enableLowPowerGyro, bool enableLowPowerMag, int gyroRange, int magRange, byte[] exg1configuration, byte[] exg2configuration, bool internalexppower)
            :base(devName, samplingRate, accelRange, gsrRange, setEnabledSensors, enableLowPowerAccel, enableLowPowerGyro, enableLowPowerMag, gyroRange, magRange, exg1configuration, exg2configuration, internalexppower)
        {
            ComPort = bComPort;
        }

        public ShimmerLogAndStreamSystemSerialPort(String devID, String bComPort, double samplingRate, int AccelRange, int GyroRange, int gsrRange, int setEnabledSensors)
            : base(devID, samplingRate, AccelRange, GyroRange, gsrRange, setEnabledSensors)
        {
            ComPort = bComPort;
        }

        protected override bool IsConnectionOpen()
        {
            //SerialPort.PortName = ComPort;
            return SerialPort.IsOpen;
        }

        protected override void CloseConnection()
        {
            SerialPort.Close();
        }
        protected override void FlushConnection()
        {
            SerialPort.DiscardInBuffer();
            SerialPort.DiscardOutBuffer();
        }
        protected override void FlushInputConnection()
        {
            try
            {
                SerialPort.ReadExisting();
                SerialPort.DiscardInBuffer();
            }
            catch
            {
            }

        }
        protected override void WriteBytes(byte[] b, int index, int length)
        {
            if (GetState() != SHIMMER_STATE_NONE)
            {
                try
                {
                    SerialPort.Write(b, index, length);
                } catch (Exception)
                {
                    CustomEventArgs newEventArgs = new CustomEventArgs((int)ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE, "Connection lost");
                    OnNewEvent(newEventArgs);
                    Disconnect();
                }
            }
        }

        protected override int ReadByte()
        {   
            if (GetState() != SHIMMER_STATE_NONE)
            {
                return SerialPort.ReadByte();
            }
            throw new InvalidOperationException();
        }

        protected override void OpenConnection()
        {
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

        public override string GetShimmerAddress()
        {
            return ComPort;
        }

        public override void SetShimmerAddress(string address)
        {
            ComPort = address;
        }
    }
}
