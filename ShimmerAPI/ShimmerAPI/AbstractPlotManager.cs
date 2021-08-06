using System;
using System.Collections.Generic;
using System.Text;

namespace ShimmerAPI
{
    abstract class AbstractPlotManager
    {
        //Add the methods from https://github.com/ShimmerEngineering/Shimmer-Java-Android-API/blob/Dev/ShimmerDriver/src/main/java/com/shimmerresearch/guiUtilities/AbstractPlotManager.java
        //Into this class
        //Note you may need to change some of the code to migrate it from Java to C#
        public static List<int[]> ListOfTraceColorsCurrentlyUsed = new List<int[]>();

        public static List<int[]> ListOfTraceColorsDefault = new List<int[]>()
        {
            //TODO fill in colors here, see AbstractPlotManager.java

            // Shimmer Orange
            new int[] { 241, 93, 34 },//------colourShimmerOrange
            new int[] { 153, 76, 0 },//-------colourBrown
            new int[] { 0, 153, 153 },//------colourCyanAqua
            new int[] { 102, 0, 204 },//------colourPurple
            new int[] { 102, 0, 0 },//--------colourMaroon
            new int[] { 0, 153, 76 },//-------colourGreen
            // Shimmer Grey
            new int[] { 119, 120, 124 },//----colourShimmerGrey
            // Shimmer Blue
            new int[] { 0, 129, 198 },//------colourShimmerBlue

            new int[] { 255, 0, 0 }//---------colourLightRed
        };

    }
}
