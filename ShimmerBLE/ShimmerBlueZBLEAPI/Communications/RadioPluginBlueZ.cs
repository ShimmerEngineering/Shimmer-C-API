using HashtagChris.DotNetBlueZ;
using HashtagChris.DotNetBlueZ.Extensions;
using shimmer.Communications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace shimmer.Communications
{
    public class RadioPluginBlueZ : IVerisenseByteCommunication
    {
        public static bool ShowRXB = true;
        public Guid Asm_uuid { get; set; }

        public event EventHandler<ByteLevelCommunicationEvent> CommunicationEvent;
        TaskCompletionSource<bool> ConnectionStatusTCS { get; set; }
        ConnectivityState State = ConnectivityState.Unknown;
        Device bluetoothDevice;
        IGattService1 ServiceTXRX { get; set; }
        GattCharacteristic UartTX { get; set; }
        GattCharacteristic UartRX { get; set; }

        static TimeSpan timeout = TimeSpan.FromSeconds(15);
        public async Task<ConnectivityState> Connect()
        {
            try
            {
                IAdapter1 adapter;
                var adapters = await BlueZManager.GetAdaptersAsync();
                if (adapters.Count == 0)
                {
                    throw new Exception("No Bluetooth adapters found.");
                }

                //adapter = await BlueZManager.GetAdapterAsync("hci0");
                adapter = adapters.First();

                var adapterPath = adapter.ObjectPath.ToString();
                var adapterName = adapterPath.Substring(adapterPath.LastIndexOf("/") + 1);
                Console.WriteLine($"Using Bluetooth adapter {adapterName}");

                // Find the Bluetooth peripheral.
                string macAddress = Asm_uuid.ToString().Split('-')[4].ToUpper();
                for (int i = 2; i < macAddress.Length; i += 2)
                {
                    macAddress = macAddress.Insert(i, ":");
                    i++;
                }
                bluetoothDevice = await adapter.GetDeviceAsync(macAddress);
                if (bluetoothDevice == null)
                {
                    throw new Exception("Device not found.");
                }

                bluetoothDevice.Connected += device_ConnectedAsync;
                bluetoothDevice.Disconnected += device_DisconnectedAsync;
                bluetoothDevice.ServicesResolved += device_ServicesResolvedAsync;

                await bluetoothDevice.ConnectAsync();

                await bluetoothDevice.WaitForPropertyValueAsync("Connected", value: true, timeout);
                Console.WriteLine("Connected.");

                Console.WriteLine("Waiting for services to resolve...");
                await bluetoothDevice.WaitForPropertyValueAsync("ServicesResolved", value: true, timeout);

                ServiceTXRX = await bluetoothDevice.GetServiceAsync("6E400001-B5A3-F393-E0A9-E50E24DCCA9E");
                UartTX = await ServiceTXRX.GetCharacteristicAsync("6E400002-B5A3-F393-E0A9-E50E24DCCA9E");
                UartRX = await ServiceTXRX.GetCharacteristicAsync("6E400003-B5A3-F393-E0A9-E50E24DCCA9E");

                UartRX.Value += Gc_ValueChanged;

                return State;
            }
            catch (Exception ex)
            {
                bluetoothDevice.Connected -= device_ConnectedAsync;
                bluetoothDevice.Disconnected -= device_DisconnectedAsync;
                bluetoothDevice.ServicesResolved -= device_ServicesResolvedAsync;
                Console.WriteLine(ex.ToString());
                return ConnectivityState.Disconnected;
            }
        }

        private async Task Gc_ValueChanged(GattCharacteristic characteristic, GattCharacteristicValueEventArgs e)
        {
            if (ShowRXB)
            {
                Console.WriteLine("RXB:" + BitConverter.ToString(e.Value));
            }

            if (CommunicationEvent != null)
            {
                CommunicationEvent.Invoke(null, new ByteLevelCommunicationEvent { Bytes = e.Value, Event = shimmer.Communications.ByteLevelCommunicationEvent.CommEvent.NewBytes });
            }
        }

        private async Task device_ConnectedAsync(Device device, BlueZEventArgs e)
        {
            try
            {
                if (e.IsStateChange)
                {
                    Console.WriteLine($"Connected to {await device.GetAddressAsync()}");
                }
                else
                {
                    Console.WriteLine($"Already connected to {await device.GetAddressAsync()}");
                }
                State = ConnectivityState.Connected;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
        }

        private async Task device_DisconnectedAsync(Device device, BlueZEventArgs e)
        {
            try
            {
                Console.WriteLine($"Disconnected from {await device.GetAddressAsync()}");
                State = ConnectivityState.Disconnected;
                ConnectionStatusTCS.TrySetResult(true);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
        }

        private static async Task device_ServicesResolvedAsync(Device device, BlueZEventArgs e)
        {
            try
            {
                if (e.IsStateChange)
                {
                    Console.WriteLine($"Services resolved for {await device.GetAddressAsync()}");
                }
                else
                {
                    Console.WriteLine($"Services already resolved for {await device.GetAddressAsync()}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
        }

        public async Task<ConnectivityState> Disconnect()
        {
            ConnectionStatusTCS = new TaskCompletionSource<bool>();
            await bluetoothDevice.DisconnectAsync();
            await ConnectionStatusTCS.Task;
            UartRX.Value -= Gc_ValueChanged;
            bluetoothDevice.Connected -= device_ConnectedAsync;
            bluetoothDevice.Disconnected -= device_DisconnectedAsync;
            bluetoothDevice.ServicesResolved -= device_ServicesResolvedAsync;

            return State;
        }


        public ConnectivityState GetConnectivityState()
        {
            return State;
        }

        public async Task<bool> WriteBytes(byte[] bytes)
        {
            IDictionary<string, object> options = new Dictionary<string, object>();
            await UartTX.WriteValueAsync(bytes, options);
            return true;
        }
    }
}
