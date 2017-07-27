//NOTE: This is only provided as an example and has not been tested extensively
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
        ShimmerLogAndStreamXamarin shimmer;
        TextView tvShimmerState;
        TextView tvAccelX;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);


            tvShimmerState = FindViewById<TextView>(Resource.Id.textViewShimmerState);
            tvAccelX = FindViewById<TextView>(Resource.Id.textViewAccelX);
            // Get our button from the layout resource,
            // and attach an event to it
            Button buttonStart = FindViewById<Button>(Resource.Id.buttonStart);

            buttonStart.Click += delegate { shimmer.StartStreaming(); };

            // Get our button from the layout resource,
            // and attach an event to it
            Button buttonStop = FindViewById<Button>(Resource.Id.buttonStop);

            buttonStop.Click += delegate { shimmer.StopStreaming(); };

            // Get our button from the layout resource,
            // and attach an event to it
            Button buttonConnect = FindViewById<Button>(Resource.Id.buttonConnect);

            buttonConnect.Click += delegate {
                int enabledSensors = ((int)ShimmerBluetooth.SensorBitmapShimmer3.SENSOR_A_ACCEL); // this is to enable Analog Accel also known as low noise accelerometer
                //byte[] defaultECGReg1 = new byte[10] { 0x00, 0xA0, 0x10, 0x40, 0x40, 0x2D, 0x00, 0x00, 0x02, 0x03 }; //see ShimmerBluetooth.SHIMMER3_DEFAULT_ECG_REG1
                //byte[] defaultECGReg2 = new byte[10] { 0x00, 0xA0, 0x10, 0x40, 0x47, 0x00, 0x00, 0x00, 0x02, 0x01 }; //see ShimmerBluetooth.SHIMMER3_DEFAULT_ECG_REG2
                byte[] defaultECGReg1 = ShimmerBluetooth.SHIMMER3_DEFAULT_TEST_REG1; //also see ShimmerBluetooth.SHIMMER3_DEFAULT_ECG_REG1
                byte[] defaultECGReg2 = ShimmerBluetooth.SHIMMER3_DEFAULT_TEST_REG2; //also see ShimmerBluetooth.SHIMMER3_DEFAULT_ECG_REG2
             
                shimmer = new ShimmerLogAndStreamXamarin("ShimmerXamarin", "00:06:66:79:E4:54", 1, 0, 4, enabledSensors, false, false, false, 0, 0, defaultECGReg1, defaultECGReg2, false);
                shimmer.UICallback += this.HandleEvent;
                shimmer.StartConnectThread();
            };

            // Get our button from the layout resource,
            // and attach an event to it
            Button buttonDisconnect = FindViewById<Button>(Resource.Id.buttonDisconnect);

            buttonDisconnect.Click += delegate {
                shimmer.Disconnect();
            };
        }
        public void HandleEvent(object sender, EventArgs args)
        {
            CustomEventArgs eventArgs = (CustomEventArgs)args;
            int indicator = eventArgs.getIndicator();

            switch (indicator)
            {
                case (int)ShimmerBluetooth.ShimmerIdentifier.MSG_IDENTIFIER_STATE_CHANGE:
                    System.Diagnostics.Debug.Write(((ShimmerBluetooth)sender).GetDeviceName() + " State = " + ((ShimmerBluetooth)sender).GetStateString() + System.Environment.NewLine);
                    int state = (int)eventArgs.getObject();
                    if (state == (int)ShimmerBluetooth.SHIMMER_STATE_CONNECTED)
                    {
                        System.Diagnostics.Debug.Write("Connected");
                        RunOnUiThread(() => tvShimmerState.Text = "Shimmer State: Connected");
                    }
                    else if (state == (int)ShimmerBluetooth.SHIMMER_STATE_CONNECTING)
                    {
                        System.Diagnostics.Debug.Write("Connecting");
                        RunOnUiThread(() => tvShimmerState.Text = "Shimmer State: Connecting");
                    }
                    else if (state == (int)ShimmerBluetooth.SHIMMER_STATE_NONE)
                    {
                        System.Diagnostics.Debug.Write("Disconnected");
                        RunOnUiThread(() => tvShimmerState.Text = "Shimmer State: Disconnected");
                    }
                    else if (state == (int)ShimmerBluetooth.SHIMMER_STATE_STREAMING)
                    {
                        System.Diagnostics.Debug.Write("Streaming");
                        RunOnUiThread(() => tvShimmerState.Text = "Shimmer State: Streaming");
                    }
                    break;
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

                    SensorData dataAccelX = objectCluster.GetData(Shimmer3Configuration.SignalNames.LOW_NOISE_ACCELEROMETER_X, "CAL");
                    RunOnUiThread(() => tvAccelX.Text = "AccelX: " + dataAccelX.Data); 
                    break;
            }

        }
    }
}

