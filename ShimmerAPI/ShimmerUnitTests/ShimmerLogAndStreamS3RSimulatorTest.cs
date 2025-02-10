using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShimmerAPI;
using ShimmerAPI.Sensors;
using ShimmerAPI.Simulators;
using ShimmerAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static ShimmerAPI.ShimmerBluetooth;
using System.Runtime.Serialization;

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

                    if (!mDevice.GetFirmwareVersionFullName().Equals("LogAndStream 0.0.1"))
                    {
                        Assert.Fail();
                    }

                    if (!mDevice.GetShimmerVersion().Equals(10))
                    {
                        Assert.Fail();
                    }

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
        public void Test002_ConnectandTestBMP390()
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

                    try
                    {
                        mDevice.CalculateBMP390PressureCalibrationCoefficientsResponse(mDevice.GetPressureResoTest());

                        string[] sensorDataType = { "u24" };

                        byte[] sensorDataP2 = { 0x00, 0x17, 0x64 };
                        byte[] sensorDataT2 = { 0x00, 0xCF, 0x7F };

                        long[] uncalibResultP2 = ProgrammerUtilities.ParseData(sensorDataP2, sensorDataType);
                        long[] uncalibResultT2 = ProgrammerUtilities.ParseData(sensorDataT2, sensorDataType);
                        double[] bmpX80caldata2 = new double[2];
                        bmpX80caldata2 = mDevice.CalibratePressure390SensorData(uncalibResultP2[0], uncalibResultT2[0]);
                        Bmp3QuantizedCalibData.TLin = bmpX80caldata2[1];

                        //Assert.AreEqual(resultP2, 100912.81758676282);
                        //Assert.AreEqual(resultT2, 23.26587201654911);
                        Assert.AreEqual(Math.Round(bmpX80caldata2[0], 4), 100912.8176);
                        Assert.AreEqual(Math.Round(bmpX80caldata2[1], 4), 23.2659);
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
                    byte[] deviceCalBytes = mDevice.CalibByteDumpGenerate();

                    mDevice.WriteLNAccelRange(0);
                    mDevice.CalibByteDumpParse(deviceCalBytes);
                    double[,] lnAccelOffset = mDevice.OffsetVectorAccel;
                    Assert.IsTrue(lnAccelOffset.Cast<double>().SequenceEqual(new double[,] { { 0 }, { 0 }, { 0 } }.Cast<double>()));

                    double[,] lnAccelAlignment = mDevice.AlignmentMatrixAccel; //it become -0.1
                    Assert.IsTrue(lnAccelAlignment.Cast<double>().SequenceEqual(new double[,] { { -1, 0, 0 }, { 0, 1, 0 }, { 0, 0, -1 } }.Cast<double>()));

                    double[,] lnAccelSensitivity0 = mDevice.SensitivityMatrixAccel;
                    Assert.IsTrue(lnAccelSensitivity0.Cast<double>().SequenceEqual(new double[,] { { 1672, 0, 0 }, { 0, 1672, 0 }, { 0, 0, 1672 } }.Cast<double>()));

                    mDevice.WriteLNAccelRange(1);
                    mDevice.CalibByteDumpParse(deviceCalBytes);
                    double[,] lnAccelSensitivity1 = mDevice.SensitivityMatrixAccel;
                    Assert.IsTrue(lnAccelSensitivity1.Cast<double>().SequenceEqual(new double[,] { { 836, 0, 0 }, { 0, 836, 0 }, { 0, 0, 836 } }.Cast<double>()));

                    mDevice.WriteLNAccelRange(2);
                    mDevice.CalibByteDumpParse(deviceCalBytes);
                    double[,] lnAccelSensitivity2 = mDevice.SensitivityMatrixAccel;
                    Assert.IsTrue(lnAccelSensitivity2.Cast<double>().SequenceEqual(new double[,] { { 418, 0, 0 }, { 0, 418, 0 }, { 0, 0, 418 } }.Cast<double>()));

                    mDevice.WriteLNAccelRange(3);
                    mDevice.CalibByteDumpParse(deviceCalBytes);
                    double[,] lnAccelSensitivity3 = mDevice.SensitivityMatrixAccel;
                    Assert.IsTrue(lnAccelSensitivity3.Cast<double>().SequenceEqual(new double[,] { { 209, 0, 0 }, { 0, 209, 0 }, { 0, 0, 209 } }.Cast<double>()));


                    mDevice.WriteGyroRange(0);
                    mDevice.CalibByteDumpParse(deviceCalBytes);
                    double[,] gyroOffset = mDevice.OffsetVectorGyro;
                    Assert.IsTrue(gyroOffset.Cast<double>().SequenceEqual(new double[,] { { 0 }, { 0 }, { 0 } }.Cast<double>()));

                    double[,] gyroAlignment = mDevice.AlignmentMatrixGyro;
                    Assert.IsTrue(gyroAlignment.Cast<double>().SequenceEqual(new double[,] { { -1, 0, 0 }, { 0, 1, 0 }, { 0, 0, -1 } }.Cast<double>()));

                    double[,] gyroSensitivity0 = mDevice.SensitivityMatrixGyro;
                    Assert.IsTrue(gyroSensitivity0.Cast<double>().SequenceEqual(new double[,] { { 229, 0, 0 }, { 0, 229, 0 }, { 0, 0, 229 } }.Cast<double>()));

                    mDevice.WriteGyroRange(1);
                    mDevice.CalibByteDumpParse(deviceCalBytes);
                    double[,] gyroSensitivity1 = mDevice.SensitivityMatrixGyro;
                    Assert.IsTrue(gyroSensitivity1.Cast<double>().SequenceEqual(new double[,] { { 114, 0, 0 }, { 0, 114, 0 }, { 0, 0, 114 } }.Cast<double>()));

                    mDevice.WriteGyroRange(2);
                    mDevice.CalibByteDumpParse(deviceCalBytes);
                    double[,] gyroSensitivity2 = mDevice.SensitivityMatrixGyro;
                    Assert.IsTrue(gyroSensitivity2.Cast<double>().SequenceEqual(new double[,] { { 57, 0, 0 }, { 0, 57, 0 }, { 0, 0, 57 } }.Cast<double>()));

                    mDevice.WriteGyroRange(3);
                    mDevice.CalibByteDumpParse(deviceCalBytes);
                    double[,] gyroSensitivity3 = mDevice.SensitivityMatrixGyro;
                    Assert.IsTrue(gyroSensitivity3.Cast<double>().SequenceEqual(new double[,] { { 29, 0, 0 }, { 0, 29, 0 }, { 0, 0, 29 } }.Cast<double>()));

                    mDevice.WriteGyroRange(4);
                    mDevice.CalibByteDumpParse(deviceCalBytes);
                    double[,] gyroSensitivity4 = mDevice.SensitivityMatrixGyro;
                    Assert.IsTrue(gyroSensitivity4.Cast<double>().SequenceEqual(new double[,] { { 14, 0, 0 }, { 0, 14, 0 }, { 0, 0, 14 } }.Cast<double>()));

                    mDevice.WriteGyroRange(5);
                    mDevice.CalibByteDumpParse(deviceCalBytes);
                    double[,] gyroSensitivity5 = mDevice.SensitivityMatrixGyro;
                    Assert.IsTrue(gyroSensitivity5.Cast<double>().SequenceEqual(new double[,] { { 7, 0, 0 }, { 0, 7, 0 }, { 0, 0, 7 } }.Cast<double>()));

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

                    byte[] deviceCalBytes = mDevice.CalibByteDumpGenerate();
                    mDevice.CalibByteDumpParse(deviceCalBytes);

                    double[,] wrAccelOffset = mDevice.OffsetVectorAccel2;
                    Assert.IsTrue(wrAccelOffset.Cast<double>().SequenceEqual(new double[,] { { 0 }, { 0 }, { 0 } }.Cast<double>()));
                    double[,] wrAccelAlignment = mDevice.AlignmentMatrixAccel2;
                    Assert.IsTrue(wrAccelAlignment.Cast<double>().SequenceEqual(new double[,] { { 0, -1, 0 }, { -1, 0, 0 }, { 0, 0, -1 } }.Cast<double>()));

                    mDevice.WriteAccelRange(0);
                    mDevice.CalibByteDumpParse(deviceCalBytes);
                    double[,] wrAccelSensitivity0 = mDevice.SensitivityMatrixAccel2;
                    Assert.IsTrue(wrAccelSensitivity0.Cast<double>().SequenceEqual(new double[,] { { 1671, 0, 0 }, { 0, 1671, 0 }, { 0, 0, 1671 } }.Cast<double>()));

                    mDevice.WriteAccelRange(1);
                    mDevice.CalibByteDumpParse(deviceCalBytes);
                    double[,] wrAccelSensitivity1 = mDevice.SensitivityMatrixAccel2;
                    Assert.IsTrue(wrAccelSensitivity1.Cast<double>().SequenceEqual(new double[,] { { 836, 0, 0 }, { 0, 836, 0 }, { 0, 0, 836 } }.Cast<double>()));

                    mDevice.WriteAccelRange(2);
                    mDevice.CalibByteDumpParse(deviceCalBytes);
                    double[,] wrAccelSensitivity2 = mDevice.SensitivityMatrixAccel2;
                    Assert.IsTrue(wrAccelSensitivity2.Cast<double>().SequenceEqual(new double[,] { { 418, 0, 0 }, { 0, 418, 0 }, { 0, 0, 418 } }.Cast<double>()));

                    mDevice.WriteAccelRange(3);
                    mDevice.CalibByteDumpParse(deviceCalBytes);
                    double[,] wrAccelSensitivity3 = mDevice.SensitivityMatrixAccel2;
                    Assert.IsTrue(wrAccelSensitivity3.Cast<double>().SequenceEqual(new double[,] { { 209, 0, 0 }, { 0, 209, 0 }, { 0, 0, 209 } }.Cast<double>()));



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
        public void Test006_ConnectandTestDefaultMagCalibParam()
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

                    byte[] deviceCalBytes = mDevice.CalibByteDumpGenerate();
                    mDevice.CalibByteDumpParse(deviceCalBytes);

                    double[,] magOffset = mDevice.OffsetVectorMag;
                    Assert.IsTrue(magOffset.Cast<double>().SequenceEqual(new double[,] { { 0 }, { 0 }, { 0 } }.Cast<double>()));
                    double[,] magAlignment = mDevice.AlignmentMatrixMag;
                    Assert.IsTrue(magAlignment.Cast<double>().SequenceEqual(new double[,] { { 1, 0, 0 }, { 0, -1, 0 }, { 0, 0, -1 } }.Cast<double>()));

                    mDevice.WriteMagRange(0);
                    mDevice.CalibByteDumpParse(deviceCalBytes);
                    double[,] magSensitivity0 = mDevice.SensitivityMatrixMag;
                    Assert.IsTrue(magSensitivity0.Cast<double>().SequenceEqual(new double[,] { { 6842, 0, 0 }, { 0, 6842, 0 }, { 0, 0, 6842 } }.Cast<double>()));

                    mDevice.WriteMagRange(1);
                    mDevice.CalibByteDumpParse(deviceCalBytes);
                    double[,] magSensitivity1 = mDevice.SensitivityMatrixMag;
                    Assert.IsTrue(magSensitivity1.Cast<double>().SequenceEqual(new double[,] { { 3421, 0, 0 }, { 0, 3421, 0 }, { 0, 0, 3421 } }.Cast<double>()));

                    mDevice.WriteMagRange(2);
                    mDevice.CalibByteDumpParse(deviceCalBytes);
                    double[,] magSensitivity2 = mDevice.SensitivityMatrixMag;
                    Assert.IsTrue(magSensitivity2.Cast<double>().SequenceEqual(new double[,] { { 2281, 0, 0 }, { 0, 2281, 0 }, { 0, 0, 2281 } }.Cast<double>()));

                    mDevice.WriteMagRange(3);
                    mDevice.CalibByteDumpParse(deviceCalBytes);
                    double[,] magSensitivity3 = mDevice.SensitivityMatrixMag;
                    Assert.IsTrue(magSensitivity3.Cast<double>().SequenceEqual(new double[,] { { 1711, 0, 0 }, { 0, 1711, 0 }, { 0, 0, 1711 } }.Cast<double>()));

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
        public void Test007_ConnectandTestDefaultHighGAccelCalibParam()
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

                    byte[] deviceCalBytes = mDevice.CalibByteDumpGenerate();
                    mDevice.CalibByteDumpParse(deviceCalBytes);

                    double[,] altAccelOffset = mDevice.OffsetVectorAltAccel;
                    Assert.IsTrue(altAccelOffset.Cast<double>().SequenceEqual(new double[,] { { 10 }, { 10 }, { 10 } }.Cast<double>()));
                    double[,] altAccelAlignment = mDevice.AlignmentMatrixAltAccel;
                    Assert.IsTrue(altAccelAlignment.Cast<double>().SequenceEqual(new double[,] { { 0, 1, 0 }, { 1, 0, 0 }, { 0, 0, -1 } }.Cast<double>()));
                    double[,] altAccelSensitivity0 = mDevice.SensitivityMatrixAltAccel;
                    Assert.IsTrue(altAccelSensitivity0.Cast<double>().SequenceEqual(new double[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } }.Cast<double>()));

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
        public void Test008_ConnectandTestDefaultWRMagCalibParam()
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

                    byte[] deviceCalBytes = mDevice.CalibByteDumpGenerate();
                    mDevice.CalibByteDumpParse(deviceCalBytes);

                    double[,] wrMagOffset = mDevice.OffsetVectorMag2;
                    Assert.IsTrue(wrMagOffset.Cast<double>().SequenceEqual(new double[,] { { 0 }, { 0 }, { 0 } }.Cast<double>()));
                    double[,] wrMagAlignment = mDevice.AlignmentMatrixMag2;
                    Assert.IsTrue(wrMagAlignment.Cast<double>().SequenceEqual(new double[,] { { -1, 0, 0 }, { 0, -1, 0 }, { 0, 0, -1 } }.Cast<double>()));
                    double[,] wrMagSensitivity0 = mDevice.SensitivityMatrixMag2;
                    Assert.IsTrue(wrMagSensitivity0.Cast<double>().SequenceEqual(new double[,] { { 667, 0, 0 }, { 0, 667, 0 }, { 0, 0, 667 } }.Cast<double>()));

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