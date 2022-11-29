using FakeItEasy;
using NUnit.Framework;
using shimmer.Models;
using ShimmerBLEAPI.Devices;
using ShimmerBLETests.Communications;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System;
using static shimmer.Models.OpConfigPayload;
using static shimmer.Models.ShimmerBLEEventData;
using System.Diagnostics;
using System.IO;
using shimmer.Sensors;
using ShimmerAPI;

namespace ShimmerBLETests
{
    public class VerisenseCommandsTest
    {
        VerisenseBLEDevice VerisenseBLEDevice;
        string uuid = "00000000-0000-0000-0000-c96117537402";
        readonly byte[] defaultBytes = new byte[] { 0x5A, 0x17, 0x74, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0xD8, 0x0F, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0xF4, 0x18, 0x1C, 0x02, 0x0A, 0x0F, 0x00, 0x18, 0x1C, 0x02, 0x0A, 0x0F, 0x00, 0x18, 0x1C, 0x02, 0x0A, 0x0F, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x3C, 0x00, 0x0E, 0x00, 0x00, 0x63, 0x28, 0xCC, 0xCC, 0x1E, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x00, 0x01 };


        [SetUp]
        public void Setup()
        {
            VerisenseBLEDevice = new TestVerisenseBLEDevice(uuid, "");
        }

        [Test]
        public async Task Connect()
        {
            var result = await VerisenseBLEDevice.Connect(false);
            if (result && VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Connected)
            {
                Assert.Pass();
            } else
                Assert.Fail();
        }

        [Test]
        public async Task Disonnect()
        {
            var result = await VerisenseBLEDevice.Connect(false);
            if (result)
            {
                result = await VerisenseBLEDevice.Disconnect();
                if (result && (VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Disconnected || VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Limited))
                {
                    Assert.Pass();
                }
            }
            Assert.Fail();
        }

        [Test]
        public async Task ReadProdConfig()
        {
            var result = await VerisenseBLEDevice.Connect(false);
            if (result && VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Connected)
            {
                await VerisenseBLEDevice.ExecuteRequest(RequestType.ReadProductionConfig);
                if (VerisenseBLEDevice.GetProductionConfig().REV_FW_MAJOR == 1 && VerisenseBLEDevice.GetProductionConfig().REV_FW_MINOR == 2)
                {
                    Assert.Pass();
                }
            }
            Assert.Fail();
        }

        [Test]
        public async Task ReadOpConfig()
        {
            var result = await VerisenseBLEDevice.Connect(false);
            if (result && VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Connected)
            {
                await VerisenseBLEDevice.ExecuteRequest(RequestType.ReadOperationalConfig);

                byte[] temp = new byte[] { 90, 151, 0, 0, 0, 48, 32, 0, 127, 0, 216, 15, 0, 0, 0, 0, 128, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 244, 24, 12, 3, 10, 15, 0, 24, 12, 3, 10, 15, 0, 24, 12, 3, 10, 15, 0, 255, 255, 0, 0, 0, 0, 0, 0, 0, 0, 99, 40, 204, 204, 30, 0, 10, 0, 0, 0, 0, 1 };

                if (VerisenseBLEDevice.GetOperationalConfigByteArray().Length == temp.Length)
                {
                    for (int i=0; i< temp.Length; i++)
                    {
                        if (VerisenseBLEDevice.GetOperationalConfigByteArray()[i] != temp[i])
                            Assert.Fail();
                    }
                    Assert.Pass();
                }
            }
            Assert.Fail();
        }

        [Test]
        public async Task ReadStatus()
        {
            var result = await VerisenseBLEDevice.Connect(false);
            if (result && VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Connected)
            {
                await VerisenseBLEDevice.ExecuteRequest(RequestType.ReadStatus);
                if (VerisenseBLEDevice.GetStatus().StorageToDel == 1312 && VerisenseBLEDevice.GetStatus().StorageBad == 768)
                {
                    Assert.Pass();
                }
            }
            Assert.Fail();
        }

