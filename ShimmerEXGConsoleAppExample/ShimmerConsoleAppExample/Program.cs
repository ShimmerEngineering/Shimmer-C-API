using ShimmerAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShimmerConsoleAppExample
{
    class Program
    {
        ShimmerLogAndStreamSystemSerialPort shimmer;

        //Data is being logged to the bin folder
        Logging logging = new Logging("exgtestsignal.csv", ",");

        static void Main(string[] args)
        {
            /* Example of using 32 feet to scan for devices
            var client = new BluetoothClient();
            BluetoothDeviceInfo[] availableDevices = client.DiscoverDevices(); // I've found this to be SLOW!
            */

            System.Console.WriteLine("Hello");
            Program p = new Program();
            p.start();
        }

        public void start()
        {
            //There are two main uses of the constructors, first one just connects to the device without setting and specific configurations
            //shimmer = new ShimmerSDBT("ShimmerID1", "COM15");
            int enabledSensors = ((int)ShimmerBluetooth.SensorBitmapShimmer3.SENSOR_EXG1_24BIT | (int)ShimmerBluetooth.SensorBitmapShimmer3.SENSOR_EXG2_24BIT); // this is to enable the two EXG Chips on the Shimmer3
            double samplingRate = 51.2;
            //byte[] defaultECGReg1 = new byte[10] { 0x00, 0xA0, 0x10, 0x40, 0x40, 0x2D, 0x00, 0x00, 0x02, 0x03 }; //see ShimmerBluetooth.SHIMMER3_DEFAULT_ECG_REG1
            //byte[] defaultECGReg2 = new byte[10] { 0x00, 0xA0, 0x10, 0x40, 0x47, 0x00, 0x00, 0x00, 0x02, 0x01 }; //see ShimmerBluetooth.SHIMMER3_DEFAULT_ECG_REG2
            byte[] defaultECGReg1 = ShimmerBluetooth.SHIMMER3_DEFAULT_TEST_REG1; //also see ShimmerBluetooth.SHIMMER3_DEFAULT_ECG_REG1 && ShimmerBluetooth.SHIMMER3_DEFAULT_EMG_REG1
            byte[] defaultECGReg2 = ShimmerBluetooth.SHIMMER3_DEFAULT_TEST_REG2; //also see ShimmerBluetooth.SHIMMER3_DEFAULT_ECG_REG2 && ShimmerBluetooth.SHIMMER3_DEFAULT_EMG_REG2
            //The constructor below allows the user to specify the shimmer configurations which is set upon connection to the device
            shimmer = new ShimmerLogAndStreamSystemSerialPort("ShimmerID1", "COM169", samplingRate, 0, 4, enabledSensors, false, false, false, 0, 0, defaultECGReg1, defaultECGReg2, false);
            shimmer.UICallback += this.HandleEvent;
            shimmer.Connect();
            if (shimmer.GetState() == ShimmerBluetooth.SHIMMER_STATE_CONNECTED)
            {
                System.Console.WriteLine("EXG CONFIGURATION SET USING SHIMMER CONSTRUCTOR");
                System.Console.WriteLine("EXG CHIP 1 CONFIGURATION");
                for (int i = 0; i < 10; i++)
                {
                    System.Console.Write(shimmer.GetEXG1RegisterContents()[i] + " ");
                }
                System.Console.WriteLine("\nEXG CHIP 2 CONFIGURATION");
                for (int i = 0; i < 10; i++)
                {
                    System.Console.Write(shimmer.GetEXG2RegisterContents()[i] + " ");
                }
                System.Console.WriteLine("\n");
              
                shimmer.WriteSensors(enabledSensors);

                System.Console.WriteLine("IN ABOUT 5 SECONDS STREAMING WILL START AFTER THE BEEP");
                Thread.Sleep(5000);
                System.Console.Beep();

                shimmer.StartStreaming();
            }
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
                        System.Console.WriteLine("Connected");
                    }
                    else if (state == (int)ShimmerBluetooth.SHIMMER_STATE_CONNECTING)
                    {
                        System.Console.WriteLine("Connecting");
                    }
                    else if (state == (int)ShimmerBluetooth.SHIMMER_STATE_NONE)
                    {
                        System.Console.WriteLine("Disconnected");
                    }
                    else if (state == (int)ShimmerBluetooth.SHIMMER_STATE_STREAMING)
                    {
                        System.Console.WriteLine("Streaming");
                        System.Console.WriteLine("Data being written to exgtestsignal.csv");
                    }
                    break;
                case (int)ShimmerBluetooth.ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE:
                    break;
                case (int)ShimmerBluetooth.ShimmerIdentifier.MSG_IDENTIFIER_DATA_PACKET:
                    ObjectCluster objectCluster = (ObjectCluster)eventArgs.getObject();
                    //SensorData data = objectCluster.GetData(Shimmer3Configuration.SignalNames.EXG1_CH1_16BIT, "CAL");
                    SensorData data = objectCluster.GetData(Shimmer3Configuration.SignalNames.EXG1_CH1, "CAL");
                    System.Console.WriteLine("EXG Channel 1: " + data.Data);
                    logging.WriteData(objectCluster);
                    break;
            }
        }
    }
}
