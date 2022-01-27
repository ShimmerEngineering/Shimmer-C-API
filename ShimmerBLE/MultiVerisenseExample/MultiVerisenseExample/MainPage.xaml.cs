using ShimmerBLEAPI;
using ShimmerBLEAPI.Devices;
using ShimmerBLEAPI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using shimmer.Sensors;
using shimmer.Communications;
using shimmer.Models;
using ShimmerAPI;
using System.ComponentModel;
using static shimmer.Models.ShimmerBLEEventData;
using static ShimmerBLEAPI.AbstractPlotManager;
using shimmer.Services;
using System.Linq;
using System.Collections.ObjectModel;

namespace MultiShimmerExample
{
    public partial class MainPage : ContentPage
    {
        public class DeviceInfo : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            // This method is called by the Set accessor of each property.  
            // The CallerMemberName attribute that is applied to the optional propertyName  
            // parameter causes the property name of the caller to be substituted as an argument.  
            private void NotifyPropertyChanged(string name)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
                }
            }

            private string _status;
            public string Status { get { return _status; } set { _status = value; NotifyPropertyChanged("Status"); } }
            private string _uuid { get; set; }
            public string Uuid { get { return _uuid; } set { _uuid = value; NotifyPropertyChanged("Uuid"); } }
            private string _payloadIndex { get; set; }
            public string PayloadIndex { get { return _payloadIndex; } set { _payloadIndex = value; NotifyPropertyChanged("PayloadIndex"); } }
            private string _transferSpeed { get; set; }
            public string TransferSpeed { get { return _transferSpeed; } set { _transferSpeed = value; NotifyPropertyChanged("TransferSpeed"); } }
            private string _binFilePath { get; set; }
            public string BinFilePath { get { return _binFilePath; } set { _binFilePath = value; NotifyPropertyChanged("BinFilePath"); } }
            private bool _isSelected { get; set; }
            public bool IsSelected { get { return _isSelected; } set { _isSelected = value; NotifyPropertyChanged("IsSelected"); } }
            private string _isPaired { get; set; }
            public string IsPaired { get { return _isPaired; } set { _isPaired = value; NotifyPropertyChanged("IsPaired"); } }
            public List<string[]> Signals { get; set; }

            public DeviceInfo(string uuid)
            {
                Uuid = uuid;
                Signals = new List<string[]>();
            }
        }

        private Dictionary<string, VerisenseBLEDevice> ConnectedDevices = new Dictionary<string, VerisenseBLEDevice>();
        private Dictionary<string, bool> IsFirstOjcForDevice = new Dictionary<string, bool>();
        private PlotManager PlotManager;
        IVerisenseBLEManager bleManager = DependencyService.Get<IVerisenseBLEManager>();
        ObservableCollection<DeviceInfo> deviceInfos = new ObservableCollection<DeviceInfo>();
        List<string> uuids = new List<string>();
        List<VerisenseBLEScannedDevice> ScannedDevices = new List<VerisenseBLEScannedDevice>();

        public MainPage()
        {
            InitializeComponent();
            PlotManager = new PlotManager("Data", "Data Point", "Timestamp", true);
            plotView.Model = PlotManager.BuildPlotModel();
            if (bleManager != null)
            {
                bleManager.BLEManagerEvent += BLEManager_BLEEvent;
            }
            deviceList.ItemsSource = deviceInfos;
        }

        //public async void GetListOfVerisenseDevicesFromOS()
        //{
        //    //DeviceInformationCollection PairedBluetoothDevices = await DeviceInformation.FindAllAsync(BluetoothDevice.GetDeviceSelectorFromPairingState(true));
        //    DeviceInformationCollection PairedBluetoothDevices = await DeviceInformation.FindAllAsync();
        //    foreach (DeviceInformation deviceInfo in PairedBluetoothDevices)
        //    {
        //        if (deviceInfo.Name.Contains("Verisense"))
        //        {
        //            String uuid = "00000000-0000-0000-0000-" + deviceInfo.Id.Split('#')[1].Replace("Dev_", "").Split('_').Last();
        //            if (!uuids.Contains(uuid))
        //            {
        //                uuids.Add(uuid);
        //                deviceInfos.Add(new DeviceInfo(uuid));
        //            }
        //        }
        //    }
        //}

        public DeviceInfo GetDeviceInfoFromUUID(string uuid)
        {
            for (int i = 0; i < deviceInfos.Count; i++)
            {
                if (uuid == deviceInfos[i].Uuid)
                {
                    return deviceInfos[i];
                }
            }
            return null;
        }

        public async void ScanDevices()
        {
            await bleManager.StartScanForDevices();
        }

        public async void ConnectDevices()
        {
            foreach (string uuid in uuids)
            {
                if (GetDeviceInfoFromUUID(uuid).IsSelected)
                {
                    VerisenseBLEDevice device = new VerisenseBLEDevice(uuid, "");
                    // to get the connecting status
                    if (ConnectedDevices.ContainsKey(uuid))
                    {
                        ConnectedDevices.Remove(uuid);
                        ConnectedDevices.Add(uuid, device);
                    }
                    else if (!ConnectedDevices.ContainsKey(uuid))
                    {
                        ConnectedDevices.Add(uuid, device);
                    }
                    device.ShimmerBLEEvent += ShimmerDevice_BLEEvent;
                    bool result = await device.Connect(true);
                    if (result)
                    {
                        Debug.WriteLine("Device Version: " + device.GetProductionConfig().REV_HW_MAJOR + "." + device.GetProductionConfig().REV_HW_MINOR);
                        Debug.WriteLine("Firmware Version: " + device.GetProductionConfig().REV_FW_MAJOR + "." + device.GetProductionConfig().REV_FW_MINOR + "." + device.GetProductionConfig().REV_FW_INTERNAL);
                        if (ConnectedDevices.ContainsKey(uuid))
                        {
                            ConnectedDevices.Remove(uuid);
                            ConnectedDevices.Add(uuid, device);
                        }
                        else if (!ConnectedDevices.ContainsKey(uuid))
                        {
                            ConnectedDevices.Add(uuid, device);
                        }
                        Debug.WriteLine("\nBT state: " + device.GetVerisenseBLEState() + "\nUUID: " + device.Asm_uuid + "\nBattery: " + device.GetStatus().BatteryPercent + "%");
                        ConfigureDevice(device);
                    }
                    else
                    {
                        if (ConnectedDevices.ContainsKey(uuid))
                        {
                            ConnectedDevices.Remove(uuid);
                        }
                        Debug.WriteLine("Failed to connect device! UUID: " + uuid);
                    }
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
            foreach (VerisenseBLEDevice device in ConnectedDevices.Values)
            {
                if (GetDeviceInfoFromUUID(device.Asm_uuid.ToString()).IsSelected)
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
        }

        public async void StartDataSync()
        {
            foreach (VerisenseBLEDevice device in ConnectedDevices.Values)
            {
                if (GetDeviceInfoFromUUID(device.Asm_uuid.ToString()).IsSelected)
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
        }

        private void ShimmerDevice_BLEEvent(object sender, ShimmerBLEEventData e)
        {
            if (e.CurrentEvent == VerisenseBLEEvent.StateChange)
            {
                DeviceInfo deviceInfo = GetDeviceInfoFromUUID(e.ASMID);
                if (deviceInfo != null && ConnectedDevices.ContainsKey(e.ASMID))
                {
                    deviceInfo.Status = ConnectedDevices[e.ASMID].GetVerisenseBLEState().ToString();
                }
            }
            else if (e.CurrentEvent == VerisenseBLEEvent.NewDataPacket)
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
                            GetDeviceInfoFromUUID(e.ASMID).Signals.Add(signal);
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
            else if (e.CurrentEvent == VerisenseBLEEvent.SyncLoggedDataNewPayload)
            {
                string[] words = e.Message.Split('(');
                DeviceInfo deviceInfo = GetDeviceInfoFromUUID(e.ASMID);
                if (deviceInfo != null)
                {
                    deviceInfo.TransferSpeed = words[0];
                    deviceInfo.PayloadIndex = words[1].Remove(words[1].Length - 1);
                    deviceInfo.BinFilePath = ConnectedDevices[e.ASMID].dataFilePath;
                }
            }
        }

        private void BLEManager_BLEEvent(object sender, BLEManagerEvent e)
        {
            if (e.CurrentEvent == BLEManagerEvent.BLEAdapterEvent.ScanCompleted)
            {
                foreach (VerisenseBLEScannedDevice device in bleManager.GetListOfScannedDevices())
                {
                    if (device.Name.Contains("Verisense") && device.IsConnectable && GetDeviceInfoFromUUID(device.Uuid.ToString()) == null)
                    {
                        uuids.Add(device.Uuid.ToString());
                        deviceInfos.Add(new DeviceInfo(device.Uuid.ToString()));
                        GetDeviceInfoFromUUID(device.Uuid.ToString()).IsPaired = device.IsPaired ? "Is Paired" : "Not Paired";
                        if (!ScannedDevices.Contains(device))
                        {
                            ScannedDevices.Add(device);
                        }
                    }
                }
            }
            else if (e.CurrentEvent == BLEManagerEvent.BLEAdapterEvent.DevicePaired)
            {
                VerisenseBLEScannedDevice dev = (VerisenseBLEScannedDevice)e.objMsg;
                if (GetDeviceInfoFromUUID(dev.Uuid.ToString()) != null)
                {
                    GetDeviceInfoFromUUID(dev.Uuid.ToString()).IsPaired = dev.IsPaired ? "Is Paired" : "Not Paired";
                }
            }
            else if (e.CurrentEvent == BLEManagerEvent.BLEAdapterEvent.DeviceDiscovered)
            {
                VerisenseBLEScannedDevice dev = (VerisenseBLEScannedDevice)e.objMsg;
                if (dev.Name.Contains("Verisense") && dev.IsConnectable && GetDeviceInfoFromUUID(dev.Uuid.ToString()) == null)
                {
                    uuids.Add(dev.Uuid.ToString());
                    deviceInfos.Add(new DeviceInfo(dev.Uuid.ToString()));
                    GetDeviceInfoFromUUID(dev.Uuid.ToString()).IsPaired = dev.IsPaired ? "Is Paired" : "Not Paired";
                    if (!ScannedDevices.Contains(dev))
                    {
                        ScannedDevices.Add(dev);
                    }
                }
            }
        }

        public async void StopStreamingDevices()
        {
            foreach (VerisenseBLEDevice device in ConnectedDevices.Values)
            {
                if (GetDeviceInfoFromUUID(device.Asm_uuid.ToString()).IsSelected)
                {
                    foreach (string[] signal in GetDeviceInfoFromUUID(device.Asm_uuid.ToString()).Signals)
                    {
                        PlotManager.RemoveSignalFromPlot(signal);
                    }
                    var result = await device.ExecuteRequest(RequestType.StopStreaming);
                    IsFirstOjcForDevice[device.Asm_uuid.ToString()] = true;
                    Debug.WriteLine("\nUUID: " + device.Asm_uuid + " stop streaming request \nResult: " + result + "\n");
                }
            }
        }

        public async void DisconnectDevices()
        {
            List<string> listOfDisconnectedUuids = new List<string>();
            foreach (VerisenseBLEDevice device in ConnectedDevices.Values)
            {
                if (GetDeviceInfoFromUUID(device.Asm_uuid.ToString()).IsSelected)
                {
                    foreach (string[] signal in GetDeviceInfoFromUUID(device.Asm_uuid.ToString()).Signals)
                    {
                        PlotManager.RemoveSignalFromPlot(signal);
                    }
                    var result = await device.Disconnect();
                    if (result)
                    {
                        listOfDisconnectedUuids.Add(device.Asm_uuid.ToString());
                    }

                    Debug.WriteLine("\nUUID: " + device.Asm_uuid + " attempt disconnect \nResult: " + result + "\nNew BLE Status: " + device.GetVerisenseBLEState());
                }
            }

            //Remove disconnected devices from the Dictionary
            foreach (var uuid in listOfDisconnectedUuids)
            {
                ConnectedDevices.Remove(uuid);
            }
        }

        //------------------------------------------------------------------------------------------

        //GUI Functionality
        private void scanDevicesButton_Clicked(object sender, EventArgs e)
        {
            ScanDevices();
        }

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

        private void syncDevicesButton_Clicked(object sender, EventArgs e)
        {
            StartDataSync();
        }
    }
}