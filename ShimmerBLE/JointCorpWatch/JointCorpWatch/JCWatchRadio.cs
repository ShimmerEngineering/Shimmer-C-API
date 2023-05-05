using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Xamarin.Forms;
using shimmer.Communications;

namespace JointCorpWatch
{
    public class JCWatchRadio : IVerisenseByteCommunication
    {
        public event EventHandler<ByteLevelCommunicationEvent> CommunicationEvent;
        public int GallCallBackErrorCount = 0;
        public Guid Asm_uuid { get; set; }
        public IDevice ConnectedASM { get; set; }
        static IAdapter adapter { get { return CrossBluetoothLE.Current.Adapter; } }
        ICharacteristic characteristic { get; set; }
        ICharacteristic characteristicrx { get; set; }
        IService Service { get; set; }
        IDescriptor descriptor { get; set; }
        public bool IsAutoConnecting { get; set; } = false;
        CancellationTokenSource cancel = new CancellationTokenSource();
        public static Guid NOTIFY = Guid.Parse("00002902-0000-1000-8000-00805f9b34fb");
        public static Guid SERVICE_DATA = Guid.Parse("0000fff0-0000-1000-8000-00805f9b34fb");
        public static Guid DATA_Characteristic = Guid.Parse("0000fff6-0000-1000-8000-00805f9b34fb");
        public static Guid NOTIFY_Characteristic = Guid.Parse("0000fff7-0000-1000-8000-00805f9b34fb");
        ConnectivityState StateOfConnectivity = ConnectivityState.Unknown;

        public JCWatchRadio(String id)
        {
            Asm_uuid = Guid.Parse(id);
            adapter.DeviceConnected += Adapter_DeviceConnected;
            adapter.DeviceDisconnected += Adapter_DeviceDisconnected;
            adapter.DeviceConnectionLost += Adapter_DeviceConnectionLost;
        }

        public async Task<ConnectivityState> Connect()
        {
            return await Connect(true, false);
        }

        public async Task<ConnectivityState> Connect(bool autoConnect)
        {
            return await Connect(autoConnect, false);
        }

