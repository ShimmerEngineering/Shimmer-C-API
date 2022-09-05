using System;
using System.Collections.Generic;
using System.Text;

namespace shimmer.Models
{
    /// <summary>
    /// Store data for log event
    /// </summary>
    public class LogEventData
    {
        public DateTime Timestamp { get; set; }
        public LogEvent CurrentEvent { get; set; }
        public int BattLevel { get; set; }
        public override string ToString()
        {
            if(CurrentEvent == LogEvent.BATTERY_VOLTAGE)
            {
                return "Batt value: " + BattLevel + " Event: " + CurrentEvent;
            }
            else
            {
                return "Timestamp:" + Timestamp.ToString() + " Event: " + CurrentEvent;
            }
        }
    }

    public enum LogEvent
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
}
