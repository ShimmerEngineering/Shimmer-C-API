using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System;
using shimmer.Models;

namespace shimmer.Helpers
{
    public static class BitHelper
    {
        public readonly static long MaxFourByteUnsignedValue = 4294967295; //2^32 -1
        #region Byte array conversion methods
        // WARNING: 
        // Using Lists here is not accidental, concatenating arrays results in IEnumerable
        // This does not work in obvious ways and would break byte order

        public static List<byte> MSBByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                return null;
            }

            byte[] data = new byte[hexString.Length / 2];
            for (int index = 0; index < data.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                data[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return data.ToList();
        }

        public static List<byte> LSBByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                return null;
            }

            byte[] data = new byte[hexString.Length / 2];
            for (int index = 0; index < data.Length; index++)
            {
                string byteValue = hexString.Substring(hexString.Length - index * 2 - 2, 2);
                data[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return data.ToList();
        }

        static int initialValue = 0xFFFF;   // initial value
        static int polynomial = 0x1021;

        public static CRCCheckResponse CheckCRC(byte[] payload)
        {
            var response = new CRCCheckResponse();

            response.computed = ComputeCRC(payload);
            response.original = GetOriginalCRC(payload);

            response.result = (response.computed == response.original);

            return response;
        }

        static int ComputeCRC(byte[] payload)
        {
            var length = payload.Length;
            int crc = initialValue;          // initial value

            for (int x = 0; x < length - 2; x++)
            {
                byte b = payload[x];
                for (int i = 0; i < 8; i++)
                {
                    bool bit = ((b >> (7 - i) & 1) == 1);
                    bool c15 = ((crc >> 15 & 1) == 1);
                    crc <<= 1;
                    if (c15 ^ bit) crc ^= polynomial;
                }
            }

            crc &= 0xffff;
            return crc;
        }

        static int GetOriginalCRC(byte[] payload)
        {
            try
            {
                byte[] crcOriginalBuf = new byte[4];
                Array.Copy(payload, payload.Length - 2, crcOriginalBuf, 0, 2);
                var crc = BitConverter.ToInt32(crcOriginalBuf, 0);
                return crc;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }

        }

        #endregion
    }
}