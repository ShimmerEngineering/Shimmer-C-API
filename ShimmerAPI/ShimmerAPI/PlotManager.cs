using System;
using System.Collections.Generic;
using System.Text;

namespace ShimmerAPI
{
    class PlotManager
    {
        public static List<int[]> ListOfTraceColorsCurrentlyUsed = new List<int[]>();

        public static List<int[]> ListOfTraceColorsDefault = new List<int[]>()
        {
            //TODO fill in colors here, see AbstractPlotManager.java
        };

        public PlotManager()
        {

        }

        public void AddSignalGenerateRandomColor(string[] channelStringArray)
        {
            AddSignal(channelStringArray);
            ListOfTraceColorsCurrentlyUsed.Add(GenerateRandomColor());
        }

        public void FilterDataAndPlot(ObjectCluster ojc)
        {
            //TODO fill in logic here, see BasicPlotManagerPC.java
        }

        private void AddSignal(string[] channelStringArray)
        {
            //TODO fill in logic here, see AbstractPlotManager.java
        }

        private int[] GenerateRandomColor()
        {
            //TODO fill in logic here, see AbstractPlotManager.java
            return null;
        }
    }
}
