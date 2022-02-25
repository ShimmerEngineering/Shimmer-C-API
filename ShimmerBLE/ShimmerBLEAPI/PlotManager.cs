using System;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Xamarin.Forms;
using System.Threading;
using ShimmerAPI;
using shimmer.Helpers;

namespace ShimmerBLEAPI
{
    //TODO JY: Replace temp timestamps method with timestamps from VCBA-22
    public class PlotManager : AbstractPlotManager
    {
        public static string TimeFormat = "HH:mm:ss";

        private List<LineSeries> ListOfLineSeries = new List<LineSeries>();
        private PlotModel PlotModel;

        private int DataPointsMax = 500;
        private string PlotTitle = "";
        private string YAxisTitle = "";
        private string XAxisTitle = "";
        private string XAxisStringFormat = null;    //X-axis data unformatted by default
        private double YAxisRangeMin = double.NaN;  //Unlimited range by default
        private double YAxisRangeMax = double.NaN;
        private double StrokeThickness = 1.5;
        private bool EnableDownsampling = false;
        private double DownsamplingFactor = 0;

        private double CountDownsample = 0;

        /// <summary>
        /// Initialize a plot manager
        /// </summary>
        /// <param name="title"></param>
        /// <param name="yAxisTitle"></param>
        /// <param name="xAxisTitle"></param>
        public PlotManager(string title, string yAxisTitle, string xAxisTitle)
        {
            PlotTitle = title;
            YAxisTitle = yAxisTitle;
            XAxisTitle = xAxisTitle;
        }

        /// <summary>
        /// Initialize a plot manager
        /// </summary>
        /// <param name="title"></param>
        /// <param name="yAxisTitle"></param>
        /// <param name="xAxisTitle"></param>
        /// <param name="displayXAxisAsTime">X axis string format</param>
        public PlotManager(string title, string yAxisTitle, string xAxisTitle, bool displayXAxisAsTime)
        {
            PlotTitle = title;
            YAxisTitle = yAxisTitle;
            XAxisTitle = xAxisTitle;
            XAxisStringFormat = displayXAxisAsTime ? TimeFormat : null;
        }

        /// <summary>
        /// Call this to initialize the plot after modifying all necessary properties, e.g. PlotTitle, YAxisRange.
        /// </summary>
        /// <returns>PlotModel - assign this to a PlotView to display the plot</returns>
        public PlotModel BuildPlotModel()
        {
            PlotModel = new PlotModel { Title = PlotTitle };

            //Vertical Axes
            var linearAxis = new LinearAxis
            {
                Title = YAxisTitle,
                Position = AxisPosition.Left,
                IsZoomEnabled = false,
                Minimum = YAxisRangeMin,
                Maximum = YAxisRangeMax
            };
            PlotModel.Axes.Add(linearAxis);

            //Horizontal Axes
            var secondLinearAxis = new DateTimeAxis
            {
                Title = XAxisTitle,
                Position = AxisPosition.Bottom,
                IsZoomEnabled = false,
                StringFormat = XAxisStringFormat
            };
            PlotModel.Axes.Add(secondLinearAxis);

            PlotModel.InvalidatePlot(true);
            return PlotModel;
        }

        /// <summary>
        /// If downsampling is enabled, some data will not be plotted
        /// </summary>
        /// <param name="ojc">object cluster</param>
        public void FilterDataAndPlot(ObjectCluster ojc)
        {
            if (EnableDownsampling)
            {
                if (CountDownsample % DownsamplingFactor == 0)
                {
                    PlotOjc(ojc);
                }
                CountDownsample++;
            }
            else
            {
                PlotOjc(ojc);
            }
        }

        /// <summary>
        /// Plot data from the object cluster
        /// </summary>
        /// <param name="ojc">object cluster</param>
        private void PlotOjc(ObjectCluster ojc)
        {
            int count = 0;

            string deviceName = ojc.GetShimmerID();

            bool xRes = DictOfXAxis.TryGetValue(deviceName, out string[] xProperties);
            double xData = -1;
            if (xRes)
            {
                xData = ojc.GetData(xProperties[(int)SignalArrayIndex.Name], xProperties[(int)SignalArrayIndex.Format], xProperties[(int)SignalArrayIndex.Unit]).Data;
            }
            //Assuming the xData is a unix timestamp in millis
            double ts = DateTimeAxis.ToDouble(DateHelper.GetDateTimeFromUnixTimestampMillis(xData));

            foreach (string[] properties in ListOfPropertiesToPlot)
            {
                if (properties[(int)SignalArrayIndex.ShimmerID].Equals(deviceName))
                {
                    double yData = ojc.GetData(properties[(int)SignalArrayIndex.Name], properties[(int)SignalArrayIndex.Format], properties[(int)SignalArrayIndex.Unit]).Data;
                    LineSeries lineSeries = ListOfLineSeries[count];

                    if (lineSeries.Points.Count <= GetMaxDataPoints())
                    {
                        lineSeries.Points.Add(new DataPoint(ts, yData));
                    }
                    else if (lineSeries.Points.Count > GetMaxDataPoints())
                    {
                        lineSeries.Points.RemoveAt(0);
                        lineSeries.Points.Add(new DataPoint(ts, yData));
                    }
                }
                count++;
            }
            PlotModel.InvalidatePlot(true);
            Thread.Sleep(25);
        }

