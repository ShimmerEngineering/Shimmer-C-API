using System;
using System.Collections.Generic;
using System.Text;

namespace ShimmerAPI.Sensors
{
    public interface AbstractSensor
    {
        int SENSOR_ID { get; }
        Dictionary<int, List<double[,]>> GetCalibDetails();

    }
}
