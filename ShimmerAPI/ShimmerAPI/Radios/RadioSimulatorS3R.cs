using System;
using System.Collections.Generic;
using System.Text;

namespace ShimmerAPI.Radios
{
    public class RadioSimulatorS3R : RadioSimulatorS3
    {
        public RadioSimulatorS3R(String address) : base(address)
        {
        }

    protected void TxShimmerVersion()
        {
            mBuffer.Add((byte)0xff);
            mBuffer.Add((byte)0x25);
            mBuffer.Add((byte)0x0A);
        }

    protected void TxFirmwareVersion()
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
    }
}
