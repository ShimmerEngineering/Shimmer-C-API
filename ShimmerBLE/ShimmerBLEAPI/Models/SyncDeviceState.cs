using System;
using System.Collections.Generic;
using System.Text;

namespace shimmer.Models
{
    public class SyncDeviceState
    {
        public string ASMID { get; set; }
        public string CurrentOperationDescriptionASMSync { get; set; }
        public double CurrentFillPercentASMSync { get; set; }
    }
}
