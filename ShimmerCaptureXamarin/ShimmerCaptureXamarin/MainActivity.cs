using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using Java.Util;
using ShimmerAPI;
using System.Collections.Generic;

namespace ShimmerCaptureXamarin
{
    [Activity(Label = "ShimmerCaptureXamarin", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;
        ShimmerAndroidXamarin shimmer;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate { shimmer.StartStreaming(); };

            shimmer = new ShimmerAndroidXamarin("", "00:06:66:79:E4:54");
            shimmer.UICallback += this.HandleEvent;
            shimmer.StartConnectThread();
        }
        public void HandleEvent(object sender, EventArgs args)
        {
            CustomEventArgs eventArgs = (CustomEventArgs)args;
            int indicator = eventArgs.getIndicator();

            switch (indicator)
            {
                case (int)ShimmerBluetooth.ShimmerIdentifier.MSG_IDENTIFIER_DATA_PACKET:
                    ObjectCluster objectCluster = new ObjectCluster((ObjectCluster)eventArgs.getObject());
                    List<Double> data = objectCluster.GetData();
                    List<String> dataNames = objectCluster.GetNames();
                    String result="";
                    String resultNames = "";
                    foreach (Double d in data)
                    {
                        result = d.ToString() + " " + result;
                    }
                    foreach (String s in dataNames)
                    {
                        resultNames = s + " " + resultNames;
                    }
                    System.Console.WriteLine(resultNames);
                    System.Console.WriteLine(result);
                    break;
            }

        }
    }
}

