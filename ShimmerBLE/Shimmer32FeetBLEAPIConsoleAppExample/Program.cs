using Acr.Collections;
using shimmer.Communications;
using shimmer.Models;
using shimmer.Sensors;
using ShimmerAPI;
using ShimmerBLEAPI.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static shimmer.Models.ShimmerBLEEventData;
using static ShimmerBLEAPI.Devices.VerisenseDevice;

namespace Shimmer32FeetBLEAPIConsoleAppExample
{
    class Program
    {
        static string MSG = "\nPress 'S' to connect with Bluetooth/ComPort \nPress 'D' to start streaming \nPress 'C' to stop the streaming \nPress 'V' to disconnect with Bluetooth/ComPort \nPress 'B' to Sync \nPress 'R' to read Op Config \nPress 'A' to enable LN Accel \nPress 'T' to Read Status \nPress 'U' to enable USB  \nPress 'I' to disable USB \nPress 'X' to delete data \nPress 'L' to list Shimmer Hardware";
        static VerisenseBLEDeviceWindows device;
        static Dictionary<string, VerisenseBLEDevice> devices = new Dictionary<string, VerisenseBLEDevice>();
        static List<string> uuids = new List<string>()
        {
            //"00000000-0000-0000-0000-e1ec063f5c80",
            //"00000000-0000-0000-0000-daa56d898b02",
               "00000000-0000-0000-0000-ec2ee3ebb799",
                //"00000000-0000-0000-0000-fbe2054c2e04",
                //"00000000-0000-0000-0000-c00419859ad5"
        };

        static List<string> comPorts = new List<string>()
        {
            "COM41",
            //"COM40",
            //"COM42",
            //"COM43",
            //"COM44""
        };
        static void Main(string[] args)
        {
            Run();
        }

