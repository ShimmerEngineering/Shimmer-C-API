using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace shimmer.Helpers
{
    public static class FileHelper
    {
        
        /// <summary>
        /// Expects the format to be "191113_160404_00500.bin" , returns null if not that format
        /// </summary>
        /// <param name="binFileName">"191113_160404_00500.bin"</param>
        /// <returns></returns>
        public static int? GetPayloadFromBinFile(string binFileName)
        {
            string[] nameArray = binFileName.Split('_');
            int? payloadIndex = null;
            foreach (string name in nameArray)
            {
                if (name.Contains(".bin"))
                {
                    string[] subname = name.Split('.');
                    payloadIndex = Int32.Parse(subname[0]);
                }
            }
            return payloadIndex;
        }
    }
}
