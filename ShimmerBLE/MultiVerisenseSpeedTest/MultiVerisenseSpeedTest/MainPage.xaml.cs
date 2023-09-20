using shimmer.Communications;
using shimmer.Models;
using shimmer.Services;
using ShimmerBLEAPI.Devices;
using ShimmerBLEAPI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using static shimmer.Models.ShimmerBLEEventData;

namespace MultiVerisenseSpeedTest
{
    public partial class MainPage : ContentPage, IObserver<String>
    {
        VerisenseBLEDevice device;
        IVerisenseBLEManager bleManager = DependencyService.Get<IVerisenseBLEManager>();
        VerisenseBLEScannedDevice selectedDevice;
        ObservableCollection<VerisenseBLEScannedDevice> ListOfScannedDevices = new ObservableCollection<VerisenseBLEScannedDevice>();
        private bool isConnected = false;
        SpeedTestService speedTestService;
        int sensorNumber = 0;
        public MainPage()
        {
            InitializeComponent();
            bleManager.BLEManagerEvent += BLEManager_BLEEvent;
            deviceList.ItemsSource = ListOfScannedDevices;
            Device.BeginInvokeOnMainThread(() =>
            {
                deviceModelEntry.IsEnabled = false;
                uuidEntry.IsEnabled = false;
                deviceModelEntry.Text = DeviceInfo.Manufacturer + " " + DeviceInfo.Model;

            });

        }
        private void BLEManager_BLEEvent(object sender, BLEManagerEvent e)
        {
            if (e.CurrentEvent == BLEManagerEvent.BLEAdapterEvent.ScanCompleted)
            {
                foreach (VerisenseBLEScannedDevice device in bleManager.GetListOfScannedDevices())
                {
                    if (device.IsConnectable)
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
            }
            else if (e.CurrentEvent == BLEManagerEvent.BLEAdapterEvent.DeviceDiscovered)
            {
                VerisenseBLEScannedDevice dev = (VerisenseBLEScannedDevice)e.objMsg;
                if (dev.IsConnectable)
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
        protected async void StopScan()
        {
            bleManager.StopScanForDevices();
        }
        private async void startSpeedTestButton_Clicked(object sender, EventArgs e)
        {
            speedTestService = new SpeedTestService(selectedDevice.Uuid.ToString());
            speedTestService.Subscribe(this);
            await speedTestService.GetKnownDevice();
            if (speedTestService.ConnectedASM != null)
            {
                System.Console.WriteLine("Memory Lookup Execution");
                await speedTestService.ExecuteMemoryLookupTableCommand();
            }
            else
            {
                System.Console.WriteLine("Connect Fail");
            }
        }
        private async void stopSpeedTestButton_Clicked(object sender, EventArgs e)
        {
            if (speedTestService != null)
            {
                speedTestService.Disconnect();
            }
        }
        private async void connectButton_Clicked(object sender, EventArgs e)
        {
            device = new VerisenseBLEDevice(selectedDevice.Uuid.ToString(), "");
            device.ShimmerBLEEvent += ShimmerDevice_BLEEvent;
          
            Device.BeginInvokeOnMainThread(() =>
            {
                
            });

            await device.Connect(true);
        }
        private async void disconnectButton_Clicked(object sender, EventArgs e)
        {
            await device.Disconnect();
        }
        private async void scanDeviceButton_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            //GetSensorNumber(int.Parse(btn.StyleId.Substring(btn.StyleId.IndexOf("_") + 1)));
            
            await bleManager.StartScanForDevices();
        }
        public void OnSelectedItem(object sender, SelectedItemChangedEventArgs e)
        {
            selectedDevice = (VerisenseBLEScannedDevice)e.SelectedItem;
            uuidEntry.Text = selectedDevice.Uuid.ToString();
        }
        public void GetSensorNumber(int number)
        {
            switch (number)
            {
                case 1:
                    sensorNumber = 1;
                    break;
                case 2:
                    sensorNumber = 2;
                    break;
                case 3:
                    sensorNumber = 3;
                    break;
                default:
                    sensorNumber = 0;
                    break;
            }

        }
        private async void ShimmerDevice_BLEEvent(object sender, ShimmerBLEEventData e)
        {
            if (e.CurrentEvent == VerisenseBLEEvent.StateChange)
            {
                var state = device.GetVerisenseBLEState();
                Device.BeginInvokeOnMainThread(() =>
                {
                    //statusEntry.Text = state.ToString();
                });

                if (state == ShimmerDeviceBluetoothState.Connected)
                {
                    isConnected = true;
                    
                }
                else if (state == ShimmerDeviceBluetoothState.Disconnected)
                {
                    
                }
                else if (state == ShimmerDeviceBluetoothState.Limited)
                {
                    await device.Disconnect();
                    Thread.Sleep(3000);
                }
            }
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(string value)
        {
            Trace.WriteLine("Works" + value);
            if (value.Contains("Transfer rate"))
            {
                speedTestService.ExecuteMemoryLookupTableCommand();
                //DeviceMessage = value;
                Device.BeginInvokeOnMainThread(() =>
                {
                    transferRateEntry.Text = value;
                });
            }
        }
    }
}
