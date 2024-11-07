using System;
using System.Collections.Generic;
using System.Text;

namespace ShimmerAPI.Radios
{
    public class AbstractRadioJSSC : AbstractRadio
    {

        public System.IO.Ports.SerialPort mSerialPort = new System.IO.Ports.SerialPort();

        public AbstractRadioJSSC(String ComPort, int ReadTimeout, int WriteTimeout) 
        {
            mSerialPort.BaudRate = 115200;
            mSerialPort.PortName = ComPort;
            mSerialPort.ReadTimeout = ReadTimeout;
            mSerialPort.WriteTimeout = WriteTimeout;
        }
        public override bool Connect()
        {
            throw new NotImplementedException();
        }

        public override bool Disconnect()
        {
            throw new NotImplementedException();
        }

        public override bool WriteBytes(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            mSerialPort.Open();
        }

        public void Close()
        {
            mSerialPort.Close();
        }

        public void DiscardInBuffer()
        {
            mSerialPort.DiscardInBuffer();
        }

        public void DiscardOutBuffer()
        {
            mSerialPort.DiscardOutBuffer();
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            mSerialPort.Write(buffer, offset, count);
        }

        public int BytesToRead()
        {
            return mSerialPort.ReadByte();
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            return mSerialPort.Read(buffer, offset, count);
        }
    }
}
