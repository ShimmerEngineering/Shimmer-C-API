using shimmer.Helpers;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using static shimmer.Models.CommunicationState;

namespace shimmer.Models
{
    public class StatusPayload : BasePayload
    {
        public string ASMID { get; set; }
        public long StatusTimestamp { get; set; }
        public int BatteryLevel { get; set; }
        public int BatteryPercent { get; set; }
        public long TransferSuccessTimestamp { get; set; }
        public long TransferFailTimestamp { get; set; }
        public long BaseStationTimestamp { get; set; }
        public int FreeStorage { get; set; }
        public bool IsSuccess { get; set; }
        public long? VBattFallCounter { get; set; }
        public long? StatusFlags { get; set; } = null;
        public bool? UsbPowered { get; set; } = null;
        public bool? RecordingPaused { get; set; } = null;
        public bool? FlashIsFull { get; set; } = null;
        public bool? PowerIsGood { get; set; } = null;
        public bool? AdaptiveScheduler { get; set; } = null;
        public bool? DfuServiceOn { get; set; } = null;
        public int SyncMode { get; set; }
        public long? NextSyncAttemptTimestamp { get; set; } = null;
        public int? StorageFull { get; set; } = null;
        public int? StorageToDel { get; set; } = null;
        public int? StorageBad { get; set; } = null;

        private long ConvertMinuteToMS(long timestamp)
        {
            if (timestamp != BitHelper.MaxFourByteUnsignedValue) //special condition where the sensor/fw returns all FF values
            {
                timestamp = timestamp * 60 * 1000; //convert from minutes to milliseconds
            }
            else
            {
                timestamp = -1;
            }
            return timestamp;
        }

        private long ConvertTicksTomS(long ticks)
        {
            return (long)((ticks / (double)App.SensorClockFrequency) * 1000.0);
        }

        private long AppendToCurrentTimestamp(long timestamp, double durationToAppend)
        {
            return (long)(timestamp + durationToAppend);
        }

        private string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public new bool ProcessPayload(byte[] response, CommunicationMode syncMode)
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
                
                var idBytes = reader.ReadBytes(6);
                Array.Reverse(idBytes);
                ASMID = BitConverter.ToString(idBytes).Replace("-", string.Empty);
                
                var tsBytes = reader.ReadBytes(4);
                Array.Reverse(tsBytes);
                StatusTimestamp = long.Parse(BitConverter.ToString(tsBytes).Replace("-", string.Empty), NumberStyles.HexNumber);
                StatusTimestamp = ConvertMinuteToMS(StatusTimestamp);
                var batteryBytes = reader.ReadBytes(2);
                Array.Reverse(batteryBytes);
                BatteryLevel = int.Parse(BitConverter.ToString(batteryBytes).Replace("-", string.Empty), NumberStyles.HexNumber);

                BatteryPercent = int.Parse(BitConverter.ToString(reader.ReadBytes(1)).Replace("-", string.Empty), NumberStyles.HexNumber);

                var successBytes = reader.ReadBytes(4);
                Array.Reverse(successBytes);
                TransferSuccessTimestamp = long.Parse(BitConverter.ToString(successBytes).Replace("-", string.Empty), NumberStyles.HexNumber);
                TransferSuccessTimestamp = ConvertMinuteToMS(TransferSuccessTimestamp);
                var failBytes = reader.ReadBytes(4);
                Array.Reverse(failBytes);
                TransferFailTimestamp = long.Parse(BitConverter.ToString(failBytes).Replace("-", string.Empty), NumberStyles.HexNumber);
                TransferFailTimestamp = ConvertMinuteToMS(TransferFailTimestamp);

                var storageBytes = reader.ReadBytes(3);
                Array.Reverse(storageBytes);
                FreeStorage = int.Parse(BitConverter.ToString(storageBytes).Replace("-", string.Empty), NumberStyles.HexNumber);
                /* I am moving this to the UI level, because this values might be meaningful in the web DB
                if (FreeStorage > App.MaxSensorStorageCapacityKB)
                {
                    FreeStorage = App.MaxSensorStorageCapacityKB;
                }
                */
                if (Length <= 24)    //old fw, no VBattFallCounter bytes 
                {
                    VBattFallCounter = null; //set to null because 0 can be a valid value
                }
                else
                {
                    var battFallBytes = reader.ReadBytes(2);
                    Array.Reverse(battFallBytes);
                    VBattFallCounter = long.Parse(BitConverter.ToString(battFallBytes).Replace("-", string.Empty), NumberStyles.HexNumber);
                }


