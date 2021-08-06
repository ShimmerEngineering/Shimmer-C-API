using System;
using System.Collections.Generic;
using System.Text;
using ShimmerAPI.Utilities;

namespace ShimmerAPI
{
    abstract class AbstractPlotManager: UtilShimmer
    {
        //Add the methods from https://github.com/ShimmerEngineering/Shimmer-Java-Android-API/blob/Dev/ShimmerDriver/src/main/java/com/shimmerresearch/guiUtilities/AbstractPlotManager.java
        //Into this class
        //Note you may need to change some of the code to migrate it from Java to C#
        public static List<int[]> ListOfTraceColorsCurrentlyUsed = new List<int[]>();

        private static List<byte[]> listOfTraceColorsDefault = new List<byte[]>()
        {
            //TODO fill in colors here, see AbstractPlotManager.java
            SHIMMER_DEFAULT_COLOURS.colourShimmerOrange,
            SHIMMER_DEFAULT_COLOURS.colourBrown,
            SHIMMER_DEFAULT_COLOURS.colourCyanAqua,
            SHIMMER_DEFAULT_COLOURS.colourPurple,
            SHIMMER_DEFAULT_COLOURS.colourMaroon,
            SHIMMER_DEFAULT_COLOURS.colourGreen,
            SHIMMER_DEFAULT_COLOURS.colourShimmerGrey,
            SHIMMER_DEFAULT_COLOURS.colourShimmerBlue
        };
    }
}
