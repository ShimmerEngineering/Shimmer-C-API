using shimmer.Communications;
using shimmer.Models;
using shimmer.Sensors;
using ShimmerAPI;
using ShimmerBLEAPI.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static shimmer.Models.ShimmerBLEEventData;

namespace Shimmer32FeetBLEAPIConsoleAppExample
{
    class Program
    {
        static VerisenseBLEDeviceWindows device;
        static Dictionary<string, VerisenseBLEDevice> devices = new Dictionary<string, VerisenseBLEDevice>();
        static List<string> uuids = new List<string>()
        {
            //"00000000-0000-0000-0000-e1ec063f5c80",
            //"00000000-0000-0000-0000-daa56d898b02",
               "00000000-0000-0000-0000-ec2ee3ebb799",
                //"00000000-0000-0000-0000-fbe2054c2e04"
        };
        static void Main(string[] args)
        {
            Run();
        }

        static void Run()
        {
            Console.WriteLine("Press 'S' to connect with Bluetooth \nPress 'D' to start streaming \nPress 'C' to stop the streaming \nPress 'V' to disconnect with Bluetooth \nPress 'B' to Sync \nPress 'R' to read Op Config");
            do
            {
                while (!Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.S:
                            ConnectDevices();
                            break;
                        case ConsoleKey.D:
                            StartStreamingDevices();
                            break;
                        case ConsoleKey.C:
                            StopStreamingDevices();
                            break;
                        case ConsoleKey.V:
                            DisconnectDevices();
                            break;
                        case ConsoleKey.B:
                            StartSyncingDevices();
                            break;
                        case ConsoleKey.R:
                            foreach (VerisenseBLEDevice device in devices.Values)
                            {
                                ReadOpConfig(device);
                            }
                            break;
                        default:
                            break;
                    }
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }

        static async void ConnectDevices()
        {
            foreach (string uuid in uuids)
            {

                if (!devices.ContainsKey(uuid))
                {
                    device = new VerisenseBLEDeviceWindows(uuid, "");
                    //device = new VerisenseBLEDeviceWindows(uuid, "","com3", VerisenseDevice.CommunicationType.SerialPort);
                    device.ShimmerBLEEvent += ShimmerDevice_BLEEvent;
                    bool result = await device.Connect(true);
                    if (result)
                    {
                        Console.WriteLine("---------------------------------------------------------------");
                        Console.WriteLine("Successfully connected to the device! UUID: " + uuid);
                        Console.WriteLine("Device Version: " + device.GetProductionConfig().REV_HW_MAJOR + "." + device.GetProductionConfig().REV_HW_MINOR);
                        Console.WriteLine("Firmware Version: " + device.GetProductionConfig().REV_FW_MAJOR + "." + device.GetProductionConfig().REV_FW_MINOR + "." + device.GetProductionConfig().REV_FW_INTERNAL);
                        devices.Add(uuid, device);
                        Console.WriteLine("\nBT state: " + device.GetVerisenseBLEState() + "\nUUID: " + device.Asm_uuid + "\nBattery: " + device.GetStatus().BatteryPercent + "%");
                        ConfigureDevices(device);
                    }
                    else
                    {
                        Console.WriteLine("Failed to connect device! UUID: " + uuid);
                    }
                }
                else if (devices.ContainsKey(uuid))
                {
                    var device = devices[uuid];
                    if (device.GetVerisenseBLEState().Equals(ShimmerDeviceBluetoothState.Disconnected))
                    {
                        var result = await device.Connect(true);
                        Console.WriteLine(result);
                    }
                    else
                    {
                        Console.WriteLine("Unable to connect device as it is already connected");
                    }

                }
            }
        }

        static async void ReadOpConfig(VerisenseBLEDevice device)
        {
            var opConfig = await device.ExecuteRequest(RequestType.ReadOperationalConfig);
            
            if (opConfig != null)
            {
                Console.WriteLine("Operational Config: " + BitConverter.ToString(((OpConfigPayload)opConfig).GetPayloadWithHeader()).Replace("-", ",0x"));
            }
            else
            {
                Console.WriteLine("Failed to read operational config");
            }
        }

        static async void ConfigureDevices(VerisenseBLEDevice device)
        {
            var clone = new VerisenseBLEDevice(device);

            SensorLIS2DW12 sensor = (SensorLIS2DW12)clone.GetSensor(SensorLIS2DW12.SensorName);
            sensor.SetAccelEnabled(true);
            sensor.SetSamplingRate(SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_25Hz);

            bool accelDetection = sensor.IsAccelEnabled();
            string accelRate = sensor.GetSamplingRate().GetDisplayName();
            string accelMode = sensor.GetMode().GetDisplayName();
            string accelLPMode = sensor.GetLowPowerMode().GetDisplayName();

            var opconfigbytes = clone.GenerateConfigurationBytes();
            await device.WriteAndReadOperationalConfiguration(opconfigbytes);

            Console.WriteLine("\n--|ACCEL|--" + "\nIsAccelEnabled: " + accelDetection + "\nAccelRate: " + accelRate + "\nAccelMode: " + accelMode + "\nAccelLowPowerMode: " + accelLPMode);
            Console.WriteLine("\nPress 'S' to connect with Bluetooth \nPress 'D' to start streaming \nPress 'C' to stop the streaming \nPress 'V' to disconnect with Bluetooth \nPress 'B' to Sync \nPress 'R' to read Op Config\"");
            Console.WriteLine("---------------------------------------------------------------");
        }
        static async void StartSyncingDevices()
        {
            foreach (VerisenseBLEDevice device in devices.Values)
            {
                var streamResult = await device.ExecuteRequest(RequestType.TransferLoggedData);
                if (device != null)
                {
                    device.ShimmerBLEEvent -= ShimmerDevice_BLEEvent;
                }
                device.ShimmerBLEEvent += ShimmerDevice_BLEEvent;
            }
        }

        static async void StartStreamingDevices()
        {
            foreach (VerisenseBLEDevice device in devices.Values)
            {
                var streamResult = await device.ExecuteRequest(RequestType.StartStreaming);
                Console.WriteLine("Stream Status: " + streamResult);
                Console.WriteLine("\nPress 'S' to connect with Bluetooth \nPress 'D' to start streaming \nPress 'C' to stop the streaming \nPress 'V' to disconnect with Bluetooth");
                if (device != null)
                {
                    device.ShimmerBLEEvent -= ShimmerDevice_BLEEvent;
                }
                device.ShimmerBLEEvent += ShimmerDevice_BLEEvent;
            }
        }

        static void ShimmerDevice_BLEEvent(object sender, ShimmerBLEEventData e)
        {
            if (e.CurrentEvent == VerisenseBLEEvent.NewDataPacket)
            {
                ObjectCluster ojc = ((ObjectCluster)e.ObjMsg);
                if (ojc.GetNames().Contains(SensorLIS2DW12.ObjectClusterSensorName.LIS2DW12_ACC_X))
                {
                    var a2x = ojc.GetData(SensorLIS2DW12.ObjectClusterSensorName.LIS2DW12_ACC_X, ShimmerConfiguration.SignalFormats.CAL);
                    var a2y = ojc.GetData(SensorLIS2DW12.ObjectClusterSensorName.LIS2DW12_ACC_Y, ShimmerConfiguration.SignalFormats.CAL);
                    var a2z = ojc.GetData(SensorLIS2DW12.ObjectClusterSensorName.LIS2DW12_ACC_Z, ShimmerConfiguration.SignalFormats.CAL);
                    Debug.WriteLine("UUID: " + ojc.GetCOMPort() + " |X : " + Math.Round(a2x.Data, 2) + "  Y : " + Math.Round(a2y.Data, 2) + "  Z : " + Math.Round(a2z.Data, 2) + "|");
                    System.Console.WriteLine("UUID: " + ojc.GetCOMPort() + " |X : " + Math.Round(a2x.Data, 2) + "  Y : " + Math.Round(a2y.Data, 2) + "  Z : " + Math.Round(a2z.Data, 2) + "|");
                }
            }
        }

        static async void StopStreamingDevices()
        {
            foreach (VerisenseBLEDevice device in devices.Values)
            {
                var result = await device.ExecuteRequest(RequestType.StopStreaming);
                Console.WriteLine("\nUUID: " + device.Asm_uuid + " stop streaming request \nResult: " + result + "\n");
                Console.WriteLine("\nPress 'S' to connect with Bluetooth \nPress 'D' to start streaming \nPress 'C' to stop the streaming \nPress 'V' to disconnect with Bluetooth");
            }

        }

        static async void DisconnectDevices()
        {
            List<string> listOfDisconnectedUuids = new List<string>();
            foreach (VerisenseBLEDevice device in devices.Values)
            {
                var result = await device.Disconnect();
                if (result)
                {
                    listOfDisconnectedUuids.Add(device.Asm_uuid.ToString());
                    device.ShimmerBLEEvent -= ShimmerDevice_BLEEvent;
                }

                Console.WriteLine("\nUUID: " + device.Asm_uuid + " attempt disconnect \nResult: " + result + "\nNew BLE Status: " + device.GetVerisenseBLEState());
                Console.WriteLine("\nPress 'S' to connect with Bluetooth \nPress 'D' to start streaming \nPress 'C' to stop the streaming \nPress 'V' to disconnect with Bluetooth");
            }

            //Remove disconnected devices from the Dictionary
            foreach (var uuid in listOfDisconnectedUuids)
            {
                devices.Remove(uuid);
            }
        }
    }
}
