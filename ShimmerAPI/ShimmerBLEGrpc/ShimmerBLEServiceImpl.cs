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
        bool Debug = false;
        ConcurrentDictionary<string, BluetoothDevice> BluetoohDeviceMap = new ConcurrentDictionary<string, BluetoothDevice>();
        ConcurrentDictionary<string, GattService> ServiceMap = new ConcurrentDictionary<string, GattService>();
        ConcurrentDictionary<string, GattCharacteristic> UartTXMap = new ConcurrentDictionary<string, GattCharacteristic>();
        ConcurrentDictionary<string, GattCharacteristic> UartRXMap = new ConcurrentDictionary<string, GattCharacteristic>();
        ConcurrentDictionary<string, ConcurrentQueue<byte[]>> QueueMap = new ConcurrentDictionary<string, ConcurrentQueue<byte[]>>();
        ConcurrentDictionary<string, bool> StreamThreadMap = new ConcurrentDictionary<string, bool>();
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
            if (UartTXMap.ContainsKey(bytes.Address.ToUpper()))
            {
                await UartTXMap[bytes.Address.ToUpper()].WriteValueWithoutResponseAsync(bytes.ByteToWrite.ToByteArray());
            }

            return new Reply
            {
                Message = "Written " + bytes.Address
            };
        }

        public override async Task<Reply> ConnectShimmer(Request request, ServerCallContext context)
        {
            string macAddress = request.Name.ToUpper();
            BluetoothDevice bluetoothDevice = await BluetoothDevice.FromIdAsync(request.Name);
            if (BluetoohDeviceMap.ContainsKey(macAddress))
            {
                BluetoohDeviceMap.TryRemove(macAddress, out var s);
            }
            BluetoohDeviceMap.TryAdd(request.Name, bluetoothDevice);
            await bluetoothDevice.Gatt.ConnectAsync();
            Console.WriteLine("current mtu value " + bluetoothDevice.Gatt.Mtu);
            bool shimmer = false;
            BluetoothUuid TxID;
            BluetoothUuid RxID;
            BluetoothUuid ServiceID;
            if (shimmer)
            {
                TxID = BluetoothUuid.FromGuid(new Guid("49535343-8841-43f4-a8d4-ecbe34729bb3"));
                RxID = BluetoothUuid.FromGuid(new Guid("49535343-1e4d-4bd9-ba61-23c647249616"));
                ServiceID = BluetoothUuid.FromGuid(new Guid("49535343-fe7d-4ae5-8fa9-9fafd205e455"));
            }
            else
            {
                TxID = BluetoothUuid.FromGuid(new Guid("6E400002-B5A3-F393-E0A9-E50E24DCCA9E"));
                RxID = BluetoothUuid.FromGuid(new Guid("6E400003-B5A3-F393-E0A9-E50E24DCCA9E"));
                ServiceID = BluetoothUuid.FromGuid(new Guid("6E400001-B5A3-F393-E0A9-E50E24DCCA9E"));
            }
            GattService ServiceTXRX = await bluetoothDevice.Gatt.GetPrimaryServiceAsync(ServiceID);
            GattCharacteristic UartTX = await ServiceTXRX.GetCharacteristicAsync(TxID);
            GattCharacteristic UartRX = await ServiceTXRX.GetCharacteristicAsync(RxID);
            if (ServiceMap.ContainsKey(macAddress))
            {
                ServiceMap.TryRemove(macAddress, out var s);
            }
            if (UartTXMap.ContainsKey(macAddress))
            {
                UartTXMap.TryRemove(macAddress, out var s);
            }
            if (UartRXMap.ContainsKey(macAddress))
            {
                UartRXMap.TryRemove(macAddress, out var s);
            }
            ServiceMap.TryAdd(macAddress, ServiceTXRX);
            UartTXMap.TryAdd(macAddress, UartTX);
            UartRXMap.TryAdd(macAddress, UartRX);
            UartRX.CharacteristicValueChanged += Gc_ValueChanged;
            await UartRX.StartNotificationsAsync();
            return new Reply
            {
                Message = "Connected " + macAddress
            };
            
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
                QueueMap.TryAdd(gc.Service.Device.Id.ToUpper(), cq);
            }
            
        }

        public override async Task<Reply> DisconnectShimmer(Request request, ServerCallContext context)
        {
            UartRXMap[request.Name.ToUpper()].CharacteristicValueChanged -= Gc_ValueChanged;
            BluetoohDeviceMap[request.Name.ToUpper()].Gatt.Disconnect();
            BluetoohDeviceMap.TryRemove(request.Name.ToUpper(), out var s);
            QueueMap.TryRemove(request.Name.ToUpper(), out var t);
            return new Reply
            {
                Message = "Disconnect " + request.Name
            };
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
                                Console.WriteLine("Dequeuing");
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
