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
        Shimmer shimmer;
        double SamplingRate = 128;
        int count = 0;
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello");
            Program p = new Program();
            p.start();
        }

        public void start()
        {
            int enabledSensors = ((int)Shimmer.SensorBitmapShimmer3.SENSOR_A_ACCEL| (int)Shimmer.SensorBitmapShimmer3.SENSOR_GSR| (int)Shimmer.SensorBitmapShimmer3.SENSOR_INT_A13); // this is to enable Accel
            //shimmer = new Shimmer("ShimmerID1", "COM17");
            shimmer = new Shimmer("ShimmerID1", "COM18", SamplingRate, 0, ShimmerBluetooth.GSR_RANGE_AUTO, enabledSensors, false, false, false, 1, 0, Shimmer3Configuration.EXG_TEST_SIGNAL_CONFIGURATION_CHIP1, Shimmer3Configuration.EXG_TEST_SIGNAL_CONFIGURATION_CHIP2, true);

            shimmer.UICallback += this.HandleEvent;
            shimmer.Connect();

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
                        shimmer.StartStreaming();
                        System.Console.WriteLine("Shimmer is Connected");
                    }
                    else if (state == (int)Shimmer.SHIMMER_STATE_CONNECTING)
                    {
                        System.Console.WriteLine("Establishing Connection to Shimmer Device");
                    }
                    else if (state == (int)Shimmer.SHIMMER_STATE_NONE)
                    {

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
                    if (count % SamplingRate == 0) //only display data every second
                    {
                        System.Console.WriteLine("AccelX: " + data.Data);
                    }
                    count++;
                    break;
            }
        }
    }
}
