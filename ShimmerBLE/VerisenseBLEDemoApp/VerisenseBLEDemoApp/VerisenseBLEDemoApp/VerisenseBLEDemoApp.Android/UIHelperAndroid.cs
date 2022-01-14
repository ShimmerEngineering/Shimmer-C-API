
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using ShimmerBLEAPI.Services;
using System;
using VerisenseBLEDemoApp.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(UIHelperAndroid))]
namespace VerisenseBLEDemoApp.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@android:style/Theme.Translucent.NoTitleBar")]
    public class UIHelperAndroid : INativeUIService
    {
        public async void Invoke(Action action)
        {
            var handler = new Handler(Looper.MainLooper);
            handler.Post(action);
        }
    }
}
