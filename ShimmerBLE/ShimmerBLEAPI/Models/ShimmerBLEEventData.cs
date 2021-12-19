using System;
using System.Collections.Generic;
using System.Text;

namespace shimmer.Models
{
    public class ShimmerBLEEventData
    {
        public string ASMID { get; set; }
        public VerisenseBLEEvent CurrentEvent { get; set; }
        public string Message { get; set; }
        public Object ObjMsg { get; set; }

        public enum VerisenseBLEEvent
        {
            StateChange = 1,
            SyncLoggedDataNewPayload = 2,
            NewDataPacket = 3,
            SyncLoggedDataComplete = 4,
            RequestResponse = 5,
            WriteResponse = 6,
            RequestResponseFail  = 7,
            DataStreamCRCFail = 8
        }
    }

}
