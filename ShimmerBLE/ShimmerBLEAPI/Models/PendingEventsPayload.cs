using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;

namespace shimmer.Models
{
    public enum PendingEvent
    {
        Data, Status, Time, Config, OperationalConfigWriteOnUnpairing
    }

    /// <summary>
    /// This class store and parse the pending events payload
    /// </summary>
    public class PendingEventsPayload : BasePayload
    {
        public Queue<PendingEvent> Events { get; set; }
        public string PendingEvents; //string representation of the events, only used for notification msgs

        public bool TimeEvent { get; set; }
        public bool StatusEvent { get; set; }
        public bool DataEvent { get; set; }
        public bool ConfigEvent { get; set; }

        /// <summary>
        /// Parse and store the payload data
        /// </summary>
        /// <param name="response">pending events payload</param>
        public new bool ProcessPayload(byte[] response)
        {
            PendingEvents = "";
            try
            {
                Payload = BitConverter.ToString(response);

                var stream = new MemoryStream(response);

                var reader = new BinaryReader(stream);

                Header = BitConverter.ToString(reader.ReadBytes(1));

                var lenthBytes = reader.ReadBytes(2);
                Array.Reverse(lenthBytes);
                Length = int.Parse(BitConverter.ToString(lenthBytes).Replace("-", string.Empty), NumberStyles.HexNumber);

                Events = new Queue<PendingEvent>();

                if (Length > 0)
                {
                    for (int i = 0; i < Length; i++)
                    {
                        var eventData = reader.ReadByte();

                        switch (eventData)
                        {
                            case 0x01:
                                Events.Enqueue(PendingEvent.Status);
                                PendingEvents = PendingEvents + PendingEvent.Status.ToString() + "," ;
                                StatusEvent = true;
                                break;
                            case 0x02:
                                Events.Enqueue(PendingEvent.Data);
                                PendingEvents = PendingEvents + PendingEvent.Data.ToString() + ",";
                                DataEvent = true;
                                break;
                            case 0x04:
                                Events.Enqueue(PendingEvent.Config);
                                PendingEvents = PendingEvents + PendingEvent.Config.ToString() + ",";
                                ConfigEvent = true;
                                break;
                            case 0x05:
                                Events.Enqueue(PendingEvent.Time);
                                PendingEvents = PendingEvents + PendingEvent.Time.ToString() + ",";
                                TimeEvent = true;
                                break;
                            default:
                                break;
                        }
                    }
                    PendingEvents = PendingEvents.Remove(PendingEvents.Length - 1, 1); //remove the last comma
                }
                reader.Close();
                stream = null;

                // DEBUG values

                //Events.Enqueue(PendingEvent.Status);                
                //Events.Enqueue(PendingEvent.Data);
                //Events.Enqueue(PendingEvent.Time);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
