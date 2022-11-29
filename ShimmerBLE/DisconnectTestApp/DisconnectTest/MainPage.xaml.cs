using ShimmerBLEAPI;
using ShimmerBLEAPI.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Essentials;
using shimmer.Models;
using static shimmer.Models.ShimmerBLEEventData;
using shimmer.Communications;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ShimmerBLEAPI.Models;


namespace DisconnectTest
{
    public partial class MainPage : ContentPage
    {
        VerisenseBLEDevice device;
        IVerisenseBLEManager bleManager = DependencyService.Get<IVerisenseBLEManager>();
        VerisenseBLEScannedDevice selectedDevice;
        ObservableCollection<VerisenseBLEScannedDevice> ListOfScannedDevices = new ObservableCollection<VerisenseBLEScannedDevice>();

        private int interval = 5;
        private int successCount = 0;
        private int failureCount = 0;
        private int totalIterationLimit = 5;
        private int currentIteration = 0;
        private int testType = 0;
        private bool isConnected = false;
        private bool connectFailed = false;

        private List<LogEventData> LogEvents;
        private LogEventData lastDisconnect;

        private bool isTestStarted = false;
        private Dictionary<int, int> ResultMap = new Dictionary<int, int>(); //-1,0,1 , unknown, fail, pass

        public MainPage()
        {
            InitializeComponent();
            bleManager.BLEManagerEvent += BLEManager_BLEEvent;
            deviceList.ItemsSource = ListOfScannedDevices;

            Device.BeginInvokeOnMainThread(() =>
            {
                intervalEntry.IsEnabled = true;
                totalIterationEntry.IsEnabled = true;
                successCountEntry.IsEnabled = false;
                failureCountEntry.IsEnabled = false;
                testProgressEntry.IsEnabled = false;
                statusEntry.IsEnabled = false;
                deviceModelEntry.IsEnabled = false;
                uuidEntry.IsEnabled = false;

                successCountEntry.Text = successCount.ToString();
                failureCountEntry.Text = failureCount.ToString();
                intervalEntry.Text = interval.ToString();
                totalIterationEntry.Text = totalIterationLimit.ToString();
                deviceModelEntry.Text = DeviceInfo.Manufacturer + " " + DeviceInfo.Model;
            });
        }

        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            testType = picker.SelectedIndex;
        }

        public async void ConnectDevices()
        {
            var result = await device.Connect(true);
        }

        public async void DisconnectDevices()
        {
            var result = await device.Disconnect();
        }

        public void OnSelectedItem(object sender, SelectedItemChangedEventArgs e)
        {
            selectedDevice = (VerisenseBLEScannedDevice)e.SelectedItem;
            uuidEntry.Text = selectedDevice.Uuid.ToString();
        }

        private async void ShimmerDevice_BLEEvent(object sender, ShimmerBLEEventData e)
        {
            if (e.CurrentEvent == VerisenseBLEEvent.StateChange)
            {
                var state = device.GetVerisenseBLEState();
                Device.BeginInvokeOnMainThread(() =>
                {
                    statusEntry.Text = state.ToString();
                });

                if (state == ShimmerDeviceBluetoothState.Connected)
                {
                    isConnected = true;
                    try
                    {
                        var eventLogs = await device.ExecuteRequest(RequestType.ReadEventLog);
                        LogEvents = ((LogEventsPayload)eventLogs).LogEvents;
                        LogEvents.Sort((x, y) => x.Timestamp.CompareTo(y.Timestamp));

                        if (lastDisconnect == null)
                        {
                            lastDisconnect = LogEvents.FindLast(a => a.CurrentEvent.Equals(LogEvent.BLE_DISCONNECTED));
                        }
                        else
                        {
                            var temp = LogEvents.FindLast(a => a.CurrentEvent.Equals(LogEvent.BLE_DISCONNECTED));
                            if (lastDisconnect.Timestamp < temp.Timestamp)
                            {
                                // disconnect success
                                lastDisconnect = temp;
                            }
                            else
                            {
                                // disconnect fail
                                throw new Exception("disconnect log event not found");
                            }
                        }
                        switch (testType)
                        {
                            case 0:
                                // disconnect
                                await device.Disconnect();
                                break;
                            case 1:
                                // WriteBytes 2B-00-00
                                await device.ExecuteRequest(RequestType.Disconnect);
                                break;
                            case 2:
                                // disconnect, disconnect
                                await device.Disconnect();
                                await device.Disconnect();
                                break;
                            case 3:
                                // power off, WriteBytes 2B-00-00
                                TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    await DisplayAlert("Alert", "Please power off the device before pressing the OK button", "OK");
                                    await device.ExecuteRequest(RequestType.Disconnect);
                                    tcs.SetResult(true);
                                    await DisplayAlert("Alert", "Please power on the device", "OK");
                                });
                                await tcs.Task;
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    await Task.Delay(4000);
                    if (ResultMap[currentIteration] == -1)
                    {
                        ResultMap[currentIteration] = 0;
                        failureCount += 1;
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            failureCountEntry.Text = failureCount.ToString();
                        });
                        await device.Disconnect();
                    }
                }
                else if (state == ShimmerDeviceBluetoothState.Disconnected)
                {
                    if (isConnected)
                    {
                        successCount += 1;
                        ResultMap[currentIteration] = 1;
                        isConnected = false;

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            successCountEntry.Text = successCount.ToString();
                        });

