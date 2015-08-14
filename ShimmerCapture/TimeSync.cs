using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.Statistics;

namespace ShimmerAPI
{
    public class TimeSync
    {
        int BufferSize = 10;
        private List<Double> DataPoints = new List<Double>();
        
        public TimeSync(int bufferSize)
        {
            BufferSize = bufferSize;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double CalculateTimeSync(double shimmertimestamp, double systemtimestamp)
        {
            double offset = systemtimestamp - shimmertimestamp;
            DataPoints.Add(offset);
            if (DataPoints.Count == BufferSize)
            {
                double minOffset = DataPoints.Min();
                double synctimestamp = shimmertimestamp + minOffset;
                DataPoints.RemoveAt(0);
                return synctimestamp;
            }
            else
            {
                return Double.NaN;
            }

        }

    }
}
