using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using ShimmerAPI.Radios;

namespace ShimmerAPI.Protocols
{
    public class ShimmerLogAndStreamSimulator : ShimmerLogAndStream
    {
        public string ComPort;
        public AbstractRadio mAbstractRadio;
        private bool mTesting = false;

        public ShimmerLogAndStreamSimulator(string devID, string bComPort)
    : base(devID)
        {
            ComPort = bComPort;
        }

        public ShimmerLogAndStreamSimulator(string devName, string bComPort, double samplingRate, int setEnabledSensors, byte[] exg1configuration, byte[] exg2configuration)
            : base(devName, samplingRate, setEnabledSensors, exg1configuration, exg2configuration)
        {
            ComPort = bComPort;
        }

        public ShimmerLogAndStreamSimulator(string devName, string bComPort, double samplingRate, int accelRange, int gsrRange, int setEnabledSensors, bool enableLowPowerAccel, bool enableLowPowerGyro, bool enableLowPowerMag, int gyroRange, int magRange, byte[] exg1configuration, byte[] exg2configuration, bool internalexppower)
            : base(devName, samplingRate, accelRange, gsrRange, setEnabledSensors, enableLowPowerAccel, enableLowPowerGyro, enableLowPowerMag, gyroRange, magRange, exg1configuration, exg2configuration, internalexppower)
        {
            ComPort = bComPort;
        }
        public ShimmerLogAndStreamSimulator(string devName, string bComPort, double samplingRate, int accelRange, int gsrRange, int setEnabledSensors, bool enableLowPowerAccel, bool enableLowPowerGyro, bool enableLowPowerMag, int gyroRange, int magRange, byte[] exg1configuration, byte[] exg2configuration, bool internalexppower, BTCRCMode CRCmode)
            : base(devName, samplingRate, accelRange, gsrRange, setEnabledSensors, enableLowPowerAccel, enableLowPowerGyro, enableLowPowerMag, gyroRange, magRange, exg1configuration, exg2configuration, internalexppower, CRCmode)
        {
            ComPort = bComPort;
        }

        public ShimmerLogAndStreamSimulator(string devID, string bComPort, double samplingRate, int AccelRange, int GyroRange, int gsrRange, int setEnabledSensors)
            : base(devID, samplingRate, AccelRange, GyroRange, gsrRange, setEnabledSensors)
        {
            ComPort = bComPort;
        }

        public void SetTestRadio(AbstractRadio radio)
        {
            mTesting = true;
            mAbstractRadio = radio;
        }

        public override string GetShimmerAddress()
        {
            return ComPort;
        }

        public override void SetShimmerAddress(string address)
        {
            ComPort = address;
        }

        protected override void CloseConnection()
        {
            mAbstractRadio.Close();
        }

        protected override void FlushConnection()
        {
            mAbstractRadio.DiscardInBuffer();
            mAbstractRadio.DiscardOutBuffer();
        }

        protected override void FlushInputConnection()
        {
            try
            {
                mAbstractRadio.ReadExisting();
                mAbstractRadio.DiscardInBuffer();
            }
            catch
            {

            }
        }
        public bool IsConnectionOpen2()
        {
            return IsConnectionOpen();
        }

        protected override bool IsConnectionOpen()
        {
            return mAbstractRadio.isOpen();
        }

        public void OpenConnection2()
        {
            OpenConnection();
        }

        protected override void OpenConnection()
        {
            //mAbstractRadio.BaudRate = 115200;
            //mAbstractRadio.PortName = ComPort;
            //mAbstractRadio.ReadTimeout = this.ReadTimeout;
            //mAbstractRadio.WriteTimeout = this.WriteTimeout;
            SetState(SHIMMER_STATE_CONNECTING);
            if (mTesting || mAbstractRadio == null)
            {
                if (!mTesting)
                {
                    mAbstractRadio = new AbstractRadioJSSC(ComPort, ReadTimeout, WriteTimeout);
                }
            }
            try
            {
                mAbstractRadio.Open();
            }
            catch
            {
            }
            mAbstractRadio.DiscardInBuffer();
            mAbstractRadio.DiscardOutBuffer();
        }

        protected override int ReadByte()
        {
            if (GetState() != SHIMMER_STATE_NONE)
            {
                return mAbstractRadio.ReadByte();
            }
            throw new InvalidOperationException();
        }

        protected override void WriteBytes(byte[] b, int index, int length)
        {
            if (GetState() != SHIMMER_STATE_NONE)
            {
                try
                {
                    mAbstractRadio.Write(b, index, length);
                }
                catch (Exception)
                {
                    CustomEventArgs newEventArgs = new CustomEventArgs((int)ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE, "Connection lost");
                    OnNewEvent(newEventArgs);
                    Disconnect();
                }
            }
        }
    }
}
