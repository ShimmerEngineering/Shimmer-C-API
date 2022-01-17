using System;
using System.Globalization;
using System.IO;
using System.Text;
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
        public int REV_HW_INTERNAL { get; set; }

        public string PasskeyID { get; set; }
        public string Passkey { get; set; }
        public string AdvertisingNamePrefix { get; set; }

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

                if (Length >= 14)
                {
                    byte[] hwInternalArray = reader.ReadBytes(2);
                    REV_HW_INTERNAL = BitConverter.ToUInt16(hwInternalArray, 0);

                    byte[] passkeyIdArray = reader.ReadBytes(2);
                    if (passkeyIdArray[0] == 0xFF)
                    {
                        PasskeyID = "";
                    }
                    else
                    {
                        PasskeyID = Encoding.Default.GetString(passkeyIdArray);
                    }

                    byte[] passkeyArray = reader.ReadBytes(6);
                    if (passkeyArray[0] == 0xFF || passkeyArray[0] == 0x00)
                    {
                        Passkey = "";
                    }
                    else
                    {
                        Passkey = Encoding.Default.GetString(passkeyArray);
                    }

                    byte[] advertisingNamePrefixArrayOriginal = reader.ReadBytes(32);
                    if (advertisingNamePrefixArrayOriginal[0] == 0xFF)
                    {
                        AdvertisingNamePrefix = "";
                    }
                    else
                    {
                        int endIndex = Array.IndexOf(advertisingNamePrefixArrayOriginal, (byte)0xFF);
                        if (endIndex == -1)
                        {
                            endIndex = 32;
                        }
                        byte[] advertisingNamePrefixArray = new byte[endIndex];
                        Array.Copy(advertisingNamePrefixArrayOriginal, 0, advertisingNamePrefixArray, 0, endIndex);
                        AdvertisingNamePrefix = Encoding.Default.GetString(advertisingNamePrefixArray);
                    }
                }

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
