using InTheHand.Bluetooth;
using shimmer.Communications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace shimmer.Communications
{
    public class RadioPlugin32Feet : IVerisenseByteCommunication
    {
        public Guid Asm_uuid { get; set; }

        public event EventHandler<ByteLevelCommunicationEvent> CommunicationEvent;
        TaskCompletionSource<bool> ConnectionStatusTCS { get; set; }
        ConnectivityState State = ConnectivityState.Unknown;
        BluetoothDevice bluetoothDevice;
        GattService ServiceTXRX { get; set; }
        GattCharacteristic UartTX { get; set; }
        GattCharacteristic UartRX { get; set; }

        public async Task<ConnectivityState> Connect()
        {
            try
            {
                ConnectionStatusTCS = new TaskCompletionSource<bool>();
                bluetoothDevice = await BluetoothDevice.FromIdAsync(Asm_uuid.ToString().Split('-')[4]);
                bluetoothDevice.GattServerDisconnected += Device_GattServerDisconnected;
                await bluetoothDevice.Gatt.ConnectAsync();

                BluetoothUuid TxID = BluetoothUuid.FromGuid(new Guid("6E400002-B5A3-F393-E0A9-E50E24DCCA9E"));
                BluetoothUuid RxID = BluetoothUuid.FromGuid(new Guid("6E400003-B5A3-F393-E0A9-E50E24DCCA9E"));
                BluetoothUuid ServiceID = BluetoothUuid.FromGuid(new Guid("6E400001-B5A3-F393-E0A9-E50E24DCCA9E"));

                ServiceTXRX = await bluetoothDevice.Gatt.GetPrimaryServiceAsync(ServiceID);
                UartTX = await ServiceTXRX.GetCharacteristicAsync(TxID);
                UartRX = await ServiceTXRX.GetCharacteristicAsync(RxID);

                UartRX.CharacteristicValueChanged += Gc_ValueChanged;
                await UartRX.StartNotificationsAsync();

                if (bluetoothDevice.Gatt.IsConnected)
                {
                    State = ConnectivityState.Connected;
                    return State;
                }
                else
                {
                    bluetoothDevice.GattServerDisconnected -= Device_GattServerDisconnected;
                    bluetoothDevice.Gatt.Disconnect();
                    State = ConnectivityState.Disconnected;
                    return State;
                }
            }
            catch (Exception ex)
            {
                bluetoothDevice.GattServerDisconnected -= Device_GattServerDisconnected;
                bluetoothDevice.Gatt.Disconnect();
                Console.WriteLine("Radio Plugin 32Feet Exception " + ex.Message + "Please retry to connect");
                return ConnectivityState.Disconnected;
            }
        }

        public async Task<ConnectivityState> Disconnect()
        {
            ConnectivityState test = State;
            ConnectionStatusTCS = new TaskCompletionSource<bool>();
            bluetoothDevice.Gatt.Disconnect();
            await ConnectionStatusTCS.Task;
            UartRX.CharacteristicValueChanged -= Gc_ValueChanged;
            bluetoothDevice.GattServerDisconnected -= Device_GattServerDisconnected;

            UartRX = null;
            UartTX = null;
            GC.Collect();
            Thread.Sleep(3000);
            return State;
        }

        private void Device_GattServerDisconnected(object sender, EventArgs e)
        {

            if (((BluetoothDevice)sender).Gatt.IsConnected)
            {
                //disconnect from UI
                State = ConnectivityState.Disconnected;
                if (CommunicationEvent != null)
                {
                    CommunicationEvent.Invoke(null, new ByteLevelCommunicationEvent { Event = ByteLevelCommunicationEvent.CommEvent.Disconnected });
                }
                ConnectionStatusTCS.TrySetResult(true);
            }
            else
            {
                //connection lost
                Console.WriteLine("Connection Lost. Please reconnect.");
                //to prevent auto reconnect
                bluetoothDevice.GattServerDisconnected -= Device_GattServerDisconnected;
                bluetoothDevice.Gatt.Disconnect();
            }

        }

        public ConnectivityState GetConnectivityState()
        {
            return State;
        }

        public async Task<bool> WriteBytes(byte[] bytes)
        {
            await UartTX.WriteValueWithoutResponseAsync(bytes);
            return true;
        }

        private void Gc_ValueChanged(object sender, GattCharacteristicValueChangedEventArgs args)
        {
            Console.WriteLine("RXB:" + BitConverter.ToString(args.Value).Replace("-", ""));

            if (CommunicationEvent != null)
            {
                CommunicationEvent.Invoke(null, new ByteLevelCommunicationEvent { Bytes = args.Value, Event = shimmer.Communications.ByteLevelCommunicationEvent.CommEvent.NewBytes });
            }
        }
    }
}
