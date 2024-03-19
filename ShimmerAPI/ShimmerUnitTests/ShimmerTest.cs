using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShimmerAPI;
using ShimmerAPI.Utilities;

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
        public void TestCRCTrue()
        {
            byte[] testPacket = new byte[] {
                (byte) 0x00,
                (byte) 0xb6, (byte) 0xf8, (byte) 0xbb,
                (byte) 0xff, (byte) 0x80, (byte) 0x00, (byte) 0x01, (byte) 0x80, (byte) 0x00, (byte) 0x01,
                (byte) 0xff, (byte) 0x80, (byte) 0x00, (byte) 0x01, (byte) 0x80, (byte) 0x00, (byte) 0x01,
                (byte) 0x8a, (byte) 0x93
            };
            byte[] crc = ShimmerUartCrcCalc(testPacket, testPacket.Length - 2);
            Assert.IsTrue(ShimmerBluetooth.ShimmerUartCrcCheck(testPacket));

            testPacket = new byte[] {
                (byte) 0x00,
                (byte) 0x00, (byte) 0x00, (byte) 0x00,
                (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00,
                (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00,
                 231,  206
            };
            crc = ShimmerUartCrcCalc(testPacket, testPacket.Length - 2);
            Assert.IsTrue(ShimmerBluetooth.ShimmerUartCrcCheck(testPacket));
        }

        [TestMethod]
        public void TestCRCFalse()
        {
            byte[] testPacket = new byte[] {
                (byte) 0x00,
                (byte) 0xb6, (byte) 0xf8, (byte) 0xbb,
                (byte) 0xff, (byte) 0x80, (byte) 0x00, (byte) 0x01, (byte) 0x80, (byte) 0x00, (byte) 0x01,
                (byte) 0xff, (byte) 0x80, (byte) 0x00, (byte) 0x01, (byte) 0x80, (byte) 0x00, (byte) 0x01,
                (byte) 0x8a, (byte) 0x94
            };
            Assert.IsFalse(ShimmerBluetooth.ShimmerUartCrcCheck(testPacket));
        }

        [TestMethod]
        public void TestMethodDeviceName()
        {
            shimmerDevice = new ShimmerLogAndStreamSystemSerialPort(deviceName, comPort);
            Assert.AreEqual(deviceName, shimmerDevice.GetDeviceName());
        }

        [TestMethod]
        public void CopyAndRemoveBytes_ShouldCopyCorrectNumberOfBytesAndRemoveFromSourceArray()
        {
            // Arrange
            byte[] sourceArray = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
            byte[] expectedCopiedArray = { 0x01, 0x02, 0x03, 0x04 };
            byte[] expectedSourceArray = { 0x05, 0x06, 0x07, 0x08 };

            int bytesToCopy = 4;

            // Act
            byte[] copiedArray = ProgrammerUtilities.CopyAndRemoveBytes(ref sourceArray, bytesToCopy);

            // Assert
            CollectionAssert.AreEqual(expectedCopiedArray, copiedArray, "Copied array does not match expected.");
            CollectionAssert.AreEqual(expectedSourceArray, sourceArray, "Source array after removal does not match expected.");
        }

        [TestMethod]
        public void AppendByteArrays_SuccessfullyAppendsArrays()
        {
            // Arrange
            byte[] array1 = new byte[] { 0x01, 0x02, 0x03 };
            byte[] array2 = new byte[] { 0x04, 0x05, 0x06 };
            byte[] expectedArray = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 };

            // Act
            byte[] combinedArray = ProgrammerUtilities.AppendByteArrays(array1, array2);

            // Assert
            CollectionAssert.AreEqual(expectedArray, combinedArray, "Arrays should be equal after appending.");
        }
        [TestMethod]
        public void AppendByteArrays_WithEmptyArray1_ReturnsArray2()
        {
            // Arrange
            byte[] array1 = new byte[0];
            byte[] array2 = new byte[] { 0x01, 0x02, 0x03 };

            // Act
            byte[] combinedArray = ProgrammerUtilities.AppendByteArrays(array1, array2);

            // Assert
            CollectionAssert.AreEqual(array2, combinedArray, "Combined array should be equal to array2.");
        }

        [TestMethod]
        public void AppendByteArrays_WithEmptyArray2_ReturnsArray1()
        {
            // Arrange
            byte[] array1 = new byte[] { 0x01, 0x02, 0x03 };
            byte[] array2 = new byte[0];

            // Act
            byte[] combinedArray = ProgrammerUtilities.AppendByteArrays(array1, array2);

            // Assert
            CollectionAssert.AreEqual(array1, combinedArray, "Combined array should be equal to array1.");
        }

        [TestMethod]
        public void AppendByteArrays_WithBothEmptyArrays_ReturnsEmptyArray()
        {
            // Arrange
            byte[] array1 = new byte[0];
            byte[] array2 = new byte[0];

            // Act
            byte[] combinedArray = ProgrammerUtilities.AppendByteArrays(array1, array2);

            // Assert
            CollectionAssert.AreEqual(array1, combinedArray, "Combined array should be an empty array.");
        }

        [TestMethod]
        public void RemoveLastBytes_RemovesCorrectNumberOfBytes()
        {
            // Arrange
            byte[] originalArray = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };
            int numberOfBytesToRemove = 2;
            byte[] expectedArray = new byte[] { 0x01, 0x02, 0x03 };

            // Act
            byte[] modifiedArray = ProgrammerUtilities.RemoveLastBytes(originalArray, numberOfBytesToRemove);

            // Assert
            CollectionAssert.AreEqual(expectedArray, modifiedArray, "Arrays should be equal after removing bytes.");
        }

        [TestMethod]
        public void RemoveLastBytes_RemovesAllBytes()
        {
            // Arrange
            byte[] originalArray = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };
            int numberOfBytesToRemove = 5;

            // Act
            byte[] modifiedArray = ProgrammerUtilities.RemoveLastBytes(originalArray, numberOfBytesToRemove);
            byte[] testExpectation = new byte[0];
            // Assert
            Assert.AreEqual(modifiedArray.Length, 0, "Modified array should be empty after removing all bytes.");
        }

        [TestMethod]
        public void RemoveLastBytes_TriesToRemoveMoreBytesThanArrayLength()
        {
            // Arrange
            byte[] originalArray = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };
            int numberOfBytesToRemove = 6;

            // Act
            byte[] modifiedArray = ProgrammerUtilities.RemoveLastBytes(originalArray, numberOfBytesToRemove);

            // Assert
            Assert.AreEqual(modifiedArray,null, "Modified array should be empty when trying to remove more bytes than the array length.");
        }

        [TestMethod]
        public void TestNudgeDouble()
        {
            Assert.AreEqual(UtilCalibration.NudgeDouble(1000, 0, 500), 500);
            System.Console.WriteLine(UtilCalibration.NudgeDouble(1000, 0, 500));
            Assert.AreEqual(UtilCalibration.NudgeDouble(-10, 10, 500), 10);
            System.Console.WriteLine(UtilCalibration.NudgeDouble(-10, 10, 500));
            Assert.AreEqual(UtilCalibration.NudgeDouble(50, 10, 500), 50);
            System.Console.WriteLine(UtilCalibration.NudgeDouble(50, 10, 500));
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

        [TestMethod]
        public void RemoveBytesFromArray_RemovesCorrectBytes()
        {
            // Arrange
            byte[] originalBytes = { 0x01, 0x02, 0x03, 0x04, 0x05 };
            int bytesToRemove = 2;

            // Act
            byte[] modifiedBytes = ProgrammerUtilities.RemoveBytesFromArray(originalBytes, bytesToRemove);

            // Assert
            byte[] expectedBytes = { 0x03, 0x04, 0x05 };
            Assert.AreEqual(expectedBytes.Length, modifiedBytes.Length, "Lengths should match after removing bytes");

            for (int i = 0; i < expectedBytes.Length; i++)
            {
                Assert.AreEqual(expectedBytes[i], modifiedBytes[i], $"Byte at index {i} does not match");
            }
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
