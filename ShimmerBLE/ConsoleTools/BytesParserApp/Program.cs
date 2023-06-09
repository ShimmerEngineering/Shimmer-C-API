using shimmer.Helpers;
using shimmer.Models;
using shimmer.Sensors;
using ShimmerBLEAPI.Devices;
using System;
using System.Threading;

namespace BytesParserApp
{
    class Program
    {
        static readonly string statusPayloadString = "31-41-00-EB-BF-01-28-07-20-DC-77-AA-01-1F-05-64-B5-50-AA-01-FF-FF-FF-FF-00-FD-07-03-00-08-00-00-00-00-02-00-00-AA-4E-1D-CD-FD-11-FF-FF-FF-7B-7D-AA-01-00-00-00-00-00-00-00-03-00-00-00-00-00-00-00-08-00-00";
        static readonly string opconfigPayloadString = "34-48-00-5A-9F-80-02-00-30-20-00-7F-00-FF-3F-00-00-00-00-80-00-00-00-00-00-00-00-00-00-00-00-00-00-03-F4-18-3C-00-0A-0F-00-18-3C-00-0A-0F-00-18-3C-00-0A-0F-00-17-04-FF-FF-FF-3C-00-0E-00-00-63-28-CC-CC-1E-00-0A-00-00-00-00-01";
        static readonly string prodconfigPayloadString = "33-37-00-5A-BB-A2-01-25-09-19-01-00-01-02-77-00-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF";
        
        public class VerisenseBLEDeviceExample : VerisenseBLEDevice
        {
            public VerisenseBLEDeviceExample(string id, string name, byte[] opconfigbytes) : base(id, name)
            {
                OpConfig = new OpConfigPayload();
                OpConfig.ConfigurationBytes = new byte[opconfigbytes.Length];
                Array.Copy(opconfigbytes, OpConfig.ConfigurationBytes, opconfigbytes.Length); //deep copy
                UpdateDeviceAndSensorConfiguration();
            }
        }

        static void Main(string[] args)
        {
            OpConfigPayload opconfig = new OpConfigPayload();
            byte[] opconfigPayloadArray = BitHelper.MSBByteArray(opconfigPayloadString.Replace("-", "")).ToArray();
            opconfig.ProcessPayload(opconfigPayloadArray);
            VerisenseBLEDevice device = new VerisenseBLEDeviceExample("00000000-0000-0000-0000-000000000000", "", opconfig.ConfigurationBytes);
            OutputOpconfig(device);

            ProdConfigPayload prodconfig = new ProdConfigPayload();
            byte[] prodconfigPayloadArray = BitHelper.MSBByteArray(prodconfigPayloadString.Replace("-", "")).ToArray();
            prodconfig.ProcessPayload(prodconfigPayloadArray);
            OutputProdConfig(prodconfig);

            StatusPayload status = new StatusPayload();
            byte[] statusPayloadArray = BitHelper.MSBByteArray(statusPayloadString.Replace("-", "")).ToArray();
            status.ProcessPayload(statusPayloadArray, 0);
            OutputStatus(status);
        }

        static void OutputProdConfig(ProdConfigPayload prodconfig)
        {
            Console.WriteLine("\n\nProdConfig\n\n");
            Console.WriteLine("ASMID: " + prodconfig.ASMID);
            Console.WriteLine("HardwareIdentifier: " + prodconfig.HardwareIdentifier);
            Console.WriteLine("REV_HW_MAJOR: " + prodconfig.REV_HW_MAJOR);
            Console.WriteLine("REV_HW_MINOR: " + prodconfig.REV_HW_MINOR);
            Console.WriteLine("REV_FW_MAJOR: " + prodconfig.REV_FW_MAJOR);
            Console.WriteLine("REV_FW_MINOR: " + prodconfig.REV_FW_MINOR);
            Console.WriteLine("REV_FW_INTERNAL: " + prodconfig.REV_FW_INTERNAL);
            Console.WriteLine("REV_HW_INTERNAL: " + prodconfig.REV_HW_INTERNAL);
            Console.WriteLine("PasskeyID: " + prodconfig.PasskeyID);
            Console.WriteLine("Passkey: " + prodconfig.Passkey);
            Console.WriteLine("AdvertisingNamePrefix: " + prodconfig.AdvertisingNamePrefix);
        }

