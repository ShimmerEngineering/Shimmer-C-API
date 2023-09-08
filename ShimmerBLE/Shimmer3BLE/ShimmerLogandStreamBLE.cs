using shimmer.Communications;
using ShimmerAPI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System.Diagnostics;

namespace Shimmer3BLE
{
    public class ShimmerLogAndStreamBLE : ShimmerLogAndStream
    {
        protected IVerisenseByteCommunication BLERadio;
        BlockingCollection<int> Buffer = new BlockingCollection<int>(2048);
        public Guid Asm_uuid { get; set; }
        public ShimmerLogAndStreamBLE(string devID) : base(devID)
        {
            Asm_uuid = Guid.Parse(devID);
        }

        public override string GetShimmerAddress()
        {
            //throw new NotImplementedException();
            return Asm_uuid.ToString();
        }

        public override void SetShimmerAddress(string address)
        {
            //throw new NotImplementedException();
        }

        protected override void CloseConnection()
        {
            //throw new NotImplementedException();
            if (ConnectedASM != null)
            {
                var timeout = 10000; //might need a longer period for windows
                var task = adapter.DisconnectDeviceAsync(ConnectedASM);
            }
        }
        protected override void FlushConnection()
        {
            //throw new NotImplementedException();
        }

        protected override void FlushInputConnection()
        {
            //throw new NotImplementedException();
        }

        protected override bool IsConnectionOpen()
        {
            if (ConnectedASM == null)
            {
                return false;
            }
            if (ConnectedASM.State.Equals(DeviceState.Connected))
            {
                return true;
            }
            return false;
        }

        private void UartRX_ValueUpdated(object sender, Plugin.BLE.Abstractions.EventArgs.CharacteristicUpdatedEventArgs e)
        {
           
            byte[] bytes = e.Characteristic.Value;
            for (int i = 0; i < bytes.Length; i++)
            {
                Buffer.Add(bytes[i]);
            }
            RequestTCS.TrySetResult(true);

        }

