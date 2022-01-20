using ShimmerBLEAPI;
using ShimmerBLEAPI.Devices;
using ShimmerBLEAPI.Models;
using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Essentials;
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
        }

        public async void ScanDevices()
        {
            await bleManager.StartScanForDevices();
        }

        public void IsSetToClinicalTrial_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                deviceAdvertisingNamePrefix.IsEnabled = false;
                passkeyId.IsEnabled = false;
                passkey.IsEnabled = false; 
            }
            else
            {
                deviceAdvertisingNamePrefix.IsEnabled = true;
                passkeyId.IsEnabled = true;
                passkey.IsEnabled = true;
            }
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
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    deviceState.Text = device.GetVerisenseBLEState().ToString();
                });
            }
        }

        private void BLEManager_BLEEvent(object sender, BLEManagerEvent e)
        {
            if (e.CurrentEvent == BLEManagerEvent.BLEAdapterEvent.ScanCompleted)
            {
                foreach (VerisenseBLEScannedDevice device in bleManager.GetListOfScannedDevices())
                {
                    bool added = false;
                    foreach (VerisenseBLEScannedDevice a in ListOfScannedDevices)
                    {
                        if (a.ID == device.ID)
                        {
                            added = true;
                            break;
                        }
                    }
                    if (!added)
                    {
                        ListOfScannedDevices.Add(device);
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
            ProdConfigPayload prodConfig = new ProdConfigPayload(BitConverter.ToString(device.GetProductionConfigByteArray()));
            byte[] prodConfigByteArray = new byte[55];

            if (isSetToClinicalTrial.IsChecked)
            {
                prodConfig.SetPasskey("");
                prodConfig.SetPasskeyID("");
                prodConfig.SetAdvertisingNamePrefix("");
                Array.Copy(prodConfig.GetPayload(), 3, prodConfigByteArray, 0, 55);
                await device.ExecuteRequest(RequestType.WriteProductionConfig, prodConfigByteArray);
            }
            else
            {
                string errorString = "";
                if (passkey.Text.Length != 0)
                {
                    if (passkey.Text.Length != 6)
                    {
                        errorString += "Passkey should have exactly six characters\n";
                    }
                    else if (!int.TryParse(passkey.Text, out _))
                    {
                        errorString += "Passkey should consist of only numerical values\n";
                    }
                }
                if (passkeyId.Text.Length != 2 && passkeyId.Text.Length != 0)
                {
                    errorString += "Passkey ID should have exactly two characters\n";
                }
                if (deviceAdvertisingNamePrefix.Text.ToString().Length > 32)
                {
                    errorString += "Advertising name prefix cannot have more than 32 characters\n";
                }
                if (errorString != "")
                {
                    await DisplayAlert("Error!", errorString, "OK");
                    return;
                }

                prodConfig.SetPasskey(passkey.Text);
                prodConfig.SetPasskeyID(passkeyId.Text);
                prodConfig.SetAdvertisingNamePrefix(deviceAdvertisingNamePrefix.Text);
                Array.Copy(prodConfig.GetPayload(), 3, prodConfigByteArray, 0, 55);
                await device.ExecuteRequest(RequestType.WriteProductionConfig, prodConfigByteArray);
            }
        }

        public async void ReadPasskeyConfigurationButton()
        {
            var result = await device.ExecuteRequest(RequestType.ReadProductionConfig);
            deviceAdvertisingNamePrefix.Text = ((ProdConfigPayload)result).AdvertisingNamePrefix;
            passkeyId.Text = ((ProdConfigPayload)result).PasskeyID;
            passkey.Text = ((ProdConfigPayload)result).Passkey;
        }

        //------------------------------------------------------------------------------------------

        //GUI Functionality
        private void scanDevicesButton_Clicked(object sender, EventArgs e)
        {
            ListOfScannedDevices.Clear();
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