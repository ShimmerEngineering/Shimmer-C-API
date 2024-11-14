using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShimmerAPI.Protocols;
using ShimmerAPI.Simulators;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShimmerBluetoothTests
{
    [TestClass]
    public class ShimmerLogAndStreamS3RSimulatorTest
    {
        ShimmerLogAndStreamS3RSimulator mDevice;
        String ComPort = "COM99";
        ConcurrentQueue<byte> cq = new ConcurrentQueue<byte>();
        TaskCompletionSource<bool> mConnectTask;

        [TestInitialize]
        public void SetUp()
        {
            mDevice = new ShimmerLogAndStreamS3RSimulator("", ComPort);
            mConnectTask = new TaskCompletionSource<bool>();
        }

        [TestMethod]
        public void Test001_testConnectandDisconnect()
        {
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

                    if (!mDevice.GetShimmerVersion().Equals(10))
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
