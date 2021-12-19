using NUnit.Framework;
using shimmer.Sensors;
using ShimmerAPI;
using ShimmerBLEAPI.Devices;
using ShimmerBLETests.Communications;
using System;
using System.Collections.Generic;
using System.Text;
using static shimmer.Models.OpConfigPayload;

namespace ShimmerBLETests
{
    class SensorLSM6DS3Test
    {
        readonly string uuid = "00000000-0000-0000-0000-daa619f04ad7";
        readonly string deviceName = "device";
        readonly byte[] defaultBytes = new byte[] { 0x5A, 0x17, 0x74, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0xD8, 0x0F, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0xF4, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0xFF, 0xFF, 0xAA, 0x01, 0x03, 0x3C, 0x00, 0x0E, 0x00, 0x00, 0x63, 0x28, 0xCC, 0xCC, 0x1E, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x00, 0x01 };

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestAccel2Enabled()
        {
            var bytes = CopyDefaultBytes();
            bytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] = 0x57;
            SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(bytes);
            if (sensor.IsAccelEnabled() == true)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestAccel2Disabled()
        {
            SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(defaultBytes);
            if (sensor.IsAccelEnabled() == false)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestGyroEnabled()
        {
            var bytes = CopyDefaultBytes();
            bytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] = 0x37;
            SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(bytes);
            if (sensor.IsGyroEnabled() == true)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestGyroDisabled()
        {
            SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(defaultBytes);
            if (sensor.IsGyroEnabled() == false)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        //[Test]
        //public void TestHighPerformanceOpModeDisabled()
        //{
        //    //default byte 16 is 10000000
        //    SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(defaultBytes);
        //    if (sensor.IsHighPerformanceOpModeEnabled() == false)
        //    {
        //        Assert.Pass();
        //    }
        //    else
        //    {
        //        Assert.Fail();
        //    }
        //}

        //[Test]
        //public void TestHighPerformanceOpModeEnabled()
        //{
        //    var bytes = CopyDefaultBytes();
        //    bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] = (byte)(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] & 0b01111111);
        //    SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(bytes);
        //    if (sensor.IsHighPerformanceOpModeEnabled() == true)
        //    {
        //        Assert.Pass();
        //    }
        //    else
        //    {
        //        Assert.Fail();
        //    }
        //}

        [Test]
        public void TestHighPassFilterEnabled()
        {
            SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(defaultBytes);
            if (sensor.IsHighPassFilterEnabled() == true)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestHighPassFilterDisabled()
        {
            var bytes = CopyDefaultBytes();
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] = (byte)(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] & 0b01111111);
            SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(bytes);
            if (sensor.IsHighPassFilterEnabled() == false)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestDigitalHPFilterResetEnabled()
        {
            var bytes = CopyDefaultBytes();
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] = (byte)(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] | 0b00010000);
            SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(bytes);
            if (sensor.IsDigitalHPFilterResetEnabled() == true)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestDigitalHPFilterResetDisabled()
        {
            SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(defaultBytes);
            if (sensor.IsDigitalHPFilterResetEnabled() == false)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestSourceRegRoundingStatusEnabled()
        {
            var bytes = CopyDefaultBytes();
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] = (byte)(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] | 0b00000100);
            SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(bytes);
            if (sensor.IsSourceRegRoundingStatusEnabled() == true)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestSourceRegRoundingStatusDisabled()
        {
            SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(defaultBytes);
            if (sensor.IsSourceRegRoundingStatusEnabled() == false)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestStepCounterAndTimestampEnabled()
        {
            var bytes = CopyDefaultBytes();
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] = (byte)(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] | 0b10000000);
            SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(bytes);
            if (sensor.IsStepCounterAndTSDataEnabled() == true)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestStepCounterAndTimestampDisabled()
        {
            SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(defaultBytes);
            if (sensor.IsStepCounterAndTSDataEnabled() == false)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestWriteInFIFOAtEveryStepDetectedEnabled()
        {
            var bytes = CopyDefaultBytes();
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] = (byte)(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] | 0b01000000);
            SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(bytes);
            if (sensor.IsWriteInFIFOAtEveryStepDetectedEnabled() == true)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestWriteInFIFOAtEveryStepDetectedDisabled()
        {
            SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(defaultBytes);
            if (sensor.IsWriteInFIFOAtEveryStepDetectedEnabled() == false)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestHPFilterCutOffFreq()
        {
            var bytes = CopyDefaultBytes();
            //set the HPFilterCutOffFreq to CutOff_Freq_0_0081
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] = (byte)(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] & 0b10011111);
            SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(bytes);
            Sensor.SensorSetting setting = sensor.GetHPFilterCutOffFreqSetting();
            if (!setting.Equals(SensorLSM6DS3.HPFilterCutOffFrequency.CutOff_Freq_0_0081))
            {
                Assert.Fail();
            }

            //set the HPFilterCutOffFreq to CutOff_Freq_0_0324
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] & 0b10011111) | 0b00100000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetHPFilterCutOffFreqSetting();
            if (!setting.Equals(SensorLSM6DS3.HPFilterCutOffFrequency.CutOff_Freq_0_0324))
            {
                Assert.Fail();
            }

