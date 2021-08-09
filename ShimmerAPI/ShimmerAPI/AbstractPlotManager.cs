using System;
using System.Collections.Generic;
using System.Text;
using ShimmerAPI.Utilities;

namespace ShimmerAPI
{
    public abstract class AbstractPlotManager
    {
        public List<string[]> ListOfPropertiesToPlot = new List<string[]>();

        protected Dictionary<string, string[]> MapOfXAxis = new Dictionary<string, string[]>();
        protected Dictionary<string, string[]> MapOfYAxis = new Dictionary<string, string[]>();
        protected Dictionary<string, string[]> MapOfZAxis = new Dictionary<string, string[]>();

        protected Dictionary<string, double> MapofXAxisGeneratedValue = new Dictionary<string, double>();
        protected Dictionary<string, double> MapofYAxisGeneratedValue = new Dictionary<string, double>();
        protected Dictionary<string, double> MapofZAxisGeneratedValue = new Dictionary<string, double>();

        public List<int[]> ListOfTraceColorsCurrentlyUsed = new List<int[]>();

        public static List<byte[]> ListOfTraceColorsDefault = new List<byte[]>()
        {
            //TODO fill in colors here, see AbstractPlotManager.java
            UtilShimmer.SHIMMER_DEFAULT_COLOURS.colourShimmerOrange,
            UtilShimmer.SHIMMER_DEFAULT_COLOURS.colourBrown,
            UtilShimmer.SHIMMER_DEFAULT_COLOURS.colourCyanAqua,
            UtilShimmer.SHIMMER_DEFAULT_COLOURS.colourPurple,
            UtilShimmer.SHIMMER_DEFAULT_COLOURS.colourMaroon,
            UtilShimmer.SHIMMER_DEFAULT_COLOURS.colourGreen,
            UtilShimmer.SHIMMER_DEFAULT_COLOURS.colourShimmerGrey,
            UtilShimmer.SHIMMER_DEFAULT_COLOURS.colourShimmerBlue
        };

        public AbstractPlotManager()
        {

        }

        public AbstractPlotManager(List<string[]> propertiestoPlot)
        {
            ListOfPropertiesToPlot = propertiestoPlot;
            ListOfTraceColorsCurrentlyUsed = GenerateRandomColorList(ListOfPropertiesToPlot.Count);
        }

        public AbstractPlotManager(List<string[]> propertiestoPlot, List<int[]> listOfColors)
        {
            ListOfPropertiesToPlot = propertiestoPlot;
            ListOfTraceColorsCurrentlyUsed = listOfColors;
        }

        protected void RemoveSignal(int index)
        {
            ListOfPropertiesToPlot.RemoveAt(index);
            if (ListOfTraceColorsCurrentlyUsed.Count > index)
            {
                ListOfTraceColorsCurrentlyUsed.RemoveAt(index);
            }
        }

        protected void RemoveAllSignals()
        {
            ListOfPropertiesToPlot.Clear();
            ListOfTraceColorsCurrentlyUsed.Clear();
        }

        protected void AddSignalGenerateRandomColor(string[] channelStringArray)
        {
            AddSignalAndUseFixedColor(channelStringArray, GenerateRandomColor());
        }

        protected void AddSignalAndUseFixedColor(string[] channelStringArray, int[] rgb)
        {
            AddSignal(channelStringArray);
            ListOfTraceColorsCurrentlyUsed.Add(rgb);
        }

        protected void AddSignalAndColor(string[] channelStringArray, int[] color)
        {
            AddSignalAndUseFixedColor(channelStringArray, color);
        }

        protected void AddSignal(string[] channelStringArray)
        {
            ListOfPropertiesToPlot.Add(channelStringArray);
        }

        public List<string[]> GetAllSignalPropertiesFromOjc(ObjectCluster ojc)
        {
            List<string[]> signals = new List<string[]>();
            for(var i=0; i<ojc.GetNames().Count; i++)
            {
                string[] signal = new string[5];
                signal[0] = ojc.GetShimmerID();
                signal[1] = ojc.GetNames()[i];
                signal[2] = ojc.GetFormats()[i];
                signal[3] = ojc.GetUnits()[i];
                signal[4] = "*";
                signals.Add(signal);
            }
            return signals;
        }

        public void AddXAxis(string[] channelStringArray)
        {
            string deviceName = channelStringArray[0];
            MapOfXAxis.Add(deviceName, channelStringArray);
        }

        public void AddYAxis(string[] channelStringArray)
        {
            string deviceName = channelStringArray[0];
            MapOfYAxis.Add(deviceName, channelStringArray);
        }

        public void AddZAxis(string[] channelStringArray)
        {
            string deviceName = channelStringArray[0];
            MapOfZAxis.Add(deviceName, channelStringArray);
        }

        public static int[] GenerateRandomColor()
        {
            Random rand = new Random();
            int min = 0;
            int max = 255;
            int[] rgb = new int[3];
            rgb[0] = rand.Next((max - min) + 1) + min;
            rgb[1] = rand.Next((max - min) + 1) + min;
            rgb[2] = rand.Next((max - min) + 1) + min;
            return rgb;
        }

        protected static List<int[]> GenerateRandomColorList(int size)
        {
            List<int[]> listofSignalColors = new List<int[]>();
            for (int i = 0; i < size; i++)
            {
                listofSignalColors.Add(GenerateRandomColor());
            }
            return listofSignalColors;
        }

    }
}
