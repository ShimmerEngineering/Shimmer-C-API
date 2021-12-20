using shimmer.Communications;
using ShimmerBLEAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShimmerBLEAPI.Services
{
    public interface IBroadcastReceiverService
    {
        void SetBroadcastReceiverDevice(VerisenseBLEScannedDevice Device, EventHandler<BLEManagerEvent> BLEManagerEvent);
    }
}
