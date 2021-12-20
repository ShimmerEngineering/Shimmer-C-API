using shimmer.Sensors;
using ShimmerAPI;
using System.Collections.Generic;

namespace ShimmerBLETests.Sensors
{
    public class TestSensorLIS2DW12 : SensorLIS2DW12
    {
        public double GetCurrentTimestampsTickCycle()
        {
            return CurrentTimestampTicksCycle;
        }

        public double GetLastReceivedTimestampTicksUnwrapped()
        {
            return LastReceivedTimestampTicksUnwrapped;
        }

        public double GetSystemTimestampOffsetFirstTime()
        {
            return SystemTimestampOffsetFirstTime;
        }

        public bool GetIsFirstTimeSystemTimestampOffsetStored()
        {
            return IsFirstTimeSystemTimestampOffsetStored;
        }

    }
}
