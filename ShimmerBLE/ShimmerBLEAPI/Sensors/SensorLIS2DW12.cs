﻿using shimmer.Helpers;
using ShimmerAPI;
using ShimmerAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using static shimmer.Models.OpConfigPayload;

namespace shimmer.Sensors
{
	/// <summary>
	/// This class contains the configuration settings for sensor accel1. Every sensor should belong to a device
	/// </summary>
	public class SensorLIS2DW12 : Sensor //ACCEL ONLY
	{
		protected bool Enabled;
		//ACCEL1_CFG_1
		protected bool HighPassFilterEnabled;
		//ACCEL1_CFG_1
		protected bool LowNoiseEnabled;
		//ACCEL1_CFG_2
		protected bool HighPassFilterRefModeEnabled;
		public static readonly string SensorName = "Accel1";

		protected SensorSetting RangeSetting = Sensor.UnknownSetting;
		protected SensorSetting RateSetting = Sensor.UnknownSetting;
		protected SensorSetting ModeSetting = Sensor.UnknownSetting;
		protected SensorSetting LPModeSetting = Sensor.UnknownSetting;
		protected SensorSetting BWFilterSetting = Sensor.UnknownSetting;
		protected SensorSetting FModeSetting = Sensor.UnknownSetting;
		int FIFOThresholdSetting = -1;

		public static readonly double[,] DEFAULT_OFFSET_VECTOR_LIS2DW12 = { { 0 }, { 0 }, { 0 } };
		public static readonly double[,] DEFAULT_ALIGNMENT_MATRIX_LIS2DW12 = { { 0, 0, 1 }, { 1, 0, 0 }, { 0, 1, 0 } };

		public static readonly double[,] DEFAULT_SENSITIVITY_MATRIX_LIS2DW12_2G = { { 1671.665922915, 0, 0 }, { 0, 1671.665922915, 0 }, { 0, 0, 1671.665922915 } };
		public static readonly double[,] DEFAULT_SENSITIVITY_MATRIX_LIS2DW12_4G = { { 835.832961457, 0, 0 }, { 0, 835.832961457, 0 }, { 0, 0, 835.832961457 } };
		public static readonly double[,] DEFAULT_SENSITIVITY_MATRIX_LIS2DW12_8G = { { 417.916480729, 0, 0 }, { 0, 417.916480729, 0 }, { 0, 0, 417.916480729 } };
		public static readonly double[,] DEFAULT_SENSITIVITY_MATRIX_LIS2DW12_16G = { { 208.958240364, 0, 0 }, { 0, 208.958240364, 0 }, { 0, 0, 208.958240364 } };

		/// <summary>
		/// This is used to store and retrieve the sensor data in the object cluster
		/// </summary>
		public static class ObjectClusterSensorName
		{
			public static String LIS2DW12_ACC_X = SensorName + "_X";
			public static String LIS2DW12_ACC_Y = SensorName + "_Y";
			public static String LIS2DW12_ACC_Z = SensorName + "_Z";
		}
		//ACCEL1_CFG_1
		/// <summary>
		/// The configuration of the full-scale range in accel1
		/// </summary>
		public static class AccelRange
		{
			/*
			[Display(Name = "Unknown")] Unknown = -1,
			[Display(Name = "\u00B12G")] TwoG = 0,
			[Display(Name = "\u00B14G")] FourG = 1,
			[Display(Name = "\u00B18G")] EightG = 2,
			[Display(Name = "\u00B116G")] SixteenG = 3
			*/
			public static readonly SensorSetting Range_Unknown = Sensor.UnknownSetting;
			public static readonly SensorSetting Range_2G = new SensorSetting("\u00B12G", 0, 2);
			public static readonly SensorSetting Range_4G = new SensorSetting("\u00B14G", 1, 4);
			public static readonly SensorSetting Range_8G = new SensorSetting("\u00B18G", 2, 8);
			public static readonly SensorSetting Range_16G = new SensorSetting("\u00B116G", 3, 16);
			public static readonly SensorSetting[] Settings = { Range_Unknown, Range_2G, Range_4G, Range_8G, Range_16G};

		}

