using ShimmerBLEAPI;
using ShimmerBLEAPI.Devices;
using ShimmerBLEAPI.Models;
using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using shimmer.Sensors;
using shimmer.Communications;
using shimmer.Models;
using ShimmerAPI;
using System.ComponentModel;
using static shimmer.Models.ShimmerBLEEventData;
using System.Collections.ObjectModel;

namespace PasskeyConfigurationApp
{
    public partial class MainPage : ContentPage
    {
        private VerisenseBLEDevice device;
        private VerisenseBLEScannedDevice SelectedDevice;
        IVerisenseBLEManager bleManager = DependencyService.Get<IVerisenseBLEManager>();
        ObservableCollection<VerisenseBLEScannedDevice> ListOfScannedDevices = new ObservableCollection<VerisenseBLEScannedDevice>();

        public MainPage()
        {
            InitializeComponent();

            var service = DependencyService.Get<IVerisenseBLEManager>();
            bleManager.BLEManagerEvent += BLEManager_BLEEvent;
            deviceList.ItemsSource = ListOfScannedDevices;

            List<string> passkeyIds = new List<string>() { "00", "01", "02", "03" };
            passkeyId.ItemsSource = passkeyIds;
            passkeyId.SelectedIndex = 0;
        }

        public async void ScanDevices()
        {
            await bleManager.StartScanForDevices();
        }

        public void OnSelectedItem(object sender, SelectedItemChangedEventArgs e)
        {
            SelectedDevice = (VerisenseBLEScannedDevice)e.SelectedItem;
            deviceName.Text = SelectedDevice.Name;
        }

        public async void ConnectDevices()
        {
            device = new VerisenseBLEDevice(SelectedDevice.Uuid.ToString(), "");
            device.ShimmerBLEEvent += ShimmerDevice_BLEEvent;
            bool result = await device.Connect(true);
            if (result)
            {
                Debug.WriteLine("Device Version: " + device.GetProductionConfig().REV_HW_MAJOR + "." + device.GetProductionConfig().REV_HW_MINOR);
                Debug.WriteLine("Firmware Version: " + device.GetProductionConfig().REV_FW_MAJOR + "." + device.GetProductionConfig().REV_FW_MINOR + "." + device.GetProductionConfig().REV_FW_INTERNAL);
                Debug.WriteLine("\nBT state: " + device.GetVerisenseBLEState() + "\nUUID: " + device.Asm_uuid + "\nBattery: " + device.GetStatus().BatteryPercent + "%");
            }
            else
            {
                Debug.WriteLine("Failed to connect device! UUID: " + SelectedDevice.Uuid.ToString());
            }
        }

        private void ShimmerDevice_BLEEvent(object sender, ShimmerBLEEventData e)
        {
            if (e.CurrentEvent == VerisenseBLEEvent.StateChange)
            {
                deviceState.Text = device.GetVerisenseBLEState().ToString();
            }
        }

        private void BLEManager_BLEEvent(object sender, BLEManagerEvent e)
        {
            if (e.CurrentEvent == BLEManagerEvent.BLEAdapterEvent.ScanCompleted)
            {
                List<VerisenseBLEScannedDevice> devices = new List<VerisenseBLEScannedDevice>();

                foreach (VerisenseBLEScannedDevice device in bleManager.GetListOfScannedDevices())
                {
                    if (device.Name.Contains("Veri") && device.IsConnectable)
                    {
                        //devices.Add(device);
                        //AddOrUpdateDevice(device);
                    }
                }
            }
            else if (e.CurrentEvent == BLEManagerEvent.BLEAdapterEvent.DeviceDiscovered)
            {
                VerisenseBLEScannedDevice dev = (VerisenseBLEScannedDevice)e.objMsg;
                if (dev.IsConnectable && dev.IsPaired)
                {
                    bool added = false;
                    foreach (VerisenseBLEScannedDevice a in ListOfScannedDevices)
                    {
                        if (a.ID == dev.ID)
                        {
                            added = true;
                            break;
                        }
                    }
                    if (!added)
                    {
                        ListOfScannedDevices.Add(dev);
                    }
                }
            }
        }