        [Test]
        public async Task ReadEventLog()
        {
            var result = await VerisenseBLEDevice.Connect(false);
            if (result && VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Connected)
            {
                await VerisenseBLEDevice.ExecuteRequest(RequestType.ReadEventLog);
                LogEventsPayload logEvents = VerisenseBLEDevice.GetLogEvents();
                if (logEvents != null && 
                    logEvents.LogEvents[0].CurrentEvent == LogEvent.BLE_DISCONNECTED &&
                    logEvents.LogEvents[2].CurrentEvent == LogEvent.BLE_CONNECTED)
                {
                    Assert.Pass();
                }
            }
            Assert.Fail();
        }

        [Test]
        public async Task ReadTime()
        {
            var result = await VerisenseBLEDevice.Connect(false);
            if (result && VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Connected)
            {
                await VerisenseBLEDevice.ExecuteRequest(RequestType.ReadRTC);
                if (VerisenseBLEDevice.GetLastReceivedRTC().Ticks == 1320190 )
                {
                    Assert.Pass();
                }
            }
            Assert.Fail();
        }

        [Test]
        public async Task ReadPendingEvents()
        {
            var result = await VerisenseBLEDevice.Connect(false);
            if (result && VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Connected)
            {
                PendingEventsPayload PendingEvents = (PendingEventsPayload)await VerisenseBLEDevice.ExecuteRequest(RequestType.ReadPendingEvents);
                if (PendingEvents.DataEvent == true)
                {
                    Assert.Pass();
                }
            }
            Assert.Fail();
        }

        [Test]
        public async Task StopStream()
        {
            var result = await VerisenseBLEDevice.Connect(false);
            if (result && VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Connected)
            {
                await VerisenseBLEDevice.ExecuteRequest(RequestType.StartStreaming);
                if (VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Streaming)
                {
                    await VerisenseBLEDevice.ExecuteRequest(RequestType.StopStreaming);
                    if (VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Connected)
                    {
                        Assert.Pass();
                    }
                }

            }
            Assert.Fail();
        }

