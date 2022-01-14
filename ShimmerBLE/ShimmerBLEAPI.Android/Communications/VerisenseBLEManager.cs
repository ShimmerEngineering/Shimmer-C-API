using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Bluetooth;
using Android.Content;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BluetoothLE;
using shimmer.Communications;
using shimmer.Helpers;
using shimmer.Models;
using ShimmerBLEAPI.Android.Communications;
using ShimmerBLEAPI.Communications;
using ShimmerBLEAPI.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(VerisenseBLEManager))]
namespace ShimmerBLEAPI.Android.Communications
{
    public class VerisenseBLEManager : IVerisenseBLEManager
    {
        public event EventHandler<BLEManagerEvent> BLEManagerEvent;
        static Plugin.BLE.Abstractions.Contracts.IAdapter adapter { get { return CrossBluetoothLE.Current.Adapter; } }

        static List<VerisenseBLEScannedDevice> ListOfScannedKnownDevices { get; set; }
        public static TaskCompletionSource<bool> RequestTCS { get; set; }
        public static IBLEPairingKeyGenerator PairingKeyGenerator;

        public List<VerisenseBLEScannedDevice> GetListOfScannedDevices()
        {
            return ListOfScannedKnownDevices;
        }

        public async Task<bool> PairVerisenseDevice(object Device, IBLEPairingKeyGenerator generator)
        {
            PairingKeyGenerator = generator;
            RequestTCS = new TaskCompletionSource<bool>();

            if (GetBondingStatus(((VerisenseBLEScannedDevice)Device).ID))
            {
                if (BLEManagerEvent != null)
                    BLEManagerEvent.Invoke(null, new BLEManagerEvent { CurrentEvent = shimmer.Communications.BLEManagerEvent.BLEAdapterEvent.DevicePaired, objMsg = (VerisenseBLEScannedDevice)Device, message = "Device Is Already Paired" });
                return true;
            }
            try
            {
                BondASMAutomatically((VerisenseBLEScannedDevice)Device);
                return await RequestTCS.Task;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }
        public static bool GetBondingStatus(string id)
        {
            var devices = adapter.GetSystemConnectedOrPairedDevices();

            var isBonded = devices.FirstOrDefault(x => x.Id.Equals(Guid.Parse(id))) != null;

            return isBonded;
        }

        public async Task<bool> StartScanForDevices()
        {
            try
            {
                BLEManagerEvent += BLEManager_BLEEvent;

                //RequestTCS = new TaskCompletionSource<bool>();
                ListOfScannedKnownDevices = new List<VerisenseBLEScannedDevice>();

                adapter.ScanMode = Plugin.BLE.Abstractions.Contracts.ScanMode.LowLatency;
                adapter.DeviceAdvertised += Adapter_DeviceAdvertised;

                await adapter.StartScanningForDevicesAsync();
                adapter.DeviceAdvertised -= Adapter_DeviceAdvertised;
                if (BLEManagerEvent != null)
                    BLEManagerEvent.Invoke(null, new BLEManagerEvent { CurrentEvent = shimmer.Communications.BLEManagerEvent.BLEAdapterEvent.ScanCompleted });
                //return RequestTCS.TrySetResult(true); //commented out as currently this does nothing
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        private static void Adapter_DeviceAdvertised(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            string uuid = e.Device.Id.ToString();
            var entry = ListOfScannedKnownDevices.FirstOrDefault(x => x.ID.Equals(uuid));

            if (entry != null)
            {
                return;
            }
           
            if (e.Device.Name != null)
            {
                ListOfScannedKnownDevices.Add(new VerisenseBLEScannedDevice
                {
                    Name = e.Device.Name,
                    ID = uuid,
                    RSSI = e.Device.Rssi,
                    Uuid = e.Device.Id,
                    IsConnectable = e.Device.Name.Contains("Verisense") ? true : false,
                    IsPaired = GetBondingStatus(uuid),
                    DeviceInfo = e.Device
                }); ;
            }
           
        }

        public Task<bool> StopScanForDevices()
        {
            throw new NotImplementedException();
        }

        List<VerisenseBLEScannedDevice> IVerisenseBLEManager.GetListOfScannedDevices()
        {
            return ListOfScannedKnownDevices;
        }

        void IVerisenseBLEManager.StopScanForDevices()
        {
            throw new NotImplementedException();
        }

        private void BLEManager_BLEEvent(object sender, BLEManagerEvent e)
        {
            if (e.CurrentEvent == shimmer.Communications.BLEManagerEvent.BLEAdapterEvent.ScanCompleted)
            {
                Console.WriteLine("Scan is completed.");
            }
            else if (e.CurrentEvent == shimmer.Communications.BLEManagerEvent.BLEAdapterEvent.DevicePaired)
            {
                Console.WriteLine(((VerisenseBLEScannedDevice)e.objMsg).Uuid.ToString() + " is paired.");
            }
        }

        public BondingStatus BondASMAutomatically(VerisenseBLEScannedDevice device)
        {
            try
            {
                BluetoothDevice bluetoothDevice;

                if (((Plugin.BLE.Abstractions.Contracts.IDevice)device.DeviceInfo).NativeDevice is BluetoothDevice)
                {
                    bluetoothDevice = (BluetoothDevice)((Plugin.BLE.Abstractions.Contracts.IDevice)device.DeviceInfo).NativeDevice;
                    bluetoothDevice.CreateBond();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                //invoke fail
                return BondingStatus.Cancel;
            }

            return BondingStatus.Bonding;
        }



        public static bool IsCorrectBluetoothDevice(VerisenseBLEScannedDevice deviceItem, BluetoothDevice bluetoothDevice)
        {
            if (deviceItem == null || bluetoothDevice == null)
            {
                return false;
            }

            if (deviceItem.Name == null || bluetoothDevice.Name == null)
            {
                return false;
            }

            if (!bluetoothDevice.Name.Contains(deviceItem.Name))
            {
                return false;
            }

            return true;
        }

        public EventHandler<BLEManagerEvent> GetBLEManagerEvent()
        {
            return BLEManagerEvent;
        }

    }

    public class BroadcastReceiverPairingRequest : BroadcastReceiver
    {
        VerisenseBLEScannedDevice deviceItem;
        public event EventHandler<BLEManagerEvent> BLEManagerEvent;

        public void SetScannedDeviceItem(VerisenseBLEScannedDevice deviceItem, EventHandler<BLEManagerEvent> BLEManagerEvent)
        {
            this.deviceItem = deviceItem;
            this.BLEManagerEvent = BLEManagerEvent;
        }

        public override void OnReceive(Context context, Intent intent)
        {
            String action = intent.Action;
            if (action.Equals(BluetoothDevice.ActionPairingRequest))
            {
                try
                {
                    BluetoothDevice bluetoothDevice = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);

                    //Ensure that the device which was selected in the list is the correct one that was returned
                    if (VerisenseBLEManager.IsCorrectBluetoothDevice(deviceItem, bluetoothDevice))
                    {
                        if (VerisenseBLEManager.PairingKeyGenerator == null)
                        {
                            throw new Exception("Pairing Key Generator Not Created");
                        }
                        string pin = VerisenseBLEManager.PairingKeyGenerator.CalculatePairingPin(deviceItem.Name);
                        byte[] pinBytes = System.Text.Encoding.ASCII.GetBytes(pin);
                        bool setPin = bluetoothDevice.SetPin(pinBytes);
                        if (setPin)
                        {
                            InvokeAbortBroadcast();    //remove Bluetooth pairing pin dialog
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    VerisenseBLEManager.RequestTCS.TrySetResult(false);
                }
            }
        }
    }

    public class BroadcastReceiverBondStateChanged : BroadcastReceiver
    {
        VerisenseBLEScannedDevice deviceItem;
        public event EventHandler<BLEManagerEvent> BLEManagerEvent;

        public void SetScannedDeviceItem(VerisenseBLEScannedDevice deviceItem, EventHandler<BLEManagerEvent> BLEManagerEvent)
        {
            this.deviceItem = deviceItem;
            this.BLEManagerEvent = BLEManagerEvent;
        }
        public override void OnReceive(Context context, Intent intent)
        {
            String action = intent.Action;
            if (action.Equals(BluetoothDevice.ActionBondStateChanged))
            {
                BluetoothDevice bluetoothDevice = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);

                //Ensure that the device which was selected in the list is the correct one that was returned
                if (VerisenseBLEManager.IsCorrectBluetoothDevice(deviceItem, bluetoothDevice))
                {
                    if (bluetoothDevice.BondState == Bond.Bonded)
                    {
                        if (BLEManagerEvent != null)
                            BLEManagerEvent.Invoke(null, new BLEManagerEvent { CurrentEvent = shimmer.Communications.BLEManagerEvent.BLEAdapterEvent.DevicePaired, objMsg = deviceItem, message = "Device Is Successfully Paired" });
                        VerisenseBLEManager.RequestTCS.TrySetResult(true);
                    }
                }
            }
        }
    }
}