        public async Task<ConnectivityState> Connect(bool autoConnect, bool timeOut)
        {
            var localTask = new TaskCompletionSource<bool>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    cancel = new CancellationTokenSource();
                    //await adapter.StartScanningForDevicesAsync();
                    //adapter.DeviceDiscovered += DeviceDiscovered;
                    //var result = adapter.DiscoveredDevices;
                    Timer timer = null;
                    if (timeOut)
                    {
                        timer = new Timer(ConnectionTimeOut, null, 600000, Timeout.Infinite); // 10 minutes
                    }
                    ConnectedASM = await adapter.ConnectToKnownDeviceAsync(Asm_uuid, new ConnectParameters(autoConnect, true), cancel.Token);
                    if(timer != null) timer.Dispose();
                    if (ConnectedASM != null)
                    {
                        ConnectedASM.UpdateConnectionInterval(ConnectionInterval.High);
                        await ConnectedASM.RequestMtuAsync(251);
                        Service = await ConnectedASM.GetServiceAsync(SERVICE_DATA);
                        characteristic = await Service.GetCharacteristicAsync(DATA_Characteristic);
                        characteristicrx = await Service.GetCharacteristicAsync(NOTIFY_Characteristic);
                        characteristicrx.ValueUpdated += UartRX_ValueUpdated;
                        await characteristicrx.StartUpdatesAsync();
                        descriptor = await characteristicrx.GetDescriptorAsync(NOTIFY);
                        //descriptor.Characteristic.ValueUpdated += Characteristic_ValueUpdated;
                        byte[] enablebytes = { 1, 0 };
                        await descriptor.WriteAsync(enablebytes);
                        //byte[] bytes = { 0x99, 0x04, 0x00, 0x5a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xf7 }; //ecg,ppg
                        var res = await characteristic.WriteAsync(JCWatch.getMacAddress);
                        Debug.WriteLine(characteristic);
                        localTask.TrySetResult(true);
                    }
                    else
                    {
                        localTask.TrySetResult(false);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Radio Plugin BLE Exception " + ex.Message);
                    AdvanceLog(nameof(JCWatch), "ConnectToKnownDeviceAsync Exception", ex.Message, Asm_uuid.ToString());
                    //GattCallback error: Failure
                    if (ex.Message.Contains("GattCallback error: Failure")) //might want to have a look at this error as well in the future GattCallback error: 133 
                    {
                        GallCallBackErrorCount++;
                    }
                    foreach (IDevice device in adapter.ConnectedDevices)
                    {
                        if (device.Id.Equals(Asm_uuid))
                        {
                            device.Dispose();
                        }
                    }
                    localTask.TrySetResult(false);
                }
            });
            var result = await localTask.Task;
            if (ConnectedASM == null)
            {
                return ConnectivityState.Disconnected;
            }
            return GetConnectivityStateFromDevice(ConnectedASM);
        }

        void ConnectionTimeOut(Object obj)
        {
            CancelConnect();
            if (CommunicationEvent != null)
            {
                CommunicationEvent.Invoke(null, new ByteLevelCommunicationEvent { Event = ByteLevelCommunicationEvent.CommEvent.Disconnected });
            }
        }

        public void CancelConnect()
        {
            if (cancel != null)
            {
                cancel.Cancel();
            }
        }
        
        private ConnectivityState GetConnectivityStateFromDevice(IDevice device)
        {
            if (device.State.Equals(DeviceState.Connected))
            {
                return ConnectivityState.Connected;
            }
            else if (device.State.Equals(DeviceState.Connecting))
            {
                return ConnectivityState.Connecting;
            }
            else if (device.State.Equals(DeviceState.Disconnected))
            {
                return ConnectivityState.Disconnected;
            }
            if (device.State.Equals(DeviceState.Limited))
            {
                return ConnectivityState.Limited;
            }
            return ConnectivityState.Unknown;
        }
        private void Adapter_DeviceConnected(object sender, DeviceEventArgs e)
        {
            Debug.WriteLine("JCWatch: " + e.Device.Name + " Connected");
        }

        private async void Adapter_DeviceDisconnected(object sender, DeviceEventArgs e)
        {
            if (ConnectedASM != null)
            {
                if (CommunicationEvent != null && e.Device.Id == ConnectedASM.Id)
                {
                    CommunicationEvent.Invoke(null, new ByteLevelCommunicationEvent { Event = ByteLevelCommunicationEvent.CommEvent.Disconnected });
                    if (ConnectedASM != null)
                    {
                        await Disconnect();
                    }
                }
            }
        }

        private void Adapter_DeviceConnectionLost(object sender, DeviceEventArgs e)
        {
            if (ConnectedASM != null)
            {
                if (CommunicationEvent != null && e.Device.Id == ConnectedASM.Id)
                {
                    CommunicationEvent.Invoke(null, new ByteLevelCommunicationEvent { Event = ByteLevelCommunicationEvent.CommEvent.Disconnected });
                }
            }
        }

        public async Task<ConnectivityState> Disconnect()
        {
            ConnectivityState state = ConnectivityState.Unknown;
            try
            {
                if (characteristicrx != null)
                {
                    characteristicrx.ValueUpdated -= UartRX_ValueUpdated;
                    characteristicrx.Service.Dispose();
                    characteristicrx = null;
                }
                if (characteristic != null)
                {
                    characteristic.Service.Dispose();
                    characteristic = null;
                }
                if (Service != null)
                {
                    Service.Dispose();
                    Service = null;
                }

                //ResponseBuffer = null;

                if (ConnectedASM != null)
                {
                    var timeout = 10000; //might need a longer period for windows
                    var task = adapter.DisconnectDeviceAsync(ConnectedASM);

                    if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
                    {
                        // task completed within timeout
                        state = ConnectivityState.Disconnected;
                    }
                    else
                    {
                        state = ConnectivityState.Limited;
                    }
                }
            }
            catch (Exception ex)
            {
                AdvanceLog(nameof(JCWatch), "DisconnectException", ex.Message, Asm_uuid.ToString());
            }
            finally
            {
                if (ConnectedASM != null)
                {
                    ConnectedASM.Dispose();
                    ConnectedASM = null;
                }
            }
            return state;
        }

        public async Task<bool> WriteBytes(byte[] value)
        {
            await characteristic.WriteAsync(value);
            return true;
        }

        /*
        private void Characteristic_ValueUpdated(object sender, CharacteristicUpdatedEventArgs e)
        {
            Debug.WriteLine("Data Received From Char: " + BitConverter.ToString(e.Characteristic.Value).Replace("-", ""));
        }
        */

        private void UartRX_ValueUpdated(object sender, CharacteristicUpdatedEventArgs e)
        {
            Debug.WriteLine("Data Received: " + BitConverter.ToString(e.Characteristic.Value).Replace("-", ""));
            if (CommunicationEvent != null)
            {
                CommunicationEvent.Invoke(null, new ByteLevelCommunicationEvent { Bytes = e.Characteristic.Value, Event = ByteLevelCommunicationEvent.CommEvent.NewBytes });
            }
        }

        private void DeviceDiscovered(object sender, DeviceEventArgs e)
        {
            Debug.WriteLine("");
        }

        public virtual void AdvanceLog(string ObjectName, string Action, object Data, string asmid)
        {
            //Just print to console
            System.Console.WriteLine(ObjectName + " " + Action + " " + Data + " " + asmid);
            Debug.WriteLine(ObjectName + " " + Action + " " + Data + " " + asmid);
        }

        public ConnectivityState GetConnectivityState()
        {
            return StateOfConnectivity;
        }
    }
}
