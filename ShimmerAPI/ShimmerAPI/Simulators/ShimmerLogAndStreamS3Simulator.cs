using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using ShimmerAPI.Radios;
using ShimmerAPI.Utilities;
using static com.shimmerresearch.radioprotocol.LiteProtocolInstructionSet.Types;

namespace ShimmerAPI.Simulators
{
    public class ShimmerLogAndStreamS3Simulator : ShimmerLogAndStream
    {
        public string ComPort;
        public AbstractRadio mAbstractRadio;
        TaskCompletionSource<bool> mConnectTask = new TaskCompletionSource<bool>();
        public bool isConnectionOpen = false;
        protected BlockingCollection<byte> mBuffer = new BlockingCollection<byte>(1000); // Fixed size 1000

        public bool isGetBmp390CalibrationCoefficientsCommand = false;
        public bool isGetPressureCalibrationCoefficientsCommand = false;
        public bool mIsNewBMPSupported;

        public ShimmerLogAndStreamS3Simulator(string devID, string bComPort)
    : base(devID)
        {
            ComPort = bComPort;
        }

        public ShimmerLogAndStreamS3Simulator(string devName, string bComPort, double samplingRate, int setEnabledSensors, byte[] exg1configuration, byte[] exg2configuration)
            : base(devName, samplingRate, setEnabledSensors, exg1configuration, exg2configuration)
        {
            ComPort = bComPort;
        }

        public ShimmerLogAndStreamS3Simulator(string devName, string bComPort, double samplingRate, int accelRange, int gsrRange, int setEnabledSensors, bool enableLowPowerAccel, bool enableLowPowerGyro, bool enableLowPowerMag, int gyroRange, int magRange, byte[] exg1configuration, byte[] exg2configuration, bool internalexppower)
            : base(devName, samplingRate, accelRange, gsrRange, setEnabledSensors, enableLowPowerAccel, enableLowPowerGyro, enableLowPowerMag, gyroRange, magRange, exg1configuration, exg2configuration, internalexppower)
        {
            ComPort = bComPort;
        }
        public ShimmerLogAndStreamS3Simulator(string devName, string bComPort, double samplingRate, int accelRange, int gsrRange, int setEnabledSensors, bool enableLowPowerAccel, bool enableLowPowerGyro, bool enableLowPowerMag, int gyroRange, int magRange, byte[] exg1configuration, byte[] exg2configuration, bool internalexppower, BTCRCMode CRCmode)
            : base(devName, samplingRate, accelRange, gsrRange, setEnabledSensors, enableLowPowerAccel, enableLowPowerGyro, enableLowPowerMag, gyroRange, magRange, exg1configuration, exg2configuration, internalexppower, CRCmode)
        {
            ComPort = bComPort;
        }

        public ShimmerLogAndStreamS3Simulator(string devID, string bComPort, double samplingRate, int AccelRange, int GyroRange, int gsrRange, int setEnabledSensors)
            : base(devID, samplingRate, AccelRange, GyroRange, gsrRange, setEnabledSensors)
        {
            ComPort = bComPort;
        }
        public void SetIsNewBMPSupported(bool isNewBMPSupported)
        {
            mIsNewBMPSupported = isNewBMPSupported;
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
        }

        protected override void FlushConnection()
        {
        }

        protected override void FlushInputConnection()
        {
        }

        protected override bool IsConnectionOpen()
        {
            return isConnectionOpen;

        }

        protected override void OpenConnection()
        {
            isConnectionOpen = true;
        }

        protected override int ReadByte()
        {
            return mBuffer.Take();
        }
        protected virtual void TxShimmerVersion()
        {
            mBuffer.Add(0xff);
            mBuffer.Add(0x25);
            mBuffer.Add(0x03);
        }

