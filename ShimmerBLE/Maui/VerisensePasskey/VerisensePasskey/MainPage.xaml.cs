using Amazon.Util;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using shimmer.Communications;
using shimmer.Models;
using ShimmerBLEMauiAPI.Devices;
using System;
using System.Collections.ObjectModel;
using static shimmer.Models.ShimmerBLEEventData;

namespace VerisensePasskey
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private readonly IAdapter _adapter;
        private readonly IBluetoothLE _bluetoothLE;
        private ObservableCollection<IDevice> _deviceList;
        private ProdConfigPayload prodConfig;
        String uuid;
        VerisensePluginBLEDevice device;
        public MainPage()
        {
            InitializeComponent();
            _bluetoothLE = CrossBluetoothLE.Current;
            _adapter = CrossBluetoothLE.Current.Adapter;

            _deviceList = new ObservableCollection<IDevice>();
            DevicesList.ItemsSource = _deviceList;

            _adapter.DeviceDiscovered += OnDeviceDiscovered;

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

            passkeySettings.ItemsSource = new List<string>
            {
                "no passkey",
                "default passkey",
                "clinical trial passkey",
                "custom"
            };

        }
       
        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private async void OnScanClicked(object sender, EventArgs e)
        {
            _deviceList.Clear();
            DeviceStateLabel.Text = "Status: Scanning...";
            if (!_bluetoothLE.IsOn)
            {
                await DisplayAlert("Bluetooth Off", "Please turn on Bluetooth", "OK");
                return;
            }

            if (_adapter.IsScanning)
            {
                await _adapter.StopScanningForDevicesAsync();
            }

            try
            {
                await _adapter.StartScanningForDevicesAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Scan Error", ex.Message, "OK");
            }
        }

        private async void onDisconnectClicked(object sender, EventArgs e)
        {  
                await device.Disconnect();
                DeviceStateLabel.Text = "Status: Disconnected";
        }

        Boolean useAdvance = false;
        private async void writePasskeyConfigurationButton_Clicked(object sender, EventArgs e)
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
                }
                catch (Exception ex)
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

        private async void OnConnectClicked(object sender, EventArgs e)
        {
            device = new VerisensePluginBLEDevice(uuid, "");
            device.ShimmerBLEEvent += ShimmerDevice_BLEEvent;
            bool result = await device.Connect(true);
            
            if (result)
            {
                prodConfig = device.GetProductionConfig();
                DeviceStateLabel.Text = device.GetVerisenseBLEState().ToString() + " " + prodConfig.REV_FW_MAJOR + "." + prodConfig.REV_FW_MINOR + "." + prodConfig.REV_FW_INTERNAL;
                if (!device.MeetsMinimumFWRequirement(1, 2, 99)) // check if meets minimum requirement of 1.2.99
                {
                    DisplayAlert("Error!", "Firmware below 1.02.99 is not supported\nYour device will now be disconnect", "OK");
                }
                int index = 3;
                if (prodConfig.AdvertisingNamePrefix == "Verisense")
                {
                    if (prodConfig.PasskeyID == "")
                    {
                        index = 2;
                    }
                    else if (prodConfig.PasskeyID == "00")
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


            }
        }
        private void PasskeySettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            string selectedPasskeyMode = (string)picker.SelectedItem;

            // Do something with the selected value
            Console.WriteLine("Selected Passkey Mode: " + selectedPasskeyMode);


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
        private void ShimmerDevice_BLEEvent(object sender, ShimmerBLEEventData e)
        {
            if (e.CurrentEvent == VerisenseBLEEvent.StateChange)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    DeviceStateLabel.Text = device.GetVerisenseBLEState().ToString();
                });
                if (device.GetVerisenseBLEState() == ShimmerDeviceBluetoothState.Connected)
                {
                    
                }
            }
        }

        private void OnDeviceDiscovered(object sender, DeviceEventArgs args)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (!_deviceList.Any(d => d.Id == args.Device.Id))
                {
                    _deviceList.Add(args.Device);
                }
            });
        }

        private void DevicesList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is IDevice device)
            {
                // Show UUID (device Id)
                UuidLabel.Text = $"Selected UUID: {device.Id}";
                uuid = device.Id.ToString();
            }

            // Optional: deselect so the same item can be selected again
            DevicesList.SelectedItem = null;
        }

    }

}
