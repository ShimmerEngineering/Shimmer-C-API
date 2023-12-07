using InTheHand.Bluetooth;
using ShimmerAPI;
using ShimmerAPI.Radios;
using ShimmerAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shimmer32FeetAPI.Radios
{
    public class BLE32FeetRadio : AbstractRadio
    {
        protected byte[] OldTestData = new byte[0];
        protected bool TestFirstByteReceived = false;
        protected long TestSignalTotalNumberOfBytes = 0;
        protected double TestSignalTSStart = 0;
        protected bool TestSignalEnabled = false;

        public BLE32FeetRadio(string macAddress)
        {
            MacAddress = macAddress;
        }


        private BluetoothDevice bluetoothDevice { get; set; }
        private GattService ServiceTXRX { get; set; }
        private GattCharacteristic UartTX { get; set; }
        private GattCharacteristic UartRX { get; set; }
        String MacAddress;
        protected void OpenConnection()
        {
            try
            {
                //SetState(SHIMMER_STATE_CONNECTING);
                bluetoothDevice = BluetoothDevice.FromIdAsync(MacAddress).GetAwaiter().GetResult();
                bluetoothDevice.Gatt.ConnectAsync().GetAwaiter().GetResult();
                Console.WriteLine("current mtu value " + bluetoothDevice.Gatt.Mtu);
                BluetoothUuid TxID = BluetoothUuid.FromGuid(new Guid("49535343-8841-43f4-a8d4-ecbe34729bb3"));
                BluetoothUuid RxID = BluetoothUuid.FromGuid(new Guid("49535343-1e4d-4bd9-ba61-23c647249616"));
                BluetoothUuid ServiceID = BluetoothUuid.FromGuid(new Guid("49535343-fe7d-4ae5-8fa9-9fafd205e455"));
                ServiceTXRX = bluetoothDevice.Gatt.GetPrimaryServiceAsync(ServiceID).GetAwaiter().GetResult();
                if (ServiceTXRX != null)
                {
                    UartTX = ServiceTXRX.GetCharacteristicAsync(TxID).GetAwaiter().GetResult();
                    if (UartTX == null)
                    {
                        Console.WriteLine("UARTTX null");
                        //return false;
                    }
                    UartRX = ServiceTXRX.GetCharacteristicAsync(RxID).GetAwaiter().GetResult();
                    if (UartRX == null)
                    {
                        Console.WriteLine("UARTRX null");
                       // return false;
                    }
                    UartRX.CharacteristicValueChanged += Gc_ValueChanged;
                    UartRX.StartNotificationsAsync().GetAwaiter().GetResult();
                    bluetoothDevice.GattServerDisconnected += Device_GattServerDisconnected;
                    Console.WriteLine("current mtu value" + bluetoothDevice.Gatt.Mtu);
                }
                else
                {
                    Console.WriteLine("Service TXRX null");
                    //return false;
                }

            }
            catch (Exception ex)
            {
                if (bluetoothDevice != null)
                {
                    bluetoothDevice.GattServerDisconnected -= Device_GattServerDisconnected;
                    bluetoothDevice.Gatt.Disconnect();
                }
                Console.WriteLine("Radio Plugin 32Feet Exception " + ex.Message);
                //return false;
            }
            //return true;
        }

        public override bool Connect()
        {
            Thread thread = new Thread(OpenConnection);
            // Start the thread
            thread.Start();
            return true;
            
            /*
            try
            {
                //SetState(SHIMMER_STATE_CONNECTING);
                bluetoothDevice = BluetoothDevice.FromIdAsync(MacAddress).GetAwaiter().GetResult();
                bluetoothDevice.Gatt.ConnectAsync().GetAwaiter().GetResult();
                Console.WriteLine("current mtu value " + bluetoothDevice.Gatt.Mtu);
                BluetoothUuid TxID = BluetoothUuid.FromGuid(new Guid("49535343-8841-43f4-a8d4-ecbe34729bb3"));
                BluetoothUuid RxID = BluetoothUuid.FromGuid(new Guid("49535343-1e4d-4bd9-ba61-23c647249616"));
                BluetoothUuid ServiceID = BluetoothUuid.FromGuid(new Guid("49535343-fe7d-4ae5-8fa9-9fafd205e455"));
                ServiceTXRX = bluetoothDevice.Gatt.GetPrimaryServiceAsync(ServiceID).GetAwaiter().GetResult();
                if (ServiceTXRX != null)
                {
                    UartTX = ServiceTXRX.GetCharacteristicAsync(TxID).GetAwaiter().GetResult();
                    if (UartTX == null)
                    {
                        Console.WriteLine("UARTTX null");
                        return false;
                    }
                    UartRX = ServiceTXRX.GetCharacteristicAsync(RxID).GetAwaiter().GetResult();
                    if (UartRX == null)
                    {
                        Console.WriteLine("UARTRX null");
                        return false;
                    }
                    UartRX.CharacteristicValueChanged += Gc_ValueChanged;
                    UartRX.StartNotificationsAsync().GetAwaiter().GetResult();
                    bluetoothDevice.GattServerDisconnected += Device_GattServerDisconnected;
                    Console.WriteLine("current mtu value" + bluetoothDevice.Gatt.Mtu);
                }
                else
                {
                    Console.WriteLine("Service TXRX null");
                    return false;
                }

            }
            catch (Exception ex)
            {
                if (bluetoothDevice != null)
                {
                    bluetoothDevice.GattServerDisconnected -= Device_GattServerDisconnected;
                    bluetoothDevice.Gatt.Disconnect();
                }
                Console.WriteLine("Radio Plugin 32Feet Exception " + ex.Message);
                return false;
            }
            return true;
            */
        }

        public void StartTestSignal()
        {
            OldTestData = new byte[0];
            TestFirstByteReceived = false;
            TestSignalTotalNumberOfBytes = 0;
            System.Console.WriteLine("Start Test Signal");
            TestSignalTSStart = (DateTime.UtcNow - ShimmerBluetooth.UnixEpoch).TotalMilliseconds;
            if (WriteBytes(new byte[2] { (byte)0xA4, (byte)0x01 }))
            {
                TestSignalEnabled = true;
            }
        }

        public override bool Disconnect()
        {
            return false;
        }

        public override bool WriteBytes(byte[] bytes)
        {
            try
            {
                UartTX.WriteValueWithoutResponseAsync(bytes);
            } catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
            return true;
        }

        private void Device_GattServerDisconnected(object sender, EventArgs e)
        {
            //connection lost
            /*
            Console.WriteLine("Connection Lost. Please reconnect.");
            bluetoothDevice.GattServerDisconnected -= Device_GattServerDisconnected;
            bluetoothDevice.Gatt.Disconnect();
            */
            bluetoothDevice.GattServerDisconnected -= Device_GattServerDisconnected;
            bluetoothDevice.Gatt.Disconnect();
            //CustomEventArgs newEventArgs = new CustomEventArgs((int)ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE, "Connection lost");
            //OnNewEvent(newEventArgs);
            Disconnect();

        }


        private void Gc_ValueChanged(object sender, GattCharacteristicValueChangedEventArgs args)
        {
            byte[] buffer = args.Value;
            if (TestSignalEnabled)
            {
                if (!TestFirstByteReceived)
                {
                    Console.WriteLine("DISCARD BYTE");
                    TestFirstByteReceived = true;
                    ProgrammerUtilities.CopyAndRemoveBytes(ref buffer, 1);

                }

                TestSignalTotalNumberOfBytes += buffer.Length;
                Console.WriteLine();
                Debug.WriteLine(ProgrammerUtilities.ByteArrayToHexString(buffer));
                byte[] data = OldTestData.Concat(buffer).ToArray();
                //byte[] data = newdata;
                double testSignalCurrentTime = (DateTime.UtcNow - ShimmerBluetooth.UnixEpoch).TotalMilliseconds;
                double duration = (testSignalCurrentTime - TestSignalTSStart) / 1000.0; //make it seconds
                Console.WriteLine("Throughput (bytes per second): " + (TestSignalTotalNumberOfBytes / duration));
                //Console.WriteLine("RXB OTD:" + BitConverter.ToString(OldTestData).Replace("-", ""));
                //Console.WriteLine("RXB:" + BitConverter.ToString(data).Replace("-", ""));
                for (int i = 0; i < (data.Length / 4); i++)
                {
                    byte[] bytes = new byte[4];
                    System.Array.Copy(data, i * 4, bytes, 0, 4);
                    //Array.Reverse(bytes);
                    int intValue = BitConverter.ToInt32(bytes, 0);
                    Console.Write(intValue + " , ");
                }
                Console.WriteLine();

                int remainder = data.Length % 4;
                if (remainder != 0)
                {
                    OldTestData = new byte[remainder];
                    System.Array.Copy(data, data.Length - remainder, OldTestData, 0, remainder);
                }
                else
                {
                    OldTestData = new byte[0];
                }
            }


            for (int i = 0; i < args.Value.Length; i++)
            {
                //cq.Enqueue(args.Value[i]);
            }
        }

    }


}
