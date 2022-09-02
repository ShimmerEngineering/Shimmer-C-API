using shimmer.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace shimmer.Models
{
    public class LogEventsPayload : BasePayload
    {
        public string ConfigBody { get; set; }
        public byte[] LogEventsBytes;
        public List<LogEventData> LogEvents { get; set; }

        public new bool ProcessPayload(byte[] response)
        {
            try
            {
                Payload = BitConverter.ToString(response);

                var stream = new MemoryStream(response);

                var reader = new BinaryReader(stream);

                Header = BitConverter.ToString(reader.ReadBytes(1));

                var lengthBytes = reader.ReadBytes(2);
                Array.Reverse(lengthBytes);
                Length = int.Parse(BitConverter.ToString(lengthBytes).Replace("-", string.Empty), NumberStyles.HexNumber);
                LogEventsBytes = reader.ReadBytes(response.Length - 3);
                ConfigBody = BitConverter.ToString(LogEventsBytes).Replace("-", string.Empty);

                LogEvents = ParseLogEventsBytes(LogEventsBytes);

                reader.Close();
                stream = null;

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public List<LogEventData> ParseLogEventsBytes(byte[] rawBytes)
        {
            var list = new List<LogEventData>();
            for (int i = 0; i < rawBytes.Length; i += 8)
            {
                byte[] packet = new byte[8];
                Array.Copy(rawBytes, i, packet, 0, 8);
                list.Add(ParseData(packet));
            }
            return list;
        }

        public LogEventData ParseData(byte[] rawData)
        {
            if(rawData.Length != 8)
            {
                throw new Exception("Each log event packet has a length of 8 bytes");
            }

            int eventType = rawData[7];
            if (eventType != 0 && Enum.IsDefined(typeof(LogEvent), eventType))
            {
                LogEventData data = new LogEventData();
                data.CurrentEvent = (LogEvent)eventType;
                if (eventType == (int)LogEvent.BATTERY_VOLTAGE)
                {
                    byte[] battLevelBytes = new byte[3];
                    Array.Copy(rawData, 0, battLevelBytes, 0, 3);
                    Array.Reverse(battLevelBytes);
                    data.BattLevel = int.Parse(BitConverter.ToString(battLevelBytes).Replace("-", string.Empty), NumberStyles.HexNumber);
                }
                else
                {
                    byte[] timestampBytes = new byte[7];
                    Array.Copy(rawData, 0, timestampBytes, 0, 7);
                    int rwc_min = 0;
                    rwc_min += (timestampBytes[0] & 0xFF);
                    rwc_min += ((timestampBytes[1] & 0xFF) << 8);
                    rwc_min += ((timestampBytes[2] & 0xFF) << 16);
                    rwc_min += ((timestampBytes[3] & 0xFF) << 24);

                    double rwc = 0.0;
                    rwc += (timestampBytes[4] & 0xFF);
                    rwc += ((timestampBytes[5] & 0xFF) << 8);
                    rwc += ((timestampBytes[6] & 0xFF) << 16);

                    double rwc_seconds = rwc / 32768;
                    double ts = rwc_min * 60;
                    ts += rwc_seconds;
                    DateTime dt = DateHelper.GetDateTimeFromSeconds(ts);
                    data.Timestamp = dt.ToString();
                }
                return data;
            }
            return null;
        }

        public void WriteRawDataToFile(string path)
        {
            using (var w = new StreamWriter(path, true))
            {
                for (int i = 0; i < LogEventsBytes.Length; i += 8)
                {
                    byte[] logEventBytes = new byte[8];
                    Array.Copy(LogEventsBytes, i, logEventBytes, 0, 8);
                    w.WriteLine(BitConverter.ToString(logEventBytes));
                }
            }
        }

        public void WriteLogEventsToFile(string path)
        {
            using (var w = new StreamWriter(path, true))
            {
                if(LogEvents.Count > 0)
                {
                    for (int i = 0; i < LogEvents.Count; i++)
                    {
                        if(LogEvents[i] != null)
                        {
                            w.WriteLine("Index: " + i + " " + LogEvents[i].ToString());
                        }
                    }
                }
                else
                {
                    w.WriteLine("No log events");
                }
            }
        }
    }
}
