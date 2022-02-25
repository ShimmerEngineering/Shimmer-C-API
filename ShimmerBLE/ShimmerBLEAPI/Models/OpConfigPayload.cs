using System;
using System.Globalization;
using System.IO;

namespace shimmer.Models
{
    /// <summary>
    /// This class store and parse the operational configuration payload
    /// </summary>
    public class OpConfigPayload : BasePayload
    {
        public string ConfigBody { get; set; }
        public byte[] ConfigurationBytes;

        public enum ConfigurationBytesIndexName
        {
            GEN_CFG_0 = 1,
            GEN_CFG_1 = 2,
            GEN_CFG_2 = 3,
            ACCEL1_CFG_0 = 5,
            ACCEL1_CFG_1 = 6,
            ACCEL1_CFG_2 = 7,
            ACCEL1_CFG_3 = 8,
            GYRO_ACCEL2_CFG_0 = 10,
            GYRO_ACCEL2_CFG_1 = 11,
            GYRO_ACCEL2_CFG_2 = 12,
            GYRO_ACCEL2_CFG_3 = 13,
            GYRO_ACCEL2_CFG_4 = 14,
            GYRO_ACCEL2_CFG_5 = 15,
            GYRO_ACCEL2_CFG_6 = 16,
            GYRO_ACCEL2_CFG_7 = 17,
            START_TIME = 21,
            END_TIME = 25,
            BLE_RETRY_COUNT = 30,
            BLE_TX_POWER = 31,
            BLE_DATA_TRANS_WKUP_INT_HRS = 32,
            BLE_DATA_TRANS_WKUP_TIME = 33,
            BLE_DATA_TRANS_WKUP_DUR = 35,
            BLE_DATA_TRANS_RETRY_INT = 36,
            BLE_STATUS_WKUP_INT_HRS = 38,
            BLE_STATUS_WKUP_TIME = 39,
            BLE_STATUS_WKUP_DUR = 41,
            BLE_STATUS_RETRY_INT = 42,
            BLE_RTC_SYNC_WKUP_INT_HRS = 44,
            BLE_RTC_SYNC_WKUP_TIME = 45,
            BLE_RTC_SYNC_WKUP_DUR = 47,
            BLE_RTC_SYNC_RETRY_INT = 48,

            ADC_CHANNEL_SETTINGS_0 = 50,
            ADC_CHANNEL_SETTINGS_1 = 51,
            ADAPTIVE_SCHEDULER_INT = 52,
            ADAPTIVE_SCHEDULER_FAILCOUNT_MAX = 54,
            PPG_REC_DUR_SECS_LSB = 55,
            PPG_REC_DUR_SECS_MSB = 56,
            PPG_REC_INT_MINS_LSB = 57,
            PPG_REC_INT_MINS_MSB = 58,
            PPG_FIFO_CONFIG = 59,
            PPG_MODE_CONFIG2 = 60,
            PPG_MA_DEFAULT = 61,
            PPG_MA_MAX_RED_IR = 62,
            PPG_MA_MAX_GREEN_BLUE = 63,
            PPG_AGC_TARGET_PERCENT_OF_RANGE = 64,
            PPG_MA_LED_PILOT = 66,
            PPG_DAC1_CROSSTALK = 67,
            PPG_DAC2_CROSSTALK = 68,
            PPG_DAC3_CROSSTALK = 69,
            PPG_DAC4_CROSSTALK = 70,
            PROX_AGC_MODE = 71
        }
        /// <summary>
        /// Parse the payload into header, length and configuration bytes
        /// </summary>
        /// <param name="response">op config payload</param>
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
    }
}
