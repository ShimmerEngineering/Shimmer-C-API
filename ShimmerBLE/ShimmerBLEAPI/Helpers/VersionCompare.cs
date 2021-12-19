using shimmer.Models;
using shimmer.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace shimmer.Helpers
{
    public class VersionCompare
    {
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

        public static bool compareVersionsEqual(int compMajor, int compMinor, int compInternal, int FirmwareMajor, int FirmwareMinor, int FirmwareInternal)
        {

            if ((compMajor == FirmwareMajor)
                && (compMinor ==FirmwareMinor)
                && (compInternal == FirmwareInternal))
            {
                return true; // if FW ID is the same and version is greater or equal 
            }
            return false; // if less or not the same FW ID
        }
    }
}
