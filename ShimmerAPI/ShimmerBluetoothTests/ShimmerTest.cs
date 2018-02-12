using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShimmerAPI;

namespace ShimmerBluetoothTests
{
    [TestClass]
    public class ShimmerTest
    {

        String comPort = "COM29";
        String deviceName = "testName";
        ShimmerLogAndStreamSystemSerialPort shimmerDevice;
        
        [TestMethod]
        public void TestMethodDeviceName()
        {
            shimmerDevice = new ShimmerLogAndStreamSystemSerialPort(deviceName, comPort);
            Assert.AreEqual(deviceName, shimmerDevice.GetDeviceName());
        }

    }
}
