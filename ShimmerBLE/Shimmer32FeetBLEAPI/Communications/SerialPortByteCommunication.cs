using shimmer.Communications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;

namespace shimmer.Communications
{
    public class SerialPortByteCommunication : IVerisenseByteCommunication
    {
        public Guid Asm_uuid { get; set; }

        public event EventHandler<ByteLevelCommunicationEvent> CommunicationEvent;
        public String ComPort { get; set; }
        public System.IO.Ports.SerialPort SerialPort = new System.IO.Ports.SerialPort();
        public async Task<ConnectivityState> Connect()
        {
            SerialPort.BaudRate = 115200;
            SerialPort.PortName = ComPort;
            SerialPort.Parity = Parity.None;
            SerialPort.DataBits = 8;
            SerialPort.StopBits = StopBits.One;
            SerialPort.DataReceived += SerialPort_DataReceived;
            SerialPort.ErrorReceived += SerialPort_ErrorReceived;
            SerialPort.WriteTimeout = 10000;
            SerialPort.ReadTimeout = 10000;
            SerialPort.RtsEnable = true;
            SerialPort.DtrEnable = true;
            
            try
            {
                SerialPort.Open();
                
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
            }
            Debug.WriteLine(SerialPort.IsOpen);
            SerialPort.DiscardInBuffer();
            SerialPort.DiscardOutBuffer();         

            return ConnectivityState.Connected;
        }

        private void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            System.Console.WriteLine(e);
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (CommunicationEvent != null)
            {
                
                SerialPort sp = (SerialPort)sender;
                int NumberofBytesToRead = SerialPort.BytesToRead;
                if (NumberofBytesToRead > 0)
                {
                    Debug.WriteLine("New RX Serial Port: " + NumberofBytesToRead);
                    byte[] buffer = new byte[NumberofBytesToRead];
                    SerialPort.Read(buffer, 0, NumberofBytesToRead);
                    CommunicationEvent.Invoke(null, new ByteLevelCommunicationEvent { Bytes = buffer, Event = Communications.ByteLevelCommunicationEvent.CommEvent.NewBytes });
                }
            }
        }

        public async Task<ConnectivityState> Disconnect()
        {
            SerialPort.Close();
            return ConnectivityState.Disconnected;
        }

        public ConnectivityState GetConnectivityState()
        {
            return ConnectivityState.Connected;
        }

        public async Task<bool> WriteBytes(byte[] bytes)
        {
            bool success = false;
            try
            {
                SerialPort.Write(bytes, 0, bytes.Length);
                success = true;
            }
            catch (Exception e )
            {
                System.Console.WriteLine(e);
                return success;
            }
            return success;
        }
    }
}
