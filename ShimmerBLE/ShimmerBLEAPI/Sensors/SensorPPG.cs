using shimmer.Helpers;
using ShimmerAPI;
using ShimmerAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using static shimmer.Models.OpConfigPayload;

namespace shimmer.Sensors
{
    public class SensorPPG : Sensor
    {
        public static readonly string SensorName = "PPG";

        protected bool PPG_Red_Enabled = false;
        protected bool PPG_Green_Enabled = false;
        protected bool PPG_IR_Enabled = false;
        protected bool PPG_Blue_Enabled = false;
        //listofMax86XXXAdcLsb = [7.8125, 15.625, 31.25, 62.5]
        //listofMax86XXXAdcBitShift = [2 * *7, 2 * *6, 2 * *5, 2 * *4]
        private double[] ArrayOfMax86XXXAdcLsb = new double[]{7.8125, 15.625, 31.25, 62.5};
        private double[] ArrayOfMax86XXXAdcBitShift = new double[] { Math.Pow(2,7), Math.Pow(2, 6), Math.Pow(2, 5), Math.Pow(2, 4) };
        int PPGRecordingDurationinSeconds = -1;
        int PPGRecordingIntervalinMinutes = -1;
        SensorSetting PPG_SMP_AVE = Sensor.UnknownSetting;
        SensorSetting PPG_ADC_RGE = Sensor.UnknownSetting;
        SensorSetting PPG_SR = Sensor.UnknownSetting;
        SensorSetting PPG_LED_PW = Sensor.UnknownSetting;
        int PPG_MA_DEFAULT = -1;
        int PPG_MA_MAX_RED_IR = -1;
        int PPG_MA_MAX_GREEN_BLUE = -1;
        int PPG_AGC_TARGET_PERCENT_OF_RANGE = -1;
        int PPG_MA_LED_PILOT = -1;
        int PPG_XTALK_DAC1 = -1;
        int PPG_XTALK_DAC2 = -1;
        int PPG_XTALK_DAC3 = -1;
        int PPG_XTALK_DAC4 = -1;
        SensorSetting PROX_AGC_MODE = Sensor.UnknownSetting;
        public static class ObjectClusterSensorName
        {
            public static String PPG_GREEN = SensorName + "_Green";
            public static String PPG_RED = SensorName + "_Red";
            public static String PPG_IR = SensorName + "_IR";
            public static String PPG_BLUE = SensorName + "_Blue";
        }
        public static class SamplingRate
        {
            public static readonly SensorSetting Rate_Unknown = Sensor.UnknownSetting;
            public static readonly SensorSetting Freq_50Hz = new SensorSetting("50.0Hz", 0, 50);
            public static readonly SensorSetting Freq_100Hz = new SensorSetting("100.0Hz", 1, 100);
            public static readonly SensorSetting Freq_200Hz = new SensorSetting("200.0Hz", 2, 200);
            public static readonly SensorSetting Freq_400Hz = new SensorSetting("400.0Hz", 3, 400);
            public static readonly SensorSetting Freq_800Hz = new SensorSetting("800.0Hz", 4, 800);
            public static readonly SensorSetting Freq_1000Hz = new SensorSetting("1000.0Hz", 5, 1000);
            public static readonly SensorSetting Freq_1600Hz = new SensorSetting("1600.0Hz", 6, 1600);
            public static readonly SensorSetting Freq_3200Hz = new SensorSetting("3200.0Hz", 7, 3200);
            public static readonly SensorSetting[] Settings = { Rate_Unknown, Freq_50Hz, Freq_100Hz , Freq_200Hz , Freq_400Hz , Freq_800Hz , Freq_1000Hz , Freq_1600Hz , Freq_3200Hz };
        }

