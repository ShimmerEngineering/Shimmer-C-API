using NUnit.Framework;
using shimmer.Models;
using System;
using static shimmer.Models.StatusPayload;
namespace ShimmerBLETests
{
    class VerisenseStatusTest
    {
        /// <summary>
        /// Note the first three bytes is the Header and then the Length (2 bytes)
        /// </summary>
        readonly byte[] defaultBytes = new byte[] { 49, 65, 0, 69, 55, 1, 39, 8, 33, 0, 0, 0, 0, 1, 0, 0, 255, 255, 255, 255, 255, 255, 255, 255, 0, 253, 7, 0, 0, 73, 0, 0, 0, 0, 0, 0, 0, 190, 6, 3, 255, 255, 255, 255, 255, 255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 8, 0, 0 };

        [Test]
        public void TestStatusStorageFull()
        {
            var status = new byte[defaultBytes.Length];
            Array.Copy(defaultBytes, status, defaultBytes.Length);

            status[(int)ConfigurationBytesIndexName.STORAGE_FULL + 3] = 0x12;
            status[(int)ConfigurationBytesIndexName.STORAGE_FULL + 4] = 0x00;
            status[(int)ConfigurationBytesIndexName.STORAGE_FULL + 5] = 0x00;
            status[(int)ConfigurationBytesIndexName.STORAGE_FULL_MSB + 3] = 0x11;

            StatusPayload statusPayload = new StatusPayload();
            statusPayload.ProcessPayload(status, 0);
            if(statusPayload.StorageFull != 285212690)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestStatusStorageToDel()
        {
            var status = new byte[defaultBytes.Length];
            Array.Copy(defaultBytes, status, defaultBytes.Length);
            //+3 to skip the headers
            status[(int)ConfigurationBytesIndexName.STORAGE_TO_DEL + 3] = 0x12;
            status[(int)ConfigurationBytesIndexName.STORAGE_TO_DEL + 4] = 0x00;
            status[(int)ConfigurationBytesIndexName.STORAGE_TO_DEL + 5] = 0x00;
            status[(int)ConfigurationBytesIndexName.STORAGE_TO_DEL_MSB + 3] = 0x11;

            StatusPayload statusPayload = new StatusPayload();
            statusPayload.ProcessPayload(status, 0);
            if (statusPayload.StorageToDel != 285212690)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestStatusStorageBad()
        {
            var status = new byte[defaultBytes.Length];
            Array.Copy(defaultBytes, status, defaultBytes.Length);
            //+3 to skip the headers
            status[(int)ConfigurationBytesIndexName.STORAGE_BAD + 3] = 0x12;
            status[(int)ConfigurationBytesIndexName.STORAGE_BAD + 4] = 0x00;
            status[(int)ConfigurationBytesIndexName.STORAGE_BAD + 5] = 0x00;
            status[(int)ConfigurationBytesIndexName.STORAGE_BAD + 6] = 0x11;

            StatusPayload statusPayload = new StatusPayload();
            statusPayload.ProcessPayload(status, 0);
            if (statusPayload.StorageBad != 285212690)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestStatusFreeStorage()
        {
            var status = new byte[defaultBytes.Length];
            Array.Copy(defaultBytes, status, defaultBytes.Length);
            //+3 to skip the headers
            status[(int)ConfigurationBytesIndexName.STORAGE_FREE + 3] = 0x12;
            status[(int)ConfigurationBytesIndexName.STORAGE_FREE + 4] = 0x00;
            status[(int)ConfigurationBytesIndexName.STORAGE_FREE + 5] = 0x00;
            status[(int)ConfigurationBytesIndexName.STORAGE_FREE_MSB + 3] = 0x11;

            StatusPayload statusPayload = new StatusPayload();
            statusPayload.ProcessPayload(status, 0);
            if (statusPayload.FreeStorage != 285212690)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestStatusStorageCapacity()
        {
            var status = new byte[defaultBytes.Length];
            Array.Copy(defaultBytes, status, defaultBytes.Length);
            //+3 to skip the headers
            status[(int)ConfigurationBytesIndexName.STORAGE_CAPACITY + 3] = 0x12;
            status[(int)ConfigurationBytesIndexName.STORAGE_CAPACITY + 4] = 0x00;
            status[(int)ConfigurationBytesIndexName.STORAGE_CAPACITY + 5] = 0x00;
            status[(int)ConfigurationBytesIndexName.STORAGE_CAPACITY + 6] = 0x11;

            StatusPayload statusPayload = new StatusPayload();
            statusPayload.ProcessPayload(status, 0);
            if (statusPayload.StorageCapacity != 285212690)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestStatusIsChargerChipPresent()
        {
            var status = new byte[defaultBytes.Length];
            Array.Copy(defaultBytes, status, defaultBytes.Length);
            StatusPayload statusPayload = new StatusPayload();
            //+3 to skip the headers
            status[(int)ConfigurationBytesIndexName.METADATA_01 + 3] = (byte)(status[(int)ConfigurationBytesIndexName.METADATA_01 + 3] & 0b11111110);
            statusPayload.ProcessPayload(status, 0);
            bool isChargerChipPresent = statusPayload.IsChargerChipPresent.HasValue ? statusPayload.IsChargerChipPresent.Value : false;
            if (isChargerChipPresent != false)
            {
                Assert.Fail();
            }
            //+3 to skip the headers
            status[(int)ConfigurationBytesIndexName.METADATA_01 + 3] = (byte)((status[(int)ConfigurationBytesIndexName.METADATA_01 + 3] & 0b11111110) | 0b00000001);
            statusPayload.ProcessPayload(status, 0);
            isChargerChipPresent = statusPayload.IsChargerChipPresent.HasValue ? statusPayload.IsChargerChipPresent.Value : false;
            if (isChargerChipPresent != true)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestStatusBattChargerStatus()
        {
            var status = new byte[defaultBytes.Length];
            Array.Copy(defaultBytes, status, defaultBytes.Length);
            //+3 to skip the headers
            status[(int)ConfigurationBytesIndexName.METADATA_01 + 3] = (byte)(status[(int)ConfigurationBytesIndexName.METADATA_01 + 3] & 0b11111001);

            StatusPayload statusPayload = new StatusPayload();
            statusPayload.ProcessPayload(status, 0);
            if (statusPayload.BattChargerStatus != StatusPayload.BatteryChargerStatus.BadBattery)
            {
                Assert.Fail();
            }
            //+3 to skip the headers
            status[(int)ConfigurationBytesIndexName.METADATA_01 + 3] = (byte)((status[(int)ConfigurationBytesIndexName.METADATA_01 + 3] & 0b11111001) | 0b00000010);
            statusPayload.ProcessPayload(status, 0);
            if (statusPayload.BattChargerStatus != StatusPayload.BatteryChargerStatus.Charging)
            {
                Assert.Fail();
            }
            //+3 to skip the headers
            status[(int)ConfigurationBytesIndexName.METADATA_01 + 3] = (byte)((status[(int)ConfigurationBytesIndexName.METADATA_01 + 3] & 0b11111001) | 0b00000100);
            statusPayload.ProcessPayload(status, 0);
            if (statusPayload.BattChargerStatus != StatusPayload.BatteryChargerStatus.ChargingComplete)
            {
                Assert.Fail();
            }
            //+3 to skip the headers
            status[(int)ConfigurationBytesIndexName.METADATA_01 + 3] = (byte)((status[(int)ConfigurationBytesIndexName.METADATA_01 + 3] & 0b11111001) | 0b00000110);
            statusPayload.ProcessPayload(status, 0);
            if (statusPayload.BattChargerStatus != StatusPayload.BatteryChargerStatus.PowerDown)
            {
                Assert.Fail();
            }
        }
    }
}
