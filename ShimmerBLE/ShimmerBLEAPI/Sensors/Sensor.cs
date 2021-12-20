using shimmer.Helpers;
using ShimmerAPI;
using System;
using System.Collections.Generic;
using System.Text;
using static ShimmerBLEAPI.Devices.VerisenseDevice;

namespace shimmer.Sensors
{
    public abstract class Sensor
    {
        public Sensor()
        {
            
        }

        /// <summary>
        /// Every sensor should belong to a device
        /// </summary>
        protected HardwareIdentifier DeviceHardwareIdentifier = HardwareIdentifier.UNKNOWN; 

        public void SetDeviceHardwareIdentifier(HardwareIdentifier id)
        {
            DeviceHardwareIdentifier = id;
        }

        int ByteStreamingPayloadSensorIds = -1;
        protected bool Clone = false;

        #region Timestamp props

        //For Shimmer internal clock timestamp (ticks):
        readonly static int ClockFreq = 32768;
        readonly static int TimestampTicksMaxValue = 1966080;  // This is based off the 32768 clock which resets after 1 min so (60 * 32768) 
        protected double LastReceivedTimestampTicksUnwrapped { get; set; } = 0;
        protected double CurrentTimestampTicksCycle { get; set; } = 0;

        //For SystemTimestampPlot:
        protected double SystemTimestampOffsetFirstTime { get; set; } = 0;
        protected bool IsFirstTimeSystemTimestampOffsetStored { get; set; } = false;

        #endregion Timestamp props

        public abstract ObjectCluster ParseSensorData(byte[] sample, ObjectCluster ojc);
        public abstract List<ObjectCluster> ParsePayloadData(byte[] payload, String deviceID);
        public abstract byte[] GenerateOperationConfig(byte[] operationalConfigBytes);

        public abstract void InitializeUsingOperationConfig(byte[] operationalConfigBytes);
        public abstract SensorSetting GetSamplingRate();
        public abstract void SetSamplingRate(SensorSetting rate);

        public abstract string GetSensorName();
        public static SensorSetting UnknownSetting = new SensorSetting("Unknown", -1, -1);

        public static SensorSetting GetSensorSettingFromDisplayName(SensorSetting[] settings, string displayName)
        {
            foreach (SensorSetting setting in settings){
                if (setting.GetDisplayName().Equals(displayName))
                {
                    return setting;
                }
            }
            return UnknownSetting;
        }

        public static SensorSetting GetSensorSettingFromConfigurationValue(SensorSetting[] settings, int value)
        {
            foreach (SensorSetting setting in settings)
            {
                if (setting.GetConfigurationValue()==value)
                {
                    return setting;
                }
            }
            return Sensor.UnknownSetting;
        }

        public class SensorSetting
        {
            protected string DisplayName;
            protected int ConfigurationValue;
            protected Object SettingsValue; //can be an integer for ie 100 Hz and can be a string for ie LowPower and a bool for enabled
            protected string Description ="";
            public SensorSetting(string displayName, int confValue, Object settingsValue)
            {
                DisplayName = displayName;
                ConfigurationValue = confValue;
                SettingsValue = settingsValue;
            }

            public SensorSetting(string displayName, int confValue, Object settingsValue, string description)
            {
                DisplayName = displayName;
                ConfigurationValue = confValue;
                SettingsValue = settingsValue;
                Description = description;
            }

            public string GetDisplayName() { return DisplayName; }
            public int GetConfigurationValue() { return ConfigurationValue; }
            public Object GetSettingsValue() { return SettingsValue; }
            public string GetDescription() { return Description; }
        }

        #region Timestamp functions

        /// <summary>
        /// Unwraps the Shimmer internal clock timestamp in ticks, to milliseconds
        /// </summary>
        /// <param name="ticks"></param>
        public double GetShimmerTimestampUnwrapped(double ticks, double systemTimestamp)
        {
            var timestampUnwrappedTicks = UnwrapTimestamp(ticks);
            var timestampUnwrappedMillis = (timestampUnwrappedTicks / ClockFreq) * 1000; //This is the timestamp for the last sample contained in the packet
            if (!IsFirstTimeSystemTimestampOffsetStored)
            {
                IsFirstTimeSystemTimestampOffsetStored = true;
                SystemTimestampOffsetFirstTime = systemTimestamp - timestampUnwrappedMillis;
            }
            return timestampUnwrappedMillis;
        }

        protected double UnwrapTimestamp(double timestampTicks)
        {
            //First convert to continuous timestamp
            double timestampUnwrappedTicks = CalculateTimestampUnwrapped(timestampTicks);

            //Check if there was a roll-over
            if (LastReceivedTimestampTicksUnwrapped > timestampUnwrappedTicks)
            {
                CurrentTimestampTicksCycle += 1;
                //Recalculate timestamp
                timestampUnwrappedTicks = CalculateTimestampUnwrapped(timestampTicks);
            }

            LastReceivedTimestampTicksUnwrapped = timestampUnwrappedTicks;

            return timestampUnwrappedTicks;
        }

        protected double CalculateTimestampUnwrapped(double timestampTicks)
        {
            return timestampTicks + (TimestampTicksMaxValue * CurrentTimestampTicksCycle);
        }

        /// <summary>
        /// Adds the Shimmer internal clock timestamp (ticks), Shimmer internal clock timestamp (millis), system timestamp (millis) and system timestamp plot (millis) to the ObjectCluster
        /// These timestamps are extrapolated backwards for all other samples in the payload as the internal clock timestamps are only recorded for the latest sample in a payload
        /// </summary>
        public void ExtrapolateTimestampsAndAddToOjc(ObjectCluster ojc, double tsLastSampleTicks, double tsLastSampleMillis, double systemTsLastSampleMillis, int numOfSamples, int i, double samplingRate)
        {
            double sampleOffset = (numOfSamples - i - 1) / samplingRate;
            double tsMillis = tsLastSampleMillis - (sampleOffset * 1000);
            double systemTsMillis = systemTsLastSampleMillis - (sampleOffset * 1000);
            double systemTsPlotMillis = tsMillis + SystemTimestampOffsetFirstTime;
            ojc.Add(ShimmerConfiguration.SignalNames.TIMESTAMP, ShimmerConfiguration.SignalFormats.RAW, ShimmerConfiguration.SignalUnits.NoUnits, tsLastSampleTicks);
            ojc.Add(ShimmerConfiguration.SignalNames.TIMESTAMP, ShimmerConfiguration.SignalFormats.CAL, ShimmerConfiguration.SignalUnits.MilliSeconds, tsMillis);
            ojc.Add(ShimmerConfiguration.SignalNames.SYSTEM_TIMESTAMP, ShimmerConfiguration.SignalFormats.CAL, ShimmerConfiguration.SignalUnits.MilliSeconds, systemTsMillis);
            ojc.Add(ShimmerConfiguration.SignalNames.SYSTEM_TIMESTAMP_PLOT, ShimmerConfiguration.SignalFormats.CAL, ShimmerConfiguration.SignalUnits.MilliSeconds, systemTsPlotMillis);
        }

        public void ResetTimestamps()
        {
            LastReceivedTimestampTicksUnwrapped = 0;
            CurrentTimestampTicksCycle = 0;
            IsFirstTimeSystemTimestampOffsetStored = false;
            SystemTimestampOffsetFirstTime = 0;
        }

        #endregion Timestamp functions

    }
}