                if (Length > 26)  //new fw support StatusFlags bytes 
                {   
                   
                    var statusFlagsBytes = reader.ReadBytes(8);
                    Array.Reverse(statusFlagsBytes);
                    //eg 0000000000000009 where 09 is the LSB (byte 26) will result in a StatusFlags value of 9
                    StatusFlags = long.Parse(BitConverter.ToString(statusFlagsBytes).Replace("-", string.Empty), NumberStyles.HexNumber);
                    //reverse so the value of 9 00001001 will be 10010000 which is easier to read via index/table provided in the document ASM-DES04
                    string statusBinary = Reverse(Convert.ToString(statusFlagsBytes[7], 2).PadLeft(8, '0'));    //read byte26 bits
                    UsbPowered = statusBinary[0].Equals('1');
                    RecordingPaused = statusBinary[1].Equals('1');
                    FlashIsFull = statusBinary[2].Equals('1');
                    PowerIsGood = statusBinary[3].Equals('1');
                    AdaptiveScheduler = statusBinary[4].Equals('1');
                    DfuServiceOn = statusBinary[5].Equals('1');
                }

                if (Length > 34)  //supported fw for ASM-1329
                {
                    var statusTimestampTicksBytes = reader.ReadBytes(3);
                    Array.Reverse(statusTimestampTicksBytes);
                    var statusTimestampTicks = long.Parse(BitConverter.ToString(statusTimestampTicksBytes).Replace("-", string.Empty), NumberStyles.HexNumber);
                    StatusTimestamp = AppendToCurrentTimestamp(StatusTimestamp, ConvertTicksTomS(statusTimestampTicks));

                    var transferSuccessTimestampTicksBytes = reader.ReadBytes(3);
                    if (TransferSuccessTimestamp != -1)
                    {
                        Array.Reverse(transferSuccessTimestampTicksBytes);
                        var transferSuccessTimestampTicks = long.Parse(BitConverter.ToString(transferSuccessTimestampTicksBytes).Replace("-", string.Empty), NumberStyles.HexNumber);
                        TransferSuccessTimestamp = AppendToCurrentTimestamp(TransferSuccessTimestamp, ConvertTicksTomS(transferSuccessTimestampTicks));
                    }

                    var transferFailTimestampTicksBytes = reader.ReadBytes(3);
                    if(TransferFailTimestamp != -1)
                    {
                        Array.Reverse(transferFailTimestampTicksBytes);
                        var transferFailTimestampTicks = long.Parse(BitConverter.ToString(transferFailTimestampTicksBytes).Replace("-", string.Empty), NumberStyles.HexNumber);
                        TransferFailTimestamp = AppendToCurrentTimestamp(TransferFailTimestamp, ConvertTicksTomS(transferFailTimestampTicks));
                    }

                    var nextSyncAttemptTimeBytes = reader.ReadBytes(4);
                    Array.Reverse(nextSyncAttemptTimeBytes);
                    NextSyncAttemptTimestamp = long.Parse(BitConverter.ToString(nextSyncAttemptTimeBytes).Replace("-", string.Empty), NumberStyles.HexNumber);
                    NextSyncAttemptTimestamp = ConvertMinuteToMS((long)NextSyncAttemptTimestamp);

                    var storageFullBytes = reader.ReadBytes(3);
                    Array.Reverse(storageFullBytes);
                    StorageFull = int.Parse(BitConverter.ToString(storageFullBytes).Replace("-", string.Empty), NumberStyles.HexNumber);
                    /* I am moving this to the UI level, because this values might be meaningful in the web DB
                    if (StorageFull > App.MaxSensorStorageCapacityKB)
                    {
                        StorageFull = App.MaxSensorStorageCapacityKB;
                    }*/
                    var storageToDelBytes = reader.ReadBytes(3);
                    Array.Reverse(storageToDelBytes);
                    StorageToDel = int.Parse(BitConverter.ToString(storageToDelBytes).Replace("-", string.Empty), NumberStyles.HexNumber);

                    var storageBadBytes = reader.ReadBytes(3);
                    Array.Reverse(storageBadBytes);
                    StorageBad = int.Parse(BitConverter.ToString(storageBadBytes).Replace("-", string.Empty), NumberStyles.HexNumber);
                }

                SyncMode = (int)syncMode;
                BaseStationTimestamp = DateHelper.GetTimestamp(DateTime.Now);

                reader.Close();
                stream = null;

                IsSuccess = true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }

            return IsSuccess;
        }
    }
}
