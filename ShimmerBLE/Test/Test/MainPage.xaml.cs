using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using JointCorpWatch;
using System.Diagnostics;
using Xamarin.Essentials;
using System.Threading;
using shimmer.Communications;
using shimmer.Models;
using static shimmer.Models.ShimmerBLEEventData;

namespace Test
{
    public partial class MainPage : ContentPage
    {
        JCWatch watch;
        private int interval = 5;
        private int successCount = 0;
        private int failureCount = 0;
        private int totalIterationLimit = 5;
        private int currentIteration = 0;
        private int retryCount = 0;
        private int retryCountLimit = 5;
        private int totalRetries = 0;
        private bool isTestStarted = false;
        private bool autoconnect = false;
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
            });
        }

        protected async void ShimmerDevice_BLEEvent(object sender, ShimmerBLEEventData e)
        {
            if (e.CurrentEvent == VerisenseBLEEvent.StateChange)
            {
                var state = watch.GetVerisenseBLEState();
                if (state == ShimmerDeviceBluetoothState.Disconnected)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        statusEntry.Text = "Disconnected";
                        Console.WriteLine("Disconnected");
                    });
                    if (ResultMap[currentIteration] == -1)
                    {
                        if (retryCount < retryCountLimit)
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                retryCount++;
                                totalRetries++;
                                retryCountEntry.Text = retryCount.ToString();
                                totalRetriesEntry.Text = totalRetries.ToString();
                                Thread.Sleep(3000);
                                await watch.Connect(autoconnect);
                                Thread.Sleep(500);
                            });
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
                }
            }
            else if (e.CurrentEvent == VerisenseBLEEvent.NewDataPacket)
            {
                JCWatchEvent ojc = (JCWatchEvent)e.ObjMsg;
                if (ojc.Identifier == JCWatchDeviceConstant.CMD_Get_Address)
                {
                    successCount += 1;
                    ResultMap[currentIteration] = 1;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        successCountEntry.Text = successCount.ToString();
                        statusEntry.Text = "Connected";
                    });
                    try
                    {
                        await watch.Disconnect();
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
                    await watch.Connect(autoconnect);
                }
            }
        }

        private async void startTestButton_Clicked(object sender, EventArgs e)
        {
            if (!isTestStarted)
            {
                totalRetries = 0;
                if (watch != null)
                {
                    await watch.Disconnect();
                }
                else
                {
                    //watch = new JCWatch("00000000-0000-0000-0000-03020205fbb0");
                    watch = new JCWatch("00000000-0000-0000-0000-03020205feaa");
                    watch.ShimmerBLEEvent += ShimmerDevice_BLEEvent;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        uuidEntry.Text = watch.Asm_uuid.ToString();
                    });
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
                ConnectTask();
            }
        }

        private void stopTestButton_Clicked(object sender, EventArgs e)
        {
            if (isTestStarted)
            {
                watch.CancelConnect();
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

        void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            autoconnect = e.Value;
        }
    }
}
