using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShimmerAPI;
using ShimmerAPI.Sensors;
using ShimmerAPI.Simulators;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ShimmerAPI.ShimmerBluetooth;

namespace ShimmerBluetoothTests
{
    [TestClass]
    public class ShimmerLogAndStreamS3SimulatorTest
    {
        ShimmerLogAndStreamS3Simulator mDevice;
        String ComPort = "COM99";
        ConcurrentQueue<byte> cq = new ConcurrentQueue<byte>();
        protected int SetEnabledSensors = (int)SensorBitmapShimmer3R.SENSOR_BMP380_PRESSURE;

        [TestInitialize]
        public void SetUp()
        {
            mDevice = new ShimmerLogAndStreamS3Simulator("", ComPort);
        }

        [TestMethod]
        public void Test001_testConnectandDisconnect()
        {
            if (mDevice != null)
            {
                try
                {
                    mDevice.SetIsNewBMPSupported(false);
                    mDevice.StartConnectThread();
                    Thread.Sleep(30000);
                    if (!mDevice.IsConnected())
                    {
                        Assert.Fail();
                    }

                    if (!mDevice.GetFirmwareVersionFullName().Equals("LogAndStream 0.16.9"))
                    {
                        Assert.Fail();
                    }

                    if (!mDevice.GetShimmerVersion().Equals(3)) //Shimmer3
                    {
                        Assert.Fail();
                    }

                     //if (!mDevice.isGetBmp390CalibrationCoefficientsCommand)
                    //{
                    //    Assert.Fail();
                    //}

                    //if (mDevice.GetEnabledSensors() != (0x00 | (int)SensorBitmapShimmer3R.SENSOR_BMP380_PRESSURE))
                    //{
                    //    Assert.Fail();
                    //}

                    try
                    {
                        mDevice.Disconnect();
                    }
                    catch (Exception ex)
                    {
                        Assert.Fail($"Test aborted due to exception: {ex.Message}");
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

        [TestMethod]
        public void Test002_testConnectandDisconnect_NewBMPSupported()
        {
            if (mDevice != null)
            {
                try
                {
                    mDevice.StartConnectThread();
                    mDevice.SetIsNewBMPSupported(false);
                    Thread.Sleep(30000);
                    if (!mDevice.IsConnected())
                    {
                        Assert.Fail();
                    }

                    if (!mDevice.GetFirmwareVersionFullName().Equals("LogAndStream 0.16.9"))
                    {
                        Assert.Fail();
                    }

                    if (!mDevice.GetShimmerVersion().Equals(3)) //Shimmer3
                    {
                        Assert.Fail();
                    }

                    //if (!mDevice.isGetBmp390CalibrationCoefficientsCommand)
                    //{
                    //    Assert.Fail();
                    //}

                    //if (mDevice.GetEnabledSensors() != (0x00 | (int)SensorBitmapShimmer3R.SENSOR_BMP380_PRESSURE))
                    //{
                    //    Assert.Fail();
                    //}

                    //foreach (byte b in mDevice.ListofSensorChannels)
                    //{
                    //    Console.WriteLine(b + " ; ");
                    //}

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

        public void ProcessCalibrationData()
        {
            byte[] calibDump = mDevice.GetCalibrationDump().ToArray();

            if (calibDump == null || calibDump.Length < 2)
            {
                throw new ArgumentException("Invalid calibDump: must contain at least 2 bytes.");
            }

            //mDevice.WriteAccelRange(0);
            LNAccel lnAccel = new LNAccel((int)ShimmerBluetooth.ShimmerVersion.SHIMMER3);
            lnAccel.RetrieveKinematicCalibrationParametersFromCalibrationDump(calibDump);
            //mDevice.WriteGyroRange(0);
            //GyroSensor gyro = new GyroSensor();
            //gyro.RetrieveKinematicCalibrationParametersFromCalibrationDump(calibDumpResponse.ToArray());
        }

    }
}
