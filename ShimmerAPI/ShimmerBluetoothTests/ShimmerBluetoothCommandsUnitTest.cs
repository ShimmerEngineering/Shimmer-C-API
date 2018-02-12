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
    public class ShimmerBluetoothCommandsUnitTest
    {
        String comPort = "COM29";
        String deviceName = "testName";
        ShimmerLogAndStreamSystemSerialPort shimmerDevice;
        
        [TestMethod]
        public void TestMethodWriteSensors()
        {
            shimmerDevice = new ShimmerLogAndStreamSystemSerialPort(deviceName, comPort);
            shimmerDevice.Connect();
            while (shimmerDevice.GetState() != ShimmerBluetooth.SHIMMER_STATE_CONNECTED)
            {
                Thread.Sleep(100);
                if (shimmerDevice.GetState() == ShimmerBluetooth.SHIMMER_STATE_NONE)
                {
                    Assert.Fail();
                }
            }

            shimmerDevice.WriteSensors((int)SensorBitmapShimmer3.SENSOR_A_ACCEL);
            Thread.Sleep(1000);
            String[] array = shimmerDevice.GetSignalNameArray();
            Assert.AreEqual(array[1], Shimmer3Configuration.SignalNames.LOW_NOISE_ACCELEROMETER_X);

            shimmerDevice.WriteSensors((int)SensorBitmapShimmer3.SENSOR_MPU9150_GYRO);
            Thread.Sleep(1000);
            array = shimmerDevice.GetSignalNameArray();
            Assert.AreEqual(array[1], Shimmer3Configuration.SignalNames.GYROSCOPE_X);
            shimmerDevice.Disconnect();
            Thread.Sleep(1000);
            shimmerDevice = null;
        }
        

    }
}
