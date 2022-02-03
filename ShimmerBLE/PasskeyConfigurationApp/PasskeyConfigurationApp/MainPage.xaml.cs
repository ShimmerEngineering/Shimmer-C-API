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
        private ProdConfigPayload prodConfig;
        private bool useAdvance = false;
        IVerisenseBLEManager bleManager = DependencyService.Get<IVerisenseBLEManager>();
        ObservableCollection<VerisenseBLEScannedDevice> ListOfScannedDevices = new ObservableCollection<VerisenseBLEScannedDevice>();

        public MainPage()
        {
            InitializeComponent();

            var service = DependencyService.Get<IVerisenseBLEManager>();
            bleManager.BLEManagerEvent += BLEManager_BLEEvent;
            deviceList.ItemsSource = ListOfScannedDevices;

            if (useAdvance)
            {
                passkeySettings.IsVisible = false;
                passkeySettingsLabel.IsVisible = false;
            }
            else
            {
                deviceAdvertisingNamePrefix.IsEnabled = false;
                passkeyId.IsEnabled = false;
                passkey.IsEnabled = false;
            }
        }

        public async void ScanDevices()
        {
            await bleManager.StartScanForDevices();
        }

        public void PasskeySettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            writePasskeyConfigurationButton.IsEnabled = true;
            switch (passkeySettings.SelectedIndex)
            {
                // no passkey
                case 0:
                    deviceAdvertisingNamePrefix.Text = prodConfig != null ? prodConfig.AdvertisingNamePrefix : "Verisense";
                    passkeyId.Text = "00";
                    passkey.Text = "";
                    break;
                // default passkey
                case 1:
                    deviceAdvertisingNamePrefix.Text = prodConfig != null ? prodConfig.AdvertisingNamePrefix : "Verisense";
                    passkeyId.Text = "01";
                    passkey.Text = "123456";
                    break;
                // clinical trial passkey
                case 2: 
                    deviceAdvertisingNamePrefix.Text = prodConfig != null ? prodConfig.AdvertisingNamePrefix : "Verisense";
                    passkeyId.Text = "";
                    passkey.Text = "";
                    break;
                // custom
                case 3:
                    writePasskeyConfigurationButton.IsEnabled = false;
                    deviceAdvertisingNamePrefix.Text = "";
                    passkeyId.Text = "";
                    passkey.Text = "";
                    break;
                default: break;
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
                prodConfig = device.GetProductionConfig();
                int index = 3;
                if (prodConfig.AdvertisingNamePrefix == "Verisense")
                {
                    if(prodConfig.PasskeyID == "")
                    {
                        index = 2;
                    }
                    else if(prodConfig.PasskeyID == "00")
                    {
                        index = 0;
                    }
                    else if (prodConfig.PasskeyID == "01")
                    {
                        index = 1;
                    }
                }
                else
                {
                    if (prodConfig.PasskeyID == "" || prodConfig.PasskeyID == "00")
                    {
                        index = 0;
                    }
                    else if (prodConfig.PasskeyID == "01")
                    {
                        index = 1;
                    }
                }
                passkeySettings.SelectedIndex = index;

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
                if(device.GetVerisenseBLEState() == ShimmerDeviceBluetoothState.Connected)
                {
                    if (!device.MeetsMinimumFWRequirement(1,2,99)) // check if meets minimum requirement of 1.2.99
                    {
                        DisconnectDevices();
                        DisplayAlert("Error!", "Firmware below 1.02.99 is not supported\nYour device will now be disconnect", "OK");
                    }
                }
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
            if (!useAdvance)
            {
                switch (passkeySettings.SelectedIndex)
                {
                    // no passkey
                    case 0:
                        prodConfig.EnableNoPasskey("");
                        break;
                    // default passkey
                    case 1:
                        prodConfig.EnableDefaultPasskey("", "01");
                        break;
                    // clinical trial passkey
                    case 2:
                        prodConfig.EnableClinicalTrialPasskey();
                        break;
                    // custom
                    case 3:
                        return;
                    default: break;
                }
            }
            else
            {
                try
                {
                    if (String.IsNullOrEmpty(passkey.Text))
                    {
                        prodConfig.EnableNoPasskey(deviceAdvertisingNamePrefix.Text);
                    }
                    else
                    {
                        prodConfig.EnableDefaultPasskey(deviceAdvertisingNamePrefix.Text, passkeyId.Text);
                    }
                } catch (Exception ex)
                {
                    await DisplayAlert("Error!", ex.Message, "OK");
                }
            }
            var result = await device.ExecuteRequest(RequestType.WriteProductionConfig, prodConfig.GetPayload());
            if (result != null)
            {
                await DisplayAlert("Write success!", "Please power cycle, unpair the sensor and pair it again", "OK");
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