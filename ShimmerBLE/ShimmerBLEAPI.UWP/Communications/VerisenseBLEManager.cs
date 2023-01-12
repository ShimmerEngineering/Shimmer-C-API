using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using shimmer.Communications;
using shimmer.Helpers;
using ShimmerBLEAPI.Communications;
using ShimmerBLEAPI.Models;
using ShimmerBLEAPI.UWP.Communications;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using Windows.UI.Core;
using Xamarin.Forms;

[assembly: Dependency(typeof(VerisenseBLEManager))]
namespace ShimmerBLEAPI.UWP.Communications
{
    public class VerisenseBLEManager : IVerisenseBLEManager
    {
        public event EventHandler<BLEManagerEvent> BLEManagerEvent;
        public DeviceWatcher deviceWatcher { get; set; }
        public ObservableCollection<DeviceInformation> resultCollection = new ObservableCollection<DeviceInformation>();
        public CoreDispatcher dispatcher { get; set; }
        public DeviceWatcher DeviceWatcher => deviceWatcher;
        public bool UpdateStatus { get; set; }
        public TaskCompletionSource<bool> RequestTCS { get; set; }
        public List<string> listOfUpdatedConnectableDevicesID { get; set; }
        public DeviceInformation DeviceInformation { get; private set; }
        public static IBLEPairingKeyGenerator PairingKeyGenerator;

        public void StartWatcher(DeviceWatcher deviceWatcher)
        {
            dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
            UpdateStatus = true;
            this.deviceWatcher = deviceWatcher;
            BLEManagerEvent += BLEManager_BLEEvent;
            RequestTCS = new TaskCompletionSource<bool>();
            listOfUpdatedConnectableDevicesID = new List<string>();
            // Connect events to update our collection as the watcher report results.
            deviceWatcher.Added += Watcher_DeviceAdded;
            deviceWatcher.Updated += Watcher_DeviceUpdated;
            deviceWatcher.Removed += Watcher_DeviceRemoved;
            deviceWatcher.EnumerationCompleted += Watcher_EnumerationCompleted;
            deviceWatcher.Stopped += Watcher_Stopped;
            deviceWatcher.Start();
        }

        public void StopWatcher()
        {
            // Since the device watcher runs in the background, it is possible that
            // a notification is "in flight" at the time we stop the watcher.
            // In other words, it is possible for the watcher to become stopped while a
            // handler is running, or for a handler to run after the watcher has stopped.

            if (IsWatcherStarted(deviceWatcher))
            {
                // We do not null out the deviceWatcher yet because we want to receive
                // the Stopped event.
                deviceWatcher.Stop();
            }
        }

        public void Reset()
        {
            if (deviceWatcher != null)
            {
                StopWatcher();
                deviceWatcher = null;
            }
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

        static bool IsWatcherStarted(DeviceWatcher watcher)
        {
            return (watcher.Status == DeviceWatcherStatus.Started) ||
                (watcher.Status == DeviceWatcherStatus.EnumerationCompleted);
        }

        public bool IsWatcherRunning()
        {
            if (deviceWatcher == null)
            {
                return false;
            }

            DeviceWatcherStatus status = deviceWatcher.Status;
            return (status == DeviceWatcherStatus.Started) ||
                (status == DeviceWatcherStatus.EnumerationCompleted) ||
                (status == DeviceWatcherStatus.Stopping);
        }

        private async void Watcher_DeviceAdded(DeviceWatcher sender, DeviceInformation deviceInfo)
        {
            // Since we have the collection databound to a UI element, we need to update the collection on the UI thread.
            await dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
                // Watcher may have stopped while we were waiting for our chance to run.
                if (IsWatcherStarted(sender))
                {
                    resultCollection.Add(deviceInfo);
                }
            });
        }