                        if (isTestStarted)
                        {
                            await Task.Delay(interval * 1000);
                            ConnectTask();
                        }
                    }
                    else
                    {
                        connectFailed = true;
                        await Task.Delay(interval * 1000);
                        ConnectTask();
                    }
                }
                else if (state == ShimmerDeviceBluetoothState.Limited)
                {
                    DisconnectDevices();
                    Thread.Sleep(3000);
                }
            }
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

        public async void ConnectTask()
        {
            if (isTestStarted)
            {
                foreach (string item in await bleManager.GetSystemConnectedOrPairedDevices())
                {
                    System.Console.WriteLine("Device Paired " + item);
                }
                if (currentIteration >= totalIterationLimit)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        intervalEntry.IsEnabled = true;
                        totalIterationEntry.IsEnabled = true;
                    });
                    ResultMap.Clear();
                    isTestStarted = false;
                    return;
                }
                else
                {
                    if (!connectFailed)
                    {
                        currentIteration += 1;
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            testProgressEntry.Text = currentIteration.ToString() + " of " + totalIterationLimit.ToString();
                        });
                        ResultMap.Add(currentIteration, -1);
                    }
                    else
                    {
                        connectFailed = false;
                    }
                    
                    await device.Connect(true);
                }
            }
        }
        public async void ScanDevices()
        {
            await bleManager.StartScanForDevices();
        }

        //------------------------------------------------------------------------------------------

        //GUI Functionality

        private async void startTestButton_Clicked(object sender, EventArgs e)
        {
            if (!isTestStarted)
            {
                if (device != null)
                {
                    await device.Disconnect();
                }
                else
                {
                    device = new VerisenseBLEDevice(selectedDevice.Uuid.ToString(), "");
                    device.ShimmerBLEEvent += ShimmerDevice_BLEEvent;
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    intervalEntry.IsEnabled = false;
                    totalIterationEntry.IsEnabled = false;
                });

                totalIterationLimit = Int16.Parse(totalIterationEntry.Text);
                currentIteration = 0;
                successCount = 0;
                failureCount = 0;
                Device.BeginInvokeOnMainThread(() =>
                {
                    successCountEntry.Text = successCount.ToString();
                    failureCountEntry.Text = failureCount.ToString();
                    testProgressEntry.Text = currentIteration.ToString() + " of " + totalIterationLimit.ToString();
                });

                isTestStarted = true;
                ConnectTask();
            }
        }

        private void stopTestButton_Clicked(object sender, EventArgs e)
        {
            if (isTestStarted)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    intervalEntry.IsEnabled = true;
                    totalIterationEntry.IsEnabled = true;
                });
                ResultMap.Clear();
                isTestStarted = false;
            }
        }

        private void scanDeviceButton_Clicked(object sender, EventArgs e)
        {
            ListOfScannedDevices.Clear();
            ScanDevices();
        }
    }
}