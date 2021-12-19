using System;
using System.Globalization;
using System.IO;
using static ShimmerBLEAPI.Devices.VerisenseDevice;

namespace shimmer.Models
{
    public class ProdConfigPayload : BasePayload
    {
        // ASM Props
        public string ConfigHeader { get; set; }
        public string ASMID { get; set; }
        public HardwareIdentifier HardwareIdentifier { get; set; }
        public int REV_HW_MAJOR { get; set; }
        public int REV_HW_MINOR { get; set; }
        public int REV_FW_MAJOR { get; set; }
        public int REV_FW_MINOR { get; set; }

        public int REV_FW_INTERNAL { get; set; }

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

                ConfigHeader = BitConverter.ToString(reader.ReadBytes(1));

                var idBytes = reader.ReadBytes(6);
                Array.Reverse(idBytes);
                ASMID = BitConverter.ToString(idBytes).Replace("-", string.Empty);

                REV_HW_MAJOR = int.Parse(BitConverter.ToString(reader.ReadBytes(1)), NumberStyles.HexNumber);
                
                HardwareIdentifier = (HardwareIdentifier)REV_HW_MAJOR;

                REV_HW_MINOR = int.Parse(BitConverter.ToString(reader.ReadBytes(1)), NumberStyles.HexNumber);

                REV_FW_MAJOR = int.Parse(BitConverter.ToString(reader.ReadBytes(1)), NumberStyles.HexNumber);

                REV_FW_MINOR = int.Parse(BitConverter.ToString(reader.ReadBytes(1)), NumberStyles.HexNumber);

                byte[] fwInternalArray = reader.ReadBytes(2);
                REV_FW_INTERNAL = BitConverter.ToUInt16(fwInternalArray, 0);

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