        static void Run()
        {
            Task.Run(() =>
            {
                var watcher = new ManagementEventWatcher(
                    "SELECT * FROM __InstanceCreationEvent WITHIN 2 " +
                    "WHERE TargetInstance ISA 'Win32_PnPEntity'"
                );

                watcher.EventArrived += (sender, e) =>
                {
                    ListDevices();
                };

                watcher.Start();
            });


            Console.WriteLine(MSG);
            do
            {
                while (!Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.L:
                            ListDevices();
                            break;
                        case ConsoleKey.S:
                            ConnectDevices();
                            break;
                        case ConsoleKey.T:
                            ReadStatus();
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
                            ReadOpConfig();
                            break;
                        case ConsoleKey.A:
                            ConfigureDevices();
                            break;
                        case ConsoleKey.U:
                            ConfigureDevicesEnableUSB();
                            break;
                        case ConsoleKey.I:
                            ConfigureDevicesDisableUSB();
                            break;
                        case ConsoleKey.X:
                            DeleteDataDevices();
                            break;
                        default:
                            break;
                    }
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }

        static async void ListDevices()
        {
            Console.WriteLine();
            ShimmerDevices.PortFilterOption portFilterOption;
            portFilterOption = ShimmerDevices.PortFilterOption.All;
            string[] ShimmerComPorts = ShimmerDevices.GetComPorts(portFilterOption);
            foreach (string comport in ShimmerComPorts)
            {
                if (comport.ToLower().Contains("verisense"))
                {
                    Console.WriteLine("Found Shimmer Comport: " + comport);
                }
            }
            Console.WriteLine(MSG);
        }

        static async void ConnectDevices()
        {
            if (uuids.IsEmpty())
            {
                foreach (string comPort in comPorts)
                {

                    if (!devices.ContainsKey(comPort))
                    {
                        device = new VerisenseBLEDeviceWindows("00000000-0000-0000-0000-000000000000", "", comPort, CommunicationType.SerialPort);
                        //device = new VerisenseBLEDeviceWindows(uuid, "","com3", VerisenseDevice.CommunicationType.SerialPort);
                        device.ShimmerBLEEvent += ShimmerDevice_BLEEvent;
                        bool result = await device.Connect(true);
                        if (result)
                        {
                            Console.WriteLine("---------------------------------------------------------------");
                            Console.WriteLine("Successfully connected to the device! Comport: " + comPort);
                            Console.WriteLine("Device Version: " + device.GetProductionConfig().REV_HW_MAJOR + "." + device.GetProductionConfig().REV_HW_MINOR);
                            Console.WriteLine("Firmware Version: " + device.GetProductionConfig().REV_FW_MAJOR + "." + device.GetProductionConfig().REV_FW_MINOR + "." + device.GetProductionConfig().REV_FW_INTERNAL);
                            devices.Add(comPort, device);
                            Console.WriteLine("\nBT state: " + device.GetVerisenseBLEState() + "\nUUID: " + device.Asm_uuid + "\nBattery: " + device.GetStatus().BatteryPercent + "%");
                        }
                        else
                        {
                            Console.WriteLine("Failed to connect device! Comport: " + comPort);
                        }
                        Console.WriteLine(MSG);
                    }
                    else if (devices.ContainsKey(comPort))
                    {
                        var device = devices[comPort];
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
                return;
            }


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
                    }
                    else
                    {
                        Console.WriteLine("Failed to connect device! UUID: " + uuid);
                    }
                    Console.WriteLine(MSG);
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

        static async void ReadStatus()
        {
            foreach (VerisenseBLEDevice device in devices.Values)
            {
                var status = await device.ExecuteRequest(RequestType.ReadStatus2);

                if (status != null)
                {
                    Console.WriteLine("Battery Level: " + ((StatusPayload)status).BatteryLevel + "; Battery Percent: " + ((StatusPayload)status).BatteryPercent + "; USB Powered: " + ((StatusPayload)status).UsbPowered + "; Batt Charger Status: " + ((StatusPayload)status).BattChargerStatus.ToString());
                }
                else
                {
                    Console.WriteLine("Failed to read status");
                }
                Console.WriteLine("");
                
            }
        }

        static async void ReadOpConfig()
        {
            foreach (VerisenseBLEDevice device in devices.Values)
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
                Console.WriteLine(MSG);
                
            }
        }

        static async void ConfigureDevices()
        {
            foreach (VerisenseBLEDevice device in devices.Values)
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
                Console.WriteLine(MSG);
                Console.WriteLine("---------------------------------------------------------------");
                
            }
        }

        static async void ConfigureDevicesEnableUSB()
        {
            foreach (VerisenseBLEDevice device in devices.Values)
            {
                var clone = new VerisenseBLEDevice(device);
                clone.setUSBEnabled(true);
                var opconfigbytes = clone.GenerateConfigurationBytes();
                await device.WriteAndReadOperationalConfiguration(opconfigbytes);
                Console.WriteLine("-------------------------ENABLE USB--------------------------------------");
                
            }
            Console.WriteLine(MSG);
        }

        static async void ConfigureDevicesDisableUSB()
        {
            foreach (VerisenseBLEDevice device in devices.Values)
            {
                var clone = new VerisenseBLEDevice(device);
                clone.setUSBEnabled(false);
                var opconfigbytes = clone.GenerateConfigurationBytes();
                await device.WriteAndReadOperationalConfiguration(opconfigbytes);
                Console.WriteLine("-------------------------Disable USB--------------------------------------");
                
            }
            Console.WriteLine(MSG);
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
                Console.WriteLine(MSG);
                if (device != null)
                {
                    device.ShimmerBLEEvent -= ShimmerDevice_BLEEvent;
                }
                device.ShimmerBLEEvent += ShimmerDevice_BLEEvent;
            }
        }

        static async void DeleteDataDevices()
        {
            foreach (VerisenseBLEDevice device in devices.Values)
            {
                var streamResult = await device.ExecuteRequest(RequestType.EraseData);
                Console.WriteLine("Erase Data Status: " + streamResult.ToString());
                Console.WriteLine(MSG);
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
                Console.WriteLine(MSG);
            }

        }

        static async void DisconnectDevices()
        {
            List<string> listOfDisconnectedDevices = new List<string>();
            foreach (VerisenseBLEDevice device in devices.Values)
            {
                var result = await device.Disconnect();
                if (result)
                {
                    if (device.CommType.Equals(CommunicationType.BLE))
                    {
                        listOfDisconnectedDevices.Add(device.Asm_uuid.ToString());
                    }
                    else
                    {
                        listOfDisconnectedDevices.Add(device.ComPort);
                    }
                    device.ShimmerBLEEvent -= ShimmerDevice_BLEEvent;
                }

                Console.WriteLine("\nUUID: " + device.Asm_uuid + " attempt disconnect \nResult: " + result + "\nNew BLE Status: " + device.GetVerisenseBLEState());
                Console.WriteLine(MSG);
            }

            //Remove disconnected devices from the Dictionary
            foreach (var comhandle in listOfDisconnectedDevices)
            {
                devices.Remove(comhandle);
            }
        }
    }
}
