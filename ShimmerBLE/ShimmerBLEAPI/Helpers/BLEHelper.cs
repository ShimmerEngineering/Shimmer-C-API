using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace shimmer.Helpers
{
    /// <summary>
    /// This class contains BLE related methods
    /// </summary>
    public class BLEHelper
    {
        /// <summary>
        /// Convert Guid eg 00000000-0000-0000-0000-19092205A2BB to bluetooth address
        /// </summary>
        /// <param name="deviceId">eg 00000000-0000-0000-0000-19092205A2BB</param>
        /// <returns>Returns bluetooth address as ulong</returns>
        public static ulong ToBluetoothAddress(Guid deviceId)
        {
            var address = deviceId
                .ToByteArray()
                .Skip(10)
                .Take(6)
                .ToArray();

            var hexAddress = BitConverter.ToString(address).Replace("-", "");
            if (ulong.TryParse(hexAddress, NumberStyles.HexNumber, null, out ulong mac))
                return mac;

            return 0L;
        }
        static readonly Regex macRegex = new Regex("(.{2})(.{2})(.{2})(.{2})(.{2})(.{2})");
        const string REGEX_REPLACE = "$1:$2:$3:$4:$5:$6";

        /// <summary>
        /// Convert ulong to mac address eg E7:A1:F7:84:2F:17
        /// </summary>
        /// <param name="address">bluetooth address in ulong</param>
        /// <returns>mac address</returns>
        public static string ToMacAddress(ulong address)
        {
            var tempMac = address.ToString("X");
            //tempMac is now 'E7A1F7842F17'

            //string.Join(":", BitConverter.GetBytes(BluetoothAddress).Reverse().Select(b => b.ToString("X2"))).Substring(6);
            var leadingZeros = new string('0', 12 - tempMac.Length);
            tempMac = leadingZeros + tempMac;

            var macAddress = macRegex.Replace(tempMac, REGEX_REPLACE);
            return macAddress;
        }
       
    }
}
