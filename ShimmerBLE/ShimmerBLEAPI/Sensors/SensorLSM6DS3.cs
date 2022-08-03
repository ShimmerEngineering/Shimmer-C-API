using shimmer.Helpers;
using ShimmerAPI;
using ShimmerAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using static shimmer.Models.OpConfigPayload;

namespace shimmer.Sensors
{
	/// <summary>
	/// This class contains the configuration settings for sensor accel2. Every sensor should belong to a device
	/// </summary>
	public class SensorLSM6DS3 : Sensor //ACCEL AND GYRO
	{
		/// <summary>
		/// Create a new sensor accel2
		/// </summary>
		public SensorLSM6DS3()
        {
			

		}
		//GEN_CFG_0
		protected bool Accel2_Enabled = false;
		protected bool Gyro_Enabled = false;
		//GEN_CFG_2
		protected bool StepCountEnabled;
		//GYRO_ACCEL2_CFG_0
		protected SensorSetting FIFOThresholdSetting = Sensor.UnknownSetting;
		//GYRO_ACCEL2_CFG_1
		protected bool StepCounterAndTimestampEnabled = false;
		protected bool WriteInFIFOAtEveryStepDetectedEnabled = false;
		//GYRO_ACCEL2_CFG_2
		protected SensorSetting GyroFIFODecimationSetting = Sensor.UnknownSetting;
		protected SensorSetting AccelFIFODecimationSetting = Sensor.UnknownSetting;
		//GYRO_ACCEL2_CFG_3
		protected SensorSetting FIFOOutputDataRateSetting = Sensor.UnknownSetting;
		protected SensorSetting FIFOModeSetting = Sensor.UnknownSetting;
		//GYRO_ACCEL2_CFG_4
		protected SensorSetting SamplingRateSetting = Sensor.UnknownSetting;
		protected SensorSetting AccelRangeSetting = Sensor.UnknownSetting;
		protected SensorSetting AccelAntiAliasingFilterBWSetting = Sensor.UnknownSetting;
		//GYRO_ACCEL2_CFG_5
		//protected SensorSetting GyroRateSetting = Sensor.UnknownSetting;
		protected SensorSetting GyroRangeSetting = Sensor.UnknownSetting;
		protected bool GyroFullScaleAt125Enabled = false;
		//GYRO_ACCEL2_CFG_6
		//protected bool HighPerformanceOpModeEnabled = false;
		protected bool HighPassFilterEnabled = false;
		protected bool DigitalHPFilterResetEnabled = false;
		protected bool SourceRegRoundingStatusEnabled = false;
		protected SensorSetting HPFilterCutOffFreqSetting = Sensor.UnknownSetting;
		//GYRO_ACCEL2_CFG_7
		protected bool AccelLowPassFilterEnabled = false;
		protected bool AccelSlopeOrHighPassFilterEnabled = false;
		protected bool AccelLowPassFilterOn6DEnabled = false;

		private static readonly int DEFAULT_FIFO_BYTE_SIZE_IN_CHIP = 8112;
		private static readonly int DEFAULT_MAX_FIFOS_IN_PAYLOAD = 4;
		private int fifoByteSizeInChip = DEFAULT_FIFO_BYTE_SIZE_IN_CHIP;
		private int fifoSizeInChip = DEFAULT_FIFO_BYTE_SIZE_IN_CHIP / 2;
		public static readonly string SensorName = "Accel2";
		protected int rangeAccel = LSM6DS3_ACCEL_RANGE_CONFIG_VALUES[1];
		protected int rangeGyro = LSM6DS3_GYRO_RANGE_CONFIG_VALUES[1];

