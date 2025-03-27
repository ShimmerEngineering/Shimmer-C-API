using com.shimmerresearch.grpc;
using Grpc.Core;
using Grpc.Core.Logging;
using InTheHand.Bluetooth;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShimmerBLEGrpc
{
    public class ShimmerBLEServiceImpl : ShimmerBLEByteServer.ShimmerBLEByteServerBase
    {
        readonly string Verisense = "Verisense";
        readonly string Shimmer = "Shimmer";
        bool Debug = false;
        //ConcurrentDictionary<string, BluetoothDevice> BluetoohDeviceMap = new ConcurrentDictionary<string, BluetoothDevice>();
        Dictionary<string, BluetoothDevice> BluetoohDeviceMap = new Dictionary<string, BluetoothDevice>();

        Dictionary<string, GattService> ServiceMap = new Dictionary<string, GattService>();
        Dictionary<string, GattCharacteristic> UartTXMap = new Dictionary<string, GattCharacteristic>();
        Dictionary<string, GattCharacteristic> UartRXMap = new Dictionary<string, GattCharacteristic>();
        Dictionary<string, ConcurrentQueue<byte[]>> QueueMap = new Dictionary<string, ConcurrentQueue<byte[]>>();
        Dictionary<string, bool> StreamThreadMap = new Dictionary<string, bool>();
        Dictionary<string, IServerStreamWriter<StateStatus>> ConnectStreamMap = new Dictionary<string, IServerStreamWriter<StateStatus>>();
        private readonly ILogger<ShimmerBLEServiceImpl> _logger;
        public ShimmerBLEServiceImpl()
        {
        }
        public ShimmerBLEServiceImpl(ILogger<ShimmerBLEServiceImpl> logger)
        {
            _logger = logger;
        }
        public override Task<Reply> SayHello(Request request, ServerCallContext context)
        {
            return Task.FromResult(new Reply
            {
                Message = "Hello " + request.Name
            });
        }

        public override async Task<Reply> WriteBytesShimmer(WriteBytes bytes, ServerCallContext context)
        {
            Console.WriteLine("Write Bytes");
            if (UartTXMap.ContainsKey(bytes.Address.ToUpper()))
            {
                await UartTXMap[bytes.Address.ToUpper()].WriteValueWithoutResponseAsync(bytes.ByteToWrite.ToByteArray());
            }

            return new Reply
            {
                Message = "Written " + bytes.Address
            };
        }

        public override async Task ConnectShimmer(Request request, IServerStreamWriter<StateStatus> stateStatusStream, ServerCallContext context)
        {
            string macAddress = request.Name.ToUpper();
            Console.WriteLine("Attempting to connect " + macAddress);
            BluetoothDevice bluetoothDevice = await BluetoothDevice.FromIdAsync(request.Name);
            if (BluetoohDeviceMap.ContainsKey(macAddress))
            {
                BluetoohDeviceMap.Remove(macAddress);
            }
            BluetoohDeviceMap.Add(request.Name, bluetoothDevice);
            await bluetoothDevice.Gatt.ConnectAsync();
            Console.WriteLine("current mtu value " + bluetoothDevice.Gatt.Mtu);
            if (bluetoothDevice.Gatt.Mtu == 23)
            {
                bluetoothDevice.Gatt.Disconnect();
                await stateStatusStream.WriteAsync(new StateStatus
                {
                    Message = "MTU is 23",
                    State = BluetoothState.Disconnected
                });
                return;
            }
            BluetoothUuid TxID;
            BluetoothUuid RxID;
            BluetoothUuid ServiceID;
            if (bluetoothDevice.Name.Contains(Shimmer))
            {
                TxID = BluetoothUuid.FromGuid(new Guid("49535343-8841-43f4-a8d4-ecbe34729bb3"));
                RxID = BluetoothUuid.FromGuid(new Guid("49535343-1e4d-4bd9-ba61-23c647249616"));
                ServiceID = BluetoothUuid.FromGuid(new Guid("49535343-fe7d-4ae5-8fa9-9fafd205e455"));
            }
            else if (bluetoothDevice.Name.Contains(Verisense))
            {
                TxID = BluetoothUuid.FromGuid(new Guid("6E400002-B5A3-F393-E0A9-E50E24DCCA9E"));
                RxID = BluetoothUuid.FromGuid(new Guid("6E400003-B5A3-F393-E0A9-E50E24DCCA9E"));
                ServiceID = BluetoothUuid.FromGuid(new Guid("6E400001-B5A3-F393-E0A9-E50E24DCCA9E"));
            }
            else // just assume it is a shimmer device
            {
                TxID = BluetoothUuid.FromGuid(new Guid("49535343-8841-43f4-a8d4-ecbe34729bb3"));
                RxID = BluetoothUuid.FromGuid(new Guid("49535343-1e4d-4bd9-ba61-23c647249616"));
                ServiceID = BluetoothUuid.FromGuid(new Guid("49535343-fe7d-4ae5-8fa9-9fafd205e455"));
            }
            GattService ServiceTXRX = await bluetoothDevice.Gatt.GetPrimaryServiceAsync(ServiceID);
            if (ServiceTXRX == null)
            {
                await stateStatusStream.WriteAsync(new StateStatus
                {
                    Message = "Service is null",
                    State = BluetoothState.Disconnected
                });
                return;
            }
            GattCharacteristic UartTX = await ServiceTXRX.GetCharacteristicAsync(TxID);
            if (UartTX == null)
            {
                await stateStatusStream.WriteAsync(new StateStatus
                {
                    Message = "UARTTX is null",
                    State = BluetoothState.Disconnected
                });
                return;
            }
            GattCharacteristic UartRX = await ServiceTXRX.GetCharacteristicAsync(RxID);
            if (UartRX == null)
            {
                await stateStatusStream.WriteAsync(new StateStatus
                {
                    Message = "UARTRX is null",
                    State = BluetoothState.Disconnected
                });
                return;
            }
            if (ServiceMap.ContainsKey(macAddress))
            {
                ServiceMap.Remove(macAddress);
            }
            if (UartTXMap.ContainsKey(macAddress))
            {
                UartTXMap.Remove(macAddress);
            }
            if (UartRXMap.ContainsKey(macAddress))
            {
                UartRXMap.Remove(macAddress);
            }
            ServiceMap.Add(macAddress, ServiceTXRX);
            UartTXMap.Add(macAddress, UartTX);
            UartRXMap.Add(macAddress, UartRX);
            if (QueueMap.ContainsKey(macAddress))
            {
                QueueMap[macAddress] = new ConcurrentQueue<byte[]>();
            }
            UartRX.CharacteristicValueChanged += Gc_ValueChanged;
            await UartRX.StartNotificationsAsync();
            var data = new StateStatus
            {
                Message = "Success",
                State = BluetoothState.Connected
            };
            bluetoothDevice.GattServerDisconnected += BluetoothDevice_GattServerDisconnected;
            ConnectStreamMap.Add(macAddress, stateStatusStream);
            await stateStatusStream.WriteAsync(data);
            while (BluetoohDeviceMap.ContainsKey(macAddress))
            {
                Thread.Sleep(100);
            }
        }

        private void BluetoothDevice_GattServerDisconnected(object sender, EventArgs e)
        {
            BluetoothDevice bluetoothDevice = (BluetoothDevice)sender;
            Console.WriteLine(bluetoothDevice.Id);
            var data = new StateStatus
            {
                Message = "Connection Lost",
                State = BluetoothState.Disconnected
            };
            ConnectStreamMap[bluetoothDevice.Id.ToUpper()].WriteAsync(data);
            ConnectStreamMap.Remove(bluetoothDevice.Id.ToUpper());
            UartRXMap[bluetoothDevice.Id.ToUpper()].CharacteristicValueChanged -= Gc_ValueChanged;
            bluetoothDevice.GattServerDisconnected -= BluetoothDevice_GattServerDisconnected;
            BluetoohDeviceMap[bluetoothDevice.Id.ToUpper()].Gatt.Disconnect();
            BluetoohDeviceMap.Remove(bluetoothDevice.Id.ToUpper());
            QueueMap.Remove(bluetoothDevice.Id.ToUpper());


        }

        private void Gc_ValueChanged(object sender, GattCharacteristicValueChangedEventArgs args)
        {
            var gc = (GattCharacteristic)sender;
            if (Debug)
            {
                Console.WriteLine("RXB:" + BitConverter.ToString(args.Value).Replace("-", ""));
            }
            if (QueueMap.ContainsKey(gc.Service.Device.Id.ToUpper()))
            {
                QueueMap[gc.Service.Device.Id.ToUpper()].Enqueue(args.Value);
            }
            else
            {
                ConcurrentQueue<byte[]> cq = new ConcurrentQueue<byte[]>();
                cq.Enqueue(args.Value);
                QueueMap.Add(gc.Service.Device.Id.ToUpper(), cq);
            }
            
        }

        public override async Task<Reply> DisconnectShimmer(Request request, ServerCallContext context)
        {
            UartRXMap[request.Name.ToUpper()].CharacteristicValueChanged -= Gc_ValueChanged;
            BluetoohDeviceMap[request.Name.ToUpper()].Gatt.Disconnect();
            BluetoohDeviceMap.Remove(request.Name.ToUpper());
            QueueMap.Remove(request.Name.ToUpper());
            return new Reply
            {
                Message = "Disconnect " + request.Name
            };
            var data = new StateStatus
            {
                Message = "Disconnected",
                State = BluetoothState.Disconnected
            };
            await ConnectStreamMap[request.Name.ToUpper()].WriteAsync(data);
            ConnectStreamMap.Remove(request.Name.ToUpper());
        }

        Dictionary<string, long> hashMap = new Dictionary<string, long>();


        public override async Task GetDataStream(StreamRequest request, IServerStreamWriter<ObjectClusterByteArray> responseStream, ServerCallContext context)
        {
            while (BluetoohDeviceMap.ContainsKey(request.Message.ToUpper()))
            {
                byte[] byteArray;
                if (QueueMap.ContainsKey(request.Message.ToUpper()))
                {
                    if (!QueueMap[request.Message.ToUpper()].IsEmpty)
                    {
                        byte[] result = new byte[0];
                        while (!QueueMap[request.Message.ToUpper()].IsEmpty)
                        {


                            if (QueueMap[request.Message.ToUpper()].TryDequeue(out byteArray))
                            {
                                result = result.Concat(byteArray).ToArray();
                                if (Debug)
                                {
                                    Console.WriteLine("Dequeuing");
                                }
                                // Get the current UTC time

                            }

                        }
                        DateTimeOffset currentTime = DateTimeOffset.UtcNow;

                        // Convert the current UTC time to Unix timestamp (seconds since Unix epoch)
                        long unixTimestamp = currentTime.ToUnixTimeSeconds();
                        var data = new ObjectClusterByteArray
                        {
                            BluetoothAddress = request.Message,
                            SystemTime = unixTimestamp,
                            BinaryData = Google.Protobuf.ByteString.CopyFrom(result)
                        };
                        await responseStream.WriteAsync(data);
                    }
                }
                Thread.Sleep(1);

            }
            Console.WriteLine("Thread STOP" + request.Message.ToUpper());
        }
        public override async Task GetTestDataStream(StreamRequest request, IServerStreamWriter<ObjectClusterByteArray> responseStream, ServerCallContext context)
        {
            Console.WriteLine("Get Test Data Stream");
            long tscount = 0;
            if (hashMap.ContainsKey(request.Message))
            {
                tscount = hashMap[request.Message];
            } else
            {
                tscount = 0;
                hashMap.Add(request.Message, tscount);
            }
            for (var i = 0; i < 100000000; i++)
            {

                // Create a random number generator
                Random random = new Random();

                // Define the size of the byte array
                int byteArraySize = 256;

                // Create a byte array
                byte[] byteArray = new byte[byteArraySize];

                // Fill the byte array with random values
                random.NextBytes(byteArray);

                // You can now use the byteArray with your random data as needed

                // Create a request with the byte array
                var data = new ObjectClusterByteArray
                {
                    BluetoothAddress = request.Message,
                    SystemTime = tscount,
                    BinaryData = Google.Protobuf.ByteString.CopyFrom(byteArray)
                };
                tscount++;
                await responseStream.WriteAsync(data);
            }
        }

    }
}
