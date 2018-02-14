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
        ShimmerBluetooth shimmer;
        int count = 0;
        int lastknowncount = -1;
        Boolean stream = true;
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
            int enabledSensors = ((int)ShimmerBluetooth.SensorBitmapShimmer3.SENSOR_A_ACCEL | (int)ShimmerBluetooth.SensorBitmapShimmer3.SENSOR_D_ACCEL); // this is to enable Analog Accel also known as low noise accelerometer
            double samplingRate = 51.2;
            //byte[] defaultECGReg1 = new byte[10] { 0x00, 0xA0, 0x10, 0x40, 0x40, 0x2D, 0x00, 0x00, 0x02, 0x03 }; //see ShimmerBluetooth.SHIMMER3_DEFAULT_ECG_REG1
            //byte[] defaultECGReg2 = new byte[10] { 0x00, 0xA0, 0x10, 0x40, 0x47, 0x00, 0x00, 0x00, 0x02, 0x01 }; //see ShimmerBluetooth.SHIMMER3_DEFAULT_ECG_REG2
            byte[] defaultECGReg1 = ShimmerBluetooth.SHIMMER3_DEFAULT_TEST_REG1; //also see ShimmerBluetooth.SHIMMER3_DEFAULT_ECG_REG1
            byte[] defaultECGReg2 = ShimmerBluetooth.SHIMMER3_DEFAULT_TEST_REG2; //also see ShimmerBluetooth.SHIMMER3_DEFAULT_ECG_REG2
            //The constructor below allows the user to specify the shimmer configurations which is set upon connection to the device
            shimmer = new ShimmerLogAndStreamSystemSerialPort("ShimmerID1", "COM12", 1, 0, 4, enabledSensors, false, false, false, 0, 0, defaultECGReg1, defaultECGReg2, false);
            //shimmer = new ShimmerLogAndStreamSystemSerialPort("ShimmerID1", "COM35", 1, 0, 4, enabledSensors, false, false, false, 0, 0, defaultECGReg1, defaultECGReg2, false);

            //shimmer = new ShimmerLogAndStream32Feet("ShimmerID1", "00:06:66:66:96:A9", 1, 0, 4, enabledSensors, false, false, false, 0, 0, defaultECGReg1, defaultECGReg2, false);

            shimmer.UICallback += this.HandleEvent;
            shimmer.Connect();
            
            
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
                        System.Console.WriteLine(count + " " + "Connected");
                        if (stream)
                        {
                            //Needs to be executed on a seperate thread, and a sleep to ensure everything on the current thread is completed
                            new Thread(() =>
                            {
                                Thread.Sleep(500);
                                shimmer.StartStreaming();
                            }).Start();
                        } else
                        {
                            new Thread(() =>
                            {
                                Thread.Sleep(500);
                                shimmer.Disconnect();
                            }).Start();
                        }
   
                    }
                    else if (state == (int)ShimmerBluetooth.SHIMMER_STATE_CONNECTING)
                    {
                        System.Diagnostics.Debug.Write("Connecting");
                        System.Console.WriteLine(count + " " + "Connecting");
                    }
                    else if (state == (int)ShimmerBluetooth.SHIMMER_STATE_NONE)
                    {
                        System.Diagnostics.Debug.Write("Disconnected");
                        System.Console.WriteLine(count + " " + "Disconnected");
                        stream = true;
                        new Thread(() =>
                        {
                            Thread.Sleep(500);
                            count++;
                            shimmer.Connect();
                        }).Start();
                    }
                    else if (state == (int)ShimmerBluetooth.SHIMMER_STATE_STREAMING)
                    {
                        System.Diagnostics.Debug.Write("Streaming");
                        System.Console.WriteLine(count + " " + "Streaming");
                       
                    }
                    break;
                case (int)ShimmerBluetooth.ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE:
                    break;
                case (int)ShimmerBluetooth.ShimmerIdentifier.MSG_IDENTIFIER_DATA_PACKET:
                    ObjectCluster objectCluster = (ObjectCluster)eventArgs.getObject();
                    SensorData data = objectCluster.GetData(Shimmer3Configuration.SignalNames.LOW_NOISE_ACCELEROMETER_X, "CAL");
                    if (lastknowncount != count)
                    {
                        System.Console.WriteLine(count + " " + "AccelX: " + data.Data);
                        lastknowncount = count;
                        stream = false;
                        new Thread(() =>
                        {
                            Thread.Sleep(500);
                            shimmer.StopStreaming();
                        }).Start();

                    }
                    
                    break;
            }
        }
    }
}
