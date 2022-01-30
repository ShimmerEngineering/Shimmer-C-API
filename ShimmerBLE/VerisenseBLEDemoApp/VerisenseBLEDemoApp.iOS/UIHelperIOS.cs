using Foundation;
using ShimmerBLEAPI.Services;
using System;
using UIKit;
using VerisenseBLEDemoApp.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(UIHelperIOS))]
namespace VerisenseBLEDemoApp.iOS
{
    public class UIHelperIOS : INativeUIService
    {
        public void Invoke(Action action)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() => { action(); });

        }
    }
}