using System;
using System.Collections.Generic;
using System.Text;
using ShimmerAPI.Utilities;

namespace ShimmerAPI
{
    public abstract class AbstractPlotManager
    {
        //Add the methods from https://github.com/ShimmerEngineering/Shimmer-Java-Android-API/blob/Dev/ShimmerDriver/src/main/java/com/shimmerresearch/guiUtilities/AbstractPlotManager.java
        //Into this class
        //Note you may need to change some of the code to migrate it from Java to C#
        public static List<int[]> ListOfTraceColorsCurrentlyUsed = new List<int[]>();

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
    }
}
