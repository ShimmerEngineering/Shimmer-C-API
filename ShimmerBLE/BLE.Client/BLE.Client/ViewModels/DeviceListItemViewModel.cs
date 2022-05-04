using System;
using MvvmCross.ViewModels;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using ShimmerBLEAPI.Models;

namespace BLE.Client.ViewModels
{
    public class DeviceListItemViewModel : MvxNotifyPropertyChanged
    {
        public VerisenseBLEScannedDevice Device { get; private set; }

        public Guid Id => Device.Uuid;
        public bool IsConnected => Device.IsConnected;
        public int Rssi => Device.RSSI;
        public string Name => Device.Name;

        public DeviceListItemViewModel(VerisenseBLEScannedDevice device)
        {
            Device = device;
        }

        public void Update(VerisenseBLEScannedDevice newDevice = null)
        {
            if (newDevice != null)
            {
                Device = newDevice;
            }
            RaisePropertyChanged(nameof(IsConnected));
            RaisePropertyChanged(nameof(Rssi));
        }
    }
}
