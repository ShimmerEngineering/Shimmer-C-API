using System;
using System.Globalization;
using System.IO;
using shimmer.Helpers;

namespace shimmer.Models
{
    public interface IBasePayload
    {
        bool ProcessPayload(byte[] response);
    }

    public class BasePayload : IBasePayload
    {
        public string Header { get; set; }
        public int Length { get; set; }
        public string Payload { get; set; }

        public bool ProcessPayload(byte[] response)
        {
            try
            {
                Payload = BitConverter.ToString(response);

                var stream = new MemoryStream(response);

                var reader = new BinaryReader(stream);

                Header = BitConverter.ToString(reader.ReadBytes(1));

                var lenthBytes = reader.ReadBytes(2);
                Array.Reverse(lenthBytes);
                Length = int.Parse(BitConverter.ToString(lenthBytes).Replace("-", string.Empty), NumberStyles.HexNumber);

                reader.Close();
                stream = null;
            }
            catch (System.Exception)
            {
                return false;
            }

            return true;
        }

        public byte[] GetPayload()
        {
            byte[] payloadWithHeader = GetPayloadWithHeader();
            byte[] payloadWithoutHeader = new byte[payloadWithHeader.Length-3];
            Array.Copy(GetPayloadWithHeader(), 3, payloadWithoutHeader, 0, payloadWithoutHeader.Length);
            return payloadWithoutHeader;
        }

        public byte[] GetPayloadWithHeader()
        {
            return BitHelper.MSBByteArray(Payload.Replace("-", "")).ToArray();
        }
    }

    public enum RequestType
    {
        ReadStatus = 11,
        TransferLoggedData = 12,
        DataACK = 42,
        DataNACK = 52,
        ReadProductionConfig = 13,
        ReadOperationalConfig = 14,
        WriteOperationalConfig = 24,
        WriteProductionConfig = 23,
        ReadRTC = 15,
        WriteRTC = 25,
        ReadPendingEvents = 17,
        Disconnect = 43,
        OperationalConfigWriteOnUnpairing = 94,
        DFU = 26,
        StartStreaming = 95,
        StopStreaming = 100,
        EraseData = 29
    }

    public enum BondingStatus
    {
        Bonded =1,
        Bonding=2,
        Cancel=3,
    }
}