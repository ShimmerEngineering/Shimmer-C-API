using Newtonsoft.Json;
using shimmer.Communications;
using shimmer.Models;
using shimmer.Sensors;
using shimmer.Services;
using ShimmerAPI;
using ShimmerBLEAPI.Devices;
using ShimmerBLEAPI.Models;
using ShimmerBLEAPI.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using static shimmer.Models.ShimmerBLEEventData;
using ShimmerBLEAPI.UWP.Communications;
using VerisenseBLEDemoApp.Advance;
using ShimmerBLEAPI.Communications;

namespace VerisenseBLEDemoApp
{
    public partial class MainPage : ContentPage
    {
        VerisenseBLEDevice verisenseBLEDevice;
        ShimmerDeviceBluetoothState CurrentState;
        ShimmerDeviceBluetoothState StateToContinue;

        IVerisenseBLEManager bleManager = DependencyService.Get<IVerisenseBLEManager>();
        VerisenseAPIDemoSettings verisenseApiDemoSettings = new VerisenseAPIDemoSettings();
        S3CloudManager cloudManager;
        VerisenseBLEScannedDevice DeviceToBePaired;
        public static IBLEPairingKeyGenerator PairingKeyGenerator;

        Logging Logging;
        public double totalPayload { get; set; }
        public static readonly double kBPerPayload = 32.0;
        public int payloadCount { get; set; }
        string uuid;
        bool LogToFile = false;
        bool IsAutoReconnect = false;
        int numLogFilesToKeep = 10;

        string _trialName = "UnknownTrialName";
        public String TrialName
        {
            get
            {
                return _trialName;
            }
            set
            {
                _trialName = value;
                OnPropertyChanged(nameof(TrialName));
            }
        }


        string _participantID = "UnknownParticipantID";
        public String ParticipantID
        {
            get
            {
                return _participantID;
            }
            set
            {
                _participantID = value;
                OnPropertyChanged(nameof(ParticipantID));
            }
        }

        bool _ShowStartDemoButton;
        public bool ShowStartDemoButton
        {
            get => _ShowStartDemoButton;

            set
            {
                if (_ShowStartDemoButton == value)
                    return;

                _ShowStartDemoButton = value;
                OnPropertyChanged(nameof(ShowStartDemoButton));
            }
        }

        bool _ShowStopDemoButton;
        public bool ShowStopDemoButton
        {
            get => _ShowStopDemoButton;

            set
            {
                if (_ShowStopDemoButton == value)
                    return;

                _ShowStopDemoButton = value;
                OnPropertyChanged(nameof(ShowStopDemoButton));
            }
        }


