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

namespace BluetoothTest
{
    public partial class MainPage : ContentPage
    {
        private Dictionary<int, int> ResultMap = new Dictionary<int, int>(); //-1,0,1 , unknown, fail, pass
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


        public MainPage()
        {
            InitializeComponent();
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
                }
            }
            else if (e.CurrentEvent == VerisenseBLEEvent.NewDataPacket)
            {
                JCWatchEvent ojc = (JCWatchEvent)e.ObjMsg;
                if (ojc.Identifier == JCWatchDeviceConstant.CMD_Get_Address)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        statusEntry.Text = "Connected";
                    });
                    try
                    {
                        await watch.Disconnect();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        private async void connectButton_Clicked(object sender, EventArgs e)
        {
            if (watch != null)
            {
                await watch.Disconnect();
            }
            else
            {
                watch = new JCWatch("00000000-0000-0000-0000-03020205feaa");
                watch.ShimmerBLEEvent += ShimmerDevice_BLEEvent;
                Device.BeginInvokeOnMainThread(() =>
                {
                    uuidEntry.Text = watch.Asm_uuid.ToString();
                });
            }
            await watch.Connect(false);
        }
    }
}
