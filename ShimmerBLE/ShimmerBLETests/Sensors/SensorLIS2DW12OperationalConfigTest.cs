using NUnit.Framework;
using shimmer.Sensors;
using ShimmerBLEAPI.Devices;
using ShimmerBLETests.Communications;
using System;
using System.Collections.Generic;
using System.Text;
using static shimmer.Models.OpConfigPayload;

namespace ShimmerBLETests
{
    class SensorLIS2DW12OperationalConfigTest
    {
        string uuid = "00000000-0000-0000-0000-daa619f04ad7";
        readonly string deviceName = "device";
        readonly byte[] defaultBytes = new byte[] { 0x5A, 0x97, 0x74, 0x00, 0x00, 0x30, 0x20, 0x00, 0x7F, 0x00, 0xD8, 0x0F, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0xF4, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0xFF, 0xFF, 0xAA, 0x01, 0x03, 0x3C, 0x00, 0x0E, 0x00, 0x00, 0x63, 0x28, 0xCC, 0xCC, 0x1E, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x00, 0x01 };

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestAccel1OpConfigEnabled()
        {
            var bytes = CopyDefaultBytes();
            bytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] = (byte)(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] | 0b10000000);
            var sensor = CreateDeviceAndReturnSensor(bytes);
            if (sensor.IsAccelEnabled() == true)
            {
                Assert.Pass();

            } else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestAccel1OpConfigGetAccelRange()
        {
            var opconfig = CopyDefaultBytes();

            //set the range to 2G
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] = (byte)(opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] & 0b11001111);
            SensorLIS2DW12 sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetAccelRange() != SensorLIS2DW12.AccelRange.Range_2G)
            {
                Assert.Fail();
            }

            //set the range to 4G
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] & 0b11001111) | 0b00010000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetAccelRange() != SensorLIS2DW12.AccelRange.Range_4G)
            {
                Assert.Fail();
            }

            //set the range to 8G
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] & 0b11001111) | 0b00100000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetAccelRange() != SensorLIS2DW12.AccelRange.Range_8G)
            {
                Assert.Fail();
            }

            //set the range to 16G
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] & 0b11001111) | 0b00110000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetAccelRange() != SensorLIS2DW12.AccelRange.Range_16G)
            {
                Assert.Fail();
            }
        }
        
        [Test]
        public void TestAccel1OpConfigSetAccelRange()
        {
            var opconfig = CopyDefaultBytes();

            // set the accel range to 2G
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] = (byte)(opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] & 0b11001111);
            SensorLIS2DW12 sensor = CreateDeviceAndReturnSensor(opconfig);

            // set the accel range to 4G
            sensor.SetAccelRange(SensorLIS2DW12.AccelRange.Range_4G);
            if (sensor.GetAccelRange() != SensorLIS2DW12.AccelRange.Range_4G)
            {
                Assert.Fail();
            }

            // set the accel range to 8G
            sensor.SetAccelRange(SensorLIS2DW12.AccelRange.Range_8G);
            if (sensor.GetAccelRange() != SensorLIS2DW12.AccelRange.Range_8G)
            {
                Assert.Fail();
            }

            // set the accel range to 16G
            sensor.SetAccelRange(SensorLIS2DW12.AccelRange.Range_16G);
            if (sensor.GetAccelRange() != SensorLIS2DW12.AccelRange.Range_16G)
            {
                Assert.Fail();
            }

            // set the accel range to 2G
            sensor.SetAccelRange(SensorLIS2DW12.AccelRange.Range_2G);
            if (sensor.GetAccelRange() != SensorLIS2DW12.AccelRange.Range_2G)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestAccel1GenOpConfigAccelRange()
        {
            var opconfig = CopyDefaultBytes();

            VerisenseBLEDevice clone = new TestVerisenseBLEDevice(uuid, "", opconfig);

            // set the accel range to 2G in clone
            ((SensorLIS2DW12)clone.GetSensor(SensorLIS2DW12.SensorName)).SetAccelRange(SensorLIS2DW12.AccelRange.Range_2G);
            // check if the bit equals to 00
            byte[] opconfigbytes = clone.GenerateConfigurationBytes();
            if ((opconfigbytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] >> 4 & 0b0011) != 0)
            {
                Assert.Fail();
            }

            // set the accel range to 4G in clone
            ((SensorLIS2DW12)clone.GetSensor(SensorLIS2DW12.SensorName)).SetAccelRange(SensorLIS2DW12.AccelRange.Range_4G);
            // check if the bit equals to 01
            opconfigbytes = clone.GenerateConfigurationBytes();
            if ((opconfigbytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] >> 4 & 0b0011) != 1)
            {
                Assert.Fail();
            }

            // set the accel range to 8G in clone
            ((SensorLIS2DW12)clone.GetSensor(SensorLIS2DW12.SensorName)).SetAccelRange(SensorLIS2DW12.AccelRange.Range_8G);
            // check if the bit equals to 10
            opconfigbytes = clone.GenerateConfigurationBytes();
            if ((opconfigbytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] >> 4 & 0b0011) != 2)
            {
                Assert.Fail();
            }

            // set the accel range to 16G in clone
            ((SensorLIS2DW12)clone.GetSensor(SensorLIS2DW12.SensorName)).SetAccelRange(SensorLIS2DW12.AccelRange.Range_16G);
            // check if the bit equals to 11
            opconfigbytes = clone.GenerateConfigurationBytes();
            if ((opconfigbytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] >> 4 & 0b0011) != 3)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestAccel1GetMode()
        {
            var opconfig = CopyDefaultBytes();

            //set mode to low power mode
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] = (byte)(opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b11110011);
            SensorLIS2DW12 sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetMode() != SensorLIS2DW12.Mode.Low_Power_Mode)
            {
                Assert.Fail();
            }

            //set mode to high performance mode
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b11110011) | 0b00000100);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetMode() != SensorLIS2DW12.Mode.High_Performance_Mode)
            {
                Assert.Fail();
            }

            //set mode to on demand
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b11110011) | 0b00001000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetMode() != SensorLIS2DW12.Mode.On_Demand)
            {
                Assert.Fail();
            }

            //set mode to reserved
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b11110011) | 0b00001100);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetMode() != SensorLIS2DW12.Mode.Reserved)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestAccel1GetAccelRate()
        {
            var opconfig = CopyDefaultBytes();

            //set the bit to 0000
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] = (byte)(opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b00001111);
            SensorLIS2DW12 sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetMode() == SensorLIS2DW12.Mode.Low_Power_Mode)
            {
				if (sensor.GetSamplingRate() != SensorLIS2DW12.LowPerformanceAccelSamplingRate.Power_Down)
                {
                    Assert.Fail();
                }
			} else if (sensor.GetMode() == SensorLIS2DW12.Mode.High_Performance_Mode)
            {
                if (sensor.GetSamplingRate() != SensorLIS2DW12.HighPerformanceAccelSamplingRate.Power_Down)
                {
                    Assert.Fail();
                }
			}

            //set the bit to 0001
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b00001111) | 0b00010000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetMode() == SensorLIS2DW12.Mode.Low_Power_Mode)
            {
                if (sensor.GetSamplingRate() != SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_1_6Hz)
                {
                    Assert.Fail();
                }
            }
            else if (sensor.GetMode() == SensorLIS2DW12.Mode.High_Performance_Mode)
            {
                if (sensor.GetSamplingRate() != SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_12_5Hz)
                {
                    Assert.Fail();
                }
            }

            //set the bit to 0010
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b00001111) | 0b00100000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetMode() == SensorLIS2DW12.Mode.Low_Power_Mode)
            {

                if (sensor.GetSamplingRate() != SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_12_5Hz)
                {
                    Assert.Fail();
                }
            }
            else if (sensor.GetMode() == SensorLIS2DW12.Mode.High_Performance_Mode)
            {

                if (sensor.GetSamplingRate() != SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_12_5Hz)
                {
                    Assert.Fail();
                }
            }

            //set the bit to 0011
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b00001111) | 0b00110000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetMode() == SensorLIS2DW12.Mode.Low_Power_Mode)
            {

                if (sensor.GetSamplingRate() != SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_25Hz)
                {
                    Assert.Fail();
                }
            }
            else if (sensor.GetMode() == SensorLIS2DW12.Mode.High_Performance_Mode)
            {

                if (sensor.GetSamplingRate() != SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_25Hz)
                {
                    Assert.Fail();
                }
            }

            //set the bit to 0100
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b00001111) | 0b01000000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetMode() == SensorLIS2DW12.Mode.Low_Power_Mode)
            {

                if (sensor.GetSamplingRate() != SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_50Hz)
                {
                    Assert.Fail();
                }
            }
            else if (sensor.GetMode() == SensorLIS2DW12.Mode.High_Performance_Mode)
            {

                if (sensor.GetSamplingRate() != SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_50Hz)
                {
                    Assert.Fail();
                }
            }

            //set the bit to 0101
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b00001111) | 0b01010000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetMode() == SensorLIS2DW12.Mode.Low_Power_Mode)
            {

                if (sensor.GetSamplingRate() != SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_100Hz)
                {
                    Assert.Fail();
                }
            }
            else if (sensor.GetMode() == SensorLIS2DW12.Mode.High_Performance_Mode)
            {

                if (sensor.GetSamplingRate() != SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_100Hz)
                {
                    Assert.Fail();
                }
            }

            //set the bit to 0110
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b00001111) | 0b01100000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetMode() == SensorLIS2DW12.Mode.Low_Power_Mode)
            {

                if (sensor.GetSamplingRate() != SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_200Hz)
                {
                    Assert.Fail();
                }
            }
            else if (sensor.GetMode() == SensorLIS2DW12.Mode.High_Performance_Mode)
            {

                if (sensor.GetSamplingRate() != SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_200Hz)
                {
                    Assert.Fail();
                }
            }

            //set the bit to 0111
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b00001111) | 0b01110000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetMode() == SensorLIS2DW12.Mode.Low_Power_Mode)
            {

                if (sensor.GetSamplingRate() != SensorLIS2DW12.LowPerformanceAccelSamplingRate.Rate_Unknown)
                {
                    Assert.Fail();
                }
            }
            else if (sensor.GetMode() == SensorLIS2DW12.Mode.High_Performance_Mode)
            {

                if (sensor.GetSamplingRate() != SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_400Hz)
                {
                    Assert.Fail();
                }
            }

            //set the bit to 1000
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b00001111) | 0b10000000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetMode() == SensorLIS2DW12.Mode.Low_Power_Mode)
            {

                if (sensor.GetSamplingRate() != SensorLIS2DW12.LowPerformanceAccelSamplingRate.Rate_Unknown)
                {
                    Assert.Fail();
                }
            }
            else if (sensor.GetMode() == SensorLIS2DW12.Mode.High_Performance_Mode)
            {

                if (sensor.GetSamplingRate() != SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_800Hz)
                {
                    Assert.Fail();
                }
            }

            //set the bit to 1001
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b00001111) | 0b10010000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetMode() == SensorLIS2DW12.Mode.Low_Power_Mode)
            {

                if (sensor.GetSamplingRate() != SensorLIS2DW12.LowPerformanceAccelSamplingRate.Rate_Unknown)
                {
                    Assert.Fail();
                }
            }
            else if (sensor.GetMode() == SensorLIS2DW12.Mode.High_Performance_Mode)
            {

                if (sensor.GetSamplingRate() != SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_1600Hz)
                {
                    Assert.Fail();
                }
            }
        }

        [Test]
        public void TestAccel1GenOpConfigAccelRate()
        {
            var opconfig = CopyDefaultBytes();

            VerisenseBLEDevice clone = new TestVerisenseBLEDevice(uuid, "", opconfig);

            // LowPerformanceAccelSamplingRate

            // set the accel rate to power down
            ((SensorLIS2DW12)clone.GetSensor(SensorLIS2DW12.SensorName)).SetSamplingRate(SensorLIS2DW12.LowPerformanceAccelSamplingRate.Power_Down);
            byte[] opconfigbytes = clone.GenerateConfigurationBytes();
            if ((opconfigbytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] >> 4 & 0b1111) != 0)
            {
                Assert.Fail();
            }

            // set the accel rate to Freq_1_6Hz
            ((SensorLIS2DW12)clone.GetSensor(SensorLIS2DW12.SensorName)).SetSamplingRate(SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_1_6Hz);
            opconfigbytes = clone.GenerateConfigurationBytes();
            if ((opconfigbytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] >> 4 & 0b1111) != 1)
            {
                Assert.Fail();
            }

            // set the accel rate to Freq_12_5Hz
            ((SensorLIS2DW12)clone.GetSensor(SensorLIS2DW12.SensorName)).SetSamplingRate(SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_12_5Hz);
            opconfigbytes = clone.GenerateConfigurationBytes();
            if ((opconfigbytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] >> 4 & 0b1111) != 2)
            {
                Assert.Fail();
            }

            // set the accel rate to Freq_25Hz
            ((SensorLIS2DW12)clone.GetSensor(SensorLIS2DW12.SensorName)).SetSamplingRate(SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_25Hz);
            opconfigbytes = clone.GenerateConfigurationBytes();
            if ((opconfigbytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] >> 4 & 0b1111) != 3)
            {
                Assert.Fail();
            }

            // set the accel rate to Freq_50Hz
            ((SensorLIS2DW12)clone.GetSensor(SensorLIS2DW12.SensorName)).SetSamplingRate(SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_50Hz);
            opconfigbytes = clone.GenerateConfigurationBytes();
            if ((opconfigbytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] >> 4 & 0b1111) != 4)
            {
                Assert.Fail();
            }

            // set the accel rate to Freq_100Hz
            ((SensorLIS2DW12)clone.GetSensor(SensorLIS2DW12.SensorName)).SetSamplingRate(SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_100Hz);
            opconfigbytes = clone.GenerateConfigurationBytes();
            if ((opconfigbytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] >> 4 & 0b1111) != 5)
            {
                Assert.Fail();
            }

            // set the accel rate to Freq_200Hz
            ((SensorLIS2DW12)clone.GetSensor(SensorLIS2DW12.SensorName)).SetSamplingRate(SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_200Hz);
            opconfigbytes = clone.GenerateConfigurationBytes();
            if ((opconfigbytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] >> 4 & 0b1111) != 6)
            {
                Assert.Fail();
            }

            // HighPerformanceAccelSamplingRate

            // set the accel rate to power down
            ((SensorLIS2DW12)clone.GetSensor(SensorLIS2DW12.SensorName)).SetSamplingRate(SensorLIS2DW12.HighPerformanceAccelSamplingRate.Power_Down);
            opconfigbytes = clone.GenerateConfigurationBytes();
            if ((opconfigbytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] >> 4 & 0b1111) != 0)
            {
                Assert.Fail();
            }

            // set the accel rate to Freq_12_5Hz
            ((SensorLIS2DW12)clone.GetSensor(SensorLIS2DW12.SensorName)).SetSamplingRate(SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_12_5Hz);
            opconfigbytes = clone.GenerateConfigurationBytes();
            if ((opconfigbytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] >> 4 & 0b1111) != 1)
            {
                Assert.Fail();
            }

            // set the accel rate to Freq_25Hz
            ((SensorLIS2DW12)clone.GetSensor(SensorLIS2DW12.SensorName)).SetSamplingRate(SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_25Hz);
            opconfigbytes = clone.GenerateConfigurationBytes();
            if ((opconfigbytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] >> 4 & 0b1111) != 3)
            {
                Assert.Fail();
            }

            // set the accel rate to Freq_50Hz
            ((SensorLIS2DW12)clone.GetSensor(SensorLIS2DW12.SensorName)).SetSamplingRate(SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_50Hz);
            opconfigbytes = clone.GenerateConfigurationBytes();
            if ((opconfigbytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] >> 4 & 0b1111) != 4)
            {
                Assert.Fail();
            }

            // set the accel rate to Freq_100Hz
            ((SensorLIS2DW12)clone.GetSensor(SensorLIS2DW12.SensorName)).SetSamplingRate(SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_100Hz);
            opconfigbytes = clone.GenerateConfigurationBytes();
            if ((opconfigbytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] >> 4 & 0b1111) != 5)
            {
                Assert.Fail();
            }

            // set the accel rate to Freq_200Hz
            ((SensorLIS2DW12)clone.GetSensor(SensorLIS2DW12.SensorName)).SetSamplingRate(SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_200Hz);
            opconfigbytes = clone.GenerateConfigurationBytes();
            if ((opconfigbytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] >> 4 & 0b1111) != 6)
            {
                Assert.Fail();
            }

            // set the accel rate to Freq_400Hz
            ((SensorLIS2DW12)clone.GetSensor(SensorLIS2DW12.SensorName)).SetSamplingRate(SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_400Hz);
            opconfigbytes = clone.GenerateConfigurationBytes();
            if ((opconfigbytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] >> 4 & 0b1111) != 7)
            {
                Assert.Fail();
            }

            // set the accel rate to Freq_800Hz
            ((SensorLIS2DW12)clone.GetSensor(SensorLIS2DW12.SensorName)).SetSamplingRate(SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_800Hz);
            opconfigbytes = clone.GenerateConfigurationBytes();
            if ((opconfigbytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] >> 4 & 0b1111) != 8)
            {
                Assert.Fail();
            }

            // set the accel rate to Freq_1600Hz
            ((SensorLIS2DW12)clone.GetSensor(SensorLIS2DW12.SensorName)).SetSamplingRate(SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_1600Hz);
            opconfigbytes = clone.GenerateConfigurationBytes();
            if ((opconfigbytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] >> 4 & 0b1111) != 9)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestAccel1OpConfigSetAccelRate()
        {
            var opconfig = CopyDefaultBytes();

            SensorLIS2DW12 sensor = CreateDeviceAndReturnSensor(opconfig);


            sensor.SetSamplingRate(SensorLIS2DW12.LowPerformanceAccelSamplingRate.Power_Down);
			if (sensor.GetSamplingRate() != SensorLIS2DW12.LowPerformanceAccelSamplingRate.Power_Down)
            {
                Assert.Fail();
            }


            sensor.SetSamplingRate(SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_1_6Hz);
            if (sensor.GetSamplingRate() != SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_1_6Hz)
            {
                Assert.Fail();
            }


            sensor.SetSamplingRate(SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_12_5Hz);
            if (sensor.GetSamplingRate() != SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_12_5Hz)
            {
                Assert.Fail();
            }


            sensor.SetSamplingRate(SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_25Hz);
            if (sensor.GetSamplingRate() != SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_25Hz)
            {
                Assert.Fail();
            }


            sensor.SetSamplingRate(SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_50Hz);
            if (sensor.GetSamplingRate() != SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_50Hz)
            {
                Assert.Fail();
            }


            sensor.SetSamplingRate(SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_100Hz);
            if (sensor.GetSamplingRate() != SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_100Hz)
            {
                Assert.Fail();
            }


            sensor.SetSamplingRate(SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_200Hz);
            if (sensor.GetSamplingRate() != SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_200Hz)
            {
                Assert.Fail();
            }


            sensor.SetSamplingRate(SensorLIS2DW12.HighPerformanceAccelSamplingRate.Power_Down);
			if (sensor.GetSamplingRate() != SensorLIS2DW12.HighPerformanceAccelSamplingRate.Power_Down)
            {
                Assert.Fail();
            }


            sensor.SetSamplingRate(SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_12_5Hz);
            if (sensor.GetSamplingRate() != SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_12_5Hz)
            {
                Assert.Fail();
            }


            sensor.SetSamplingRate(SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_25Hz);
            if (sensor.GetSamplingRate() != SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_25Hz)
            {
                Assert.Fail();
            }


            sensor.SetSamplingRate(SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_50Hz);
            if (sensor.GetSamplingRate() != SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_50Hz)
            {
                Assert.Fail();
            }


            sensor.SetSamplingRate(SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_100Hz);
            if (sensor.GetSamplingRate() != SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_100Hz)
            {
                Assert.Fail();
            }


            sensor.SetSamplingRate(SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_200Hz);
            if (sensor.GetSamplingRate() != SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_200Hz)
            {
                Assert.Fail();
            }

            sensor.SetSamplingRate(SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_400Hz);
            if (sensor.GetSamplingRate() != SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_400Hz)
            {
                Assert.Fail();
            }

            sensor.SetSamplingRate(SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_800Hz);
            if (sensor.GetSamplingRate() != SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_800Hz)
            {
                Assert.Fail();
            }
            sensor.SetSamplingRate(SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_1600Hz);
            if (sensor.GetSamplingRate() != SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_1600Hz)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestAccel1OpConfigGetLowPowerMode()
        {
            var opconfig = CopyDefaultBytes();

            //set the bit to 00
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] = (byte)(opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b11111100);
            SensorLIS2DW12 sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetLowPowerMode() != SensorLIS2DW12.LowPowerMode.Low_Power_Mode_1)
            {
                Assert.Fail();
            }

            //set the bit to 01
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b11111100) | 0b00000001);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetLowPowerMode() != SensorLIS2DW12.LowPowerMode.Low_Power_Mode_2)
            {
                Assert.Fail();
            }

            //set the bit to 10
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b11111100) | 0b00000010);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetLowPowerMode() != SensorLIS2DW12.LowPowerMode.Low_Power_Mode_3)
            {
                Assert.Fail();
            }

            //set the bit to 11
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b11111100) | 0b00000011);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetLowPowerMode() != SensorLIS2DW12.LowPowerMode.Low_Power_Mode_4)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestHighPassFilterEnabled()
        {
            var opconfig = CopyDefaultBytes();
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] = (byte)(opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] | 0b00001000);
            SensorLIS2DW12 sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.IsHighPassFilterEnabled() != true)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestLowNoiseEnabled()
        {
            var opconfig = CopyDefaultBytes();
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] = (byte)(opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] | 0b00000100);
            SensorLIS2DW12 sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.IsLowNoiseEnabled() != true)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestHighPassFilterRefModeEnabled()
        {
            var opconfig = CopyDefaultBytes();
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_2] = (byte)(opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_2] | 0b00000010);
            SensorLIS2DW12 sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.IsHighPassFilterRefModeEnabled() != true)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestBWFilter()
        {
            var opconfig = CopyDefaultBytes();
            //set bw filter to 2Hz
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] = (byte)(opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] & 0b00111111);
            SensorLIS2DW12 sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetAccelBandwidthFilter() != SensorLIS2DW12.LowPerformanceBandwidthFilter.Bandwidth_Filter_1)
            {
                Assert.Fail();
            }

            //set bw filter to 4Hz
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] & 0b00111111) | 0b01000000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetAccelBandwidthFilter() != SensorLIS2DW12.LowPerformanceBandwidthFilter.Bandwidth_Filter_2)
            {
                Assert.Fail();
            }

            //set bw filter to 10Hz
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] & 0b00111111) | 0b10000000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetAccelBandwidthFilter() != SensorLIS2DW12.LowPerformanceBandwidthFilter.Bandwidth_Filter_3)
            {
                Assert.Fail();
            }

            //set bw filter to 20Hz
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] & 0b00111111) | 0b11000000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetAccelBandwidthFilter() != SensorLIS2DW12.LowPerformanceBandwidthFilter.Bandwidth_Filter_4)
            {
                Assert.Fail();
            }

            //set mode to high performance mode
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b11110011) | 0b00000100);

            //set bw filter to ODR
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] = (byte)(opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] & 0b00111111);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetAccelBandwidthFilter() != SensorLIS2DW12.HighPerformanceBandwidthFilter.Bandwidth_Filter_1)
            {
                Assert.Fail();
            }

            //set bw filter to ODR
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] & 0b00111111) | 0b01000000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetAccelBandwidthFilter() != SensorLIS2DW12.HighPerformanceBandwidthFilter.Bandwidth_Filter_2)
            {
                Assert.Fail();
            }

            //set bw filter to ODR
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] & 0b00111111) | 0b10000000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetAccelBandwidthFilter() != SensorLIS2DW12.HighPerformanceBandwidthFilter.Bandwidth_Filter_3)
            {
                Assert.Fail();
            }

            //set bw filter to ODR
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] & 0b00111111) | 0b11000000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetAccelBandwidthFilter() != SensorLIS2DW12.HighPerformanceBandwidthFilter.Bandwidth_Filter_4)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestFMode()
        {
            var opconfig = CopyDefaultBytes();
            //set fmode to fmode 1
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_3] = (byte)(opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_3] & 0b00011111);
            SensorLIS2DW12 sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetAccelFMode() != SensorLIS2DW12.FMode.FMode_1)
            {
                Assert.Fail();
            }
            //set fmode to fmode 2
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_3] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_3] & 0b00011111) | 0b00100000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetAccelFMode() != SensorLIS2DW12.FMode.FMode_2)
            {
                Assert.Fail();
            }
            //set fmode to fmode 3
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_3] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_3] & 0b00011111) | 0b01100000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetAccelFMode() != SensorLIS2DW12.FMode.FMode_3)
            {
                Assert.Fail();
            }
            //set fmode to fmode 4
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_3] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_3] & 0b00011111) | 0b10000000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetAccelFMode() != SensorLIS2DW12.FMode.FMode_4)
            {
                Assert.Fail();
            }
            //set fmode to fmode 5
            opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_3] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ACCEL1_CFG_3] & 0b00011111) | 0b11000000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetAccelFMode() != SensorLIS2DW12.FMode.FMode_5)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestGenerateOpConfig()
        {
            var bytes = CopyDefaultBytes();
            SensorLIS2DW12 sensor = CreateDeviceAndReturnSensor(bytes);

            sensor.SetAccelEnabled(true);
            sensor.SetHighPassFilterEnabled(true);
            sensor.SetLowNoiseEnabled(true);
            sensor.SetHighPassFilterRefModeEnabled(true);
            sensor.SetAccelRange(SensorLIS2DW12.AccelRange.Range_16G);
            sensor.SetMode(SensorLIS2DW12.Mode.Low_Power_Mode);
            sensor.SetSamplingRate(SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_200Hz);
            sensor.SetBandwidthFilter(SensorLIS2DW12.LowPerformanceBandwidthFilter.Bandwidth_Filter_4);
            sensor.SetAccelFMode(SensorLIS2DW12.FMode.FMode_1);
            sensor.SetLPMode(SensorLIS2DW12.LowPowerMode.Low_Power_Mode_1);
            sensor.GenerateOperationConfig(bytes);

            if (!(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] & 0b10000000).Equals(0b10000000) || //Check enabled sensors
                !(bytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] & 0b00001000).Equals(0b00001000) || //Check HighPassFilterEnabled
                !(bytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] & 0b00000100).Equals(0b00000100) || //Check LowNoiseEnabled
                !(bytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_2] & 0b00000010).Equals(0b00000010) || //Check HighPassFilterRefModeEnabled
                !(bytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] & 0b00110000).Equals(0b00110000) || //Check accel range
                !(bytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b00001100).Equals(0b00000000) || //Check mode
                !(bytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b11110000).Equals(0b01100000) || //Check rate
                !(bytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] & 0b11000000).Equals(0b11000000) || //Check bandwidth filter
                !(bytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_3] & 0b11100000).Equals(0b00000000) || //Check fmode
                !(bytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b0000011).Equals(0b00000000) //Check lpmode
                )
            {
                Assert.Fail();
            }

            sensor.SetAccelEnabled(false);
            sensor.SetHighPassFilterEnabled(false);
            sensor.SetLowNoiseEnabled(false);
            sensor.SetHighPassFilterRefModeEnabled(false);
            sensor.SetAccelRange(SensorLIS2DW12.AccelRange.Range_8G);
            sensor.SetMode(SensorLIS2DW12.Mode.High_Performance_Mode);
            sensor.SetSamplingRate(SensorLIS2DW12.HighPerformanceAccelSamplingRate.Freq_1600Hz);
            sensor.SetBandwidthFilter(SensorLIS2DW12.HighPerformanceBandwidthFilter.Bandwidth_Filter_3);
            sensor.SetAccelFMode(SensorLIS2DW12.FMode.FMode_2);
            sensor.SetLPMode(SensorLIS2DW12.LowPowerMode.Low_Power_Mode_4);
            sensor.GenerateOperationConfig(bytes);

            if (!(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] & 0b10000000).Equals(0b00000000) || //Check enabled sensors
                !(bytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] & 0b00001000).Equals(0b00000000) || //Check HighPassFilterEnabled
                !(bytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] & 0b00000100).Equals(0b00000000) || //Check LowNoiseEnabled
                !(bytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_2] & 0b00000010).Equals(0b00000000) || //Check HighPassFilterRefModeEnabled
                !(bytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] & 0b00110000).Equals(0b00100000) || //Check accel range
                !(bytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b00001100).Equals(0b00000100) || //Check mode
                !(bytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b11110000).Equals(0b10010000) || //Check rate
                !(bytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] & 0b11000000).Equals(0b10000000) || //Check bandwidth filter
                !(bytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_3] & 0b11100000).Equals(0b00100000) || //Check fmode
                !(bytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b0000011).Equals(0b00000011) //Check lpmode
                )
            {
                Assert.Fail();
            }
            Assert.Pass();
        }

        private byte[] CopyDefaultBytes()
        {
            var bytes = new byte[defaultBytes.Length];
            Array.Copy(defaultBytes, bytes, defaultBytes.Length);
            return bytes;
        }

        private SensorLIS2DW12 CreateDeviceAndReturnSensor(byte[] bytes)
        {
            VerisenseBLEDevice clone = new TestVerisenseBLEDevice(uuid, deviceName, bytes);
            VerisenseBLEDevice bleDevice = new VerisenseBLEDevice(clone);
            return (SensorLIS2DW12)bleDevice.GetSensor(SensorLIS2DW12.SensorName);
        }
    }
}
            