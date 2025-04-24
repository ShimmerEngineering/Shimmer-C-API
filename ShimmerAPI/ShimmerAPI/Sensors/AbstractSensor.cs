using System;
using System.Collections.Generic;
using System.Text;

namespace ShimmerAPI.Sensors
{
    public interface AbstractSensor
    {
        int SENSOR_ID { get; }
        int ShimmerHardwareVersion { get; }
        Dictionary<int, List<double[,]>> CalibDetails { get; }

    }
}
