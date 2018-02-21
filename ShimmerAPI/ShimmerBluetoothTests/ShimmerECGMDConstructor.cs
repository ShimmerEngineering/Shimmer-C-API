using System;
using System.Collections;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShimmerAPI;

namespace ShimmerBluetoothTests
{
    [TestClass]
    public class ShimmerECGMDConstructor
    {

        String comport = "COM29";
        ArrayList ojcArray = new ArrayList();
        ObjectCluster ojc = null;

        [TestMethod]
        public void TestMethodConstructor()
        {
            ShimmerLogAndStreamSystemSerialPort shimmerDevice = new ShimmerLogAndStreamSystemSerialPort("",comport);
            shimmerDevice.UICallback += this.HandleEvent;
            ojcArray.Clear();
            ojc = null;
            shimmerDevice.Connect();
            while (shimmerDevice.GetState() != ShimmerBluetooth.SHIMMER_STATE_CONNECTED)
            {
                Thread.Sleep(100);
                if (shimmerDevice.GetState() == ShimmerBluetooth.SHIMMER_STATE_NONE)
                {
                    Assert.Fail();
                }
            }
            int enabledSensors = ((int)ShimmerBluetooth.SensorBitmapShimmer3.SENSOR_EXG1_24BIT | (int)ShimmerBluetooth.SensorBitmapShimmer3.SENSOR_EXG2_24BIT); // this is to enable the two EXG Chips on the Shimmer3
            byte[] defaultECGReg1 = ShimmerBluetooth.SHIMMER3_DEFAULT_ECG_REG1; //also see ShimmerBluetooth.SHIMMER3_DEFAULT_TEST_REG1 && ShimmerBluetooth.SHIMMER3_DEFAULT_EMG_REG1
            byte[] defaultECGReg2 = ShimmerBluetooth.SHIMMER3_DEFAULT_ECG_REG2; //also see ShimmerBluetooth.SHIMMER3_DEFAULT_TEST_REG2 && ShimmerBluetooth.SHIMMER3_DEFAULT_EMG_REG2
            shimmerDevice.WriteEXGConfigurations(defaultECGReg1, defaultECGReg2);
            Thread.Sleep(500);
            shimmerDevice.WriteSensors(enabledSensors);
            Thread.Sleep(1000);

            String[] array = shimmerDevice.GetSignalNameArray();
            Assert.AreEqual(array[1], Shimmer3Configuration.SignalNames.EXG1_STATUS);

            shimmerDevice.StartStreaming();
            System.Console.WriteLine("StartStreaming");
            Thread.Sleep(5000);
            if (ojc == null)
            {
                Assert.AreEqual(true, false);
            }
            else
            {
                SensorData data = ojc.GetData(Shimmer3Configuration.SignalNames.ECG_LA_RA, "CAL");
                Assert.AreNotEqual(null, data);
            }
            shimmerDevice.StopStreaming();
            Thread.Sleep(200);
            shimmerDevice.Disconnect();
            Thread.Sleep(1000);
            shimmerDevice = null;
        }

        [TestMethod]
        public void TestMethodInitializingConstructor()
        {
            int enabledSensors = ((int)ShimmerBluetooth.SensorBitmapShimmer3.SENSOR_EXG1_24BIT | (int)ShimmerBluetooth.SensorBitmapShimmer3.SENSOR_EXG2_24BIT); // this is to enable the two EXG Chips on the Shimmer3
            byte[] defaultECGReg1 = ShimmerBluetooth.SHIMMER3_DEFAULT_TEST_REG1; //also see ShimmerBluetooth.SHIMMER3_DEFAULT_TEST_REG1 && ShimmerBluetooth.SHIMMER3_DEFAULT_EMG_REG1
            byte[] defaultECGReg2 = ShimmerBluetooth.SHIMMER3_DEFAULT_TEST_REG2; //also see ShimmerBluetooth.SHIMMER3_DEFAULT_TEST_REG2 && ShimmerBluetooth.SHIMMER3_DEFAULT_EMG_REG2

            ShimmerLogAndStreamSystemSerialPort shimmerDevice = new ShimmerLogAndStreamSystemSerialPort("", comport,51.2, enabledSensors, defaultECGReg1, defaultECGReg2);
            shimmerDevice.UICallback += this.HandleEvent;
            ojcArray.Clear();
            ojc = null;
            shimmerDevice.Connect();
            while (shimmerDevice.GetState() != ShimmerBluetooth.SHIMMER_STATE_CONNECTED)
            {
                Thread.Sleep(100);
                if (shimmerDevice.GetState() == ShimmerBluetooth.SHIMMER_STATE_NONE)
                {
                    Assert.Fail();
                }
            }
           
            String[] array = shimmerDevice.GetSignalNameArray();
            Assert.AreEqual(array[1], Shimmer3Configuration.SignalNames.EXG1_STATUS);

            shimmerDevice.StartStreaming();
            System.Console.WriteLine("StartStreaming");
            Thread.Sleep(5000);
            if (ojc == null)
            {
                Assert.AreEqual(true, false);
            }
            else
            {
                SensorData data = ojc.GetData(Shimmer3Configuration.SignalNames.EXG1_CH1, "CAL");
                Assert.AreNotEqual(null, data);
            }
            shimmerDevice.StopStreaming();
            Thread.Sleep(200);
            shimmerDevice.Disconnect();
            Thread.Sleep(1000);
            shimmerDevice = null;
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
                    ObjectCluster objectCluster = new ObjectCluster((ObjectCluster)eventArgs.getObject());
                    ojc = objectCluster;
                    ojcArray.Add(objectCluster);
                    break;
            }
        }
    }
}