		public static readonly int[] LSM6DS3_ACCEL_RANGE_CONFIG_VALUES={0,2,3,1};  // Config values order might change
		public static readonly int[] LSM6DS3_GYRO_RANGE_CONFIG_VALUES ={ 0,1,2,3};  // Config values order might change
		/// <summary>
		/// This is used to store and retrieve the sensor data in the object cluster
		/// </summary>
		public static class ObjectClusterSensorName
		{
			public static String LSM6DS3_ACC_X = SensorName + "_X";
			public static String LSM6DS3_ACC_Y = SensorName + "_Y";
			public static String LSM6DS3_ACC_Z = SensorName + "_Z";
			public static String LSM6DS3_GYRO_X = "Gyro_X";
			public static String LSM6DS3_GYRO_Y = "Gyro_Y";
			public static String LSM6DS3_GYRO_Z = "Gyro_Z";
		}
		/// <summary>
		/// The configuration of the FIFO threshold level setting in accel2
		/// </summary>
		public static class FIFOThreshold
        {
			public static readonly SensorSetting Threshold_Unknown = Sensor.UnknownSetting;
			public static readonly SensorSetting Threshold_1 = new SensorSetting("Threshold 1", 4056, 4056, "Recommended for 12.5Hz, 26Hz, 52Hz to save power");
			public static readonly SensorSetting Threshold_2 = new SensorSetting("Threshold 2", 2028, 2028, "Recommended for 104Hz to save power");
			public static readonly SensorSetting Threshold_3 = new SensorSetting("Threshold 3", 1014, 1014, "Recommended for 208Hz to save power");
			public static readonly SensorSetting Threshold_4 = new SensorSetting("Threshold 4", 540, 540, "Recommended for 416Hz to save power");
			public static readonly SensorSetting Threshold_5 = new SensorSetting("Threshold 5", 288, 288, "Recommended for 833Hz to save power");
			public static readonly SensorSetting Threshold_6 = new SensorSetting("Threshold 6", 150, 150, "Recommended for 1660Hz to save power");
			public static readonly SensorSetting[] Settings = { Threshold_Unknown, Threshold_1, Threshold_2, Threshold_3, Threshold_4, Threshold_5, Threshold_6 };
		}
		//GYRO_ACCEL2_CFG_2
		/// <summary>
		/// The configuration of the gyroscope FIFO decimation setting
		/// </summary>
		public static class GyroFIFODecimation
		{
			public static readonly SensorSetting Decimation_Unknown = Sensor.UnknownSetting;
			public static readonly SensorSetting Decimation_No_Sensor = new SensorSetting("Gyroscope sensor not in FIFO", 0, "");
			public static readonly SensorSetting Decimation_0 = new SensorSetting("No decimation", 1, "");
			public static readonly SensorSetting Decimation_2 = new SensorSetting("Decimation with factor 2", 2, "");
			public static readonly SensorSetting Decimation_3 = new SensorSetting("Decimation with factor 3", 3, "");
			public static readonly SensorSetting Decimation_4 = new SensorSetting("Decimation with factor 4", 4, "");
			public static readonly SensorSetting Decimation_8 = new SensorSetting("Decimation with factor 8", 5, "");
			public static readonly SensorSetting Decimation_16 = new SensorSetting("Decimation with factor 16", 6, "");
			public static readonly SensorSetting Decimation_32 = new SensorSetting("Decimation with factor 32", 7, "");
			public static readonly SensorSetting[] Settings = { Decimation_Unknown, Decimation_No_Sensor, Decimation_0, Decimation_2, Decimation_3, Decimation_4, Decimation_8, Decimation_16, Decimation_32 };
		}
		//GYRO_ACCEL2_CFG_2
		/// <summary>
		/// The configuration of the accelerometer FIFO decimation setting
		/// </summary>
		public static class AccelFIFODecimation
		{
			public static readonly SensorSetting Decimation_Unknown = Sensor.UnknownSetting;
			public static readonly SensorSetting Decimation_No_Sensor = new SensorSetting("Accelerometer sensor not in FIFO", 0, "");
			public static readonly SensorSetting Decimation_0 = new SensorSetting("No decimation", 1, "");
			public static readonly SensorSetting Decimation_2 = new SensorSetting("Decimation with factor 2", 2, "");
			public static readonly SensorSetting Decimation_3 = new SensorSetting("Decimation with factor 3", 3, "");
			public static readonly SensorSetting Decimation_4 = new SensorSetting("Decimation with factor 4", 4, "");
			public static readonly SensorSetting Decimation_8 = new SensorSetting("Decimation with factor 8", 5, "");
			public static readonly SensorSetting Decimation_16 = new SensorSetting("Decimation with factor 16", 6, "");
			public static readonly SensorSetting Decimation_32 = new SensorSetting("Decimation with factor 32", 7, "");
			public static readonly SensorSetting[] Settings = { Decimation_Unknown, Decimation_No_Sensor, Decimation_0, Decimation_2, Decimation_3, Decimation_4, Decimation_8, Decimation_16, Decimation_32 };
		}
		//GYRO_ACCEL2_CFG_3
		/// <summary>
		/// The configuration of the FIFO output data rate setting
		/// </summary>
		public static class FIFOOutputDataRate
		{
			public static readonly SensorSetting ODR_Unknown = Sensor.UnknownSetting;
			public static readonly SensorSetting ODR_Disabled = new SensorSetting("FIFO Disabled", 0, 0.0);
			public static readonly SensorSetting ODR_12_5 = new SensorSetting("12.5Hz", 1, 12.5);
			public static readonly SensorSetting ODR_26 = new SensorSetting("26.0Hz", 2, 26);
			public static readonly SensorSetting ODR_52 = new SensorSetting("52.0Hz", 3, 52);
			public static readonly SensorSetting ODR_104 = new SensorSetting("104.0Hz", 4, 104);
			public static readonly SensorSetting ODR_208 = new SensorSetting("208.0Hz", 5, 208);
			public static readonly SensorSetting ODR_416 = new SensorSetting("416.0Hz", 6, 416);
			public static readonly SensorSetting ODR_833 = new SensorSetting("833.0Hz", 7, 833);
			public static readonly SensorSetting ODR_1666 = new SensorSetting("1666.0Hz", 8, 1666);
			public static readonly SensorSetting[] Settings = { ODR_Unknown, ODR_Disabled, ODR_12_5, ODR_26, ODR_52, ODR_104, ODR_208, ODR_416, ODR_833, ODR_1666 };
		}
		//GYRO_ACCEL2_CFG_3
		/// <summary>
		/// The configuration of the FIFO mode setting
		/// </summary>
		public static class FIFOMode
		{
			public static readonly SensorSetting FIFOMode_Unknown = Sensor.UnknownSetting;
			public static readonly SensorSetting FIFOMode_1 = new SensorSetting("Bypass Mode", 0, "", "FIFO disabled");
			public static readonly SensorSetting FIFOMode_2 = new SensorSetting("FIFO Mode", 1, "", "Stops collecting data when FIFO is full");
			public static readonly SensorSetting FIFOMode_3 = new SensorSetting("Continuous to FIFO", 3, "", "Stream mode until trigger is deasserted, then FIFO mode");
			public static readonly SensorSetting FIFOMode_4 = new SensorSetting("Bypass to Continuous", 4, "", "Bypass mode until trigger is deasserted, then Continuous mode");
			public static readonly SensorSetting FIFOMode_5 = new SensorSetting("Continuous Mode", 6, "", "If the FIFO is full, the new sample overwrites the older sample");
			public static readonly SensorSetting[] Settings = { FIFOMode_Unknown, FIFOMode_1, FIFOMode_2, FIFOMode_3, FIFOMode_4, FIFOMode_5 };
		}
		//GYRO_ACCEL2_CFG_4
		/// <summary>
		/// The configuration of the sampling rate setting
		/// </summary>
		public static class SamplingRate
		{
			public static readonly SensorSetting Rate_Unknown = Sensor.UnknownSetting;
			public static readonly SensorSetting Power_Down = new SensorSetting("Power-down", 0, 0);
			public static readonly SensorSetting Freq_12_5Hz = new SensorSetting("12.5Hz", 1, 12.5);
			public static readonly SensorSetting Freq_26Hz = new SensorSetting("26.0Hz", 2, 26);
			public static readonly SensorSetting Freq_52Hz = new SensorSetting("52.0Hz", 3, 52);
			public static readonly SensorSetting Freq_104Hz = new SensorSetting("104.0Hz", 4, 104);
			public static readonly SensorSetting Freq_208Hz = new SensorSetting("208.0Hz", 5, 208);
			public static readonly SensorSetting Freq_416Hz = new SensorSetting("416.0Hz", 6, 416);
			public static readonly SensorSetting Freq_833Hz = new SensorSetting("833.0Hz", 7, 833);
			public static readonly SensorSetting Freq_1666Hz = new SensorSetting("1666.0Hz", 8, 1666);
			public static readonly SensorSetting[] Settings = { Rate_Unknown, Power_Down, Freq_12_5Hz, Freq_26Hz, Freq_52Hz, Freq_104Hz, Freq_208Hz, Freq_416Hz, Freq_833Hz, Freq_1666Hz };
		}
		//GYRO_ACCEL2_CFG_4
		/// <summary>
		/// The configuration of the full-scale range in accel2
		/// </summary>
		public static class AccelRange
		{

