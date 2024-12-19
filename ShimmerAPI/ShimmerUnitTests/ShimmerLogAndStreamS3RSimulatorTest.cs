using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShimmerAPI.Simulators;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShimmerBluetoothTests
{
    [TestClass]
    public class ShimmerLogAndStreamS3RSimulatorTest
    {
        ShimmerLogAndStreamS3RSimulator mDevice;
        String ComPort = "COM99";

        [TestInitialize]
        public void SetUp()
        {
            mDevice = new ShimmerLogAndStreamS3RSimulator("", ComPort);
        }

        [TestMethod]
        public void Test001_testConnectandDisconnect()
        {
            //Comment out test as it is not completed
            /* 
            if (mDevice != null)
            {
                try
                {
                    mDevice.StartConnectThread();
                    Thread.Sleep(30000);
                    if (!mDevice.IsConnected())
                    {
                        Assert.Fail();
                    }

                    if (!mDevice.GetFirmwareVersionFullName().Equals("LogAndStream 0.0.1"))
                    {
                        Assert.Fail();
                    }

                    if (!mDevice.GetShimmerVersion().Equals(10))    //Shimmer3R
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
            */
        }
    }
}
