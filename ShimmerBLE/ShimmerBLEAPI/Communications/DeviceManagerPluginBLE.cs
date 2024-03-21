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
    /// <summary>
    /// Contains methods related to communicating with the BLE devices
    /// </summary>
    public class DeviceManagerPluginBLE : IVerisenseBLEManager
    {
        public event EventHandler<BLEManagerEvent> BLEManagerEvent;
        protected readonly IAdapter Adapter;
        /// <summary>
        /// Create a DeviceManagerPluginBLE instance
        /// </summary>
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

        /// <summary>
        /// Pair with a verisense device
        /// </summary>
        /// <param name="Device">device to be paired</param>
        /// <param name="generator">use to generate the passkey</param>
        public async Task<bool> PairVerisenseDevice(Object Device, IBLEPairingKeyGenerator generator)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Start scanning for BLE devices
        /// </summary>
        public async Task<bool> StartScanForDevices()
        {
            Adapter.StartScanningForDevicesAsync();
            return true;
        }

        /// <summary>
        /// Stop scanning for BLE devices
        /// </summary>
        public void StopScanForDevices()
        {
            Adapter.StopScanningForDevicesAsync();
        }

        /// <summary>
        /// Return the list of scanned BLE devices
        /// </summary>
        /// <returns>list of <see cref="VerisenseBLEScannedDevice"/></returns>
        public List<VerisenseBLEScannedDevice> GetListOfScannedDevices()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the event handler
        /// </summary>
        public EventHandler<BLEManagerEvent> GetBLEManagerEvent()
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetSystemConnectedOrPairedDevices()
        {
            throw new NotImplementedException();
        }
    }
}
