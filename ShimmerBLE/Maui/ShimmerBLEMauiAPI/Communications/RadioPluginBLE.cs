using shimmer.Communications;
using System;
using System.Linq;
using System.Threading.Tasks;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System.Threading;

namespace ShimmerBLEMauiAPI.Communications
{
    internal class RadioPluginBLE : IVerisenseByteCommunication
    {
        public static bool ShowRXB = true;

        public Guid Asm_uuid { get; set; }

        public event EventHandler<ByteLevelCommunicationEvent> CommunicationEvent;

        private IAdapter _adapter;
        private IDevice _device;
        private IService _serviceTXRX;
        private ICharacteristic _uartTX;
        private ICharacteristic _uartRX;

        private ConnectivityState State = ConnectivityState.Unknown;
        private TaskCompletionSource<bool> ConnectionStatusTCS;

        private readonly IBluetoothLE _bluetoothLE;

        public RadioPluginBLE()
        {
            _bluetoothLE = CrossBluetoothLE.Current;
            _adapter = CrossBluetoothLE.Current.Adapter;
        }

        public async Task<ConnectivityState> Connect()
        {
            try
            {
                ConnectionStatusTCS = new TaskCompletionSource<bool>();

                var knownDevices = _adapter.GetSystemConnectedOrPairedDevices(new Guid[] { Asm_uuid });

                _device = knownDevices.FirstOrDefault(d => d.Id == Asm_uuid);
                if (_device == null)
                {
                    State = ConnectivityState.Disconnected;
                    return State;
                }

                // With this corrected implementation:
                _adapter.DeviceDisconnected += Device_Disconnected;

                await _adapter.ConnectToDeviceAsync(_device);

                var services = await _device.GetServicesAsync();
                _serviceTXRX = services.FirstOrDefault(s => s.Id == new Guid("6E400001-B5A3-F393-E0A9-E50E24DCCA9E"));
                if (_serviceTXRX == null)
                {
                    throw new Exception("TXRX Service not found");
                }

                _uartTX = (await _serviceTXRX.GetCharacteristicsAsync())
                    .FirstOrDefault(c => c.Id == new Guid("6E400002-B5A3-F393-E0A9-E50E24DCCA9E"));

                _uartRX = (await _serviceTXRX.GetCharacteristicsAsync())
                    .FirstOrDefault(c => c.Id == new Guid("6E400003-B5A3-F393-E0A9-E50E24DCCA9E"));

                if (_uartRX != null)
                {
                    _uartRX.ValueUpdated += UartRX_ValueUpdated;
                    await _uartRX.StartUpdatesAsync();
                }

                State = ConnectivityState.Connected;
                return State;
            }
            catch (Exception ex)
            {
                Console.WriteLine("RadioPluginBLE Connection Error: " + ex.Message);
                Disconnect();
                return ConnectivityState.Disconnected;
            }
        }

        public async Task<ConnectivityState> Disconnect()
        {
            try
            {
                if (_uartRX != null)
                {
                    _uartRX.ValueUpdated -= UartRX_ValueUpdated;
                    await _uartRX.StopUpdatesAsync();
                }

                if (_device != null)
                {
                    // With this corrected implementation:
                    _adapter.DeviceDisconnected += Device_Disconnected;
                    await _adapter.DisconnectDeviceAsync(_device);
                }

                _uartTX = null;
                _uartRX = null;
                _serviceTXRX = null;
                _device = null;

                State = ConnectivityState.Disconnected;
                GC.Collect();
                Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during Disconnect: " + ex.Message);
            }

            return State;
        }

        private void Device_Disconnected(object sender, DeviceEventArgs e)
        {
            State = ConnectivityState.Disconnected;
            CommunicationEvent?.Invoke(this, new ByteLevelCommunicationEvent
            {
                Event = ByteLevelCommunicationEvent.CommEvent.Disconnected
            });
            ConnectionStatusTCS?.TrySetResult(true);
        }

        public ConnectivityState GetConnectivityState()
        {
            return State;
        }

        public async Task<bool> WriteBytes(byte[] bytes)
        {
            if (_uartTX != null)
            {
                // Replace WriteWithoutResponseAsync with WriteAsync as per the ICharacteristic interface  
                await _uartTX.WriteAsync(bytes);
                return true;
            }

            return false;
        }

        private void UartRX_ValueUpdated(object sender, CharacteristicUpdatedEventArgs e)
        {
            if (ShowRXB)
            {
                Console.WriteLine("RXB:" + BitConverter.ToString(e.Characteristic.Value).Replace("-", ""));
            }

            CommunicationEvent?.Invoke(this, new ByteLevelCommunicationEvent
            {
                Bytes = e.Characteristic.Value,
                Event = ByteLevelCommunicationEvent.CommEvent.NewBytes
            });
        }
    }
}