        private void CloudManager_Event(object sender, CloudManagerEvent e)
        {
            if (e.CurrentEvent == CloudManagerEvent.CloudEvent.UploadSuccessful)
            {
                DependencyService.Get<INativeUIService>().Invoke(() =>
                {
                    LBLCloud.Text = "File Uploaded: " + e.message;
                    DemoMode = false;
                    ShowStartDemoButton = true;
                    ShowStopDemoButton = false;
                });
            }else if (e.CurrentEvent == CloudManagerEvent.CloudEvent.UploadProgressUpdate)
            {
                DependencyService.Get<INativeUIService>().Invoke(() => {
                    LBLCloud.Text = "File Upload Progress (%): " + e.message;
                });
            }
            else if (e.CurrentEvent == CloudManagerEvent.CloudEvent.UploadFail)
            {
                DependencyService.Get<INativeUIService>().Invoke(() => {
                    LBLCloud.Text = "File Failed to Upload: " + e.message;
                    DemoMode = false;
                    ShowStartDemoButton = true;
                    ShowStopDemoButton = false;
                });
            }else if (e.CurrentEvent == CloudManagerEvent.CloudEvent.UploadedFileDeleteSuccessful)
            {
                DependencyService.Get<INativeUIService>().Invoke(async () =>
                {
                    LBLCloud.Text = "Uploaded Bin File Deleted From: " + e.message;
                });
            }
            else if (e.CurrentEvent == CloudManagerEvent.CloudEvent.UploadedFileDeleteFail)
            {
                DependencyService.Get<INativeUIService>().Invoke(async () =>
                {
                    LBLCloud.Text = "Uploaded Bin File Delete Failed: " + e.message;
                });
            }

        }
        private void BLEManager_BLEEvent(object sender, BLEManagerEvent e)
        {
            if (e.CurrentEvent == BLEManagerEvent.BLEAdapterEvent.ScanCompleted)
            {
                List<VerisenseBLEScannedDevice> devices = new List<VerisenseBLEScannedDevice>();

                foreach(VerisenseBLEScannedDevice device in bleManager.GetListOfScannedDevices())
                {
                    if (device.Name.Contains("Verisense") && device.IsConnectable)
                    {
                        devices.Add(device); //please note for iOS the bluetooth manager does not know if the device is paired, as result we recommend always running the pairing code before attempting to connect
                    }
                }

                if (devices.Count > 1)
                {
                    int maxRSSI = -100;
                    foreach(VerisenseBLEScannedDevice item in devices)
                    {
                        if (item.RSSI > maxRSSI)
                        {
                            maxRSSI = item.RSSI;
                            DeviceToBePaired = item;
                            LBLScanning.Text = "Device to be paired = " + DeviceToBePaired.Uuid.ToString();

                        }
                    }
                }
                else if(devices.Count == 1)
                {
                    DeviceToBePaired = devices[0];
                    LBLScanning.Text = "Device to be paired = " + DeviceToBePaired.Uuid.ToString();
                } else
                {
                    LBLScanning.Text = "Device to be paired = None";
                }

                if (Device.RuntimePlatform == Device.iOS && DeviceToBePaired != null)
                {
                    PairingKeyGenerator = new VerisenseBLEPairingKeyGenerator();
                    LBLPairingKey.Text = "Pairing Key: " + PairingKeyGenerator.CalculatePairingPin(DeviceToBePaired.Name.ToString());
                }
            }
            else if (e.CurrentEvent == BLEManagerEvent.BLEAdapterEvent.DevicePaired)
            {
                LBLPairing.Text = e.message;
                
                if (verisenseBLEDevice != null)
                {
                    verisenseBLEDevice.ShimmerBLEEvent -= ShimmerDevice_BLEEvent;
                }
                
                uuid = ((VerisenseBLEScannedDevice)e.objMsg).Uuid.ToString();
                verisenseBLEDevice = new VerisenseBLEDevice(((VerisenseBLEScannedDevice)e.objMsg).Uuid.ToString(), ((VerisenseBLEScannedDevice)e.objMsg).Name);
            }
        }

