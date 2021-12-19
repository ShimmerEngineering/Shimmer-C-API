using System;
using System.Globalization;
using System.IO;

namespace shimmer.Models
{
    public class TimePayload : BasePayload
    {
        public readonly double ClockFreqHz = 32768;
        public int Minutes { get; set; }
        public int Ticks { get; set; }

        /// <summary>
        /// This is to be added to the minutes for more accuracy (e.g. convert minutes to seconds and then add this value)
        /// </summary>
        public double RemainingSeconds { get; set; }
        public new bool ProcessPayload(byte[] response)
        {
            try
            {
                Payload = BitConverter.ToString(response);

                var stream = new MemoryStream(response);

                var reader = new BinaryReader(stream);

                Header = BitConverter.ToString(reader.ReadBytes(1));

                var lenthBytes = reader.ReadBytes(2);
                Array.Reverse(lenthBytes);
                Length = BitConverter.ToInt16(lenthBytes, 0);

                var minuteBytes = reader.ReadBytes(4);
                Array.Reverse(minuteBytes);
                Minutes = int.Parse(BitConverter.ToString(minuteBytes).Replace("-", string.Empty), NumberStyles.HexNumber);

                var tickBytes = reader.ReadBytes(3);
                Array.Reverse(tickBytes);
                Ticks = int.Parse(BitConverter.ToString(tickBytes, 0).Replace("-", string.Empty), NumberStyles.HexNumber);
                RemainingSeconds = Ticks / ClockFreqHz;
                reader.Close();
                stream = null;

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
