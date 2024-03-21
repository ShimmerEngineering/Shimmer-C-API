using System;
using System.Collections.Generic;
using System.Text;

namespace shimmer.Models
{
    /// <summary>
    /// This class is used to store CRC check result
    /// </summary>
    public class CRCCheckResponse
    {
        public int computed { get; set; }
        public int original { get; set; }
        public bool result { get; set; }
    }
}
