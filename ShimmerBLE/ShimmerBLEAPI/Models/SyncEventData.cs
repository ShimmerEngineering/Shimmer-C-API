using System;
using System.Collections.Generic;
using System.Text;

namespace shimmer.Models
{
    public class SyncEventData
    {
        public string ASMID { get; set; }
        public SyncEvent CurrentEvent { get; set; }
        public string SyncProgress { get; set; }

    }

    public enum SyncEvent
    {
        Started =0,
        Connected = 10,
        PendingEvents = 20,
        DeviceStatus = 30,
        ClockSync = 40,
        DataSync = 50,
        OperationalConfig = 60,
        FirmwareUpdate = 70,
        PostFirmwareUpdate = 80,
        CloudSync = 90,
        Stopped = 100,
        FinishCloudSync = 110
    }
}
