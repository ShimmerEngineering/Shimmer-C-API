using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ShimmerAPI;
using ShimmerAPI.Utilities;

namespace ShimmerBLEAPI
{
    /// <summary>
    /// Base class for plot manager
    /// </summary>
    public abstract class AbstractPlotManager
    {
        public SynchronizedCollection<string[]> ListOfPropertiesToPlot = new SynchronizedCollection<string[]>();
        protected ConcurrentDictionary<string, string[]> DictOfXAxis = new ConcurrentDictionary<string, string[]>();
        
        public SynchronizedCollection<int[]> ListOfTraceColorsCurrentlyUsed = new SynchronizedCollection<int[]>();
        public static List<byte[]> ListOfTraceColorsDefault = new List<byte[]>()
        {
            UtilShimmer.SHIMMER_DEFAULT_COLOURS.colourShimmerOrange,
            UtilShimmer.SHIMMER_DEFAULT_COLOURS.colourBrown,
            UtilShimmer.SHIMMER_DEFAULT_COLOURS.colourCyanAqua,
            UtilShimmer.SHIMMER_DEFAULT_COLOURS.colourPurple,
            UtilShimmer.SHIMMER_DEFAULT_COLOURS.colourMaroon,
            UtilShimmer.SHIMMER_DEFAULT_COLOURS.colourGreen,
            UtilShimmer.SHIMMER_DEFAULT_COLOURS.colourShimmerGrey,
            UtilShimmer.SHIMMER_DEFAULT_COLOURS.colourShimmerBlue
        };

        public enum SignalArrayIndex
        {
            ShimmerID = 0,
            Name = 1,
            Format = 2,
            Unit = 3,
            Calibration = 4
        }

        public AbstractPlotManager()
        {

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
            DictOfXAxis.Clear();
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

        protected void AddSignalUseDefaultColors(string[] channelStringArray)
        {
            try
            {
                AddSignal(channelStringArray);
                bool mFound = false;
                int[] newColorToAdd = null;
                if (ListOfTraceColorsCurrentlyUsed.Count > 0)
                {
                    IEnumerator<byte[]> entries = ListOfTraceColorsDefault.GetEnumerator();
                    while (entries.MoveNext())
                    {
                        int[] rgbdefaultC = Array.ConvertAll(entries.Current, c => (int)c);
                        mFound = false;

                        foreach (int[] rgbp in ListOfTraceColorsCurrentlyUsed)
                        {
                            if (rgbdefaultC[0] == rgbp[0] && rgbdefaultC[1] == rgbp[1] && rgbdefaultC[2] == rgbp[2])
                            {
                                mFound = true;
                            }
                        }

                        if (mFound != true)
                        {
                            newColorToAdd = rgbdefaultC;
                        }
                    }
                }
                else
                {
                    newColorToAdd = Array.ConvertAll(ListOfTraceColorsDefault[0], c => (int)c);
                }

                if (newColorToAdd != null)
                {
                    ListOfTraceColorsCurrentlyUsed.Add(newColorToAdd);
                }
                else
                {
                    ListOfTraceColorsCurrentlyUsed.Add(GenerateRandomColor());
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }


        protected void AddSignalAndColor(string[] channelStringArray, int[] color)
        {
            AddSignalAndUseFixedColor(channelStringArray, color);
        }

        protected void AddSignal(string[] channelStringArray)
        {
            ListOfPropertiesToPlot.Add(channelStringArray);
        }

        /// <summary>
        /// Add x-axis to <see cref="DictOfXAxis"/> if it does not already exist
        /// </summary>
        /// <param name="channelStringArray"></param>
        public void AddXAxis(string[] channelStringArray)
        {
            string deviceName = channelStringArray[(int)SignalArrayIndex.ShimmerID];
            bool res = DictOfXAxis.TryAdd(deviceName, channelStringArray);
            if(!res)
            {
                Console.WriteLine("WARNING: Unable to add X axis as it already exists for key: " + deviceName);
            }
        }

        /// <summary>
        /// Remove x-axis from <see cref="DictOfXAxis"/>
        /// </summary>
        /// <param name="key"></param>
        public void RemoveXAxis(string key)
        {
            bool res = DictOfXAxis.TryRemove(key, out var val); 
            if (!res)
            {
                Console.WriteLine("WARNING: Unable to remove X axis for key: " + key);
            }
        }

        /// <summary>
        /// Returns index of signal in <see cref="ListOfPropertiesToPlot"/>
        /// </summary>
        /// <param name="signalToFind"></param>
        public int FindSignalIndex(string[] signalToFind)
        {
            int res = ListOfPropertiesToPlot.IndexOf(signalToFind);
            return res;
        }

        /// <summary>
        /// Returns true if signal exists in <see cref="ListOfPropertiesToPlot"/>
        /// </summary>
        /// <param name="signal"></param>
        public bool CheckSignalExists(string[] signal)
        {
            foreach(var property in ListOfPropertiesToPlot)
            {
                if(property.SequenceEqual(signal))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns signal properties from the object cluster provided
        /// </summary>
        /// <param name="ojc"></param>
        /// <returns>List of signal properties</returns>
        public List<string[]> GetAllSignalPropertiesFromOjc(ObjectCluster ojc)
        {
            List<string[]> signals = new List<string[]>();
            for(var i=0; i<ojc.GetNames().Count; i++)
            {
                string[] signal = new string[Enum.GetNames(typeof(SignalArrayIndex)).Length];
                signal[(int)SignalArrayIndex.ShimmerID] = ojc.GetShimmerID();
                signal[(int)SignalArrayIndex.Name] = ojc.GetNames()[i];
                signal[(int)SignalArrayIndex.Format] = ojc.GetFormats()[i];
                signal[(int)SignalArrayIndex.Unit] = ojc.GetUnits()[i];
                //signal[(int)SignalArrayIndex.Calibration] = "*";
                signals.Add(signal);
            }
            return signals;
        }

        /// <summary>
        /// Returns random color as rgb array
        /// </summary>
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
