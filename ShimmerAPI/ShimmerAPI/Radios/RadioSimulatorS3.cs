using ShimmerAPI.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ShimmerAPI.Radios
{
    public class RadioSimulatorS3 : AbstractRadio
    {

        protected BlockingCollection<byte> mBuffer = new BlockingCollection<byte>(1000); // Fixed size 1000

        public class ShimmerObject 
        {
            public const byte GET_SAMPLING_RATE_COMMAND = 0x03;
            public const byte GET_SHIMMER_VERSION_COMMAND_NEW = (byte)0x3F;
            public const byte GET_VBATT_COMMAND = (byte)0x95;
            public const byte SET_CRC_COMMAND = (byte)0x8B;
            public const byte GET_FW_VERSION_COMMAND = (byte)0x2E;
            public const byte GET_DAUGHTER_CARD_ID_COMMAND = (byte)0x66;
            public const byte GET_INFOMEM_COMMAND = (byte)0x8E;
            public const byte GET_BMP280_CALIBRATION_COEFFICIENTS_COMMAND = (byte)0xA0;
            public const byte GET_BLINK_LED = (byte)0x32;
            public const byte GET_STATUS_COMMAND = (byte)0x72;
            public const byte INQUIRY_COMMAND = (byte)0x01;
            public const byte GET_BT_FW_VERSION_STR_COMMAND = (byte)0xA1;
            public const byte GET_CALIB_DUMP_COMMAND = (byte)0x9A;
            public const byte SET_RWC_COMMAND = (byte)0x8F;
        }

        public RadioSimulatorS3(String address)
        {
            // TODO Auto-generated constructor stub
        }

        protected void TxShimmerVersion()
        {
            mBuffer.Add(0xff);
            mBuffer.Add(0x25);
            mBuffer.Add(0x03);
        }

        protected void TxFirmwareVersion()
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

        public override bool Connect()
        {
            throw new NotImplementedException();
        }

        public override bool Disconnect()
        {
            throw new NotImplementedException();
        }

        public byte[] ReadBytes(int byteCount, int timeout)
        {
            if (byteCount <= 0)
            {
                throw new ArgumentException("Number of bytes to read must be positive.");
            }
            byte[] result = new byte[byteCount];

            for (int index = 0; index < byteCount; index++)
            {
                try
                {
                    result[index] = mBuffer.Take();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }

            return result;
        }

        public override bool WriteBytes(byte[] buffer)
        {
            if (buffer[0] == ShimmerObject.GET_SHIMMER_VERSION_COMMAND_NEW)
            {
                Console.WriteLine(UtilShimmer.BytesToHexString(buffer));
                TxShimmerVersion();
            }
            else if (buffer[0] == ShimmerObject.GET_VBATT_COMMAND)
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
            else if (buffer[0] == ShimmerObject.GET_SAMPLING_RATE_COMMAND)
            {
                mBuffer.Add((byte)0xff);
                mBuffer.Add((byte)0x04);
                mBuffer.Add((byte)0x80);
                mBuffer.Add((byte)0x02);
            }
            else if (buffer[0] == ShimmerObject.SET_CRC_COMMAND)
            {
                mBuffer.Add((byte)0xff);
                mBuffer.Add((byte)0x04);
                mBuffer.Add((byte)0xf4);
            }
            else if (buffer[0] == ShimmerObject.GET_FW_VERSION_COMMAND)
            {
                TxFirmwareVersion();
            }
            else if (buffer[0] == ShimmerObject.GET_DAUGHTER_CARD_ID_COMMAND)
            {
                mBuffer.Add((byte)0xff);
                mBuffer.Add((byte)0x65);
                mBuffer.Add((byte)0x03);
                mBuffer.Add((byte)0x30);
                mBuffer.Add((byte)0x04);
                mBuffer.Add((byte)0x02);
            }
            else if (buffer[0] == ShimmerObject.GET_INFOMEM_COMMAND)
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
            else if (buffer[0] == ShimmerObject.GET_BMP280_CALIBRATION_COEFFICIENTS_COMMAND)
            {
                mBuffer.Add((byte)0xff);
                mBuffer.Add((byte)0x9f);
                byte[] bytes = UtilShimmer.HexStringToByteArray("7A6A0D6632007F9016D7D00BBC1B2AFFF9FF8C3CF8C67017");
                foreach (byte byteValue in bytes)
                {
                    mBuffer.Add(byteValue);
                }
                mBuffer.Add((byte)0xDF);
            }
            else if (buffer[0] == ShimmerObject.GET_BLINK_LED)
            {
                mBuffer.Add((byte)0xff);
                mBuffer.Add((byte)0x31);
                mBuffer.Add((byte)0x01);
                mBuffer.Add((byte)0xE0);
            }
            else if (buffer[0] == ShimmerObject.GET_STATUS_COMMAND)
            {
                mBuffer.Add((byte)0xff);
                mBuffer.Add((byte)0x8A);
                mBuffer.Add((byte)0x71);
                mBuffer.Add((byte)0x21);
                mBuffer.Add((byte)0xF4);
            }
            else if (buffer[0] == ShimmerObject.INQUIRY_COMMAND)
            {
                mBuffer.Add((byte)0xff);
                mBuffer.Add((byte)0x02);
                byte[] bytes = UtilShimmer.HexStringToByteArray("800202FF01080001");
                foreach (byte byteValue in bytes)
                {
                    mBuffer.Add(byteValue);
                }
                mBuffer.Add((byte)0x86);
            }
            else if (buffer[0] == ShimmerObject.GET_BT_FW_VERSION_STR_COMMAND)
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
            else if (buffer[0] == ShimmerObject.GET_CALIB_DUMP_COMMAND)
            {
                if (buffer[1] == (byte)0x80 && buffer[2] == (byte)0x00 && buffer[3] == (byte)0x00) //[0x9A 0x80 0x00 0x00]
                {
                    mBuffer.Add((byte)0xff);
                    mBuffer.Add((byte)0x99);
                    byte[] bytes = UtilShimmer.HexStringToByteArray("8000005201030002000000160702000015BE0FB95C7E330000083C081F0825005300530054019C009C0100FEFF9C1E0000150000000000000000000000000000332C332C332C009C009C000000009C1E0001150000000000000000FDC00474FFC219D31957133C009CFF9C0100FE009C1E00021500000000000000000000000000000C");
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
                    byte[] bytes = UtilShimmer.HexStringToByteArray("808000D00CD00CD0009C009C000000009C1E0003150000000000000000000000000000066806680668009C009C000000009C1F00001539D4942779330000FFF5018CFD240683067F0692FF9C00640000F8FE9C1F000115000000000000000000000000000000D100D100D1009C0064000000009C1F0002150000000000000000FFEB01");
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
                    byte[] bytes = UtilShimmer.HexStringToByteArray("5400015AFDF8034203410348009C0064FF00FC009C1F000315000000000000000000000000000001A201A201A2009C0064000000009C200000150000000000000000002AFBEDFC21010700E500FD009C0064000000009C");
                    foreach (byte byteValue in bytes)
                    {
                        mBuffer.Add(byteValue);
                    }
                    mBuffer.Add((byte)0x6A);
                }
            }
            else if (buffer[0] == ShimmerObject.SET_RWC_COMMAND)
            {
                mBuffer.Add((byte)0xff);
                mBuffer.Add((byte)0xf4);
            }
            else
            {

                Console.WriteLine("Unresolved: " + UtilShimmer.BytesToHexString(buffer));
            }

            return true;
        }
    }
}
