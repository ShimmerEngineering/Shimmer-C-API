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

        protected Dictionary<string, double> MapofXAxisGeneratedValue = new Dictionary<string, double>();

        public List<int[]> ListOfTraceColorsCurrentlyUsed = new List<int[]>();

        public static List<byte[]> listOfTraceColorsDefault = new List<byte[]>()
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

        public AbstractPlotManager(List<String[]> propertiestoPlot)
        {
            ListOfPropertiesToPlot = propertiestoPlot;
            ListOfTraceColorsCurrentlyUsed = generateRandomColorList(ListOfPropertiesToPlot.Count);
        }

        public AbstractPlotManager(List<String[]> propertiestoPlot, List<int[]> listOfColors)
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

    }
}
