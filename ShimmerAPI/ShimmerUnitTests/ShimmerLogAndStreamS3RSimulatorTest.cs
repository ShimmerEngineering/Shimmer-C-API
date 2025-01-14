using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShimmerAPI;
using ShimmerAPI.Simulators;
using ShimmerAPI.Utilities;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static ShimmerAPI.ShimmerBluetooth;

namespace ShimmerBluetoothTests
{
    [TestClass]
    public class ShimmerLogAndStreamS3RSimulatorTest
    {
        ShimmerLogAndStreamS3RSimulator mDevice;
        String ComPort = "COM99";
        ConcurrentQueue<byte> cq = new ConcurrentQueue<byte>();

        [TestInitialize]
        public void SetUp()
        {
            mDevice = new ShimmerLogAndStreamS3RSimulator("", ComPort);
        }

        [TestMethod]
        public void Test001_testConnectandDisconnect()
        {
            //Comment out test as it is not completed
            //if (mDevice != null)
            //{
            //    try
            //    {
            //        mDevice.StartConnectThread();
            //        mDevice.SetIsNewBMPSupported(true);
            //        Thread.Sleep(30000);
            //        if (!mDevice.IsConnected())
            //        {
            //            Assert.Fail();
            //        }

            //        if (!mDevice.GetFirmwareVersionFullName().Equals("LogAndStream 0.0.1"))
            //        {
            //            Assert.Fail();
            //        }

            //        if (!mDevice.GetShimmerVersion().Equals(10))    //Shimmer3R
            //        {
            //            Assert.Fail();
            //        }

            //        if (!mDevice.isGetPressureCalibrationCoefficientsCommand)
            //        {
            //            Assert.Fail();
            //        }

            //        if (mDevice.GetEnabledSensors() != (0x00 | (int)SensorBitmapShimmer3R.SENSOR_BMP380_PRESSURE))
            //        {
            //            Assert.Fail();
            //        }

            //        foreach (byte b in mDevice.ListofSensorChannels)
            //        {
            //            Debug.WriteLine(b + " ; ");
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Assert.Fail($"Test aborted due to exception: {ex.Message}");
            //    }
            //}
            //else
            //{
            //    Assert.Fail("mDevice is null");
            //}
        }

        [TestMethod]
        public void Test002_ConnectandTestBMP390()
        {
            //if (mDevice != null)
            //{
            //    try
            //    {
            //        mDevice.StartConnectThread();
            //        mDevice.SetIsNewBMPSupported(true);
            //        Thread.Sleep(30000);
            //        if (!mDevice.IsConnected())
            //        {
            //            Assert.Fail();
            //        }

            //        try
            //        {
            //            mDevice.CalculateBMP390PressureCalibrationCoefficientsResponse(mDevice.GetPressureResoTest());

            //            string[] sensorDataType = { "u24" };

            //            byte[] sensorDataP2 = { 0x00, 0x17, 0x64 };
            //            byte[] sensorDataT2 = { 0x00, 0xCF, 0x7F };

            //            long[] uncalibResultP2 = ProgrammerUtilities.ParseData(sensorDataP2, sensorDataType);
            //            long[] uncalibResultT2 = ProgrammerUtilities.ParseData(sensorDataT2, sensorDataType);
            //            double[] bmpX80caldata2 = new double[2];
            //            bmpX80caldata2 = mDevice.CalibratePressure390SensorData(uncalibResultP2[0], uncalibResultT2[0]);
            //            Bmp3QuantizedCalibData.TLin = bmpX80caldata2[1];

            //            //Assert.AreEqual(resultP2, 100912.81758676282);
            //            //Assert.AreEqual(resultT2, 23.26587201654911);
            //            Assert.AreEqual(Math.Round(bmpX80caldata2[0], 4), 100912.8176);
            //            Assert.AreEqual(Math.Round(bmpX80caldata2[1], 4), 23.2659);
            //        }
            //        catch (Exception ex)
            //        {
            //            Assert.Fail($"Test aborted due to exception: {ex.Message}");
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Assert.Fail($"Test aborted due to exception: {ex.Message}");
            //    }
            //}
            //else
            //{
            //    Assert.Fail("mDevice is null");
            //}
        }