			public static readonly SensorSetting Range_Unknown = Sensor.UnknownSetting;
			public static readonly SensorSetting Range_2G = new SensorSetting("\u00B12G", 0, 2);
			public static readonly SensorSetting Range_4G = new SensorSetting("\u00B14G", 2, 4);
			public static readonly SensorSetting Range_8G = new SensorSetting("\u00B18G", 3, 8);
			public static readonly SensorSetting Range_16G = new SensorSetting("\u00B116G", 1, 16);
			public static readonly SensorSetting[] Settings = { Range_Unknown, Range_2G, Range_4G, Range_8G, Range_16G };
		}
		//GYRO_ACCEL2_CFG_4
		/// <summary>
		/// The configuration of the Anti-aliasing filter bandwidth in accel2
		/// </summary>
		public static class AccelAntiAliasingFilterBW
		{
			public static readonly SensorSetting Filter_BW_Unknown = Sensor.UnknownSetting;
			public static readonly SensorSetting Filter_BW_400 = new SensorSetting("400Hz", 0, 400);
			public static readonly SensorSetting Filter_BW_200 = new SensorSetting("200Hz", 1, 200);
			public static readonly SensorSetting Filter_BW_100 = new SensorSetting("100Hz", 2, 100);
			public static readonly SensorSetting Filter_BW_50 = new SensorSetting("50Hz", 3, 50);
			public static readonly SensorSetting[] Settings = { Filter_BW_Unknown, Filter_BW_400, Filter_BW_200, Filter_BW_100, Filter_BW_50 };
		}
		//GYRO_ACCEL2_CFG_5
		/*
		public static class GyroSamplingRate
		{
			public static readonly SensorSetting Rate_Unknown = Sensor.UnknownSetting;
			public static readonly SensorSetting Power_Down = new SensorSetting("Power-down", 0, 0);
			public static readonly SensorSetting Freq_12_5Hz = new SensorSetting("12.5Hz", 1, 12.5);
			public static readonly SensorSetting Freq_26Hz = new SensorSetting("26.0Hz", 2, 26);
			public static readonly SensorSetting Freq_52Hz = new SensorSetting("52.0Hz", 3, 52);
			public static readonly SensorSetting Freq_104Hz = new SensorSetting("104.0Hz", 4, 104);
			public static readonly SensorSetting Freq_208Hz = new SensorSetting("208.0Hz", 5, 208);
			public static readonly SensorSetting Freq_416Hz = new SensorSetting("416.0Hz", 6, 416);
			public static readonly SensorSetting Freq_833Hz = new SensorSetting("833.0Hz", 7, 833);
			public static readonly SensorSetting Freq_1666Hz = new SensorSetting("1666.0Hz", 8, 1666);
			public static readonly SensorSetting[] Settings = { Rate_Unknown, Power_Down, Freq_12_5Hz, Freq_26Hz, Freq_52Hz, Freq_104Hz, Freq_208Hz, Freq_416Hz, Freq_833Hz, Freq_1666Hz };
		}
		*/
		//GYRO_ACCEL2_CFG_5
		/// <summary>
		/// The configuration of the full-scale range in gyroscope
		/// </summary>
		public static class GyroRange
		{
			public static readonly SensorSetting Range_Unknown = Sensor.UnknownSetting;
			public static readonly SensorSetting Range_250dps = new SensorSetting("250dps", 0, 2);
			public static readonly SensorSetting Range_500dps = new SensorSetting("500dps", 1, 4);
			public static readonly SensorSetting Range_1000dps = new SensorSetting("1000dps", 2, 8);
			public static readonly SensorSetting Range_2000dps = new SensorSetting("2000dps", 3, 16);
			public static readonly SensorSetting[] Settings = { Range_Unknown, Range_250dps, Range_500dps, Range_1000dps, Range_2000dps };
		}
		//GYRO_ACCEL2_CFG_6
		/// <summary>
		/// The configuration of the Gyroscope high-pass filter cut-off frequency
		/// </summary>
		public static class HPFilterCutOffFrequency
		{
			public static readonly SensorSetting CutOff_Freq_Unknown = Sensor.UnknownSetting;
			public static readonly SensorSetting CutOff_Freq_0_0081 = new SensorSetting("0.0081Hz", 0, 0.0081);
			public static readonly SensorSetting CutOff_Freq_0_0324 = new SensorSetting("0.0324Hz", 1, 0.0324);
			public static readonly SensorSetting CutOff_Freq_2_07 = new SensorSetting("2.07Hz", 2, 2.07);
			public static readonly SensorSetting CutOff_Freq_16_32 = new SensorSetting("16.32Hz", 3, 16.32);
			public static readonly SensorSetting[] Settings = { CutOff_Freq_Unknown, CutOff_Freq_0_0081, CutOff_Freq_0_0324, CutOff_Freq_2_07, CutOff_Freq_16_32};
		}
		

		public static readonly double[,] DEFAULT_OFFSET_VECTOR_LSM6DS3 = { { 0 }, { 0 }, { 0 } };
		public static readonly double[,] DEFAULT_ALIGNMENT_MATRIX_LSM6DS3 = { { 0, 0, 1 }, { -1, 0, 0 }, { 0, -1, 0 } };

