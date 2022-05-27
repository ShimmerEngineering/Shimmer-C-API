using System;
using shimmer.Models;
using shimmer.Helpers;
using static shimmer.Models.ShimmerBLEEventData;
using System.IO;
using shimmer.Communications;
using System.Reflection;
using ShimmerBLEAPI.Devices;
using System.Runtime.InteropServices;

namespace VerisenseConfigureAndSyncConsole
{
    internal class Program
    {
        static VerisenseBLEDevice device;

        static async System.Threading.Tasks.Task Main(string[] args)
        {
            //args[0] - uuid
            //args[1] - action
            //args[2] - bin file path / op config bytes / default config
            //args[3] - trial name
            //args[4] - participant id
            if (args.Length == 1)
            {
                if (args[0] == "VERSION")
                {
                    Version version = Assembly.GetEntryAssembly().GetName().Version;
                    Console.WriteLine("Version: " + version.ToString());
                }
            }
            else
            {
                if (args.Length >= 2)
                {
                    if(args[1] == "WRITE_DEFAULT_OPCONFIG")
                    {
                        var result = await ConnectDevice(args[0], args[2]);
                        if (!result)
                        {
                            return;
                        }
                    }
                    else
                    {
                        var result = await ConnectDevice(args[0]);
                        if (result)
                        {
                            if (args[1] == "DATA_SYNC")
                            {
                                if (args.Length >= 5)
                                {
                                    await StartDataSync(args[2], args[3], args[4]);
                                }
                                else if (args.Length >= 4)
                                {
                                    await StartDataSync(args[2], args[3]);
                                }
                                else if (args.Length >= 3)
                                {
                                    await StartDataSync(args[2]);
                                }
                                else
                                {
                                    await StartDataSync();
                                }
                            }
                            else if (args[1] == "WRITE_OPCONFIG")
                            {
                                await WriteOpConfig(args[2]);
                            }
                            else if (args[1] == "DISABLE_LOGGING")
                            {
                                await DisableLogging();
                            }
                            else if (args[1] == "ERASE_DATA")
                            {
                                await EraseData();
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    await DisconnectDevice();
                }
                else
                {
                    Console.WriteLine("At least two arguments are needed");
                    Console.WriteLine("Usage: start VerisenseConfigureAndSyncConsole.exe [-uuid] [-options] [args...]");
                    Console.WriteLine("where options include:");
                    Console.WriteLine("\t-VERSION");
                    Console.WriteLine("\t-DATA_SYNC (with three arguments)");
                    Console.WriteLine("\t\t-bin file path");
                    Console.WriteLine("\t\t-trial name");
                    Console.WriteLine("\t\t-participant id");
                    Console.WriteLine("\t -WRITE_OPCONFIG (with one argument)");
                    Console.WriteLine("\t\t-operational config bytes e.g. 5A-97-00-00-00-...");
                    Console.WriteLine("e.g. start VerisenseConfigureAndSyncConsole.exe 00000000-0000-0000-0000-d02b463da2bb DATA_SYNC C:\\Users\\UserName\\Desktop trialA participantB");
                }
            }
        }

        static async System.Threading.Tasks.Task<bool> ConnectDevice(string uuid, string defaultConfig = "")
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                device = new VerisenseBLEDeviceWindowsBlueZ(uuid, "");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                device = new VerisenseBLEDeviceWindows(uuid, "");
            }

            device.ShimmerBLEEvent += ShimmerDevice_BLEEvent;
            bool result = false;
            switch (defaultConfig)
            {
                case "":
                    result = await device.Connect(true);
                    break;
                case "ACCEL1":
                    result = await device.Connect(true, VerisenseDevice.DefaultVerisenseConfiguration.Accel1_Default_Setting, false);
                    break;
                case "ACCEL2_GYRO":
                    result = await device.Connect(true, VerisenseDevice.DefaultVerisenseConfiguration.Accel2_Gyro_Default_Setting, false);
                    break;
                case "GSR_BATT_ACCEL1":
                    result = await device.Connect(true, VerisenseDevice.DefaultVerisenseConfiguration.GSR_Batt_Accel1_Default_Setting, false);
                    break;
                case "GSR_BATT":
                    result = await device.Connect(true, VerisenseDevice.DefaultVerisenseConfiguration.GSR_Batt_Default_Setting, false);
                    break;
                case "PPG":
                    result = await device.Connect(true, VerisenseDevice.DefaultVerisenseConfiguration.PPG_Default_Setting, false);
                    break;
                default:
                    Console.WriteLine("The available default configurations are ACCEL1, ACCEL2_GYRO, GSR_BATT_ACCEL1, GSR_BATT, PPG");
                    return false;
            }
            if (result)
            {
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine("Successfully connected to the device! UUID: " + uuid);
                Console.WriteLine("Device Version: " + device.GetProductionConfig().REV_HW_MAJOR + "." + device.GetProductionConfig().REV_HW_MINOR);
                Console.WriteLine("Firmware Version: " + device.GetProductionConfig().REV_FW_MAJOR + "." + device.GetProductionConfig().REV_FW_MINOR + "." + device.GetProductionConfig().REV_FW_INTERNAL);
                Console.WriteLine("\nBT state: " + device.GetVerisenseBLEState() + "\nUUID: " + device.Asm_uuid + "\nBattery: " + device.GetStatus().BatteryPercent + "%");
                return true;
            }
            else
            {
                Console.WriteLine("Failed to connect device! UUID: " + uuid);
            }
            return false;
        }

        static async System.Threading.Tasks.Task<bool> StartDataSync(string path = "", string trialName = "", string participantID = "")
        {
            if (!string.IsNullOrEmpty(path))
            {
                if (!System.IO.Directory.Exists(path))
                {
                    Console.WriteLine("Please enter a valid path");
                    return false;
                }
            }
            else
            {
                path = Directory.GetCurrentDirectory();
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                VerisenseBLEDeviceWindowsBlueZ.path = path;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                VerisenseBLEDeviceWindows.path = path;
            }
            
            RadioPlugin32Feet.ShowRXB = false;
            if (!string.IsNullOrEmpty(trialName))
            {
                device.SetTrialName(trialName);
            }
            if (!string.IsNullOrEmpty(participantID))
            {
                device.SetParticipantID(participantID);
            }

            var data = await device.ExecuteRequest(RequestType.TransferLoggedData);
            if (device != null)
            {
                device.ShimmerBLEEvent -= ShimmerDevice_BLEEvent;
            }
            device.ShimmerBLEEvent += ShimmerDevice_BLEEvent;
            if(data != null)
            {
                return true;
            }
            return false;
        }

        static async System.Threading.Tasks.Task<bool> WriteOpConfig(string bytesstring)
        {
            byte[] opconfigbytes = BitHelper.MSBByteArray(bytesstring.Replace("-", "")).ToArray();
            var result = await device.ExecuteRequest(RequestType.WriteOperationalConfig, opconfigbytes);
            if (result != null)
            {
                Console.WriteLine("Write Success");
                return true;
            }
            return false;
        }

        static async System.Threading.Tasks.Task<bool> DisableLogging()
        {
            device.setLoggingEnabled(false);
            var opconfigbytes = device.GenerateConfigurationBytes();
            var result = await device.WriteAndReadOperationalConfiguration(opconfigbytes);
            if (result != null)
            {
                Console.WriteLine("Logging disabled");
                return true;
            }
            return false;
        }

        static async System.Threading.Tasks.Task<bool> EraseData()
        {
            var result = await device.ExecuteRequest(RequestType.EraseData);
            if (result != null)
            {
                return true;
            }
            return false;
        }

        static void ShimmerDevice_BLEEvent(object sender, ShimmerBLEEventData e)
        {
            if (e.CurrentEvent == VerisenseBLEEvent.NewDataPacket)
            {
                
            }
        }

        static async System.Threading.Tasks.Task<bool> DisconnectDevice()
        {
            var result = await device.Disconnect();
            if (result)
            {
                device.ShimmerBLEEvent -= ShimmerDevice_BLEEvent;
                Console.WriteLine("\nUUID: " + device.Asm_uuid + " attempt disconnect \nResult: " + result + "\nNew BLE Status: " + device.GetVerisenseBLEState());
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
