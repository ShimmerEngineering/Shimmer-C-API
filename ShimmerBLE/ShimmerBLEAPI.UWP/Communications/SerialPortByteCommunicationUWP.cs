using shimmer.Communications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;

namespace ShimmerBLEAPI.UWP.Communications
{
    public class SerialPortByteCommunicationUWP : IVerisenseByteCommunication
    {
        public Guid Asm_uuid { get; set; }
        public string id { get; set; }

        public event EventHandler<ByteLevelCommunicationEvent> CommunicationEvent;
        public String ComPort { get; set; }
        public SerialDevice serialDevice;
        DataWriter dataWriterObject = null;
        DataReader dataReaderObject = null;
        public async Task<ConnectivityState> Connect()
        {
            //string selector = SerialDevice.GetDeviceSelector("COM14");
            //DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(selector);
            //if (devices.Count > 0)
            //{
            //}
            //DeviceInformation deviceInfo = devices[0];
            //serialDevice = await SerialDevice.FromIdAsync(deviceInfo.Id);
            serialDevice = await SerialDevice.FromIdAsync(ComPort);
            if (serialDevice != null)
            {
                serialDevice.BaudRate = 115200;
                serialDevice.DataBits = 8;
                serialDevice.StopBits = SerialStopBitCount.One;
                serialDevice.Parity = SerialParity.None;
                serialDevice.ReadTimeout = TimeSpan.FromMilliseconds(1000);
                serialDevice.WriteTimeout = TimeSpan.FromMilliseconds(1000);
                serialDevice.IsRequestToSendEnabled = true;
                serialDevice.IsDataTerminalReadyEnabled = true;
                //serialDevice.PortName = ComPort;
                //SerialPort.DataReceived += SerialPort_DataReceived;
                serialDevice.ErrorReceived += SerialPort_ErrorReceived;
                //var device = new NmeaParser.SerialPortDevice(serialDevice);
                //device.MessageReceived += device_NmeaMessageReceived;
                Listen();

                return ConnectivityState.Connected;
            }
            return ConnectivityState.Disconnected;
        }

        private async void Listen()
        {
            try
            {
                if (serialDevice != null)
                {
                    dataReaderObject = new DataReader(serialDevice.InputStream);
                    while (true)
                    {
                        await ReadData();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "TaskCanceledException")
                {

                }
            }
            finally
            {
                if (dataReaderObject != null)
                {
                    dataReaderObject.DetachStream();
                    dataReaderObject = null;
                }
            }
        }

        private async Task ReadData()
        {
            Task<UInt32> loadAsyncTask;
            uint ReadBufferLength = 1024;
            dataReaderObject.InputStreamOptions = InputStreamOptions.Partial;
            loadAsyncTask = dataReaderObject.LoadAsync(ReadBufferLength).AsTask();
            UInt32 bytesRead = await loadAsyncTask;
            
            if (bytesRead > 0)
            {
                if (bytesRead == 204)
                {
                    bytesRead = 3;
                    byte[] buffer2 = new byte[bytesRead];
                    dataReaderObject.ReadBytes(buffer2);
                    Debug.WriteLine("New RX Serial Port: " + string.Join(", ", buffer2));
                    if (CommunicationEvent != null)
                    {
                        CommunicationEvent.Invoke(null, new ByteLevelCommunicationEvent { Bytes = buffer2, Event = shimmer.Communications.ByteLevelCommunicationEvent.CommEvent.NewBytes });
                    }
                    bytesRead = 201;
                }
                byte[] buffer = new byte[bytesRead];
                dataReaderObject.ReadBytes(buffer);
                Debug.WriteLine("New RX Serial Port: " + string.Join(", ", buffer));
                if (CommunicationEvent != null)
                {
                    CommunicationEvent.Invoke(null, new ByteLevelCommunicationEvent { Bytes = buffer, Event = shimmer.Communications.ByteLevelCommunicationEvent.CommEvent.NewBytes });
                }
            }
        }

        private void SerialPort_ErrorReceived(object sender, ErrorReceivedEventArgs e)
        {
            Console.WriteLine(e);
        }

        public async Task<ConnectivityState> Disconnect()
        {
            if (serialDevice != null)
            {
                serialDevice.Dispose();
            }
            serialDevice = null;
            return ConnectivityState.Disconnected;
        }

        public ConnectivityState GetConnectivityState()
        {
            return ConnectivityState.Connected;
        }

        public async Task<bool> WriteBytes(byte[] bytes)
        {
            dataWriterObject = new DataWriter(serialDevice.OutputStream);
            try
            {
                Task<UInt32> storeAsyncTask;
                dataWriterObject.WriteBytes(bytes);
                storeAsyncTask = dataWriterObject.StoreAsync().AsTask();
                UInt32 bytesWritten = await storeAsyncTask;
                if (bytesWritten > 0)
                {
                    Console.WriteLine(string.Join(" ", bytes));
                }
                dataWriterObject.DetachStream();
                dataWriterObject = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return true;
        }
    }
}