		public static readonly double[,] DEFAULT_SENSITIVITY_MATRIX_LSM6DS3_2G = { { 1671.665922915, 0, 0 }, { 0, 1671.665922915, 0 }, { 0, 0, 1671.665922915 } };
		public static readonly double[,] DEFAULT_SENSITIVITY_MATRIX_LSM6DS3_4G = { { 835.832961457, 0, 0 }, { 0, 835.832961457, 0 }, { 0, 0, 835.832961457 } };
		public static readonly double[,] DEFAULT_SENSITIVITY_MATRIX_LSM6DS3_8G = { { 417.916480729, 0, 0 }, { 0, 417.916480729, 0 }, { 0, 0, 417.916480729 } };
		public static readonly double[,] DEFAULT_SENSITIVITY_MATRIX_LSM6DS3_16G = { { 208.958240364, 0, 0 }, { 0, 208.958240364, 0 }, { 0, 0, 208.958240364 } };

		//	public static readonly double[,] DEFAULT_SENSITIVITY_MATRIX_LSM6DS3_125DPS = {{228.571428571,0,0},{0,228.571428571,0},{0,0,228.571428571}};
		public static readonly double[,] DEFAULT_SENSITIVITY_MATRIX_LSM6DS3_250DPS = { { 114.285714286, 0, 0 }, { 0, 114.285714286, 0 }, { 0, 0, 114.285714286 } };
		public static readonly double[,] DEFAULT_SENSITIVITY_MATRIX_LSM6DS3_500DPS = { { 57.142857143, 0, 0 }, { 0, 57.142857143, 0 }, { 0, 0, 57.142857143 } };
		public static readonly double[,] DEFAULT_SENSITIVITY_MATRIX_LSM6DS3_1000DPS = { { 28.571428571, 0, 0 }, { 0, 28.571428571, 0 }, { 0, 0, 28.571428571 } };
		public static readonly double[,] DEFAULT_SENSITIVITY_MATRIX_LSM6DS3_2000DPS = { { 14.285714286, 0, 0 }, { 0, 14.285714286, 0 }, { 0, 0, 14.285714286 } };

		/// <summary>
		/// Turns on/off the data collection from the secondary accelerometer
		/// </summary>
		/// <param name="enable"></param>
		public void SetAccelEnabled(bool enable)
        {
			Accel2_Enabled = enable;
        }

		/// <summary>
		/// Turns on/off data collection from the gyroscope
		/// </summary>
		/// <param name="enable"></param>
		public void SetGyroEnabled(bool enable)
        {
			Gyro_Enabled = enable;
        }

		/// <summary>
		/// Returns true if step count is enabled
		/// </summary>
		public bool IsStepCountEnabled()
		{
			return StepCountEnabled;
		}

		/// <summary>
		/// Enable or disable step count
		/// </summary>
		/// <param name="enabled"></param>
		public void SetStepCountEnabled(bool enabled)
		{
			StepCountEnabled = enabled;
		}

		/// <summary>
		/// Returns true if the data collection from the secondary accelerometer is enabled
		/// </summary>
		/// <returns></returns>
		public bool IsAccelEnabled()
        {
			return Accel2_Enabled;
        }

