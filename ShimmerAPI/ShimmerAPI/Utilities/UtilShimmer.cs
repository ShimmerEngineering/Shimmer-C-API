using System;
using System.Collections.Generic;
using System.Text;

namespace ShimmerAPI.Utilities
{
    public class UtilShimmer
    {
		public static class SHIMMER_DEFAULT_COLOURS
		{
			// Shimmer Orange
			public static byte[] colourShimmerOrange = new byte[] { 241, 93, 34 };
			public static byte[] colourBrown = new byte[] { 153, 76, 0 };
			public static byte[] colourCyanAqua = new byte[] { 0, 153, 153 };
			public static byte[] colourPurple = new byte[] { 102, 0, 204 };
			public static byte[] colourMaroon = new byte[] { 102, 0, 0 };
			public static byte[] colourGreen = new byte[] { 0, 153, 76 };
			// Shimmer Grey
			public static byte[] colourShimmerGrey = new byte[] { 119, 120, 124 };
			// Shimmer Blue
			public static byte[] colourShimmerBlue = new byte[] { 0, 129, 198 };

			public static byte[] colourLightRed = new byte[] { 255, 0, 0 };
		}
        public static byte[] HexStringToByteArray(string s)
        {
            int len = s.Length;
            byte[] data = new byte[1];
            if (s.ToCharArray().Length == 0)
            {
                data[0] = 0;
            }
            else if (s.ToCharArray().Length == 1)
            {
                data[0] = (byte)Convert.ToByte(s[0].ToString(), 16);
            }
            else
            {
                data = new byte[len / 2];
                for (int i = 0; i < len; i += 2)
                {
                    data[i / 2] = (byte)((Convert.ToByte(s[i].ToString(), 16) << 4) +
                                         Convert.ToByte(s[i + 1].ToString(), 16));
                }
            }
            return data;
        }

        protected static readonly char[] HexArray = "0123456789ABCDEF".ToCharArray();
        public static String BytesToHexString(byte[] bytes)
        {
            if (bytes != null)
            {
                char[] hexChars = new char[bytes.Length * 2];
                for (int j = 0; j < bytes.Length; j++)
                {
                    int v = bytes[j] & 0xFF;
                    hexChars[j * 2] = HexArray[v >> 4];
                    hexChars[j * 2 + 1] = HexArray[v & 0x0F];
                }
                return new String(hexChars);
            }
            else
            {
                return null;
            }
        }
    }
}