        [TestMethod]
        public void Test003_ConnectandTestCalibParamRead()
        {
            //need to work on calib dump 
            //if (mDevice != null)
            //{
            //    try
            //    {
            //        mDevice.StartConnectThread();
            //        Thread.Sleep(30000);
            //        if (!mDevice.IsConnected())
            //        {
            //            Assert.Fail();
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        Assert.Fail($"Test aborted due to exception: {ex.Message}");
            //    }
            //}
            //else
            //{
            //    Assert.Fail("mDevice is null");
            //}
        }

        [TestMethod]
        public void Test004_ConnectandTestDefaultLNAccelAndGyroCalibParam()
        {
            if (mDevice != null)
            {
                try
                {
                    mDevice.StartConnectThread();
                    mDevice.SetIsNewBMPSupported(true);
                    Thread.Sleep(30000);
                    if (!mDevice.IsConnected())
                    {
                        Assert.Fail();
                    }

                    //mDevice.ReadCalibrationParameters("All");
                    //mDevice.RetrieveKinematicCalibrationParametersFromPacket();

                    mDevice.WriteAccelRange(0);
                    mDevice.WriteSensors((int)ShimmerBluetooth.SensorBitmapShimmer3.SENSOR_A_ACCEL);
                    double[,] lnAccelOffset = mDevice.OffsetVectorAccel;
                    Assert.IsTrue(lnAccelOffset.Cast<double>().SequenceEqual(new double[,] { { 0 }, { 0 }, { 0 } }.Cast<double>()));

                    double[,] lnAccelAlignment = mDevice.AlignmentMatrixAccel;
                    Assert.IsTrue(lnAccelAlignment.Cast<double>().SequenceEqual(new double[,] { { -1, 0, 0 }, { 0, 1, 0 }, { 0, 0, -1 } }.Cast<double>()));

                    double[,] lnAccelSensitivity0 = mDevice.SensitivityMatrixAccel;
                    Assert.IsTrue(lnAccelSensitivity0.Cast<double>().SequenceEqual(new double[,] { { 1672, 0, 0 }, { 0, 1672, 0 }, { 0, 0, 1672 } }.Cast<double>()));

                    mDevice.WriteAccelRange(1);
                    double[,] lnAccelSensitivity1 = mDevice.SensitivityMatrixAccel;
                    Assert.IsTrue(lnAccelSensitivity1.Cast<double>().SequenceEqual(new double[,] { { 836, 0, 0 }, { 0, 836, 0 }, { 0, 0, 836 } }.Cast<double>()));

                    mDevice.WriteAccelRange(2);
                    double[,] lnAccelSensitivity2 = mDevice.SensitivityMatrixAccel;
                    Assert.IsTrue(lnAccelSensitivity2.Cast<double>().SequenceEqual(new double[,] { { 418, 0, 0 }, { 0, 418, 0 }, { 0, 0, 418 } }.Cast<double>()));

                    mDevice.WriteAccelRange(3);
                    double[,] lnAccelSensitivity3 = mDevice.SensitivityMatrixAccel;
                    Assert.IsTrue(lnAccelSensitivity3.Cast<double>().SequenceEqual(new double[,] { { 209, 0, 0 }, { 0, 209, 0 }, { 0, 0, 209 } }.Cast<double>()));


                    mDevice.WriteGyroRange(0);
                    double[,] gyroOffset = mDevice.OffsetVectorGyro;
                    Assert.IsTrue(gyroOffset.Cast<double>().SequenceEqual(new double[,] { { 1843 }, { 1843 }, { 1843 } }.Cast<double>()));

                    double[,] gyroAlignment = mDevice.AlignmentMatrixGyro;
                    Assert.IsTrue(gyroAlignment.Cast<double>().SequenceEqual(new double[,] { { 0, -1, 0 }, { -1, 0, 0 }, { 0, 0, -1 } }.Cast<double>()));

                    double[,] gyroSensitivity0 = mDevice.SensitivityMatrixGyro;
                    Assert.IsTrue(gyroSensitivity0.Cast<double>().SequenceEqual(new double[,] { { 2.73, 0, 0 }, { 0, 2.73, 0 }, { 0, 0, 2.73 } }.Cast<double>()));

                    mDevice.WriteGyroRange(1);
                    double[,] gyroSensitivity1 = mDevice.SensitivityMatrixGyro;
                    Assert.IsTrue(gyroSensitivity1.Cast<double>().SequenceEqual(new double[,] { { 2.73, 0, 0 }, { 0, 2.73, 0 }, { 0, 0, 2.73 } }.Cast<double>()));

                    mDevice.WriteGyroRange(2);
                    double[,] gyroSensitivity2 = mDevice.SensitivityMatrixGyro;
                    Assert.IsTrue(gyroSensitivity2.Cast<double>().SequenceEqual(new double[,] { { 2.73, 0, 0 }, { 0, 2.73, 0 }, { 0, 0, 2.73 } }.Cast<double>()));
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
        public void Test005_ConnectandTestDefaultWRAccelCalibParam()
        {
            //if (mDevice != null)
            //{
            //    try
            //    {
            //        mDevice.StartConnectThread();
            //        mDevice.SetIsNewBMPSupported(true);
            //        Thread.Sleep(30000);
            //        if (!mDevice.IsConnected())
            //        {
            //            Assert.Fail();
            //        }


            //    }
            //    catch (Exception ex)
            //    {
            //        Assert.Fail($"Test aborted due to exception: {ex.Message}");
            //    }
            //}
            //else
            //{
            //    Assert.Fail("mDevice is null");
            //}
        }
        [TestMethod]
        public void Test006_ConnectandTestDefaultMagCalibParam()
        {
            //if (mDevice != null)
            //{
            //    try
            //    {
            //        mDevice.StartConnectThread();
            //        mDevice.SetIsNewBMPSupported(true);
            //        Thread.Sleep(30000);
            //        if (!mDevice.IsConnected())
            //        {
            //            Assert.Fail();
            //        }


            //    }
            //    catch (Exception ex)
            //    {
            //        Assert.Fail($"Test aborted due to exception: {ex.Message}");
            //    }
            //}
            //else
            //{
            //    Assert.Fail("mDevice is null");
            //}
        }
        [TestMethod]
        public void Test007_ConnectandTestDefaultHighGAccelCalibParam()
        {
            //if (mDevice != null)
            //{
            //    try
            //    {
            //        mDevice.StartConnectThread();
            //        mDevice.SetIsNewBMPSupported(true);
            //        Thread.Sleep(30000);
            //        if (!mDevice.IsConnected())
            //        {
            //            Assert.Fail();
            //        }


            //    }
            //    catch (Exception ex)
            //    {
            //        Assert.Fail($"Test aborted due to exception: {ex.Message}");
            //    }
            //}
            //else
            //{
            //    Assert.Fail("mDevice is null");
            //}
        }
        [TestMethod]
        public void Test008_ConnectandTestDefaultWRMagCalibParam()
        {
            //if (mDevice != null)
            //{
            //    try
            //    {
            //        mDevice.StartConnectThread();
            //        mDevice.SetIsNewBMPSupported(true);
            //        Thread.Sleep(30000);
            //        if (!mDevice.IsConnected())
            //        {
            //            Assert.Fail();
            //        }


            //    }
            //    catch (Exception ex)
            //    {
            //        Assert.Fail($"Test aborted due to exception: {ex.Message}");
            //    }
            //}
            //else
            //{
            //    Assert.Fail("mDevice is null");
            //}
        }
    }
}