        static void OutputOpconfig(VerisenseBLEDevice device)
        {
            Console.WriteLine("\n\nDevice Settings\n\n");
            Console.WriteLine("DeviceLogging: " + device.IsLoggingEnabled());
            Console.WriteLine("DeviceEnabled: " + device.IsDeviceEnabled());
            Console.WriteLine("BluetoothEnabled: " + device.IsBluetoothEnabled());
            Console.WriteLine("USBEnabled: " + device.IsUSBEnabled());
            DateTime StartDateAndTime = device.ConvertUnixTSInMinuteIntoDateTime(device.GetStartTimeinMinutes());
            DateTime EndDateAndTime = device.ConvertUnixTSInMinuteIntoDateTime(device.GetEndTimeinMinutes());
            Console.WriteLine("StartTime: " + StartDateAndTime.TimeOfDay);
            Console.WriteLine("StartDate: " + StartDateAndTime.Date);
            Console.WriteLine("EndTime: " + EndDateAndTime.TimeOfDay);
            Console.WriteLine("EndDate: " + EndDateAndTime.Date);
            Console.WriteLine("RadioOutputPower: " + device.GetBLETXPower().GetDisplayName());
            Console.WriteLine("BLEWakeupRetryCount: " + device.GetBLERetryCount().ToString());
            Console.WriteLine("DataTransferInterval: " + device.GetDataTransferInterval().ToString());
            Console.WriteLine("DataTransferTime: " + device.GetDataTransferStartTime().ToString());
            Console.WriteLine("DataTransferDuration: " + device.GetDataTransferDuration().ToString());
            Console.WriteLine("DataTransferRetryInterval: " + device.GetDataTransferRetryInterval().ToString());
            Console.WriteLine("StatusTransferInterval: " + device.GetStatusInterval().ToString());
            Console.WriteLine("StatusTransferTime: " + device.GetStatusStartTime().ToString());
            Console.WriteLine("StatusTransferDuration: " + device.GetStatusDuration().ToString());
            Console.WriteLine("StatusTransferRetryInterval: " + device.GetStatusRetryInterval().ToString());
            Console.WriteLine("RTCSyncInterval: " + device.GetRTCSyncInterval().ToString());
            Console.WriteLine("RTCSyncTime: " + device.GetRTCSyncTime().ToString());
            Console.WriteLine("RTCSyncDuration: " + device.GetRTCSyncDuration().ToString());
            Console.WriteLine("RTCSyncRetryInterval: " + device.GetRTCSyncRetryInterval().ToString());

            Console.WriteLine("\n\nSensor Accel Settings\n\n");
            Console.WriteLine("SensorAccel: " + ((SensorLIS2DW12)device.GetSensor(SensorLIS2DW12.SensorName)).IsAccelEnabled());
            Console.WriteLine("AccelRange: " + ((SensorLIS2DW12)device.GetSensor(SensorLIS2DW12.SensorName)).GetAccelRange().GetDisplayName());
            Console.WriteLine("AccelRate: " + ((SensorLIS2DW12)device.GetSensor(SensorLIS2DW12.SensorName)).GetSamplingRate().GetDisplayName());
            Console.WriteLine("AccelMode " + ((SensorLIS2DW12)device.GetSensor(SensorLIS2DW12.SensorName)).GetMode().GetDisplayName());
            Console.WriteLine("AccelLPMode: " + ((SensorLIS2DW12)device.GetSensor(SensorLIS2DW12.SensorName)).GetLowPowerMode().GetDisplayName());
            Console.WriteLine("BandwidthFilter: " + ((SensorLIS2DW12)device.GetSensor(SensorLIS2DW12.SensorName)).GetAccelBandwidthFilter().GetDisplayName());
            Console.WriteLine("FIFOthreshold: " + ((SensorLIS2DW12)device.GetSensor(SensorLIS2DW12.SensorName)).GetAccelFIFOThreshold().ToString());
            Console.WriteLine("FMode: " + ((SensorLIS2DW12)device.GetSensor(SensorLIS2DW12.SensorName)).GetAccelFMode().GetDisplayName());
            Console.WriteLine("SensorAccelLowNoise: " + ((SensorLIS2DW12)device.GetSensor(SensorLIS2DW12.SensorName)).IsLowNoiseEnabled());
            Console.WriteLine("SensorAccelHighPassFilter: " + ((SensorLIS2DW12)device.GetSensor(SensorLIS2DW12.SensorName)).IsHighPassFilterEnabled());
            Console.WriteLine("SensorAccelHighPassFilterRefMode: " + ((SensorLIS2DW12)device.GetSensor(SensorLIS2DW12.SensorName)).IsHighPassFilterRefModeEnabled());

            Console.WriteLine("\n\nSensor Accel2 and Gyro Settings\n\n");
            Console.WriteLine("SensorAccel2: " + ((SensorLSM6DS3)device.GetSensor(SensorLSM6DS3.SensorName)).IsAccelEnabled());
            Console.WriteLine("SensorGyro: " + ((SensorLSM6DS3)device.GetSensor(SensorLSM6DS3.SensorName)).IsGyroEnabled());
            Console.WriteLine("Accel2GyroRate: " + ((SensorLSM6DS3)device.GetSensor(SensorLSM6DS3.SensorName)).GetSamplingRate().GetDisplayName());
            Console.WriteLine("Accel2Range: " + ((SensorLSM6DS3)device.GetSensor(SensorLSM6DS3.SensorName)).GetAccelRange().GetDisplayName());
            Console.WriteLine("GyroRange: " + ((SensorLSM6DS3)device.GetSensor(SensorLSM6DS3.SensorName)).GetGyroRange().GetDisplayName());
            Console.WriteLine("SensorGyroFullScaleAt125: " + ((SensorLSM6DS3)device.GetSensor(SensorLSM6DS3.SensorName)).IsGyroFullScaleAt125Enabled());
            Console.WriteLine("GyroFIFODecimation: " + ((SensorLSM6DS3)device.GetSensor(SensorLSM6DS3.SensorName)).GetGyroFIFODecimationSetting().GetDisplayName());
            Console.WriteLine("Accel2FIFODecimation: " + ((SensorLSM6DS3)device.GetSensor(SensorLSM6DS3.SensorName)).GetAccelFIFODecimationSetting().GetDisplayName());
            Console.WriteLine("FIFOOutputDataRate: " + ((SensorLSM6DS3)device.GetSensor(SensorLSM6DS3.SensorName)).GetFIFOOutputDataRateSetting().GetDisplayName());
            Console.WriteLine("FIFOMode: " + ((SensorLSM6DS3)device.GetSensor(SensorLSM6DS3.SensorName)).GetFIFOModeSetting().GetDisplayName());
            Console.WriteLine("Accel2AntiAliasingFilterBW: " + ((SensorLSM6DS3)device.GetSensor(SensorLSM6DS3.SensorName)).GetAccelAntiAliasingFilterBWSetting().GetDisplayName());
            Console.WriteLine("HPFilterCutOffFreq: " + ((SensorLSM6DS3)device.GetSensor(SensorLSM6DS3.SensorName)).GetHPFilterCutOffFreqSetting().GetDisplayName());
            Console.WriteLine("SensorGyroHighPassFilter: " + ((SensorLSM6DS3)device.GetSensor(SensorLSM6DS3.SensorName)).IsHighPassFilterEnabled());
            Console.WriteLine("SensorGyroDigitalHPReset: " + ((SensorLSM6DS3)device.GetSensor(SensorLSM6DS3.SensorName)).IsDigitalHPFilterResetEnabled());
            Console.WriteLine("SourceRegisterRoundingStatus: " + ((SensorLSM6DS3)device.GetSensor(SensorLSM6DS3.SensorName)).IsSourceRegRoundingStatusEnabled());
            Console.WriteLine("StepCounterAndTimestamp: " + ((SensorLSM6DS3)device.GetSensor(SensorLSM6DS3.SensorName)).IsStepCounterAndTSDataEnabled());
            Console.WriteLine("WriteInFIFOAtEveryStepDetected: " + ((SensorLSM6DS3)device.GetSensor(SensorLSM6DS3.SensorName)).IsWriteInFIFOAtEveryStepDetectedEnabled());
            Console.WriteLine("Accel2LowPassFilter: " + ((SensorLSM6DS3)device.GetSensor(SensorLSM6DS3.SensorName)).IsAccelLowPassFilterEnabled());
            Console.WriteLine("Accel2SlopeOrHighPassFilter: " + ((SensorLSM6DS3)device.GetSensor(SensorLSM6DS3.SensorName)).IsAccelSlopeOrHighPassFilterEnabled());
            Console.WriteLine("Accel2LowPassFilterOn6D: " + ((SensorLSM6DS3)device.GetSensor(SensorLSM6DS3.SensorName)).IsAccelLowPassFilterOn6DEnabled());
            Console.WriteLine("FIFOThreshold: " + ((SensorLSM6DS3)device.GetSensor(SensorLSM6DS3.SensorName)).GetFIFOThreshold().GetDisplayName());

            Console.WriteLine("\n\nSensor GSR and battery Settings\n\n");
            Console.WriteLine("GSROversamplingRate: " + ((shimmer.Sensors.SensorGSR)device.GetSensor(shimmer.Sensors.SensorGSR.SensorName)).GetOversamplingRate().GetDisplayName());
            Console.WriteLine("SensorGSR: " + ((shimmer.Sensors.SensorGSR)device.GetSensor(shimmer.Sensors.SensorGSR.SensorName)).IsGSREnabled());
            Console.WriteLine("SensorBatt: " + ((shimmer.Sensors.SensorGSR)device.GetSensor(shimmer.Sensors.SensorGSR.SensorName)).IsBattEnabled());
            Console.WriteLine("GSRRange: " + ((shimmer.Sensors.SensorGSR)device.GetSensor(shimmer.Sensors.SensorGSR.SensorName)).GetGSRRange().GetDisplayName());
            Console.WriteLine("GSRRate: " + ((shimmer.Sensors.SensorGSR)device.GetSensor(shimmer.Sensors.SensorGSR.SensorName)).GetSamplingRate().GetDisplayName());

            Console.WriteLine("\n\nPPG Settings\n\n");
            Console.WriteLine("SensorPPGBlue: " + ((SensorPPG)device.GetSensor(SensorPPG.SensorName)).IsPPGBlueEnabled());
            Console.WriteLine("SensorPPGGreen: " + ((SensorPPG)device.GetSensor(SensorPPG.SensorName)).IsPPGGreenEnabled());
            Console.WriteLine("SensorPPGIR: " + ((SensorPPG)device.GetSensor(SensorPPG.SensorName)).IsPPGIREnabled());
            Console.WriteLine("SensorPPGRed: " + ((SensorPPG)device.GetSensor(SensorPPG.SensorName)).IsPPGRedEnabled());
            Console.WriteLine("PPGRecDuration: " + ((SensorPPG)device.GetSensor(SensorPPG.SensorName)).GetPPGRecordingDurationinSeconds().ToString());
            Console.WriteLine("PPGRecInterval: " + ((SensorPPG)device.GetSensor(SensorPPG.SensorName)).GetPPGRecordingIntervalinMinutes().ToString());
            Console.WriteLine("PPG_MA_DEFAULT: " + ((SensorPPG)device.GetSensor(SensorPPG.SensorName)).GetPGGDefaultLEDPulseAmplitude().ToString());
            Console.WriteLine("PPG_MA_MAX_RED_IR: " + ((SensorPPG)device.GetSensor(SensorPPG.SensorName)).GetMaxLEDPulseAmplitudeRedIR().ToString());
            Console.WriteLine("PPG_MA_MAX_GREEN_BLUE: " + ((SensorPPG)device.GetSensor(SensorPPG.SensorName)).GetMaxLEDPulseAmplitudeGreenBlue().ToString());
            Console.WriteLine("PPG_AGC_TARGET_PERCENT_OF_RANGE: " + ((SensorPPG)device.GetSensor(SensorPPG.SensorName)).GetAGCTargetRange().ToString());
            Console.WriteLine("PPG_MA_LED_PILOT: " + ((SensorPPG)device.GetSensor(SensorPPG.SensorName)).GetLEDPilotPulseAmplitude().ToString());
            Console.WriteLine("PPG_XTALK_DAC1: " + ((SensorPPG)device.GetSensor(SensorPPG.SensorName)).GetDAC1CROSSTALK().ToString());
            Console.WriteLine("PPG_XTALK_DAC2: " + ((SensorPPG)device.GetSensor(SensorPPG.SensorName)).GetDAC2CROSSTALK().ToString());
            Console.WriteLine("PPG_XTALK_DAC3: " + ((SensorPPG)device.GetSensor(SensorPPG.SensorName)).GetDAC3CROSSTALK().ToString());
            Console.WriteLine("PPG_XTALK_DAC4: " + ((SensorPPG)device.GetSensor(SensorPPG.SensorName)).GetDAC4CROSSTALK().ToString());
            Console.WriteLine("ProxAGCMode: " + ((SensorPPG)device.GetSensor(SensorPPG.SensorName)).GetProxAGCMode().GetDisplayName());
            Console.WriteLine("PPGSamplingAverage: " + ((SensorPPG)device.GetSensor(SensorPPG.SensorName)).GetPPGSampleAverage().GetDisplayName());
            Console.WriteLine("PPGWidth: " + ((SensorPPG)device.GetSensor(SensorPPG.SensorName)).GetPPGPulseWidth().GetDisplayName());
            Console.WriteLine("PPGRate: " + ((SensorPPG)device.GetSensor(SensorPPG.SensorName)).GetSamplingRate().GetDisplayName());
            Console.WriteLine("PPGRange: " + ((SensorPPG)device.GetSensor(SensorPPG.SensorName)).GetPPGRange().GetDisplayName());
        }

