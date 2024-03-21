using System;

using Android;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace ConnectionTest.Droid
{
    [Activity(Label = "ConnectionTest", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private readonly string[] Permissions =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation,
        };
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            CheckPermissions();

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            OxyPlot.Xamarin.Forms.Platform.Android.PlotViewRenderer.Init();
            LoadApplication(new App());
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
        }
    }
}