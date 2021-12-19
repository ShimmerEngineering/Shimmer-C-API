using NUnit.Framework;
using shimmer.Helpers;
using shimmer.Sensors;
using ShimmerAPI;
using ShimmerBLEAPI.Devices;
using ShimmerBLETests.Communications;
using ShimmerBLETests.Sensors;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShimmerBLETests
{
    public class TimestampTest
    {
        TestSensorLIS2DW12 sensorLIS2DW12; //Test sensor class instance here in order to get access to to Sensor.cs protected variables
        SensorLSM6DS3 sensorLSM6DS3;
        SensorGSR sensorGSR;
        SensorPPG sensorPPG;

        string uuid = "00000000-0000-0000-0000-daa619f04ad7";
        double[] TimestampsRaw = { 1551929, 1594006, 1636085, 1930631, 6628, 48704, 1437284, 1942215, 18213, 60292 };


        [SetUp]
        public void Setup()
        { 
            sensorLIS2DW12 = new TestSensorLIS2DW12();
            sensorLIS2DW12.SetMode(SensorLIS2DW12.Mode.Low_Power_Mode);
            sensorLIS2DW12.SetSamplingRate(SensorLIS2DW12.LowPerformanceAccelSamplingRate.Freq_25Hz);

            sensorLSM6DS3 = new SensorLSM6DS3();
            sensorLSM6DS3.SetSamplingRate(SensorLSM6DS3.SamplingRate.Freq_52Hz);

            sensorGSR = new SensorGSR();
            sensorGSR.SetSamplingRate(SensorGSR.GSRRate.Freq_10_24Hz);

            sensorPPG = new SensorPPG();
            //sensorPPG.SetPPGGreenEnabled(true); //Setting this to true to prevent division by 0 in ParsePayloadData()
            sensorPPG.SetSamplingRate(SensorPPG.SamplingRate.Freq_200Hz);
        }

        [Test]
        public async Task TestGetShimmerTimestampUnwrapped()
        {
            var systemTimestamp = DateHelper.GetUnixTimestampMillis();
            var timestampUnwrapped = ((TestSensorLIS2DW12)sensorLIS2DW12).GetShimmerTimestampUnwrapped(TimestampsRaw[0], systemTimestamp);
            var systemTimestampOffsetRef = systemTimestamp - timestampUnwrapped;
            if (Math.Round(timestampUnwrapped, 4) != 47361.1145)
            {
                Assert.Fail();
            }

            timestampUnwrapped = ((TestSensorLIS2DW12)sensorLIS2DW12).GetShimmerTimestampUnwrapped(TimestampsRaw[1], systemTimestamp);
            if(Math.Round(timestampUnwrapped, 4) != 48645.2026)
            {
                Assert.Fail();
            }
            if (((TestSensorLIS2DW12)sensorLIS2DW12).GetSystemTimestampOffsetFirstTime() != systemTimestampOffsetRef)
            {
                Assert.Fail();
            }

            timestampUnwrapped = ((TestSensorLIS2DW12)sensorLIS2DW12).GetShimmerTimestampUnwrapped(TimestampsRaw[4], systemTimestamp);
            if (Math.Round(timestampUnwrapped, 4) != 60202.2705)
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public async Task TestUnwrapTimestamp()
        {
            foreach (var ts in TimestampsRaw)
            {
                double systemTsLastSampleMillis = DateHelper.GetUnixTimestampMillis();
                var ojcs = sensorLIS2DW12.GetShimmerTimestampUnwrapped(ts, systemTsLastSampleMillis);
            }

            //Check if there were 2 roll-overs (timestamp reset to 0/went backwards)
            if(((TestSensorLIS2DW12)sensorLIS2DW12).GetCurrentTimestampsTickCycle() == 2)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public async Task TestExtrapolateTimestampsAndAddToOjc()
        {
            List<Sensor> sensors = new List<Sensor>()
            {
                sensorLIS2DW12,
                sensorLSM6DS3,
                sensorGSR,
                sensorPPG
            };

            foreach (var sensor in sensors)
            {
                foreach (var ts in TimestampsRaw)
                {
                    List<ObjectCluster> ojcs = GetNewObjectClusters();
                    double systemTsLastSampleMillis = DateHelper.GetUnixTimestampMillis();
                    double tsLastSampleMillis = sensor.GetShimmerTimestampUnwrapped(ts, systemTsLastSampleMillis);
                    var samplingRate = Convert.ToDouble(sensor.GetSamplingRate().GetSettingsValue());
                    var numOfSamples = ojcs.Count;
                    int i = 0;
                    foreach (ObjectCluster ojc in ojcs)
                    {
                        sensor.ExtrapolateTimestampsAndAddToOjc(ojc, ts, tsLastSampleMillis, systemTsLastSampleMillis, numOfSamples, i, samplingRate);
                        i++;
                    }
                    bool res = TestTimestampsListOjcs(ojcs, Convert.ToDouble(sensor.GetSamplingRate().GetSettingsValue()));
                    if (!res)
                    {
                        Assert.Fail();
                    }
                }
            }

            Assert.Pass();
        }

        [Test]
        public async Task TestResetTimestamps()
        {
            foreach (var ts in TimestampsRaw)
            {
                sensorLIS2DW12.ParsePayloadData(new byte[60], uuid);
            }

            sensorLIS2DW12.ResetTimestamps();
            if(((TestSensorLIS2DW12)sensorLIS2DW12).GetCurrentTimestampsTickCycle() != 0)
            {
                Assert.Fail();
            }

            if (((TestSensorLIS2DW12)sensorLIS2DW12).GetLastReceivedTimestampTicksUnwrapped() != 0)
            {
                Assert.Fail();
            }

            if (((TestSensorLIS2DW12)sensorLIS2DW12).GetIsFirstTimeSystemTimestampOffsetStored() != false)
            {
                Assert.Fail();
            }

            if (((TestSensorLIS2DW12)sensorLIS2DW12).GetSystemTimestampOffsetFirstTime() != 0)
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        private bool TestTimestampsListOjcs(List<ObjectCluster> ojcs, double samplingRate)
        {
            List<string> ListTimestampsToTest = new List<string>
            {
                ShimmerConfiguration.SignalNames.TIMESTAMP,
                ShimmerConfiguration.SignalNames.SYSTEM_TIMESTAMP,
                ShimmerConfiguration.SignalNames.SYSTEM_TIMESTAMP_PLOT
            };

            double expectedDiff = (1 / samplingRate) * 1000;  //Expected difference between samples, in milliseconds
            foreach (var signalName in ListTimestampsToTest)
            {
                double lastTs = -1;
                foreach (var ojc in ojcs)
                {
                    double tsUnwrapped = ojc.GetData(signalName, ShimmerConfiguration.SignalFormats.CAL, ShimmerConfiguration.SignalUnits.MilliSeconds).Data;
                    if (lastTs != -1)
                    {
                        double diff = tsUnwrapped - lastTs;
                        if (Math.Round(diff, 2) != Math.Round(expectedDiff, 2))
                        {
                            return false;
                        }
                    }
                    lastTs = tsUnwrapped;
                }
            }

            return true;
        }

        private List<ObjectCluster> GetNewObjectClusters()
        {
            List<ObjectCluster> listOjcs = new List<ObjectCluster>();
            for(var i=0; i<10; i++)
            {
                listOjcs.Add(new ObjectCluster("", ""));
            }
            return listOjcs;
        }

    }
}
