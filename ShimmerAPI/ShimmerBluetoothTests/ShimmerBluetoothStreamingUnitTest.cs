using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShimmerAPI;
using System.Threading;
using static ShimmerAPI.ShimmerBluetooth;
using static ShimmerAPI.ShimmerConfiguration;
using System.Collections;

namespace ShimmerBluetoothTests
{
    [TestClass]
    public class ShimmerBluetoothUnitTest
    {
        String comPort = "COM29";
        String deviceName = "testName";
        ShimmerLogAndStreamSystemSerialPort shimmerDevice;
        ObjectCluster ojc = null;
        ArrayList ojcArray = new ArrayList();
     
        [TestInitialize]
        public void InitializeConnection()
        {
            shimmerDevice = new ShimmerLogAndStreamSystemSerialPort(deviceName, comPort);
            shimmerDevice.UICallback += this.HandleEvent;
            ojc = null;
            ojcArray.Clear();
            shimmerDevice.Connect();
            while (shimmerDevice.GetState() != ShimmerBluetooth.SHIMMER_STATE_CONNECTED)
            {
                Thread.Sleep(100);
                if (shimmerDevice.GetState() == ShimmerBluetooth.SHIMMER_STATE_NONE)
                {
                    Assert.Fail();
                }
            }
        }

        [TestCleanup]
        public void EndConnection()
        {
            shimmerDevice.StopStreaming();
            Thread.Sleep(200);
            shimmerDevice.Disconnect();
            Thread.Sleep(1000);
            shimmerDevice = null;
        }

        [TestMethod]
        public void TestECG()
        {
            int enabledSensors = ((int)ShimmerBluetooth.SensorBitmapShimmer3.SENSOR_EXG1_24BIT | (int)ShimmerBluetooth.SensorBitmapShimmer3.SENSOR_EXG2_24BIT); // this is to enable the two EXG Chips on the Shimmer3
            shimmerDevice.WriteSensors(enabledSensors);
            byte[] defaultECGReg1 = ShimmerBluetooth.SHIMMER3_DEFAULT_ECG_REG1; //also see ShimmerBluetooth.SHIMMER3_DEFAULT_TEST_REG1 && ShimmerBluetooth.SHIMMER3_DEFAULT_EMG_REG1
            byte[] defaultECGReg2 = ShimmerBluetooth.SHIMMER3_DEFAULT_ECG_REG2; //also see ShimmerBluetooth.SHIMMER3_DEFAULT_TEST_REG2 && ShimmerBluetooth.SHIMMER3_DEFAULT_EMG_REG2
            shimmerDevice.WriteEXGConfigurations(defaultECGReg1, defaultECGReg2);
            Thread.Sleep(1000);

            String[] array = shimmerDevice.GetSignalNameArray();
            Assert.AreEqual(array[1], Shimmer3Configuration.SignalNames.EXG1_STATUS);

            shimmerDevice.StartStreaming();
            System.Console.WriteLine("StartStreaming");
            Thread.Sleep(5000);
            if (ojc == null)
            {
                Assert.AreEqual(true, false);
            } else
            {
                SensorData data = ojc.GetData(Shimmer3Configuration.SignalNames.ECG_LA_RA, "CAL");
                Assert.AreNotEqual(null,data);
            }

           
        }

        [TestMethod]
        public void TestMethodEXGSaw()
        {
            if (shimmerDevice.GetFirmwareIdentifier() == ShimmerBluetooth.FW_IDENTIFIER_SHIMMERECGMD)
            {
                double freq = 51.2;
                double samplingperiodinms = (1 / freq) * 1000;
                shimmerDevice.WriteSamplingRate(51.2);
                int enabledSensors = ((int)ShimmerBluetooth.SensorBitmapShimmer3.SENSOR_EXG1_24BIT | (int)ShimmerBluetooth.SensorBitmapShimmer3.SENSOR_EXG2_24BIT); // this is to enable the two EXG Chips on the Shimmer3
                shimmerDevice.WriteSensors(enabledSensors);
                byte[] defaultECGReg1 = ShimmerBluetooth.SHIMMER3_DEFAULT_ECG_REG1; //also see ShimmerBluetooth.SHIMMER3_DEFAULT_TEST_REG1 && ShimmerBluetooth.SHIMMER3_DEFAULT_EMG_REG1
                byte[] defaultECGReg2 = ShimmerBluetooth.SHIMMER3_DEFAULT_ECG_REG2; //also see ShimmerBluetooth.SHIMMER3_DEFAULT_TEST_REG2 && ShimmerBluetooth.SHIMMER3_DEFAULT_EMG_REG2
                shimmerDevice.WriteEXGConfigurations(defaultECGReg1, defaultECGReg2);
                Thread.Sleep(1000);

                String[] array = shimmerDevice.GetSignalNameArray();
                Assert.AreEqual(array[1], Shimmer3Configuration.SignalNames.EXG1_STATUS);

                shimmerDevice.StartStreamingEXGSawtoothTestSignal(5000);
                Thread.Sleep(5000);
                if (ojc == null)
                {
                    Assert.Fail();
                }
                else
                {
                    SensorData data = ojc.GetData(Shimmer3Configuration.SignalNames.ECG_LA_RA, SignalFormats.CAL);
                    Assert.AreNotEqual(null, data);
                    data = ojc.GetData(Shimmer3Configuration.SignalNames.ECG_LL_RA, SignalFormats.CAL);
                    Assert.AreNotEqual(null, data);
                    data = ojc.GetData(Shimmer3Configuration.SignalNames.ECG_VX_RL, SignalFormats.CAL);
                    Assert.AreNotEqual(null, data);
                }

                if (ojcArray.Count > 2)
                {
                    SensorData data1 = ((ObjectCluster)ojcArray[0]).GetData(Shimmer3Configuration.SignalNames.ECG_LA_RA, SignalFormats.RAW);
                    SensorData data2 = ((ObjectCluster)ojcArray[1]).GetData(Shimmer3Configuration.SignalNames.ECG_LA_RA, SignalFormats.RAW);
                    SensorData datats1 = ((ObjectCluster)ojcArray[0]).GetData(ShimmerConfiguration.SignalNames.TIMESTAMP, SignalFormats.CAL);
                    SensorData datats2 = ((ObjectCluster)ojcArray[1]).GetData(ShimmerConfiguration.SignalNames.TIMESTAMP, SignalFormats.CAL);
                    double numberofsamples = Math.Round((datats2.Data - datats1.Data) / samplingperiodinms);
                    double difference = data2.Data - data1.Data;
                    if (difference == 5000 * numberofsamples)
                    {
                        System.Console.WriteLine(difference + " " + numberofsamples);
                    }
                    else
                    {
                        Assert.Fail();
                    }

                }
                else
                {
                    Assert.Fail();
                }

            }
        }


        public void HandleEvent(object sender, EventArgs args)
        {
            CustomEventArgs eventArgs = (CustomEventArgs)args;
            int indicator = eventArgs.getIndicator();

            switch (indicator)
            {
                case (int)ShimmerBluetooth.ShimmerIdentifier.MSG_IDENTIFIER_STATE_CHANGE:

                    System.Diagnostics.Debug.Write(((ShimmerBluetooth)sender).GetDeviceName() + " State = " + ((ShimmerBluetooth)sender).GetStateString() + System.Environment.NewLine);
                    int state = (int)eventArgs.getObject();
                    if (state == (int)ShimmerBluetooth.SHIMMER_STATE_CONNECTED)
                    {

                    }
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
