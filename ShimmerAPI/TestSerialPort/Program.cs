using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ShimmerAPI.ShimmerBluetooth;

namespace TestSerialPort
{
    class Program
    {
        static void Main(string[] args)
        {


            System.IO.Ports.SerialPort SerialPort = new System.IO.Ports.SerialPort();
            int packetSize = 23; //LN Accel + EXG TEST SIGNAL Using L&S0.11
            SerialPort.BaudRate = 115200;
            SerialPort.PortName = "COM46";
            SerialPort.ReadTimeout = 2000;
            SerialPort.WriteTimeout = 2000;
            try
            {
                SerialPort.Open();
            }
            catch
            {

            }

            SerialPort.Write(new byte[1] { (byte)PacketTypeShimmer2.START_STREAMING_COMMAND }, 0, 1);
            SerialPort.ReadByte();

            for (int i = 0; i < 1000000; i++)
            {
                for (int j = 0; j < (packetSize + 1); j++) {
                    try
                    {
                        int b = SerialPort.ReadByte();
                        System.Console.Write(j + "," + b + ",");
                    } catch (System.TimeoutException)
                    {
                        
                        System.Console.WriteLine(j + " TimeoutException");
                        break;
                    }
                }
                System.Console.WriteLine();
            }
        }
    }
}