        private void ShimmerDevice_BLEEvent(object sender, ShimmerBLEEventData e)
        {
            if (e.CurrentEvent == VerisenseBLEEvent.NewDataPacket)
            {
                ObjectCluster ojc = ((ObjectCluster)e.ObjMsg);
                //GSR data can be processed here to detect periods of invalid GSR data (e.g. leads not connected)

                if (ojc.GetNames().Contains(SensorLIS2DW12.ObjectClusterSensorName.LIS2DW12_ACC_X))
                {
                    if (Logging == null && LogToFile)
                    {
                        var folder = Path.Combine(DependencyService.Get<ILocalFolderService>().GetAppLocalFolder());
                        if (!Directory.Exists(folder))
                        {
                            Directory.CreateDirectory(folder);
                        }
                        DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                        double time = (DateTime.UtcNow - UnixEpoch).TotalMilliseconds;
                        Logging = new Logging(Path.Combine(folder, time.ToString() + "SensorLIS2DW12.csv"), ",");
                    }

                    if (LogToFile)
                    {
                        Logging.WriteData(ojc);
                    }

                    var a2x = ojc.GetData(SensorLIS2DW12.ObjectClusterSensorName.LIS2DW12_ACC_X, ShimmerConfiguration.SignalFormats.CAL);
                    var a2y = ojc.GetData(SensorLIS2DW12.ObjectClusterSensorName.LIS2DW12_ACC_Y, ShimmerConfiguration.SignalFormats.CAL);
                    var a2z = ojc.GetData(SensorLIS2DW12.ObjectClusterSensorName.LIS2DW12_ACC_Z, ShimmerConfiguration.SignalFormats.CAL);
                    var ts = ojc.GetData(ShimmerConfiguration.SignalNames.SYSTEM_TIMESTAMP, ShimmerConfiguration.SignalFormats.CAL);
                    Debug.WriteLine("Sensor Accel Timestamp: " + ts.Data);
                    System.Console.WriteLine("New Data Packet: " + "  X : " + a2x.Data + "  Y : " + a2y.Data + "  Z : " + a2z.Data);
                    DependencyService.Get<INativeUIService>().Invoke(() => {
                        LBLStreaming.Text = SensorLIS2DW12.SensorName + " New Data Packet: " + "  X : " + Math.Round(a2x.Data, 2) + "  Y : " + Math.Round(a2y.Data, 2) + "  Z : " + Math.Round(a2z.Data, 2);
                    }
               );
                }
                if (ojc.GetNames().Contains(SensorGSR.ObjectClusterSensorName.GSR))
                {
                    var gsr = ojc.GetData(SensorGSR.ObjectClusterSensorName.GSR, ShimmerConfiguration.SignalFormats.CAL, ShimmerConfiguration.SignalUnits.MicroSiemens);
                    var ts = ojc.GetData(ShimmerConfiguration.SignalNames.SYSTEM_TIMESTAMP, ShimmerConfiguration.SignalFormats.CAL);
                    Debug.WriteLine("Sensor GSR Timestamp: " + ts.Data);
                    DependencyService.Get<INativeUIService>().Invoke(() =>
                    {
                        LBLStreaming.Text = SensorGSR.SensorName + " New Data Packet: GSR (" + ShimmerConfiguration.SignalUnits.MicroSiemens + ") : " + gsr.Data + " ;  GSR (Connectivity) : " + ((SensorGSR)verisenseBLEDevice.GetSensor(SensorGSR.SensorName)).GetGSRConnectivityLevel().ToString();
                    });
                }

            }
            else if (e.CurrentEvent == VerisenseBLEEvent.StateChange)
            {
                DependencyService.Get<INativeUIService>().Invoke(() => {
                    LBLDeviceState.Text = verisenseBLEDevice.Asm_uuid.ToString() + " Bluetooth State: " + verisenseBLEDevice.GetVerisenseBLEState().ToString();

                    if (verisenseBLEDevice.GetVerisenseBLEState().Equals(ShimmerDeviceBluetoothState.Connected) && ReconnectTimer==null) //null means its not trying to reconnect
                    {
                        StateToContinue = ShimmerDeviceBluetoothState.Connected;
                    }

                    ShimmerDeviceBluetoothState previousState = CurrentState;
                    CurrentState = verisenseBLEDevice.GetVerisenseBLEState();
                    if ((previousState.Equals(ShimmerDeviceBluetoothState.StreamingLoggedData) || previousState.Equals(ShimmerDeviceBluetoothState.Streaming))
                    && CurrentState.Equals(ShimmerDeviceBluetoothState.Disconnected)) //if it went from a streaming state to disconnect, we want to remember what the previous state was so can execute it upon reconnect
                    {
                        StateToContinue = previousState;
                    }

                    if ((previousState.Equals(ShimmerDeviceBluetoothState.Connected) || previousState.Equals(ShimmerDeviceBluetoothState.Streaming) || previousState.Equals(ShimmerDeviceBluetoothState.StreamingLoggedData) || previousState.Equals(ShimmerDeviceBluetoothState.Connecting)) &&
                        verisenseBLEDevice.GetVerisenseBLEState().Equals(ShimmerDeviceBluetoothState.Disconnected) && !DisconnectPressed && IsAutoReconnect)
                    {
                        ReconnectTimer = new System.Threading.Timer(Reconnect, null, 5000, Timeout.Infinite);
                    }

                    DisconnectPressed = false;
                }
               );
            }
            else if (e.CurrentEvent == VerisenseBLEEvent.SyncLoggedDataNewPayload)
            {
                payloadCount++;
                double dataTransferPercent = GetPayloadTransferProgress(payloadCount - 1);
                DependencyService.Get<INativeUIService>().Invoke(() => {
                    LBLTransferLoggedData.Text = "Data Transfer " + dataTransferPercent + "% " + e.CurrentEvent.ToString() + ": " + e.Message;
                }
               );
            }
            else if (e.CurrentEvent == VerisenseBLEEvent.SyncLoggedDataComplete)
            {

                double dataTransferPercent = GetPayloadTransferProgress(payloadCount);
                DependencyService.Get<INativeUIService>().Invoke(async () => {
                    LBLTransferLoggedData.Text = "Data Transfer " + dataTransferPercent + "% " + e.CurrentEvent.ToString();
                    if (DemoMode) // if in demo mode straight away upload the file
                    {
                        try
                        {
                            InitializeCloudManager();
                            var pathToUpload = Path.Combine(DependencyService.Get<ILocalFolderService>().GetAppLocalFolder(), verisenseBLEDevice.binFileFolderDir);
                            var result = await cloudManager.UploadFile(pathToUpload);
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Error", e.Message, "OK");
                        }
                    }
                }
              );
            }
            else if (e.CurrentEvent == VerisenseBLEEvent.RequestResponse)
            {
                if ((RequestType)e.ObjMsg == RequestType.ReadStatus)
                {
                    DependencyService.Get<INativeUIService>().Invoke(() => {
                        LBLStatusConfig.Text = "Status Received (Batt Percentage): " + verisenseBLEDevice.GetStatus().BatteryPercent.ToString() + "%";
                    }
                    );
                }
                if ((RequestType)e.ObjMsg == RequestType.ReadProductionConfig)
                {
                    DependencyService.Get<INativeUIService>().Invoke(() => {
                        LBLProdConfig.Text = "Firmare Version: v" + verisenseBLEDevice.GetProductionConfig().REV_FW_MAJOR + "." + verisenseBLEDevice.GetProductionConfig().REV_FW_MINOR + "." + verisenseBLEDevice.GetProductionConfig().REV_FW_INTERNAL + " Hardware Version: v" + verisenseBLEDevice.GetProductionConfig().REV_HW_MAJOR + "." + verisenseBLEDevice.GetProductionConfig().REV_HW_MINOR;
                    }
                    );
                }
            }
            else if (e.CurrentEvent == VerisenseBLEEvent.RequestResponseFail)
            {
                if (e.ObjMsg is RequestType)
                {
                    DependencyService.Get<INativeUIService>().Invoke(() => {
                        LBLStatusConfig.Text = "Response Failed: " + ((RequestType)e.ObjMsg).ToString();
                    }
                   );
                }
            }
            else if (e.CurrentEvent == VerisenseBLEEvent.WriteResponse)
            {
                if ((RequestType)e.ObjMsg == RequestType.WriteOperationalConfig)
                {
                    DependencyService.Get<INativeUIService>().Invoke(() => {
                        LBLOpConfig.Text = "Operational Config Written Successfully";
                    }
                  );
                }
            }
            else if (e.CurrentEvent == VerisenseBLEEvent.DataStreamCRCFail)
            {
                DependencyService.Get<INativeUIService>().Invoke(() =>
                {
                    LBLStreaming.Text = "Payload CRC Check Failed";
                }
              );
            }
        }

