using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShimmerAPI
{
    public class SensorData
    {
        public String Unit;
        public Double Data;

        public SensorData(String unit, Double data)
        {
            Unit = unit;
            Data = data;
        }

        public String GetUnit()
        {
            return Unit;
        }
        public Double GetData()
        {
            return Data;
        }
    }
}