		//ACCEL1_CFG_0
		/// <summary>
		/// The configuration of high performance mode sampling rate in accel1
		/// </summary>
		public static class HighPerformanceAccelSamplingRate
		{
			public static readonly SensorSetting Rate_Unknown = Sensor.UnknownSetting;
			public static readonly SensorSetting Power_Down = new SensorSetting("Power-down",0,0);
			public static readonly SensorSetting Freq_12_5Hz = new SensorSetting("12.5Hz", 1, 12.5);
			public static readonly SensorSetting Freq_25Hz = new SensorSetting("25.0Hz", 3, 25);
			public static readonly SensorSetting Freq_50Hz = new SensorSetting("50.0Hz", 4, 50);
			public static readonly SensorSetting Freq_100Hz = new SensorSetting("100.0Hz", 5, 100);
			public static readonly SensorSetting Freq_200Hz = new SensorSetting("200.0Hz", 6, 200);
			public static readonly SensorSetting Freq_400Hz = new SensorSetting("400.0Hz", 7, 400);
			public static readonly SensorSetting Freq_800Hz = new SensorSetting("800.0Hz", 8, 800);
			public static readonly SensorSetting Freq_1600Hz = new SensorSetting("1600.0Hz", 9, 1600);
			public static readonly SensorSetting[] Settings = {Rate_Unknown, Power_Down, Freq_12_5Hz, Freq_25Hz, Freq_50Hz, Freq_100Hz, Freq_200Hz, Freq_400Hz, Freq_800Hz, Freq_1600Hz};
		}
		//ACCEL1_CFG_0
		/// <summary>
		/// The configuration of low power mode sampling rate in accel1
		/// </summary>
		public static class LowPerformanceAccelSamplingRate
		{
			public static readonly SensorSetting Rate_Unknown = Sensor.UnknownSetting;
			public static readonly SensorSetting Power_Down = new SensorSetting("Power-down", 0, 0);
			public static readonly SensorSetting Freq_1_6Hz = new SensorSetting("1.6Hz", 1, 1.6);
			public static readonly SensorSetting Freq_12_5Hz = new SensorSetting("12.5Hz", 2, 12.5);
			public static readonly SensorSetting Freq_25Hz = new SensorSetting("25.0Hz", 3, 25);
			public static readonly SensorSetting Freq_50Hz = new SensorSetting("50.0Hz", 4, 50);
			public static readonly SensorSetting Freq_100Hz = new SensorSetting("100.0Hz", 5, 100);
			public static readonly SensorSetting Freq_200Hz = new SensorSetting("200.0Hz", 6, 200);
			public static readonly SensorSetting[] Settings = {Rate_Unknown, Power_Down, Freq_1_6Hz, Freq_12_5Hz, Freq_25Hz, Freq_50Hz, Freq_100Hz, Freq_200Hz};
		}
		//ACCEL1_CFG_0
		/// <summary>
		/// The configuration of the mode in accel1
		/// </summary>
		public static class Mode
		{
			public static readonly SensorSetting Mode_Unknown = Sensor.UnknownSetting;
			public static readonly SensorSetting Low_Power_Mode = new SensorSetting("Low-power Mode", 0, "", "Low-Power Mode (12/14-bit resolution)");
			public static readonly SensorSetting High_Performance_Mode = new SensorSetting("High-performance Mode", 1, "", "High-Performance Mode (14-bit resolution)");
			public static readonly SensorSetting On_Demand = new SensorSetting("On Demand", 2, "", "Single data conversion on demand mode (12/14-bit resolution)");
			public static readonly SensorSetting Reserved = new SensorSetting("Reserved", 3, "", "Reserved");
			public static readonly SensorSetting[] Settings = { Mode_Unknown, Low_Power_Mode, High_Performance_Mode, On_Demand, Reserved };
		}
		//ACCEL1_CFG_0
		/// <summary>
		/// The configuration of the lower power mode in accel1
		/// </summary>
		public static class LowPowerMode
		{
			public static readonly SensorSetting Mode_Unknown = Sensor.UnknownSetting;
			public static readonly SensorSetting Low_Power_Mode_1 = new SensorSetting("Low-power Mode 1", 0, "", "Low-Power Mode 1 (12-bit resolution, Noise = 4.5mg(RMS))");
			public static readonly SensorSetting Low_Power_Mode_2 = new SensorSetting("Low-power Mode 2", 1, "", "Low-Power Mode 2 (14-bit resolution, Noise = 2.4mg(RMS))");
			public static readonly SensorSetting Low_Power_Mode_3 = new SensorSetting("Low-power Mode 3", 2, "", "Low-Power Mode 3 (14-bit resolution, Noise = 1.8mg(RMS))");
			public static readonly SensorSetting Low_Power_Mode_4 = new SensorSetting("Low-power Mode 4", 3, "", "Low-Power Mode 4 (14-bit resolution, Noise = 1.3mg(RMS))");
			public static readonly SensorSetting[] Settings = { Mode_Unknown, Low_Power_Mode_1, Low_Power_Mode_2, Low_Power_Mode_3, Low_Power_Mode_4 };
		}
		//ACCEL1_CFG_1
		/// <summary>
		/// The configuration of the high performance mode digital filter cut-off frequency / bandwidth in accel1
		/// </summary>
		public static class HighPerformanceBandwidthFilter
		{
			public static readonly SensorSetting Bandwidth_Unknown = Sensor.UnknownSetting;
			public static readonly SensorSetting Bandwidth_Filter_1 = new SensorSetting("Bandwidth Filter 1", 0, "");
			public static readonly SensorSetting Bandwidth_Filter_2 = new SensorSetting("Bandwidth Filter 2", 1, "");
			public static readonly SensorSetting Bandwidth_Filter_3 = new SensorSetting("Bandwidth Filter 3", 2, "");
			public static readonly SensorSetting Bandwidth_Filter_4 = new SensorSetting("Bandwidth Filter 4", 3, "");
			public static readonly SensorSetting[] Settings = { Bandwidth_Unknown, Bandwidth_Filter_1, Bandwidth_Filter_2, Bandwidth_Filter_3, Bandwidth_Filter_4 };
		}
		//ACCEL1_CFG_1
		/// <summary>
		/// The configuration of the low power mode digital filter cut-off frequency / bandwidth in accel1
		/// </summary>
		public static class LowPerformanceBandwidthFilter
		{
			public static readonly SensorSetting Bandwidth_Unknown = Sensor.UnknownSetting;
			public static readonly SensorSetting Bandwidth_Filter_1 = new SensorSetting("Bandwidth Filter 1", 0, "");
			public static readonly SensorSetting Bandwidth_Filter_2 = new SensorSetting("Bandwidth Filter 2", 1, "");
			public static readonly SensorSetting Bandwidth_Filter_3 = new SensorSetting("Bandwidth Filter 3", 2, "");
			public static readonly SensorSetting Bandwidth_Filter_4 = new SensorSetting("Bandwidth Filter 4", 3, "");
			public static readonly SensorSetting[] Settings = { Bandwidth_Unknown, Bandwidth_Filter_1, Bandwidth_Filter_2, Bandwidth_Filter_3, Bandwidth_Filter_4 };
		}
		//ACCEL1_CFG_3
		/// <summary>
		/// The configuration of the FIFO memory buffer in accel1
		/// </summary>
		public static class FMode
		{
			public static readonly SensorSetting FMode_Unknown = Sensor.UnknownSetting;
			public static readonly SensorSetting FMode_1 = new SensorSetting("Bypass Mode", 0, "", "FIFO turned off");
			public static readonly SensorSetting FMode_2 = new SensorSetting("FIFO Mode", 1, "", "Stops collecting data when FIFO is full");
			public static readonly SensorSetting FMode_3 = new SensorSetting("Continuous to FIFO", 3, "", "Stream mode until trigger is deasserted, then FIFO mode");
			public static readonly SensorSetting FMode_4 = new SensorSetting("Bypass to Continuous", 4, "", "Bypass mode until trigger is deasserted, then FIFO mode");
			public static readonly SensorSetting FMode_5 = new SensorSetting("Continuous", 6, "", "If the FIFO is full, the new sample overwrites the older sample");
			public static readonly SensorSetting[] Settings = { FMode_Unknown, FMode_1, FMode_2, FMode_3, FMode_4, FMode_5 };
		}