		/// <summary>
		/// Returns true if the data collection from the gyroscope is enabled
		/// </summary>
		/// <returns></returns>
		public bool IsGyroEnabled()
        {
			return Gyro_Enabled;
        }
		/// <summary>
		/// Returns true if the pedometer step counter and timestamp as 4th FIFO data set is enabled
		/// </summary>
		/// <returns></returns>
		public bool IsStepCounterAndTSDataEnabled()
		{
			return StepCounterAndTimestampEnabled;
		}
		/// <summary>
		/// Enable or disable pedometer step counter and timestamp as 4th FIFO data set
		/// </summary>
		/// <param name="enable"></param>
		public void SetStepCounterAndTSDataEnabled(bool enable)
		{
			StepCounterAndTimestampEnabled = enable;
		}
		/// <summary>
		/// Returns true if write in FIFO at every step detected by step counter is enabled
		/// </summary>
		/// <returns></returns>
		public bool IsWriteInFIFOAtEveryStepDetectedEnabled()
		{
			return WriteInFIFOAtEveryStepDetectedEnabled;
		}
		/// <summary>
		/// Enable or disable write in FIFO at every step detected by step counter
		/// </summary>
		/// <param name="enable">enable write in FIFO based on XL/Gyro data-ready when the value is false</param>
		public void SetWriteInFIFOAtEveryStepDetectedEnabled(bool enable)
		{
			WriteInFIFOAtEveryStepDetectedEnabled = enable;
		}
		/// <summary>
		/// Returns true if Gyro full scale at 125 dps is enabled
		/// </summary>
		/// <returns></returns>
		public bool IsGyroFullScaleAt125Enabled()
		{
			return GyroFullScaleAt125Enabled;
		}
		/// <summary>
		/// Enable or disable gyro full scale at 125 dps
		/// </summary>
		/// <param name="enable"></param>
		public void SetGyroFullScaleAt125Enabled(bool enable)
		{
			GyroFullScaleAt125Enabled = enable;
		}
		/// <summary>
		/// Returns the gyroscope FIFO decimation setting
		/// </summary>
		/// <returns></returns>
		public SensorSetting GetGyroFIFODecimationSetting()
		{
			return GyroFIFODecimationSetting;
		}
		/// <summary>
		/// Set the the gyroscope FIFO decimation setting
		/// </summary>
		/// <param name="decimation"><see cref="GyroFIFODecimation"/></param>
		public void SetGyroFIFODecimationSetting(SensorSetting decimation)
		{
			GyroFIFODecimationSetting = decimation;
		}
		/// <summary>
		/// Returns the accel2 FIFO decimation setting
		/// </summary>
		/// <returns></returns>
		public SensorSetting GetAccelFIFODecimationSetting()
		{
			return AccelFIFODecimationSetting;
		}
		/// <summary>
		/// Set the the accel2 FIFO decimation setting
		/// </summary>
		/// <param name="decimation"><see cref="AccelFIFODecimation"/></param>
		public void SetAccelFIFODecimationSetting(SensorSetting decimation)
		{
			AccelFIFODecimationSetting = decimation;
		}
		/// <summary>
		/// Returns the FIFO output data rate setting
		/// </summary>
		/// <returns></returns>
		public SensorSetting GetFIFOOutputDataRateSetting()
		{
			return FIFOOutputDataRateSetting;
		}
		/// <summary>
		/// Set the FIFO output data rate setting
		/// </summary>
		/// <param name="odr"><see cref="FIFOOutputDataRate"/></param>
		public void SetFIFOOutputDataRateSetting(SensorSetting odr)
		{
			FIFOOutputDataRateSetting = odr;
		}
		/// <summary>
		/// Returns the Anti-aliasing filter bandwidth setting of the accel2
		/// </summary>
		/// <returns></returns>
		public SensorSetting GetAccelAntiAliasingFilterBWSetting()
		{
			return AccelAntiAliasingFilterBWSetting;
		}
		/// <summary>
		/// Set the Anti-aliasing filter bandwidth setting of the accel2
		/// </summary>
		/// <param name="bandwidth"><see cref="AccelAntiAliasingFilterBW"/></param>
		public void SetAccelAntiAliasingFilterBWSetting(SensorSetting bandwidth)
		{
			AccelAntiAliasingFilterBWSetting = bandwidth;
		}
		/// <summary>
		/// Returns the FIFO mode setting
		/// </summary>
		/// <returns></returns>
		public SensorSetting GetFIFOModeSetting()
		{
			return FIFOModeSetting;
		}
		/// <summary>
		/// Set the FIFO mode setting
		/// </summary>
		/// <param name="mode"><see cref="FIFOMode"/></param>
		public void SetFIFOModeSetting(SensorSetting mode)
		{
			FIFOModeSetting = mode;
		}
		//public bool IsHighPerformanceOpModeEnabled()
		//{
		//	return HighPerformanceOpModeEnabled;
		//}
		//public void SetHighPerformanceOpModeEnabled(bool enable)
		//{
		//	HighPerformanceOpModeEnabled = enable;
		//}
		/// <summary>
		/// Returns true if Gyroscope digital high-pass filter is enabled
		/// </summary>
		/// <returns></returns>
		public bool IsHighPassFilterEnabled()
		{
			return HighPassFilterEnabled;
		}
		/// <summary>
		/// Enable or disable Gyroscope digital high-pass filter
		/// </summary>
		/// <param name="enable"></param>
		public void SetHighPassFilterEnabled(bool enable)
		{
			HighPassFilterEnabled = enable;
		}
		/// <summary>
		/// Returns Gyroscope high-pass filter cut-off frequency setting
		/// </summary>
		/// <returns></returns>
		public SensorSetting GetHPFilterCutOffFreqSetting()
		{
			return HPFilterCutOffFreqSetting;
		}
		/// <summary>
		/// Set the Gyroscope high-pass filter cut-off frequency
		/// </summary>
		/// <param name="cutoffFreq"><see cref="HPFilterCutOffFrequency"/></param>
		public void SetHPFilterCutOffFreqSetting(SensorSetting cutoffFreq)
		{
			HPFilterCutOffFreqSetting = cutoffFreq;
		}
		/// <summary>
		/// Returns true if Gyroscope digital HP filter reset is enabled
		/// </summary>
		/// <returns></returns>
		public bool IsDigitalHPFilterResetEnabled()
		{
			return DigitalHPFilterResetEnabled;
		}
		/// <summary>
		/// Enable or disable Gyroscope digital HP filter reset
		/// </summary>
		/// <param name="enable"></param>
		public void SetDigitalHPFilterResetEnabled(bool enable)
		{
			DigitalHPFilterResetEnabled = enable;
		}
		/// <summary>
		/// Returns true if source register rounding function is enabled
		/// </summary>
		/// <returns></returns>
		public bool IsSourceRegRoundingStatusEnabled()
		{
			return SourceRegRoundingStatusEnabled;
		}
		/// <summary>
		/// Enable or disable source register rounding function
		/// </summary>
		/// <param name="enable"></param>
		public void SetRoundingStatusEnabled(bool enable)
		{
			SourceRegRoundingStatusEnabled = enable;
		}
		/// <summary>
		/// Returns true if Accelerometer low-pass filter is enabled
		/// </summary>
		/// <returns></returns>
		public bool IsAccelLowPassFilterEnabled()
		{
			return AccelLowPassFilterEnabled;
		}
		/// <summary>
		/// Enable or disable Accelerometer low-pass filter
		/// </summary>
		/// <param name="enable"></param>
		public void SetAccelLowPassFilterEnabled(bool enable)
		{
			AccelLowPassFilterEnabled = enable;
		}
		/// <summary>
		/// Returns true if Accelerometer slope filter / high-pass filter is enabled
		/// </summary>
		/// <returns></returns>
		public bool IsAccelSlopeOrHighPassFilterEnabled()
		{
			return AccelSlopeOrHighPassFilterEnabled;
		}
		/// <summary>
		/// Enable or disable Accelerometer slope filter / high-pass filter
		/// </summary>
		/// <param name="enable"></param>
		public void SetAccelSlopeOrHighPassFilterEnabled(bool enable)
		{
			AccelSlopeOrHighPassFilterEnabled = enable;
		}
		/// <summary>
		/// Returns true if Accelerometer Low-pass filter on 6D function is enabled
		/// </summary>
		/// <returns></returns>
		public bool IsAccelLowPassFilterOn6DEnabled()
		{
			return AccelLowPassFilterOn6DEnabled;
		}
		/// <summary>
		/// Enable or disable Accelerometer Low-pass filter on 6D function
		/// </summary>
		/// <param name="enable"></param>
		public void SetAccelLowPassFilterOn6DEnabled(bool enable)
		{
			AccelLowPassFilterOn6DEnabled = enable;
		}
		/// <summary>
		/// Parse a single sample and store in the object cluster provided. This is typically used to parse all the samples within a payload. 
		/// </summary>
		/// <param name="ojc"></param>
		/// <param name="sample">one set of data</param>
		public override ObjectCluster ParseSensorData(byte[] sample, ObjectCluster ojc)
		{
			int accelStartOfIndex = 0;
			int gyroStartOfIndex = 0;
			if (Gyro_Enabled && Accel2_Enabled)
            {
				accelStartOfIndex = 6;
            }
			if (Accel2_Enabled)
			{
				var accelx = BitConverter.ToInt16(sample, accelStartOfIndex);
				var accely = BitConverter.ToInt16(sample, accelStartOfIndex+2);
				var accelz = BitConverter.ToInt16(sample, accelStartOfIndex+4);
				//System.Console.WriteLine("ACCEL 2 : " + accel_2_x + " " + accel_2_y + " " + accel_2_z);
				ojc.Add(ObjectClusterSensorName.LSM6DS3_ACC_X, ShimmerConfiguration.SignalFormats.RAW, ShimmerConfiguration.SignalUnits.NoUnits, accelx);
				ojc.Add(ObjectClusterSensorName.LSM6DS3_ACC_Y, ShimmerConfiguration.SignalFormats.RAW, ShimmerConfiguration.SignalUnits.NoUnits, accely);
				ojc.Add(ObjectClusterSensorName.LSM6DS3_ACC_Z, ShimmerConfiguration.SignalFormats.RAW, ShimmerConfiguration.SignalUnits.NoUnits, accelz);
				double[] datatemp = new double[3] { accelx, accely, accelz };
				double[,] SensitivityMatrix = DEFAULT_SENSITIVITY_MATRIX_LSM6DS3_2G;
				if (AccelRangeSetting.GetConfigurationValue() == AccelRange.Range_2G.GetConfigurationValue())
				{
					SensitivityMatrix = DEFAULT_SENSITIVITY_MATRIX_LSM6DS3_2G;
				}
				else if (AccelRangeSetting.GetConfigurationValue() == AccelRange.Range_4G.GetConfigurationValue())
				{
					SensitivityMatrix = DEFAULT_SENSITIVITY_MATRIX_LSM6DS3_4G;
				}
				else if (AccelRangeSetting.GetConfigurationValue() == AccelRange.Range_8G.GetConfigurationValue())
				{
					SensitivityMatrix = DEFAULT_SENSITIVITY_MATRIX_LSM6DS3_8G;
				}
				else if (AccelRangeSetting.GetConfigurationValue() == AccelRange.Range_16G.GetConfigurationValue())
				{
					SensitivityMatrix = DEFAULT_SENSITIVITY_MATRIX_LSM6DS3_16G;
				}
				double[] calData = UtilCalibration.CalibrateInertialSensorData(datatemp, DEFAULT_ALIGNMENT_MATRIX_LSM6DS3, SensitivityMatrix, DEFAULT_OFFSET_VECTOR_LSM6DS3);
				ojc.Add(ObjectClusterSensorName.LSM6DS3_ACC_X, ShimmerConfiguration.SignalFormats.CAL, ShimmerConfiguration.SignalUnits.MeterPerSecondSquared_DefaultCal, calData[0]);
				ojc.Add(ObjectClusterSensorName.LSM6DS3_ACC_Y, ShimmerConfiguration.SignalFormats.CAL, ShimmerConfiguration.SignalUnits.MeterPerSecondSquared_DefaultCal, calData[1]);
				ojc.Add(ObjectClusterSensorName.LSM6DS3_ACC_Z, ShimmerConfiguration.SignalFormats.CAL, ShimmerConfiguration.SignalUnits.MeterPerSecondSquared_DefaultCal, calData[2]);
			} 
			if (Gyro_Enabled)
            {
				var gyrox = BitConverter.ToInt16(sample, gyroStartOfIndex+0);
				var gyroy = BitConverter.ToInt16(sample, gyroStartOfIndex+2);
				var gyroz = BitConverter.ToInt16(sample, gyroStartOfIndex+4);
				//System.Console.WriteLine("ACCEL 2 : " + accel_2_x + " " + accel_2_y + " " + accel_2_z);
				ojc.Add(ObjectClusterSensorName.LSM6DS3_GYRO_X, ShimmerConfiguration.SignalFormats.RAW, ShimmerConfiguration.SignalUnits.NoUnits, gyrox);
				ojc.Add(ObjectClusterSensorName.LSM6DS3_GYRO_Y, ShimmerConfiguration.SignalFormats.RAW, ShimmerConfiguration.SignalUnits.NoUnits, gyroy);
				ojc.Add(ObjectClusterSensorName.LSM6DS3_GYRO_Z, ShimmerConfiguration.SignalFormats.RAW, ShimmerConfiguration.SignalUnits.NoUnits, gyroz);
				double[] datatemp = new double[3] { gyrox, gyroy, gyroz };
				double[,] SensitivityMatrix = DEFAULT_SENSITIVITY_MATRIX_LSM6DS3_250DPS;
				if (GyroRangeSetting.GetConfigurationValue() == GyroRange.Range_250dps.GetConfigurationValue())
				{
					SensitivityMatrix = DEFAULT_SENSITIVITY_MATRIX_LSM6DS3_250DPS;
				}
				else if (GyroRangeSetting.GetConfigurationValue() == GyroRange.Range_500dps.GetConfigurationValue())
				{
					SensitivityMatrix = DEFAULT_SENSITIVITY_MATRIX_LSM6DS3_500DPS;
				}
				else if (GyroRangeSetting.GetConfigurationValue() == GyroRange.Range_1000dps.GetConfigurationValue())
				{
					SensitivityMatrix = DEFAULT_SENSITIVITY_MATRIX_LSM6DS3_1000DPS;
				}
				else if (GyroRangeSetting.GetConfigurationValue() == GyroRange.Range_2000dps.GetConfigurationValue())
				{
					SensitivityMatrix = DEFAULT_SENSITIVITY_MATRIX_LSM6DS3_2000DPS;
				}
				double[] calData = UtilCalibration.CalibrateInertialSensorData(datatemp, DEFAULT_ALIGNMENT_MATRIX_LSM6DS3, SensitivityMatrix, DEFAULT_OFFSET_VECTOR_LSM6DS3);
				ojc.Add(ObjectClusterSensorName.LSM6DS3_GYRO_X, ShimmerConfiguration.SignalFormats.CAL, ShimmerConfiguration.SignalUnits.DegreePerSecond_DefaultCal, calData[0]);
				ojc.Add(ObjectClusterSensorName.LSM6DS3_GYRO_Y, ShimmerConfiguration.SignalFormats.CAL, ShimmerConfiguration.SignalUnits.DegreePerSecond_DefaultCal, calData[1]);
				ojc.Add(ObjectClusterSensorName.LSM6DS3_GYRO_Z, ShimmerConfiguration.SignalFormats.CAL, ShimmerConfiguration.SignalUnits.DegreePerSecond_DefaultCal, calData[2]);

			}
			return ojc;
		}

