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
        ShimmerSDBT32Feet shimmer;

        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello");
            Program p = new Program();
            p.start();
        }

        public void start()
        {
            int enabledSensors = ((int)Shimmer.SensorBitmapShimmer3.SENSOR_A_ACCEL | (int)Shimmer.SensorBitmapShimmer3.SENSOR_GSR | (int)Shimmer.SensorBitmapShimmer3.SENSOR_INT_A13);

            //shimmer = new Shimmer32Feet("ShimmerID1", "00:06:66:66:96:86");
            shimmer = new ShimmerSDBT32Feet("ShimmerID1", "00:06:66:66:96:A9", 102.4, 0, ShimmerBluetooth.GSR_RANGE_AUTO, enabledSensors, false, false, false, 1, 0, Shimmer3Configuration.EXG_EMG_CONFIGURATION_CHIP1, Shimmer3Configuration.EXG_EMG_CONFIGURATION_CHIP2, true);

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
                    int state = (int)eventArgs.getObject();
                    if (state == (int)Shimmer.SHIMMER_STATE_CONNECTED)
                    {

                    }
                    else if (state == (int)Shimmer.SHIMMER_STATE_CONNECTING)
                    {

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
                    System.Console.WriteLine("AccelX: " + data.Data);
                    
                    break;
            }
        }
    }
}
