using System;
using System.Collections.Concurrent;
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

        public static byte[] DequeueBytes(ConcurrentQueue<byte> queue, int count)
        {
            byte[] result = new byte[count];
            for (int i = 0; i < count; i++)
            {
                if (queue.TryDequeue(out byte dequeuedByte))
                {
                    result[i] = dequeuedByte;
                }
                else
                {
                    // Queue is empty before dequeuing the desired count of bytes
                    // You can handle this case based on your requirements
                    Array.Resize(ref result, i);
                    break;
                }
            }
            return result;
        }


        public static byte[] RemoveBytesFromArray(byte[] originalArray, int bytesToRemove)
        {
            if (bytesToRemove >= originalArray.Length)
            {
                // If the number of bytes to remove is greater than or equal to the array length,
                // return an empty byte array.
                return new byte[0];
            }

            byte[] newArray = new byte[originalArray.Length - bytesToRemove];
            Array.Copy(originalArray, bytesToRemove, newArray, 0, newArray.Length);

            return newArray;
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

        public static byte[] CopyAndRemoveBytes(ref byte[] sourceArray, int bytesToCopy)
        {
            byte[] destinationArray = new byte[bytesToCopy];
            Array.Copy(sourceArray, 0, destinationArray, 0, bytesToCopy);

            // Remove copied bytes from the source array by shifting the remaining elements
            Array.Copy(sourceArray, bytesToCopy, sourceArray, 0, sourceArray.Length - bytesToCopy);
            Array.Resize(ref sourceArray, sourceArray.Length - bytesToCopy);

            return destinationArray;
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

        /**
	 * Converts the raw packet byte values, into the corresponding calibrated and uncalibrated sensor values, the Instruction String determines the output 
	 * @param newPacket a byte array containing the current received packet
	 * @param Instructions an array string containing the commands to execute. It is currently not fully supported
	 * @return
	 */

        public static long[] ParseData(byte[] data, String[] dataType)
        {
            int iData = 0;
            long[] formattedData = new long[dataType.Length];

            for (int i = 0; i < dataType.Length; i++)
                if (dataType[i] == "u8")
                {
                    formattedData[i] = (int)data[iData];
                    iData = iData + 1;
                }
                else if (dataType[i] == "i8")
                {
                    formattedData[i] = Calculatetwoscomplement((int)((int)0xFF & data[iData]), 8);
                    iData = iData + 1;
                }
                else if (dataType[i] == "u12")
                {
                    formattedData[i] = (int)((int)(data[iData] & 0xFF) + ((int)(data[iData + 1] & 0xFF) << 8));
                    iData = iData + 2;
                }
                else if (dataType[i] == "i12>")
                {
                    formattedData[i] = Calculatetwoscomplement((int)((int)(data[iData] & 0xFF) + ((int)(data[iData + 1] & 0xFF) << 8)), 16);
                    formattedData[i] = formattedData[i] >> 4; // shift right by 4 bits
                    iData = iData + 2;
                }
                else if (dataType[i] == "u16")
                {
                    formattedData[i] = (int)((int)(data[iData] & 0xFF) + ((int)(data[iData + 1] & 0xFF) << 8));
                    iData = iData + 2;
                }
                else if (dataType[i] == "u16r")
                {
                    formattedData[i] = (int)((int)(data[iData + 1] & 0xFF) + ((int)(data[iData + 0] & 0xFF) << 8));
                    iData = iData + 2;
                }
                else if (dataType[i] == "i16")
                {
                    formattedData[i] = Calculatetwoscomplement((int)((int)(data[iData] & 0xFF) + ((int)(data[iData + 1] & 0xFF) << 8)), 16);
                    //formattedData[i]=ByteBuffer.wrap(arrayb).order(ByteOrder.LITTLE_ENDIAN).getShort();
                    iData = iData + 2;
                }
                else if (dataType[i] == "i16*")
                {
                    formattedData[i] = Calculatetwoscomplement((int)((int)(data[iData + 1] & 0xFF) + ((int)(data[iData] & 0xFF) << 8)), 16);
                    //formattedData[i]=ByteBuffer.wrap(arrayb).order(ByteOrder.LITTLE_ENDIAN).getShort();
                    iData = iData + 2;
                }
                else if (dataType[i] == "i16r")
                {
                    formattedData[i] = Calculatetwoscomplement((int)((int)(data[iData + 1] & 0xFF) + ((int)(data[iData] & 0xFF) << 8)), 16);
                    //formattedData[i]=ByteBuffer.wrap(arrayb).order(ByteOrder.LITTLE_ENDIAN).getShort();
                    iData = iData + 2;
                }
                else if (dataType[i] == "u24")
                {
                    long xmsb = ((long)(data[iData + 2] & 0xFF) << 16);
                    long msb = ((long)(data[iData + 1] & 0xFF) << 8);
                    long lsb = ((long)(data[iData + 0] & 0xFF));
                    formattedData[i] = xmsb + msb + lsb;
                    iData = iData + 3;
                }
                else if (dataType[i] == "u24r")
                {
                    long xmsb = ((long)(data[iData + 0] & 0xFF) << 16);
                    long msb = ((long)(data[iData + 1] & 0xFF) << 8);
                    long lsb = ((long)(data[iData + 2] & 0xFF));
                    formattedData[i] = xmsb + msb + lsb;
                    iData = iData + 3;
                }
                else if (dataType[i] == "i24r")
                {
                    long xmsb = ((long)(data[iData + 0] & 0xFF) << 16);
                    long msb = ((long)(data[iData + 1] & 0xFF) << 8);
                    long lsb = ((long)(data[iData + 2] & 0xFF));
                    formattedData[i] = xmsb + msb + lsb;
                    formattedData[i] = Calculatetwoscomplement((int)formattedData[i], 24);
                    iData = iData + 3;
                }
            return formattedData;
        }

        public static int Calculatetwoscomplement(int signedData, int bitLength)
        {
            int newData = signedData;
            if (signedData >= (1 << (bitLength - 1)))
            {
                newData = -((signedData ^ (int)(Math.Pow(2, bitLength) - 1)) + 1);
            }

            return newData;
        }


    }
}
