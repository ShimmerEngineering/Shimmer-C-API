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
    class SensorGSROperationalConfigTest
    {
        readonly byte[] defaultBytes = new byte[] { 0x5A, 0x97, 0x74, 0x00, 0x00, 0x30, 0x20, 0x00, 0x7F, 0x00, 0xD8, 0x0F, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0xF4, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0xFF, 0xFF, 0xAA, 0x01, 0x03, 0x3C, 0x00, 0x0E, 0x00, 0x00, 0x63, 0x28, 0xCC, 0xCC, 0x1E, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x00, 0x01 };
        readonly string deviceName = "device";
        string uuid = "00000000-0000-0000-0000-daa619f04ad7";

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestGSR1OpConfigEnabled()
        {
            var opconfig = CopyDefaultBytes();

            //set last bit to 1
            opconfig[(int)ConfigurationBytesIndexName.GEN_CFG_1] = (byte)((opconfig[(int)ConfigurationBytesIndexName.GEN_CFG_1] & 0b01111111) | 0b10000000);
            SensorGSR sensor = CreateDeviceAndReturnSensor(opconfig);
            //check if GSR is enabled
            if (sensor.IsGSREnabled() != true)
            {
                Assert.Fail();
            }

            //set last bit to 0
            opconfig[(int)ConfigurationBytesIndexName.GEN_CFG_1] = (byte)(opconfig[(int)ConfigurationBytesIndexName.GEN_CFG_1] & 0b01111111);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            //check if GSR is disabled
            if (sensor.IsGSREnabled() != false)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestGSR1OpConfigBattEnabled()
        {
            var opconfig = CopyDefaultBytes();

            //set second bit to 1
            opconfig[(int)ConfigurationBytesIndexName.GEN_CFG_2] = (byte)((opconfig[(int)ConfigurationBytesIndexName.GEN_CFG_2] & 0b11111101) | 0b00000010);
            SensorGSR sensor = CreateDeviceAndReturnSensor(opconfig);
            //check if battery is enabled
            if (sensor.IsBattEnabled() != true)
            {
                Assert.Fail();
            }

            //set second bit to 0
            opconfig[(int)ConfigurationBytesIndexName.GEN_CFG_2] = (byte)(opconfig[(int)ConfigurationBytesIndexName.GEN_CFG_2] & 0b11111101);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            //check if battery is disabled
            if (sensor.IsBattEnabled() != false)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestGSR1OpConfigGetGSRRange()
        {
            var opconfig = CopyDefaultBytes();

            //set the byte to 0x00
            opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] = (byte)(opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] & 0b11111000);
            SensorGSR sensor = CreateDeviceAndReturnSensor(opconfig);
            //check if the range is correct
            if (sensor.GetGSRRange() != SensorGSR.GSRRange.Range_0)
            {
                Assert.Fail();
            }

            //set the byte to 0x01
            opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] & 0b11111000) | 0x01);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            //check if the range is correct
            if (sensor.GetGSRRange() != SensorGSR.GSRRange.Range_1)
            {
                Assert.Fail();
            }

            //set the byte to 0x02
            opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] & 0b11111000) | 0x02);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            //check if the range is correct
            if (sensor.GetGSRRange() != SensorGSR.GSRRange.Range_2)
            {
                Assert.Fail();
            }

            //set the byte to 0x03
            opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] & 0b11111000) | 0x03);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            //check if the range is correct
            if (sensor.GetGSRRange() != SensorGSR.GSRRange.Range_3)
            {
                Assert.Fail();
            }

            //set the byte to 0x04
            opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] & 0b11111000) | 0x04);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            //check if the range is correct
            if (sensor.GetGSRRange() != SensorGSR.GSRRange.Range_Auto)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestGSR1OpConfigGetGSROversamplingRate()
        {
            var opconfig = CopyDefaultBytes();

            //set oversampling rate to ADC_Oversampling_Disabled
            opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] = (byte)(opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] & 0b00001111);
            SensorGSR sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetOversamplingRate() != SensorGSR.ADCOversamplingRate.ADC_Oversampling_Disabled)
            {
                Assert.Fail();
            }

            //set oversampling rate to ADC_Oversampling_2x
            opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] & 0b00001111) | 0b00010000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetOversamplingRate() != SensorGSR.ADCOversamplingRate.ADC_Oversampling_2x)
            {
                Assert.Fail();
            }

            //set oversampling rate to ADC_Oversampling_4x
            opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] & 0b00001111) | 0b00100000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetOversamplingRate() != SensorGSR.ADCOversamplingRate.ADC_Oversampling_4x)
            {
                Assert.Fail();
            }

            //set oversampling rate to ADC_Oversampling_8x
            opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] & 0b00001111) | 0b00110000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetOversamplingRate() != SensorGSR.ADCOversamplingRate.ADC_Oversampling_8x)
            {
                Assert.Fail();
            }

            //set oversampling rate to ADC_Oversampling_16x
            opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] & 0b00001111) | 0b01000000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetOversamplingRate() != SensorGSR.ADCOversamplingRate.ADC_Oversampling_16x)
            {
                Assert.Fail();
            }

            //set oversampling rate to ADC_Oversampling_32x
            opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] & 0b00001111) | 0b01010000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetOversamplingRate() != SensorGSR.ADCOversamplingRate.ADC_Oversampling_32x)
            {
                Assert.Fail();
            }

            //set oversampling rate to ADC_Oversampling_64x
            opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] & 0b00001111) | 0b01100000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetOversamplingRate() != SensorGSR.ADCOversamplingRate.ADC_Oversampling_64x)
            {
                Assert.Fail();
            }

            //set oversampling rate to ADC_Oversampling_128x
            opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] & 0b00001111) | 0b01110000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetOversamplingRate() != SensorGSR.ADCOversamplingRate.ADC_Oversampling_128x)
            {
                Assert.Fail();
            }

            //set oversampling rate to ADC_Oversampling_256x
            opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] & 0b00001111) | 0b10000000);
            sensor = CreateDeviceAndReturnSensor(opconfig);
            if (sensor.GetOversamplingRate() != SensorGSR.ADCOversamplingRate.ADC_Oversampling_256x)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestGSR1OpConfigGetGSRRate()
        {
            var opconfig = CopyDefaultBytes();

            //set the byte to 0x21
            opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_0] = 0x21;
            SensorGSR sensor = CreateDeviceAndReturnSensor(opconfig);
            //check if the range is correct
            if (sensor.GetSamplingRate() != SensorGSR.GSRRate.Freq_5_12Hz)
            {
                Assert.Fail();
            }

            //set the byte to 0x1E
            opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_0] = 0x1E;
            sensor = CreateDeviceAndReturnSensor(opconfig);
            //check if the range is correct
            if (sensor.GetSamplingRate() != SensorGSR.GSRRate.Freq_10_24Hz)
            {
                Assert.Fail();
            }

            //set the byte to 0x1B
            opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_0] = 0x1B;
            sensor = CreateDeviceAndReturnSensor(opconfig);
            //check if the range is correct

            if (sensor.GetSamplingRate() != SensorGSR.GSRRate.Freq_20_48Hz)
            {
                Assert.Fail();
            }

            //set the byte to 0x17
            opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_0] = 0x17;
            sensor = CreateDeviceAndReturnSensor(opconfig);
            //check if the range is correct
            if (sensor.GetSamplingRate() != SensorGSR.GSRRate.Freq_51_2Hz)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestGenerateOpConfig()
        {
            var bytes = CopyDefaultBytes();
            SensorGSR sensor = CreateDeviceAndReturnSensor(bytes);

            sensor.SetGSREnabled(true);
            sensor.SetBattEnabled(false);
            sensor.SetGSRRange(SensorGSR.GSRRange.Range_2);
            sensor.SetSamplingRate(SensorGSR.GSRRate.Freq_20_48Hz);
            sensor.SetOversamplingRate(SensorGSR.ADCOversamplingRate.ADC_Oversampling_32x);
            sensor.GenerateOperationConfig(bytes);
            if (!(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] & 0b10000000).Equals(0b10000000) || //Check enabled sensors
                !(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_2] & 0b00000010).Equals(0b00000000) || //Check disabled battery
                !(bytes[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_0] & 0b00111111).Equals(0b00011011) || //Check GSR rate
                !(bytes[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] & 0b11110111).Equals(0b01010010) //Check GSR range and oversampling rate
                )
            {
                Assert.Fail();
            }

            sensor.SetGSREnabled(false);
            sensor.SetBattEnabled(true);
            sensor.SetGSRRange(SensorGSR.GSRRange.Range_3);
            sensor.SetSamplingRate(SensorGSR.GSRRate.Freq_51_2Hz);
            sensor.SetOversamplingRate(SensorGSR.ADCOversamplingRate.ADC_Oversampling_128x);
            sensor.GenerateOperationConfig(bytes);

            if (!(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] & 0b10000000).Equals(0b00000000) || //Check enabled sensors
                !(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_2] & 0b00000010).Equals(0b00000010) || //Check enabled battery
                !(bytes[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_0] & 0b00111111).Equals(0b00010111) || //Check GSR rate
                !(bytes[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] & 0b11110111).Equals(0b01110011) //Check GSR range and oversampling rate
                )
            {
                Assert.Fail();
            }
            Assert.Pass();
        }

        [Test]
        public void TestInitializeUsingOperationConfig()
        {
            var opconfig = CopyDefaultBytes();
            //Enable GSR
            opconfig[(int)ConfigurationBytesIndexName.GEN_CFG_1] = (byte)((opconfig[(int)ConfigurationBytesIndexName.GEN_CFG_1] & 0b01111111) | 0b10000000);
            //Enable Batt
            opconfig[(int)ConfigurationBytesIndexName.GEN_CFG_2] = (byte)((opconfig[(int)ConfigurationBytesIndexName.GEN_CFG_2] & 0b11111101) | 0b00000010);
            //Set range to range_1
            opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] & 0b11111000) | 0x01);
            //Set oversampling rate to ADC_Oversampling_64x
            opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] = (byte)((opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_1] & 0b00001111) | 0b01100000);
            //Set rate to Freq_10_24Hz
            opconfig[(int)ConfigurationBytesIndexName.ADC_CHANNEL_SETTINGS_0] = 0x1E;

            var sensor = CreateDeviceAndReturnSensor(opconfig);
            sensor.InitializeUsingOperationConfig(opconfig);
            if (sensor.IsGSREnabled() && 
                sensor.IsBattEnabled() &&
                sensor.GetSamplingRate().Equals(SensorGSR.GSRRate.Freq_10_24Hz) &&
                sensor.GetOversamplingRate().Equals(SensorGSR.ADCOversamplingRate.ADC_Oversampling_64x) &&
                sensor.GetGSRRange().Equals(SensorGSR.GSRRange.Range_1))
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

        private SensorGSR CreateDeviceAndReturnSensor(byte[] bytes)
        {
            VerisenseBLEDevice clone = new TestVerisenseBLEDevice(uuid, deviceName, bytes);
            VerisenseBLEDevice bleDevice = new VerisenseBLEDevice(clone);
            return (SensorGSR)bleDevice.GetSensor(SensorGSR.SensorName);
        }
    }
}
