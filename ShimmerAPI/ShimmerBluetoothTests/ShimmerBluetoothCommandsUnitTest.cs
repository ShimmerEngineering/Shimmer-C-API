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
        
        [TestInitialize]
        public void TestInitialize()
        {
            shimmerDevice = new ShimmerLogAndStreamSystemSerialPort(deviceName, comPort);
            shimmerDevice.Connect();
            while (shimmerDevice.GetState() != ShimmerBluetooth.SHIMMER_STATE_CONNECTED)
            {
                Thread.Sleep(100);
                if (shimmerDevice.GetState() == ShimmerBluetooth.SHIMMER_STATE_NONE)
                {
                    Assert.Fail("ConnectionFail");
                }
            }
        }

        [TestCleanup]
        public void Finish()
        {
            shimmerDevice.Disconnect();
            Thread.Sleep(1000);
            shimmerDevice = null;
        }

        [TestMethod]
        public void TestMethodWriteSensors()
        {

            shimmerDevice.WriteSensors((int)SensorBitmapShimmer3.SENSOR_A_ACCEL);
            Thread.Sleep(1000);
            String[] array = shimmerDevice.GetSignalNameArray();
            Assert.AreEqual(array[1], Shimmer3Configuration.SignalNames.LOW_NOISE_ACCELEROMETER_X);

            shimmerDevice.WriteSensors((int)SensorBitmapShimmer3.SENSOR_MPU9150_GYRO);
            Thread.Sleep(1000);
            array = shimmerDevice.GetSignalNameArray();
            Assert.AreEqual(array[1], Shimmer3Configuration.SignalNames.GYROSCOPE_X);
            
        }
        
        [TestMethod]
        public void TestBatteryVoltage()
        {
            shimmerDevice.ReadBattery();
            if (shimmerDevice.getBatteryVoltage()<2 || shimmerDevice.getBatteryVoltage() > 5)
            {
                System.Console.WriteLine("Battery Voltage: " + shimmerDevice.getBatteryVoltage());
                System.Console.WriteLine("Battery Status: " + shimmerDevice.getBatteryChargingStatus());
                Assert.Fail();
            } else
            {
                System.Console.WriteLine("Battery Voltage: " + shimmerDevice.getBatteryVoltage());
                System.Console.WriteLine("Battery Status: " + shimmerDevice.getBatteryChargingStatus());
            }

        }


        [TestMethod]
        public void TestBatteryVoltageWhileStreaming()
        {
            if (shimmerDevice.GetFirmwareIdentifier() == ShimmerBluetooth.FW_IDENTIFIER_LOGANDSTREAM)
            {
                shimmerDevice.StartStreaming();
                Thread.Sleep(5000);
                shimmerDevice.ReadBattery();
                Thread.Sleep(1000);
                if (shimmerDevice.getBatteryVoltage() < 2 || shimmerDevice.getBatteryVoltage() > 5)
                {
                    System.Console.WriteLine("Battery Voltage: " + shimmerDevice.getBatteryVoltage());
                    System.Console.WriteLine("Battery Status: " + shimmerDevice.getBatteryChargingStatus());
                    Assert.Fail();
                }
                else
                {
                    System.Console.WriteLine("Battery Voltage: " + shimmerDevice.getBatteryVoltage());
                    System.Console.WriteLine("Battery Status: " + shimmerDevice.getBatteryChargingStatus());
                }
                shimmerDevice.StopStreaming();
            } else
            {
                Assert.Fail("This Shimmer Device Firmware version is not supported");
            }
        }

    }
}
