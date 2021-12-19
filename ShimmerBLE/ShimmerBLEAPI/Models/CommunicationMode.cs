using System;
using System.Collections.Generic;
using System.Text;

namespace shimmer.Models
{
    public class CommunicationState
    {   public enum CommunicationMode
        {
            ReadAndClearPendingEvents,
            ForceDataTransferSync,
            Unpair,
            Pair
        }
    }
}
