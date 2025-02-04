using ShimmerAPI.Protocols;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShimmerAPI.Simulators
{
    public class ShimmerLogAndStreamS3RSimulator : ShimmerLogAndStreamS3Simulator
    {
        public ShimmerLogAndStreamS3RSimulator(string devID, string bComPort) : base(devID, bComPort)
        {
        }

        protected override void TxShimmerVersion()
        {
            mBuffer.Add((byte)0xff);
            mBuffer.Add((byte)0x25);
            mBuffer.Add((byte)0x0A);
        }

        protected override void TxFirmwareVersion()
        {
            mBuffer.Add((byte)0xff);
            mBuffer.Add((byte)0x2f);
            mBuffer.Add((byte)0x03);
            mBuffer.Add((byte)0x00);
            mBuffer.Add((byte)0x00);
            mBuffer.Add((byte)0x00);
            mBuffer.Add((byte)0x00);
            mBuffer.Add((byte)0x01);
        }

        public byte[] GetPressureResoTest()
        {
            byte[] pressureResoResTest =
            {
                (byte) 0xE7, (byte) 0x6B, (byte) 0xF0, (byte) 0x4A, (byte) 0xF9,
                (byte) 0xAB, (byte) 0x1C, (byte) 0x9B, (byte) 0x15, (byte) 0x06,
                (byte) 0x01, (byte) 0xD2, (byte) 0x49, (byte) 0x18, (byte) 0x5F,
                (byte) 0x03, (byte) 0xFA, (byte) 0x3A, (byte) 0x0F, (byte) 0x07,
                (byte) 0xF5
            };
            return pressureResoResTest;
        }

        public byte[] GetTestDataPacket()
        {
            byte[] newPacket = {
                188, 19, 112, 203, 7, 9, 8, 190, 4, 24, 251, 7, 2, 240, 186,
                (byte)0x00, (byte)0xCF, (byte)0x7F, (byte)0x00, (byte)0x17, (byte)0x64,
                128, 186, 181, 80, 4, 169, 40, 128, 127, 255, 255, 253, 80, 71
            };
            return newPacket;
        }

        public String[] GetTestDataType()
        {
            String[] dataType = { "u24", "i16", "i16", "i16", "i16", "i16", "i16", "u24", "u24", "u8", "i24r", "i24r", "u8", "i24r", "i24r", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };
            return dataType;
        }
    }

}
