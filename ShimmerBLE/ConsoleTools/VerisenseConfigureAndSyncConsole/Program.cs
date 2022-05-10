using System;
using shimmer.Models;
using shimmer.Helpers;
using static shimmer.Models.ShimmerBLEEventData;

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

            var result = await ConnectDevice(args[0]);
            if (result)
            {
                if (args[1] == "DATA_SYNC")
                {
                    await StartDataSync(args[2]);
                }
                else if (args[1] == "WRITE_OP_CONFIG")
                {
                    await WriteOpConfig(args[2]);
                }
            }
            await DisconnectDevice();

            //var result = await ConnectDevice("00000000-0000-0000-0000-d02b463da2bb");
            //if (result)
            //{
            //    //await StartDataSync("C:\\Users\\WeiWentan\\Desktop");
            //    await WriteOpConfig("5A-97-00-00-00-30-30-00-7F-00-D8-0F-09-16-24-0C-80-00-00-00-00-00-00-00-00-00-00-00-00-00-03-F4-18-3C-00-0A-0F-00-18-3C-00-0A-0F-00-18-3C-00-0A-0F-00-FF-FF-AA-01-03-3C-00-0E-00-00-63-28-CC-CC-1E-00-0A-00-00-00-00-01");
            //}
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

        static async System.Threading.Tasks.Task<bool> StartDataSync(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                Console.WriteLine("Please enter a valid path");
                return false;
            }
            VerisenseBLEDeviceMatlab.path = path;

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
