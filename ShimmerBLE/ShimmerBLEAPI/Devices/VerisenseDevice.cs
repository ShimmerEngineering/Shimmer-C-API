﻿using Newtonsoft.Json;
using shimmer.Helpers;
using shimmer.Models;
using shimmer.Sensors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using static shimmer.Models.OpConfigPayload;

namespace ShimmerBLEAPI.Devices
{
    [Serializable]
    public abstract class VerisenseDevice
    {
        public static readonly DeviceByteSetting Unknown_Device_Setting = new DeviceByteSetting("Unknown", -1, "Unknown");

        public class DefaultVerisenseConfiguration
        {
            public static readonly DeviceByteArraySettings Unknown_Device_OpConfig_Setting = new DeviceByteArraySettings("Unknown", null, "Unknown");
            public static readonly DeviceByteArraySettings Accel1_Default_Setting = new DeviceByteArraySettings("ACCEL 1",new byte[] {0x5A,0x97,0x00,0x00,0x00,0x30,0x30,0x00,0x7F,0x00,0xD8,0x0F,0x09,0x16,0x24,0x0C,0x80,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x03,0xF4,0x18,0x3C,0x00,0x0A,0x0F,0x00,0x18,0x3C,0x00,0x0A,0x0F,0x00,0x18,0x3C,0x00,0x0A,0x0F,0x00,0xFF,0xFF,0xAA,0x01,0x03,0x3C,0x00,0x0E,0x00,0x00,0x63,0x28,0xCC,0xCC,0x1E,0x00,0x0A,0x00,0x00,0x00,0x00,0x01}, "SensorLIS2DW12 - 16G, 25Hz, Using Low Power, Lower Power Mode 1");
            public static readonly DeviceByteArraySettings Accel2_Gyro_Default_Setting = new DeviceByteArraySettings("ACCEL 2 and Gyro", new byte[] {0x5A,0x77,0x00,0x00,0x00,0x30,0x20,0x00,0x7F,0x00,0xD8,0x0F,0x09,0x16,0x24,0x2C,0x80,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x03,0xF4,0x18,0x3C,0x00,0x0A,0x0F,0x00,0x18,0x3C,0x00,0x0A,0x0F,0x00,0x18,0x3C,0x00,0x0A,0x0F,0x00,0xFF,0xFF,0xAA,0x01,0x03,0x3C,0x00,0x0E,0x00,0x00,0x63,0x28,0xCC,0xCC,0x1E,0x00,0x0A,0x00,0x00,0x00,0x00,0x01}, "SensorLSM6DS3 - Accel 16G, Gyro 2000deg/s, 26Hz, Threshold 1");
            public static readonly DeviceByteArraySettings GSR_Batt_Accel1_Default_Setting = new DeviceByteArraySettings("GSR, Battery and Accel 1", new byte[] { 0x5A,0x97,0x80,0x02,0x00,0x30,0x20,0x00,0x7F,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x80,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x03,0xF4,0x18,0x3C,0x00,0x0A,0x0F,0x00,0x18,0x3C,0x00,0x0A,0x0F,0x00,0x18,0x3C,0x00,0x0A,0x0F,0x00,0x17,0x04,0xFF,0xFF,0xFF,0x3C,0x00,0x0E,0x00,0x00,0x63,0x28,0xCC,0xCC,0x1E,0x00,0x0A,0x00,0x00,0x00,0x00,0x01}, "GSR and Battery, 51.2Hz, GSR Auto Range. Accel (SensorLIS2DW12) at 8G, 25Hz, Using Low Power, Lower Power Mode 1");
            public static readonly DeviceByteArraySettings GSR_Batt_Default_Setting = new DeviceByteArraySettings("GSR and Battery", new byte[] { 0x5A, 0x17, 0x80, 0x02, 0x00, 0x30, 0x20, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0xF4, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0x17, 0x04, 0xFF, 0xFF, 0xFF, 0x3C, 0x00, 0x0E, 0x00, 0x00, 0x63, 0x28, 0xCC, 0xCC, 0x1E, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x00, 0x01 }, "GSR and Battery, 51.2Hz, GSR Auto Range.");
            public static readonly DeviceByteArraySettings PPG_Default_Setting = new DeviceByteArraySettings("PPG", new byte[] { 0x5A, 0x17, 0x74, 0x00, 0x00, 0x00, 0x20, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0xF4, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0x18, 0x3C, 0x00, 0x0A, 0x0F, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x63, 0x28, 0xCC, 0xCC, 0x1E, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x00, 0x01 }, "Red Green IR Blue Leds enabled, 50 Hz, Sample Average = 1, Range 4, 420\u00B5s Pulse Width");
            public static readonly DeviceByteArraySettings[] Settings = new DeviceByteArraySettings[]{ Unknown_Device_OpConfig_Setting, Accel1_Default_Setting, Accel2_Gyro_Default_Setting, GSR_Batt_Accel1_Default_Setting, GSR_Batt_Default_Setting, PPG_Default_Setting };
        }

