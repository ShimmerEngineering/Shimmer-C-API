using shimmer.Helpers;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using static shimmer.Models.CommunicationState;

namespace shimmer.Models
{
    public class LogEventsPayload : BasePayload
    {
        public string ConfigBody { get; set; }
        public byte[] ConfigurationBytes;

        public enum LogEvents
        {
            NONE = 0,
            BATTERY_FALL = 1,
            BATTERY_RECOVER = 2,
            WRITE_TO_FLASH_SUCCESS = 3,
            WRITE_TO_FLASH_FAIL_GENERAL = 4,
            WRITE_TO_FLASH_FULL = 5,
            WRITE_TO_FLASH_FAIL_CHECK_ADDR_FREE = 6,
            WRITE_TO_FLASH_FAIL_LOW_BATT_CHECK_ADDR_FREE = 7,
            WRITE_TO_FLASH_FAIL_LOW_BATT_FLASH_ON = 8,
            WRITE_TO_FLASH_FAIL_LOW_BATT_FLASH_WRITE = 9,
            WRITE_TO_FLASH_FAIL_LOW_BATT_BEFORE_START = 10,
            USB_PLUGGED_IN = 11,
            USB_PLUGGED_OUT = 12,
            RECORDING_PAUSED = 13,
            RECORDING_RESUMED = 14,
            BATTERY_RECOVER_IN_BATT_CHECK_TIMER = 15,
            TSK_FREE_UP_FLASH = 16,
            FREE_UP_FLASH_FAIL_LOW_BATT = 17,
            PAYLOAD_PACKAGING_TASK_SET = 18,
            PAYLOAD_PACKAGING_FUNCTION_CALL = 19,
            BATTERY_VOLTAGE = 20,
            TSK_WRITE_LOOKUP_TBL_CHANGES_TO_EEPROM = 21,
            LPCOMP_ON = 22,
            LPCOMP_ON_ALREADY = 23,
            LPCOMP_OFF = 24,
            LPCOMP_TRIED_BUT_BATT_LOW = 25,
            BLE_CONNECTED = 26,
            BLE_DISCONNECTED = 27,
            TSK_WRITE_FLASH = 28,
            PPG_TIMER_START = 29,
            PAYLOAD_OVERSHOT = 30,
            ADVERTISING_START = 31,
            ADVERTISING_STOP = 32,
        };

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
                ConfigurationBytes = reader.ReadBytes(response.Length - 3);
                ConfigBody = BitConverter.ToString(ConfigurationBytes).Replace("-", string.Empty);

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

        public void WriteParsedDataToFile(byte[] ResponseBuffer, string path)
        {
            byte[] arr = ResponseBuffer.Skip(3).ToArray();
            int eventType = 0;
            bool event_logged = false;
            LogEvents event_string;
            using (var w = new StreamWriter(path, true))
            {
                for (int i = 0; i < arr.Length; i += 8)
                {
                    eventType = arr[i + 7];
                    if (eventType != 0)
                    {
                        event_logged = true;
                        event_string = (LogEvents)eventType;
                        if (Enum.IsDefined(typeof(LogEvents), eventType))
                        {
                            if (eventType == (int)LogEvents.BATTERY_VOLTAGE)
                            {
                                byte[] result = new byte[3];
                                Array.Copy(arr, i, result, 0, 3);
                                Array.Reverse(result);
                                int battLevel = int.Parse(BitConverter.ToString(result).Replace("-", string.Empty), NumberStyles.HexNumber);
                                w.WriteLine("index: " + i / 8 + " batt value:" + battLevel + " event: " + event_string);
                            }
                            else
                            {
                                byte[] result = new byte[7];
                                Array.Copy(arr, i, result, 0, 7);
                                int rwc_min = 0;
                                rwc_min += (result[0] & 0xFF);
                                rwc_min += ((result[1] & 0xFF) << 8);
                                rwc_min += ((result[2] & 0xFF) << 16);
                                rwc_min += ((result[3] & 0xFF) << 24);

                                double rwc = 0.0;
                                rwc += (result[4] & 0xFF);
                                rwc += ((result[5] & 0xFF) << 8);
                                rwc += ((result[6] & 0xFF) << 16);

                                double rwc_seconds = rwc / 32768;
                                double ts = rwc_min * 60;
                                ts += rwc_seconds;
                                DateTime dt = DateHelper.GetDateTimeFromSeconds(ts);
                                w.WriteLine("index: " + i / 8 + " timestamp:" + dt.ToString() + " event: " + event_string);
                            }
                        }
                    }
                }
            }
            if (!event_logged)
            {

            }
        }
    }
}
