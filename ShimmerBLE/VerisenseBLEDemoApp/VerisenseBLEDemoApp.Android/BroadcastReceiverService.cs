using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using shimmer.Communications;
using ShimmerBLEAPI.Models;
using ShimmerBLEAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VerisenseBLEDemoApp.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(BroadcastReceiverService))]
namespace VerisenseBLEDemoApp.Droid
{
    public class BroadcastReceiverService : IBroadcastReceiverService
    {
        public void SetBroadcastReceiverDevice(VerisenseBLEScannedDevice Device, EventHandler<BLEManagerEvent> BLEManagerEvent)
        {
            MainActivity.BroadcastReceiverPairingRequest.SetScannedDeviceItem(Device, BLEManagerEvent);
            MainActivity.BroadcastReceiverBondStateChanged.SetScannedDeviceItem(Device, BLEManagerEvent);
        }
    }
}