        public enum HardwareIdentifier
        {
            UNKNOWN = 00,
            VERISENSE_IMU_01 = 01,
            VERISENSE_IMU = 61,
            VERISENSE_GSR_PLUS = 62,
            VERISENSE_PPG = 63,
            VERISENSE_DEV_BOARD = 64,
            VERISENSE_PULSE_PLUS = 68
        }

        public static class BT5RadioOutputPower
        {
            public static readonly DeviceByteSetting Power_Unknown = new DeviceByteSetting("Unknown", -1, -1);
            public static readonly DeviceByteSetting Power_1 = new DeviceByteSetting("+8dBm", 8, 8);
            public static readonly DeviceByteSetting Power_2 = new DeviceByteSetting("+7dBm", 7, 8);
            public static readonly DeviceByteSetting Power_3 = new DeviceByteSetting("+6dBm", 6, 8);
            public static readonly DeviceByteSetting Power_4 = new DeviceByteSetting("+5dBm", 5, 8);
            public static readonly DeviceByteSetting Power_5 = new DeviceByteSetting("+4dBm", 4, 8);
            public static readonly DeviceByteSetting Power_6 = new DeviceByteSetting("+3dBm", 3, 8);
            public static readonly DeviceByteSetting Power_7 = new DeviceByteSetting("+2dBm", 2, 8);
            public static readonly DeviceByteSetting Power_8 = new DeviceByteSetting("0dBm", 0, 8);
            public static readonly DeviceByteSetting Power_9 = new DeviceByteSetting("-4dBm", 252, 8);
            public static readonly DeviceByteSetting Power_10 = new DeviceByteSetting("-8dBm", 248, 8);
            public static readonly DeviceByteSetting Power_11 = new DeviceByteSetting("-12dBm", 244, 8);
            public static readonly DeviceByteSetting Power_12 = new DeviceByteSetting("-16dBm", 240, 8);
            public static readonly DeviceByteSetting Power_13 = new DeviceByteSetting("-20dBm", 236, 8);
            public static readonly DeviceByteSetting Power_14 = new DeviceByteSetting("-40dBm", 255, 8);
            public static readonly DeviceByteSetting Power_15 = new DeviceByteSetting("-40dBm", 216, 8);
            public static readonly DeviceByteSetting[] Settings = { Power_Unknown, Power_1, Power_2, Power_3, Power_4, Power_5, Power_6, Power_7, Power_8, Power_9, Power_10, Power_11, Power_12, Power_13, Power_14, Power_15, };
        }

        protected StatusPayload Status { get; set; }
        protected ProdConfigPayload ProdConfig { get; set; }
        protected TimePayload Time { get; set; }
        protected OpConfigPayload OpConfig { get; set; }
        protected BasePayload WriteResponse { get; set; }
        protected PendingEventsPayload PendingEvents { get; set; }

        public Guid Asm_uuid { get; set; }
        protected string ASMName { get; set; }
        protected Dictionary<int, Sensor> SensorList = new Dictionary<int, Sensor>();


        protected void CreateSensorMap()
        {
            //ADC	1
            //ACCEL_1 2
            //GYRO_ACCEL2 3
            //PPG 4
            //BioZ    5
            SensorList.Add(1, new SensorGSR());
            SensorList.Add(2, new SensorLIS2DW12());
            SensorList.Add(3, new SensorLSM6DS3());
            SensorList.Add(4, new SensorPPG());

        }