		/// <summary>
		/// Return the configuration of the digital filter cut-off frequency / bandwidth in accel1
		/// </summary>
		/// <returns></returns>
		public SensorSetting GetAccelBandwidthFilter()
		{
			return BWFilterSetting;
		}
		/// <summary>
		/// Set the configuration of the digital filter cut-off frequency / bandwidth in accel1
		/// </summary>
		/// <param name="bandwidth"><see cref="HighPerformanceBandwidthFilter"/> or <see cref="LowPerformanceBandwidthFilter"/></param>
		public void SetBandwidthFilter(SensorSetting bandwidth)
		{
			BWFilterSetting = bandwidth;
		}
		/// <summary>
		/// Return the configuration of the FIFO memory buffer in accel1
		/// </summary>
		/// <returns></returns>
		public SensorSetting GetAccelFMode()
		{
			return FModeSetting;
		}
		/// <summary>
		/// Set the configuration of the FIFO memory buffer in accel1
		/// </summary>
		/// <param name="fmode"><see cref="FMode"/></param>
		public void SetAccelFMode(SensorSetting fmode)
		{
			FModeSetting = fmode;
		}
		/// <summary>
		/// Returns the FIFO threshold level setting in accel1
		/// </summary>
		/// <returns></returns>
		public int GetAccelFIFOThreshold()
		{
			return FIFOThresholdSetting;
		}
		/// <summary>
		/// Set the FIFO threshold level setting in accel1
		/// </summary>
		/// <param name="threshold"></param>
		public void SetAccelFIFOThreshold(int threshold)
		{
			FIFOThresholdSetting = threshold;
		}
		/// <summary>
		/// Returns true if high pass data filter is enabled
		/// </summary>
		/// <returns></returns>
		public bool IsHighPassFilterEnabled()
		{
			return HighPassFilterEnabled;
		}
		/// <summary>
		/// Set whether a high or low pass data filter is selected.
		/// </summary>
		/// <param name="enable"></param>
		public void SetHighPassFilterEnabled(bool enable)
		{
			HighPassFilterEnabled = enable;
		}
		/// <summary>
		/// Returns true if low-noise configuration is enabled
		/// </summary>
		public bool IsLowNoiseEnabled()
		{
			return LowNoiseEnabled;
		}
		/// <summary>
		/// Enable or disable the low-noise configuration
		/// </summary>
		public void SetLowNoiseEnabled(bool enable)
		{
			LowNoiseEnabled = enable;
		}
		/// <summary>
		/// Returns true if high pass filter reference mode is enabled
		/// </summary>
		/// <returns></returns>
		public bool IsHighPassFilterRefModeEnabled()
		{
			return HighPassFilterRefModeEnabled;
		}
		/// <summary>
		/// Enable or disable the high pass filter reference mode
		/// </summary>
		/// <param name="enable"></param>
		public void SetHighPassFilterRefModeEnabled(bool enable)
		{
			HighPassFilterRefModeEnabled = enable;
		}
		/// <summary>
		/// Returns the configuration of the full-scale range in accel1
		/// </summary>
		/// <returns></returns>
		public SensorSetting GetAccelRange()
        {
			return RangeSetting;
        }
		/// <summary>
		/// Set the configuration of the full-scale range in accel1
		/// </summary>
		/// <param name="range"><see cref="AccelRange"/></param>
		public void SetAccelRange(SensorSetting range)
        {
			RangeSetting = range;
        }
		/// <summary>
		/// Returns the configuration of sampling rate in accel1
		/// </summary>
		public override SensorSetting GetSamplingRate()
        {
			return RateSetting;
        }
		/// <summary>
		/// Set the configuration of the mode in accel1
		/// </summary>
		/// <param name="mode"><see cref="Mode"/></param>
		public void SetMode(SensorSetting mode)
        {
			ModeSetting = mode;
        }
		/// <summary>
		/// Returns the configuration of the mode in accel1
		/// </summary>
		/// <returns></returns>
		public SensorSetting GetMode()
		{
			return ModeSetting;
		}
		/// <summary>
		/// Set the configuration of the lower power mode in accel1
		/// </summary>
		/// <param name="lpmode"><see cref="LowPowerMode"/></param>
		public void SetLPMode(SensorSetting lpmode)
        {
			LPModeSetting = lpmode;
        }
		/// <summary>
		/// Returns the configuration of the lower power mode in accel1
		/// </summary>
		/// <returns></returns>
		public SensorSetting GetLowPowerMode()
		{
			return LPModeSetting;
		}
		/// <summary>
		/// Set the configuration of sampling rate in accel1
		/// </summary>
		/// <param name="rate"><see cref="HighPerformanceAccelSamplingRate"/> or <see cref="LowPerformanceAccelSamplingRate"/></param>
		public override void SetSamplingRate(SensorSetting rate)
        {
			RateSetting = rate;
        }
		/*
		public static AccelRange GetValueFromName(string name)
		{
			var type = typeof(AccelRange);
			if (!type.IsEnum) throw new InvalidOperationException();

			foreach (var field in type.GetFields())
			{
				var attribute = Attribute.GetCustomAttribute(field,
					typeof(DisplayAttribute)) as DisplayAttribute;
				if (attribute != null)
				{
					if (attribute.Name == name)
					{
						return (AccelRange)field.GetValue(null);
					}
				}
				else
				{
					if (field.Name == name)
						return (AccelRange)field.GetValue(null);
				}
			}

			throw new ArgumentOutOfRangeException("name");
		}
		*/

