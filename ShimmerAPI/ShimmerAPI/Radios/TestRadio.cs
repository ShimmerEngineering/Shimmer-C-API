using ShimmerAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ShimmerAPI.Radios
{
    public class TestRadio : AbstractRadio
    {
        private bool StartThread = false;
        public override bool Connect()
        {
            return true;
        }

        public override bool Disconnect()
        {
            throw new NotImplementedException();
        }

        public override bool WriteBytes(byte[] bytes)
        {
            if (bytes[0] == 0xA4 && bytes[1] == 1)
            {
                StartThread = true;
                Thread thread = new Thread(GenerateBytes);
                // Start the thread
                thread.Start();
            }
            if (bytes[0] == 0xA4 && bytes[1] == 0)
            {
                StartThread = false;
            }
            return true;
        }
        int count = 0;
        byte[] buffer = new byte[] { };
        public void GenerateBytes()
        {
            bool firstTime = true;
            while (StartThread)
            {
                if (firstTime)
                {
                    byte[] startbyte = new byte[] { 0 };
                    BytesReceived?.Invoke(this, startbyte);
                    firstTime = false;
                }
                byte[] bytes = BitConverter.GetBytes(count);
                buffer = ProgrammerUtilities.AppendByteArrays(buffer, bytes);
                if (buffer.Length>5000)
                {
                    BytesReceived?.Invoke(this, buffer);
                    buffer = new byte[] { };
                    Thread.Sleep(1);
                }
                count++;
                
            }
        }

    }
}