        public static class SampleAverage
        {
            public static readonly SensorSetting Sample_Average_Unknown = Sensor.UnknownSetting;
            public static readonly SensorSetting Sample_Average_1 = new SensorSetting("Sample Average = 1", 0, 1);
            public static readonly SensorSetting Sample_Average_2 = new SensorSetting("Sample Average = 2", 1, 2);
            public static readonly SensorSetting Sample_Average_4 = new SensorSetting("Sample Average = 4", 2, 4);
            public static readonly SensorSetting Sample_Average_8 = new SensorSetting("Sample Average = 8", 3, 8);
            public static readonly SensorSetting Sample_Average_16 = new SensorSetting("Sample Average = 16", 4, 16);
            public static readonly SensorSetting Sample_Average_32 = new SensorSetting("Sample Average = 32", 5, 32);
            public static readonly SensorSetting[] Settings = { Sample_Average_Unknown, Sample_Average_1, Sample_Average_2, Sample_Average_4, Sample_Average_8, Sample_Average_16, Sample_Average_32 };
        }

        public static class ADCRange
        {
            public static readonly SensorSetting Range_Unknown = Sensor.UnknownSetting;
            public static readonly SensorSetting Range_1 = new SensorSetting("Range 1", 0, 4096, "Full Scale = 4096");
            public static readonly SensorSetting Range_2 = new SensorSetting("Range 2", 1, 8192, "Full Scale = 8192");
            public static readonly SensorSetting Range_3 = new SensorSetting("Range 3", 2, 16384, "Full Scale = 16384");
            public static readonly SensorSetting Range_4 = new SensorSetting("Range 4", 3, 32768, "Full Scale = 32768");
            public static readonly SensorSetting[] Settings = { Range_Unknown, Range_1 , Range_2 , Range_3 , Range_4 };
        }

        public static class LEDPulseWidth
        {
            public static readonly SensorSetting Pulse_Width_Unknown = Sensor.UnknownSetting;
            public static readonly SensorSetting Width_70uS = new SensorSetting("70\u00B5s", 0, 70, "Pulse Width 70\u00B5s, Integration Time 50\u00B5s, 19 Bits Resolution");
            public static readonly SensorSetting Width_120uS = new SensorSetting("120\u00B5s", 1, 120, "Pulse Width 120\u00B5s, Integration Time 100\u00B5s, 19 Bits Resolution");
            public static readonly SensorSetting Width_220uS = new SensorSetting("220\u00B5s", 2, 220, "Pulse Width 220\u00B5s, Integration Time 200\u00B5s, 19 Bits Resolution");
            public static readonly SensorSetting Width_420uS = new SensorSetting("420\u00B5s", 3, 420, "Pulse Width 420\u00B5s, Integration Time 400\u00B5s, 19 Bits Resolution");
            public static readonly SensorSetting[] Settings = { Pulse_Width_Unknown, Width_70uS, Width_120uS, Width_220uS, Width_420uS };
        }

        public static class ProxAGCMode
        {
            public static readonly SensorSetting Mode_Unknown = Sensor.UnknownSetting;
            public static readonly SensorSetting Mode_1 = new SensorSetting("Mode 1", 0, "", "Auto-gain control disabled, proximity detection disabled");
            public static readonly SensorSetting Mode_2 = new SensorSetting("Mode 2", 1, "", "Auto-gain control enabled, proximity detection enabled (driver approach)");
            public static readonly SensorSetting Mode_3 = new SensorSetting("Mode 3", 2, "", "Auto-gain control enabled, proximity detection enabled (hybrid approach)");
            public static readonly SensorSetting[] Settings = { Mode_Unknown, Mode_1, Mode_2, Mode_3 };
        }

