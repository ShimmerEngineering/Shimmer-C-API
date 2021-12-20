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
    class VerisenseSyncTest
    {

        string uuid = "00000000-0000-0000-0000-c96117537402";
        public static bool IsSyncNACKTest { get; set; }
        public static bool IsSyncMTUSizeTest { get; set; }

        [Test]
        public async Task SyncData()
        {
            TestVerisenseBLEDevice VerisenseBLEDevice = new TestVerisenseBLEDevice(uuid, "");

            VerisenseBLEDevice.ShimmerBLEEvent += delegate (object sender, ShimmerBLEEventData e)
            {
                if (e.CurrentEvent == VerisenseBLEEvent.StateChange)
                {
                    Trace.WriteLine("Shimmer State: " + VerisenseBLEDevice.GetVerisenseBLEState().ToString());
                };

                if (e.CurrentEvent == VerisenseBLEEvent.SyncLoggedDataNewPayload)
                {
                    ((TestVerisenseBLEDevice)VerisenseBLEDevice).InjectDataSyncEndBytes();
                };

            };

            var result = await VerisenseBLEDevice.Connect(false);

            if (result && VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Connected)
            {
                IsSyncNACKTest = false;
                IsSyncMTUSizeTest = false;
                bool IsValidBinFile = true;

                var data = await VerisenseBLEDevice.ExecuteRequest(RequestType.TransferLoggedData);
                byte[] dataBytesSent = new byte[] { 58, 198, 0, 2, 180, 0, 22, 144, 255, 96, 255, 16, 240, 160, 255, 96, 255, 224, 239, 176, 255, 144, 255, 224, 239, 176, 255, 128, 255, 0, 240, 208, 255, 112, 255, 0, 240, 208, 255, 128, 255, 240, 239, 176, 255, 176, 255, 16, 240, 128, 255, 128, 255, 224, 239, 176, 255, 112, 255, 240, 239, 160, 255, 128, 255, 0, 240, 208, 255, 96, 255, 240, 239, 160, 255, 96, 255, 32, 240, 144, 255, 96, 255, 240, 239, 224, 255, 96, 255, 16, 240, 160, 255, 144, 255, 16, 240, 192, 255, 144, 255, 0, 240, 192, 255, 144, 255, 16, 240, 176, 255, 96, 255, 0, 240, 160, 255, 112, 255, 240, 239, 224, 255, 112, 255, 16, 240, 160, 255, 96, 255, 0, 240, 176, 255, 160, 255, 208, 239, 176, 255, 112, 255, 0, 240, 176, 255, 128, 255, 32, 240, 208, 255, 112, 255, 208, 239, 160, 255, 112, 255, 240, 239, 160, 255, 112, 255, 32, 240, 160, 255, 112, 255, 0, 240, 144, 255, 112, 255, 240, 239, 208, 255, 160, 255, 0, 240, 160, 255, 80, 255, 16, 240, 176, 255, 144, 255, 32, 240, 92, 241 };
                byte[] binFileBytes = File.ReadAllBytes(VerisenseBLEDevice.dataFilePath);

                if (binFileBytes.Length != dataBytesSent.Length - 3)
                {
                    IsValidBinFile = false;
                }

                for (int i = 0; i < binFileBytes.Length; i++)
                {
                    if (dataBytesSent[i + 3] != binFileBytes[i])
                    {
                        IsValidBinFile = false;
                    }
                }

                if (data != null && VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Connected
                    && File.Exists(VerisenseBLEDevice.dataFilePath) && !VerisenseBLEDevice.dataFileName.Contains("BadCRC") && IsValidBinFile)
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            }
        }

        [Test]
        public async Task SyncDataTestMTU()
        {
            //this test must not be run right after or before SyncData() test to avoid create bin file error. Since the unit test is ordered alphabetically,
            //thus the naming of this unit test must be kept to be run after SyncDataNACK() (or any other unit test in between)

            TestVerisenseBLEDevice VerisenseBLEDevice = new TestVerisenseBLEDevice(uuid, "");

            VerisenseBLEDevice.ShimmerBLEEvent += delegate (object sender, ShimmerBLEEventData e)
            {
                if (e.CurrentEvent == VerisenseBLEEvent.StateChange)
                {
                    Trace.WriteLine("Shimmer State: " + VerisenseBLEDevice.GetVerisenseBLEState().ToString());
                };

                if (e.CurrentEvent == VerisenseBLEEvent.SyncLoggedDataNewPayload)
                {
                    ((TestVerisenseBLEDevice)VerisenseBLEDevice).InjectDataSyncEndBytes();
                };

            };

            var result = await VerisenseBLEDevice.Connect(false);

            if (result && VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Connected)
            {
                IsSyncNACKTest = false;
                IsSyncMTUSizeTest = true;
                bool IsValidBinFile = true;

                var data = await VerisenseBLEDevice.ExecuteRequest(RequestType.TransferLoggedData);
                byte[] dataBytesSent = new byte[] { 58, 198, 0, 2, 180, 0, 22, 144, 255, 96, 255, 16, 240, 160, 255, 96, 255, 224, 239, 176, 255, 144, 255, 224, 239, 176, 255, 128, 255, 0, 240, 208, 255, 112, 255, 0, 240, 208, 255, 128, 255, 240, 239, 176, 255, 176, 255, 16, 240, 128, 255, 128, 255, 224, 239, 176, 255, 112, 255, 240, 239, 160, 255, 128, 255, 0, 240, 208, 255, 96, 255, 240, 239, 160, 255, 96, 255, 32, 240, 144, 255, 96, 255, 240, 239, 224, 255, 96, 255, 16, 240, 160, 255, 144, 255, 16, 240, 192, 255, 144, 255, 0, 240, 192, 255, 144, 255, 16, 240, 176, 255, 96, 255, 0, 240, 160, 255, 112, 255, 240, 239, 224, 255, 112, 255, 16, 240, 160, 255, 96, 255, 0, 240, 176, 255, 160, 255, 208, 239, 176, 255, 112, 255, 0, 240, 176, 255, 128, 255, 32, 240, 208, 255, 112, 255, 208, 239, 160, 255, 112, 255, 240, 239, 160, 255, 112, 255, 32, 240, 160, 255, 112, 255, 0, 240, 144, 255, 112, 255, 240, 239, 208, 255, 160, 255, 0, 240, 160, 255, 80, 255, 16, 240, 176, 255, 144, 255, 32, 240, 92, 241 };
                byte[] binFileBytes = File.ReadAllBytes(VerisenseBLEDevice.dataFilePath);

                if (binFileBytes.Length != dataBytesSent.Length - 3)
                {
                    IsValidBinFile = false;
                }

                for (int i = 0; i < binFileBytes.Length; i++)
                {
                    if (dataBytesSent[i + 3] != binFileBytes[i])
                    {
                        IsValidBinFile = false;
                    }
                }

                if (data != null && VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Connected
                    && File.Exists(VerisenseBLEDevice.dataFilePath) && !VerisenseBLEDevice.dataFileName.Contains("BadCRC") && IsValidBinFile)
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            }
        }

        [Test]
        public async Task SyncDataNACK()
        {
            TestVerisenseBLEDevice VerisenseBLEDevice = new TestVerisenseBLEDevice(uuid, "");

            VerisenseBLEDevice.ShimmerBLEEvent += delegate (object sender, ShimmerBLEEventData e)
            {
                if (e.CurrentEvent == VerisenseBLEEvent.StateChange)
                {
                    Trace.WriteLine("Shimmer State: " + VerisenseBLEDevice.GetVerisenseBLEState().ToString());
                };

                if (e.CurrentEvent == VerisenseBLEEvent.SyncLoggedDataNewPayload)
                {
                    ((TestVerisenseBLEDevice)VerisenseBLEDevice).InjectDataSyncEndBytes();
                };

            };

            var result = await VerisenseBLEDevice.Connect(false);

            if (result && VerisenseBLEDevice.GetVerisenseBLEState() == shimmer.Communications.ShimmerDeviceBluetoothState.Connected)
            {
                IsSyncNACKTest = true;
                IsSyncMTUSizeTest = false;

                var data = await VerisenseBLEDevice.ExecuteRequest(RequestType.TransferLoggedData);

                if (data == null)
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            }
        }
    }
}
