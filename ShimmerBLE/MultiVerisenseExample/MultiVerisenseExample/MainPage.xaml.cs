using ShimmerBLEAPI;
using ShimmerBLEAPI.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using shimmer.Sensors;
using shimmer.Models;
using ShimmerAPI;
using static shimmer.Models.ShimmerBLEEventData;
using static ShimmerBLEAPI.AbstractPlotManager;

namespace MultiShimmerExample
{
    public partial class MainPage : ContentPage
    {
        private Dictionary<string, VerisenseBLEDevice> Devices = new Dictionary<string, VerisenseBLEDevice>();
        private Dictionary<string, bool> IsFirstOjcForDevice = new Dictionary<string, bool>();
        private PlotManager PlotManager;

        List<string> uuids = new List<string>()
        {
            //"00000000-0000-0000-0000-e1ec063f5c80",
            //"00000000-0000-0000-0000-daa56d898b02"
            //"00000000-0000-0000-0000-e7452c6d6f14",
            //"00000000-0000-0000-0000-c96117537402",
            //"def7b570-bb64-5167-aa2c-76f634454258",
            //"7b3eba6c-026c-0861-bb0d-45d23d4dad64"
            "00000000-0000-0000-0000-daa619f04ad7"
        }; 

        public MainPage()
        {
            InitializeComponent();
            PlotManager = new PlotManager("Data", "Data Point", "Timestamp", true);
            plotView.Model = PlotManager.BuildPlotModel();
        }

        public async void ConnectDevices()
        {
            foreach (string uuid in uuids)
            {
                VerisenseBLEDevice device = new VerisenseBLEDevice(uuid, "");
                bool result = await device.Connect(true);
                if (result)
                {
                    Debug.WriteLine("Device Version: " + device.GetProductionConfig().REV_HW_MAJOR + "." + device.GetProductionConfig().REV_HW_MINOR);
                    Debug.WriteLine("Firmware Version: " + device.GetProductionConfig().REV_FW_MAJOR + "." + device.GetProductionConfig().REV_FW_MINOR + "." + device.GetProductionConfig().REV_FW_INTERNAL);
                    if (Devices.ContainsKey(uuid))
                    {
                        Devices.Remove(uuid);
                        Devices.Add(uuid, device);
                    }
                    else if (!Devices.ContainsKey(uuid))
                    {
                        Devices.Add(uuid, device);
                    }
                    Debug.WriteLine("\nBT state: " + device.GetVerisenseBLEState() + "\nUUID: " + device.Asm_uuid + "\nBattery: " + device.GetStatus().BatteryPercent + "%");
                    ConfigureDevice(device);
                }
                else
                {
                    Debug.WriteLine("Failed to connect device! UUID: " + uuid);
                }
            }
        }

        public void ConfigureDevice(VerisenseBLEDevice device)
        {
            VerisenseBLEDevice clone = new VerisenseBLEDevice(device);
            clone.setDeviceEnabled(true);
            clone.setLoggingEnabled(true);

            SensorLIS2DW12 sensor = (SensorLIS2DW12)clone.GetSensor(SensorLIS2DW12.SensorName);
            sensor.SetAccelEnabled(true);
            sensor.SetSamplingRate(SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_25Hz);

            bool accelDetection = sensor.IsAccelEnabled();
            string accelRate = sensor.GetSamplingRate().GetDisplayName();
            string accelMode = sensor.GetMode().GetDisplayName();
            string accelLPMode = sensor.GetLowPowerMode().GetDisplayName();
            Debug.WriteLine("\n--|ACCEL|--" + "\nIsAccelEnabled: " + accelDetection + "\nAccelRate: " + accelRate + "\nAccelMode: " + accelMode + "\nAccelLowPowerMode: " + accelLPMode);
            var opconfigbytes = clone.GenerateConfigurationBytes();
            device.WriteAndReadOperationalConfiguration(opconfigbytes);
        }

        public async void StartStreamingDevices()
        {
            foreach (VerisenseBLEDevice device in Devices.Values)
            {
                var streamResult = await device.ExecuteRequest(RequestType.StartStreaming);
                Debug.WriteLine("Stream Status: " + streamResult);
                if (device != null)
                {
                    if (!IsFirstOjcForDevice.ContainsKey(device.Asm_uuid.ToString()))
                    {
                        IsFirstOjcForDevice.Add(device.Asm_uuid.ToString(), true);
                    }

                    device.ShimmerBLEEvent -= ShimmerDevice_BLEEvent;
                }
                device.ShimmerBLEEvent += ShimmerDevice_BLEEvent;
            }
        }