            //set the HPFilterCutOffFreq to CutOff_Freq_2_07
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] & 0b10011111) | 0b01000000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetHPFilterCutOffFreqSetting();
            if (!setting.Equals(SensorLSM6DS3.HPFilterCutOffFrequency.CutOff_Freq_2_07))
            {
                Assert.Fail();
            }

            //set the HPFilterCutOffFreq to CutOff_Freq_16_32
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] & 0b10011111) | 0b01100000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetHPFilterCutOffFreqSetting();
            if (!setting.Equals(SensorLSM6DS3.HPFilterCutOffFrequency.CutOff_Freq_16_32))
            {
                Assert.Fail();
            }
            Assert.Pass();
        }

        [Test]
        public void TestGyroFIFODecimation()
        {
            var bytes = CopyDefaultBytes();
            //set the GyroFIFODecimation to Decimation_No_Sensor
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] = (byte)(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] & 0b11000111);
            SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(bytes);
            Sensor.SensorSetting setting = sensor.GetGyroFIFODecimationSetting();
            if (!setting.Equals(SensorLSM6DS3.GyroFIFODecimation.Decimation_No_Sensor))
            {
                Assert.Fail();
            }

            //set the GyroFIFODecimation to Decimation_0
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] & 0b11000111) | 0b00001000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetGyroFIFODecimationSetting();
            if (!setting.Equals(SensorLSM6DS3.GyroFIFODecimation.Decimation_0))
            {
                Assert.Fail();
            }

            //set the GyroFIFODecimation to Decimation_2
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] & 0b11000111) | 0b00010000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetGyroFIFODecimationSetting();
            if (!setting.Equals(SensorLSM6DS3.GyroFIFODecimation.Decimation_2))
            {
                Assert.Fail();
            }

            //set the GyroFIFODecimation to Decimation_3
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] & 0b11000111) | 0b00011000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetGyroFIFODecimationSetting();
            if (!setting.Equals(SensorLSM6DS3.GyroFIFODecimation.Decimation_3))
            {
                Assert.Fail();
            }

            //set the GyroFIFODecimation to Decimation_4
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] & 0b11000111) | 0b00100000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetGyroFIFODecimationSetting();
            if (!setting.Equals(SensorLSM6DS3.GyroFIFODecimation.Decimation_4))
            {
                Assert.Fail();
            }

            //set the GyroFIFODecimation to Decimation_8
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] & 0b11000111) | 0b00101000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetGyroFIFODecimationSetting();
            if (!setting.Equals(SensorLSM6DS3.GyroFIFODecimation.Decimation_8))
            {
                Assert.Fail();
            }

            //set the GyroFIFODecimation to Decimation_16
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] & 0b11000111) | 0b00110000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetGyroFIFODecimationSetting();
            if (!setting.Equals(SensorLSM6DS3.GyroFIFODecimation.Decimation_16))
            {
                Assert.Fail();
            }

            //set the GyroFIFODecimation to Decimation_32
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] & 0b11000111) | 0b00111000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetGyroFIFODecimationSetting();
            if (!setting.Equals(SensorLSM6DS3.GyroFIFODecimation.Decimation_32))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestAccelFIFODecimation()
        {
            var bytes = CopyDefaultBytes();
            //set the AccelFIFODecimation to Decimation_No_Sensor
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] = (byte)(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] & 0b11111000);
            SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(bytes);
            Sensor.SensorSetting setting = sensor.GetAccelFIFODecimationSetting();
            if (!setting.Equals(SensorLSM6DS3.AccelFIFODecimation.Decimation_No_Sensor))
            {
                Assert.Fail();
            }

            //set the AccelFIFODecimation to Decimation_0
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] & 0b11111000) | 0b00000001);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetAccelFIFODecimationSetting();
            if (!setting.Equals(SensorLSM6DS3.AccelFIFODecimation.Decimation_0))
            {
                Assert.Fail();
            }

            //set the AccelFIFODecimation to Decimation_2
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] & 0b11111000) | 0b00000010);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetAccelFIFODecimationSetting();
            if (!setting.Equals(SensorLSM6DS3.AccelFIFODecimation.Decimation_2))
            {
                Assert.Fail();
            }

            //set the AccelFIFODecimation to Decimation_3
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] & 0b11111000) | 0b00000011);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetAccelFIFODecimationSetting();
            if (!setting.Equals(SensorLSM6DS3.AccelFIFODecimation.Decimation_3))
            {
                Assert.Fail();
            }

            //set the AccelFIFODecimation to Decimation_4
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] & 0b11111000) | 0b00000100);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetAccelFIFODecimationSetting();
            if (!setting.Equals(SensorLSM6DS3.AccelFIFODecimation.Decimation_4))
            {
                Assert.Fail();
            }

            //set the AccelFIFODecimation to Decimation_8
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] & 0b11111000) | 0b00000101);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetAccelFIFODecimationSetting();
            if (!setting.Equals(SensorLSM6DS3.AccelFIFODecimation.Decimation_8))
            {
                Assert.Fail();
            }

            //set the AccelFIFODecimation to Decimation_16
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] & 0b11111000) | 0b00000110);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetAccelFIFODecimationSetting();
            if (!setting.Equals(SensorLSM6DS3.AccelFIFODecimation.Decimation_16))
            {
                Assert.Fail();
            }

            //set the AccelFIFODecimation to Decimation_32
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] & 0b11111000) | 0b00000111);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetAccelFIFODecimationSetting();
            if (!setting.Equals(SensorLSM6DS3.AccelFIFODecimation.Decimation_32))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestFIFOOutputDataRate()
        {
            var bytes = CopyDefaultBytes();
            //set the FIFOOutputDataRate to ODR_Disabled
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] = (byte)(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] & 0b10000111);
            SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(bytes);
            Sensor.SensorSetting setting = sensor.GetFIFOOutputDataRateSetting();
            if (!setting.Equals(SensorLSM6DS3.FIFOOutputDataRate.ODR_Disabled))
            {
                Assert.Fail();
            }

            //set the FIFOOutputDataRate to ODR_12_5
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] & 0b10000111) | 0b00001000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetFIFOOutputDataRateSetting();
            if (!setting.Equals(SensorLSM6DS3.FIFOOutputDataRate.ODR_12_5))
            {
                Assert.Fail();
            }

            //set the FIFOOutputDataRate to ODR_26
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] & 0b10000111) | 0b00010000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetFIFOOutputDataRateSetting();
            if (!setting.Equals(SensorLSM6DS3.FIFOOutputDataRate.ODR_26))
            {
                Assert.Fail();
            }

            //set the FIFOOutputDataRate to ODR_52
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] & 0b10000111) | 0b00011000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetFIFOOutputDataRateSetting();
            if (!setting.Equals(SensorLSM6DS3.FIFOOutputDataRate.ODR_52))
            {
                Assert.Fail();
            }

            //set the FIFOOutputDataRate to ODR_104
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] & 0b10000111) | 0b00100000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetFIFOOutputDataRateSetting();
            if (!setting.Equals(SensorLSM6DS3.FIFOOutputDataRate.ODR_104))
            {
                Assert.Fail();
            }

            //set the FIFOOutputDataRate to ODR_208
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] & 0b10000111) | 0b00101000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetFIFOOutputDataRateSetting();
            if (!setting.Equals(SensorLSM6DS3.FIFOOutputDataRate.ODR_208))
            {
                Assert.Fail();
            }

            //set the FIFOOutputDataRate to ODR_416
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] & 0b10000111) | 0b00110000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetFIFOOutputDataRateSetting();
            if (!setting.Equals(SensorLSM6DS3.FIFOOutputDataRate.ODR_416))
            {
                Assert.Fail();
            }

            //set the FIFOOutputDataRate to ODR_833
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] & 0b10000111) | 0b00111000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetFIFOOutputDataRateSetting();
            if (!setting.Equals(SensorLSM6DS3.FIFOOutputDataRate.ODR_833))
            {
                Assert.Fail();
            }

            //set the FIFOOutputDataRate to ODR_1666
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] & 0b10000111) | 0b01000000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetFIFOOutputDataRateSetting();
            if (!setting.Equals(SensorLSM6DS3.FIFOOutputDataRate.ODR_1666))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestFIFOMode()
        {
            var bytes = CopyDefaultBytes();
            //set the FIFOMode to FIFOMode_1
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] = (byte)(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] & 0b11111000);
            SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(bytes);
            Sensor.SensorSetting setting = sensor.GetFIFOModeSetting();
            if (!setting.Equals(SensorLSM6DS3.FIFOMode.FIFOMode_1))
            {
                Assert.Fail();
            }

            //set the FIFOMode to FIFOMode_2
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] & 0b11111000) | 0b00000001);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetFIFOModeSetting();
            if (!setting.Equals(SensorLSM6DS3.FIFOMode.FIFOMode_2))
            {
                Assert.Fail();
            }

            //set the FIFOMode to FIFOMode_3
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] & 0b11111000) | 0b00000011);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetFIFOModeSetting();
            if (!setting.Equals(SensorLSM6DS3.FIFOMode.FIFOMode_3))
            {
                Assert.Fail();
            }

            //set the FIFOMode to FIFOMode_4
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] & 0b11111000) | 0b00000100);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetFIFOModeSetting();
            if (!setting.Equals(SensorLSM6DS3.FIFOMode.FIFOMode_4))
            {
                Assert.Fail();
            }

            //set the FIFOMode to FIFOMode_5
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] & 0b11111000) | 0b00000110);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetFIFOModeSetting();
            if (!setting.Equals(SensorLSM6DS3.FIFOMode.FIFOMode_5))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        //[Test]
        //public void TestAccelOutputDataRate()
        //{
        //    var bytes = CopyDefaultBytes();
        //    //set the AccelOutputDataRate to Accel_ODR_Power_Down
        //    bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0b00001111);
        //    SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(bytes);
        //    Sensor.SensorSetting setting = sensor.GetAccelOutputDataRateSetting();
        //    if (!setting.Equals(SensorLSM6DS3.AccelOutputDataRate.Accel_ODR_Power_Down))
        //    {
        //        Assert.Fail();
        //    }

        //    //set the AccelOutputDataRate to Accel_ODR_12_5
        //    bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0b00001111) | 0b00010000);
        //    sensor = CreateDeviceAndReturnSensor(bytes);
        //    setting = sensor.GetAccelOutputDataRateSetting();
        //    if (!setting.Equals(SensorLSM6DS3.AccelOutputDataRate.Accel_ODR_12_5))
        //    {
        //        Assert.Fail();
        //    }

        //    //set the AccelOutputDataRate to Accel_ODR_26
        //    bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0b00001111) | 0b00100000);
        //    sensor = CreateDeviceAndReturnSensor(bytes);
        //    setting = sensor.GetAccelOutputDataRateSetting();
        //    if (!setting.Equals(SensorLSM6DS3.AccelOutputDataRate.Accel_ODR_26))
        //    {
        //        Assert.Fail();
        //    }

        //    //set the AccelOutputDataRate to Accel_ODR_52
        //    bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0b00001111) | 0b00110000);
        //    sensor = CreateDeviceAndReturnSensor(bytes);
        //    setting = sensor.GetAccelOutputDataRateSetting();
        //    if (!setting.Equals(SensorLSM6DS3.AccelOutputDataRate.Accel_ODR_52))
        //    {
        //        Assert.Fail();
        //    }

        //    //set the AccelOutputDataRate to Accel_ODR_104
        //    bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0b00001111) | 0b01000000);
        //    sensor = CreateDeviceAndReturnSensor(bytes);
        //    setting = sensor.GetAccelOutputDataRateSetting();
        //    if (!setting.Equals(SensorLSM6DS3.AccelOutputDataRate.Accel_ODR_104))
        //    {
        //        Assert.Fail();
        //    }

        //    //set the AccelOutputDataRate to Accel_ODR_208
        //    bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0b00001111) | 0b01010000);
        //    sensor = CreateDeviceAndReturnSensor(bytes);
        //    setting = sensor.GetAccelOutputDataRateSetting();
        //    if (!setting.Equals(SensorLSM6DS3.AccelOutputDataRate.Accel_ODR_208))
        //    {
        //        Assert.Fail();
        //    }

        //    //set the AccelOutputDataRate to Accel_ODR_416
        //    bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0b00001111) | 0b01100000);
        //    sensor = CreateDeviceAndReturnSensor(bytes);
        //    setting = sensor.GetAccelOutputDataRateSetting();
        //    if (!setting.Equals(SensorLSM6DS3.AccelOutputDataRate.Accel_ODR_416))
        //    {
        //        Assert.Fail();
        //    }

        //    //set the AccelOutputDataRate to Accel_ODR_833
        //    bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0b00001111) | 0b01110000);
        //    sensor = CreateDeviceAndReturnSensor(bytes);
        //    setting = sensor.GetAccelOutputDataRateSetting();
        //    if (!setting.Equals(SensorLSM6DS3.AccelOutputDataRate.Accel_ODR_833))
        //    {
        //        Assert.Fail();
        //    }

        //    //set the AccelOutputDataRate to Accel_ODR_1666
        //    bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0b00001111) | 0b10000000);
        //    sensor = CreateDeviceAndReturnSensor(bytes);
        //    setting = sensor.GetAccelOutputDataRateSetting();
        //    if (!setting.Equals(SensorLSM6DS3.AccelOutputDataRate.Accel_ODR_1666))
        //    {
        //        Assert.Fail();
        //    }

        //    Assert.Pass();
        //}

        //[Test]
        //public void TestGyroOutputDataRate()
        //{
        //    var bytes = CopyDefaultBytes();
        //    //set the GyroOutputDataRate to Gyro_ODR_Power_Down
        //    bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0b00001111);
        //    SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(bytes);
        //    Sensor.SensorSetting setting = sensor.GetGyroOutputDataRateSetting();
        //    if (!setting.Equals(SensorLSM6DS3.GyroOutputDataRate.Gyro_ODR_Power_Down))
        //    {
        //        Assert.Fail();
        //    }

        //    //set the GyroOutputDataRate to Gyro_ODR_12_5
        //    bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0b00001111) | 0b00010000);
        //    sensor = CreateDeviceAndReturnSensor(bytes);
        //    setting = sensor.GetGyroOutputDataRateSetting();
        //    if (!setting.Equals(SensorLSM6DS3.GyroOutputDataRate.Gyro_ODR_12_5))
        //    {
        //        Assert.Fail();
        //    }

        //    //set the GyroOutputDataRate to Gyro_ODR_26
        //    bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0b00001111) | 0b00100000);
        //    sensor = CreateDeviceAndReturnSensor(bytes);
        //    setting = sensor.GetGyroOutputDataRateSetting();
        //    if (!setting.Equals(SensorLSM6DS3.GyroOutputDataRate.Gyro_ODR_26))
        //    {
        //        Assert.Fail();
        //    }

        //    //set the GyroOutputDataRate to Gyro_ODR_52
        //    bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0b00001111) | 0b00110000);
        //    sensor = CreateDeviceAndReturnSensor(bytes);
        //    setting = sensor.GetGyroOutputDataRateSetting();
        //    if (!setting.Equals(SensorLSM6DS3.GyroOutputDataRate.Gyro_ODR_52))
        //    {
        //        Assert.Fail();
        //    }

        //    //set the GyroOutputDataRate to Gyro_ODR_104
        //    bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0b00001111) | 0b01000000);
        //    sensor = CreateDeviceAndReturnSensor(bytes);
        //    setting = sensor.GetGyroOutputDataRateSetting();
        //    if (!setting.Equals(SensorLSM6DS3.GyroOutputDataRate.Gyro_ODR_104))
        //    {
        //        Assert.Fail();
        //    }

        //    //set the GyroOutputDataRate to Gyro_ODR_208
        //    bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0b00001111) | 0b01010000);
        //    sensor = CreateDeviceAndReturnSensor(bytes);
        //    setting = sensor.GetGyroOutputDataRateSetting();
        //    if (!setting.Equals(SensorLSM6DS3.GyroOutputDataRate.Gyro_ODR_208))
        //    {
        //        Assert.Fail();
        //    }

        //    //set the GyroOutputDataRate to Gyro_ODR_416
        //    bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0b00001111) | 0b01100000);
        //    sensor = CreateDeviceAndReturnSensor(bytes);
        //    setting = sensor.GetGyroOutputDataRateSetting();
        //    if (!setting.Equals(SensorLSM6DS3.GyroOutputDataRate.Gyro_ODR_416))
        //    {
        //        Assert.Fail();
        //    }

        //    //set the GyroOutputDataRate to Gyro_ODR_833
        //    bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0b00001111) | 0b01110000);
        //    sensor = CreateDeviceAndReturnSensor(bytes);
        //    setting = sensor.GetGyroOutputDataRateSetting();
        //    if (!setting.Equals(SensorLSM6DS3.GyroOutputDataRate.Gyro_ODR_833))
        //    {
        //        Assert.Fail();
        //    }

        //    //set the GyroOutputDataRate to Gyro_ODR_1666
        //    bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0b00001111) | 0b10000000);
        //    sensor = CreateDeviceAndReturnSensor(bytes);
        //    setting = sensor.GetGyroOutputDataRateSetting();
        //    if (!setting.Equals(SensorLSM6DS3.GyroOutputDataRate.Gyro_ODR_1666))
        //    {
        //        Assert.Fail();
        //    }

        //    Assert.Pass();
        //}

        [Test]
        public void TestGenerateOpConfig()
        {
            var bytes = CopyDefaultBytes();
            SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(bytes);

            sensor.SetAccelEnabled(true);
            sensor.SetGyroEnabled(true);
            //sensor.SetHighPerformanceOpModeEnabled(true);
            sensor.SetHighPassFilterEnabled(true);
            sensor.SetDigitalHPFilterResetEnabled(true);
            sensor.SetRoundingStatusEnabled(true);
            sensor.SetStepCounterAndTSDataEnabled(true);
            sensor.SetWriteInFIFOAtEveryStepDetectedEnabled(true);
            sensor.SetAccelRange(SensorLSM6DS3.AccelRange.Range_16G);
            sensor.SetSamplingRate(SensorLSM6DS3.SamplingRate.Freq_833Hz);
            sensor.SetGyroRange(SensorLSM6DS3.GyroRange.Range_2000dps);
            //sensor.SetGyroRate(SensorLSM6DS3.GyroSamplingRate.Freq_416Hz);
            sensor.SetFIFOThreshold(SensorLSM6DS3.FIFOThreshold.Threshold_5);
            sensor.SetHPFilterCutOffFreqSetting(SensorLSM6DS3.HPFilterCutOffFrequency.CutOff_Freq_2_07);
            sensor.SetGyroFIFODecimationSetting(SensorLSM6DS3.GyroFIFODecimation.Decimation_16);
            sensor.SetAccelFIFODecimationSetting(SensorLSM6DS3.AccelFIFODecimation.Decimation_8);
            sensor.SetFIFOOutputDataRateSetting(SensorLSM6DS3.FIFOOutputDataRate.ODR_833);
            sensor.SetFIFOModeSetting(SensorLSM6DS3.FIFOMode.FIFOMode_3);
            sensor.GenerateOperationConfig(bytes);

            //Check if Accel2 + Gyro enabled
            if (!(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] & 0b01100000).Equals(0b01100000) || //Check enabled sensors
                !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0b00001100).Equals(0b00000100) || //Check Accel2 range
                !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0b11110000).Equals(0b01110000) || //Check Accel2 rate
                !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0b00001100).Equals(0b00001100) || //Check Gyro range
                //!(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0b11110000).Equals(0b01100000) || //Check Gyro rate
                //!(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] & 0b10000000).Equals(0b00000000) || //Check enabled HighPerformanceOpMode
                !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] & 0b10000000).Equals(0b10000000) || //Check enabled HighPassFilter
                !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] & 0b00010000).Equals(0b00010000) || //Check enabled DigitalHPFilterReset
                !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] & 0b00001100).Equals(0b00000100) || //Check enabled RoundingStatus
                !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] & 0b01100000).Equals(0b01000000) || //Check HPFilterCutOffFrequency
                !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] & 0b10000000).Equals(0b10000000) || //Check enabled StepCounterAndTimestamp
                !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] & 0b01000000).Equals(0b01000000) || //Check enabled WriteInFIFOAtEveryStepDetected
                !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] & 0b00111000).Equals(0b00110000) || //Check GyroFIFODecimation
                !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] & 0b00000111).Equals(0b00000101) || //Check AccelFIFODecimation
                !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] & 0b01111000).Equals(0b00111000) || //Check FIFOOutputDataRate
                !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] & 0b00000111).Equals(0b00000011) || //Check FIFOMode
                !bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_0].Equals(0b00100000) || !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] & 0b00001111).Equals(0b00000001) //Check FIFO Threshold
                )
            {
                Assert.Fail();
            }

            sensor.SetAccelRange(SensorLSM6DS3.AccelRange.Range_4G);
            sensor.SetSamplingRate(SensorLSM6DS3.SamplingRate.Freq_12_5Hz);
            sensor.SetGyroRange(SensorLSM6DS3.GyroRange.Range_500dps);
            //sensor.SetGyroRate(SensorLSM6DS3.GyroSamplingRate.Freq_12_5Hz);
            sensor.SetFIFOThreshold(SensorLSM6DS3.FIFOThreshold.Threshold_2);
            sensor.SetHPFilterCutOffFreqSetting(SensorLSM6DS3.HPFilterCutOffFrequency.CutOff_Freq_16_32);
            sensor.SetGyroFIFODecimationSetting(SensorLSM6DS3.GyroFIFODecimation.Decimation_8);
            sensor.SetAccelFIFODecimationSetting(SensorLSM6DS3.AccelFIFODecimation.Decimation_4);
            sensor.SetFIFOOutputDataRateSetting(SensorLSM6DS3.FIFOOutputDataRate.ODR_416);
            sensor.SetFIFOModeSetting(SensorLSM6DS3.FIFOMode.FIFOMode_2);
            sensor.GenerateOperationConfig(bytes);
            if (!(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] & 0b01100000).Equals(0b01100000) || //Check enabled sensors
                !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0b00001100).Equals(0b00001000) || //Check Accel2 range
                !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0b11110000).Equals(0b00010000) || //Check Accel2 rate
                !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0b00001100).Equals(0b00000100) || //Check Gyro range
                //!(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0b11110000).Equals(0b00010000) || //Check Gyro rate
                !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] & 0b00110000).Equals(0b00110000) || //Check HPFilterCutOffFrequency
                !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] & 0b00111000).Equals(0b00101000) || //Check GyroFIFODecimation
                !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] & 0b00000111).Equals(0b00000100) || //Check AccelFIFODecimation
                !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] & 0b01111000).Equals(0b00110000) || //Check FIFOOutputDataRate
                !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] & 0b00000111).Equals(0b00000001) || //Check FIFOMode
                !bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_0].Equals(0b11101100) || !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] & 0b00001111).Equals(0b00000111) //Check FIFO Threshold
                )
            {
                Assert.Fail();
            }

            sensor.SetAccelEnabled(false);
            sensor.SetGyroEnabled(false);
            //sensor.SetHighPerformanceOpModeEnabled(false);
            sensor.SetHighPassFilterEnabled(false);
            sensor.SetDigitalHPFilterResetEnabled(false);
            sensor.SetRoundingStatusEnabled(false);
            sensor.SetStepCounterAndTSDataEnabled(false);
            sensor.SetWriteInFIFOAtEveryStepDetectedEnabled(false);
            sensor.GenerateOperationConfig(bytes);
            if (!(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] & 0b01100000).Equals(0b00000000) || //Check disabled sensors
                //!(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] & 0b10000000).Equals(0b10000000) || //Check disabled HighPerformanceOpMode
                !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] & 0b10000000).Equals(0b00000000) || //Check disabled HighPassFilter
                !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] & 0b00010000).Equals(0b00000000) || //Check disabled DigitalHPFilterReset
                !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] & 0b10000000).Equals(0b00000000) || //Check disabled StepCounterAndTimestamp
                !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] & 0b01000000).Equals(0b00000000) || //Check disabled WriteInFIFOAtEveryStepDetected
                !(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] & 0b00001100).Equals(0b00000000)) //Check disabled RoundingStatus
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestAccel2Range()
        {
            var bytes = CopyDefaultBytes();
            //Clear the Accel2 range bits (default is 00 -> +-2g)
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0xF3);
            SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(bytes);
            Sensor.SensorSetting setting = sensor.GetAccelRange();
            if(!setting.Equals(SensorLSM6DS3.AccelRange.Range_2G))
            {
                Assert.Fail();
            }

            //Set the range to +-16g
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0xF3) | 0x4);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetAccelRange();
            if (!setting.Equals(SensorLSM6DS3.AccelRange.Range_16G))
            {
                Assert.Fail();
            }

            //Set the range to +-4g
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0xF3) | 0x8);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetAccelRange();
            if (!setting.Equals(SensorLSM6DS3.AccelRange.Range_4G))
            {
                Assert.Fail();
            }

            //Set the range to +-8g
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0xF3) | 0xC);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetAccelRange();
            if (!setting.Equals(SensorLSM6DS3.AccelRange.Range_8G))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestAccel2GyroRate()
        {
            var bytes = CopyDefaultBytes();
            //Clear the Accel2 rate bits (default 0000 -> power down)
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0xF);
            SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(bytes);
            Sensor.SensorSetting setting = sensor.GetSamplingRate();
            if (!setting.Equals(SensorLSM6DS3.SamplingRate.Power_Down))
            {
                Assert.Fail();
            }

            //Set the rate to 12.5 Hz
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0xF) | 0x10);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetSamplingRate();
            if (!setting.Equals(SensorLSM6DS3.SamplingRate.Freq_12_5Hz))
            {
                Assert.Fail();
            }

            //Set the rate to 26 Hz
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0xF) | 0x20);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetSamplingRate();
            if (!setting.Equals(SensorLSM6DS3.SamplingRate.Freq_26Hz))
            {
                Assert.Fail();
            }

            //Set the rate to 52 Hz
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0xF) | 0x30);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetSamplingRate();
            if (!setting.Equals(SensorLSM6DS3.SamplingRate.Freq_52Hz))
            {
                Assert.Fail();
            }

            //Set the rate to 104 Hz
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0xF) | 0x40);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetSamplingRate();
            if (!setting.Equals(SensorLSM6DS3.SamplingRate.Freq_104Hz))
            {
                Assert.Fail();
            }

            //Set the rate to 208 Hz
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0xF) | 0x50);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetSamplingRate();
            if (!setting.Equals(SensorLSM6DS3.SamplingRate.Freq_208Hz))
            {
                Assert.Fail();
            }

            //Set the rate to 416 Hz
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0xF) | 0x60);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetSamplingRate();
            if (!setting.Equals(SensorLSM6DS3.SamplingRate.Freq_416Hz))
            {
                Assert.Fail();
            }

            //Set the rate to 833 Hz
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0xF) | 0x70);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetSamplingRate();
            if (!setting.Equals(SensorLSM6DS3.SamplingRate.Freq_833Hz))
            {
                Assert.Fail();
            }

            //Set the rate to 1.66 kHz
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0xF) | 0x80);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetSamplingRate();
            if (!setting.Equals(SensorLSM6DS3.SamplingRate.Freq_1666Hz))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestGyroRange()
        {
            var bytes = CopyDefaultBytes();
            //Clear the Gyro range bits (default is 00 -> 250dps)
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0xF3);
            SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(bytes);
            Sensor.SensorSetting setting = sensor.GetGyroRange();
            if (!setting.Equals(SensorLSM6DS3.GyroRange.Range_250dps))
            {
                Assert.Fail();
            }

            //Set the range to 500dps
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0xF3) | 0x4);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetGyroRange();
            if (!setting.Equals(SensorLSM6DS3.GyroRange.Range_500dps))
            {
                Assert.Fail();
            }

            //Set the range to 1000dps
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0xF3) | 0x8);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetGyroRange();
            if (!setting.Equals(SensorLSM6DS3.GyroRange.Range_1000dps))
            {
                Assert.Fail();
            }

            //Set the range to 2000dps
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0xF3) | 0xC);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetGyroRange();
            if (!setting.Equals(SensorLSM6DS3.GyroRange.Range_2000dps))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        /* test below is no longer applicable commenting it out
        [Test]
        public void TestGyroRate()
        {
            var bytes = CopyDefaultBytes();
            //Clear the Gyro rate bits (default 0000 -> power down)
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0xF);
            SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(bytes);
            Sensor.SensorSetting setting = sensor.GetGyroRate();
            if (!setting.Equals(SensorLSM6DS3.GyroSamplingRate.Power_Down))
            {
                Assert.Fail();
            }

            //Set the rate to 12.5 Hz
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0xF) | 0x10);
            sensor = CreateDeviceAndReturnSensor(bytes); 
            setting = sensor.GetGyroRate();
            if (!setting.Equals(SensorLSM6DS3.GyroSamplingRate.Freq_12_5Hz))
            {
                Assert.Fail();
            }

            //Set the rate to 26 Hz
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0xF) | 0x20);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetGyroRate();
            if (!setting.Equals(SensorLSM6DS3.GyroSamplingRate.Freq_26Hz))
            {
                Assert.Fail();
            }

            //Set the rate to 52 Hz
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0xF) | 0x30);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetGyroRate();
            if (!setting.Equals(SensorLSM6DS3.GyroSamplingRate.Freq_52Hz))
            {
                Assert.Fail();
            }

            //Set the rate to 104 Hz
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0xF) | 0x40);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetGyroRate();
            if (!setting.Equals(SensorLSM6DS3.GyroSamplingRate.Freq_104Hz))
            {
                Assert.Fail();
            }

            //Set the rate to 208 Hz
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0xF) | 0x50);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetGyroRate();
            if (!setting.Equals(SensorLSM6DS3.GyroSamplingRate.Freq_208Hz))
            {
                Assert.Fail();
            }

            //Set the rate to 416 Hz
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0xF) | 0x60);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetGyroRate();
            if (!setting.Equals(SensorLSM6DS3.GyroSamplingRate.Freq_416Hz))
            {
                Assert.Fail();
            }

            //Set the rate to 833 Hz
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0xF) | 0x70);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetGyroRate();
            if (!setting.Equals(SensorLSM6DS3.GyroSamplingRate.Freq_833Hz))
            {
                Assert.Fail();
            }

            //Set the rate to 1.66 kHz
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0xF) | 0x80);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetGyroRate();
            if (!setting.Equals(SensorLSM6DS3.GyroSamplingRate.Freq_1666Hz))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }
        */

        [Test]
        public void TestFIFOThreshold()
        {
            var bytes = CopyDefaultBytes();

            //Set the FIFO to default (Threshold 1)
            SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(bytes);
            Sensor.SensorSetting setting = sensor.GetFIFOThreshold();
            if(!setting.Equals(SensorLSM6DS3.FIFOThreshold.Threshold_1))
            {
                Assert.Fail();
            }

            //Set the FIFO to Threshold 2
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_0] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_0] & 0x0) | 0b11101100);
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] & 0xF0) | 0b0111);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetFIFOThreshold();
            if (!setting.Equals(SensorLSM6DS3.FIFOThreshold.Threshold_2))
            {
                Assert.Fail();
            }

            //Set the FIFO to Threshold 3
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_0] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_0] & 0x0) | 0b11110110);
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] & 0xF0) | 0b0011);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetFIFOThreshold();
            if (!setting.Equals(SensorLSM6DS3.FIFOThreshold.Threshold_3))
            {
                Assert.Fail();
            }

            //Set the FIFO to Threshold 4
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_0] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_0] & 0x0) | 0b00011100);
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] & 0xF0) | 0b0010);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetFIFOThreshold();
            if (!setting.Equals(SensorLSM6DS3.FIFOThreshold.Threshold_4))
            {
                Assert.Fail();
            }

            //Set the FIFO to Threshold 5
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_0] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_0] & 0x0) | 0b00100000);
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] & 0xF0) | 0b0001);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetFIFOThreshold();
            if (!setting.Equals(SensorLSM6DS3.FIFOThreshold.Threshold_5))
            {
                Assert.Fail();
            }

            //Set the FIFO to Threshold 6
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_0] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_0] & 0x0) | 0b10010110);
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] & 0xF0) | 0b0000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            setting = sensor.GetFIFOThreshold();
            if (!setting.Equals(SensorLSM6DS3.FIFOThreshold.Threshold_6))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestInitializeUsingOperationConfig()
        {
            var bytes = CopyDefaultBytes();
            //Enable Accel2 and Gyro sensor as it is disabled in the default config bytes
            bytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] = (byte)((bytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] | 0b01100000));
            //Set Accel2 range to +-8g
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0xF3) | 0xC);
            //Set Accel2 rate to 104 Hz
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0xF) | 0x40);
            //Set Gyro range to 1000 dps
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0xF3) | 0x8);
            //Set Gyro rate to 208 Hz
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0xF) | 0x50);
            //Set FIFO Threshold to 3
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_0] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_0] & 0x0) | 0b11110110);
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] & 0xF0) | 0b0011);
            //Set the HPFilterCutOffFreq to CutOff_Freq_16_32
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] & 0b10011111) | 0b01100000);
            //Enable HighPassFilter, DigitalHPFilterReset and SourceRegRoundingStatus
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] = (byte)(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] | 0b10010100);
            //Enable HighPerformanceOpMode
            //bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] = (byte)(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] & 0b01111111);
            //Enable StepCounterAndTimestamp and WriteInFIFOAtEveryStepDetected
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] = (byte)(bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] | 0b11000000);
            //Set GyroFIFODecimation to Decimation_32
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] & 0b11000111) | 0b00111000);
            //Set AccelFIFODecimation to Decimation_32
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] & 0b11111000) | 0b00000111);
            //Set FIFOOutputDataRate to ODR_1666
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] & 0b10000111) | 0b01000000);
            //Set FIFOMode to FIFOMode_5
            bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] & 0b11111000) | 0b00000110);
            //Set AccelOutputDataRate to ODR_833
            //bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0b00001111) | 0b01110000);
            //Set GyroOutputDataRate to ODR_416
            //bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)((bytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0b00001111) | 0b01100000);

            var sensor = CreateDeviceAndReturnSensor(bytes);
            sensor.InitializeUsingOperationConfig(bytes);
            if (sensor.IsAccelEnabled() && sensor.IsGyroEnabled() && 
                sensor.GetAccelRange().Equals(SensorLSM6DS3.AccelRange.Range_8G) && 
                sensor.GetSamplingRate().Equals(SensorLSM6DS3.SamplingRate.Freq_104Hz) && 
                sensor.GetGyroRange().Equals(SensorLSM6DS3.GyroRange.Range_1000dps) &&
                sensor.GetFIFOThreshold().Equals(SensorLSM6DS3.FIFOThreshold.Threshold_3) &&
                sensor.GetHPFilterCutOffFreqSetting().Equals(SensorLSM6DS3.HPFilterCutOffFrequency.CutOff_Freq_16_32) &&
                sensor.IsSourceRegRoundingStatusEnabled() &&
                sensor.IsDigitalHPFilterResetEnabled() &&
                sensor.IsHighPassFilterEnabled() &&
                //sensor.IsHighPerformanceOpModeEnabled() &&
                sensor.IsWriteInFIFOAtEveryStepDetectedEnabled() &&
                sensor.IsStepCounterAndTSDataEnabled() &&
                sensor.GetGyroFIFODecimationSetting().Equals(SensorLSM6DS3.GyroFIFODecimation.Decimation_32) &&
                sensor.GetAccelFIFODecimationSetting().Equals(SensorLSM6DS3.AccelFIFODecimation.Decimation_32) &&
                sensor.GetFIFOOutputDataRateSetting().Equals(SensorLSM6DS3.FIFOOutputDataRate.ODR_1666) &&
                //sensor.GetAccelOutputDataRateSetting().Equals(SensorLSM6DS3.AccelOutputDataRate.Accel_ODR_833) &&
                //sensor.GetGyroOutputDataRateSetting().Equals(SensorLSM6DS3.GyroOutputDataRate.Gyro_ODR_416) &&
                sensor.GetFIFOModeSetting().Equals(SensorLSM6DS3.FIFOMode.FIFOMode_5))
                
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestGetSensorName()
        {
            var bytes = new byte[defaultBytes.Length];
            Array.Copy(defaultBytes, bytes, defaultBytes.Length);
            SensorLSM6DS3 sensor = CreateDeviceAndReturnSensor(bytes);
            if(sensor.GetSensorName().Equals(SensorLSM6DS3.SensorName))
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        private byte[] CopyDefaultBytes()
        {
            var bytes = new byte[defaultBytes.Length];
            Array.Copy(defaultBytes, bytes, defaultBytes.Length);
            return bytes;
        }

        private SensorLSM6DS3 CreateDeviceAndReturnSensor(byte[] bytes)
        {
            VerisenseBLEDevice clone = new TestVerisenseBLEDevice(uuid, deviceName, bytes);
            VerisenseBLEDevice bleDevice = new VerisenseBLEDevice(clone);
            return (SensorLSM6DS3)bleDevice.GetSensor(SensorLSM6DS3.SensorName);
        }
    }
}