        ICharacteristic UartRX { get; set; }
        ICharacteristic UartTX { get; set; }
        IService ServiceTXRX { get; set; }
        public int GallCallBackErrorCount = 0;
        public IDevice ConnectedASM { get; set; }
        static IAdapter adapter { get { return CrossBluetoothLE.Current.Adapter; } }
        void TimeoutConnect(Object obj)
        {
            cancel.Cancel();
        }
        CancellationTokenSource cancel = new CancellationTokenSource();
        TaskCompletionSource<bool> RequestTCS { get; set; }
        public new async Task<bool> Connect()
        {
            BLERadio = new RadioPluginBLE();

            BLERadio.Asm_uuid = Asm_uuid;

            var localTask = new TaskCompletionSource<bool>();

                     try
                {
                    var timeout = 5000;
                    cancel = new CancellationTokenSource();
                    TimeSpan timespan = new TimeSpan(0, 0, 5);
                    Timer timer = new Timer(TimeoutConnect, null, 10000, Timeout.Infinite);
                    SetState(SHIMMER_STATE_CONNECTING);
                    ConnectedASM = await adapter.ConnectToKnownDeviceAsync(Asm_uuid, new ConnectParameters(false, true), cancel.Token);
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

                    ConnectedASM.UpdateConnectionInterval(ConnectionInterval.High);
                    await ConnectedASM.RequestMtuAsync(251);

                    if (ConnectedASM.State != DeviceState.Connected)
                    {
                        localTask.TrySetResult(false);
                        return false;
                    }

                    await Task.Delay(500);
                    System.Console.WriteLine("Getting Service");
                    ServiceTXRX = await ConnectedASM.GetServiceAsync(new Guid("49535343-fe7d-4ae5-8fa9-9fafd205e455"));

                    if (ServiceTXRX != null)
                    {
                        UartTX = await ServiceTXRX.GetCharacteristicAsync(new Guid("49535343-8841-43f4-a8d4-ecbe34729bb3"));
                        System.Console.WriteLine("Getting TX Characteristics Completed");

                        UartRX = await ServiceTXRX.GetCharacteristicAsync(new Guid("49535343-1e4d-4bd9-ba61-23c647249616"));
                        System.Console.WriteLine("Getting RX Characteristics Completed");
                        UartRX.ValueUpdated += UartRX_ValueUpdated;

                        await UartRX.StartUpdatesAsync();

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

            if (IsConnectionOpen())
            {
                StopReading = false;
                ReadThread = new Thread(new ThreadStart(ReadData));
                ReadThread.Name = "Read Thread for Device: " + DeviceName;
                ReadThread.Start();

                // Set default firmware version values, if there is not response it means that this values remain, and the old firmware version has been detected
                // The following are the three main identifiers used to identify the firmware version
                FirmwareIdentifier = 1;

                FirmwareVersionFullName = "BoilerPlate 0.1.0";
                FirmwareInternal = 0;

                RequestTCS = new TaskCompletionSource<bool>();

                //await UartTX.WriteAsync(new byte[1] { (byte)PacketTypeShimmer2.GET_FW_VERSION_COMMAND });
                //await WriteBytes(new byte[1] { (byte)PacketTypeShimmer2.GET_FW_VERSION_COMMAND }, 0, 1);
                //System.Threading.Thread.Sleep(200);
                //var result = await RequestTCS.Task;

                WriteBytes(new byte[1] { (byte)PacketTypeShimmer2.GET_FW_VERSION_COMMAND }, 0, 1);
                //await UartTX.WriteAsync(new byte[1] { (byte)PacketTypeShimmer2.GET_FW_VERSION_COMMAND });
                System.Threading.Thread.Sleep(200);
                //var result = await RequestTCS.Task;
                if (FirmwareMajor == 1 && FirmwareMinor == 2)//FirmwareVersion != 1.2) //Shimmer2r and Shimmer3 commands differ, using FWVersion to determine if its a Shimmer2r for the time being, future revisions of BTStream (Shimmer2r, should update the command to 3F)
                {
                    WriteBytes(new byte[1] { (byte)PacketTypeShimmer2.GET_SHIMMER_VERSION_COMMAND }, 0, 1);
                }
                else
                {
                    WriteBytes(new byte[1] { (byte)PacketTypeShimmer3.GET_SHIMMER_VERSION_COMMAND }, 0, 1);
                    //await UartTX.WriteAsync(new byte[1] { (byte)PacketTypeShimmer3.GET_SHIMMER_VERSION_COMMAND });
                    //result = await RequestTCS.Task;
                }
                System.Threading.Thread.Sleep(400);

                ReadBlinkLED();
                //result = await RequestTCS.Task;
                if (HardwareVersion == (int)ShimmerBluetooth.ShimmerVersion.SHIMMER2R || HardwareVersion == (int)ShimmerBluetooth.ShimmerVersion.SHIMMER2)
                {
                    InitializeShimmer2();
                }
                else if (HardwareVersion == (int)ShimmerBluetooth.ShimmerVersion.SHIMMER3)
                {
                    if (GetFirmwareIdentifier() == FW_IDENTIFIER_LOGANDSTREAM)
                    {
                        //WriteBatteryFrequency(0);
                        ReadExpansionBoard();
                        InitializeShimmer3SDBT();
                    }
                    else if (GetFirmwareIdentifier() == FW_IDENTIFIER_BTSTREAM)
                    {
                        //WriteBatteryFrequency(0);
                        InitializeShimmer3();
                    }
                    else if (GetFirmwareIdentifier() == 13)
                    {
                        //WriteBatteryFrequency(0);
                        InitializeShimmer3();
                    }
                    else if (GetFirmwareIdentifier() == FW_IDENTIFIER_SHIMMERECGMD)
                    {
                        //WriteBatteryFrequency(0);
                        InitializeShimmerECGMD();
                    }
                }

            }

            return false ;


        }

        protected override int ReadByte()
        {
            return (int)Buffer.Take();
        }

        protected override void WriteBytes(byte[] b, int index, int length)
        {
            var res = UartTX.WriteAsync(b);
            res.Wait(1000);
        }

        protected override void OpenConnection()
        {
            throw new NotImplementedException();
        }
    }
}