		/// <summary>
		/// Update the operational configuration byte array based on current sensor configuration
		/// </summary>
		/// <param name="operationalConfigBytes">byte array to be update</param>
		/// <returns></returns>
		public override byte[] GenerateOperationConfig(byte[] operationalConfigBytes)
        {
			operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_0] = (byte)(FIFOThresholdSetting.GetConfigurationValue() & 0b11111111); //216
			operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] = (byte)((operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] & 0b11110000) | (FIFOThresholdSetting.GetConfigurationValue() >> 8)); //15


			if (Accel2_Enabled)
            {
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] | 0b01000000);
				
				//operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] | 0b00010110);
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] = 0b00010110;
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] | 0b00001001);

            }
            else
            {
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] & 0b10111111);
			}

			//temp
			operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)((operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0b00001111) | (SamplingRateSetting.GetConfigurationValue() << 4));
			operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)((operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0b11110011) | (AccelRangeSetting.GetConfigurationValue() << 2));


			if (Gyro_Enabled)
            {
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] | 0b00100000);
			}
			else
            {
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] & 0b11011111);
			}

			if (StepCountEnabled)
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_2] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_2] | 0b00100000);
			}
			else
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_2] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_2] & 0b11011111);
			}

			//temp
			operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)((operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0b0001111) | (SamplingRateSetting.GetConfigurationValue() << 4));
			operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)((operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0b11110011) | (GyroRangeSetting.GetConfigurationValue() << 2));

			if (StepCounterAndTimestampEnabled)
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] | 0b10000000);
			}
			else
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] & 0b01111111);
			}
			if (WriteInFIFOAtEveryStepDetectedEnabled)
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] | 0b01000000);
			}
			else
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] & 0b10111111);
			}
			if (GyroFullScaleAt125Enabled)
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)((operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0b11111100) | 0b00000001);
			}
			else
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] & 0b11111100);
			}
			if (!GyroFIFODecimationSetting.Equals(GyroFIFODecimation.Decimation_Unknown))
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] = (byte)((operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] & 0b11000111) | (GyroFIFODecimationSetting.GetConfigurationValue() << 3));
			}
			if (!AccelFIFODecimationSetting.Equals(GyroFIFODecimation.Decimation_Unknown))
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] = (byte)((operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] & 0b11111000) | (AccelFIFODecimationSetting.GetConfigurationValue()));
			}
			if (!FIFOOutputDataRateSetting.Equals(FIFOOutputDataRate.ODR_Unknown))
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] = (byte)((operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] & 0b10000111) | (FIFOOutputDataRateSetting.GetConfigurationValue() << 3));
			}
			if (!FIFOModeSetting.Equals(FIFOMode.FIFOMode_Unknown))
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] = (byte)((operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] & 0b11111000) | (FIFOModeSetting.GetConfigurationValue()));
			}
			if (!AccelAntiAliasingFilterBWSetting.Equals(AccelAntiAliasingFilterBW.Filter_BW_Unknown))
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] = (byte)((operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] & 0b11111100) | (AccelAntiAliasingFilterBWSetting.GetConfigurationValue()));
			}
			//if (HighPerformanceOpModeEnabled)
			//{
			//	operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] & 0b01111111);
			//}
			//else
			//{
			//	operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] | 0b10000000);
			//}
			if (HighPassFilterEnabled)
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] | 0b10000000);
			}
			else
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] & 0b01111111);
			}
			if (!HPFilterCutOffFreqSetting.Equals(HPFilterCutOffFrequency.CutOff_Freq_Unknown))
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] = (byte)((operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] & 0b10011111) | (HPFilterCutOffFreqSetting.GetConfigurationValue() << 5));
			}
			if (DigitalHPFilterResetEnabled)
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] | 0b00010000);
			}
			else
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] & 0b11101111);
			}
			if (SourceRegRoundingStatusEnabled)
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] | 0b00000100);
			}
			else
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] & 0b11111011);
			}
			if (AccelLowPassFilterEnabled)
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_7] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_7] | 0b10000000);
			}
			else
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_7] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_7] & 0b01111111);
			}
			if (AccelSlopeOrHighPassFilterEnabled)
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_7] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_7] | 0b00000100);
			}
			else
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_7] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_7] & 0b11111011);
			}
			if (AccelLowPassFilterOn6DEnabled)
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_7] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_7] | 0b00000001);
			}
			else
			{
				operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_7] = (byte)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_7] & 0b11111110);
			}

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
			if (Gyro_Enabled && Accel2_Enabled)
            {
				numberofBytesPerSample = 12;
            }
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
		/// Returns the configuration of the full-scale range in accel2
		/// </summary>
		/// <returns></returns>
		public SensorSetting GetAccelRange()
        {
			return AccelRangeSetting;
        }

		/// <summary>
		/// Set the configuration of the full-scale range in accel2
		/// </summary>
		/// <param name="setting"><see cref="AccelRange"/></param>
		public void SetAccelRange(SensorSetting setting)
        {
			AccelRangeSetting = setting;
        }
		/// <summary>
		/// Returns the configuration of the full-scale range in gyroscope
		/// </summary>
		/// <returns></returns>
		public SensorSetting GetGyroRange()
		{
			return GyroRangeSetting;
		}
		/// <summary>
		/// Set the configuration of the full-scale range in gyroscope
		/// </summary>
		/// <param name="setting"><see cref="GyroRange"/></param>
		public void SetGyroRange(SensorSetting setting)
		{
			GyroRangeSetting = setting;
		}
		/// <summary>
		/// Returns the configuration of the sampling rate setting
		/// </summary>
		/// <returns></returns>
		public override SensorSetting GetSamplingRate()
		{
			return SamplingRateSetting;
		}
		/// <summary>
		/// Set the configuration of the sampling rate setting
		/// </summary>
		/// <param name="rate"><see cref="SamplingRate"/></param>
		public override void SetSamplingRate(SensorSetting rate)
		{
			SamplingRateSetting = rate;
		}
		/// <summary>
		/// Returns the configuration of the FIFO threshold level setting in accel2
		/// </summary>
		/// <returns></returns>
		public SensorSetting GetFIFOThreshold()
        {
			return FIFOThresholdSetting;
        }
		/// <summary>
		/// Set the configuration of the FIFO threshold level setting in accel2
		/// </summary>
		/// <param name="setting"><see cref="FIFOThreshold"/></param>
		public void SetFIFOThreshold(SensorSetting setting)
        {
			FIFOThresholdSetting = setting;
        }
		/// <summary>
		/// Initialize the configuration settings using the operational config bytes provided
		/// </summary>
		/// <param name="operationalConfigBytes"></param>
		public override void InitializeUsingOperationConfig(byte[] operationalConfigBytes)
        {
			int accelRange = (operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] >> 2) & 0b11;
			AccelRangeSetting = GetSensorSettingFromConfigurationValue(AccelRange.Settings,accelRange);
			int gyroRange = (operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] >> 2 ) & 0b11;
			GyroRangeSetting = GetSensorSettingFromConfigurationValue(GyroRange.Settings,gyroRange);
			int samplingRate = (operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4] >> 4) & 0b1111;
			SamplingRateSetting = GetSensorSettingFromConfigurationValue(SamplingRate.Settings, samplingRate);
			int gyroRate = (operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5] >> 4) & 0b1111;
			//As there can only be one sampling rate, we just assume the value is the same below
			//GyroRateSetting = GetSensorSettingFromConfigurationValue(GyroSamplingRate.Settings, gyroRate);

			int ftlsb = operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_0];
			int ftmsb = operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] & 0b1111;
			FIFOThresholdSetting = GetSensorSettingFromConfigurationValue(FIFOThreshold.Settings, ftlsb|(ftmsb<<8));

			if ((int)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] & 0b01000000 )> 0){
				Accel2_Enabled = true;
			} else
            {
				Accel2_Enabled = false;
            }
			if ((int)(operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] & 0b00100000) > 0)
			{
				Gyro_Enabled = true;
			}
			else
			{
				Gyro_Enabled = false;
			}
			if ((operationalConfigBytes[(int)ConfigurationBytesIndexName.GEN_CFG_2] & 0b00100000) > 5)
			{
				StepCountEnabled = true;
			}
			else
			{
				StepCountEnabled = false;
			}
			if (((operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] >> 7) & 0b00000001) == 1)
			{
				StepCounterAndTimestampEnabled = true;
			}
			else
			{
				StepCounterAndTimestampEnabled = false;
			}
			if (((operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_1] >> 6) & 0b00000001) == 1)
			{
				WriteInFIFOAtEveryStepDetectedEnabled = true;
			}
			else
			{
				WriteInFIFOAtEveryStepDetectedEnabled = false;
			}
			if (((operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_5]) & 0b00000001) == 1)
			{
				GyroFullScaleAt125Enabled = true;
			}
			else
			{
				GyroFullScaleAt125Enabled = false;
			}
			//if (((operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] >> 7) & 0b00000001) == 0)
			//{
			//	HighPerformanceOpModeEnabled = true;
			//}
			//else
			//{
			//	HighPerformanceOpModeEnabled = false;
			//}
			if (((operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] >> 7) & 0b00000001) == 1)
			{
				HighPassFilterEnabled = true;
			}
			else
			{
				HighPassFilterEnabled = false;
			}
			if (((operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] >> 4) & 0b00000001) == 1)
			{
				DigitalHPFilterResetEnabled = true;
			}
			else
			{
				DigitalHPFilterResetEnabled = false;
			}
			if (((operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] >> 2) & 0b00000001) == 1)
			{
				SourceRegRoundingStatusEnabled = true;
			}
			else
			{
				SourceRegRoundingStatusEnabled = false;
			}
			if (((operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_7] >> 7) & 0b00000001) == 1)
			{
				AccelLowPassFilterEnabled = true;
			}
			else
			{
				AccelLowPassFilterEnabled = false;
			}
			if (((operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_7] >> 2) & 0b00000001) == 1)
			{
				AccelSlopeOrHighPassFilterEnabled = true;
			}
			else
			{
				AccelSlopeOrHighPassFilterEnabled = false;
			}
			if (((operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_7]) & 0b00000001) == 1)
			{
				AccelLowPassFilterOn6DEnabled = true;
			}
			else
			{
				AccelLowPassFilterOn6DEnabled = false;
			}

			//operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] = (byte)((operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] & 0b11001111) | (HPFilterCutOffFreqSetting.GetConfigurationValue() << 4));

			int gyroFIFODecimation = (operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2] >> 3) & 0b00000111;
			GyroFIFODecimationSetting = GetSensorSettingFromConfigurationValue(GyroFIFODecimation.Settings, gyroFIFODecimation);

			int accelFIFODecimation = (operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_2]) & 0b00000111;
			AccelFIFODecimationSetting = GetSensorSettingFromConfigurationValue(AccelFIFODecimation.Settings, accelFIFODecimation);

			int fifoODR = (operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3] >> 3) & 0b00001111;
			FIFOOutputDataRateSetting = GetSensorSettingFromConfigurationValue(FIFOOutputDataRate.Settings, fifoODR);

			int fifoMode = (operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_3]) & 0b00000111;
			FIFOModeSetting = GetSensorSettingFromConfigurationValue(FIFOMode.Settings, fifoMode);
			
			int accelAntiAliasingBW = (operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_4]) & 0b00000011;
			AccelAntiAliasingFilterBWSetting = GetSensorSettingFromConfigurationValue(AccelAntiAliasingFilterBW.Settings, accelAntiAliasingBW);

			int hpFilterCutOffFreq = (operationalConfigBytes[(int)ConfigurationBytesIndexName.GYRO_ACCEL2_CFG_6] >> 5) & 0b00000011;
			HPFilterCutOffFreqSetting = GetSensorSettingFromConfigurationValue(HPFilterCutOffFrequency.Settings, hpFilterCutOffFreq);
		}
		/// <summary>
		/// Returns the sensor name
		/// </summary>
		public override string GetSensorName()
        {
			return SensorName;
        }

    }
}
