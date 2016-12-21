using ShimmerAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShimmerConsoleAppExample
{
    class Program
    {
        ShimmerSDBT shimmer;

        static void Main(string[] args)
        {
            var client = new BluetoothClient();
            BluetoothDeviceInfo[] availableDevices = client.DiscoverDevices(); // I've found this to be SLOW!


            System.Console.WriteLine("Hello");
            Program p = new Program();
            p.start();
        }

        public void start()
        {
            int enabledSensors = ((int)Shimmer.SensorBitmapShimmer3.SENSOR_A_ACCEL); // this is to enable Accel
            shimmer = new ShimmerSDBT("ShimmerID1", "COM15");
            shimmer.UICallback += this.HandleEvent;
            shimmer.Connect();
            if (shimmer.GetState() == Shimmer.SHIMMER_STATE_CONNECTED)
            {
                shimmer.WriteSensors(enabledSensors);
                shimmer.StartStreaming();
            }
        }
        public void HandleEvent(object sender, EventArgs args)
        {
            CustomEventArgs eventArgs = (CustomEventArgs)args;
            int indicator = eventArgs.getIndicator();

            switch (indicator)
            {
                case (int)Shimmer.ShimmerIdentifier.MSG_IDENTIFIER_STATE_CHANGE:
                    System.Diagnostics.Debug.Write(((Shimmer)sender).GetDeviceName() + " State = " + ((Shimmer)sender).GetStateString() + System.Environment.NewLine);
                    int state = (int)eventArgs.getObject();
                    if (state == (int)Shimmer.SHIMMER_STATE_CONNECTED)
                    {

                    }
                    else if (state == (int)Shimmer.SHIMMER_STATE_CONNECTING)
                    {

                    }
                    else if (state == (int)Shimmer.SHIMMER_STATE_NONE)
                    {
                        System.Diagnostics.Debug.Write("Disconnected");

                    }
                    else if (state == (int)Shimmer.SHIMMER_STATE_STREAMING)
                    {

                    }
                    break;
                case (int)Shimmer.ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE:
                    break;
                case (int)Shimmer.ShimmerIdentifier.MSG_IDENTIFIER_DATA_PACKET:
                    ObjectCluster objectCluster = (ObjectCluster)eventArgs.getObject();
                    SensorData data = objectCluster.GetData("Low Noise Accelerometer X", "CAL");
                    System.Console.WriteLine("AccelX: " + data.Data);
                    
                    break;
            }
        }
    }
}
