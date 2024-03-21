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
                Console.WriteLine("Please Key In Shimmer Address You Want To Pair:");
                string keyinAddress = Console.ReadLine();
                BluetoothAddress address = BluetoothAddress.Parse(keyinAddress);
                bool isPaired = BluetoothSecurity.PairRequest(address, "1234");
                if (isPaired)
                {
                    BluetoothDeviceInfo bdinfo = new BluetoothDeviceInfo(address);
                    bdinfo.SetServiceState(BluetoothService.SerialPort, true, true);
                    Console.WriteLine(bdinfo.DeviceAddress + " Is Paired");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Fail");
            }

        }
    }
}
