using FakeItEasy;
using NUnit.Framework;
using shimmer.Models;
using ShimmerBLEAPI.Devices;
using ShimmerBLETests.Communications;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System;
using static shimmer.Models.ShimmerBLEEventData;
using static shimmer.Models.ProdConfigPayload;
using System.Diagnostics;
using System.IO;
using shimmer.Sensors;
using ShimmerAPI;
namespace ShimmerBLETests
{
    class VerisenseProdConfigTest
    {
        readonly byte[] defaultProdConfigBytes = new byte[] { 0x33, 0x37, 0x00, 0x5A, 0xBB, 0xA2, 0x01, 0x25, 0x09, 0x19, 0x01, 0x00, 0x01, 0x02, 0x63, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
        readonly int PasskeyIDLength = 2;
        readonly int PasskeyLength = 6;
        readonly int AdvertisingNameLength = 32;

        [Test]
        public void TestEnableClinicalTrialPasskey()
        {
            ProdConfigPayload prodConfig = new ProdConfigPayload();
            prodConfig.ProcessPayload(defaultProdConfigBytes);
            prodConfig.EnableNoPasskey("TEST");
            prodConfig.EnableClinicalTrialPasskey();

            byte[] prodConfigByteArray = prodConfig.GetPayload();

            for (int i = 0; i < PasskeyIDLength; i++)
            {
                if (prodConfigByteArray[(int)ConfigurationBytesIndexName.PASSKEY_ID + i] != 0xFF)
                {
                    Assert.Fail();
                }
            }
            for (int i = 0; i < PasskeyLength; i++)
            {
                if (prodConfigByteArray[(int)ConfigurationBytesIndexName.PASSKEY + i] != 0xFF)
                {
                    Assert.Fail();
                }
            }
            for (int i = 0; i < AdvertisingNameLength; i++)
            {
                if (prodConfigByteArray[(int)ConfigurationBytesIndexName.ADVERTISING_NAME_PREFIX + i] != 0xFF)
                {
                    Assert.Fail();
                }
            }
        }

        [Test]
        public void TestEnableNoPasskey()
        {
            ProdConfigPayload prodConfig = new ProdConfigPayload();
            prodConfig.ProcessPayload(defaultProdConfigBytes);
            string advertisingName = "aaaaaaaa";
            prodConfig.EnableNoPasskey(advertisingName);

            byte[] prodConfigByteArray = prodConfig.GetPayload();

            //passkey id 00
            for (int i = 0; i < PasskeyIDLength; i++)
            {
                if (prodConfigByteArray[(int)ConfigurationBytesIndexName.PASSKEY_ID + i] != 0x30)
                {
                    Assert.Fail();
                }
            }
            for (int i = 0; i < PasskeyLength; i++)
            {
                if (prodConfigByteArray[(int)ConfigurationBytesIndexName.PASSKEY + i] != 0xFF)
                {
                    Assert.Fail();
                }
            }
            for (int i = 0; i < advertisingName.Length; i++)
            {
                if (prodConfigByteArray[(int)ConfigurationBytesIndexName.ADVERTISING_NAME_PREFIX + i] != 0x61)
                {
                    Assert.Fail();
                }
            }
            for (int i = advertisingName.Length; i < AdvertisingNameLength; i++)
            {
                if (prodConfigByteArray[(int)ConfigurationBytesIndexName.ADVERTISING_NAME_PREFIX + i] != 0xFF)
                {
                    Assert.Fail();
                }
            }
        }

        [Test]
        public void TestEnableDefaultPasskey()
        {
            ProdConfigPayload prodConfig = new ProdConfigPayload();
            prodConfig.ProcessPayload(defaultProdConfigBytes);
            string advertisingName = "aaaaaaaa";
            string passkeyId = "01";
            prodConfig.EnableDefaultPasskey(advertisingName, passkeyId);
            byte[] prodConfigByteArray = prodConfig.GetPayload();

            //passkey id 01
            if (prodConfigByteArray[(int)ConfigurationBytesIndexName.PASSKEY_ID] != 0x30 ||
                prodConfigByteArray[(int)ConfigurationBytesIndexName.PASSKEY_ID + 1] != 0x31)
            {
                Assert.Fail();
            }
            // passkey 123456
            if (prodConfigByteArray[(int)ConfigurationBytesIndexName.PASSKEY] != 0x31 ||
                prodConfigByteArray[(int)ConfigurationBytesIndexName.PASSKEY + 1] != 0x32 ||
                prodConfigByteArray[(int)ConfigurationBytesIndexName.PASSKEY + 2] != 0x33 ||
                prodConfigByteArray[(int)ConfigurationBytesIndexName.PASSKEY + 3] != 0x34 ||
                prodConfigByteArray[(int)ConfigurationBytesIndexName.PASSKEY + 4] != 0x35 ||
                prodConfigByteArray[(int)ConfigurationBytesIndexName.PASSKEY + 5] != 0x36)
            {
                Assert.Fail();
            }
            for (int i = 0; i < advertisingName.Length; i++)
            {
                if (prodConfigByteArray[(int)ConfigurationBytesIndexName.ADVERTISING_NAME_PREFIX + i] != 0x61)
                {
                    Assert.Fail();
                }
            }
            for (int i = advertisingName.Length; i < AdvertisingNameLength; i++)
            {
                if (prodConfigByteArray[(int)ConfigurationBytesIndexName.ADVERTISING_NAME_PREFIX + i] != 0xFF)
                {
                    Assert.Fail();
                }
            }
        }

        [Test]
        public void TestProcessPayload()
        {
            var prodConfigBytes = new byte[defaultProdConfigBytes.Length];
            Array.Copy(defaultProdConfigBytes, prodConfigBytes, defaultProdConfigBytes.Length);

            prodConfigBytes[(int)ConfigurationBytesIndexName.PASSKEY_ID + 3] = 0x30;
            prodConfigBytes[(int)ConfigurationBytesIndexName.PASSKEY_ID + 4] = 0x31;

            prodConfigBytes[(int)ConfigurationBytesIndexName.PASSKEY + 3] = 0x31;
            prodConfigBytes[(int)ConfigurationBytesIndexName.PASSKEY + 4] = 0x31;
            prodConfigBytes[(int)ConfigurationBytesIndexName.PASSKEY + 5] = 0x31;
            prodConfigBytes[(int)ConfigurationBytesIndexName.PASSKEY + 6] = 0x31;
            prodConfigBytes[(int)ConfigurationBytesIndexName.PASSKEY + 7] = 0x31;
            prodConfigBytes[(int)ConfigurationBytesIndexName.PASSKEY + 8] = 0x31;

            prodConfigBytes[(int)ConfigurationBytesIndexName.ADVERTISING_NAME_PREFIX + 3] = 0x54;
            prodConfigBytes[(int)ConfigurationBytesIndexName.ADVERTISING_NAME_PREFIX + 4] = 0x45;
            prodConfigBytes[(int)ConfigurationBytesIndexName.ADVERTISING_NAME_PREFIX + 5] = 0x53;
            prodConfigBytes[(int)ConfigurationBytesIndexName.ADVERTISING_NAME_PREFIX + 6] = 0x54;

            ProdConfigPayload prodConfig = new ProdConfigPayload();
            prodConfig.ProcessPayload(prodConfigBytes);

            if(prodConfig.AdvertisingNamePrefix != "TEST")
            {
                Assert.Fail();
            }
            if (prodConfig.PasskeyID != "01")
            {
                Assert.Fail();
            }
            if (prodConfig.Passkey != "111111")
            {
                Assert.Fail();
            }

            for (int i = 0; i < PasskeyIDLength; i++)
            {
                prodConfigBytes[(int)ConfigurationBytesIndexName.PASSKEY_ID + 3 + i] = 0xFF;
            }
            for (int i = 0; i < PasskeyLength; i++)
            {
                prodConfigBytes[(int)ConfigurationBytesIndexName.PASSKEY + 3 + i] = 0xFF;
            }
            for (int i = 0; i < AdvertisingNameLength; i++)
            {
                prodConfigBytes[(int)ConfigurationBytesIndexName.ADVERTISING_NAME_PREFIX + 3 + i] = 0xFF;
            }
            prodConfig.ProcessPayload(prodConfigBytes);
            if (prodConfig.AdvertisingNamePrefix != "Verisense")
            {
                Assert.Fail();
            }
            if (prodConfig.PasskeyID != "")
            {
                Assert.Fail();
            }
            if (prodConfig.Passkey != "")
            {
                Assert.Fail();
            }
        }
    }
}