        public class VerisenseAPIDemoSettings
        {
            public S3CloudInfo S3CloudInfo { get; set; }
            public string FWVersion { get; set; }
        }
        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            ShowStopDemoButton = false;
            ShowStartDemoButton = true;
            LBLDeviceState.Text = "Device State";
            GetDemoSettingsJsonFile();
            bleManager.BLEManagerEvent += BLEManager_BLEEvent;
            try
            {

                string logFilePath = Path.Combine(DependencyService.Get<ILocalFolderService>().GetAppLocalFolder(), "AppLogs");
                if (!Directory.Exists(logFilePath))
                {
                    Directory.CreateDirectory(logFilePath);
                }
                var currentLogfiles = new DirectoryInfo(logFilePath).GetFiles().OrderBy(f => f.LastWriteTime).ToArray();
                if (currentLogfiles.Length >= numLogFilesToKeep)
                {
                    //Sort files by newest first
                    Array.Reverse(currentLogfiles);
                    for (int i = numLogFilesToKeep - 1; i < currentLogfiles.Length; i++)
                    {
                        try
                        {
                            File.Delete(currentLogfiles[i].FullName);
                        }
                        catch (IOException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }

                }
                DependencyService.Get<INativeUIService>().Invoke(() => {
                    try
                    {
                        string pathlog = Path.Combine(logFilePath, DateTime.Now.ToString("yyMMdd_HHmmss") + "_debug.txt");
                        FileStream filestream = new FileStream(pathlog, FileMode.Create);
                        var streamwriter = new StreamWriter(filestream);
                        streamwriter.AutoFlush = true;
                        Console.SetOut(streamwriter);
                        //Console.SetError(streamwriter);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine();
                    }
                });

            }catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async void GetDemoSettingsJsonFile()
        {
            try
            {
                var demoSettingsFilePath = Path.Combine(DependencyService.Get<ILocalFolderService>().GetAppLocalFolder(), "VerisenseAPIDemoSettings.json");
                if (!File.Exists(demoSettingsFilePath))
                {
                    await DisplayAlert("Error", "No VerisenseAPIDemoSettings.json file found. For further details on the json file please refer to ReadMe. Please put json file in " + DependencyService.Get<ILocalFolderService>().GetAppLocalFolder(), "OK and Close the App");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    return;
                }
                StreamReader r = new StreamReader(demoSettingsFilePath);
                string jsonString = r.ReadToEnd();
                verisenseApiDemoSettings = JsonConvert.DeserializeObject<VerisenseAPIDemoSettings>(jsonString);
            }
            catch (Exception e)
            {
                if(e.Message.Contains("Access to the path") && e.Message.Contains("is denied"))
                {
                    await DisplayAlert("Error", "Access to VerisenseAPIDemoSettings file in local folder is denied. Please allow access to files and media on your device in the app settings and restart.", "OK and Close the App");
                }
                else
                {
                    await DisplayAlert("Error", e.Message, "OK and Close the App");
                }

                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

        }

        public void InitializeCloudManager()
        {
            S3CloudInfo s3CloudInfo = new S3CloudInfo
            {
                S3AccessKey = verisenseApiDemoSettings.S3CloudInfo.S3AccessKey,
                S3SecretKey = verisenseApiDemoSettings.S3CloudInfo.S3SecretKey,
                S3RegionName = verisenseApiDemoSettings.S3CloudInfo.S3RegionName,
                S3BucketName = verisenseApiDemoSettings.S3CloudInfo.S3BucketName,
                S3SubFolder = verisenseBLEDevice.binFileFolderDir
            };

            cloudManager = new S3CloudManager(s3CloudInfo);
            cloudManager.CloudManagerEvent += CloudManager_Event;
            cloudManager.DeleteAfterUpload = true;
        }

        bool DisconnectPressed = false;
        async void OnButtonClickedDisconnect(object sender, EventArgs args)
        {
            DisconnectPressed = true;
            await verisenseBLEDevice.Disconnect();
        }

        protected Boolean TryingToReconnect = false;
        System.Threading.Timer ReconnectTimer = null;
        protected async void Reconnect(Object obj)
        {
            if (verisenseBLEDevice.GetVerisenseBLEState().Equals(ShimmerDeviceBluetoothState.Disconnected))
            {
                try
                {
                    verisenseBLEDevice.ShimmerBLEEvent -= ShimmerDevice_BLEEvent;
                    verisenseBLEDevice = null;
                    verisenseBLEDevice = new VerisenseBLEDevice(uuid, "DemoSensor");
                    verisenseBLEDevice.ShimmerBLEEvent += ShimmerDevice_BLEEvent;
                    var result = await verisenseBLEDevice.Connect(true);
                    if (result)
                    {
                        if (StateToContinue.Equals(ShimmerDeviceBluetoothState.Streaming))
                        {
                            verisenseBLEDevice.ExecuteRequest(RequestType.StartStreaming);
                        }
                        else if (StateToContinue.Equals(ShimmerDeviceBluetoothState.StreamingLoggedData))
                        {
                            verisenseBLEDevice.ExecuteRequest(RequestType.TransferLoggedData);
                        }
                        ReconnectTimer = null;
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
        }

        void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            IsAutoReconnect = e.Value;
        }
        async void OnButtonClickedConnect(object sender, EventArgs args)
        {
            if (verisenseBLEDevice == null)
            {
                //Comment this back in if you want to test without scanning and pairing
                //uuid = "00000000-0000-0000-0000-daa619f04ad7";
                //verisenseBLEDevice = new VerisenseBLEDevice(uuid, "DemoSensor");
                //00000000-0000-0000-0000-e2870abb7c82
                //verisenseBLEDevice = new VerisenseBLEDevice("00000000-0000-0000-0000-c96117537402", "Verisense-190925017402");
                //verisenseBLEDevice = new VerisenseBLEDevice("def7b570-bb64-5167-aa2c-76f634454258", "Verisense-7402");

                LBLDeviceState.Text = "Please scan and pair a device";
                return;

            }
            verisenseBLEDevice.ShimmerBLEEvent -= ShimmerDevice_BLEEvent;

            verisenseBLEDevice.SetParticipantID(ParticipantID);
            verisenseBLEDevice.SetTrialName(TrialName);

            verisenseBLEDevice.ShimmerBLEEvent += ShimmerDevice_BLEEvent;

            await verisenseBLEDevice.Connect(true);
        }
        private bool DemoMode = false;

        private async Task<bool> CheckStopDemo()
        {
            if (StopDemo)
            {
                DemoMode = false;
                ShowStartDemoButton = true;
                ShowStopDemoButton = false;
                if (verisenseBLEDevice != null)
                {
                    if (!verisenseBLEDevice.GetVerisenseBLEState().Equals(ShimmerDeviceBluetoothState.Disconnected))
                    {
                        await verisenseBLEDevice.Disconnect();
                    }
                }
                return true;
            }
            return false;
        }
        async void OnButtonClickedStartDemo(object sender, EventArgs args)
        {
            DemoMode = true;
            ShowStartDemoButton = false;
            ShowStopDemoButton = true;
            StopDemo = false;
            DependencyService.Get<INativeUIService>().Invoke(() => {
                LBLScanning.Text = "Start Scanning...";
            }
            );
            await bleManager.StartScanForDevices();
            if (await CheckStopDemo())
            {
                return;
            }
            await Task.Delay(2000);
            if (DeviceToBePaired != null)
            {
                if (DependencyService.Get<IBroadcastReceiverService>() != null) //skip for UWP and iOS
                {
                    DependencyService.Get<IBroadcastReceiverService>().SetBroadcastReceiverDevice(DeviceToBePaired, bleManager.GetBLEManagerEvent());
                }

                var result = await bleManager.PairVerisenseDevice(DeviceToBePaired, new VerisenseBLEPairingKeyGenerator());
                if (!result)
                {
                    LBLPairing.Text = "Device Failed To Pair";
                    return;
                }
                
                if (await CheckStopDemo())
                {
                    return;
                }
                await Task.Delay(2000);

                verisenseBLEDevice.ShimmerBLEEvent += ShimmerDevice_BLEEvent;
                verisenseBLEDevice.SetParticipantID(ParticipantID);
                verisenseBLEDevice.SetTrialName(TrialName);
                await verisenseBLEDevice.Connect(true);
                if (await CheckStopDemo())
                {
                    return;
                }
                await Task.Delay(2000);
                ConfigureDevice();
                if (await CheckStopDemo())
                {
                    return;
                }
                await Task.Delay(2000); //some delay just to make the demo clearer
                await verisenseBLEDevice.ExecuteRequest(RequestType.StartStreaming);
                if (await CheckStopDemo())
                {
                    return;
                }
                await Task.Delay(10000);
                await verisenseBLEDevice.ExecuteRequest(RequestType.StopStreaming);
                if (await CheckStopDemo())
                {
                    return;
                }
                await Task.Delay(2000); //some delay just to make the demo clearer
                StatusPayload status = (StatusPayload)await verisenseBLEDevice.ExecuteRequest(RequestType.ReadStatus);
                if (status != null)
                {
                    totalPayload = Math.Ceiling((double)status.StorageFull / kBPerPayload);
                }
                payloadCount = 0;
                await verisenseBLEDevice.ExecuteRequest(RequestType.TransferLoggedData);
                if (await CheckStopDemo())
                {
                    return;
                }
                await Task.Delay(2000); //some delay just to make the demo clearer
                                        //await cloudManager.UploadFile(verisenseBLEDevice.binFileFolder); // move this to on success
                                        //await Task.Delay(2000); //some delay just to make the demo clearer
                await verisenseBLEDevice.Disconnect();
                if (await CheckStopDemo())
                {
                    return;
                }
                //await cloudManager.UploadFile("0101.bin");
            } else
            {
                DemoMode = false;
                ShowStartDemoButton = true;
                ShowStopDemoButton = false;
            }

        }
        public double GetPayloadTransferProgress(int uploadedPayloadCount)
        {
            double dataTransferPercent = Math.Round((uploadedPayloadCount/totalPayload)*100);
            return dataTransferPercent;
        }
        bool StopDemo = false;
        async void OnButtonClickedStopDemo(object sender, EventArgs args)
        {
            StopDemo = true;
        }

        async void OnButtonClickedReadProdConf(object sender, EventArgs args)
        {
            ProdConfigPayload result = (ProdConfigPayload)await verisenseBLEDevice.ExecuteRequest(RequestType.ReadProductionConfig);
            LBLProdConfig.Text = "Firmare Version: v" + result.REV_FW_MAJOR + "." + result.REV_FW_MINOR + " Hardware Version: v" + result.REV_HW_MAJOR + "." + result.REV_HW_MINOR;
        }
        async void OnButtonClickedReadStatusConf(object sender, EventArgs args)
        {
            verisenseBLEDevice.ExecuteRequest(RequestType.ReadStatus);
        }

        public async void ConfigureDevice()
        {

            try {
                if (verisenseBLEDevice.GetProductionConfig().REV_HW_MAJOR == (int)VerisenseDevice.HardwareIdentifier.VERISENSE_IMU_01)
                {
                    //create a clone, new settings are applied to the clone
                    VerisenseBLEDevice cloneDevice = new VerisenseBLEDevice(verisenseBLEDevice);
                    var sensor = cloneDevice.GetSensor(SensorLIS2DW12.SensorName);
                    ((SensorLIS2DW12)sensor).SetAccelEnabled(true);
                    ((SensorLIS2DW12)sensor).SetSamplingRate(SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_25Hz);
                    sensor = cloneDevice.GetSensor(SensorGSR.SensorName);
                    ((SensorGSR)sensor).SetGSREnabled(false);
                    ((SensorGSR)sensor).SetBattEnabled(false);
                    sensor = cloneDevice.GetSensor(SensorLSM6DS3.SensorName);
                    ((SensorLSM6DS3)sensor).SetAccelEnabled(false);
                    ((SensorLSM6DS3)sensor).SetGyroEnabled(false);
                    //once the clone is updated with the new settings we generate the new configuration bytes to be sent over Bluetooth
                    byte[] opconfigBytes = cloneDevice.GenerateConfigurationBytes();
                    verisenseBLEDevice.ExecuteRequest(RequestType.WriteOperationalConfig, opconfigBytes);
                } else if (verisenseBLEDevice.GetProductionConfig().REV_HW_MAJOR == (int)VerisenseDevice.HardwareIdentifier.VERISENSE_PULSE_PLUS)
                {
                    verisenseBLEDevice.ExecuteRequest(RequestType.WriteOperationalConfig, VerisenseBLEDevice.DefaultVerisenseConfiguration.GSR_Batt_Default_Setting.GetOperationalConfigurationBytes());
                } else if (verisenseBLEDevice.GetProductionConfig().REV_HW_MAJOR == (int)VerisenseDevice.HardwareIdentifier.VERISENSE_GSR_PLUS)
                {
                    await verisenseBLEDevice.ExecuteRequest(RequestType.WriteOperationalConfig, VerisenseBLEDevice.DefaultVerisenseConfiguration.GSR_Batt_Default_Setting.GetOperationalConfigurationBytes());
                    /*//Comment out below to enable accelerometer and set GSR to 128 Hz
                    VerisenseBLEDevice cloneDevice = new VerisenseBLEDevice(verisenseBLEDevice);
                    var gsrSensor = cloneDevice.GetSensor(SensorGSR.SensorName);
                    var sR = gsrSensor.GetSamplingRate();
                    System.Console.WriteLine("GSR Sampling Rate: " + sR);
                    ((SensorGSR)gsrSensor).SetSamplingRate(SensorGSR.GSRRate.Freq_128Hz);
                    var accelSensor = cloneDevice.GetSensor(SensorLIS2DW12.SensorName);
                    ((SensorLIS2DW12)accelSensor).SetAccelEnabled(true);
                    ((SensorLIS2DW12)accelSensor).SetAccelRange(SensorLIS2DW12.AccelRange.Range_2G);
                    ((SensorLIS2DW12)accelSensor).SetSamplingRate(SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_12_5Hz);
                    byte[] opconfigBytes = cloneDevice.GenerateConfigurationBytes();
                    await verisenseBLEDevice.ExecuteRequest(RequestType.WriteOperationalConfig, opconfigBytes);
                    */

                }
            } catch (Exception ex) {
                System.Console.WriteLine(ex);
            }

        }

        async void OnButtonClickedConfigure(object sender, EventArgs args)
        {
            ConfigureDevice();
        }
        async void OnButtonClickedTransferLoggedData(object sender, EventArgs args)
        {
            StatusPayload status = (StatusPayload)await verisenseBLEDevice.ExecuteRequest(RequestType.ReadStatus);
            if (status != null)
            {
                totalPayload = Math.Ceiling((double)status.StorageFull / kBPerPayload);
            }
            payloadCount = 0;
            await verisenseBLEDevice.ExecuteRequest(RequestType.TransferLoggedData);
        }
        async void OnButtonClickedScan(object sender, EventArgs args)
        {
            DependencyService.Get<INativeUIService>().Invoke(() => {
                LBLScanning.Text = "Start Scanning...";
            }
            );
            await bleManager.StartScanForDevices();
        }
        async void OnButtonClickedPair(object sender, EventArgs args)
        {
            if (DeviceToBePaired != null)
            {
                if (verisenseBLEDevice != null)
                {
                    verisenseBLEDevice.ShimmerBLEEvent -= ShimmerDevice_BLEEvent;
                }
                if (DependencyService.Get<IBroadcastReceiverService>() != null) //skip for UWP and iOS
                {
                    DependencyService.Get<IBroadcastReceiverService>().SetBroadcastReceiverDevice(DeviceToBePaired, bleManager.GetBLEManagerEvent());
                }

                LBLPairing.Text = "Pairing...";
                var result = await bleManager.PairVerisenseDevice(DeviceToBePaired, new VerisenseBLEPairingKeyGenerator());
                if (!result)
                {
                    LBLPairing.Text = "Device Failed To Pair";
                }
            }
        }
        async void OnButtonClickedUpload(object sender, EventArgs args)
        {
            //string localFolder = ApplicationData.Current.LocalFolder.Path + "\\testfolderupload";

            try
            {
                InitializeCloudManager();
                var pathToUpload = Path.Combine(DependencyService.Get<ILocalFolderService>().GetAppLocalFolder(), verisenseBLEDevice.binFileFolderDir);
                var result = await cloudManager.UploadFile(pathToUpload);

            }catch(Exception e)
            {
                await DisplayAlert("Error", e.Message, "OK");
            }
        }
        async void OnButtonClickedStartStreaming(object sender, EventArgs args)
        {
            await verisenseBLEDevice.ExecuteRequest(RequestType.StartStreaming);
        }
        async void OnButtonClickedStopStreaming(object sender, EventArgs args)
        {
            await verisenseBLEDevice.ExecuteRequest(RequestType.StopStreaming);
        }
    }
}
