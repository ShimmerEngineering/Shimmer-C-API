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
        ShimmerLogAndStreamSimulator mDevice;
        String ComPort = "COM99";
        ConcurrentQueue<byte> cq = new ConcurrentQueue<byte>();

        [SetUp]
        public void SetUp()
        {
            mDevice = new ShimmerLogAndStreamSimulator("", ComPort);
            mDevice.SetTestRadio(new RadioSimulatorS3(ComPort));
        }

        [Test]
        public void Test001_testConnectandDisconnect()
        {
            try
            {
                mDevice.OpenConnection2();

                // Check device connection
                Assert.IsTrue(mDevice.IsConnectionOpen2(), "Device should be connected.");

                // Check firmware version
                Assert.AreSame("LogAndStream v0.16.9", mDevice.GetFirmwareVersionFullName(), "Firmware version should be 'LogAndStream v0.16.9'.");

                // Check hardware version
                Assert.AreSame(3, mDevice.GetShimmerVersion(), "Hardware version should be SHIMMER_3.");

            }
            catch (Exception ex)
            {
                Assert.Fail($"Test aborted due to exception: {ex.Message}");
            }
        }


    }
}
