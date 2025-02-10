using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShimmerAPI;
using ShimmerAPI.Sensors;
using ShimmerAPI.Simulators;
using ShimmerAPI.Utilities;
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

                    mDevice.ReadPressureCalibrationCoefficients();
                    if (!mDevice.isGetPressureCalibrationCoefficientsCommand)
                    {
                        Assert.Fail();
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
        public void Test003_ConnectandTestCalibParamRead()
        {
            if (mDevice != null)
            {
                try
                {
                    mDevice.StartConnectThread();
                    Thread.Sleep(30000);

                    if (!mDevice.IsConnected())
                    {
                        Assert.Fail();
                    }

                    byte[] deviceCalBytes = mDevice.CalibByteDumpGenerate();
                    System.Console.WriteLine("deviceCalBytes : " + UtilShimmer.BytesToHexString(deviceCalBytes));
                    mDevice.CalibByteDumpParse(deviceCalBytes);

                    Object returnValue = mDevice.mapOfSensorCalibration;

                    Assert.IsNotNull(returnValue);

                    Dictionary<int, Dictionary<int, List<double[,]>>> mapOfKinematicSensorCalibrationAll = returnValue as Dictionary<int, Dictionary<int, List<double[,]>>>;

                    foreach (int sensorId in mapOfKinematicSensorCalibrationAll.Keys)
                    {
                        Dictionary<int, List<double[,]>> route1CalParamMapPerSensor = mapOfKinematicSensorCalibrationAll[sensorId];

                        Object sensorDetails = mDevice.GetSensorDetails(sensorId);
                        Assert.IsNotNull(sensorDetails);

                        Console.WriteLine("sensorId : " + sensorId);

                        Dictionary<int, List<double[,]>> mapOfKinematicCalibPerRange = mapOfKinematicSensorCalibrationAll[sensorId];
                        foreach (var calibDetails in mapOfKinematicCalibPerRange)
                        {
                            Console.WriteLine(UtilShimmer.GetDebugString(calibDetails.Key, calibDetails.Value));
                        }
                        Console.WriteLine("\n");

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

    }
}
