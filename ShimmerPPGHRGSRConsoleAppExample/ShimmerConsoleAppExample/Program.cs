using ShimmerAPI;
using ShimmerLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShimmerConsoleAppExample
{
    class Program
    {
        Filter LPF_PPG;
        Filter HPF_PPG;
        PPGToHRAlgorithm PPGtoHeartRateCalculation;
        int NumberOfHeartBeatsToAverage = 1;
        int TrainingPeriodPPG = 10; //10 second buffer
        double LPF_CORNER_FREQ_HZ = 5;
        double HPF_CORNER_FREQ_HZ = 0.5;
        ShimmerLogAndStreamSystemSerialPort Shimmer;
        double SamplingRate = 128;
        int Count = 0;
        bool FirstTime = true;

        //The index of the signals originating from ShimmerBluetooth 
        int IndexAccelX;
        int IndexAccelY;
        int IndexAccelZ;
        int IndexGSR;
        int IndexPPG;
        int IndexTimeStamp;

        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello");
            Program p = new Program();
            p.start();
        }

        public void start()
        {
            //Setup PPG to HR filters and algorithm
            PPGtoHeartRateCalculation = new PPGToHRAlgorithm(SamplingRate, NumberOfHeartBeatsToAverage, TrainingPeriodPPG);
            LPF_PPG = new Filter(Filter.LOW_PASS, SamplingRate, new double[] { LPF_CORNER_FREQ_HZ });
            HPF_PPG = new Filter(Filter.HIGH_PASS, SamplingRate, new double[] { HPF_CORNER_FREQ_HZ });


            int enabledSensors = ((int)ShimmerBluetooth.SensorBitmapShimmer3.SENSOR_A_ACCEL| (int)ShimmerBluetooth.SensorBitmapShimmer3.SENSOR_GSR| (int)ShimmerBluetooth.SensorBitmapShimmer3.SENSOR_INT_A13);
            //int enabledSensors = ((int)Shimmer.SensorBitmapShimmer3.SENSOR_A_ACCEL | (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG1_24BIT | (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG2_24BIT); 

            //shimmer = new Shimmer("ShimmerID1", "COM17");
            Shimmer = new ShimmerLogAndStreamSystemSerialPort("ShimmerID1", "COM11", SamplingRate, 0, ShimmerBluetooth.GSR_RANGE_AUTO, enabledSensors, false, false, false, 1, 0, Shimmer3Configuration.EXG_EMG_CONFIGURATION_CHIP1, Shimmer3Configuration.EXG_EMG_CONFIGURATION_CHIP2, true);

            Shimmer.UICallback += this.HandleEvent;
            Shimmer.Connect();

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
                        System.Console.WriteLine("Shimmer is Connected");
                        Task ignoredAwaitableResult = this.delayedWork();
                    }
                    else if (state == (int)ShimmerBluetooth.SHIMMER_STATE_CONNECTING)
                    {
                        System.Console.WriteLine("Establishing Connection to Shimmer Device");
                    }
                    else if (state == (int)ShimmerBluetooth.SHIMMER_STATE_NONE)
                    {
                        System.Console.WriteLine("Shimmer is Disconnected");
                    }
                    else if (state == (int)ShimmerBluetooth.SHIMMER_STATE_STREAMING)
                    {
                        System.Console.WriteLine("Shimmer is Streaming");
                    }
                    break;
                case (int)ShimmerBluetooth.ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE:
                    break;
                case (int)ShimmerBluetooth.ShimmerIdentifier.MSG_IDENTIFIER_DATA_PACKET:
                    ObjectCluster objectCluster = (ObjectCluster)eventArgs.getObject();
                    if (FirstTime)
                    {
                        IndexAccelX = objectCluster.GetIndex(Shimmer3Configuration.SignalNames.LOW_NOISE_ACCELEROMETER_X, ShimmerConfiguration.SignalFormats.CAL);
                        IndexAccelY = objectCluster.GetIndex(Shimmer3Configuration.SignalNames.LOW_NOISE_ACCELEROMETER_Y, ShimmerConfiguration.SignalFormats.CAL);
                        IndexAccelZ = objectCluster.GetIndex(Shimmer3Configuration.SignalNames.LOW_NOISE_ACCELEROMETER_Z, ShimmerConfiguration.SignalFormats.CAL);
                        IndexGSR = objectCluster.GetIndex(Shimmer3Configuration.SignalNames.GSR, ShimmerConfiguration.SignalFormats.CAL);
                        IndexPPG = objectCluster.GetIndex(Shimmer3Configuration.SignalNames.INTERNAL_ADC_A13, ShimmerConfiguration.SignalFormats.CAL);
                        IndexTimeStamp = objectCluster.GetIndex(ShimmerConfiguration.SignalNames.SYSTEM_TIMESTAMP, ShimmerConfiguration.SignalFormats.CAL);
                        FirstTime = false;
                    }
                    SensorData datax = objectCluster.GetData(IndexAccelX);
                    SensorData datay = objectCluster.GetData(IndexAccelY);
                    SensorData dataz = objectCluster.GetData(IndexAccelZ);
                    SensorData dataGSR = objectCluster.GetData(IndexGSR);
                    SensorData dataPPG = objectCluster.GetData(IndexPPG);
                    SensorData dataTS = objectCluster.GetData(IndexTimeStamp);

                    //Process PPG signal and calculate heart rate
                    double dataFilteredLP = LPF_PPG.filterData(dataPPG.Data);
                    double dataFilteredHP = HPF_PPG.filterData(dataFilteredLP);
                    int heartRate = (int)Math.Round(PPGtoHeartRateCalculation.ppgToHrConversion(dataFilteredHP, dataTS.Data));


                    if (Count % SamplingRate == 0) //only display data every second
                    {
                        System.Console.WriteLine("AccelX: " + datax.Data + " " + datax.Unit + " AccelY: " + datay.Data + " " + datay.Unit+ " AccelZ: " + dataz.Data + " " + dataz.Unit);
                        System.Console.WriteLine("Time Stamp: "+ dataTS.Data+ " " + dataTS.Unit + " GSR: " + dataGSR.Data + " "+ dataGSR.Unit + " PPG: " + dataPPG.Data + " " + dataPPG.Unit + " HR: " + heartRate +" BPM");
                    }
                    Count++;
                    break;
            }
        }

        private async Task delayedWork()
        {
            await Task.Delay(1000);
            Shimmer.StartStreaming();
        }

    }
}
