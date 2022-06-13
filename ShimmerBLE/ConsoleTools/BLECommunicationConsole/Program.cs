using System;
using ShimmerBLEAPI.Communications;
using ShimmerBLEAPI.Devices;
using System.Threading;
using shimmer.Models;
using System.Threading.Tasks;
using System.Diagnostics;
using shimmer.Communications;
using shimmer.Services;
using System.Numerics;
using System.Globalization;

namespace BLECommunicationConsole
{
    class Program
    {
        static RadioPlugin32Feet dev;
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            //string uuid = "00000000-0000-0000-0000-d02b463da2bb";
            //string uuid = "00000000-0000-0000-0000-e7ec37a0d234";
            //string uuid = "00000000-0000-0000-0000-e7452c6d6f14";
            if (args.Length > 0)
            {
                string uuid = args[0];
                //Write110000 - read status
                //Write120000 - sync
                dev = new RadioPlugin32Feet();
                dev.Asm_uuid = Guid.Parse(uuid);
            }
            else
            {
                Console.WriteLine("Error no uuid specified");
                return;
            }
            try
            {
                while (true)
                {
                    string action = ReadActionFromJava();
                    if (action != null)
                    {
                        if (action == "Connect")
                        {
                            Console.WriteLine("Connecting");
                            ConnectivityState State = await dev.Connect();
                            if (State == ConnectivityState.Connected)
                            {
                                Console.WriteLine("Connected");
                            }
                            else
                            {
                                Console.WriteLine("Connect failed");
                            }
                        }
                        else if (action.Contains("Write"))
                        {
                            if (dev.GetConnectivityState() == ConnectivityState.Connected)
                            {
                                var payload = action.Split("Write")[1];
                                byte[] payloadBytes = BigInteger.Parse(payload, NumberStyles.HexNumber).ToByteArray();
                                Array.Reverse(payloadBytes);
                                var result = await dev.WriteBytes(payloadBytes);
                                if (result == true)
                                {
                                    Console.WriteLine("Write success");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Connect first");
                            }

                        }
                        else if (action == "Disconnect")
                        {
                            if (dev.GetConnectivityState() == ConnectivityState.Connected)
                            {
                                ConnectivityState State = await dev.Disconnect();
                                if (State == ConnectivityState.Disconnected)
                                {
                                    Console.WriteLine("Disconnected");
                                }
                                else
                                {
                                    Console.WriteLine("Disconnect Failed");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Connect first");
                            }
                        }
                        else if (action == "Stop")
                        {
                            return; //Environment.Exit(0);
                        }
                        else
                        {
                            Console.WriteLine("Unrecognized Command");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static string ReadActionFromJava()
        {
            String line = Console.ReadLine();
            return line;
        }
    }
}