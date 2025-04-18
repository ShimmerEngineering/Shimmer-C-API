﻿using ShimmerAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace ShimmerAPI.Radios
{

    public class SerialPortRadio : AbstractRadio
    {
        public System.IO.Ports.SerialPort SerialPort = new System.IO.Ports.SerialPort();
        protected String ComPort;
        public int ReadTimeout = 1000; //ms
        public int WriteTimeout = 1000; //ms
        protected bool ReadDataThread = false;
        public SerialPortRadio(String comPort)
        {
            ComPort = comPort;
        }

        public override bool Connect()
        {
            CurrentRadioStatus = RadioStatus.Connecting;
            RadioStatusChanged?.Invoke(this, CurrentRadioStatus);
            SerialPort.BaudRate = 115200;
            SerialPort.PortName = ComPort;
            SerialPort.ReadTimeout = this.ReadTimeout;
            SerialPort.WriteTimeout = this.WriteTimeout;
            try
            {
                SerialPort.Open();
            }
            catch (Exception ex)
            {
                CurrentRadioStatus = RadioStatus.Disconnected;
                RadioStatusChanged?.Invoke(this, CurrentRadioStatus);
                return false;
            }
            SerialPort.DiscardInBuffer();
            SerialPort.DiscardOutBuffer();
            ReadDataThread = true;
            Thread thread = new Thread(ReadData);
            // Start the thread
            thread.Start();
            CurrentRadioStatus = RadioStatus.Connected;
            RadioStatusChanged?.Invoke(this, CurrentRadioStatus);
            return true;
        }

        public override bool Disconnect()
        {
            ReadDataThread = false;
            try
            {
                SerialPort.Close();
            }
            catch
            {
                return false;
            }
            CurrentRadioStatus = RadioStatus.Disconnected;
            RadioStatusChanged?.Invoke(this, CurrentRadioStatus);
            return true;
        }

        public override bool WriteBytes(byte[] bytes)
        {
            try
            {
                SerialPort.Write(bytes, 0, bytes.Length);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        protected void ReadData()
        {
            while(ReadDataThread)
            {
                int NumberofBytesToRead = SerialPort.BytesToRead;
                if (NumberofBytesToRead > 0)
                {
                    byte[] buffer = new byte[NumberofBytesToRead];
                    SerialPort.Read(buffer, 0, NumberofBytesToRead);
                    SendBytesReceived(buffer);
                    //Thread.Sleep(1); // Simulate some work
                }
            }
        }
    }
}