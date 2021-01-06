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
                if (args.Length > 0)
                {
                    //"00066679E454"
                    BluetoothAddress address = BluetoothAddress.Parse(args[0]);
                    bool isPaired = BluetoothSecurity.PairRequest(address, "1234");
                    if (isPaired)
                    {
                        BluetoothDeviceInfo bdinfo = new BluetoothDeviceInfo(address);
                        bdinfo.SetServiceState(BluetoothService.SerialPort, true, true);
                        Console.WriteLine(bdinfo.DeviceAddress + " Is Paired");
                    }
                }
                
            } catch (Exception ex)
            {
                Console.WriteLine("Fail");
            }

        }
    }
}
