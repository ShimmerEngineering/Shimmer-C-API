using shimmer.Models;
using shimmer.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace shimmer.Helpers
{
    /// <summary>
    /// This class contains methods to compare the firmware version
    /// </summary>
    public class VersionCompare
    {
        /// <summary>
        /// Return true if compared version is greater or equal to firmware version
        /// </summary>
        /// <param name="compInternal"></param>
        /// <param name="compMajor"></param>
        /// <param name="compMinor"></param>
        /// <param name="FirmwareInternal"></param>
        /// <param name="FirmwareMajor"></param>
        /// <param name="FirmwareMinor"></param>
        /// <returns></returns>
        public static bool compareVersionsGreaterOrEqual(int compMajor, int compMinor, int compInternal,int FirmwareMajor, int FirmwareMinor, int FirmwareInternal)
        {

            if ((compMajor > FirmwareMajor)
                    || (FirmwareMajor == compMajor && compMinor > FirmwareMinor)
                    || (FirmwareMajor == compMajor && FirmwareMinor == compMinor && compInternal >= FirmwareInternal))
            {
                return true; // if FW ID is the same and version is greater or equal 
            }
            return false; // if less or not the same FW ID
        }

        /// <summary>
        /// Return true if firmware version is equal to compared version
        /// </summary>
        /// <param name="compInternal"></param>
        /// <param name="compMajor"></param>
        /// <param name="compMinor"></param>
        /// <param name="FirmwareInternal"></param>
        /// <param name="FirmwareMajor"></param>
        /// <param name="FirmwareMinor"></param>
        /// <returns></returns>
        public static bool compareVersionsEqual(int compMajor, int compMinor, int compInternal, int FirmwareMajor, int FirmwareMinor, int FirmwareInternal)
        {

            if ((compMajor == FirmwareMajor)
                && (compMinor ==FirmwareMinor)
                && (compInternal == FirmwareInternal))
            {
                return true; // if FW ID is the same and version is equal 
            }
            return false; // if not the same FW ID
        }
    }
}
