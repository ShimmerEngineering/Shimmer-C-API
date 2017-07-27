//NOTE: This is only provided as an example and has not been tested extensively

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

    public class ShimmerLogAndStreamXamarin : ShimmerLogAndStream
    {

        protected String ShimmerBluetoothAddress;
        UUID mSPP_UUID = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB");
        String BluetoothAddress;
        BluetoothSocket Socket;
        OutputStream output;
        InputStream input;

        public ShimmerLogAndStreamXamarin(String devID, String bluetoothAddress)
            : base(devID)
        {
            SetShimmerAddress(bluetoothAddress);
        }

        public ShimmerLogAndStreamXamarin(String devName, String bluetoothAddress, double samplingRate, int accelRange, int gsrRange, int setEnabledSensors, bool enableLowPowerAccel, bool enableLowPowerGyro, bool enableLowPowerMag, int gyroRange, int magRange, byte[] exg1configuration, byte[] exg2configuration, bool internalexppower)
            :base(devName, samplingRate, accelRange, gsrRange, setEnabledSensors, enableLowPowerAccel, enableLowPowerGyro, enableLowPowerMag, gyroRange, magRange, exg1configuration, exg2configuration, internalexppower)
        {
            SetShimmerAddress(bluetoothAddress);
        }

        public ShimmerLogAndStreamXamarin(String devID, String bluetoothAddress, double samplingRate, int AccelRange, int GyroRange, int gsrRange, int setEnabledSensors)
            : base(devID, samplingRate, AccelRange, GyroRange, gsrRange, setEnabledSensors)
        {
            SetShimmerAddress(bluetoothAddress);
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
            int byteRead = -1;
            try
            {
                byteRead = input.Read();
            } catch (Java.IO.IOException)
            {
                CustomEventArgs newEventArgs = new CustomEventArgs((int)ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE, "Connection lost");
                OnNewEvent(newEventArgs);
                Disconnect();
            }
            return byteRead;
        }

        protected override void WriteBytes(byte[] b, int index, int length)
        {
            output.Write(b, index, length);
        }

        public override string GetShimmerAddress()
        {
            return BluetoothAddress;
        }

        protected override void FlushConnection()
        {

        }

        protected override void FlushInputConnection()
        {

        }

        public override void SetShimmerAddress(string address)
        {
            BluetoothAddress = address;
        }
    }
}
