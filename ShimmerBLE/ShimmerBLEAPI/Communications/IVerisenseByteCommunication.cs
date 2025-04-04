using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace shimmer.Communications
{
    public interface IVerisenseByteCommunication
    {
        Guid Asm_uuid { get; set; }
        Task<ConnectivityState> Connect();
        Task<ConnectivityState> Disconnect();

        event EventHandler<ByteLevelCommunicationEvent> CommunicationEvent;
        ConnectivityState GetConnectivityState();
        Task<bool> WriteBytes(byte[] bytes);
    }

    public enum ConnectivityState
    {
        Unknown,
        Connected,
        Disconnected,
        Connecting,
        Limited
    }

    /// <summary>
    /// Store the event data
    /// </summary>
    public class ByteLevelCommunicationEvent
    {
        public byte[] Bytes;
        public CommEvent Event;
        public enum CommEvent
        {
            Disconnected = 1,
            NewBytes = 2,
            NewSteps = 3
        }
    }
}
