using System;
using System.Collections.Generic;
using System.Text;
using shimmer.Communications;
using ShimmerBLEAPI.Communications;
using Xamarin.Forms;
using ShimmerBLEAPI.UWP.Communications;
using System.Collections.ObjectModel;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using ShimmerBLEAPI.Models;
using System.Threading.Tasks;
using Windows.UI.Core;

[assembly: Dependency(typeof(VerisenseSerialPortManager))]
namespace ShimmerBLEAPI.UWP.Communications
{
    public class VerisenseSerialPortManager : IVerisenseSerialPortManager
    {
        public ObservableCollection<DeviceInformation> resultCollection = new ObservableCollection<DeviceInformation>();
        public DeviceWatcher deviceWatcher { get; set; }
        public CoreDispatcher dispatcher { get; set; }
        public TaskCompletionSource<bool> RequestTCS { get; set; }

        public async Task<bool> StartScanForSerialPorts()
        {
            resultCollection.Clear();
            var deviceSelector = SerialDevice.GetDeviceSelector();
            deviceWatcher = DeviceInformation.CreateWatcher(deviceSelector);
            StartWatcher(deviceWatcher);
            return await RequestTCS.Task;
        }

        public void StopScanForSerialPorts()
        {
            if (IsWatcherStarted(deviceWatcher))
            {
                // We do not null out the deviceWatcher yet because we want to receive
                // the Stopped event.
                deviceWatcher.Stop();
            }
        }

        public void StartWatcher(DeviceWatcher deviceWatcher)
        {
            dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
            this.deviceWatcher = deviceWatcher;
            RequestTCS = new TaskCompletionSource<bool>();

            deviceWatcher.Added += Watcher_DeviceAdded;
            deviceWatcher.Removed += Watcher_DeviceRemoved;
            deviceWatcher.Updated += Watcher_DeviceUpdated;
            deviceWatcher.EnumerationCompleted += Watcher_EnumerationCompleted;
            deviceWatcher.Stopped += Watcher_Stopped;

            if (!IsWatcherStarted(deviceWatcher))
            {
                deviceWatcher.Start();
            }
        }

        private async void Watcher_DeviceAdded(DeviceWatcher sender, DeviceInformation deviceInformation)
        {
            // Since we have the collection databound to a UI element, we need to update the collection on the UI thread.
            await dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
                // Watcher may have stopped while we were waiting for our chance to run.
                if (IsWatcherStarted(sender))
                {
                    resultCollection.Add(deviceInformation);
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
                    }
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

                }
            });
        }

        private async void Watcher_EnumerationCompleted(DeviceWatcher sender, Object args)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
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

        static bool IsWatcherStarted(DeviceWatcher watcher)
        {
            return (watcher.Status == DeviceWatcherStatus.Started) ||
                (watcher.Status == DeviceWatcherStatus.EnumerationCompleted);
        }

        public List<VerisenseSerialDevice> GetListOfSerialDevices()
        {
            List<VerisenseSerialDevice> listOfSerialDevices = new List<VerisenseSerialDevice>();
            foreach (var item in resultCollection)
            {
                listOfSerialDevices.Add(new VerisenseSerialDevice(item.Id));
            }
            return listOfSerialDevices;
        }
    }
}
