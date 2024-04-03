using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using InTheHand.Net;
using System.IO;



namespace _32FeetConsoleAppLib
{


    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                BluetoothClient client = new BluetoothClient();
                BluetoothDeviceInfo[] devices = client.DiscoverDevicesInRange();
                List<BluetoothDeviceInfo> shimmerDevices = new List<BluetoothDeviceInfo>();
                bool isPaired = false;
                foreach (BluetoothDeviceInfo device in devices)
                {
                    // if the device is not paired, pair it!

                    if (device.DeviceName.Contains("Shimmer"))
                    {
                        shimmerDevices.Add(device);
                        Console.WriteLine(device.DeviceAddress + ";" + device.DeviceName + ";" + device.Authenticated);
                    }
                }
            } catch (Exception ex)
            {
                //Console.WriteLine("Fail");
            }

        }
    }
}
