using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using shimmer.Communications;
using ShimmerBLEAPI.Communications;
using ShimmerBLEAPI.Devices;
using ShimmerBLEAPI.iOS.Communications;
using ShimmerBLEAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(VerisenseBLEManager))]
namespace ShimmerBLEAPI.iOS.Communications
{
    public class VerisenseBLEManager : IVerisenseBLEManager
    {
        public event EventHandler<BLEManagerEvent> BLEManagerEvent;
        static List<VerisenseBLEScannedDevice> ListOfScannedKnownDevices { get; set; }
        static List<IDevice> deviceList { get; set; }
        public static TaskCompletionSource<bool> RequestTCS { get; set; }
        static IAdapter Adapter { get { return CrossBluetoothLE.Current.Adapter; } }

        public EventHandler<BLEManagerEvent> GetBLEManagerEvent()
        {
            return BLEManagerEvent;
        }
        private void BLEManager_BLEEvent(object sender, BLEManagerEvent e)
        {
            if (e.CurrentEvent == shimmer.Communications.BLEManagerEvent.BLEAdapterEvent.ScanCompleted)
            {
                Console.WriteLine("Scan is completed.");
            }
            else if (e.CurrentEvent == shimmer.Communications.BLEManagerEvent.BLEAdapterEvent.DevicePaired)
            {
                Console.WriteLine(((VerisenseBLEDeviceIOS)e.objMsg).Asm_uuid.ToString() + " is paired.");
            }
        }
        public List<VerisenseBLEScannedDevice> GetListOfScannedDevices()
        {
            return ListOfScannedKnownDevices;
        }

        public async Task<bool> StartScanForDevices()
        {
            try
            {
                BLEManagerEvent += BLEManager_BLEEvent;

                //RequestTCS = new TaskCompletionSource<bool>();
                ListOfScannedKnownDevices = new List<VerisenseBLEScannedDevice>();
                Adapter.ScanMode = ScanMode.LowLatency;
                Adapter.DeviceAdvertised += Adapter_DeviceAdvertised;

                //Adapter.DeviceDiscovered += (s, a) => deviceList.Add(a.Device); 
                await Adapter.StartScanningForDevicesAsync();
                Adapter.DeviceAdvertised -= Adapter_DeviceAdvertised;

                if (BLEManagerEvent != null)
                    BLEManagerEvent.Invoke(null, new BLEManagerEvent { CurrentEvent = shimmer.Communications.BLEManagerEvent.BLEAdapterEvent.ScanCompleted });
                //return RequestTCS.TrySetResult(true); //commented out as currently this does nothing
                return true;
            } catch (Exception e)
            {
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
                    //IsPaired = GetBondingStatus(uuid), //unable to get bonding status in ios
                    IsPaired = false,
                    DeviceInfo = e.Device
                }); ;
            }

        }

        public static bool GetBondingStatus(string id)
        {
            var devices = Adapter.GetSystemConnectedOrPairedDevices();

            var isBonded = devices.FirstOrDefault(x => x.Id.Equals(Guid.Parse(id))) != null;

            return isBonded;
        }
        public async Task<bool> PairVerisenseDevice(object Device, IBLEPairingKeyGenerator generator)
        {
            var pairingResult = await ((VerisenseBLEDeviceIOS)Device).PairDeviceAsync();
            if (pairingResult)
            {
                if (BLEManagerEvent != null)
                    BLEManagerEvent.Invoke(null, new BLEManagerEvent { CurrentEvent = shimmer.Communications.BLEManagerEvent.BLEAdapterEvent.DevicePaired, objMsg = (VerisenseBLEDeviceIOS)Device, message = "Device Is Successfully Paired" });
            }
            return pairingResult;
        }

        public void StopScanForDevices()
        {
            throw new NotImplementedException();
        }

    }
}
