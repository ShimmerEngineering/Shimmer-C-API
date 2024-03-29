﻿using System;
using System.Collections.Generic;

using Hoho.Android.UsbSerial.Driver;
using Hoho.Android.UsbSerial.Extensions;
using Hoho.Android.UsbSerial.Util;
using Android.Content;
using Android.Hardware.Usb;
using shimmer.Communications;
using System.Threading.Tasks;

namespace ShimmerBLEAPI.Android.Communications
{
    public class SerialPortByteCommunicationAndroid : IVerisenseByteCommunication
    {
        public static readonly int VID = 0x1915;
        public static readonly int PID = 0x520F;
        public UsbManager usbManager;
        UsbSerialPort port;
        UsbSerialDriver driver;
        SerialInputOutputManager serialIoManager;
        public static Context context { get; set; }

        public Guid Asm_uuid { get; set; }
        public string id { get; set; }

        public event EventHandler<ByteLevelCommunicationEvent> CommunicationEvent;
        public String ComPort { get; set; }
        ConnectivityState state = ConnectivityState.Disconnected;

        /// <summary>
        /// Note that it is necessary to set the context before connecting to a serial device
        /// </summary>
        public SerialPortByteCommunicationAndroid()
        {

        }

        public SerialPortByteCommunicationAndroid(Context context1)
        {
            context = context1;
        }

        public static void setContext(Context context1)
        {
            context = context1;
        }

        public async Task<ConnectivityState> Connect()
        {
            if(context == null)
            {
                return ConnectivityState.Disconnected;
            }
            usbManager = context.GetSystemService(Context.UsbService) as UsbManager;
            var drivers = await FindAllDriversAsync(usbManager);
            // get first driver for now
            foreach(var x in drivers)
            {
                driver = (UsbSerialDriver)x;
                break;
            }
            port = driver.Ports[0];
            var permissionGranted = await usbManager.RequestPermissionAsync(port.Driver.Device, context);
            if (permissionGranted)
            {
                serialIoManager = new SerialInputOutputManager(port)
                {
                    BaudRate = 115200,
                    DataBits = 8,
                    StopBits = StopBits.One,
                    Parity = Parity.None,
                };
                serialIoManager.DataReceived += (sender, e) => {
                    DataReceived(e.Data);
                };
                serialIoManager.ErrorReceived += (sender, e) => {
                    ErrorReceived();
                };

                try
                {
                    serialIoManager.Open(usbManager);
                    port.SetDTR(true);
                    port.SetRTS(true);
                }
                catch (Java.IO.IOException e)
                {
                    return ConnectivityState.Disconnected;
                }
                state = ConnectivityState.Connected;
                return ConnectivityState.Connected;
            }
            
            return ConnectivityState.Disconnected;
        }

        internal static Task<IList<IUsbSerialDriver>> FindAllDriversAsync(UsbManager usbManager)
        {
            var table = UsbSerialProber.DefaultProbeTable;

            //Verisense
            table.AddProduct(VID, PID, typeof(CdcAcmSerialDriver));

            var prober = new UsbSerialProber(table);

            return prober.FindAllDriversAsync(usbManager);
        }

        public async Task<ConnectivityState> Disconnect()
        {
            if (serialIoManager != null && serialIoManager.IsOpen)
            {
                try
                {
                    serialIoManager.Close();
                }
                catch (Java.IO.IOException)
                {
                    // ignore
                }
            }
            state = ConnectivityState.Disconnected;
            return ConnectivityState.Disconnected;
        }

        void DataReceived(byte[] data)
        {
            if (CommunicationEvent != null)
            {
                CommunicationEvent.Invoke(null, new ByteLevelCommunicationEvent { Bytes = data, Event = shimmer.Communications.ByteLevelCommunicationEvent.CommEvent.NewBytes });
            }
        }

        void ErrorReceived()
        {
            Console.WriteLine("Error Received");
        }

        public ConnectivityState GetConnectivityState()
        {
            return state;
        }

        public async Task<bool> WriteBytes(byte[] bytes)
        {
            if (serialIoManager.IsOpen)
            {
                port.Write(bytes, 1000);
            }
            return true;
        }
    }
}