        public async void DisconnectDevices()
        {
            var result = await device.Disconnect();
            if (result)
            {

            }
            Debug.WriteLine("\nUUID: " + device.Asm_uuid + " attempt disconnect \nResult: " + result + "\nNew BLE Status: " + device.GetVerisenseBLEState());
        }

        public async void WritePasskeyConfigurationButton()
        {
            string errorString = "";
            if(passkey.Text.ToString().Length != 6)
            {
                errorString += "Passkey should have exactly six characters\n";
            }
            if(passkeyId.SelectedItem == null)
            {
                errorString += "Please select a passkey id\n";
            }
            if(deviceAdvertisingNamePrefix.Text.ToString().Length > 32)
            {
                errorString += "Advertising name prefix cannot have more than 32 characters\n";
            }
            if(errorString != "")
            {
                await DisplayAlert("Error!", errorString, "OK");
                return;
            }

            byte[] passkeyIdArray;
            byte[] passkeyArray;
            byte[] advertisingNamePrefixByteArray;
            byte[] prodConfig = new byte[55];
            Array.Copy(device.GetProductionConfigByteArray(), 3, prodConfig, 0, 55);
            if (passkey.Text.ToString().Length == 6 &&
                passkeyId.SelectedItem != null &&
                deviceAdvertisingNamePrefix.Text.ToString().Length <= 32)
            {
                passkeyIdArray = Encoding.UTF8.GetBytes(passkeyId.SelectedItem.ToString());
                for (int i = 0; i < passkeyIdArray.Length; i++)
                {
                    prodConfig.SetValue(passkeyIdArray[i], i + 15);
                }

                passkeyArray = Encoding.UTF8.GetBytes(passkey.Text.ToString());
                for (int i = 0; i < passkeyArray.Length; i++)
                {
                    prodConfig.SetValue(passkeyArray[i], i + 17);
                }

                advertisingNamePrefixByteArray = Encoding.UTF8.GetBytes(deviceAdvertisingNamePrefix.Text.ToString());
                for (int i = 0; i < advertisingNamePrefixByteArray.Length; i++)
                {
                    prodConfig.SetValue(advertisingNamePrefixByteArray[i], i + 23);
                }
                for (int i = advertisingNamePrefixByteArray.Length + 23; i < 55; i++)
                {
                    //set the remaining bytes to 0xFF
                    prodConfig.SetValue((byte)255, i);
                }
                var result = await device.ExecuteRequest(RequestType.WriteProductionConfig, prodConfig);
            }
        }

        public async void ReadPasskeyConfigurationButton()
        {
            var result = await device.ExecuteRequest(RequestType.ReadProductionConfig);
            deviceAdvertisingNamePrefix.Text = ((ProdConfigPayload)result).AdvertisingNamePrefix;
            passkeyId.SelectedItem = ((ProdConfigPayload)result).PasskeyID;
            passkey.Text = ((ProdConfigPayload)result).Passkey;
        }

        //------------------------------------------------------------------------------------------

        //GUI Functionality
        private void scanDevicesButton_Clicked(object sender, EventArgs e)
        {
            ScanDevices();
        }
        
        private void writePasskeyConfigurationButton_Clicked(object sender, EventArgs e)
        {
            WritePasskeyConfigurationButton();
        }

        private void readPasskeyConfigurationButton_Clicked(object sender, EventArgs e)
        {
            ReadPasskeyConfigurationButton();
        }

        private void connectDevicesButton_Clicked(object sender, EventArgs e)
        {
            ConnectDevices();
        }

        private void disconnectDevicesButton_Clicked(object sender, EventArgs e)
        {
            DisconnectDevices();
        }
    }
}