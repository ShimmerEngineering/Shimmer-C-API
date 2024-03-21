using System;
using System.Collections.Generic;
using System.Text;

namespace shimmer.Models
{
    /// <summary>
    /// Communication mode
    /// </summary>
    public class CommunicationState
    {   
        public enum CommunicationMode
        {
            ReadAndClearPendingEvents,
            ForceDataTransferSync,
            Unpair,
            Pair
        }
    }
}
