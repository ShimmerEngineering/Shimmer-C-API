using Newtonsoft.Json;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using shimmer.Helpers;
using shimmer.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace shimmer.Services
{
    /// <summary>
    /// Speed test service
    /// </summary>
    public class SpeedTestService : IObservable<String>
    {
        public IDevice ConnectedASM { get; set; }
        static IAdapter adapter { get { return CrossBluetoothLE.Current.Adapter; } }
        ICharacteristic UartRX { get; set; }
        public ICharacteristic UartTX { get; set; }
        public string Asm_uuid { get; set; }
        byte[] ReadMemoryLookupTableRequest = new byte[] { 0x29, 0x01, 0x00, 0x01 };
        bool ReceivingMemoryLookupData { get; set; }
        bool NewChunk { get; set; }
        public byte[] ResponseBuffer { get; set; }
        public DataChunkNew DataBuffer { get; set; }
        TaskCompletionSource<bool> RequestTCS { get; set; }
        public double SpeedinKBS = 0;
        private long TSsub = DateHelper.GetTimestamp(DateTime.UtcNow);

        //override
        /// <summary>
        /// Create a SpeedTestService instance
        /// </summary>
        /// <param name="id">the uuid for the address of which is used to connect to via BLE e.g. "00000000-0000-0000-0000-e7452c6d6f14" note that the uuid across OS (android vs iOS) can differ </param>
        public SpeedTestService(string id)
        {
            Asm_uuid = id;
        }

        protected virtual void SendMessage(DataDebugItem data)
        {
            MessagingCenter.Send(this, "BLELog", data);
        }

        protected virtual async void DisplayAlertASMFailedConnection()
        {

        }
        //override
        /// <summary>
        /// Connects to the verisense device using <see cref="Asm_uuid"/>
        /// </summary>
        /// <returns>Returns true if successful</returns>
        public async Task<bool> GetKnownDevice()
        {
            var localTask = new TaskCompletionSource<bool>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    ConnectedASM = await adapter.ConnectToKnownDeviceAsync(Guid.Parse(Asm_uuid));
                    await Task.Delay(500);
                    if (ConnectedASM.State != DeviceState.Connected)
                    {
                        DisplayAlertASMFailedConnection();
                        //await NavigationService.DisplayAlert(ShimmerBLEAPI.Resources.AppResources.ASMConnectionFailedTryAgain);
                        localTask.TrySetResult(false);
                    }

                    ConnectedASM.UpdateConnectionInterval(ConnectionInterval.High);
                    await ConnectedASM.RequestMtuAsync(251);
                }
                catch (Exception ex)
                {
                    var data = new DataDebugItem
                    {
                        Date = DateTime.Now,
                        Action = "ConnectToKnownDeviceAsync Exception",
                        Data = JsonConvert.SerializeObject(ex),
                    };
                    SendMessage(data);
                    Console.WriteLine(ex);
                    return;
                }


                try
                {
                    var service = await ConnectedASM.GetServiceAsync(App.ServiceID);

                    if (service != null)
                    {
                        UartTX = await service.GetCharacteristicAsync(App.TxID);
                        UartRX = await service.GetCharacteristicAsync(App.RxID);

                        await UartRX.StartUpdatesAsync();

                        UartRX.ValueUpdated += UartRX_ValueUpdated; ;

                        var data = new DataDebugItem
                        {
                            Date = DateTime.Now,
                            Action = "GetKnownDevice",
                            Data = "Success",
                        };
                        SendMessage( data);
                        localTask.TrySetResult(true);
                    } else
                    {
                        localTask.TrySetResult(false);
                    }
                }
                catch (Exception ex)
                {
                    var data = new DataDebugItem
                    {
                        Date = DateTime.Now,
                        Action = "OnDeviceConnected",
                        Data = JsonConvert.SerializeObject(ex),
                    };
                    SendMessage( data);
                    Console.WriteLine(ex);
                    localTask.TrySetResult(false);
                    return;
                }
            });
            var result = await localTask.Task;
            return result;
        }

        protected virtual void EventDisconnect()
        {
            //Do something
        }

        //override
        /// <summary>
        /// Disconnects the verisense device
        /// </summary>
        public async Task Disconnect()
        {
            EventDisconnect();
            try
            {
                MessagingCenter.Send(this, "ASMDisconnected");

                UartRX = null;
                UartTX = null;
                ResponseBuffer = null;

                await adapter.DisconnectDeviceAsync(ConnectedASM);
                ConnectedASM.Dispose();

                var data = new DataDebugItem
                {
                    Date = DateTime.Now,
                    Action = "Disconnect",
                    Data = "Success",
                };
                SendMessage( data);

            }
            catch (Exception ex)
            {
                var data = new DataDebugItem
                {
                    Date = DateTime.Now,
                    Action = "DisconnectException",
                    Data = JsonConvert.SerializeObject(ex),
                };
                SendMessage( data);

                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Excute request <see cref="ReadMemoryLookupTableRequest"/>
        /// </summary>
        public async Task ExecuteMemoryLookupTableCommand()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                RequestTCS = new TaskCompletionSource<bool>();
                try
                {
                    ReadMemoryLookupTable();
                    await RequestTCS.Task;
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return;
                }
            });
        }

        void ReadMemoryLookupTable()
        {
            try
            {
                NewChunk = true;
                ReceivingMemoryLookupData = true;
                UartTX.WriteAsync(ReadMemoryLookupTableRequest);
            }
            catch (Exception ex)
            {
                var data = new DataDebugItem
                {
                    Date = DateTime.Now,
                    Action = "ReadTableException",
                    Data = JsonConvert.SerializeObject(ex),
                };
                SendMessage( data);

            }

        }

        /// <summary>
        /// Characteristic value updated event
        /// </summary>
        public void UartRX_ValueUpdated(object sender, Plugin.BLE.Abstractions.EventArgs.CharacteristicUpdatedEventArgs e)
        {
            ResponseBuffer = e.Characteristic.Value;
            
            if (ReceivingMemoryLookupData)
            {
                HandleMemoryLookupDataPayload(ResponseBuffer);
                return;
            }
            
        }

        void HandleMemoryLookupDataPayload(byte[] payload)
        {
            try
            {
                if (NewChunk)
                {
                    var chunkResult = CreateMemoryPayloadChunk(payload);

                    if (!chunkResult)
                        return;
                }
                else
                {
                    Buffer.BlockCopy(payload, 0, DataBuffer.Packets, DataBuffer.CurrentLength, payload.Count());
                    DataBuffer.CurrentLength += payload.Length;

                    var data = new DataDebugItem
                    {
                        Date = DateTime.Now,
                        Action = "PayloadChunk",
                        Data = string.Format("Chunk Length" + " =" + payload.Length + "; " + "Current Length" + " =" + DataBuffer.CurrentLength + "; " + "Expected Length" + " =" + DataBuffer.ExpectedLength),
                    };
                    SendMessage( data);
                }
                Trace.Message("Speed Test, Current Length vs Expected Length: " +DataBuffer.CurrentLength + " " + DataBuffer.ExpectedLength);
                if (DateHelper.GetTimestamp(DateTime.UtcNow) - TSsub > 1000)
                {
                    TSsub = DateHelper.GetTimestamp(DateTime.UtcNow);
                    foreach (var observer in observers)
                        observer.OnNext("Speed Test, Current Length vs Expected Length: " + DataBuffer.CurrentLength + " " + DataBuffer.ExpectedLength);
                }


                if (DataBuffer.CurrentLength >= DataBuffer.ExpectedLength)
                {
                    DataBuffer.Finish = DateTime.Now;
                    DataBuffer.Transfer = (DataBuffer.CurrentLength / (DataBuffer.Finish - DataBuffer.Start).TotalSeconds) / 1024.0;

                    var data = new DataDebugItem
                    {
                        Date = DateTime.Now,
                        Action = "",
                        Data = string.Format("Transfer Rate" + " =" + DataBuffer.Transfer + "KB/s"),
                    };
                    SendMessage( data);
                    Trace.Message("Transfer rate in KB/s: " + DataBuffer.Transfer);
                    Console.WriteLine("Transfer rate = {0} KB/s", DataBuffer.Transfer);
                    foreach (var observer in observers)
                        observer.OnNext("Transfer rate in KB/s: " + DataBuffer.Transfer);
                    HandleMemoryContent(DataBuffer.Packets);
                    SpeedinKBS = DataBuffer.Transfer;

                    NewChunk = true;
                    RequestTCS.SetResult(true);
                }
            }
            catch (Exception ex)
            {
                var data = new DataDebugItem
                {
                    Date = DateTime.Now,
                    Action = "ProcessingMemoryLookupDataPayloadException",
                    Data = JsonConvert.SerializeObject(ex),
                };
                SendMessage( data);
            }

        }

        bool CreateMemoryPayloadChunk(byte[] payload)
        {
            try
            {
                if (payload.Length != 3)
                {
                    RequestTCS.SetResult(false);
                    return false;
                }
                var data = new DataDebugItem
                {
                    Date = DateTime.Now,
                    Action = "NewPayloadHead",
                    Data = BitConverter.ToString(payload),
                };
                // SendMessage( data);

                var stream = new MemoryStream(payload);
                var reader = new BinaryReader(stream);

                var header = reader.ReadByte();

                var lengthBytes = reader.ReadBytes(2);
                Array.Reverse(lengthBytes);
                var length = int.Parse(BitConverter.ToString(lengthBytes).Replace("-", string.Empty), NumberStyles.HexNumber);
                DataBuffer = new DataChunkNew(length);
                int offset = 3;
                DataBuffer.CurrentLength += payload.Length - offset;

                DataBuffer.ExpectedLength = length;
                ReceivingMemoryLookupData = true;
                NewChunk = false;
                
                reader.Close();
                stream = null;

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                var data = new DataDebugItem
                {
                    Date = DateTime.Now,
                    Action = "CreateChunk Exception",
                    Data = JsonConvert.SerializeObject(ex),
                };
                SendMessage( data);
                return false;
            }

        }

        void HandleMemoryContent(byte[] packets)
        {
            try
            {

                int countZero = 0;
                int countFull = 0;
                int countToDel = 0;
                int countEmpty = 0;
                int countBad = 0;
                int countDef = 0;
                int offset = 4;
                var stream = new MemoryStream(packets);
                var reader = new BinaryReader(stream);

                var tailIndex = reader.ReadBytes(2);
                Array.Reverse(tailIndex);
                var tailLoc = int.Parse(BitConverter.ToString(tailIndex).Replace("-", string.Empty), NumberStyles.HexNumber);

                var headIndex = reader.ReadBytes(2);
                Array.Reverse(headIndex);
                var headLoc = int.Parse(BitConverter.ToString(headIndex).Replace("-", string.Empty), NumberStyles.HexNumber);
                int content = packets.Length - offset;  //exclude first 4bytes
                offset = 3;

                for (int i = 0; i < content; i += offset)
                {
                    var map = new Dictionary<int, int>();
                    var header = reader.ReadByte();

                    switch (header)
                    {
                        case 0:
                            countZero++;
                            break;
                        case 1:
                            countFull++;
                            break;
                        case 2:
                            countToDel++;
                            break;
                        case 3:
                            countEmpty++;
                            break;
                        case 4:
                            countBad++;
                            break;
                        default:
                            countDef++;
                            break;
                    }

                    var skipBytes = reader.ReadBytes(2);    //skip payload index of the memory stored

                }
                var data = new DataDebugItem
                {
                    Date = DateTime.Now,
                    Action = "",
                    Data = "Summary Table" + ": \n\n" + "Key" + " = " + "Empty" + ", " + "Count" + " = " + countZero +
                    "\n" + "Key" + " = " + "Full" + ", " + "Count" + " = " + countFull +
                    "\n" + "Key" + " = " + "ToDel" + ", " + "Count" + " = " + countToDel +
                    "\n" + "Key" + " = " + "Empty" + ", " + "Count" + " = " + countEmpty +
                    "\n" + "Key" + " = " + "Bad" + ", " + "Count" + " = " + countBad +
                    "\n" + "Key" + " = " + "Default" + ", " + "Count" + " = " + countDef +
                    "\n\n" + "Current Tail Location" + " = " + tailLoc +
                    "\n" + "Current Head Location" + " = " + headLoc

                };
                SendMessage( data);
                
            }
            catch (Exception ex)
            {
                var data = new DataDebugItem
                {
                    Date = DateTime.Now,
                    Action = "ProcessingMemoryContentException",
                    Data = JsonConvert.SerializeObject(ex),
                };
                SendMessage( data);
            }
        }
        private List<IObserver<String>> observers = new List<IObserver<String>>();
        /// <summary>
        /// Observer subscribe to the speed test to receive notifications
        /// </summary>
        public IDisposable Subscribe(IObserver<String> observer)
        {
            // Check whether observer is already registered. If not, add it
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
                // Provide observer with existing data.
                //observer.OnNext(DataBuffer.Transfer);
            }
            return new Unsubscriber<String>(observers, observer);
        }
        internal class Unsubscriber<String> : IDisposable
        {
            private List<IObserver<String>> _observers;
            private IObserver<String> _observer;

            internal Unsubscriber(List<IObserver<String>> observers, IObserver<String> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }
    
}
