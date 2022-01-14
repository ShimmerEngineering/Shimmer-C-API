using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;
using ShimmerBLEAPI.Android.Communications;
using Android.Bluetooth;
using Android.Content;

namespace VerisenseBLEDemoApp.Droid
{
    [Activity(Label = "VerisenseBLEDemoApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static BroadcastReceiverPairingRequest BroadcastReceiverPairingRequest;
        public static BroadcastReceiverBondStateChanged BroadcastReceiverBondStateChanged;
        public static BluetoothAdapter bluetoothAdapter;

        private readonly string[] Permissions =
        {
            Manifest.Permission.Bluetooth,
            Manifest.Permission.BluetoothAdmin,
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation,
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage
        }; 
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            CheckPermissions();

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
        }
        protected override void OnDestroy()
        {
            if (bluetoothAdapter != null)
            {
                if (bluetoothAdapter.IsDiscovering)
                {
                    bluetoothAdapter.CancelDiscovery();
                }
            }
            try
            {
                UnregisterReceiver(BroadcastReceiverPairingRequest);
                UnregisterReceiver(BroadcastReceiverBondStateChanged);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            base.OnDestroy();
        }
        private void CheckPermissions()
        {
            bool minimumPermissionsGranted = true;

            foreach (string permission in Permissions)
            {
                if (CheckSelfPermission(permission) != Permission.Granted)
                {
                    minimumPermissionsGranted = false;
                }
            }

            if (!minimumPermissionsGranted)
            {
                RequestPermissions(Permissions, 0);
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            LoadApplication(new App());

            bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            BroadcastReceiverPairingRequest = new BroadcastReceiverPairingRequest();
            BroadcastReceiverBondStateChanged = new BroadcastReceiverBondStateChanged();

            IntentFilter filterPairingRequest = new IntentFilter(BluetoothDevice.ActionPairingRequest);
            filterPairingRequest.Priority = (int)Android.Content.IntentFilterPriority.HighPriority;
            RegisterReceiver(BroadcastReceiverPairingRequest, filterPairingRequest);
            IntentFilter filterBondStateChanged = new IntentFilter(BluetoothDevice.ActionBondStateChanged);
            RegisterReceiver(BroadcastReceiverBondStateChanged, filterBondStateChanged);
        }
    }
}