        [Test]
        public void TestIsLoggingEnabled()
        {
            VerisenseBLEDevice clone = new TestVerisenseBLEDevice(uuid, "", defaultBytes);
            VerisenseBLEDevice bleDevice = new VerisenseBLEDevice(clone);
            if (bleDevice.IsLoggingEnabled())
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestStartTime()
        {
            VerisenseBLEDevice clone = new TestVerisenseBLEDevice(uuid, "", defaultBytes);
            VerisenseBLEDevice bleDevice = new VerisenseBLEDevice(clone);
            bleDevice.SetStartTimeInMinutes(1000);
            if (!bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.START_TIME].Equals(0b11101000) ||
                !bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.START_TIME + 1].Equals(0b00000011) ||
                !bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.START_TIME + 2].Equals(0b00000000) ||
                !bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.START_TIME + 3].Equals(0b00000000))
            {
                Assert.Fail();
            }

            if (!bleDevice.GetStartTimeinMinutes().Equals(1000))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestEndTime()
        {
            VerisenseBLEDevice clone = new TestVerisenseBLEDevice(uuid, "", defaultBytes);
            VerisenseBLEDevice bleDevice = new VerisenseBLEDevice(clone);
            bleDevice.SetEndTimeInMinutes(2000);
            if (!bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.END_TIME].Equals(0b11010000) ||
                !bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.END_TIME + 1].Equals(0b00000111) ||
                !bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.END_TIME + 2].Equals(0b00000000) ||
                !bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.END_TIME + 3].Equals(0b00000000))
            {
                Assert.Fail();
            }

            if (!bleDevice.GetEndTimeinMinutes().Equals(2000))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestBLERetryCount()
        {
            VerisenseBLEDevice clone = new TestVerisenseBLEDevice(uuid, "", defaultBytes);
            VerisenseBLEDevice bleDevice = new VerisenseBLEDevice(clone);
            //default value
            if(!bleDevice.GetBLERetryCount().Equals(3))
            {
                Assert.Fail();
            }
            bleDevice.SetBLERetryCount(10);
            if (!bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.BLE_RETRY_COUNT].Equals(0b00001010))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestBLETXPower()
        {
            var bytes = CopyDefaultBytes();
            VerisenseBLEDevice clone = new TestVerisenseBLEDevice(uuid, "", bytes);
            VerisenseBLEDevice bleDevice = new VerisenseBLEDevice(clone);
            //default value
            if (!bleDevice.GetBLETXPower().Equals(VerisenseDevice.BT5RadioOutputPower.Power_11))
            {
                Assert.Fail();
            }

            //set power to +8dBm
            bytes[(int)ConfigurationBytesIndexName.BLE_TX_POWER] = 0b00001000;
            clone = new TestVerisenseBLEDevice(uuid, "", bytes);
            bleDevice = new VerisenseBLEDevice(clone);
            if (!bleDevice.GetBLETXPower().Equals(VerisenseDevice.BT5RadioOutputPower.Power_1))
            {
                Assert.Fail();
            }

            //set power to +7dBm
            bytes[(int)ConfigurationBytesIndexName.BLE_TX_POWER] = 0b00000111;
            clone = new TestVerisenseBLEDevice(uuid, "", bytes);
            bleDevice = new VerisenseBLEDevice(clone);
            if (!bleDevice.GetBLETXPower().Equals(VerisenseDevice.BT5RadioOutputPower.Power_2))
            {
                Assert.Fail();
            }

            //set power to +6dBm
            bytes[(int)ConfigurationBytesIndexName.BLE_TX_POWER] = 0b00000110;
            clone = new TestVerisenseBLEDevice(uuid, "", bytes);
            bleDevice = new VerisenseBLEDevice(clone);
            if (!bleDevice.GetBLETXPower().Equals(VerisenseDevice.BT5RadioOutputPower.Power_3))
            {
                Assert.Fail();
            }

            //set power to +5dBm
            bytes[(int)ConfigurationBytesIndexName.BLE_TX_POWER] = 0b00000101;
            clone = new TestVerisenseBLEDevice(uuid, "", bytes);
            bleDevice = new VerisenseBLEDevice(clone);
            if (!bleDevice.GetBLETXPower().Equals(VerisenseDevice.BT5RadioOutputPower.Power_4))
            {
                Assert.Fail();
            }

            //set power to +4dBm
            bytes[(int)ConfigurationBytesIndexName.BLE_TX_POWER] = 0b00000100;
            clone = new TestVerisenseBLEDevice(uuid, "", bytes);
            bleDevice = new VerisenseBLEDevice(clone);
            if (!bleDevice.GetBLETXPower().Equals(VerisenseDevice.BT5RadioOutputPower.Power_5))
            {
                Assert.Fail();
            }

            //set power to +3dBm
            bytes[(int)ConfigurationBytesIndexName.BLE_TX_POWER] = 0b00000011;
            clone = new TestVerisenseBLEDevice(uuid, "", bytes);
            bleDevice = new VerisenseBLEDevice(clone);
            if (!bleDevice.GetBLETXPower().Equals(VerisenseDevice.BT5RadioOutputPower.Power_6))
            {
                Assert.Fail();
            }

            //set power to +2dBm
            bytes[(int)ConfigurationBytesIndexName.BLE_TX_POWER] = 0b00000010;
            clone = new TestVerisenseBLEDevice(uuid, "", bytes);
            bleDevice = new VerisenseBLEDevice(clone);
            if (!bleDevice.GetBLETXPower().Equals(VerisenseDevice.BT5RadioOutputPower.Power_7))
            {
                Assert.Fail();
            }

            //set power to +0dBm
            bytes[(int)ConfigurationBytesIndexName.BLE_TX_POWER] = 0b00000000;
            clone = new TestVerisenseBLEDevice(uuid, "", bytes);
            bleDevice = new VerisenseBLEDevice(clone);
            if (!bleDevice.GetBLETXPower().Equals(VerisenseDevice.BT5RadioOutputPower.Power_8))
            {
                Assert.Fail();
            }

            //set power to -4dBm
            bytes[(int)ConfigurationBytesIndexName.BLE_TX_POWER] = 0b11111100;
            clone = new TestVerisenseBLEDevice(uuid, "", bytes);
            bleDevice = new VerisenseBLEDevice(clone);
            if (!bleDevice.GetBLETXPower().Equals(VerisenseDevice.BT5RadioOutputPower.Power_9))
            {
                Assert.Fail();
            }

            //set power to -8dBm
            bytes[(int)ConfigurationBytesIndexName.BLE_TX_POWER] = 0b11111000;
            clone = new TestVerisenseBLEDevice(uuid, "", bytes);
            bleDevice = new VerisenseBLEDevice(clone);
            if (!bleDevice.GetBLETXPower().Equals(VerisenseDevice.BT5RadioOutputPower.Power_10))
            {
                Assert.Fail();
            }

            //set power to -12dBm
            bytes[(int)ConfigurationBytesIndexName.BLE_TX_POWER] = 0b11110100;
            clone = new TestVerisenseBLEDevice(uuid, "", bytes);
            bleDevice = new VerisenseBLEDevice(clone);
            if (!bleDevice.GetBLETXPower().Equals(VerisenseDevice.BT5RadioOutputPower.Power_11))
            {
                Assert.Fail();
            }

            //set power to -16dBm
            bytes[(int)ConfigurationBytesIndexName.BLE_TX_POWER] = 0b11110000;
            clone = new TestVerisenseBLEDevice(uuid, "", bytes);
            bleDevice = new VerisenseBLEDevice(clone);
            if (!bleDevice.GetBLETXPower().Equals(VerisenseDevice.BT5RadioOutputPower.Power_12))
            {
                Assert.Fail();
            }

            //set power to -20dBm
            bytes[(int)ConfigurationBytesIndexName.BLE_TX_POWER] = 0b11101100;
            clone = new TestVerisenseBLEDevice(uuid, "", bytes);
            bleDevice = new VerisenseBLEDevice(clone);
            if (!bleDevice.GetBLETXPower().Equals(VerisenseDevice.BT5RadioOutputPower.Power_13))
            {
                Assert.Fail();
            }

            //set power to -40dBm
            bytes[(int)ConfigurationBytesIndexName.BLE_TX_POWER] = 0b11111111;
            clone = new TestVerisenseBLEDevice(uuid, "", bytes);
            bleDevice = new VerisenseBLEDevice(clone);
            if (!bleDevice.GetBLETXPower().Equals(VerisenseDevice.BT5RadioOutputPower.Power_14))
            {
                Assert.Fail();
            }

            //set power to -40dBm
            bytes[(int)ConfigurationBytesIndexName.BLE_TX_POWER] = 0b11011000;
            clone = new TestVerisenseBLEDevice(uuid, "", bytes);
            bleDevice = new VerisenseBLEDevice(clone);
            if (!bleDevice.GetBLETXPower().Equals(VerisenseDevice.BT5RadioOutputPower.Power_15))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestDataTransferInterval()
        {
            VerisenseBLEDevice clone = new TestVerisenseBLEDevice(uuid, "", defaultBytes);
            VerisenseBLEDevice bleDevice = new VerisenseBLEDevice(clone);
            //default value
            if (!bleDevice.GetDataTransferInterval().Equals(24))
            {
                Assert.Fail();
            }
            bleDevice.SetDataTransferInterval(20);
            if (!bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.BLE_DATA_TRANS_WKUP_INT_HRS].Equals(0b00010100))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestDataTransferDuration()
        {
            VerisenseBLEDevice clone = new TestVerisenseBLEDevice(uuid, "", defaultBytes);
            VerisenseBLEDevice bleDevice = new VerisenseBLEDevice(clone);
            //default value
            if (!bleDevice.GetDataTransferDuration().Equals(10))
            {
                Assert.Fail();
            }
            bleDevice.SetDataTransferDuration(20);
            if (!bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.BLE_DATA_TRANS_WKUP_DUR].Equals(0b00010100))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestDataTransferStartTime()
        {
            VerisenseBLEDevice clone = new TestVerisenseBLEDevice(uuid, "", defaultBytes);
            VerisenseBLEDevice bleDevice = new VerisenseBLEDevice(clone);
            //default value
            if (!bleDevice.GetDataTransferStartTime().Equals(540))
            {
                Assert.Fail();
            }
            bleDevice.SetDataTransferStartTime(720);
            if (!bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.BLE_DATA_TRANS_WKUP_TIME].Equals(0b11010000) ||
                !bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.BLE_DATA_TRANS_WKUP_TIME + 1].Equals(0b00000010))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestDataTransferRetryInterval()
        {
            VerisenseBLEDevice clone = new TestVerisenseBLEDevice(uuid, "", defaultBytes);
            VerisenseBLEDevice bleDevice = new VerisenseBLEDevice(clone);
            //default value
            if (!bleDevice.GetDataTransferRetryInterval().Equals(15))
            {
                Assert.Fail();
            }
            bleDevice.SetDataTransferRetryInterval(500);
            if (!bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.BLE_DATA_TRANS_RETRY_INT].Equals(0b11110100) ||
                !bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.BLE_DATA_TRANS_RETRY_INT + 1].Equals(0b00000001))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestStatusInterval()
        {
            VerisenseBLEDevice clone = new TestVerisenseBLEDevice(uuid, "", defaultBytes);
            VerisenseBLEDevice bleDevice = new VerisenseBLEDevice(clone);
            //default value
            if (!bleDevice.GetStatusInterval().Equals(24))
            {
                Assert.Fail();
            }
            bleDevice.SetStatusInterval(30);
            if (!bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.BLE_STATUS_WKUP_INT_HRS].Equals(0b00011110))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }
        [Test]
        public void TestStatusDuration()
        {
            VerisenseBLEDevice clone = new TestVerisenseBLEDevice(uuid, "", defaultBytes);
            VerisenseBLEDevice bleDevice = new VerisenseBLEDevice(clone);
            //default value
            if (!bleDevice.GetStatusDuration().Equals(10))
            {
                Assert.Fail();
            }
            bleDevice.SetStatusDuration(20);
            if (!bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.BLE_STATUS_WKUP_DUR].Equals(0b00010100))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestStatusStartTime()
        {
            VerisenseBLEDevice clone = new TestVerisenseBLEDevice(uuid, "", defaultBytes);
            VerisenseBLEDevice bleDevice = new VerisenseBLEDevice(clone);
            //default value
            if (!bleDevice.GetStatusStartTime().Equals(540))
            {
                Assert.Fail();
            }
            bleDevice.SetStatusStartTime(600);
            if (!bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.BLE_STATUS_WKUP_TIME].Equals(0b01011000) ||
                !bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.BLE_STATUS_WKUP_TIME + 1].Equals(0b00000010))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestStatusRetryInterval()
        {
            VerisenseBLEDevice clone = new TestVerisenseBLEDevice(uuid, "", defaultBytes);
            VerisenseBLEDevice bleDevice = new VerisenseBLEDevice(clone);
            //default value
            if (!bleDevice.GetStatusRetryInterval().Equals(15))
            {
                Assert.Fail();
            }
            bleDevice.SetStatusRetryInterval(30);
            if (!bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.BLE_STATUS_RETRY_INT].Equals(0b00011110) ||
                !bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.BLE_STATUS_RETRY_INT + 1].Equals(0b00000000))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }
        [Test]
        public void TestRTCSyncInterval()
        {
            VerisenseBLEDevice clone = new TestVerisenseBLEDevice(uuid, "", defaultBytes);
            VerisenseBLEDevice bleDevice = new VerisenseBLEDevice(clone);
            //default value
            if (!bleDevice.GetRTCSyncInterval().Equals(24))
            {
                Assert.Fail();
            }
            bleDevice.SetRTCSyncInterval(48);
            if (!bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.BLE_RTC_SYNC_WKUP_INT_HRS].Equals(0b00110000))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestRTCSyncDuration()
        {
            VerisenseBLEDevice clone = new TestVerisenseBLEDevice(uuid, "", defaultBytes);
            VerisenseBLEDevice bleDevice = new VerisenseBLEDevice(clone);
            //default value
            if (!bleDevice.GetRTCSyncDuration().Equals(10))
            {
                Assert.Fail();
            }
            bleDevice.SetRTCSyncDuration(20);
            if (!bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.BLE_RTC_SYNC_WKUP_DUR].Equals(0b00010100))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }
        [Test]
        public void TestRTCSyncTime()
        {
            VerisenseBLEDevice clone = new TestVerisenseBLEDevice(uuid, "", defaultBytes);
            VerisenseBLEDevice bleDevice = new VerisenseBLEDevice(clone);
            //default value
            if (!bleDevice.GetRTCSyncTime().Equals(540))
            {
                Assert.Fail();
            }
            bleDevice.SetRTCSyncTime(960);
            if (!bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.BLE_RTC_SYNC_WKUP_TIME].Equals(0b11000000) ||
                !bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.BLE_RTC_SYNC_WKUP_TIME + 1].Equals(0b00000011))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }
        [Test]
        public void TestRTCSyncRetryInterval()
        {
            VerisenseBLEDevice clone = new TestVerisenseBLEDevice(uuid, "", defaultBytes);
            VerisenseBLEDevice bleDevice = new VerisenseBLEDevice(clone);
            //default value
            if (!bleDevice.GetRTCSyncRetryInterval().Equals(15))
            {
                Assert.Fail();
            }
            bleDevice.SetRTCSyncRetryInterval(30);
            if (!bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.BLE_RTC_SYNC_RETRY_INT].Equals(0b00011110) ||
                !bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.BLE_RTC_SYNC_RETRY_INT + 1].Equals(0b00000000))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void TestAdaptiveSchedulerInterval()
        {
            VerisenseBLEDevice clone = new TestVerisenseBLEDevice(uuid, "", defaultBytes);
            VerisenseBLEDevice bleDevice = new VerisenseBLEDevice(clone);
            //default value
            if (!bleDevice.GetAdaptiveSchedulerInterval().Equals(65535))
            {
                Assert.Fail();
            }
            bleDevice.SetAdaptiveSchedulerInterval(61680);
            if (!bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.ADAPTIVE_SCHEDULER_INT].Equals(0b11110000) ||
                !bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.ADAPTIVE_SCHEDULER_INT + 1].Equals(0b11110000))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }
        [Test]
        public void TestAdaptiveSchedulerMaxFailCount()
        {
            VerisenseBLEDevice clone = new TestVerisenseBLEDevice(uuid, "", defaultBytes);
            VerisenseBLEDevice bleDevice = new VerisenseBLEDevice(clone);
            //default value
            if (!bleDevice.GetAdaptiveSchedulerMaxFailCount().Equals(255))
            {
                Assert.Fail();
            }
            bleDevice.SetAdaptiveSchedulerMaxFailCount(20);
            if (!bleDevice.GetOperationalConfigByteArray()[(int)ConfigurationBytesIndexName.ADAPTIVE_SCHEDULER_FAILCOUNT_MAX].Equals(0b00010100))
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        private byte[] CopyDefaultBytes()
        {
            var bytes = new byte[defaultBytes.Length];
            Array.Copy(defaultBytes, bytes, defaultBytes.Length);
            return bytes;
        }
    }
}