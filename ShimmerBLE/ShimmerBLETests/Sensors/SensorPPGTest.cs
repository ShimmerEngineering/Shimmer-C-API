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
    class SensorPPGTest
    {
        readonly string uuid = "00000000-0000-0000-0000-daa619f04ad7";
        readonly string deviceName = "device";
        readonly byte[] defaultBytes = new byte[] { 0x5A, 0x17, 0x74, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0xD8, 0x0F, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0xF4, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0xFF, 0xFF, 0xAA, 0x01, 0x03, 0x3C, 0x00, 0x0E, 0x00, 0x00, 0x63, 0x28, 0xCC, 0xCC, 0x1E, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x00, 0x01 };

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestPPG_Green_Enabled()
        {
            var bytes = CopyDefaultBytes();

            //set bit 6 to 1, PPG_Green_Enabled to true
            bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] = (byte)(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] | 0b01000000);
            SensorPPG sensor = CreateDeviceAndReturnSensor(bytes);
            if (sensor.IsPPGGreenEnabled() != true)
            {
                Assert.Fail();
            }

            //set bit 6 to 0, PPG_Green_Enabled to false
            bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] = (byte)(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] & 0b10111111);
            sensor = CreateDeviceAndReturnSensor(bytes);
            if (sensor.IsPPGGreenEnabled() != false)
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestPPG_Red_Enabled()
        {
            var bytes = CopyDefaultBytes();

            //set bit 5 to 1, PPG_Red_Enabled to true
            bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] = (byte)(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] | 0b00100000);
            SensorPPG sensor = CreateDeviceAndReturnSensor(bytes);
            if (sensor.IsPPGRedEnabled() != true)
            {
                Assert.Fail();
            }

            //set bit 5 to 1, PPG_Red_Enabled to false
            bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] = (byte)(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] & 0b11011111);
            sensor = CreateDeviceAndReturnSensor(bytes);
            if (sensor.IsPPGRedEnabled() != false)
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestPPG_IR_Enabled()
        {
            var bytes = CopyDefaultBytes();

            //set bit 4 to 1, PPG_IR_Enabled to true
            bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] = (byte)(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] | 0b00010000);
            SensorPPG sensor = CreateDeviceAndReturnSensor(bytes);
            if (sensor.IsPPGIREnabled() != true)
            {
                Assert.Fail();
            }

            //set bit 4 to 1, PPG_IR_Enabled to false
            bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] = (byte)(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] & 0b11101111);
            sensor = CreateDeviceAndReturnSensor(bytes);
            if (sensor.IsPPGIREnabled() != false)
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestPPG_Blue_Enabled()
        {
            var bytes = CopyDefaultBytes();

            //set bit 2 to 1, PPG_Blue_Enabled to true
            bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] = (byte)(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] | 0b00000100);
            SensorPPG sensor = CreateDeviceAndReturnSensor(bytes);
            if (sensor.IsPPGBlueEnabled() != true)
            {
                Assert.Fail();
            }

            //set bit 2 to 1, PPG_Blue_Enabled to false
            bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] = (byte)(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] & 0b11111011);
            sensor = CreateDeviceAndReturnSensor(bytes);
            if (sensor.IsPPGBlueEnabled() != false)
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestGenerateOpConfig()
        {
            var bytes = CopyDefaultBytes();
            SensorPPG sensor = CreateDeviceAndReturnSensor(bytes);

            sensor.SetPPGGreenEnabled(true);
            sensor.SetPPGRedEnabled(true);
            sensor.SetPPGIREnabled(true);
            sensor.SetPPGBlueEnabled(true);
            sensor.SetPPGRecordingDurationinSeconds(99);
            sensor.SetPPGRecordingIntervalinMinutes(99);
            sensor.SetPPGSampleAverage(SensorPPG.SampleAverage.Sample_Average_16);
            sensor.SetSamplingRate(SensorPPG.SamplingRate.Freq_1600Hz);
            sensor.SetPPGPulseWidth(SensorPPG.LEDPulseWidth.Width_220uS);
            sensor.SetPPGRange(SensorPPG.ADCRange.Range_3);
            sensor.SetPGGDefaultLEDPulseAmplitude(100);
            sensor.SetMaxLEDPulseAmplitudeRedIR(90);
            sensor.SetMaxLEDPulseAmplitudeGreenBlue(80);
            sensor.SetAGCTargetRange(70);
            sensor.SetLEDPilotPulseAmplitude(60);

            sensor.GenerateOperationConfig(bytes);

            if (!(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] & 0b01000000).Equals(0b01000000) || //Check if PPGGreenEnabled is enabled
                !(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] & 0b00100000).Equals(0b00100000) || //Check if PPGRedEnabled is enabled
                !(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] & 0b00010000).Equals(0b00010000) || //Check if PPGIREnabled is enabled
                !(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] & 0b00000100).Equals(0b00000100) || //Check if PPGBlueEnabled is enabled
                !(bytes[(int)ConfigurationBytesIndexName.PPG_FIFO_CONFIG] & 0b11100000).Equals(0b10000000) || //Check if sample average equals 16
                !(bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b00011100).Equals(0b00011000) || //Check if sampling rate equals 1600Hz
                !(bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b01100000).Equals(0b01000000) || //Check if ADCRange equals Range_3
                !(bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b00000011).Equals(0b00000010) || //Check if LEDPulseWidth equals 220uS
                !bytes[(int)ConfigurationBytesIndexName.PPG_REC_DUR_SECS_LSB].Equals(0b01100011) || //Check if PPGRecordingInterval equals 99
                !bytes[(int)ConfigurationBytesIndexName.PPG_REC_INT_MINS_LSB].Equals(0b01100011) ||//Check if PPGRecordingDuration equals 99
                !bytes[(int)ConfigurationBytesIndexName.PPG_MA_DEFAULT].Equals(0b01100100) ||//Check if PPG_MA_DEFAULT equals 100
                !bytes[(int)ConfigurationBytesIndexName.PPG_MA_MAX_RED_IR].Equals(0b01011010) ||//Check if PPG_MA_MAX_RED_IR equals 90
                !bytes[(int)ConfigurationBytesIndexName.PPG_MA_MAX_GREEN_BLUE].Equals(0b01010000) ||//Check if PPG_MA_MAX_GREEN_BLUE equals 80
                !bytes[(int)ConfigurationBytesIndexName.PPG_AGC_TARGET_PERCENT_OF_RANGE].Equals(0b01000110) ||//Check if PPG_AGC_TARGET_PERCENT_OF_RANGE equals 70
                !bytes[(int)ConfigurationBytesIndexName.PPG_MA_LED_PILOT].Equals(0b00111100) //Check if PPG_MA_LED_PILOT equals 60
                )
            {
                Assert.Fail();
            }

            sensor.SetPPGGreenEnabled(false);
            sensor.SetPPGRedEnabled(false);
            sensor.SetPPGIREnabled(false);
            sensor.SetPPGBlueEnabled(false);
            sensor.SetPPGRecordingDurationinSeconds(168);
            sensor.SetPPGRecordingIntervalinMinutes(168);
            sensor.SetPPGSampleAverage(SensorPPG.SampleAverage.Sample_Average_8);
            sensor.SetSamplingRate(SensorPPG.SamplingRate.Freq_800Hz);
            sensor.SetPPGPulseWidth(SensorPPG.LEDPulseWidth.Width_120uS);
            sensor.SetPPGRange(SensorPPG.ADCRange.Range_2);
            sensor.SetDAC1CROSSTALK(20);
            sensor.SetDAC2CROSSTALK(20);
            sensor.SetDAC3CROSSTALK(20);
            sensor.SetDAC4CROSSTALK(20);
            sensor.SetProxAGCMode(SensorPPG.ProxAGCMode.Mode_3);

            sensor.GenerateOperationConfig(bytes);

            if (!(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] & 0b01000000).Equals(0b00000000) || //Check if PPGGreenEnabled is disabled
                !(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] & 0b00100000).Equals(0b00000000) || //Check if PPGRedEnabled is disabled
                !(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] & 0b00010000).Equals(0b00000000) || //Check if PPGIREnabled is disabled
                !(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] & 0b00000100).Equals(0b00000000) || //Check if PPGBlueEnabled is disabled
                !(bytes[(int)ConfigurationBytesIndexName.PPG_FIFO_CONFIG] & 0b11100000).Equals(0b01100000) || //Check if sample average equals 8
                !(bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b00011100).Equals(0b00010000) || //Check if sampling rate equals 800Hz
                !(bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b01100000).Equals(0b00100000) || //Check if ADCRange equals Range_2
                !(bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b00000011).Equals(0b00000001) || //Check if LEDPulseWidth equals 120uS
                !bytes[(int)ConfigurationBytesIndexName.PPG_REC_DUR_SECS_LSB].Equals(0b10101000) || //Check if PPGRecordingInterval equals 168
                !bytes[(int)ConfigurationBytesIndexName.PPG_REC_INT_MINS_LSB].Equals(0b10101000) ||//Check if PPGRecordingDuration equals 168
                !(bytes[(int)ConfigurationBytesIndexName.PPG_DAC1_CROSSTALK] & 0b00011111).Equals(0b00010100) ||//Check if PPG_DAC1_CROSSTALK equals 20
                !(bytes[(int)ConfigurationBytesIndexName.PPG_DAC2_CROSSTALK] & 0b00011111).Equals(0b00010100) ||//Check if PPG_DAC2_CROSSTALK equals 20
                !(bytes[(int)ConfigurationBytesIndexName.PPG_DAC3_CROSSTALK] & 0b00011111).Equals(0b00010100) ||//Check if PPG_DAC3_CROSSTALK equals 20
                !(bytes[(int)ConfigurationBytesIndexName.PPG_DAC4_CROSSTALK] & 0b00011111).Equals(0b00010100) ||//Check if PPG_DAC4_CROSSTALK equals 20
                !(bytes[(int)ConfigurationBytesIndexName.PROX_AGC_MODE] & 0b00000011).Equals(0b0000000010) //Check if PROX_AGC_MODE equals Mode 3
                )
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestSamplingRate()
        {
            var bytes = CopyDefaultBytes();

            //set the SamplingRate to Freq_50Hz
            bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] = (byte)((bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b11100011));
            SensorPPG sensor = CreateDeviceAndReturnSensor(bytes);
            if (!sensor.GetSamplingRate().Equals(SensorPPG.SamplingRate.Freq_50Hz))
            {
                Assert.Fail();
            }

            //set the SamplingRate to Freq_100Hz
            bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] = (byte)((bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b11100011) | 0b00000100);
            sensor = CreateDeviceAndReturnSensor(bytes);
            if (!sensor.GetSamplingRate().Equals(SensorPPG.SamplingRate.Freq_100Hz))
            {
                Assert.Fail();
            }

            //set the SamplingRate to Freq_200Hz
            bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] = (byte)((bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b11100011) | 0b00001000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            if (!sensor.GetSamplingRate().Equals(SensorPPG.SamplingRate.Freq_200Hz))
            {
                Assert.Fail();
            }

            //set the SamplingRate to Freq_400Hz
            bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] = (byte)((bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b11100011) | 0b00001100);
            sensor = CreateDeviceAndReturnSensor(bytes);
            if (!sensor.GetSamplingRate().Equals(SensorPPG.SamplingRate.Freq_400Hz))
            {
                Assert.Fail();
            }

            //set the SamplingRate to Freq_800Hz
            bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] = (byte)((bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b11100011) | 0b00010000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            if (!sensor.GetSamplingRate().Equals(SensorPPG.SamplingRate.Freq_800Hz))
            {
                Assert.Fail();
            }

            //set the SamplingRate to Freq_1000Hz
            bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] = (byte)((bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b11100011) | 0b00010100);
            sensor = CreateDeviceAndReturnSensor(bytes);
            if (!sensor.GetSamplingRate().Equals(SensorPPG.SamplingRate.Freq_1000Hz))
            {
                Assert.Fail();
            }

            //set the SamplingRate to Freq_1600Hz
            bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] = (byte)((bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b11100011) | 0b00011000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            if (!sensor.GetSamplingRate().Equals(SensorPPG.SamplingRate.Freq_1600Hz))
            {
                Assert.Fail();
            }

            //set the SamplingRate to Freq_3200Hz
            bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] = (byte)((bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b11100011) | 0b00011100);
            sensor = CreateDeviceAndReturnSensor(bytes);
            if (!sensor.GetSamplingRate().Equals(SensorPPG.SamplingRate.Freq_3200Hz))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestSampleAverage()
        {
            var bytes = CopyDefaultBytes();

            //set the SampleAverage to Sample_Average_1
            bytes[(int)ConfigurationBytesIndexName.PPG_FIFO_CONFIG] = (byte)((bytes[(int)ConfigurationBytesIndexName.PPG_FIFO_CONFIG] & 0b00011111));
            SensorPPG sensor = CreateDeviceAndReturnSensor(bytes);
            if (!sensor.GetPPGSampleAverage().Equals(SensorPPG.SampleAverage.Sample_Average_1))
            {
                Assert.Fail();
            }

            //set the SampleAverage to Sample_Average_2
            bytes[(int)ConfigurationBytesIndexName.PPG_FIFO_CONFIG] = (byte)((bytes[(int)ConfigurationBytesIndexName.PPG_FIFO_CONFIG] & 0b00011111) | 0b00100000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            if (!sensor.GetPPGSampleAverage().Equals(SensorPPG.SampleAverage.Sample_Average_2))
            {
                Assert.Fail();
            }

            //set the SampleAverage to Sample_Average_4
            bytes[(int)ConfigurationBytesIndexName.PPG_FIFO_CONFIG] = (byte)((bytes[(int)ConfigurationBytesIndexName.PPG_FIFO_CONFIG] & 0b00011111) | 0b01000000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            if (!sensor.GetPPGSampleAverage().Equals(SensorPPG.SampleAverage.Sample_Average_4))
            {
                Assert.Fail();
            }

            //set the SampleAverage to Sample_Average_8
            bytes[(int)ConfigurationBytesIndexName.PPG_FIFO_CONFIG] = (byte)((bytes[(int)ConfigurationBytesIndexName.PPG_FIFO_CONFIG] & 0b00011111) | 0b01100000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            if (!sensor.GetPPGSampleAverage().Equals(SensorPPG.SampleAverage.Sample_Average_8))
            {
                Assert.Fail();
            }

            //set the SampleAverage to Sample_Average_16
            bytes[(int)ConfigurationBytesIndexName.PPG_FIFO_CONFIG] = (byte)((bytes[(int)ConfigurationBytesIndexName.PPG_FIFO_CONFIG] & 0b00011111) | 0b10000000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            if (!sensor.GetPPGSampleAverage().Equals(SensorPPG.SampleAverage.Sample_Average_16))
            {
                Assert.Fail();
            }

            //set the SampleAverage to Sample_Average_32
            bytes[(int)ConfigurationBytesIndexName.PPG_FIFO_CONFIG] = (byte)((bytes[(int)ConfigurationBytesIndexName.PPG_FIFO_CONFIG] & 0b00011111) | 0b10100000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            if (!sensor.GetPPGSampleAverage().Equals(SensorPPG.SampleAverage.Sample_Average_32))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestADCRange()
        {
            var bytes = CopyDefaultBytes();

            //set the ADCRange to Range_1
            bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] = (byte)((bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b10011111));
            SensorPPG sensor = CreateDeviceAndReturnSensor(bytes);
            if (!sensor.GetPPGRange().Equals(SensorPPG.ADCRange.Range_1))
            {
                Assert.Fail();
            }

            //set the ADCRange to Range_2
            bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] = (byte)((bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b10011111) | 0b00100000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            if (!sensor.GetPPGRange().Equals(SensorPPG.ADCRange.Range_2))
            {
                Assert.Fail();
            }

            //set the ADCRange to Range_3
            bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] = (byte)((bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b10011111) | 0b01000000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            if (!sensor.GetPPGRange().Equals(SensorPPG.ADCRange.Range_3))
            {
                Assert.Fail();
            }

            //set the ADCRange to Range_4
            bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] = (byte)((bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b10011111) | 0b01100000);
            sensor = CreateDeviceAndReturnSensor(bytes);
            if (!sensor.GetPPGRange().Equals(SensorPPG.ADCRange.Range_4))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestLEDPulseWidth()
        {
            var bytes = CopyDefaultBytes();

            //set the LEDPulseWidth to Width_70uS
            bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] = (byte)((bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b11111100));
            SensorPPG sensor = CreateDeviceAndReturnSensor(bytes);
            if(!sensor.GetPPGPulseWidth().Equals(SensorPPG.LEDPulseWidth.Width_70uS))
            {
                Assert.Fail();
            }

            //set the LEDPulseWidth to Width_120uS
            bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] = (byte)((bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b11111100) | 0b00000001);
            sensor = CreateDeviceAndReturnSensor(bytes);
            if (!sensor.GetPPGPulseWidth().Equals(SensorPPG.LEDPulseWidth.Width_120uS))
            {
                Assert.Fail();
            }

            //set the LEDPulseWidth to Width_220uS
            bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] = (byte)((bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b11111100) | 0b00000010);
            sensor = CreateDeviceAndReturnSensor(bytes);
            if (!sensor.GetPPGPulseWidth().Equals(SensorPPG.LEDPulseWidth.Width_220uS))
            {
                Assert.Fail();
            }

            //set the LEDPulseWidth to Width_420uS
            bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] = (byte)((bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b11111100) | 0b00000011);
            sensor = CreateDeviceAndReturnSensor(bytes);
            if (!sensor.GetPPGPulseWidth().Equals(SensorPPG.LEDPulseWidth.Width_420uS))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestInitializeUsingOperationConfig()
        {
            var bytes = CopyDefaultBytes();

            //set PPG_Green_Enabled to true
            bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] = (byte)(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] | 0b01000000);
            //set PPG_Red_Enabled to true
            bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] = (byte)(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] | 0b00100000);
            //set PPG_IR_Enabled to true
            bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] = (byte)(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] | 0b00010000);
            //set PPG_Blue_Enabled to true
            bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] = (byte)(bytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] | 0b00000100);
            //set the SampleAverage to Sample_Average_32
            bytes[(int)ConfigurationBytesIndexName.PPG_FIFO_CONFIG] = (byte)((bytes[(int)ConfigurationBytesIndexName.PPG_FIFO_CONFIG] & 0b00011111) | 0b10100000);
            //set the SamplingRate to Freq_3200Hz
            bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] = (byte)((bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b11100011) | 0b00011100);
            //set the ADCRange to Range_4
            bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] = (byte)((bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b10011111) | 0b01100000);
            //set the LEDPulseWidth to Width_420uS
            bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] = (byte)((bytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b11111100) | 0b00000011);
            //set the PPGRecordingDuration to 100
            bytes[(int)ConfigurationBytesIndexName.PPG_REC_DUR_SECS_LSB] = 0b01100100;
            //bytes[(int)ConfigurationBytesIndexName.PPG_REC_DUR_SECS_MSB] = 0b01100100;
            //set the PPGRecordingInterval to 10
            bytes[(int)ConfigurationBytesIndexName.PPG_REC_INT_MINS_LSB] = 0b00001010;
            //bytes[(int)ConfigurationBytesIndexName.PPG_REC_INT_MINS_MSB] = 0b01100100;

            //set the PPG_MA_DEFAULT to 50
            bytes[(int)ConfigurationBytesIndexName.PPG_MA_DEFAULT] = 0b00110010;
            //set the PPG_MA_MAX_RED_IR to 60
            bytes[(int)ConfigurationBytesIndexName.PPG_MA_MAX_RED_IR] = 0b00111100;
            //set the PPG_MA_MAX_GREEN_BLUE to 70
            bytes[(int)ConfigurationBytesIndexName.PPG_MA_MAX_GREEN_BLUE] = 0b01000110;
            //set the PPG_AGC_TARGET_PERCENT_OF_RANGE to 80
            bytes[(int)ConfigurationBytesIndexName.PPG_AGC_TARGET_PERCENT_OF_RANGE] = 0b01010000;
            //set the PPG_MA_LED_PILOT to 90
            bytes[(int)ConfigurationBytesIndexName.PPG_MA_LED_PILOT] = 0b01011010;

            //set the PPG_DAC1_CROSSTALK to 31
            bytes[(int)ConfigurationBytesIndexName.PPG_DAC1_CROSSTALK] = (byte)(bytes[(int)ConfigurationBytesIndexName.PPG_DAC1_CROSSTALK] | 0b00011111);
            //set the PPG_DAC2_CROSSTALK to 31
            bytes[(int)ConfigurationBytesIndexName.PPG_DAC2_CROSSTALK] = (byte)(bytes[(int)ConfigurationBytesIndexName.PPG_DAC2_CROSSTALK] | 0b00011111);
            //set the PPG_DAC3_CROSSTALK to 31
            bytes[(int)ConfigurationBytesIndexName.PPG_DAC3_CROSSTALK] = (byte)(bytes[(int)ConfigurationBytesIndexName.PPG_DAC3_CROSSTALK] | 0b00011111);
            //set the PPG_DAC4_CROSSTALK to 31
            bytes[(int)ConfigurationBytesIndexName.PPG_DAC4_CROSSTALK] = (byte)(bytes[(int)ConfigurationBytesIndexName.PPG_DAC4_CROSSTALK] | 0b00011111);
            //set the PROX_AGC_MODE to Mode_2
            bytes[(int)ConfigurationBytesIndexName.PROX_AGC_MODE] = (byte)((bytes[(int)ConfigurationBytesIndexName.PROX_AGC_MODE] & 0b11111100) | 0b00000001);

            SensorPPG sensor = CreateDeviceAndReturnSensor(bytes);

            if (sensor.IsPPGGreenEnabled() && 
                sensor.IsPPGRedEnabled() && 
                sensor.IsPPGIREnabled() && 
                sensor.IsPPGBlueEnabled() &&
                sensor.GetPPGSampleAverage().Equals(SensorPPG.SampleAverage.Sample_Average_32) && 
                sensor.GetPPGRecordingDurationinSeconds().Equals(100) &&
                sensor.GetPPGRecordingIntervalinMinutes().Equals(10) &&
                sensor.GetPPGRange().Equals(SensorPPG.ADCRange.Range_4) &&
                sensor.GetSamplingRate().Equals(SensorPPG.SamplingRate.Freq_3200Hz) &&
                sensor.GetPPGPulseWidth().Equals(SensorPPG.LEDPulseWidth.Width_420uS) &&
                sensor.GetPGGDefaultLEDPulseAmplitude().Equals(50) &&
                sensor.GetMaxLEDPulseAmplitudeRedIR().Equals(60) &&
                sensor.GetMaxLEDPulseAmplitudeGreenBlue().Equals(70) &&
                sensor.GetAGCTargetRange().Equals(80) &&
                sensor.GetLEDPilotPulseAmplitude().Equals(90) &&
                sensor.GetDAC1CROSSTALK().Equals(31) &&
                sensor.GetDAC2CROSSTALK().Equals(31) &&
                sensor.GetDAC3CROSSTALK().Equals(31) &&
                sensor.GetDAC4CROSSTALK().Equals(31) &&
                sensor.GetProxAGCMode().Equals(SensorPPG.ProxAGCMode.Mode_2))
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
            var bytes = CopyDefaultBytes();
            SensorPPG sensor = CreateDeviceAndReturnSensor(bytes);
            if (sensor.GetSensorName().Equals(SensorPPG.SensorName))
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

        private SensorPPG CreateDeviceAndReturnSensor(byte[] bytes)
        {
            VerisenseBLEDevice clone = new TestVerisenseBLEDevice(uuid, deviceName, bytes);
            VerisenseBLEDevice bleDevice = new VerisenseBLEDevice(clone);
            return (SensorPPG)bleDevice.GetSensor(SensorPPG.SensorName);
        }
    }
}
