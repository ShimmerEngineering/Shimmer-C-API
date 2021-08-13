using System;
using System.Collections.Generic;
using System.Text;

namespace ShimmerAPI.Utilities
{
    public static class ProgrammerUtilities
    {
        //Note there is a method Calculatetwoscomplement in ShimmerBluetooth class that would be more suited to be in this class as well
        public static int ByteArrayToInt(byte[] data, bool lsbOrder, bool isSigned)
        {
            var number = 0;
            int i = 0;
            foreach (byte b in data)
            {
                if (lsbOrder)
                {
                    number += (b << i * 8);
                } else
                {
                    number = (number << 8) + b;
                }
                i++;
            }

            if (isSigned)
            {
                var bitLength = data.Length * 8;
                if (number >= (1 << (bitLength - 1)))
                {
                    number = -((number ^ (int)(Math.Pow(2, bitLength) - 1)) + 1);
                }
            }
            return number;
        }
    }
}
