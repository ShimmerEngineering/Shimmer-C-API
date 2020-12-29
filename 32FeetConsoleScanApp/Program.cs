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
                    }
                }
                BluetoothDeviceInfo bdinfo = null;
                if (shimmerDevices.Count > 1)
                {
                    System.Console.WriteLine("Multiple Shimmer Devices Detected, Please Key In Address You Want To Pair:");
                    foreach (BluetoothDeviceInfo device in shimmerDevices)
                    {
                        System.Console.WriteLine(device.DeviceName + ";" + device.DeviceAddress);
                    }
                    string address = System.Console.ReadLine();
                    foreach (BluetoothDeviceInfo device in shimmerDevices)
                    {
                        if (device.DeviceAddress.ToString().Equals(address))
                        {
                            bdinfo = device;
                        }
                    }
                }else if (shimmerDevices.Count == 1)
                {
                    bdinfo = shimmerDevices[0];
                }
                if (bdinfo != null)
                {
                    // replace DEVICE_PIN here, synchronous method, but fast
                    if (!bdinfo.Authenticated)
                    {
                        isPaired = BluetoothSecurity.PairRequest(bdinfo.DeviceAddress, "1234");
                        if (isPaired)
                        {
                            bdinfo.SetServiceState(BluetoothService.SerialPort, true, true);
                        }
                    }
                    else
                    {
                        isPaired = true;
                    }
                    if (isPaired)
                    {
                        // now it is paired
                        Console.WriteLine(bdinfo.DeviceAddress+ ";" + bdinfo.DeviceName + ";Is Paired"); ;
                    }
                    else
                    {
                        // pairing failed
                        Console.WriteLine(bdinfo.DeviceAddress + ";" + bdinfo.DeviceName + ";Is Not Paired");
                    }
                }
                
            } catch (Exception ex)
            {
                Console.WriteLine("Fail");
            }

        }
    }
}
