﻿using System;
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

namespace shimmer.Communications
{
    public class RadioPluginBLE : IVerisenseByteCommunication
    {
        public int GallCallBackErrorCount = 0;
        public Guid Asm_uuid { get; set; }
        public event EventHandler<ByteLevelCommunicationEvent> CommunicationEvent;
        public IDevice ConnectedASM { get; set; }
        static IAdapter adapter { get { return CrossBluetoothLE.Current.Adapter; } }
        ICharacteristic UartRX { get; set; }
        ICharacteristic UartTX { get; set; }
        IService ServiceTXRX { get; set; }
        ConnectivityState StateOfConnectivity = ConnectivityState.Unknown;

        public RadioPluginBLE(){
            adapter.DeviceConnected += Adapter_DeviceConnected;
            adapter.DeviceDisconnected += Adapter_DeviceDisconnected;
            adapter.DeviceConnectionLost += Adapter_DeviceConnectionLost;
        }
        void TimeoutConnect(Object obj)
        {
            cancel.Cancel();
        }
        CancellationTokenSource cancel = new CancellationTokenSource();
        public async Task<ConnectivityState> Connect()
        {
            var localTask = new TaskCompletionSource<bool>();

            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var timeout = 5000;
                    cancel = new CancellationTokenSource();
                    TimeSpan timespan = new TimeSpan(0, 0, 5);
                    Timer timer = new Timer(TimeoutConnect, null, 5000, Timeout.Infinite);
                    ConnectedASM = await adapter.ConnectToKnownDeviceAsync(Asm_uuid, new ConnectParameters(false, true),cancel.Token);
                    timer.Dispose();
                    /*if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
                    {
                        // task completed within timeout
                        ConnectedASM = task.Result;
                    }
                    else
                    {
                        // timeout logic
                        cancel.Cancel();
                        cancel.Dispose();
                        await Task.Delay(1000);
                        localTask.TrySetResult(false);
                        return;
                    }*/
                    await Task.Delay(500);

                    if (ConnectedASM.State != DeviceState.Connected)
                    {
                        localTask.TrySetResult(false);
                        return;
                    }

                    ConnectedASM.UpdateConnectionInterval(ConnectionInterval.High);
                    await ConnectedASM.RequestMtuAsync(251);

                    AdvanceLog(nameof(RadioPluginBLE), "Connect ASM Hash", ConnectedASM.GetHashCode(), Asm_uuid.ToString());
                    await Task.Delay(500);
                    System.Console.WriteLine("Getting Service");
                     ServiceTXRX = await ConnectedASM.GetServiceAsync(App.ServiceID);

                    if (ServiceTXRX != null)
                    {
                        UartTX = await ServiceTXRX.GetCharacteristicAsync(App.TxID);
                        System.Console.WriteLine("Getting TX Characteristics Completed");

                        UartRX = await ServiceTXRX.GetCharacteristicAsync(App.RxID);
                        System.Console.WriteLine("Getting RX Characteristics Completed");
                        UartRX.ValueUpdated += UartRX_ValueUpdated;
                        await UartRX.StartUpdatesAsync();

                        AdvanceLog(nameof(RadioPluginBLE), "GetKnownDevice", "Success", Asm_uuid.ToString());
                        //StateChange(ShimmerDeviceBluetoothState.Connected);
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
                    AdvanceLog(nameof(RadioPluginBLE), "ConnectToKnownDeviceAsync Exception", ex.Message, Asm_uuid.ToString());
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

        private ConnectivityState GetConnectivityStateFromDevice(IDevice device)
        {
            if (device.State.Equals(DeviceState.Connected)){
                return ConnectivityState.Connected;
            } else if (device.State.Equals(DeviceState.Connecting))
            {
                return ConnectivityState.Connecting;
            } else if (device.State.Equals(DeviceState.Disconnected))
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
            Debug.WriteLine("RadioPluginBLE: " + e.Device.Name + " Connected");
        }

        private async void Adapter_DeviceDisconnected(object sender, DeviceEventArgs e)
        {
            if (CommunicationEvent != null)
            {
                CommunicationEvent.Invoke(null, new ByteLevelCommunicationEvent { Event = Communications.ByteLevelCommunicationEvent.CommEvent.Disconnected });
                if (ConnectedASM != null)
                {
                    await Disconnect();
                }
            }
        }

        private void Adapter_DeviceConnectionLost(object sender, DeviceEventArgs e)
        {
            if (CommunicationEvent != null)
            {
                CommunicationEvent.Invoke(null, new ByteLevelCommunicationEvent { Event = Communications.ByteLevelCommunicationEvent.CommEvent.Disconnected });
            }
        }

        public async Task<ConnectivityState> Disconnect()
        {
            
            ConnectivityState state = ConnectivityState.Unknown;
            try
            {
                
                if (UartRX != null)
                {
                    UartRX.ValueUpdated -= UartRX_ValueUpdated;
                    UartRX.Service.Dispose();
                    UartRX = null;
                }
                if (UartTX != null)
                {
                    UartTX.Service.Dispose();
                    UartTX = null;
                }
                if (ServiceTXRX != null)
                {
                    ServiceTXRX.Dispose();
                    ServiceTXRX = null;
                }
                
                //ResponseBuffer = null;

                if (ConnectedASM != null)
                {
                    var timeout = 10000; //might need a longer period for windows
                    var task = adapter.DisconnectDeviceAsync(ConnectedASM);

                    AdvanceLog(nameof(RadioPluginBLE), "Disconnect Device", ConnectedASM.GetHashCode(), Asm_uuid.ToString());

                    if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
                    {
                        // task completed within timeout
                        state = ConnectivityState.Disconnected;
                    }
                    else
                    {
                        state = ConnectivityState.Limited;
                    }

                    AdvanceLog(nameof(RadioPluginBLE), "ASM Connection Status", state.ToString(), Asm_uuid.ToString());
                    //state = GetConnectivityStateFromDevice(ConnectedASM);
                }
                //StateChange(ShimmerDeviceBluetoothState.Disconnected);
                AdvanceLog(nameof(RadioPluginBLE), "Disconnect", "Success", Asm_uuid.ToString());
            }
            catch (Exception ex)
            {
                AdvanceLog(nameof(RadioPluginBLE), "DisconnectException", ex.Message, Asm_uuid.ToString());
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

        public async Task<bool> WriteBytes(byte[] bytes)
        {
            var writeTCS = new TaskCompletionSource<bool>();
            AdvanceLog(nameof(RadioPluginBLE), "WriteRequest", BitConverter.ToString(bytes), Asm_uuid.ToString());

            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var res = await UartTX.WriteAsync(bytes);
                    writeTCS.TrySetResult(true);
                }
                catch (Exception ex)
                {
                    AdvanceLog(nameof(RadioPluginBLE), "WriteRequestException", ex.Message, Asm_uuid.ToString());
                    writeTCS.TrySetResult(false);
                }
            });

            var success = await writeTCS.Task;
            return success;
        }

        private void UartRX_ValueUpdated(object sender, Plugin.BLE.Abstractions.EventArgs.CharacteristicUpdatedEventArgs e)
        {
            if (CommunicationEvent != null)
            {
                CommunicationEvent.Invoke(null, new ByteLevelCommunicationEvent { Bytes = e.Characteristic.Value , Event = Communications.ByteLevelCommunicationEvent.CommEvent.NewBytes});
            }
        }

        private bool disposedValue = false;
        public void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    try
                    {
                       
                        if (UartRX != null)
                        {
                            UartRX.ValueUpdated -= UartRX_ValueUpdated;
                            UartRX = null;
                        }

                        UartTX = null;
                        //ResponseBuffer = null;

                        if (ConnectedASM != null)
                        {
                            adapter.DisconnectDeviceAsync(ConnectedASM);
                            AdvanceLog(nameof(RadioPluginBLE), "DisconnectDevice", ConnectedASM.GetHashCode(), Asm_uuid.ToString());
                        }

                        AdvanceLog(nameof(RadioPluginBLE), "Dispose Finished", this.GetHashCode(), Asm_uuid.ToString());
                    }
                    catch (Exception ex)
                    {
                        AdvanceLog(nameof(RadioPluginBLE), "Dispose Exception", ex.Message, Asm_uuid.ToString());
                    }
                    finally
                    {
                        if (ConnectedASM != null)
                        {
                            ConnectedASM.Dispose();
                        }
                        adapter.DeviceDisconnected -= Adapter_DeviceDisconnected;
                        adapter.DeviceConnectionLost -= Adapter_DeviceConnectionLost;

                    }

                }

                disposedValue = true;
            }
        }
        public ConnectivityState GetConnectivityState()
        {
            return StateOfConnectivity;
        }

        public virtual void AdvanceLog(string ObjectName, string Action, object Data, string asmid)
        {
            //Just print to console
            System.Console.WriteLine(ObjectName + " " + Action + " " + Data + " " + asmid);
            Debug.WriteLine(ObjectName + " " + Action + " " + Data + " " + asmid);
        }

    }
}
