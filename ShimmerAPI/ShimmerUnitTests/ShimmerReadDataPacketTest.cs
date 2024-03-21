using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using ShimmerAPI;
using System.Collections;
using System.Collections.Generic;

namespace ShimmerBluetoothTests
{
    [TestClass]
    public class ShimmerReadDataPacketTest
    {
        ArrayList ojcArray = new ArrayList();
        ShimmerBluetoothReadData sbrd;
        [TestInitialize]
        public void Initialize()
        {

            sbrd = new ShimmerBluetoothReadData("test");
            sbrd.UICallback += this.HandleEvent;
            ojcArray = new ArrayList();
        }

        [TestMethod]
        public void TestPacketParserNoErrors()
        {
            sbrd.start();
            while (ojcArray.Count < 10)
            {
                Thread.Sleep(1);
            }
            sbrd.stop();

            for(int i=0;i<ojcArray.Count; i++)
            {
                ObjectClusterByteArray ojc = (ObjectClusterByteArray) ojcArray[i];
                for (int j = 0; j < 10; j++)
                {
                    //System.Console.Write(ojc.packet[j] + " ");
                    if (j == 0)
                    {
                        if (ojc.packet[j] == 1)
                        {
                        } else
                        {
                            Assert.Fail();
                        }
                    }
                }

            //System.Console.WriteLine();
            }

        }

        [TestMethod]
        public void TestPacketParserCRCNoErrors()
        {
            sbrd.WriteCRCMode(ShimmerBluetooth.BTCRCMode.TWO_BYTE);
            sbrd.data = new byte[] {
                (byte) 0x00,
                (byte) 0xb6, (byte) 0xf8, (byte) 0xbb,
                (byte) 0xff, (byte) 0x80, (byte) 0x00, (byte) 0x01, (byte) 0x80, (byte) 0x00, (byte) 0x01,
                (byte) 0xff, (byte) 0x80, (byte) 0x00, (byte) 0x01, (byte) 0x80, (byte) 0x00, (byte) 0x01,
                (byte) 0x8a, (byte) 0x93
            };
            sbrd.stopReadingAtEndOfDataStream = true;
            sbrd.start(sbrd.data.Length-3); //-1 for the starting byte 00 and crc has 2 bytes
            Thread.Sleep(50);
            sbrd.stop();
            sbrd.stopReadingAtEndOfDataStream = false;
            if (ojcArray.Count == 1)
            {
                Assert.IsTrue(true);
            } else
            {
                Assert.IsFalse(true);
            }
            sbrd.WriteCRCMode(ShimmerBluetooth.BTCRCMode.OFF);
        }

        [TestMethod]
        public void TestPacketParserCRCNoErrorsMultiplePackets()
        {
            int numberOfPackets = 10;
            sbrd.WriteCRCMode(ShimmerBluetooth.BTCRCMode.TWO_BYTE);
            sbrd.data = new byte[] {
                (byte) 0x00,
                (byte) 0xb6, (byte) 0xf8, (byte) 0xbb,
                (byte) 0xff, (byte) 0x80, (byte) 0x00, (byte) 0x01, (byte) 0x80, (byte) 0x00, (byte) 0x01,
                (byte) 0xff, (byte) 0x80, (byte) 0x00, (byte) 0x01, (byte) 0x80, (byte) 0x00, (byte) 0x01,
                (byte) 0x8a, (byte) 0x93
            };
            int packetSize = (sbrd.data.Length - 3); //-1 for the starting byte 00 and crc has 2 bytes
            List<Byte> dataList = new List<Byte>();
            for (int i = 0; i < numberOfPackets; i++)
            {
                foreach(byte b in sbrd.data)
                {
                    dataList.Add(b);
                }                
            }
            sbrd.data = dataList.ToArray();
            sbrd.stopReadingAtEndOfDataStream = true;
            sbrd.start(packetSize); //-1 for the starting byte 00 and crc has 2 bytes
            Thread.Sleep(50);
            sbrd.stop();
            sbrd.stopReadingAtEndOfDataStream = false;
            if (ojcArray.Count == 10)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(true);
            }
            sbrd.WriteCRCMode(ShimmerBluetooth.BTCRCMode.OFF);
        }