        /// <summary>
        /// To clone a device
        /// </summary>
        /// <param name="device"></param>
        public VerisenseDevice(VerisenseDevice device)
        {
            OpConfig = new OpConfigPayload();
            OpConfig.ConfigurationBytes = new byte[device.OpConfig.ConfigurationBytes.Length];
            Array.Copy(device.OpConfig.ConfigurationBytes, OpConfig.ConfigurationBytes, device.OpConfig.ConfigurationBytes.Length); //deep copy
            CreateSensorMap();
            UpdateDeviceAndSensorConfiguration();
        }

        public VerisenseDevice()
        {
            CreateSensorMap();
        }

        /// <summary>
        /// This is to maintain the device level settings
        /// </summary>
        /// <param name="deviceOpConfig"></param>
        /// <param name="opConfigToWrite"></param>
        /// <returns></returns>
        protected byte[] UpdateDefaultDeviceConfigBytes(OpConfigPayload deviceOpConfig, byte[] opConfigToWrite)
        {
            //OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] = DefaultConfigBytes.ConfigurationBytes[(int)ConfigurationBytesIndexName.GEN_CFG_0];
            opConfigToWrite[(int)ConfigurationBytesIndexName.START_TIME] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.START_TIME];
            opConfigToWrite[(int)ConfigurationBytesIndexName.START_TIME + 1] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.START_TIME + 1];
            opConfigToWrite[(int)ConfigurationBytesIndexName.START_TIME + 2] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.START_TIME +2];
            opConfigToWrite[(int)ConfigurationBytesIndexName.START_TIME + 3] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.START_TIME + 3];
            opConfigToWrite[(int)ConfigurationBytesIndexName.END_TIME] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.END_TIME];
            opConfigToWrite[(int)ConfigurationBytesIndexName.END_TIME + 1] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.END_TIME + 1];
            opConfigToWrite[(int)ConfigurationBytesIndexName.END_TIME + 2] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.END_TIME + 2];
            opConfigToWrite[(int)ConfigurationBytesIndexName.END_TIME + 3] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.END_TIME + 3];
            opConfigToWrite[(int)ConfigurationBytesIndexName.BLE_RETRY_COUNT] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_RETRY_COUNT];
            opConfigToWrite[(int)ConfigurationBytesIndexName.BLE_TX_POWER] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_TX_POWER];
            opConfigToWrite[(int)ConfigurationBytesIndexName.BLE_DATA_TRANS_WKUP_INT_HRS] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_DATA_TRANS_WKUP_INT_HRS];
            opConfigToWrite[(int)ConfigurationBytesIndexName.BLE_DATA_TRANS_WKUP_TIME] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_DATA_TRANS_WKUP_TIME];
            opConfigToWrite[(int)ConfigurationBytesIndexName.BLE_DATA_TRANS_WKUP_TIME + 1] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_DATA_TRANS_WKUP_TIME + 1];
            opConfigToWrite[(int)ConfigurationBytesIndexName.BLE_DATA_TRANS_WKUP_DUR] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_DATA_TRANS_WKUP_DUR];
            opConfigToWrite[(int)ConfigurationBytesIndexName.BLE_DATA_TRANS_RETRY_INT] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_DATA_TRANS_RETRY_INT];
            opConfigToWrite[(int)ConfigurationBytesIndexName.BLE_STATUS_WKUP_INT_HRS] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_STATUS_WKUP_INT_HRS];
            opConfigToWrite[(int)ConfigurationBytesIndexName.BLE_STATUS_WKUP_TIME] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_STATUS_WKUP_TIME];
            opConfigToWrite[(int)ConfigurationBytesIndexName.BLE_STATUS_WKUP_TIME + 1] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_STATUS_WKUP_TIME + 1];
            opConfigToWrite[(int)ConfigurationBytesIndexName.BLE_STATUS_WKUP_DUR] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_STATUS_WKUP_DUR];
            opConfigToWrite[(int)ConfigurationBytesIndexName.BLE_STATUS_RETRY_INT] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_STATUS_RETRY_INT];
            opConfigToWrite[(int)ConfigurationBytesIndexName.BLE_RTC_SYNC_WKUP_INT_HRS] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_RTC_SYNC_WKUP_INT_HRS];
            opConfigToWrite[(int)ConfigurationBytesIndexName.BLE_RTC_SYNC_WKUP_TIME] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_RTC_SYNC_WKUP_TIME];
            opConfigToWrite[(int)ConfigurationBytesIndexName.BLE_RTC_SYNC_WKUP_TIME + 1] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_RTC_SYNC_WKUP_TIME + 1];
            opConfigToWrite[(int)ConfigurationBytesIndexName.BLE_RTC_SYNC_WKUP_DUR] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_RTC_SYNC_WKUP_DUR];
            opConfigToWrite[(int)ConfigurationBytesIndexName.BLE_RTC_SYNC_RETRY_INT] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_RTC_SYNC_RETRY_INT];
            opConfigToWrite[(int)ConfigurationBytesIndexName.ADAPTIVE_SCHEDULER_INT] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.ADAPTIVE_SCHEDULER_INT];
            opConfigToWrite[(int)ConfigurationBytesIndexName.ADAPTIVE_SCHEDULER_INT + 1] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.ADAPTIVE_SCHEDULER_INT + 1];
            opConfigToWrite[(int)ConfigurationBytesIndexName.ADAPTIVE_SCHEDULER_FAILCOUNT_MAX] = deviceOpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.ADAPTIVE_SCHEDULER_FAILCOUNT_MAX];

            return opConfigToWrite;
        }

        public void setPasskeyEnabled(bool enabled)
        {
            if(enabled)
            {
                OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.GEN_CFG_2] = (byte)(OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.GEN_CFG_2] & 0b11111011);
            }
            else
            {
                OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.GEN_CFG_2] = (byte)((OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.GEN_CFG_2] | 0b00000100) & 0b11110111);
            }
        }

        public void setLoggingEnabled(bool enabled)
        {
            if (enabled)
            {
                OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] = (byte)(OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] | 0b00000001);
            } else
            {
                OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] = (byte)(OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] & 0b11111110);
            }
        }

        public void setDeviceEnabled(bool enabled)
        {
            if (enabled)
            {
                OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] = (byte)(OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] | 0b00000010);
            }
            else
            {
                OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] = (byte)(OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] & 0b11111101);
            }
        }

        // MINUTES SINCE 1 JAN 1970
        /// <summary>
        /// The clock system on a Verisense sensor is in local time (e.g. unix time expressed in your time zone, e.g. for Kuala Lumpur unixtime + 08:00) 
        /// </summary>
        /// <param name="time">The local time</param>
        public void SetStartTimeInMinutes(long time)
        {
            byte[] minutesPassed = new byte[4];
            minutesPassed = BitConverter.GetBytes(time);
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.START_TIME] = minutesPassed[0];
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.START_TIME + 1] = minutesPassed[1];
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.START_TIME + 2] = minutesPassed[2];
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.START_TIME + 3] = minutesPassed[3];
        }

        public int GetStartTimeinMinutes()
        {
            byte[] minutesPassed = new byte[4];
            Array.Copy(OpConfig.ConfigurationBytes, (int)ConfigurationBytesIndexName.START_TIME, minutesPassed, 0, 4);
            int time = BitConverter.ToInt32(minutesPassed, 0);
            return time;
        }

        public void SetEndTimeInMinutes(long time)
        {
            byte[] minutesPassed = new byte[4];
            minutesPassed = BitConverter.GetBytes(time);
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.END_TIME] = minutesPassed[0];
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.END_TIME + 1] = minutesPassed[1];
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.END_TIME + 2] = minutesPassed[2];
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.END_TIME + 3] = minutesPassed[3];
        }

        public int GetEndTimeinMinutes()
        {
            byte[] minutesPassed = new byte[4];
            Array.Copy(OpConfig.ConfigurationBytes, (int)ConfigurationBytesIndexName.END_TIME, minutesPassed, 0, 4);
            int time = BitConverter.ToInt32(minutesPassed, 0);
            return time;
        }

        public DateTime ConvertUnixTSInMinuteIntoDateTime(int unixTimeStampInMinute)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTimeStampInMinute*60);
            return dateTimeOffset.DateTime;
        }

        public long ConvertDateTimeIntoUnixTSInMinute(DateTime datetime)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, datetime.Kind);
            var unixTimestamp = System.Convert.ToInt64((datetime - date).TotalMinutes);
            return unixTimestamp;
        }

        public void SetBLERetryCount(int count)
        {
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_RETRY_COUNT] = (byte)count;
        }

        public int GetBLERetryCount()
        {
            return OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_RETRY_COUNT];
        }

        //NOT SURE
        public void SetBLETXPower(int power)
        {
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_TX_POWER] = (byte)power;
        }

        public DeviceByteSetting GetBLETXPower()
        {
            int power = OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_TX_POWER];
            DeviceByteSetting powerSetting = GetDeviceSettingFromConfigurationValue(BT5RadioOutputPower.Settings, power);
            return powerSetting;
        }

        public void SetDataTransferInterval(int interval)
        {
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_DATA_TRANS_WKUP_INT_HRS] = (byte)interval;
        }

        public int GetDataTransferInterval()
        {
            return OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_DATA_TRANS_WKUP_INT_HRS];
        }

        public void SetDataTransferStartTime(int startTime)
        {
            byte[] minutesUntilStart = new byte[2];
            minutesUntilStart = BitConverter.GetBytes(startTime);
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_DATA_TRANS_WKUP_TIME] = minutesUntilStart[0];
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_DATA_TRANS_WKUP_TIME + 1] = minutesUntilStart[1];
        }

        public int GetDataTransferStartTime()
        {
            byte[] minutesUntilStart = new byte[4];
            Array.Copy(OpConfig.ConfigurationBytes, (int)ConfigurationBytesIndexName.BLE_DATA_TRANS_WKUP_TIME, minutesUntilStart, 0, 2);
            int time = BitConverter.ToInt32(minutesUntilStart, 0);
            return time;
        }
        public void SetDataTransferDuration(int duration)
        {
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_DATA_TRANS_WKUP_DUR] = (byte)duration;
        }

        public int GetDataTransferDuration()
        {
            return OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_DATA_TRANS_WKUP_DUR];
        }

        public void SetDataTransferRetryInterval(int interval)
        {
            byte[] retryInterval = new byte[2];
            retryInterval = BitConverter.GetBytes(interval);
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_DATA_TRANS_RETRY_INT] = retryInterval[0];
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_DATA_TRANS_RETRY_INT + 1] = retryInterval[1];
        }

        public int GetDataTransferRetryInterval()
        {
            byte[] retryInterval = new byte[4];
            Array.Copy(OpConfig.ConfigurationBytes, (int)ConfigurationBytesIndexName.BLE_DATA_TRANS_RETRY_INT, retryInterval, 0, 2);
            int time = BitConverter.ToInt32(retryInterval, 0);
            return time;
        }

        public void SetStatusInterval(int interval)
        {
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_STATUS_WKUP_INT_HRS] = (byte)interval;
        }

        public int GetStatusInterval()
        {
            return OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_STATUS_WKUP_INT_HRS];
        }

        public void SetStatusStartTime(int time)
        {
            byte[] startTime = new byte[2];
            startTime = BitConverter.GetBytes(time);
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_STATUS_WKUP_TIME] = startTime[0];
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_STATUS_WKUP_TIME + 1] = startTime[1];
        }

        public int GetStatusStartTime()
        {
            byte[] startTime = new byte[4];
            Array.Copy(OpConfig.ConfigurationBytes, (int)ConfigurationBytesIndexName.BLE_STATUS_WKUP_TIME, startTime, 0, 2);
            int time = BitConverter.ToInt32(startTime, 0);
            return time;
        }

        public void SetStatusDuration(int interval)
        {
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_STATUS_WKUP_DUR] = (byte)interval;
        }

        public int GetStatusDuration()
        {
            return OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_STATUS_WKUP_DUR];
        }

        public void SetStatusRetryInterval(int interval)
        {
            byte[] retryInterval = new byte[2];
            retryInterval = BitConverter.GetBytes(interval);
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_STATUS_RETRY_INT] = retryInterval[0];
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_STATUS_RETRY_INT + 1] = retryInterval[1];
        }

        public int GetStatusRetryInterval()
        {
            byte[] retryInterval = new byte[4];
            Array.Copy(OpConfig.ConfigurationBytes, (int)ConfigurationBytesIndexName.BLE_STATUS_RETRY_INT, retryInterval, 0, 2);
            int time = BitConverter.ToInt32(retryInterval, 0);
            return time;
        }

        public void SetRTCSyncInterval(int interval)
        {
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_RTC_SYNC_WKUP_INT_HRS] = (byte)interval;
        }

        public int GetRTCSyncInterval()
        {
            return OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_RTC_SYNC_WKUP_INT_HRS];
        }

        public void SetRTCSyncTime(int time)
        {
            byte[] startTime = new byte[2];
            startTime = BitConverter.GetBytes(time);
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_RTC_SYNC_WKUP_TIME] = startTime[0];
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_RTC_SYNC_WKUP_TIME + 1] = startTime[1];
        }

        public int GetRTCSyncTime()
        {
            byte[] startTime = new byte[4];
            Array.Copy(OpConfig.ConfigurationBytes, (int)ConfigurationBytesIndexName.BLE_RTC_SYNC_WKUP_TIME, startTime, 0, 2);
            int time = BitConverter.ToInt32(startTime, 0);
            return time;
        }

        public void SetRTCSyncDuration(int duration)
        {
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_RTC_SYNC_WKUP_DUR] = (byte)duration;
        }

        public int GetRTCSyncDuration()
        {
            return OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_RTC_SYNC_WKUP_DUR];
        }

        public void SetRTCSyncRetryInterval(int interval)
        {
            byte[] retryInterval = new byte[2];
            retryInterval = BitConverter.GetBytes(interval);
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_RTC_SYNC_RETRY_INT] = retryInterval[0];
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.BLE_RTC_SYNC_RETRY_INT + 1] = retryInterval[1];
        }

        public int GetRTCSyncRetryInterval()
        {
            byte[] retryInterval = new byte[4];
            Array.Copy(OpConfig.ConfigurationBytes, (int)ConfigurationBytesIndexName.BLE_RTC_SYNC_RETRY_INT, retryInterval, 0, 2);
            int time = BitConverter.ToInt32(retryInterval, 0);
            return time;
        }

        public void SetAdaptiveSchedulerInterval(int interval)
        {
            byte[] adaptiveSchedulerInterval = new byte[2];
            adaptiveSchedulerInterval = BitConverter.GetBytes(interval);
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.ADAPTIVE_SCHEDULER_INT] = adaptiveSchedulerInterval[0];
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.ADAPTIVE_SCHEDULER_INT + 1] = adaptiveSchedulerInterval[1];
        }

        public int GetAdaptiveSchedulerInterval()
        {
            byte[] adaptiveSchedulerInterval = new byte[4];
            Array.Copy(OpConfig.ConfigurationBytes, (int)ConfigurationBytesIndexName.ADAPTIVE_SCHEDULER_INT, adaptiveSchedulerInterval, 0, 2);
            int time = BitConverter.ToInt32(adaptiveSchedulerInterval, 0);
            return time;
        }

        public void SetAdaptiveSchedulerMaxFailCount(int count)
        {
            OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.ADAPTIVE_SCHEDULER_FAILCOUNT_MAX] = (byte)count;
        }

        public int GetAdaptiveSchedulerMaxFailCount()
        {
            return OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.ADAPTIVE_SCHEDULER_FAILCOUNT_MAX];
        }

        public bool IsLoggingEnabled()
        {
            if (OpConfig.ConfigurationBytes == null)
            {
                throw new Exception("Configuration Bytes Unknown");
            }
            if ((int)(OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] & 0b00000001) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsDeviceEnabled()
        {
            if (OpConfig.ConfigurationBytes == null)
            {
                throw new Exception("Configuration Bytes Unknown");
            }
            if ((int)(OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.GEN_CFG_0] & 0b00000010) == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsPasskeyEnabled()
        {
            if (OpConfig.ConfigurationBytes == null)
            {
                throw new Exception("Configuration Bytes Unknown");
            }
            if ((int)(OpConfig.ConfigurationBytes[(int)ConfigurationBytesIndexName.GEN_CFG_2] & 0b00000100) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Sensor GetSensor(string key)
        {
            foreach (Sensor sensor in SensorList.Values)
            {
                if (sensor.GetSensorName().Equals(key))
                {
                    return sensor;
                }
            }
            return null;
        }
        public void UpdateDeviceAndSensorConfiguration()
        {
            foreach (Sensor sensor in SensorList.Values)
            {
                if (ProdConfig != null)
                {
                    sensor.SetDeviceHardwareIdentifier(ProdConfig.HardwareIdentifier);
                } else
                {
                    sensor.SetDeviceHardwareIdentifier(HardwareIdentifier.UNKNOWN);
                }
                if (OpConfig != null)
                {
                    sensor.InitializeUsingOperationConfig(OpConfig.ConfigurationBytes);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual byte[] GenerateConfigurationBytes()
        {
            var configBytes = OpConfig.ConfigurationBytes;
            foreach (Sensor sensor in SensorList.Values)
            {
                configBytes = sensor.GenerateOperationConfig(configBytes);
            }
            return configBytes;
        }

        public byte[] GetOperationalConfigByteArray()
        {
            return OpConfig.ConfigurationBytes;
        }

        public byte[] GetProductionConfigByteArray()
        {
            return BitHelper.MSBByteArray(ProdConfig.Payload.Replace("-", "")).ToArray();
        }

        public StatusPayload GetStatus()
        {
            return Status;
        }

        public TimePayload GetLastReceivedRTC()
        {
            return Time;
        }
        public ProdConfigPayload GetProductionConfig()
        {
            return ProdConfig;
        }

        public static double CalibrateADCValueToMilliVolts(int uncalValue, HardwareIdentifier id)
        {
            return CalibrateADCValueToVolts(uncalValue, id)*1000;
        }

        public static double CalibrateADCValueToVolts(int uncalValue, HardwareIdentifier id)
        {
            var adcRange = Math.Pow(2, 12) - 1;  // 12-bit
            var refVoltage = (1.8 / 4.0);
            if (id.Equals(HardwareIdentifier.VERISENSE_GSR_PLUS))
            {
                refVoltage = (3.0 / 4.0);
            }
            var adcScaling = (1.0 / 4.0);
            var adcOffset = 0;
            var calValue = ((((uncalValue - adcOffset) * refVoltage) / adcRange) / adcScaling);
            return calValue;
        }

        public static DeviceByteArraySettings GetDeviceOpSettingFromDisplayName(DeviceByteArraySettings[] settings, string displayName)
        {
            foreach (DeviceByteArraySettings setting in settings)
            {
                if (setting.GetDisplayName().Equals(displayName))
                {
                    return setting;
                }
            }
            return DefaultVerisenseConfiguration.Unknown_Device_OpConfig_Setting;
        }

        public class DeviceByteArraySettings
        {
            protected string DisplayName;
            protected byte[] OperationalConfigurationBytes;
            protected string Description = "";
           
            public DeviceByteArraySettings(string displayName, byte[] operationalConfigurationBytes, string description)
            {
                DisplayName = displayName;
                OperationalConfigurationBytes = operationalConfigurationBytes;
                Description = description;
            }

            public string GetDisplayName() { return DisplayName; }
            public byte[] GetOperationalConfigurationBytes() { return OperationalConfigurationBytes; }
            public string GetDescription() { return Description; }
        }

        public static DeviceByteSetting GetDeviceSettingFromConfigurationValue(DeviceByteSetting[] settings, int value)
        {
            foreach (DeviceByteSetting setting in settings)
            {
                if (setting.GetConfigurationByte() == value)
                {
                    return setting;
                }
            }
            return new DeviceByteSetting("Unknown", -1, -1);
        }

        public static DeviceByteSetting GetDeviceOpSettingFromDisplayName(DeviceByteSetting[] settings, string displayName)
        {
            foreach (DeviceByteSetting setting in settings)
            {
                if (setting.GetDisplayName().Equals(displayName))
                {
                    return setting;
                }
            }
            return Unknown_Device_Setting;
        }

        public class DeviceByteSetting
        {
            protected string DisplayName;
            protected int OperationalConfigurationByte;
            protected Object SettingsValue;
            protected string Description = "";

            public DeviceByteSetting(string displayName, int operationalConfigurationByte, Object settingsValue)
            {
                DisplayName = displayName;
                OperationalConfigurationByte = operationalConfigurationByte;
                SettingsValue = settingsValue;
            }

            public string GetDisplayName() { return DisplayName; }
            public int GetConfigurationByte() { return OperationalConfigurationByte; }
            public Object GetSettingsValue() { return SettingsValue; }
            public string GetDescription() { return Description; }

        }
    }
}