        protected virtual void TxFirmwareVersion()
        {
            mBuffer.Add(0xff);
            mBuffer.Add(0x2f);
            mBuffer.Add(0x03);
            mBuffer.Add(0x00);
            mBuffer.Add(0x00);
            mBuffer.Add(0x00);
            mBuffer.Add(0x10);
            mBuffer.Add(0x09);
        }
        protected override void WriteBytes(byte[] buffer, int index, int length)
        {

            if (buffer[0] == (byte)PacketTypeShimmer3.GET_SHIMMER_VERSION_COMMAND)
            {
                Console.WriteLine(UtilShimmer.BytesToHexString(buffer));
                TxShimmerVersion();
            }
            else if (buffer[0] == (byte)PacketTypeShimmer3.GET_VBATT_COMMAND)
            {
                mBuffer.Add((byte)0xff);
                mBuffer.Add((byte)0x8A);
                mBuffer.Add((byte)0x94);
                byte[] bytes = UtilShimmer.HexStringToByteArray("240B80");
                foreach (byte byteValue in bytes)
                {
                    mBuffer.Add(byteValue);
                }
                mBuffer.Add((byte)0x61);
            }
            else if (buffer[0] == (byte)PacketTypeShimmer2.GET_SAMPLING_RATE_COMMAND)
            {
                mBuffer.Add((byte)0xff);
                mBuffer.Add((byte)0x04);
                mBuffer.Add((byte)0x80);
                mBuffer.Add((byte)0x02);
            }
            else if (buffer[0] == (byte)PacketTypeShimmer2.GET_FW_VERSION_COMMAND)
            {
                TxFirmwareVersion();
            }
            else if (buffer[0] == (byte)PacketTypeShimmer3.GET_EXPANSION_BOARD_COMMAND)
            {
                mBuffer.Add((byte)0xff);
                mBuffer.Add((byte)0x65);
                mBuffer.Add((byte)0x03);
                mBuffer.Add((byte)0x30);
                mBuffer.Add((byte)0x04);
                mBuffer.Add((byte)0x02);
            }
            else if (buffer[0] == (byte)PacketTypeShimmer3SDBT.GET_INFOMEM_COMMAND)
            {
                if (buffer[1] == (byte)0x80 && buffer[2] == (byte)0x00 && buffer[3] == (byte)0x00) //0x8E 0x80 0x00 0x00
                {
                    mBuffer.Add((byte)0xff);
                    mBuffer.Add((byte)0x8D);
                    mBuffer.Add((byte)0x80);
                    byte[] bytes = UtilShimmer.HexStringToByteArray("800201E010004D9B0E08008010000000000002010080100000000000020109000000083C081F0825005300530054019C009C0100FEFF9C0000000000000CD00CD00CD0009C009C000000009C002AFBEDFC21010700E500FD009C0064000000009C00000000000001A201A201A2009C0064000000009C0000000000FFFFFFFFFF");
                    foreach (byte byteValue in bytes)
                    {
                        mBuffer.Add(byteValue);
                    }
                    mBuffer.Add((byte)0x8c);
                }
                else if (buffer[1] == (byte)0x80 && buffer[2] == (byte)0x80 && buffer[3] == (byte)0x00) //0x8E 0x80 0x80 0x00
                {
                    mBuffer.Add((byte)0xff);
                    mBuffer.Add((byte)0x8D);
                    mBuffer.Add((byte)0x80);
                    byte[] bytes = UtilShimmer.HexStringToByteArray("00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000005368696D6D65725F33453336574354455354FFFFFFFFFFFF670773A70000B9003600000000E8EB1B713E360000000000000000000000000000000000000000000000000000");
                    foreach (byte byteValue in bytes)
                    {
                        mBuffer.Add(byteValue);
                    }

                    mBuffer.Add((byte)0x32);
                }
                else if (buffer[1] == (byte)0x80 && buffer[2] == (byte)0x00 && buffer[3] == (byte)0x01) //[0x8E 0x80 0x00 0x01]
                {
                    mBuffer.Add((byte)0xff);
                    mBuffer.Add((byte)0x8D);
                    mBuffer.Add((byte)0x80);
                    byte[] bytes = UtilShimmer.HexStringToByteArray("0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                    foreach (byte byteValue in bytes)
                    {
                        mBuffer.Add(byteValue);
                    }

                    mBuffer.Add((byte)0x9B);
                }
            }
            else if (buffer[0] == (byte)PacketTypeShimmer3RSDBT.GET_PRESSURE_CALIBRATION_COEFFICIENTS_COMMAND)
            {
                isGetPressureCalibrationCoefficientsCommand = true;
                mBuffer.Add((byte)0xff);
                mBuffer.Add((byte)0xa6);
                byte[] bytes = UtilShimmer.HexStringToByteArray("19011D6BBA643200859289D6D00BC918CBFFF9FF7B1A1FEE4DFC");
                foreach (byte byteValue in bytes)
                {
                    mBuffer.Add(byteValue);
                }
                mBuffer.Add((byte)0xDF);
            }
            else if (buffer[0] == (byte)PacketTypeShimmer2.GET_BLINK_LED)
            {
                mBuffer.Add((byte)0xff);
                mBuffer.Add((byte)0x31);
                mBuffer.Add((byte)0x01);
                mBuffer.Add((byte)0xE0);
            }
            else if (buffer[0] == (byte)PacketTypeShimmer3SDBT.GET_STATUS_COMMAND)
            {
                mBuffer.Add((byte)0xff);
                mBuffer.Add((byte)0x8A);
                mBuffer.Add((byte)0x71);
                mBuffer.Add((byte)0x21);
                mBuffer.Add((byte)0xF4);
            }
            else if (buffer[0] == (byte)PacketTypeShimmer2.INQUIRY_COMMAND)
            {
                mBuffer.Add((byte)0xff);
                mBuffer.Add((byte)0x02);
                byte[] bytes;
                if (GetShimmerVersion() == (int)ShimmerBluetooth.ShimmerVersion.SHIMMER3R)
                {
                    bytes = UtilShimmer.HexStringToByteArray("800202FF01080000000001");
                }
                else
                {
                    bytes = UtilShimmer.HexStringToByteArray("800202FF01080001");
                }
                foreach (byte byteValue in bytes)
                {
                    mBuffer.Add(byteValue);
                }
                mBuffer.Add((byte)0x86);
            }
            else if (buffer[0] == (byte)PacketTypeShimmer3SDBT.GET_BT_FW_VERSION_STR_COMMAND)
            {
                mBuffer.Add((byte)0xff);
                mBuffer.Add((byte)0xa2);
                mBuffer.Add((byte)0x35);
                byte[] bytes = UtilShimmer.HexStringToByteArray("524E343637382056312E30302E352031312F31352F32303136202863294D6963726F6368697020546563686E6F6C6F677920496E63");
                foreach (byte byteValue in bytes)
                {
                    mBuffer.Add(byteValue);
                }
                mBuffer.Add((byte)0x79);
            }
            else if (buffer[0] == (byte)PacketTypeShimmer3SDBT.GET_CALIB_DUMP_COMMAND)
            {
                if (buffer[1] == (byte)0x80 && buffer[2] == (byte)0x00 && buffer[3] == (byte)0x00) //[0x9A 0x80 0x00 0x00]
                {
                    mBuffer.Add((byte)0xff);
                    mBuffer.Add((byte)0x99);
                    byte[] bytes = UtilShimmer.HexStringToByteArray("1802030003000000100B0200001520902D20D23300000805083907F9005300530054009C039C00FFF9019C1E0000150000000000000000000000000000332C332C332C009C009C000000009C1E0001154B57A224D23300000043FFCDFEEA19301911198A039CFF9DF50409049D1E00021500000000000000000000000000000C");
                    foreach (byte byteValue in bytes)
                    {
                        mBuffer.Add(byteValue);
                    }
                    mBuffer.Add((byte)0xB5);
                }
                else if (buffer[1] == (byte)0x80 && buffer[2] == (byte)0x80 && buffer[3] == (byte)0x00) //[0x9A 0x80 0x80 0x00]
                {
                    mBuffer.Add((byte)0xff);
                    mBuffer.Add((byte)0x99);
                    byte[] bytes = UtilShimmer.HexStringToByteArray("D00CD00CD0009C009C000000009C1E0003150000000000000000000000000000066806680668009C009C000000009C1F00001533B3A220D2330000FFF60047F7C7064B067606729CFFFFFF64FDFA019C1F0001150000000000000000000000000000032F032F032F9C000000640000009C1F0002150000000000000000000000");
                    foreach (byte byteValue in bytes)
                    {
                        mBuffer.Add(byteValue);
                    }
                    mBuffer.Add((byte)0xFA);
                }
                else if (buffer[1] == (byte)0x54 && buffer[2] == (byte)0x00 && buffer[3] == (byte)0x01) //[0x9A 0x54 0x00 0x01]
                {
                    mBuffer.Add((byte)0xff);
                    mBuffer.Add((byte)0x99);
                    byte[] bytes = UtilShimmer.HexStringToByteArray("0000000198019801989C000000640000009C1F00031500000000000000000000000000000087008700879C000000640000009C20000115F5980C0000000000000000000000044C044C03D49C000000640000009C2000021500000000000000000000000000000357035702F89C000000640000009C2000031500000000000000");
                    foreach (byte byteValue in bytes)
                    {
                        mBuffer.Add(byteValue);
                    }
                    mBuffer.Add((byte)0x6A);
                }
            }
            else
            {

                Console.WriteLine("Unresolved: " + UtilShimmer.BytesToHexString(buffer));
            }

        }

        protected override void readInfoMem()
        {
            //string status_text = "";
            //PChangeStatusLabel("Inquiring Shimmer Info"); // need to be in a UI thread for update
            SetDataReceived(false);

            // btsd changes 2
            string status_text = "Acquiring Accelerometer Range...";
            CustomEventArgs newEventArgs = new CustomEventArgs((int)ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE, (object)status_text);
            OnNewEvent(newEventArgs);
            //PControlForm.status_text = "Acquiring Accelerometer Range...";
            //worker.ReportProgress(15, status_text);
            ReadAccelRange();

            if (HardwareVersion == (int)ShimmerBluetooth.ShimmerVersion.SHIMMER3R)
            {
                ReadLNAccelRange();
            }

            status_text = "Acquiring ADC Sampling Rate...";
            newEventArgs = new CustomEventArgs((int)ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE, (object)status_text);
            OnNewEvent(newEventArgs);
            //PControlForm.status_text = "Acquiring ADC Sampling Rate...";
            //worker.ReportProgress(20, status_text);
            ReadSamplingRate();

            status_text = "Acquiring Magnetometer Range...";
            newEventArgs = new CustomEventArgs((int)ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE, (object)status_text);
            OnNewEvent(newEventArgs);
            //PControlForm.status_text = "Acquiring Magnetometer Range...";
            //worker.ReportProgress(25, status_text);
            ReadMagRange();

            status_text = "Acquiring Gyro Range...";
            newEventArgs = new CustomEventArgs((int)ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE, (object)status_text);
            OnNewEvent(newEventArgs);
            //PControlForm.status_text = "Acquiring Gyro Range...";
            //worker.ReportProgress(30, status_text);
            ReadGyroRange();

            status_text = "Acquiring Accel Sampling Rate...";
            newEventArgs = new CustomEventArgs((int)ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE, (object)status_text);
            OnNewEvent(newEventArgs);
            //PControlForm.status_text = "Acquiring Accel Sampling Rate...";
            //worker.ReportProgress(35, status_text);
            ReadAccelSamplingRate();

            status_text = "Acquiring Calibration settings...";
            //PControlForm.status_text = "Acquiring Calibration settings...";
            //worker.ReportProgress(40, status_text);
            if (HardwareVersion == (int)ShimmerBluetooth.ShimmerVersion.SHIMMER3R)
            {
                ReadCalibDump();
                //ReadCalibrationParameters("All");
            }
            else
            {
                ReadCalibDump();
                //ReadCalibrationParameters("All");
            }

            status_text = "Acquiring EXG1 configure settings...";
            newEventArgs = new CustomEventArgs((int)ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE, (object)status_text);
            OnNewEvent(newEventArgs);
            //PControlForm.status_text = "Acquiring EXG1 configure settings...";
            //worker.ReportProgress(45, status_text);
            ReadEXGConfigurations(1);

            status_text = "Acquiring EXG2 configure settings...";
            newEventArgs = new CustomEventArgs((int)ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE, (object)status_text);
            OnNewEvent(newEventArgs);
            //PControlForm.status_text = "Acquiring EXG2 configure settings...";
            //worker.ReportProgress(50, status_text);
            ReadEXGConfigurations(2);

            //There is an inquiry below so no need for this
            status_text = "Acquiring Internal Exp Power setting...";
            newEventArgs = new CustomEventArgs((int)ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE, (object)status_text);
            OnNewEvent(newEventArgs);
            ReadInternalExpPower();

            status_text = "Acquiring trial configure settings...";
            newEventArgs = new CustomEventArgs((int)ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE, (object)status_text);
            OnNewEvent(newEventArgs);
            //PControlForm.status_text = "Acquiring trial configure settings...";
            //worker.ReportProgress(55, status_text);
            WriteBytes(new byte[1] { (byte)ShimmerBluetooth.PacketTypeShimmer3SDBT.GET_TRIAL_CONFIG_COMMAND }, 0, 1);
            waitTilTimeOut();

            status_text = "Acquiring center name...";
            newEventArgs = new CustomEventArgs((int)ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE, (object)status_text);
            OnNewEvent(newEventArgs);
            //PControlForm.status_text = "Acquiring center name...";
            //worker.ReportProgress(60, status_text);
            WriteBytes(new byte[1] { (byte)ShimmerBluetooth.PacketTypeShimmer3SDBT.GET_CENTER_COMMAND }, 0, 1);
            waitTilTimeOut();

            status_text = "Acquiring shimmer name...";
            newEventArgs = new CustomEventArgs((int)ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE, (object)status_text);
            OnNewEvent(newEventArgs);
            //PControlForm.status_text = "Acquiring shimmer name...";
            //worker.ReportProgress(65, status_text);
            WriteBytes(new byte[1] { (byte)ShimmerBluetooth.PacketTypeShimmer3SDBT.GET_SHIMMERNAME_COMMAND }, 0, 1);
            waitTilTimeOut();

            status_text = "Acquiring experiment ID...";
            newEventArgs = new CustomEventArgs((int)ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE, (object)status_text);
            OnNewEvent(newEventArgs);
            //PControlForm.status_text = "Acquiring experiment ID...";
            //worker.ReportProgress(70, status_text);
            WriteBytes(new byte[1] { (byte)ShimmerBluetooth.PacketTypeShimmer3SDBT.GET_EXPID_COMMAND }, 0, 1);
            waitTilTimeOut();

            status_text = "Acquiring Multi Shimmer settings...";
            newEventArgs = new CustomEventArgs((int)ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE, (object)status_text);
            OnNewEvent(newEventArgs);
            //PControlForm.status_text = "Acquiring Multi Shimmer settings...";
            //worker.ReportProgress(75, status_text);
            WriteBytes(new byte[1] { (byte)ShimmerBluetooth.PacketTypeShimmer3SDBT.GET_MYID_COMMAND }, 0, 1);
            waitTilTimeOut();

            WriteBytes(new byte[1] { (byte)ShimmerBluetooth.PacketTypeShimmer3SDBT.GET_NSHIMMER_COMMAND }, 0, 1);
            waitTilTimeOut();

            status_text = "Acquiring configure time...";
            newEventArgs = new CustomEventArgs((int)ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE, (object)status_text);
            OnNewEvent(newEventArgs);
            //PControlForm.status_text = "Acquiring configure time...";
            //worker.ReportProgress(80, status_text);
            WriteBytes(new byte[1] { (byte)ShimmerBluetooth.PacketTypeShimmer3SDBT.GET_CONFIGTIME_COMMAND }, 0, 1);
            waitTilTimeOut();

            status_text = "Acquiring general settings...";
            newEventArgs = new CustomEventArgs((int)ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE, (object)status_text);
            OnNewEvent(newEventArgs);
            //PControlForm.status_text = "Acquiring general settings...";
            //worker.ReportProgress(85, status_text);
            Inquiry();
        }

    }
}
