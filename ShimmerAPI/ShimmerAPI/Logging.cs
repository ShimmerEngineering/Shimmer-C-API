using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace ShimmerAPI
{
    public class Logging
    {
        private StreamWriter PCsvFile = null;
        private String FileName;
        private String Delimeter = ",";
        private Boolean FirstWrite = true;

        public Logging(String fileName, String delimeter){
            Delimeter = delimeter;
            FileName = fileName;
            try
            {
                PCsvFile = new StreamWriter(FileName, false);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
	    }

        public void WriteData(ObjectCluster obj)
        {
            if (FirstWrite)
            {
                WriteHeader(obj);
                FirstWrite = false;
            }
            Double[] data = obj.GetData().ToArray();
            for (int i = 0; i < data.Length; i++)
            {
                PCsvFile.Write(data[i].ToString() + Delimeter);
            }
            PCsvFile.WriteLine();
        }

        private void WriteHeader(ObjectCluster obj)
        {
            ObjectCluster objectCluster = new ObjectCluster(obj);
            List<String> names = objectCluster.GetNames();
            List<String> formats = objectCluster.GetFormats();
            List<String> units = objectCluster.GetUnits();
            List<Double> data = objectCluster.GetData();
            String deviceId = objectCluster.GetShimmerID();

            for (int i = 0; i < data.Count; i++)
            {
                PCsvFile.Write(deviceId + Delimeter);
            }
            PCsvFile.WriteLine();
            for (int i = 0; i < data.Count; i++)
            {
                PCsvFile.Write(names[i] + Delimeter);
            }
            PCsvFile.WriteLine();
            for (int i = 0; i < data.Count; i++)
            {
                PCsvFile.Write(formats[i] + Delimeter);
            }
            PCsvFile.WriteLine();
            for (int i = 0; i < data.Count; i++)
            {
                PCsvFile.Write(units[i] + Delimeter);
            }
            PCsvFile.WriteLine();
        }

        public void CloseFile()
        {
            PCsvFile.Close();
        }
    }
}
