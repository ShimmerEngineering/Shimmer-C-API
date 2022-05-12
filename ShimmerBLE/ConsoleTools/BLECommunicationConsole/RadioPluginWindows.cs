using shimmer.Communications;
using ShimmerBLEAPI.Devices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;

namespace ShimmerBLEAPI.Communications
{
    [Obsolete("Not used any more", true)]
    public class RadioPluginWindows : IVerisenseByteCommunication
    {
        public Guid Asm_uuid { get; set; }

        public event EventHandler<ByteLevelCommunicationEvent> CommunicationEvent;
        GattCharacteristic TX;
        GattCharacteristic RX;
        GattDeviceService Service;
        BluetoothLEDevice BluetoothLeDevice;
        ConnectivityState State = ConnectivityState.Unknown;
        TaskCompletionSource<bool> ConnectionStatusTCS { get; set; }
        public async Task<ConnectivityState> Connect()
        {
            ConnectionStatusTCS = new TaskCompletionSource<bool>();

            var address = VerisenseBLEDevice.ConvertMACAddress(Asm_uuid.ToString().Split('-')[4]);
            BluetoothLeDevice = await BluetoothLEDevice.FromBluetoothAddressAsync(address);

            BluetoothLeDevice.ConnectionStatusChanged += ConnectionLostDetection;
            BluetoothLeDevice.ConnectionStatusChanged += ConnectionStatusChangedToConnected;

            String TxID = "6E400002-B5A3-F393-E0A9-E50E24DCCA9E";
            String RxID = "6E400003-B5A3-F393-E0A9-E50E24DCCA9E";
            String ServiceID = "6E400001-B5A3-F393-E0A9-E50E24DCCA9E";
            Service = BluetoothLeDevice.GetGattService(new Guid(ServiceID));
            var rx = Service.GetCharacteristics(new Guid(RxID));
            foreach (var gc in rx)
            {
                RX = gc;
                RX.ValueChanged += Gc_ValueChanged;
            }
            var tx = Service.GetCharacteristics(new Guid(TxID));
            foreach (var gc in tx)
            {
                TX = gc;
            }
            //await Task.Delay(500); //give it time to connect
            StartConnectionStatusTimer();
            var result = await ConnectionStatusTCS.Task;
            BluetoothLeDevice.ConnectionStatusChanged -= ConnectionStatusChangedToConnected;

            if (this.BluetoothLeDevice.ConnectionStatus.Equals(BluetoothConnectionStatus.Disconnected))
            {
                State = ConnectivityState.Disconnected;
                return State;
            } else if (this.BluetoothLeDevice.ConnectionStatus.Equals(BluetoothConnectionStatus.Connected))
            {
                State = ConnectivityState.Connected;
                return State;
            }

            return ConnectivityState.Unknown;
        }

        public async Task<ConnectivityState> Disconnect()
        {
            ConnectionStatusTCS = new TaskCompletionSource<bool>();
            BluetoothLeDevice.ConnectionStatusChanged -= ConnectionLostDetection;
            BluetoothLeDevice.ConnectionStatusChanged += ConnectionStatusChangedToDisconnected;
            RX.ValueChanged -= Gc_ValueChanged;
            BluetoothLeDevice.Dispose();
            Service.Dispose();
            StartConnectionStatusTimer();
            var result = await ConnectionStatusTCS.Task;
            BluetoothLeDevice.ConnectionStatusChanged -= ConnectionStatusChangedToDisconnected;
            if (this.BluetoothLeDevice.ConnectionStatus.Equals(BluetoothConnectionStatus.Disconnected))
            {
                State = ConnectivityState.Disconnected;
                return State;
            }
            else if (this.BluetoothLeDevice.ConnectionStatus.Equals(BluetoothConnectionStatus.Connected))
            {
                State = ConnectivityState.Connected;
                return State;
            }
            return ConnectivityState.Unknown;
        }
        Timer ConnectionStatusTimer;
        protected virtual void StartConnectionStatusTimer()
        {
            ConnectionStatusTimer = new Timer(ConnectTimerCallback, null, 5000, Timeout.Infinite);
        }
        protected virtual void StopConnectionStatusTimer()
        {
            ConnectionStatusTimer.Dispose();
            ConnectionStatusTimer = null;
        }

        void ConnectTimerCallback(object state)
        {
            Console.WriteLine("Connection Time out");
            BluetoothLeDevice.Dispose();
            Service.Dispose();
            ConnectionStatusTCS.TrySetResult(false);
        }

        public void ConnectionStatusChangedToConnected(BluetoothLEDevice sender, object args)
        {
            if (sender.ConnectionStatus.Equals(BluetoothConnectionStatus.Connected))
            {
                StopConnectionStatusTimer();
                ConnectionStatusTCS.TrySetResult(true);
            }
            else
            {
                ConnectionStatusTCS.TrySetResult(false);
            }
        }

        public void ConnectionLostDetection(BluetoothLEDevice sender, object args)
        {
            if (sender.ConnectionStatus.Equals(BluetoothConnectionStatus.Disconnected))
            {
                Console.WriteLine("Connection Lost. Please reconnect.");
                State = ConnectivityState.Disconnected;
                if (CommunicationEvent != null)
                {
                    CommunicationEvent.Invoke(null, new ByteLevelCommunicationEvent { Event = ByteLevelCommunicationEvent.CommEvent.Disconnected });
                }
                BluetoothLeDevice.Dispose();
                Service.Dispose();
            }
        }

        public void ConnectionStatusChangedToDisconnected(BluetoothLEDevice sender, object args)
        {
            if (sender.ConnectionStatus.Equals(BluetoothConnectionStatus.Disconnected))
            {
                StopConnectionStatusTimer();
                ConnectionStatusTCS.TrySetResult(true);
            }
            else
                ConnectionStatusTCS.TrySetResult(false);
        }

        public async Task<bool> WriteBytes(byte[] bytes)
        {
            var writeBuffer = CryptographicBuffer.CreateFromByteArray(bytes);
            var result = await TX.WriteValueAsync(writeBuffer);
            if (result.Equals(GattCommunicationStatus.Success))
            {
                return true;
            } else
            {
                return false;
            }
        }

        private void Gc_ValueChanged(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic sender, Windows.Devices.Bluetooth.GenericAttributeProfile.GattValueChangedEventArgs args)
        {
            DataReader dataReader = DataReader.FromBuffer(args.CharacteristicValue);
            byte[] bytes = new byte[args.CharacteristicValue.Length];
            dataReader.ReadBytes(bytes);
            System.Console.WriteLine("RXB:" + BitConverter.ToString(bytes).Replace("-",""));
            if (CommunicationEvent != null)
            {
                CommunicationEvent.Invoke(null, new ByteLevelCommunicationEvent { Bytes = bytes, Event = ByteLevelCommunicationEvent.CommEvent.NewBytes });
            }
        }

        public ConnectivityState GetConnectivityState()
        {
            return State;
        }
    }
}