        public async void StartDataSync()
        {
            foreach (VerisenseBLEDevice device in Devices.Values)
            {
                var syncResult = device.ExecuteRequest(RequestType.TransferLoggedData);
                Debug.WriteLine("Sync Status: " + syncResult);
                if (device != null)
                {
                    device.ShimmerBLEEvent -= ShimmerDevice_BLEEvent;
                }
                device.ShimmerBLEEvent += ShimmerDevice_BLEEvent;
            }
        }

        private void ShimmerDevice_BLEEvent(object sender, ShimmerBLEEventData e)
        {
            if (e.CurrentEvent == VerisenseBLEEvent.NewDataPacket)
            {
                ObjectCluster ojc = (ObjectCluster)e.ObjMsg;

                //Plot signals
                if (IsFirstOjcForDevice[ojc.GetCOMPort()])
                {
                    IsFirstOjcForDevice[ojc.GetCOMPort()] = false;
                    List<string[]> signals = PlotManager.GetAllSignalPropertiesFromOjc(ojc);
                    foreach (string[] signal in signals)
                    {
                        if (signal[(int)SignalArrayIndex.Format].Equals(ShimmerConfiguration.SignalFormats.CAL) && signal[(int)SignalArrayIndex.Name].Contains("Accel"))
                        {
                            PlotManager.AddSignalToPlotDefaultColors(signal);
                        }
                        else if (signal[(int)SignalArrayIndex.Name].Equals(ShimmerConfiguration.SignalNames.SYSTEM_TIMESTAMP))
                        {
                            PlotManager.AddXAxis(signal);
                        }
                    }
                }
                PlotManager.FilterDataAndPlot(ojc);

                //Log signals to console
                var a2x = ojc.GetData(SensorLIS2DW12.ObjectClusterSensorName.LIS2DW12_ACC_X, ShimmerConfiguration.SignalFormats.CAL);
                var a2y = ojc.GetData(SensorLIS2DW12.ObjectClusterSensorName.LIS2DW12_ACC_Y, ShimmerConfiguration.SignalFormats.CAL);
                var a2z = ojc.GetData(SensorLIS2DW12.ObjectClusterSensorName.LIS2DW12_ACC_Z, ShimmerConfiguration.SignalFormats.CAL);
                var systemTimestampPlot = ojc.GetData(ShimmerConfiguration.SignalNames.SYSTEM_TIMESTAMP_PLOT, ShimmerConfiguration.SignalFormats.CAL).Data;
                //Debug.WriteLine("UUID: " + ojc.GetCOMPort() + " |X : " + Math.Round(a2x.Data, 2) + "  Y : " + Math.Round(a2y.Data, 2) + "  Z : " + Math.Round(a2z.Data, 2) + "| Systen TS Plot: " + systemTimestampPlot + " ms");
            }
        }

        public async void StopStreamingDevices()
        {
            PlotManager.RemoveAllSignalsFromPlot();
            foreach (VerisenseBLEDevice device in Devices.Values)
            {
                var result = await device.ExecuteRequest(RequestType.StopStreaming);
                IsFirstOjcForDevice[device.Asm_uuid.ToString()] = true;
                Debug.WriteLine("\nUUID: " + device.Asm_uuid + " stop streaming request \nResult: " + result + "\n");
            }
        }

        public async void DisconnectDevices()
        {
            PlotManager.RemoveAllSignalsFromPlot();
            List<string> listOfDisconnectedUuids = new List<string>();
            foreach (VerisenseBLEDevice device in Devices.Values)
            {
                var result = await device.Disconnect();
                if (result)
                {
                    listOfDisconnectedUuids.Add(device.Asm_uuid.ToString());
                }

                Debug.WriteLine("\nUUID: " + device.Asm_uuid + " attempt disconnect \nResult: " + result + "\nNew BLE Status: " + device.GetVerisenseBLEState());
            }

            //Remove disconnected devices from the Dictionary
            foreach (var uuid in listOfDisconnectedUuids)
            {
                Devices.Remove(uuid);
            }
        }

        //------------------------------------------------------------------------------------------

        //GUI Functionality
        private void connectDevicesButton_Clicked(object sender, EventArgs e)
        {
            ConnectDevices();
        }

        private void startStreamingButton_Clicked(object sender, EventArgs e)
        {
            StartStreamingDevices();
        }

        private void stopStreamingButton_Clicked(object sender, EventArgs e)
        {
            StopStreamingDevices();
        }

        private void disconnectDevicesButton_Clicked(object sender, EventArgs e)
        {
            DisconnectDevices();
        }

        private void syncDevicesButton_Clicked(object sender,EventArgs e)
        {
            StartDataSync();
        }
    }
}