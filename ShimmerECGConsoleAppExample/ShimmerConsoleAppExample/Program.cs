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
        Logging logging = new Logging("ecg.csv",",");
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
            double samplingRate = 512;
            //byte[] defaultECGReg1 = new byte[10] { 0x00, 0xA0, 0x10, 0x40, 0x40, 0x2D, 0x00, 0x00, 0x02, 0x03 }; //see ShimmerBluetooth.SHIMMER3_DEFAULT_ECG_REG1
            //byte[] defaultECGReg2 = new byte[10] { 0x00, 0xA0, 0x10, 0x40, 0x47, 0x00, 0x00, 0x00, 0x02, 0x01 }; //see ShimmerBluetooth.SHIMMER3_DEFAULT_ECG_REG2
            byte[] defaultECGReg1 = ShimmerBluetooth.SHIMMER3_DEFAULT_ECG_REG1; //also see ShimmerBluetooth.SHIMMER3_DEFAULT_TEST_REG1 && ShimmerBluetooth.SHIMMER3_DEFAULT_EMG_REG1
            byte[] defaultECGReg2 = ShimmerBluetooth.SHIMMER3_DEFAULT_ECG_REG2; //also see ShimmerBluetooth.SHIMMER3_DEFAULT_TEST_REG2 && ShimmerBluetooth.SHIMMER3_DEFAULT_EMG_REG2
            //The constructor below allows the user to specify the shimmer configurations which is set upon connection to the device
            shimmer = new ShimmerLogAndStreamSystemSerialPort("ShimmerID1", "COM169", samplingRate, 0, 4, enabledSensors, false, false, false, 0, 0, defaultECGReg1, defaultECGReg2, false);
            shimmer.UICallback += this.HandleEvent;
            shimmer.Connect();
            if (shimmer.GetState() == ShimmerBluetooth.SHIMMER_STATE_CONNECTED)
            {
                System.Console.WriteLine("\n");
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
                
                //Note the data rate of the EXG chips (exgrate) is automatically set via writesamplingrate(). For a sampling rate of 512Hz, the exgrate is set to 0x03, which corresponds to 1000 SPS. 
                //The exgrate can be set to a different settting afterwards, e.g. to 2000 SPS:
                shimmer.WriteEXGRate(4);

                //The gain of the EXG chips is also configurable. The default gain = 4 (0x04). Now the gain is set to 0x05, which corresponds to a gain of 8:
                shimmer.WriteEXGGain(5);
                
                shimmer.ReadEXGConfigurations(1);
                shimmer.ReadEXGConfigurations(2);
                System.Console.WriteLine("EXG CONFIGURATION AFTER MANUALLY SETTING THE VALUES (RATE and GAIN)");
                System.Console.WriteLine("EXG CHIP 1 CONFIGURATION");
                for (int i = 0; i < 10; i++) {
                    System.Console.Write(shimmer.GetEXG1RegisterContents()[i] + " ");
                }
                System.Console.WriteLine("\nEXG CHIP 2 CONFIGURATION");
                for (int i = 0; i < 10; i++)
                {
                    System.Console.Write(shimmer.GetEXG2RegisterContents()[i] + " ");
                }
                System.Console.WriteLine("\n");

                //Example code for changing the ecg resolution from 24bit to 16bit - to limit the amount of data transmitted over the Bluetooth link
                enabledSensors = ((int)ShimmerBluetooth.SensorBitmapShimmer3.SENSOR_EXG1_16BIT | (int)ShimmerBluetooth.SensorBitmapShimmer3.SENSOR_EXG2_16BIT); // this is to enable the two EXG Chips on the Shimmer3 at 16 bit resolution
                shimmer.WriteSensors(enabledSensors);

                //Example code for changing the ecg resolution back to 24bit - recommended
                enabledSensors = ((int)ShimmerBluetooth.SensorBitmapShimmer3.SENSOR_EXG1_24BIT | (int)ShimmerBluetooth.SensorBitmapShimmer3.SENSOR_EXG2_24BIT); // this is to enable the two EXG Chips on the Shimmer3 at 24 bit resolution
                shimmer.WriteSensors(enabledSensors);

                //Example code for setting the default ecg settings again (exggain = 4, exgrate >= sampling rate)
                //First write the default register settings for both EXG chips
                shimmer.WriteEXGConfigurations(defaultECGReg1, defaultECGReg2);
                shimmer.ReadEXGConfigurations(1);
                shimmer.ReadEXGConfigurations(2);
                System.Console.WriteLine("SET DEFAULT ECG CONFIGURATIONS AGAIN");
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
                //Second write the exgrate via writesamplingrate() this time
                shimmer.WriteSamplingRate(512);
                shimmer.ReadEXGConfigurations(1);
                shimmer.ReadEXGConfigurations(2);
                System.Console.WriteLine("SET ECG DATA RATE AUTOMATICALLY BY CHANGING THE SAMPLING RATE OF THE SHIMMER");
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
                        System.Console.WriteLine("Data being written to ecg.csv");
                    }
                    break;
                case (int)ShimmerBluetooth.ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE:
                    break;
                case (int)ShimmerBluetooth.ShimmerIdentifier.MSG_IDENTIFIER_DATA_PACKET:
                    ObjectCluster objectCluster = (ObjectCluster)eventArgs.getObject();
                    SensorData data = objectCluster.GetData(Shimmer3Configuration.SignalNames.ECG_LA_RA, "CAL");
                    if (data!=null)
                    System.Console.Write(data.Data+",");
                    logging.WriteData(objectCluster);
                    break;
            }
        }
    }
}
