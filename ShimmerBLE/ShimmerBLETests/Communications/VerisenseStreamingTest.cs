using FakeItEasy;
using NUnit.Framework;
using shimmer.Models;
using ShimmerBLEAPI.Devices;
using ShimmerBLETests.Communications;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System;
using static shimmer.Models.OpConfigPayload;
using static shimmer.Models.ShimmerBLEEventData;
using System.Diagnostics;
using System.IO;
using shimmer.Sensors;
using ShimmerAPI;
namespace ShimmerBLETests
{
    class VerisenseStreamingTest
    {
        string uuid = "00000000-0000-0000-0000-000000000000";

        [Test]
        public async Task StreamDataState()
        {
            TestVerisenseBLEDevice VerisenseBLEDevice = new TestVerisenseBLEDevice(uuid, "");

            VerisenseBLEDevice.ShimmerBLEEvent += delegate (object sender, ShimmerBLEEventData e)
            {
                if (e.CurrentEvent == VerisenseBLEEvent.StateChange)
                {
                    Trace.WriteLine("Shimmer State: " + VerisenseBLEDevice.GetVerisenseBLEState().ToString());
                };

                if (e.CurrentEvent == VerisenseBLEEvent.NewDataPacket)
                {
                    Trace.WriteLine("New Packet: ");
                };
            };

            var result = await VerisenseBLEDevice.Connect(false);

            if (result && VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Connected)
            {
                await VerisenseBLEDevice.ExecuteRequest(RequestType.StartStreaming);

                if (VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Streaming)
                {
                    ((TestVerisenseBLEDevice)VerisenseBLEDevice).InjectRawPacketAccel1();
                    Assert.Pass();
                }
            }
        }
        [Test]
        public async Task StreamDataAccel1()
        {
            //Byte and bit index starts with 0
            //Enable Accel1 (Byte 1 = 0b10010111
            //Accel Range (byte 6 bit 4 and 5) = 0x20 = 0b00100000 = 8G
            byte[] defaultBytes = new byte[] { 0x5A, 0x97, 0x74, 0x00, 0x00, 0x30, 0x20, 0x00, 0x7F, 0x00, 0xD8, 0x0F, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0xF4, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0xFF, 0xFF, 0xAA, 0x01, 0x03, 0x3C, 0x00, 0x0E, 0x00, 0x00, 0x63, 0x28, 0xCC, 0xCC, 0x1E, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x00, 0x01 };
            Boolean packetCorrectValue = true;
            Boolean packetReceived = false;
            TestVerisenseBLEDevice VerisenseBLEDevice = new TestVerisenseBLEDevice(uuid, "", defaultBytes);
            VerisenseBLEDevice.ShimmerBLEEvent += delegate (object sender, ShimmerBLEEventData e)
            {
                if (e.CurrentEvent == VerisenseBLEEvent.StateChange)
                {
                    Trace.WriteLine("Shimmer State: " + VerisenseBLEDevice.GetVerisenseBLEState().ToString());
                };

                if (e.CurrentEvent == VerisenseBLEEvent.NewDataPacket)
                {
                    ObjectCluster ojc = ((ObjectCluster)e.ObjMsg);
                    var ax = ojc.GetData(SensorLIS2DW12.ObjectClusterSensorName.LIS2DW12_ACC_X, ShimmerConfiguration.SignalFormats.CAL).Data;
                    var ay = ojc.GetData(SensorLIS2DW12.ObjectClusterSensorName.LIS2DW12_ACC_Y, ShimmerConfiguration.SignalFormats.CAL).Data;
                    var az = ojc.GetData(SensorLIS2DW12.ObjectClusterSensorName.LIS2DW12_ACC_Z, ShimmerConfiguration.SignalFormats.CAL).Data;
                    packetReceived = true;
                    double xExpectedValue = -0.38285161599968776;
                    double yExpectedValue = -9.8775716927919444;
                    double zExpectedValue = -0.22971096959981263;
                    if (Math.Abs(ay - yExpectedValue) < 0.2
                    && Math.Abs(ax - xExpectedValue) < 0.2
                    && Math.Abs(az - zExpectedValue) < 0.2)
                    {
                        packetCorrectValue = true;
                    }
                    else
                    {
                        packetCorrectValue = false;
                    }
                };
            };

            var result = await VerisenseBLEDevice.Connect(false);
            if (result && VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Connected)
            {
                await VerisenseBLEDevice.ExecuteRequest(RequestType.StartStreaming);

                if (VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Streaming)
                {
                    try
                    {
                        ((TestVerisenseBLEDevice)VerisenseBLEDevice).InjectRawPacketAccel1();
                    }
                    catch (Exception ex)
                    {
                        Assert.Fail();
                    }
                }
            }

            if (packetCorrectValue && packetReceived)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public async Task StreamDataAccel2AndGyro()
        {
            //Byte and bit index starts with 0
            //Enable Accel2 and Guro (Byte 1 = 0b01110111 = 0x77)
            byte[] defaultBytes = new byte[] { 0x5A, 0x77, 0x74, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0xD8, 0x0F, 0x00, 0x00, 
                //start of byte index 14 below
                //Accel2 Range (byte 14 bit 3 and 2) = 0x04 = 0b00000100 = 16G
                //Gyro Range (byte 15 bit 3 and 2) = 0x0C = 0b00001100 = 200dps
                0x04, 0x0C, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0xF4, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0xFF, 0xFF, 0xAA, 0x01, 0x03, 0x3C, 0x00, 0x0E, 0x00, 0x00, 0x63, 0x28, 0xCC, 0xCC, 0x1E, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x00, 0x01 };
            Boolean packetCorrectValue = true;
            Boolean packetReceived = false;
            TestVerisenseBLEDevice VerisenseBLEDevice = new TestVerisenseBLEDevice(uuid, "", defaultBytes);
            VerisenseBLEDevice.ShimmerBLEEvent += delegate (object sender, ShimmerBLEEventData e)
            {
                if (e.CurrentEvent == VerisenseBLEEvent.StateChange)
                {
                    Trace.WriteLine("Shimmer State: " + VerisenseBLEDevice.GetVerisenseBLEState().ToString());
                };

                if (e.CurrentEvent == VerisenseBLEEvent.NewDataPacket)
                {
                    ObjectCluster ojc = ((ObjectCluster)e.ObjMsg);
                    var accel2x = ojc.GetData(SensorLSM6DS3.ObjectClusterSensorName.LSM6DS3_ACC_X, ShimmerConfiguration.SignalFormats.CAL).Data;
                    var accel2y = ojc.GetData(SensorLSM6DS3.ObjectClusterSensorName.LSM6DS3_ACC_Y, ShimmerConfiguration.SignalFormats.CAL).Data;
                    var accel2z = ojc.GetData(SensorLSM6DS3.ObjectClusterSensorName.LSM6DS3_ACC_Z, ShimmerConfiguration.SignalFormats.CAL).Data;
                    var gyrox = ojc.GetData(SensorLSM6DS3.ObjectClusterSensorName.LSM6DS3_GYRO_X, ShimmerConfiguration.SignalFormats.CAL).Data;
                    var gyroy = ojc.GetData(SensorLSM6DS3.ObjectClusterSensorName.LSM6DS3_GYRO_Y, ShimmerConfiguration.SignalFormats.CAL).Data;
                    var gyroz = ojc.GetData(SensorLSM6DS3.ObjectClusterSensorName.LSM6DS3_GYRO_Z, ShimmerConfiguration.SignalFormats.CAL).Data;
                    packetReceived = true;
                    double a2xExpectedValue = -0.057427742400090576;
                    double a2yExpectedValue = 9.5569334644150725;
                    double a2zExpectedValue = 0.83270226480131326;
                    double gxExpectedValue = 2.3099999999538006;
                    double gyExpectedValue = 3.4299999999314008;
                    double gzExpectedValue = 1.2599999999748004;
                    if (Math.Abs(accel2x - a2xExpectedValue) < 0.1
                    && Math.Abs(accel2y - a2yExpectedValue) < 0.2
                    && Math.Abs(accel2z - a2zExpectedValue) < 0.2
                    && Math.Abs(gyrox - gxExpectedValue) < 0.3
                    && Math.Abs(gyroy - gyExpectedValue) < 0.3
                    && Math.Abs(gyroz - gzExpectedValue) < 0.3)
                    {
                        packetCorrectValue = true;
                    }
                    else
                    {
                        packetCorrectValue = false;
                    }
                };
            };

            var result = await VerisenseBLEDevice.Connect(false);
            
            if (result && VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Connected)
            {
                await VerisenseBLEDevice.ExecuteRequest(RequestType.StartStreaming);

                if (VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Streaming)
                {
                    try
                    {
                        ((TestVerisenseBLEDevice)VerisenseBLEDevice).InjectRawPacketAccel2Gyro();
                    }
                    catch (Exception ex)
                    {
                        Assert.Fail();
                    }
                }
            }

            if (packetCorrectValue && packetReceived)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public async Task StreamDataGSR()
        {
            //Byte and bit index starts with 0
            //Disable all sensors (Byte 1 = 0b00010111 = 0x17)
            //Enable GSR (Byte 2 = 0b10000000 = 0x80)
            //Enable Batt (Byte 3 = 0b00000010 = 0x02)
            byte[] defaultBytes = new byte[] { 0x5A, 0x17, 0x80, 0x02, 0x00, 0x30, 0x20, 0x00, 0x7F, 0x00, 0xD8, 0x0F, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0xF4, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0xFF, 
            //GSR AUTO RANGE BYTE 51 = 0x04    
                0x04, 0xAA, 0x01, 0x03, 0x3C, 0x00, 0x0E, 0x00, 0x00, 0x63, 0x28, 0xCC, 0xCC, 0x1E, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x00, 0x01 };
            Boolean packetCorrectValue = true;
            Boolean packetReceived = false;
            TestVerisenseBLEDevice VerisenseBLEDevice = new TestVerisenseBLEDevice(uuid, "", defaultBytes);
            VerisenseBLEDevice.ShimmerBLEEvent += delegate (object sender, ShimmerBLEEventData e)
            {
                if (e.CurrentEvent == VerisenseBLEEvent.StateChange)
                {
                    Trace.WriteLine("Shimmer State: " + VerisenseBLEDevice.GetVerisenseBLEState().ToString());
                };

                if (e.CurrentEvent == VerisenseBLEEvent.NewDataPacket)
                {
                    ObjectCluster ojc = ((ObjectCluster)e.ObjMsg);
                    var gsrOhms = ojc.GetData(SensorGSR.ObjectClusterSensorName.GSR, ShimmerConfiguration.SignalFormats.CAL, "kOhms").Data;
                    var gsrSiemens = ojc.GetData(SensorGSR.ObjectClusterSensorName.GSR, ShimmerConfiguration.SignalFormats.CAL, "uSiemens").Data;
                    var batt = ojc.GetData(SensorGSR.ObjectClusterSensorName.Batt, ShimmerConfiguration.SignalFormats.CAL).Data;
                    packetReceived = true;
                    double gsrOhmsExpectedValue = 574.06005551349983;
                    double gsrSiemensExpectedValue = 1.7472527472527475;
                    double battExpectedValue = 1376.7032967032969;
                    if (Math.Abs(gsrOhms - gsrOhmsExpectedValue) < 10
                    && Math.Abs(gsrSiemens - gsrSiemensExpectedValue) < .3
                    && Math.Abs(batt - battExpectedValue) < 10)
                    {
                        packetCorrectValue = true;
                    }
                    else
                    {
                        packetCorrectValue = false;
                    }

                };
            };

            var result = await VerisenseBLEDevice.Connect(false);
            if (result && VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Connected)
            {
                await VerisenseBLEDevice.ExecuteRequest(RequestType.StartStreaming);

                if (VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Streaming)
                {
                    try
                    {
                        ((TestVerisenseBLEDevice)VerisenseBLEDevice).InjectRawPacketGSR();
                    }
                    catch (Exception ex)
                    {
                        Assert.Fail();
                    }
                }
            }

            if (packetCorrectValue && packetReceived)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public async Task StreamDataPPG()
        {
            //Byte and bit index starts with 0
            //Disable all sensors (Byte 1 = 0b00010111 = 0x17)
            //Enable PPG all four (Byte 2 = 0b01110100 = 0x74)
            byte[] defaultBytes = new byte[] { 0x5A, 0x17, 0x74, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0xD8, 0x0F, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0xF4, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0xFF, 0xFF, 0xAA, 0x01, 0x03, 0x3C, 0x00, 0x0E, 0x00, 0x00, 
                //PPG range 2
                0x20, 0x28, 0xCC, 0xCC, 0x1E, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x00, 0x01 };
            Boolean packetCorrectValue = true;
            Boolean packetReceived = false;
            TestVerisenseBLEDevice VerisenseBLEDevice = new TestVerisenseBLEDevice(uuid, "", defaultBytes);
            VerisenseBLEDevice.ShimmerBLEEvent += delegate (object sender, ShimmerBLEEventData e)
            {
                if (e.CurrentEvent == VerisenseBLEEvent.StateChange)
                {
                    Trace.WriteLine("Shimmer State: " + VerisenseBLEDevice.GetVerisenseBLEState().ToString());
                };

                if (e.CurrentEvent == VerisenseBLEEvent.NewDataPacket)
                {
                    ObjectCluster ojc = ((ObjectCluster)e.ObjMsg);
                    var ppgBlue = ojc.GetData(SensorPPG.ObjectClusterSensorName.PPG_BLUE, ShimmerConfiguration.SignalFormats.CAL).Data;
                    var ppgRed = ojc.GetData(SensorPPG.ObjectClusterSensorName.PPG_RED, ShimmerConfiguration.SignalFormats.CAL).Data;
                    var ppgGreen = ojc.GetData(SensorPPG.ObjectClusterSensorName.PPG_GREEN, ShimmerConfiguration.SignalFormats.CAL).Data;
                    var ppgIR = ojc.GetData(SensorPPG.ObjectClusterSensorName.PPG_IR, ShimmerConfiguration.SignalFormats.CAL).Data;
                    packetReceived = true;
                    double ppgBlueExpectedValue = 60.60107421875;
                    double ppgRedExpectedValue = 60.60107421875;
                    double ppgGreenExpectedValue = 60.60107421875;
                    double ppgIRExpectedValue = 60.60107421875;
                    if (Math.Abs(ppgBlue - ppgBlueExpectedValue) == 0
                    && Math.Abs(ppgRed - ppgRedExpectedValue) == 0
                    && Math.Abs(ppgGreen - ppgGreenExpectedValue) == 0
                    && Math.Abs(ppgIR - ppgIRExpectedValue) == 0 )
                    {
                        packetCorrectValue = true;
                    }
                    else
                    {
                        packetCorrectValue = false;
                    }

                };
            };

            var result = await VerisenseBLEDevice.Connect(false);

            if (result && VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Connected)
            {
                await VerisenseBLEDevice.ExecuteRequest(RequestType.StartStreaming);

                if (VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Streaming)
                {
                    try
                    {
                        ((TestVerisenseBLEDevice)VerisenseBLEDevice).InjectRawPacketPPG();
                    }
                    catch (Exception ex)
                    {
                        Assert.Fail();
                    }
                }
            }

            if (packetCorrectValue && packetReceived)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Note the current maximum mtu supported is 5 (refer to data sync , offset = 5)
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task StreamDataMTUSize()
        {
            byte[] defaultBytes = new byte[] { 0x5A, 0x97, 0x74, 0x00, 0x00, 0x30, 0x20, 0x00, 0x7F, 0x00, 0xD8, 0x0F, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0xF4, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0xFF, 0xFF, 0xAA, 0x01, 0x03, 0x3C, 0x00, 0x0E, 0x00, 0x00, 0x63, 0x28, 0xCC, 0xCC, 0x1E, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x00, 0x01 };

            TestVerisenseBLEDevice VerisenseBLEDevice = new TestVerisenseBLEDevice(uuid, "", defaultBytes);
            Boolean PacketReceived = false;
            VerisenseBLEDevice.ShimmerBLEEvent += delegate (object sender, ShimmerBLEEventData e)
            {
                if (e.CurrentEvent == VerisenseBLEEvent.StateChange)
                {
                    Trace.WriteLine("Shimmer State: " + VerisenseBLEDevice.GetVerisenseBLEState().ToString());
                };

                if (e.CurrentEvent == VerisenseBLEEvent.NewDataPacket)
                {
                    Trace.WriteLine("New Packet: ");
                    PacketReceived = true;
                };
            };

            var result = await VerisenseBLEDevice.Connect(false);
            VerisenseBLEDevice cloneDevice = new VerisenseBLEDevice(VerisenseBLEDevice);
            var sensor = cloneDevice.GetSensor(SensorLIS2DW12.SensorName);
            ((SensorLIS2DW12)sensor).SetAccelEnabled(true);
            ((SensorLIS2DW12)sensor).SetAccelRange(SensorLIS2DW12.AccelRange.Range_8G);
            byte[] opconfigBytes = cloneDevice.GenerateConfigurationBytes();
            await VerisenseBLEDevice.ExecuteRequest(RequestType.WriteOperationalConfig, opconfigBytes);

            if (result && VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Connected)
            {
                await VerisenseBLEDevice.ExecuteRequest(RequestType.StartStreaming);

                if (VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Streaming)
                {
                    try
                    {
                        ((TestVerisenseBLEDevice)VerisenseBLEDevice).InjectRawPacket(5);
                    }
                    catch (Exception ex)
                    {
                        Assert.Fail();
                    }
                }
            }
            if (!PacketReceived)
            {
                Assert.Fail();
            }
            else
            {
                Assert.Pass();
            }
        }

    }
}
