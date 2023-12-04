using System;
using System.Collections.Generic;
using System.Text;

namespace ShimmerAPI.Utilities
{
    public static class ProgrammerUtilities
    {
        public static string ByteArrayToHexString(byte[] byteArray)
        {
            StringBuilder hex = new StringBuilder(byteArray.Length * 2);
            foreach (byte b in byteArray)
            {
                hex.AppendFormat("{0:X2}", b);
            }
            return hex.ToString();
        }

        public static byte[] RemoveLastBytes(byte[] byteArray, int x)
        {
            if (x > byteArray.Length)
            {
                // If x is greater than or equal to the length of the array,
                // return an empty byte array or handle it as needed in your scenario.
                return null;
            }

            int newLength = byteArray.Length - x;
            byte[] result = new byte[newLength];

            Array.Copy(byteArray, 0, result, 0, newLength);

            return result;
        }

        public static byte[] AppendByteArrays(byte[] array1, byte[] array2)
        {
            if (array1 == null || array1.Length == 0)
            {
                // If the first array is null or empty, return the second array
                return array2;
            }
            else if (array2 == null || array2.Length == 0)
            {
                // If the second array is null or empty, return the first array
                return array1;
            }

            byte[] result = new byte[array1.Length + array2.Length];
            Buffer.BlockCopy(array1, 0, result, 0, array1.Length);
            Buffer.BlockCopy(array2, 0, result, array1.Length, array2.Length);

            return result;
        }

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
