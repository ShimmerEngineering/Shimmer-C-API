using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Bluetooth;
using Java.Util;
using Java.IO;



namespace ShimmerAPI
{
    class ShimmerAndroidXamarin : ShimmerBluetooth
    {
        UUID mSPP_UUID = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB");
        String BluetoothAddress;
        BluetoothSocket Socket;
        OutputStream output;
        InputStream input;
        public ShimmerAndroidXamarin(String devID, String bAddress)
            : base(devID)
        {
            BluetoothAddress = bAddress;
        }
        protected override void CloseConnection()
        {
            Socket.Close();
        }
        protected override bool IsConnectionOpen()
        {
            if (Socket == null)
            {
                return false;
            }
            return Socket.IsConnected;
        }

        protected override void OpenConnection()
        {
            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
            BluetoothDevice device = adapter.GetRemoteDevice(BluetoothAddress);

            Socket = device.CreateInsecureRfcommSocketToServiceRecord(mSPP_UUID);
            adapter.CancelDiscovery();
            Socket.Connect();
            output = new Java.IO.DataOutputStream(Socket.OutputStream);
            input = new Java.IO.DataInputStream(Socket.InputStream);
        }

        protected override int ReadByte()
        {
            return input.Read();
        }

        protected override void WriteBytes(byte[] b, int index, int length)
        {
            output.Write(b, index, length);
        }

        protected override string GetShimmerAddress()
        {
            return BluetoothAddress;
        }

        protected override void FlushConnection()
        {
            
        }

        protected override void FlushInputConnection()
        {
           
        }

  
    
    }
}