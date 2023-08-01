using shimmer.Helpers;
using shimmer.Models;
using shimmer.Sensors;
using ShimmerBLEAPI.Devices;
using System;

namespace GenerateVerisenseOpConfigString
{
    class Program
    {
        static readonly string defaultOpconfigPayloadString = "34-48-00-5A-9F-80-02-00-30-20-00-7F-00-FF-3F-00-00-00-00-80-00-00-00-00-00-00-00-00-00-00-00-00-00-03-F4-18-3C-00-0A-0F-00-18-3C-00-0A-0F-00-18-3C-00-0A-0F-00-17-04-FF-FF-FF-3C-00-0E-00-00-63-28-CC-CC-1E-00-0A-00-00-00-00-01";

        public class VerisenseBLEDeviceClone : VerisenseBLEDevice
        {
            public VerisenseBLEDeviceClone(string id, string name, byte[] opconfigbytes) : base(id, name)
            {
                OpConfig = new OpConfigPayload();
                OpConfig.ConfigurationBytes = new byte[opconfigbytes.Length];
                Array.Copy(opconfigbytes, OpConfig.ConfigurationBytes, opconfigbytes.Length); //deep copy
                UpdateDeviceAndSensorConfiguration();
            }
        }

        static void Main(string[] args)
        {
            OpConfigPayload opconfig = new OpConfigPayload();
            byte[] opconfigPayloadArray = BitHelper.MSBByteArray(defaultOpconfigPayloadString.Replace("-", "")).ToArray();
            opconfig.ProcessPayload(opconfigPayloadArray);
            VerisenseBLEDevice device = new VerisenseBLEDeviceClone("00000000-0000-0000-0000-000000000000", "", opconfig.ConfigurationBytes);
            
            //configure the device here, for example
            ((SensorLIS2DW12)device.GetSensor("Accel1")).SetSamplingRate(SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_12_5Hz);
            ((SensorLIS2DW12)device.GetSensor("Accel1")).SetAccelRange(SensorLIS2DW12.AccelRange.Range_8G);
            Console.WriteLine(BitConverter.ToString(device.GenerateConfigurationBytes()));

            Console.WriteLine(BitConverter.ToString(device.GenerateConfigurationBytes()).Replace("-",""));
        }
    }
}