        private async void Watcher_DeviceUpdated(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
        {
            // Since we have the collection databound to a UI element, we need to update the collection on the UI thread.
            await dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
                // Watcher may have stopped while we were waiting for our chance to run.
                if (IsWatcherStarted(sender))
                {
                    try
                    {
                        if (deviceInfoUpdate.Properties.ContainsKey("System.Devices.Aep.Bluetooth.Le.IsConnectable"))
                        {
                            if ((bool?)deviceInfoUpdate.Properties["System.Devices.Aep.Bluetooth.Le.IsConnectable"] == true)
                            {
                                if (!listOfUpdatedConnectableDevicesID.Contains(deviceInfoUpdate.Id.Split('-')[1]))
                                {
                                    listOfUpdatedConnectableDevicesID.Add(deviceInfoUpdate.Id.Split('-')[1]);
                                }

                                foreach (DeviceInformation deviceInfo in resultCollection)
                                {
                                    if (deviceInfo.Id == deviceInfoUpdate.Id)
                                    {
                                        deviceInfo.Update(deviceInfoUpdate);
                                        var dev = new VerisenseBLEScannedDevice
                                        {
                                            Name = deviceInfo.Name,
                                            ID = deviceInfo.Id.Split('-')[1],
                                            RSSI = (int)deviceInfo.Properties["System.Devices.Aep.SignalStrength"],
                                            Uuid = new Guid("00000000-0000-0000-0000-" + deviceInfo.Id.Split('-')[1].Replace(":", "")),
                                            IsPaired = deviceInfo.Pairing.IsPaired,
                                            IsConnectable = listOfUpdatedConnectableDevicesID.Contains(deviceInfo.Id.Split('-')[1]),
                                            IsConnected = (bool?)deviceInfo.Properties["System.Devices.Aep.IsConnected"] == true,
                                            DeviceInfo = deviceInfo
                                        };

                                if (BLEManagerEvent != null)
                                            BLEManagerEvent.Invoke(null, new BLEManagerEvent { CurrentEvent = shimmer.Communications.BLEManagerEvent.BLEAdapterEvent.DeviceDiscovered, objMsg = dev });

                                    }
                                }

                                
                            }
                        } 

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }

                    // Find the corresponding updated DeviceInformation in the collection and pass the update object
                    // to the Update method of the existing DeviceInformation. This automatically updates the object
                    // for us.
                    foreach (DeviceInformation deviceInfo in resultCollection)
                    {
                        if (deviceInfo.Id == deviceInfoUpdate.Id)
                        {
                            deviceInfo.Update(deviceInfoUpdate);
                            break;
                        }
                    }
                }
            });
        }

        private async void Watcher_DeviceRemoved(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
        {
            // Since we have the collection databound to a UI element, we need to update the collection on the UI thread.
            await dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
                // Watcher may have stopped while we were waiting for our chance to run.
                if (IsWatcherStarted(sender))
                {
                    // Find the corresponding DeviceInformation in the collection and remove it
                    foreach (DeviceInformation deviceInfo in resultCollection)
                    {
                        if (deviceInfo.Id == deviceInfoUpdate.Id)
                        {
                            resultCollection.Remove(deviceInfo);
                            break;
                        }

                        if (listOfUpdatedConnectableDevicesID.Count > 0 && listOfUpdatedConnectableDevicesID.Contains(deviceInfoUpdate.Id.Split('-')[1]))
                        {
                            listOfUpdatedConnectableDevicesID.Remove(deviceInfoUpdate.Id.Split('-')[1]);                          
                        }
                    }
                }
            });
        }

        private async void Watcher_EnumerationCompleted(DeviceWatcher sender, object obj)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
                if (BLEManagerEvent != null)
                    BLEManagerEvent.Invoke(null, new BLEManagerEvent { CurrentEvent = shimmer.Communications.BLEManagerEvent.BLEAdapterEvent.ScanCompleted });

                RequestTCS.TrySetResult(true);
            });
        }

        private async void Watcher_Stopped(DeviceWatcher sender, object obj)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
                Console.WriteLine("Watcher stopped.");
            });
        }
     
        public List<VerisenseBLEScannedDevice> GetListOfScannedDevices()
        {
            List<VerisenseBLEScannedDevice> listOfScannedDevices = new List<VerisenseBLEScannedDevice>();
            foreach (var item in resultCollection)
            {
                listOfScannedDevices.Add(new VerisenseBLEScannedDevice
                {
                    Name = item.Name,
                    ID = item.Id.Split('-')[1],
                    RSSI = (int)item.Properties["System.Devices.Aep.SignalStrength"],
                    Uuid = new Guid("00000000-0000-0000-0000-" + item.Id.Split('-')[1].Replace(":","")),
                    IsPaired = item.Pairing.IsPaired,
                    IsConnectable = listOfUpdatedConnectableDevicesID.Contains(item.Id.Split('-')[1]),
                    IsConnected = (bool?)item.Properties["System.Devices.Aep.IsConnected"] == true,
                    DeviceInfo = item
                }); ;
            }
            return listOfScannedDevices;
        }

        public async Task<bool> PairVerisenseDevice(Object Device, IBLEPairingKeyGenerator generator)
        {
            PairingKeyGenerator = generator;
            if (((VerisenseBLEScannedDevice)Device).IsPaired)
            {
                if (BLEManagerEvent != null)
                    BLEManagerEvent.Invoke(null, new BLEManagerEvent { CurrentEvent = shimmer.Communications.BLEManagerEvent.BLEAdapterEvent.DevicePaired, objMsg = (VerisenseBLEScannedDevice)Device, message = "Device Is Already Paired" });
                return true;
            }
            Console.WriteLine("Pairing started. Please wait...");
            DeviceInformationCustomPairing customPairing = ((DeviceInformation)((VerisenseBLEScannedDevice)Device).DeviceInfo).Pairing.Custom;
            customPairing.PairingRequested += PairingRequestedHandler;
            DevicePairingResult result = await customPairing.PairAsync(DevicePairingKinds.ProvidePin, DevicePairingProtectionLevel.Default);
            customPairing.PairingRequested -= PairingRequestedHandler;
            Console.WriteLine("Pairing result = " + result.Status.ToString());

            if (result.Status.Equals(DevicePairingResultStatus.Paired))
            {
                if (BLEManagerEvent != null)
                    BLEManagerEvent.Invoke(null, new BLEManagerEvent { CurrentEvent = shimmer.Communications.BLEManagerEvent.BLEAdapterEvent.DevicePaired, objMsg = (VerisenseBLEScannedDevice)Device, message = "Device Is Successfully Paired" });
                return true;
            }
            else
            {
                return false;
            }
        }

        private async void PairingRequestedHandler( DeviceInformationCustomPairing sender,DevicePairingRequestedEventArgs args)
        {
            string pin = PairingKeyGenerator.CalculatePairingPin(args.DeviceInformation.Name);
            if (!string.IsNullOrEmpty(pin))
            {
                args.Accept(pin); 
            }
        }
        public async Task<bool> StartScanForDevices()
        {
            resultCollection.Clear();
            DeviceWatcher deviceWatcher;
            string selector = "(System.Devices.Aep.ProtocolId:=\"{bb7bb05e-5972-42b5-94fc-76eaa7084d49}\")"; //protocol id for BLE devices

            string[] RequestedProperties = { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected", "System.Devices.Aep.Bluetooth.Le.IsConnectable", "System.Devices.Aep.SignalStrength" };

            deviceWatcher = DeviceInformation.CreateWatcher(
                    selector,
                    RequestedProperties, 
                    DeviceInformationKind.AssociationEndpoint);

            StartWatcher(deviceWatcher);
            return await RequestTCS.Task;
        }

        public void StopScanForDevices()
        {
            if (IsWatcherStarted(deviceWatcher))
            {
                // We do not null out the deviceWatcher yet because we want to receive
                // the Stopped event.
                deviceWatcher.Stop();
            }
        }

        public EventHandler<BLEManagerEvent> GetBLEManagerEvent()
        {
            return BLEManagerEvent;
        }

        public async Task<List<string>> GetSystemConnectedOrPairedDevices()
        {
            List<string> list = new List<string>();
            DeviceInformationCollection PairedBluetoothDevices = await DeviceInformation.FindAllAsync(BluetoothDevice.GetDeviceSelectorFromPairingState(true));
            foreach (var item in PairedBluetoothDevices)
            {
                if (item.Name.Contains("Verisense"))
                {
                    list.Add(item.Name);
                }
            }
            return list;
        }
    }
}
