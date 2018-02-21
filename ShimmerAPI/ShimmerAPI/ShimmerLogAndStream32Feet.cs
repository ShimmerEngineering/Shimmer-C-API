//NOTE: This is only provided as an example and has not been tested extensively

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InTheHand.Net;
using InTheHand.Net.Sockets;
using System.IO;

namespace ShimmerAPI
{

    public class ShimmerLogAndStream32Feet : ShimmerLogAndStream
    {

        protected String ShimmerBluetoothAddress;
        Guid g = new Guid("00001101-0000-1000-8000-00805F9B34FB");
        BluetoothEndPoint btEndpoint;
        BluetoothClient btClient = new BluetoothClient();
        BluetoothAddress addr;
        Stream peerStream;

        public ShimmerLogAndStream32Feet(String devID, String bluetoothAddress)
            : base(devID)
        {
            SetShimmerAddress(bluetoothAddress);
        }

        public ShimmerLogAndStream32Feet(String devName, String bluetoothAddress, double samplingRate, int setEnabledSensors, byte[] exg1configuration, byte[] exg2configuration)
            : base(devName, samplingRate, setEnabledSensors, exg1configuration, exg2configuration)
        {
            SetShimmerAddress(bluetoothAddress);
        }

        public ShimmerLogAndStream32Feet(String devName, String bluetoothAddress, double samplingRate, int accelRange, int gsrRange, int setEnabledSensors, bool enableLowPowerAccel, bool enableLowPowerGyro, bool enableLowPowerMag, int gyroRange, int magRange, byte[] exg1configuration, byte[] exg2configuration, bool internalexppower)
            :base(devName, samplingRate, accelRange, gsrRange, setEnabledSensors, enableLowPowerAccel, enableLowPowerGyro, enableLowPowerMag, gyroRange, magRange, exg1configuration, exg2configuration, internalexppower)
        {
            SetShimmerAddress(bluetoothAddress);
        }

        public ShimmerLogAndStream32Feet(String devID, String bluetoothAddress, double samplingRate, int AccelRange, int GyroRange, int gsrRange, int setEnabledSensors)
            : base(devID, samplingRate, AccelRange, GyroRange, gsrRange, setEnabledSensors)
        {
            SetShimmerAddress(bluetoothAddress);
        }
        protected override bool IsConnectionOpen()
        {
            return btClient.Connected;
        }

        protected override void CloseConnection()
        {
            btClient.Close();
        }
        protected override void FlushConnection()
        {
            peerStream.Flush();
        }
        protected override void FlushInputConnection()
        {
            peerStream.Flush();
        }
        protected override void WriteBytes(byte[] b, int index, int length)
        {
            peerStream.Write(b, index, length);
        }


        public override string GetShimmerAddress()
        {
            return ShimmerBluetoothAddress;
        }
        protected override int ReadByte()
        {
            int byteRead = peerStream.ReadByte();
            if (byteRead == -1 && GetState() == ShimmerBluetooth.SHIMMER_STATE_STREAMING)
            {
                CustomEventArgs newEventArgs = new CustomEventArgs((int)ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE, "Connection lost");
                OnNewEvent(newEventArgs);
                Disconnect();
            } else if (byteRead == -1 && GetState() == ShimmerBluetooth.SHIMMER_STATE_CONNECTED)
            {
                CustomEventArgs newEventArgs = new CustomEventArgs((int)ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE, "Connection lost");
                OnNewEvent(newEventArgs);
                Disconnect();
            }
            return byteRead;
        }
        protected override void OpenConnection()
        {
            btEndpoint = new BluetoothEndPoint(addr, g);
            btClient = new BluetoothClient();
            SetState(SHIMMER_STATE_CONNECTING);
            btClient.Connect(btEndpoint);
            peerStream = btClient.GetStream();
        }

        public override void SetShimmerAddress(string address)
        {
            ShimmerBluetoothAddress = address;
            addr = BluetoothAddress.Parse(address);
        }
    }
}
