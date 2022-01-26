using ShimmerBLEAPI;
using ShimmerBLEAPI.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Essentials;
using shimmer.Sensors;
using shimmer.Models;
using ShimmerAPI;
using static shimmer.Models.ShimmerBLEEventData;
using static ShimmerBLEAPI.AbstractPlotManager;
using shimmer.Communications;
using System.Threading;
using System.Threading.Tasks;


namespace ConnectionTest
{
    public partial class MainPage : ContentPage
    {
        VerisenseBLEDevice device;
        String uuid = "00000000-0000-0000-0000-d02b463da2bb";
        //String uuid = "00000000-0000-0000-0000-e7ec37a0d234";
        //String uuid = "04514419-5AB1-6EEE-A83D-334220DADE3D";

        private int interval = 1;
        private int successCount = 0;
        private int failureCount = 0;
        private int totalIterationLimit = 5;
        private int currentIteration = 0;
        private int retryCount = 0;
        private int retryCountLimit = 5;
        private int totalRetries = 0;

        private bool isTestStarted = false;
        private Dictionary<int, int> ResultMap = new Dictionary<int, int>(); //-1,0,1 , unknown, fail, pass

        public MainPage()
        {
            InitializeComponent();

            Device.BeginInvokeOnMainThread(() =>
            {
                intervalEntry.IsEnabled = true;
                retryCountLimitEntry.IsEnabled = true;
                totalIterationEntry.IsEnabled = true;
                successCountEntry.IsEnabled = false;
                failureCountEntry.IsEnabled = false;
                retryCountEntry.IsEnabled = false;
                totalRetriesEntry.IsEnabled = false;
                testProgressEntry.IsEnabled = false;
                statusEntry.IsEnabled = false;
                firmwareEntry.IsEnabled = false;
                deviceModelEntry.IsEnabled = false;
                uuidEntry.IsEnabled = false;

                successCountEntry.Text = successCount.ToString();
                failureCountEntry.Text = failureCount.ToString();
                totalRetriesEntry.Text = totalRetries.ToString();
                intervalEntry.Text = interval.ToString();
                totalIterationEntry.Text = totalIterationLimit.ToString();
                retryCountEntry.Text = retryCount.ToString();
                retryCountLimitEntry.Text = retryCountLimit.ToString();
                deviceModelEntry.Text = DeviceInfo.Manufacturer + " " + DeviceInfo.Model;
                uuidEntry.Text = uuid;
            });
        }

        public async void ConnectDevices()
        {
            bool result = await device.Connect(true);
        }

        public async void DisconnectDevices()
        {
            var result = await device.Disconnect();
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
                    successCount += 1;
                    ResultMap[currentIteration] = 1;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        successCountEntry.Text = successCount.ToString();
                        firmwareEntry.Text = device.GetProductionConfig().REV_FW_MAJOR + "." + device.GetProductionConfig().REV_FW_MINOR + "." + device.GetProductionConfig().REV_FW_INTERNAL;
                    });
                    
                    try
                    {
                        await device.Disconnect();
                    }
                    catch (Exception ex)
                    {

                    }

                    if (isTestStarted)
                    {
                        await Task.Delay(interval * 1000);
                        ConnectTask();
                    }
                }
                else if (state == ShimmerDeviceBluetoothState.Disconnected)
                {
                    if (ResultMap[currentIteration] == -1)
                    {
                        if (retryCount < retryCountLimit)
                        {
                            retryCount++;
                            totalRetries++;
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                retryCountEntry.Text = retryCount.ToString();
                                totalRetriesEntry.Text = totalRetries.ToString();
                            });
                            Thread.Sleep(3000);
                            ConnectDevices();
                            Thread.Sleep(500);
                        }
                        else
                        {
                            if (ResultMap[currentIteration] == -1)
                            {
                                ResultMap[currentIteration] = 0;

                                failureCount += 1;
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    failureCountEntry.Text = failureCount.ToString();
                                });
                            }
                            if (isTestStarted && currentIteration < totalIterationLimit)
                            {
                                await Task.Delay(interval * 1000);
                                ConnectTask();
                            }
                        }
                    }
                    else if (state == ShimmerDeviceBluetoothState.Limited)
                    {
                        DisconnectDevices();
                        Thread.Sleep(3000);
                    }
                }
            }
        }


        public async void ConnectTask()
        {
            if (isTestStarted)
            {
                retryCount = 0;
                Device.BeginInvokeOnMainThread(() =>
                {
                    retryCountEntry.Text = retryCount.ToString();
                });

                if (currentIteration >= totalIterationLimit)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        intervalEntry.IsEnabled = true;
                        retryCountLimitEntry.IsEnabled = true;
                        totalIterationEntry.IsEnabled = true;
                    });
                    ResultMap.Clear();
                    isTestStarted = false;
                    return;
                }
                else
                {
                    currentIteration += 1;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        testProgressEntry.Text = currentIteration.ToString() + " of " + totalIterationLimit.ToString();
                    });
                    ResultMap.Add(currentIteration, -1);
                    await device.Connect(true);
                }
            }
        }

        //------------------------------------------------------------------------------------------

        //GUI Functionality

        private async void startTestButton_Clicked(object sender, EventArgs e)
        {
            if (!isTestStarted)
            {
                totalRetries = 0;
                if (device != null)
                {
                    await device.Disconnect();
                }
                else
                {
                    device = new VerisenseBLEDevice(uuid, "");
                    device.ShimmerBLEEvent += ShimmerDevice_BLEEvent;
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    intervalEntry.IsEnabled = false;
                    retryCountLimitEntry.IsEnabled = false;
                    totalIterationEntry.IsEnabled = false;
                });

                totalIterationLimit = Int16.Parse(totalIterationEntry.Text);
                retryCountLimit = Int16.Parse(retryCountLimitEntry.Text);
                currentIteration = 0;
                successCount = 0;
                failureCount = 0;
                Device.BeginInvokeOnMainThread(() =>
                {
                    successCountEntry.Text = successCount.ToString();
                    failureCountEntry.Text = failureCount.ToString();
                    totalRetriesEntry.Text = totalRetries.ToString();
                    testProgressEntry.Text = currentIteration.ToString() + " of " + totalIterationLimit.ToString();
                });
                
                isTestStarted = true;

                //var timer = new Timer(TimerTask, null, 1000, Timeout.Infinite);
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
                    retryCountLimitEntry.IsEnabled = true;
                    totalIterationEntry.IsEnabled = true;
                });
                ResultMap.Clear();
                isTestStarted = false;
            }
        }
    }
}