        /// <summary>
        /// Add signal to plot using random color
        /// </summary>
        /// <param name="signal">signal to be add</param>
        public void AddSignalToPlotRandomColor(string[] signal)
        {
            int[] color = GenerateRandomColor();
            LineSeries lineSeries = new LineSeries
            {
                Title = signal[(int)SignalArrayIndex.Name] + " " + signal[(int)SignalArrayIndex.Format],
                Color = OxyColor.FromRgb((byte)color[0], (byte)color[1], (byte)color[2]),
                StrokeThickness = StrokeThickness,
            };
            ListOfLineSeries.Add(lineSeries);
            PlotModel.Series.Add(lineSeries);

            AddSignalAndColor(signal, color);
        }

        /// <summary>
        /// Add signal to plot using default color
        /// </summary>
        /// <param name="signal">signal to be add</param>
        public void AddSignalToPlotDefaultColors(string[] signal)
        {
            AddSignalUseDefaultColors(signal);
            //Get the latest color added
            int[] color = ListOfTraceColorsCurrentlyUsed[ListOfTraceColorsCurrentlyUsed.Count - 1];
            LineSeries lineSeries = new LineSeries
            {
                Title = signal[(int)SignalArrayIndex.Name] + " " + signal[(int)SignalArrayIndex.Format],
                Color = OxyColor.FromRgb((byte)color[0], (byte)color[1], (byte)color[2]),
                StrokeThickness = StrokeThickness,
            };
            ListOfLineSeries.Add(lineSeries);
            PlotModel.Series.Add(lineSeries);
        }

        /// <summary>
        /// Remove signal from plot
        /// </summary>
        /// <param name="signal">signal to be remove</param>
        public void RemoveSignalFromPlot(string[] signal)
        {
            int index = FindSignalIndex(signal);
            if (index != -1)
            {
                ListOfLineSeries.RemoveAt(index);
                PlotModel.Series.RemoveAt(index);
                PlotModel.InvalidatePlot(true);
                RemoveSignal(index);
            }
            else
            {
                Console.WriteLine("WARNING: Unable to find index for signal: " + signal);
            }
        }

        /// <summary>
        /// Empty the plot
        /// </summary>
        public void RemoveAllSignalsFromPlot()
        {
            ListOfLineSeries.Clear();
            PlotModel.Series.Clear();
            PlotModel.InvalidatePlot(true);
            RemoveAllSignals();
        }

        /// <summary>
        /// Maximum number of data points on the plot
        /// </summary>
        public void SetMaxDataPoints(int points)
        {
            DataPointsMax = points;
        }
        /// <summary>
        /// Maximum number of data points on the plot
        /// </summary>
        public int GetMaxDataPoints()
        {
            return DataPointsMax;
        }

        /// <summary>
        /// Plot title
        /// </summary>
        public void SetPlotTitle(string title)
        {
            PlotTitle = title;
        }
        /// <summary>
        /// Plot title
        /// </summary>
        public string GetPlotTitle()
        {
            return PlotTitle;
        }

        /// <summary>
        /// Y axis title
        /// </summary>
        public void SetYAxisTitleLeft(string title)
        {
            YAxisTitle = title;
        }
        /// <summary>
        /// Y axis title
        /// </summary>
        public string GetYAxisTitleLeft()
        {
            return YAxisTitle;
        }

        /// <summary>
        /// X axis title
        /// </summary>
        public void SetXAxisTitleBottom(string title)
        {
            XAxisTitle = title;
        }
        /// <summary>
        /// X axis title
        /// </summary>
        public string GetXAxisTitleBottom()
        {
            return XAxisTitle;
        }

        /// <summary>
        /// X axis string format
        /// </summary>
        public void SetXAxisStringFormat(string format)
        {
            XAxisStringFormat = format;
        }
        /// <summary>
        /// X axis string format
        /// </summary>
        public string GetXAxisStringFormat()
        {
            return XAxisStringFormat;
        }

        /// <summary>
        /// Y axis minimum range
        /// </summary>
        public void SetYAxisRangeMin(double value)
        {
            YAxisRangeMin = value;
        }
        /// <summary>
        /// Y axis minimum range
        /// </summary>
        public double GetYAxisRangeMin()
        {
            return YAxisRangeMin;
        }

        /// <summary>
        /// Y axis maximum range
        /// </summary>
        public void SetYAxisRangeMax(double value)
        {
            YAxisRangeMax = value;
        }
        /// <summary>
        /// Y axis maximum range
        /// </summary>
        public double GetYAxisRangeMax()
        {
            return YAxisRangeMax;
        }

        /// <summary>
        /// Stroke thickness
        /// </summary>
        public void SetStrokeThickness(double value)
        {
            StrokeThickness = value;
        }
        /// <summary>
        /// Stroke thickness
        /// </summary>
        public double GetStrokeThickness()
        {
            return StrokeThickness;
        }

        /// <summary>
        /// Enables or disables downsampling
        /// </summary>
        public void SetEnableDownsampling(bool value)
        {
            EnableDownsampling = value;
        }
        /// <summary>
        /// Enables or disables downsampling
        /// </summary>
        public bool GetEnableDownsampling()
        {
            return EnableDownsampling;
        }

        /// <summary>
        /// Downsampling factor
        /// </summary>
        public void SetDownsamplingFactor(double factor)
        {
            DownsamplingFactor = factor;
        }
        /// <summary>
        /// Downsampling factor
        /// </summary>
        public double GetDownsamplingFactor()
        {
            return DownsamplingFactor;
        }
    }
}