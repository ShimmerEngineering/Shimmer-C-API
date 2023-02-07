using System;
using System.IO;
using InTheHand.Bluetooth;
using System.Collections.Concurrent;
using System.Threading;

namespace ShimmerAPI
{
    public class ShimmerLogAndStreamBLE : ShimmerLogAndStream
    {
        private BluetoothDevice bluetoothDevice { get; set; }
        private GattService ServiceTXRX { get; set; }
        private GattCharacteristic UartTX { get; set; }
        private GattCharacteristic UartRX { get; set; }
        protected String macAddress { get; set; }
        ConcurrentQueue<byte> cq = new ConcurrentQueue<byte>();

        public ShimmerLogAndStreamBLE(String devID, String bMacAddress)
          : base(devID)
        {
            macAddress = bMacAddress;
        }

        public override string GetShimmerAddress()
        {
            return macAddress;
        }

        public override void SetShimmerAddress(string address)
        {
            macAddress = address;
        }

        protected override void CloseConnection()
        {
            bluetoothDevice.GattServerDisconnected -= Device_GattServerDisconnected;
            bluetoothDevice.Gatt.Disconnect();
        }

        protected override void FlushConnection()
        {

        }

        protected override void FlushInputConnection()
        {

        }

        protected override bool IsConnectionOpen()
        {
            if(bluetoothDevice == null)
            {
                return false;
            }
            return bluetoothDevice.Gatt.IsConnected;
        }

        protected override async void OpenConnection()
        {
            try
            {
                SetState(SHIMMER_STATE_CONNECTING);
                bluetoothDevice = await BluetoothDevice.FromIdAsync(macAddress);
                bluetoothDevice.GattServerDisconnected += Device_GattServerDisconnected;
                await bluetoothDevice.Gatt.ConnectAsync();
                Console.WriteLine("current mtu value" + bluetoothDevice.Gatt.Mtu);
                BluetoothUuid TxID = BluetoothUuid.FromGuid(new Guid("49535343-8841-43f4-a8d4-ecbe34729bb3"));
                BluetoothUuid RxID = BluetoothUuid.FromGuid(new Guid("49535343-1e4d-4bd9-ba61-23c647249616"));
                BluetoothUuid ServiceID = BluetoothUuid.FromGuid(new Guid("49535343-fe7d-4ae5-8fa9-9fafd205e455"));
                ServiceTXRX = await bluetoothDevice.Gatt.GetPrimaryServiceAsync(ServiceID);
                UartTX = await ServiceTXRX.GetCharacteristicAsync(TxID);
                UartRX = await ServiceTXRX.GetCharacteristicAsync(RxID);

                UartRX.CharacteristicValueChanged += Gc_ValueChanged;
                await UartRX.StartNotificationsAsync();
                Console.WriteLine("current mtu value" + bluetoothDevice.Gatt.Mtu);
            }
            catch (Exception ex)
            {
                bluetoothDevice.GattServerDisconnected -= Device_GattServerDisconnected;
                bluetoothDevice.Gatt.Disconnect();
                Console.WriteLine("Radio Plugin 32Feet Exception " + ex.Message);
            }
        }

        private void Device_GattServerDisconnected(object sender, EventArgs e)
        {
            //connection lost
            if (!((BluetoothDevice)sender).Gatt.IsConnected)
            {
                Console.WriteLine("Connection Lost. Please reconnect.");
                bluetoothDevice.GattServerDisconnected -= Device_GattServerDisconnected;
                bluetoothDevice.Gatt.Disconnect();
            }
        }

        private void Gc_ValueChanged(object sender, GattCharacteristicValueChangedEventArgs args)
        {
            Console.WriteLine("RXB:" + BitConverter.ToString(args.Value).Replace("-", ""));
            for (int i = 0; i < args.Value.Length; i++)
            {
                cq.Enqueue(args.Value[i]);
            }
        }

        protected override int ReadByte()
        {
            if (GetState() != SHIMMER_STATE_NONE)
            {
                byte b = 0xFF;
                Timer timer = new Timer((obj) => throw new TimeoutException(), null, 30000, Timeout.Infinite);
                while (!cq.TryDequeue(out b))
                {

                }
                timer.Dispose();
                return b;
            }
            throw new InvalidOperationException();
        }

        protected override async void WriteBytes(byte[] b, int index, int length)
        {
            if (GetState() != SHIMMER_STATE_NONE)
            {
                await UartTX.WriteValueWithoutResponseAsync(b);
            }
        }
    }
}
