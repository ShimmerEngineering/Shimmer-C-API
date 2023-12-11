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

        public override bool Disconnect()
        {
            bluetoothDevice.GattServerDisconnected -= Device_GattServerDisconnected;
            try
            {
                bluetoothDevice.Gatt.Disconnect();
            } catch
            {
                return false;
            }
            return true;
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
            BytesReceived?.Invoke(this, buffer);
        }

    }


}
