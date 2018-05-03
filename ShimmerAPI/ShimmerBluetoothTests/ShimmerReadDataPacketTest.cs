using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using ShimmerAPI;
using System.Collections;

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
            sbrd.data = new byte[]{ 0, 1, 2, 3, 0, 5, 6, 7, 8, 9, 10 };
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
