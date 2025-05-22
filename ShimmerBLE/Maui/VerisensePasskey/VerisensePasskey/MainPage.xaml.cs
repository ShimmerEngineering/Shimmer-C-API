using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System.Collections.ObjectModel;

namespace VerisensePasskey
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private readonly IAdapter _adapter;
        private readonly IBluetoothLE _bluetoothLE;
        private ObservableCollection<IDevice> _deviceList;
        public MainPage()
        {
            InitializeComponent();
            _bluetoothLE = CrossBluetoothLE.Current;
            _adapter = CrossBluetoothLE.Current.Adapter;

            _deviceList = new ObservableCollection<IDevice>();
            DevicesList.ItemsSource = _deviceList;

            _adapter.DeviceDiscovered += OnDeviceDiscovered;
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private async void OnScanClicked(object sender, EventArgs e)
        {
            _deviceList.Clear();

            if (!_bluetoothLE.IsOn)
            {
                await DisplayAlert("Bluetooth Off", "Please turn on Bluetooth", "OK");
                return;
            }

            if (_adapter.IsScanning)
            {
                await _adapter.StopScanningForDevicesAsync();
            }

            try
            {
                await _adapter.StartScanningForDevicesAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Scan Error", ex.Message, "OK");
            }
        }
        private void OnConnectClicked(object sender, EventArgs e)
        {
            
        }

        private void OnDeviceDiscovered(object sender, DeviceEventArgs args)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (!_deviceList.Any(d => d.Id == args.Device.Id))
                {
                    _deviceList.Add(args.Device);
                }
            });
        }
    }

}
