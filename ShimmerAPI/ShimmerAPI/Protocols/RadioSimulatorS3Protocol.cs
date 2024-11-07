using NUnit.Framework;
using ShimmerAPI.Radios;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ShimmerAPI.Protocols
{
    public class RadioSimulatorS3Protocol
    {
        SerialPortRadio mDevice;
        String ComPort = "COM99";
        ConcurrentQueue<byte> cq = new ConcurrentQueue<byte>();

        [SetUp]
        public void SetUp()
        {
            mDevice = new SerialPortRadio(ComPort);
            mDevice.SetTestRadio(new RadioSimulatorS3(ComPort));
        }

        [Test]
        public void Test001_testConnectandDisconnect()
        {
            bool isConnected = mDevice.Connect();
            if (!isConnected)
            {
                Assert.False(false);
            }
            else
            {
                Assert.True(true);
            }
        }

    }
}
