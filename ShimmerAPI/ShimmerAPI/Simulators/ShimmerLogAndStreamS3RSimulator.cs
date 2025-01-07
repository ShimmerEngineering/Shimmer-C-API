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
    }

}