        public override byte[] GenerateOperationConfig(byte[] operationalConfigBytes)
        {
            if (PPG_Green_Enabled)
            {
                operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] | 0b01000000);
            }
            else
            {
                operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] & 0b10111111);
            }

            if (PPG_Red_Enabled)
            {
                operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] | 0b00100000);
            }
            else
            {
                operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] & 0b11011111);
            }

            if (PPG_IR_Enabled)
            {
                operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] | 0b00010000);
            }
            else
            {
                operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] & 0b11101111);
            }

            if (PPG_Blue_Enabled)
            {
                operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] | 0b00000100);
            }
            else
            {
                operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] & 0b11111011);
            }
            operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_FIFO_CONFIG] = (byte)((operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_FIFO_CONFIG] & 0b00011111) | (PPG_SMP_AVE.GetConfigurationValue() << 5));
            operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] = (byte)((operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b10011111) | (PPG_ADC_RGE.GetConfigurationValue() << 5));
            operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] = (byte)((operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b11100011) | (PPG_SR.GetConfigurationValue() << 2));
            operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] = (byte)((operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0b11111100) | (PPG_LED_PW.GetConfigurationValue()));

            operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_REC_DUR_SECS_LSB] = (byte)(PPGRecordingDurationinSeconds & 0xFF);
            operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_REC_DUR_SECS_MSB] = (byte)((PPGRecordingDurationinSeconds >> 8) & 0xFF);

            operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_REC_INT_MINS_LSB] = (byte)(PPGRecordingIntervalinMinutes & 0xFF);
            operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_REC_INT_MINS_MSB] = (byte)((PPGRecordingIntervalinMinutes >> 8) & 0xFF);

            operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_MA_DEFAULT] = (byte)(PPG_MA_DEFAULT & 0xFF);
            operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_MA_MAX_RED_IR] = (byte)(PPG_MA_MAX_RED_IR & 0xFF);
            operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_MA_MAX_GREEN_BLUE] = (byte)(PPG_MA_MAX_GREEN_BLUE & 0xFF);
            operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_AGC_TARGET_PERCENT_OF_RANGE] = (byte)(PPG_AGC_TARGET_PERCENT_OF_RANGE & 0xFF);
            operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_MA_LED_PILOT] = (byte)(PPG_MA_LED_PILOT & 0xFF);

            operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_DAC1_CROSSTALK] = (byte)(PPG_XTALK_DAC1 & 0b00011111);
            operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_DAC2_CROSSTALK] = (byte)(PPG_XTALK_DAC2 & 0b00011111);
            operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_DAC3_CROSSTALK] = (byte)(PPG_XTALK_DAC3 & 0b00011111);
            operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_DAC4_CROSSTALK] = (byte)(PPG_XTALK_DAC4 & 0b00011111);

            operationalConfigBytes[(int)ConfigurationBytesIndexName.PROX_AGC_MODE] = (byte)((operationalConfigBytes[(int)ConfigurationBytesIndexName.PROX_AGC_MODE] & 0b11111100) | (PROX_AGC_MODE.GetConfigurationValue()));

            return operationalConfigBytes;
        }

        public override string GetSensorName()
        {
            return SensorName;
        }

        public override void InitializeUsingOperationConfig(byte[] operationalConfigBytes)
        {
            if ((int)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] & 0b01000000) > 0)
            {
                PPG_Green_Enabled = true;
            }
            else
            {
                PPG_Green_Enabled = false;
            }
            if ((int)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] & 0b00100000) > 0)
            {
                PPG_Red_Enabled = true;
            }
            else
            {
                PPG_Red_Enabled = false;
            }
            if ((int)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] & 0b00010000) > 0)
            {
                PPG_IR_Enabled = true;
            }
            else
            {
                PPG_IR_Enabled = false;
            }
            if ((int)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_1] & 0b00000100) > 0)
            {
                PPG_Blue_Enabled = true;
            }
            else
            {
                PPG_Blue_Enabled = false;
            }

            int ppgsmpavg = ((operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_FIFO_CONFIG] & 0xF0)>> 5);
            PPG_SMP_AVE = GetSensorSettingFromConfigurationValue(SampleAverage.Settings, ppgsmpavg);
            PPGRecordingDurationinSeconds = ((operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_REC_DUR_SECS_LSB]) + (operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_REC_DUR_SECS_MSB] >> 8));
            PPGRecordingIntervalinMinutes = ((operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_REC_INT_MINS_LSB]) + (operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_REC_INT_MINS_MSB] >> 8));
            int ppgadcrge = ((operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0x60) >> 5);
            PPG_ADC_RGE = GetSensorSettingFromConfigurationValue(ADCRange.Settings, ppgadcrge);
            int ppgsr = ((operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0x1C) >> 2);
            PPG_SR = GetSensorSettingFromConfigurationValue(SamplingRate.Settings, ppgsr);
            int ppgpw = ((operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_MODE_CONFIG2] & 0x03));
            PPG_LED_PW = GetSensorSettingFromConfigurationValue(LEDPulseWidth.Settings, ppgpw);

            PPG_MA_DEFAULT = operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_MA_DEFAULT];
            PPG_MA_MAX_RED_IR = operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_MA_MAX_RED_IR];
            PPG_MA_MAX_GREEN_BLUE = operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_MA_MAX_GREEN_BLUE];
            PPG_AGC_TARGET_PERCENT_OF_RANGE = operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_AGC_TARGET_PERCENT_OF_RANGE];
            PPG_MA_LED_PILOT = operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_MA_LED_PILOT];

            PPG_XTALK_DAC1 = (operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_DAC1_CROSSTALK] & 0b00011111);
            PPG_XTALK_DAC2 = (operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_DAC2_CROSSTALK] & 0b00011111);
            PPG_XTALK_DAC3 = (operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_DAC3_CROSSTALK] & 0b00011111);
            PPG_XTALK_DAC4 = (operationalConfigBytes[(int)ConfigurationBytesIndexName.PPG_DAC4_CROSSTALK] & 0b00011111);

            int ppgproxagcmode = ((operationalConfigBytes[(int)ConfigurationBytesIndexName.PROX_AGC_MODE] & 0b00000011));
            PROX_AGC_MODE = GetSensorSettingFromConfigurationValue(ProxAGCMode.Settings, ppgproxagcmode);
        }

        public override List<ObjectCluster> ParsePayloadData(byte[] payload, string deviceID)
        {
            var numberOfBytesPerSample = 0;
            /*  Element Order
            1   PPG_RED
            2   PPG_IR
            3   PPG_GREEN
            4   PPG_BLUE
            */
            if (PPG_Red_Enabled)
            {
                numberOfBytesPerSample += 3;
            }
            if (PPG_IR_Enabled)
            {
                numberOfBytesPerSample += 3;
            }
            if (PPG_Green_Enabled)
            {
                numberOfBytesPerSample += 3;
            }
            if (PPG_Blue_Enabled)
            {
                numberOfBytesPerSample += 3;
            }
            var numberofSamples = payload.Length / numberOfBytesPerSample;
            numberofSamples = 17;
            List<ObjectCluster> listOfOJCs = new List<ObjectCluster>();
            for (int i = 0; i < numberofSamples; i++)
            {
                int startingIndex = numberOfBytesPerSample * i;
                byte[] sampleData = new byte[numberOfBytesPerSample];
                Array.Copy(payload, startingIndex, sampleData, 0, numberOfBytesPerSample);
                ObjectCluster ojc = new ObjectCluster(deviceID, deviceID);

                //ojc.RawTimeStamp = (int)tick;
                listOfOJCs.Add(ParseSensorData(sampleData, ojc));
                //if (ShimmerBLEEvent != null)
                //ShimmerBLEEvent.Invoke(null, new ShimmerBLEEventData { ASMID = Asm_uuid.ToString(), CurrentEvent = VerisenseBLEEvent.NewDataPacket, ObjMsg = ojc });
            }

            return listOfOJCs;
        }

        public override ObjectCluster ParseSensorData(byte[] sample, ObjectCluster ojc)
        {
            int startingIndex = 0;
            if (PPG_Red_Enabled)
            {
                byte[] samplesensor = new byte[3];
                Array.Copy(sample, startingIndex, samplesensor, 0, 3);
                var uncalValue = ProgrammerUtilities.ByteArrayToInt(samplesensor, false, false);
                uncalValue &= 0x7FFFF;
                ojc.Add(ObjectClusterSensorName.PPG_RED, ShimmerConfiguration.SignalFormats.RAW, ShimmerConfiguration.SignalUnits.NoUnits, uncalValue);
                var calValue = CalibrateValuesIntoMilliVolts(uncalValue);
                ojc.Add(ObjectClusterSensorName.PPG_RED, ShimmerConfiguration.SignalFormats.CAL, ShimmerConfiguration.SignalUnits.NanoAmpere, calValue);
                startingIndex += 3;
            }
            if (PPG_IR_Enabled)
            {
                byte[] samplesensor = new byte[3];
                Array.Copy(sample, startingIndex, samplesensor, 0, 3);
                var uncalValue = ProgrammerUtilities.ByteArrayToInt(samplesensor, false, false);
                uncalValue &= 0x7FFFF;
                ojc.Add(ObjectClusterSensorName.PPG_IR, ShimmerConfiguration.SignalFormats.RAW, ShimmerConfiguration.SignalUnits.NoUnits, uncalValue);
                var calValue = CalibrateValuesIntoMilliVolts(uncalValue);
                ojc.Add(ObjectClusterSensorName.PPG_IR, ShimmerConfiguration.SignalFormats.CAL, ShimmerConfiguration.SignalUnits.NanoAmpere, calValue);
                startingIndex += 3;
            }
            if (PPG_Green_Enabled)
            {
                byte[] samplesensor = new byte[3];
                Array.Copy(sample, startingIndex, samplesensor, 0, 3);
                var uncalValue = ProgrammerUtilities.ByteArrayToInt(samplesensor, false, false);
                uncalValue &= 0x7FFFF;
                ojc.Add(ObjectClusterSensorName.PPG_GREEN, ShimmerConfiguration.SignalFormats.RAW, ShimmerConfiguration.SignalUnits.NoUnits, uncalValue);
                var calValue = CalibrateValuesIntoMilliVolts(uncalValue);
                ojc.Add(ObjectClusterSensorName.PPG_GREEN, ShimmerConfiguration.SignalFormats.CAL, ShimmerConfiguration.SignalUnits.NanoAmpere, calValue);
                startingIndex += 3;
            }
            if (PPG_Blue_Enabled)
            {
                byte[] samplesensor = new byte[3];
                Array.Copy(sample, startingIndex, samplesensor, 0, 3);
                var uncalValue = ProgrammerUtilities.ByteArrayToInt(samplesensor, false, false);
                uncalValue &= 0x7FFFF;
                ojc.Add(ObjectClusterSensorName.PPG_BLUE, ShimmerConfiguration.SignalFormats.RAW, ShimmerConfiguration.SignalUnits.NoUnits, uncalValue);
                var calValue = CalibrateValuesIntoMilliVolts(uncalValue);
                ojc.Add(ObjectClusterSensorName.PPG_BLUE, ShimmerConfiguration.SignalFormats.CAL, ShimmerConfiguration.SignalUnits.NanoAmpere, calValue);
                
            }
            return ojc;
        }

        private double CalibrateValuesIntoMilliVolts(double uncalValue)
        {
            //listofMax86XXXAdcLsb = [7.8125, 15.625, 31.25, 62.5]
            //listofMax86XXXAdcBitShift = [2 * *7, 2 * *6, 2 * *5, 2 * *4]
            //adc_resolution = asm_device.configOperational.current_settings.PPG_ADC_RGE
            //cal_value = uncal_value / asm_device.configOperational.listofMax86XXXAdcBitShift[adc_resolution]
            //cal_value *= asm_device.configOperational.listofMax86XXXAdcLsb[adc_resolution]
            //cal_value /= 1000
            var adcResolution = PPG_ADC_RGE.GetConfigurationValue();
            var calValue = uncalValue / ArrayOfMax86XXXAdcBitShift[adcResolution];
            calValue *= ArrayOfMax86XXXAdcLsb[adcResolution];
            calValue /= 1000;
            return calValue;
        }

        public void SetPPGGreenEnabled(bool enabled)
        {
            PPG_Green_Enabled = enabled;
        }
        public void SetPPGRedEnabled(bool enabled)
        {
            PPG_Red_Enabled = enabled;
        }
        public void SetPPGIREnabled(bool enabled)
        {
            PPG_IR_Enabled = enabled;
        }
        public void SetPPGBlueEnabled(bool enabled)
        {
            PPG_Blue_Enabled = enabled;
        }
        public bool IsPPGGreenEnabled()
        {
            return PPG_Green_Enabled;
        }
        public bool IsPPGRedEnabled()
        {
            return PPG_Red_Enabled;
        }
        public bool IsPPGIREnabled()
        {
            return PPG_IR_Enabled;
        }
        public bool IsPPGBlueEnabled()
        {
            return PPG_Blue_Enabled;
        }

        public int GetPPGRecordingDurationinSeconds()
        {
            return PPGRecordingDurationinSeconds;
        }
        public void SetPPGRecordingDurationinSeconds(int ppgRecordingDuration)
        {
            PPGRecordingDurationinSeconds = ppgRecordingDuration;
        }
        public int GetPPGRecordingIntervalinMinutes()
        {
            return PPGRecordingIntervalinMinutes;
        }
        public void SetPPGRecordingIntervalinMinutes(int ppgRecordingInterval)
        {
            PPGRecordingIntervalinMinutes = ppgRecordingInterval;
        }
        public int GetPGGDefaultLEDPulseAmplitude()
        {
            return PPG_MA_DEFAULT;
        }
        public void SetPGGDefaultLEDPulseAmplitude(int pggDefaultLEDPulseAmplitude)
        {
            PPG_MA_DEFAULT = pggDefaultLEDPulseAmplitude;
        }
        public int GetMaxLEDPulseAmplitudeRedIR()
        {
            return PPG_MA_MAX_RED_IR;
        }
        public void SetMaxLEDPulseAmplitudeRedIR(int maxLEDPulseAmplitudeRedIR)
        {
            PPG_MA_MAX_RED_IR = maxLEDPulseAmplitudeRedIR;
        }
        public int GetMaxLEDPulseAmplitudeGreenBlue()
        {
            return PPG_MA_MAX_GREEN_BLUE;
        }
        public void SetMaxLEDPulseAmplitudeGreenBlue(int maxLEDPulseAmplitudeGreenBlue)
        {
            PPG_MA_MAX_GREEN_BLUE = maxLEDPulseAmplitudeGreenBlue;
        }
        public int GetAGCTargetRange()
        {
            return PPG_AGC_TARGET_PERCENT_OF_RANGE;
        }
        public void SetAGCTargetRange(int targetRange)
        {
            PPG_AGC_TARGET_PERCENT_OF_RANGE = targetRange;
        }
        public int GetLEDPilotPulseAmplitude()
        {
            return PPG_MA_LED_PILOT;
        }
        public void SetLEDPilotPulseAmplitude(int amplitude)
        {
            PPG_MA_LED_PILOT = amplitude;
        }
        public int GetDAC1CROSSTALK()
        {
            return PPG_XTALK_DAC1;
        }
        public void SetDAC1CROSSTALK(int value)
        {
            PPG_XTALK_DAC1 = value;
        }
        public int GetDAC2CROSSTALK()
        {
            return PPG_XTALK_DAC2;
        }
        public void SetDAC2CROSSTALK(int value)
        {
            PPG_XTALK_DAC2 = value;
        }
        public int GetDAC3CROSSTALK()
        {
            return PPG_XTALK_DAC3;
        }
        public void SetDAC3CROSSTALK(int value)
        {
            PPG_XTALK_DAC3 = value;
        }
        public int GetDAC4CROSSTALK()
        {
            return PPG_XTALK_DAC4;
        }
        public void SetDAC4CROSSTALK(int value)
        {
            PPG_XTALK_DAC4 = value;
        }
        public SensorSetting GetProxAGCMode()
        {
            return PROX_AGC_MODE;
        }
        public void SetProxAGCMode(SensorSetting value)
        {
            PROX_AGC_MODE = value;
        }
        public SensorSetting GetPPGSampleAverage()
        {
            return PPG_SMP_AVE;
        }
        public void SetPPGSampleAverage(SensorSetting setting)
        {
            PPG_SMP_AVE = setting;
        }
        public override SensorSetting GetSamplingRate()
        {
            return PPG_SR;
        }
        public override void SetSamplingRate(SensorSetting rate)
        {
            PPG_SR = rate;
        }
        public SensorSetting GetPPGPulseWidth()
        {
            return PPG_LED_PW;
        }
        public void SetPPGPulseWidth(SensorSetting setting)
        {
            PPG_LED_PW = setting;
        }
        public SensorSetting GetPPGRange()
        {
            return PPG_ADC_RGE;
        }
        public void SetPPGRange(SensorSetting setting)
        {
            PPG_ADC_RGE = setting;
        }
    }
}
