using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShimmerAPI.Radios;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ShimmerAPI.Protocols
{
    [TestClass]
    public class Shimmer3RadioSimulatorTest
    {
        ShimmerLogAndStreamSimulator mDevice;
        String ComPort = "COM99";
        ConcurrentQueue<byte> cq = new ConcurrentQueue<byte>();

        [TestInitialize]
        public void SetUp()
        {
            mDevice = new ShimmerLogAndStreamSimulator("", ComPort);
            mDevice.SetTestRadio(new RadioSimulatorS3(ComPort));
        }

        [TestMethod]
        public void Test001_testConnectandDisconnect()
        {
            if (mDevice != null)
            {
                try
                {
                    mDevice.OpenConnection2();

                    if (!mDevice.IsConnectionOpen2())
                    {
                        Assert.Fail();
                    }

                    if (!mDevice.GetFirmwareVersionFullName().Equals("LogAndStream v0.16.9"))
                    {
                        Assert.Fail();
                    }

                    if (!mDevice.GetShimmerVersion().Equals(3))
                    {
                        Assert.Fail();
                    }
                }
                catch (Exception ex)
                {
                    Assert.Fail($"Test aborted due to exception: {ex.Message}");
                }
            }
            else
            {
                Assert.Fail("mDevice is null");
            }
            
        }


    }
}