		/// <summary>
		/// Create a new sensor accel1 and initialize the sensor configuration settings
		/// </summary>
		/// <param name="operationalConfigBytes"></param>
		public SensorLIS2DW12(byte[] operationalConfigBytes) : base()
		{
			InitializeUsingOperationConfig(operationalConfigBytes);

		}
		/// <summary>
		/// Turns on/off data collection from the primary accelerometer
		/// </summary>
		/// <param name="enable"></param>
		public void SetAccelEnabled(bool enable)
        {
			Enabled = enable;
        }
		/// <summary>
		/// Returns true if the data collection from the primary accelerometer is enabled
		/// </summary>
		/// <returns></returns>
		public bool IsAccelEnabled()
        {
			return Enabled;
        }
		/// <summary>
		/// Create a new sensor accel1
		/// </summary>
		public SensorLIS2DW12():base()
        {

		}
		/// <summary>
		/// Parse a single sample and store in the object cluster provided. This is typically used to parse all the samples within a payload. 
		/// </summary>
		/// <param name="ojc"></param>
		/// <param name="sample">one set of data</param>
		public override ObjectCluster ParseSensorData(byte[] sample, ObjectCluster ojc)
        {
			var accelx = BitConverter.ToInt16(sample, 0);
			var accely = BitConverter.ToInt16(sample, 2);
			var accelz = BitConverter.ToInt16(sample, 4);
			//System.Console.WriteLine("ACCEL 2 : " + accel_2_x + " " + accel_2_y + " " + accel_2_z);
			ojc.Add(ObjectClusterSensorName.LIS2DW12_ACC_X, ShimmerConfiguration.SignalFormats.RAW, ShimmerConfiguration.SignalUnits.NoUnits, accelx);
			ojc.Add(ObjectClusterSensorName.LIS2DW12_ACC_Y, ShimmerConfiguration.SignalFormats.RAW, ShimmerConfiguration.SignalUnits.NoUnits, accely);
			ojc.Add(ObjectClusterSensorName.LIS2DW12_ACC_Z, ShimmerConfiguration.SignalFormats.RAW, ShimmerConfiguration.SignalUnits.NoUnits, accelz);
			double[] datatemp = new double[3] {accelx, accely, accelz};
			double[,] SensitivityMatrix = DEFAULT_SENSITIVITY_MATRIX_LIS2DW12_2G;
			if (RangeSetting.GetConfigurationValue() == 0)
            {
				SensitivityMatrix = DEFAULT_SENSITIVITY_MATRIX_LIS2DW12_2G;
			} else if (RangeSetting.GetConfigurationValue() == 1)
            {
				SensitivityMatrix = DEFAULT_SENSITIVITY_MATRIX_LIS2DW12_4G;
			}
			else if (RangeSetting.GetConfigurationValue() == 2)
            {
				SensitivityMatrix = DEFAULT_SENSITIVITY_MATRIX_LIS2DW12_8G;
			}
			else if (RangeSetting.GetConfigurationValue() == 3)
            {
				SensitivityMatrix = DEFAULT_SENSITIVITY_MATRIX_LIS2DW12_16G;
			}
			double[] calData = UtilCalibration.CalibrateInertialSensorData(datatemp, DEFAULT_ALIGNMENT_MATRIX_LIS2DW12, SensitivityMatrix, DEFAULT_OFFSET_VECTOR_LIS2DW12);
			ojc.Add(ObjectClusterSensorName.LIS2DW12_ACC_X, ShimmerConfiguration.SignalFormats.CAL, ShimmerConfiguration.SignalUnits.MeterPerSecondSquared_DefaultCal, calData[0]);
			ojc.Add(ObjectClusterSensorName.LIS2DW12_ACC_Y, ShimmerConfiguration.SignalFormats.CAL, ShimmerConfiguration.SignalUnits.MeterPerSecondSquared_DefaultCal, calData[1]);
			ojc.Add(ObjectClusterSensorName.LIS2DW12_ACC_Z, ShimmerConfiguration.SignalFormats.CAL, ShimmerConfiguration.SignalUnits.MeterPerSecondSquared_DefaultCal, calData[2]);


			return ojc;
		}
		/// <summary>
		/// Update the operational configuration byte array based on current sensor configuration
		/// </summary>
		/// <param name="operationalConfigBytes">byte array to be update</param>
		/// <returns></returns>
		public override byte[] GenerateOperationConfig(byte[] operationalConfigBytes)
        {
            if (Enabled)
            {
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] | 0b10000000);
			} else
            {
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] & 0b01111111);
			}
			operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] = (byte)((operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] & 0b11001111) | (RangeSetting.GetConfigurationValue() << 4));
			operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] = (byte)((operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b00001111) | (RateSetting.GetConfigurationValue() << 4));
			if (!ModeSetting.Equals(Mode.Mode_Unknown))
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] = (byte)((operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b11110011) | (ModeSetting.GetConfigurationValue() << 2));
			}
			if (!LPModeSetting.Equals(LowPowerMode.Mode_Unknown))
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] = (byte)((operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] & 0b11111100) | (LPModeSetting.GetConfigurationValue()));
			}
			if (!FModeSetting.Equals(FMode.FMode_Unknown))
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_3] = (byte)((operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_3] & 0b00011111) | (FModeSetting.GetConfigurationValue() << 5));
			}
			if (LowNoiseEnabled)
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] | 0b00000100);
			}
			else
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] & 0b11111011);
			}
			if (HighPassFilterRefModeEnabled)
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_2] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_2] | 0b00000010);
			}
			else
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_2] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_2] & 0b11111101);
			}
			if (HighPassFilterEnabled)
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] | 0b00001000);
			}
			else
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] & 0b11110111);
			}

			operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] = (byte)((operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] & 0b00111111) | (BWFilterSetting.GetConfigurationValue() << 6));
			operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_3] = (byte)((operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_3] & 0b00011111) | (FModeSetting.GetConfigurationValue() << 5));

			operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_3] = (byte)((operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_3] & 011100000) | FIFOThresholdSetting);

			return operationalConfigBytes;
        }
		/// <summary>
		/// Parse the raw payload data received
		/// </summary>
		/// <param name="payload">the payload data that is received from the sensor</param>
		/// <param name="deviceID">the uuid for the address of which is used to connect to via BLE "00000000-0000-0000-0000-e7452c6d6f14" note that the uuid across OS (android vs iOS) can differ</param>
		/// <returns></returns>
		public override List<ObjectCluster> ParsePayloadData(byte[] payload, String deviceID)
        {
            var numberofBytesPerSample = 6;
			var numberofSamples = payload.Length / numberofBytesPerSample;
			List<ObjectCluster> listOfOJCs = new List<ObjectCluster>();
			for (int i = 0; i < numberofSamples; i++)
			{
				int startingIndex = numberofBytesPerSample * i;
				byte[] sampleData = new byte[numberofBytesPerSample];
				Array.Copy(payload, startingIndex, sampleData, 0, numberofBytesPerSample);
				ObjectCluster ojc = new ObjectCluster(deviceID, deviceID);

				//ojc.RawTimeStamp = (int)tick;

				listOfOJCs.Add(ParseSensorData(sampleData, ojc));
				//if (ShimmerBLEEvent != null)
					//ShimmerBLEEvent.Invoke(null, new ShimmerBLEEventData { ASMID = Asm_uuid.ToString(), CurrentEvent = VerisenseBLEEvent.NewDataPacket, ObjMsg = ojc });
			}
			return listOfOJCs;
		}
		/// <summary>
		/// Initialize the configuration settings using the operational config bytes provided
		/// </summary>
		/// <param name="operationalConfigBytes"></param>
		public override void InitializeUsingOperationConfig(byte[] operationalConfigBytes)
        {
			if ((operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] >>7) == 1)
			{
				Enabled = true;
			} else
            {
				Enabled = false;
            }
			if (((operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] >> 3) & 0b00000001) == 1)
			{
				HighPassFilterEnabled = true;
			}
			else
			{
				HighPassFilterEnabled = false;
			}
			if (((operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_2] >> 1) & 0b00000001) == 1)
			{
				HighPassFilterRefModeEnabled = true;
			}
			else
			{
				HighPassFilterRefModeEnabled = false;
			}
			if (((operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] >> 2) & 0b00000001) == 1)
			{
				LowNoiseEnabled = true;
			}
			else
			{
				LowNoiseEnabled = false;
			}
			int range = (operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] >> 4) & 0b00000011;
			RangeSetting = GetSensorSettingFromConfigurationValue(AccelRange.Settings, range);

			int mode = (operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] >> 2) & 0b00000011;
			int lpmode = (operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0]) & 0b00000011;
			LPModeSetting = GetSensorSettingFromConfigurationValue(LowPowerMode.Settings, lpmode);
			ModeSetting = GetSensorSettingFromConfigurationValue(Mode.Settings, mode);
			int rate = (operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_0] >> 4) & 0b00001111;
			if (ModeSetting.GetConfigurationValue() == Mode.Low_Power_Mode.GetConfigurationValue())
            {
				RateSetting = GetSensorSettingFromConfigurationValue(LowPerformanceAccelSamplingRate.Settings, rate);
			} else if (ModeSetting.GetConfigurationValue() == Mode.High_Performance_Mode.GetConfigurationValue())
            {
				RateSetting = GetSensorSettingFromConfigurationValue(HighPerformanceAccelSamplingRate.Settings, rate);
			}
			int bwfilter = operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_1] >> 6;
			if (ModeSetting.GetConfigurationValue() == Mode.Low_Power_Mode.GetConfigurationValue())
			{
				BWFilterSetting = GetSensorSettingFromConfigurationValue(LowPerformanceBandwidthFilter.Settings, bwfilter);
			}
			else if (ModeSetting.GetConfigurationValue() == Mode.High_Performance_Mode.GetConfigurationValue())
			{
				BWFilterSetting = GetSensorSettingFromConfigurationValue(HighPerformanceBandwidthFilter.Settings, bwfilter);
			}
			int fmode = (operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_3] >> 5);
			FModeSetting = GetSensorSettingFromConfigurationValue(FMode.Settings, fmode);

			FIFOThresholdSetting = (operationalConfigBytes[(int)ConfigurationBytesIndexName.ACCEL1_CFG_3]) & 0b00011111;
		}
		/// <summary>
		/// Returns the sensor name
		/// </summary>
		/// <returns></returns>
		public override string GetSensorName()
        {
			return SensorName;
        }
    }
}
