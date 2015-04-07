using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShimmerAPI
{
    public class ObjectCluster
    {
        public int RawTimeStamp;
        private readonly String COMPort;
        private readonly String ShimmerID;
        private readonly List<String> SignalNames = new List<String>();
        private readonly List<String> Format = new List<String>();
        private readonly List<String> Units = new List<String>();
        private readonly List<Double> Data = new List<Double>();

        public ObjectCluster(String comPort, String shimmerId)
        {
            COMPort = comPort;
            ShimmerID = shimmerId;
        }

        public ObjectCluster(String comPort, String shimmerId, List<String> names, List<String> format, List<String> units, List<Double> data)
        {
            COMPort = comPort;
            ShimmerID = shimmerId;
            SignalNames = names;
            Format = format;
            Units = units;
            Data = data;
        }

        public ObjectCluster(String comPort, String shimmerId, List<String> names, List<String> format, List<String> units)
        {
            COMPort = comPort;
            ShimmerID = shimmerId;
            SignalNames = names;
            Format = format;
            Units = units;
        }

        public ObjectCluster(ObjectCluster obj)
        {
            double[] data = obj.GetData().ToArray();
            string[] names = obj.GetNames().ToArray();
            string[] formats = obj.GetFormats().ToArray();
            string[] units = obj.GetUnits().ToArray();
            Data = new List<double>();
            SignalNames = new List<String>();
            Format = new List<String>();
            Units = new List<String>();
            COMPort = obj.GetCOMPort();
            ShimmerID = obj.GetShimmerID();
            for (int count = 0; count < data.Length; count++)
            {
                Data.Add(obj.GetData()[count]);
                SignalNames.Add(obj.GetNames()[count]);
                Format.Add(obj.GetFormats()[count]);
                Units.Add(obj.GetUnits()[count]);
            }
        }

        public void Add(String name, String format, String unit, Double data)
        {
            SignalNames.Add(name);
            Format.Add(format);
            Units.Add(unit);
            Data.Add(data);
        }
        /// <summary>
        /// Get the index of the signal from the object cluster, returns -1 of not found
        /// </summary>
        /// <param name="name"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public int GetIndex(String name, String format)
        {
            int index = -1;
            for (int i = 0; i < SignalNames.Count; i++)
            {
                if (SignalNames[i].Equals(name) && Format[i].Equals(format))
                { 
                    index = i;
                }
            }
            return index;
        }

        public SensorData GetData(int index)
        {
            SensorData sensorData = new SensorData(Units[index], Data[index]);
            return sensorData;
        }

        public SensorData GetData(String name, String format)
        {
            SensorData sensorData = null;
            for (int i = 0; i < SignalNames.Count; i++)
            {
                if (SignalNames[i].Equals(name) && Format[i].Equals(format))
                {
                    sensorData = new SensorData(Units[i], Data[i]);
                }
            }
            return sensorData;
        }

        public String GetCOMPort()
        {
            return COMPort;
        }
        public String GetShimmerID()
        {
            return ShimmerID;
        }
        public List<String> GetNames()
        {
            return SignalNames;
        }
        public List<String> GetFormats()
        {
            return Format;
        }
        public List<String> GetUnits()
        {
            return Units;
        }
        public List<Double> GetData()
        {
            return Data;
        }
    }
}
