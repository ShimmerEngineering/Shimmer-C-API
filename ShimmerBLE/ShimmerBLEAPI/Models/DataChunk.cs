using System.Collections.Generic;
using System;

namespace shimmer.Models
{
    public class DataChunkNew
    {
        public byte[] Packets { get; set; }
        public int ExpectedLength { get; set; }
        public int CurrentLength { get; set; }

        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public double Transfer { get; set; }
        const int packetMaxSize = 32767; //Considering we are using int16 to get the length the maximum value is 7F FF which is 32767
        public bool CRCErrorPayload = false;
        public DataChunkNew()
        {
            Packets = new byte[packetMaxSize];
            Start = DateTime.Now;
        }

        public DataChunkNew(int memoryPacketSize)
        {
            Packets = new byte[memoryPacketSize];
            Start = DateTime.Now;
        }
    }

    public class DataChunk
    {
        public List<byte[]> Packets { get; set; }
        public int ExpectedLength { get; set; }
        public int CurrentLength { get; set; }
        public int Position { get; set; }
    }
}
