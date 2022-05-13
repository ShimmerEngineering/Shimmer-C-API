using System;
using shimmer.Models;
using shimmer.Helpers;
using static shimmer.Models.ShimmerBLEEventData;
using System.IO;

namespace VerisenseConfigureAndSyncConsole
{
    internal class Program
    {
        static VerisenseBLEDeviceMatlab device;

        static async System.Threading.Tasks.Task Main(string[] args)
        {
            //args[0] - uuid
            //args[1] - sync / write prod config
            //args[2] - bin file path / op config bytes
            //args[3] - trial name
            //args[4] - participant id
            if(args.Length >= 2)
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
                    else if (args[1] == "WRITE_OP_CONFIG")
                    {
                        await WriteOpConfig(args[2]);
                    }
                }
                await DisconnectDevice();
            }
            else
            {
                Console.WriteLine("At least two arguments are needed");
                Console.WriteLine("Usage: start VerisenseConfigureAndSyncConsole.exe [-uuid] [-options] [args...]");
                Console.WriteLine("where options include:");
                Console.WriteLine("\t-DATA_SYNC (with three arguments)");
                Console.WriteLine("\t\t-bin file path");
                Console.WriteLine("\t\t-trial name");
                Console.WriteLine("\t\t-participant id");
                Console.WriteLine("\t -WRITE_OP_CONFIG (with one argument)");
                Console.WriteLine("\t\t-operational config bytes e.g. 5A-97-00-00-00-...");
                Console.WriteLine("e.g. start VerisenseConfigureAndSyncConsole.exe 00000000-0000-0000-0000-d02b463da2bb DATA_SYNC C:\\Users\\UserName\\Desktop trialA participantB");
            }
        }

        static async System.Threading.Tasks.Task<bool> ConnectDevice(string uuid)
        {
            Console.WriteLine("CONNECT");
            device = new VerisenseBLEDeviceMatlab(uuid, "");
            device.ShimmerBLEEvent += ShimmerDevice_BLEEvent;
            bool result = await device.Connect(true);
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
            VerisenseBLEDeviceMatlab.path = path;
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