        static void OutputStatus(StatusPayload status)
        {
            Console.WriteLine("\n\nStatus payload\n\n");
            Console.WriteLine("ASM ID: " + status.ASMID);
            Console.WriteLine("StatusTimestamp: " + status.StatusTimestamp);
            Console.WriteLine("BatteryLevel: " + status.BatteryLevel);
            Console.WriteLine("BatteryPercent: " + status.BatteryPercent);
            Console.WriteLine("TransferSuccessTimestamp: " + status.TransferSuccessTimestamp);
            Console.WriteLine("TransferFailTimestamp: " + status.TransferFailTimestamp);
            Console.WriteLine("BaseStationTimestamp: " + status.BaseStationTimestamp);
            Console.WriteLine("FreeStorage: " + status.FreeStorage);
            Console.WriteLine("IsSuccess: " + status.IsSuccess);
            Console.WriteLine("VBattFallCounter: " + status.VBattFallCounter);
            Console.WriteLine("StatusFlags: " + status.StatusFlags);
            Console.WriteLine("UsbPowered: " + status.UsbPowered);
            Console.WriteLine("RecordingPaused: " + status.RecordingPaused);
            Console.WriteLine("FlashIsFull: " + status.FlashIsFull);
            Console.WriteLine("PowerIsGood: " + status.PowerIsGood);
            Console.WriteLine("AdaptiveScheduler: " + status.AdaptiveScheduler);
            Console.WriteLine("DfuServiceOn: " + status.DfuServiceOn);
            Console.WriteLine("SyncMode: " + status.SyncMode);
            Console.WriteLine("NextSyncAttemptTimestamp: " + status.NextSyncAttemptTimestamp);
            Console.WriteLine("StorageFull: " + status.StorageFull);
            Console.WriteLine("StorageToDel: " + status.StorageToDel);
            Console.WriteLine("StorageBad: " + status.StorageBad);
            Console.WriteLine("StorageCapacity: " + status.StorageCapacity);
            Console.WriteLine("Metadata_01: " + status.Metadata_01);
        }
    }
}