using shimmer.Communications;
using ShimmerBLEAPI.Devices;
using System;
using System.Collections.Generic;
using System.Text;

namespace shimmer.Communications
{
    public class VerisenseBLEDeviceWindows : VerisenseBLEDevice
    {
        public VerisenseBLEDeviceWindows(string uuid, string id) : base(uuid, id)
        {

        }

        protected override void StartExecuteRequestTimer()
        {
        }

        protected override void StartDataRequestTimer()
        {
        }

        protected override void InitializeRadio()
        {
            BLERadio = new RadioPlugin32Feet();
        }
    }
}
