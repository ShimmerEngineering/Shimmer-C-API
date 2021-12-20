using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using ShimmerBLEAPI.Communications;
using ShimmerBLEAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace shimmer.Communications
{
    public class DeviceManagerPluginBLE : IVerisenseBLEManager
    {
        public event EventHandler<BLEManagerEvent> BLEManagerEvent;
        protected readonly IAdapter Adapter;
        public DeviceManagerPluginBLE()
        {
            Adapter = CrossBluetoothLE.Current.Adapter;
            Adapter.DeviceDiscovered += OnDeviceDiscovered;
            Adapter.DeviceAdvertised += OnDeviceDiscovered;
            Adapter.ScanTimeoutElapsed += Adapter_ScanTimeoutElapsed;
            Adapter.DeviceDisconnected += OnDeviceDisconnected;
            Adapter.DeviceConnectionLost += OnDeviceConnectionLost;
            //Adapter.DeviceConnected += (sender, e) => Adapter.DisconnectDeviceAsync(e.Device);

            Adapter.ScanMode = ScanMode.LowLatency;
        }
        private void OnDeviceConnectionLost(object sender, DeviceErrorEventArgs e)
        {
        }
        private void OnDeviceDisconnected(object sender, DeviceEventArgs e)
        {
        
        }
        private void Adapter_ScanTimeoutElapsed(object sender, EventArgs e)
        {
        
        }
        private void OnDeviceDiscovered(object sender, DeviceEventArgs args)
        {
            System.Console.WriteLine(args.Device.Name);
        }

        public async Task<bool> PairVerisenseDevice(Object Device, IBLEPairingKeyGenerator generator)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> StartScanForDevices()
        {
            Adapter.StartScanningForDevicesAsync();
            return true;
        }

        public void StopScanForDevices()
        {
            Adapter.StopScanningForDevicesAsync();
        }

        public List<VerisenseBLEScannedDevice> GetListOfScannedDevices()
        {
            throw new NotImplementedException();
        }

        public EventHandler<BLEManagerEvent> GetBLEManagerEvent()
        {
            throw new NotImplementedException();
        }
    }
}
