using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShimmerAPI;

namespace ShimmerBluetoothTests
{
    [TestClass]
    public class ShimmerTest : ShimmerBluetooth
    {

        String comPort = "COM29";
        String deviceName = "testName";
        ShimmerLogAndStreamSystemSerialPort shimmerDevice;

        public ShimmerTest()
        {

        }
       
        [TestMethod]
        public void TestMethodDeviceName()
        {
            shimmerDevice = new ShimmerLogAndStreamSystemSerialPort(deviceName, comPort);
            Assert.AreEqual(deviceName, shimmerDevice.GetDeviceName());
        }

        [TestMethod]
        public void TestNudgeDouble()
        {
            Assert.AreEqual(NudgeDouble(1000, 0, 500), 500);
            System.Console.WriteLine(NudgeDouble(1000, 0, 500));
            Assert.AreEqual(NudgeDouble(-10, 10, 500), 10);
            System.Console.WriteLine(NudgeDouble(-10, 10, 500));
            Assert.AreEqual(NudgeDouble(50, 10, 500), 50);
            System.Console.WriteLine(NudgeDouble(50, 10, 500));
        }

        [TestMethod]
        public void TestNudgeGsrResistance()
        {
            /*
              {8.0, 63.0}, 		//Range 0
			{63.0, 220.0}, 		//Range 1
			{220.0, 680.0}, 	//Range 2
			{680.0, 4700.0}}; 	//Range 3
            */
            Assert.AreEqual(NudgeGsrResistance(7,0),8);
            Assert.AreEqual(NudgeGsrResistance(70, 0),63);
            Assert.AreEqual(NudgeGsrResistance(59, 1), 63);
            Assert.AreEqual(NudgeGsrResistance(90, 1), 90);
            Assert.AreEqual(NudgeGsrResistance(230, 1), 220);
            Assert.AreEqual(NudgeGsrResistance(230, 2), 230);
            Assert.AreEqual(NudgeGsrResistance(200, 2), 220);
            Assert.AreEqual(NudgeGsrResistance(690, 3), 690);
            Assert.AreEqual(NudgeGsrResistance(5000, 3), 4700);
        }



        public override string GetShimmerAddress()
        {
            throw new NotImplementedException();
        }

        public override void SetShimmerAddress(string address)
        {
            throw new NotImplementedException();
        }
        
        protected override void CloseConnection()
        {
            throw new NotImplementedException();
        }

        protected override void FlushConnection()
        {
            throw new NotImplementedException();
        }

        protected override void FlushInputConnection()
        {
            throw new NotImplementedException();
        }

        protected override bool IsConnectionOpen()
        {
            throw new NotImplementedException();
        }

        protected override void OpenConnection()
        {
            throw new NotImplementedException();
        }

        protected override int ReadByte()
        {
            throw new NotImplementedException();
        }

        protected override void WriteBytes(byte[] b, int index, int length)
        {
            throw new NotImplementedException();
        }
    }
}