        [TestMethod]
        public void TestPacketParserCRCNoErrorsMultiplePacketsStartWithWrongAlignment()
        {
            int numberOfPackets = 10;
            sbrd.WriteCRCMode(ShimmerBluetooth.BTCRCMode.TWO_BYTE);
            sbrd.byteDataIndex = 3;
            sbrd.data = new byte[] {
                (byte) 0x00,
                (byte) 0xb6, (byte) 0xf8, (byte) 0xbb,
                (byte) 0xff, (byte) 0x80, (byte) 0x00, (byte) 0x01, (byte) 0x80, (byte) 0x00, (byte) 0x01,
                (byte) 0xff, (byte) 0x80, (byte) 0x00, (byte) 0x01, (byte) 0x80, (byte) 0x00, (byte) 0x01,
                (byte) 0x8a, (byte) 0x93
            };
            int packetSize = (sbrd.data.Length - 3); //-1 for the starting byte 00 and crc has 2 bytes
            List<Byte> dataList = new List<Byte>();
            for (int i = 0; i < numberOfPackets; i++)
            {
                foreach (byte b in sbrd.data)
                {
                    dataList.Add(b);
                }
            }
            sbrd.data = dataList.ToArray();
            sbrd.stopReadingAtEndOfDataStream = true;
            sbrd.start(packetSize); //-1 for the starting byte 00 and crc has 2 bytes
            Thread.Sleep(50);
            sbrd.stop();
            sbrd.stopReadingAtEndOfDataStream = false;
            if (ojcArray.Count == 9)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(true);
            }
            sbrd.WriteCRCMode(ShimmerBluetooth.BTCRCMode.OFF);
        }

        [TestMethod]
        public void TestPacketParserCRCMultiplePacketBadCRC()
        {
            int numberOfPackets = 10; //only even
            sbrd.WriteCRCMode(ShimmerBluetooth.BTCRCMode.TWO_BYTE);
            byte[] gooddata = new byte[] {
                (byte) 0x00,
                (byte) 0xb6, (byte) 0xf8, (byte) 0xbb,
                (byte) 0xff, (byte) 0x80, (byte) 0x00, (byte) 0x01, (byte) 0x80, (byte) 0x00, (byte) 0x01,
                (byte) 0xff, (byte) 0x80, (byte) 0x00, (byte) 0x01, (byte) 0x80, (byte) 0x00, (byte) 0x01,
                (byte) 0x8a, (byte) 0x93
            };
            byte[] baddata = new byte[] {
                (byte) 0x00,
                (byte) 0xb6, (byte) 0xf8, (byte) 0xbb,
                (byte) 0xff, (byte) 0x80, (byte) 0x00, (byte) 0x01, (byte) 0x80, (byte) 0x00, (byte) 0x01,
                (byte) 0xff, (byte) 0x80, (byte) 0x00, (byte) 0x01, (byte) 0x80, (byte) 0x00, (byte) 0x01,
                (byte) 0x8a, (byte) 0x94
            };
            int packetSize = (gooddata.Length - 3); //-1 for the starting byte 00 and crc has 2 bytes
            List<Byte> dataList = new List<Byte>();
            for (int i = 0; i < (numberOfPackets/2); i++)
            {
                foreach (byte b in gooddata)
                {
                    dataList.Add(b);
                }

                foreach (byte b in baddata)
                {
                    dataList.Add(b);
                }
            }
            sbrd.data = dataList.ToArray();
            sbrd.stopReadingAtEndOfDataStream = true;
            sbrd.start(packetSize); //-1 for the starting byte 00 and crc has 2 bytes
            Thread.Sleep(50);
            sbrd.stop();
            sbrd.stopReadingAtEndOfDataStream = false;
            if (ojcArray.Count == 5)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(true);
            }
            sbrd.WriteCRCMode(ShimmerBluetooth.BTCRCMode.OFF);
        }

        [TestMethod]
        public void TestPacketParserNoErrorsWithTimeout()
        {
            sbrd.data = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            sbrd.enableReadTimeoutException(true);
            sbrd.start();
            while (ojcArray.Count < 10)
            {
                Thread.Sleep(1);
            }
            sbrd.stop();

            for (int i = 0; i < ojcArray.Count; i++)
            {
                ObjectClusterByteArray ojc = (ObjectClusterByteArray)ojcArray[i];
                for (int j = 0; j < 10; j++)
                {
                    System.Console.Write(ojc.packet[j] + " ");
                    if (j == 0)
                    {
                        if (ojc.packet[j] == 1)
                        {
                        }
                        else
                        {
                            Assert.Fail();
                        }
                    }
                }

                System.Console.WriteLine();
            }
            sbrd.enableReadTimeoutException(false);

        }

        [TestMethod]
        public void TestPacketParserIncorrectStartingByteAndRepeatZeroInPacketByteArray()
        {
            sbrd.byteDataIndex = 3;
            sbrd.data = new byte[]{ 0, 1, 2, 3, 0, 0, 6, 7, 8, 9, 0};
            sbrd.mEnableTimeStampAlignmentCheck = true;
            sbrd.start();
            while (ojcArray.Count < 10)
            {
                Thread.Sleep(1);
            }
            sbrd.stop();

            for (int i = 0; i < ojcArray.Count; i++)
            {
                ObjectClusterByteArray ojc = (ObjectClusterByteArray)ojcArray[i];
                for (int j = 0; j < 10; j++)
                {
                    System.Console.Write(ojc.packet[j] + " ");
                    if (j == 0)
                    {
                        if (ojc.packet[j] == 1)
                        {
                            
                        }
                        else
                        {
                            Assert.Fail();
                        }
                    }
                }

                System.Console.WriteLine();
            }

        }

        [TestMethod]
        public void TestPacketParserIncorrectStartingByteAndInconsistentZeroInPacketByteArray()
        {
            sbrd.byteDataIndex = 2;
            sbrd.data = new byte[] 
            { 0, 1, 2, 3, 4, 0, 6, 7, 8, 9, 10,
             0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            sbrd.start();
            while (ojcArray.Count < 10)
            {
                Thread.Sleep(1);
            }
            sbrd.stop(); ;

            for (int i = 0; i < ojcArray.Count; i++)
            {
                ObjectClusterByteArray ojc = (ObjectClusterByteArray)ojcArray[i];
                for (int j = 0; j < 10; j++)
                {
                    System.Console.Write(ojc.packet[j] + " ");
                    if (j == 0)
                    {
                        if (ojc.packet[j] == 1)
                        {
                        }
                        else
                        {
                            Assert.Fail();
                        }
                    }
                }

                System.Console.WriteLine();
            }

        }

        public void HandleEvent(object sender, EventArgs args)
        {
            CustomEventArgs eventArgs = (CustomEventArgs)args;
            int indicator = eventArgs.getIndicator();

            switch (indicator)
            {
                case (int)ShimmerBluetooth.ShimmerIdentifier.MSG_IDENTIFIER_STATE_CHANGE:

                    
                    break;
                case (int)ShimmerBluetooth.ShimmerIdentifier.MSG_IDENTIFIER_DATA_PACKET:
                    // this is essential to ensure the object is not a reference
                    ObjectClusterByteArray objectCluster = (ObjectClusterByteArray)eventArgs.getObject();
                    ojcArray.Add(objectCluster);
                    break;
            }
        }

    }
}
