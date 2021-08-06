using System;
using System.Collections.Generic;
using System.Text;
using ShimmerAPI.Utilities;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Xamarin.Forms;

namespace ShimmerAPI
{
    class PlotManager
    {
        public static List<int[]> ListOfTraceColorsCurrentlyUsed = new List<int[]>();

        private static List<byte[]> listOfTraceColorsDefault = new List<byte[]>()
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

        LineSeries PLOT_LINE_STYLE_X = new LineSeries
        {
            Title = "X",
            Color = OxyColor.FromRgb(listOfTraceColorsDefault[0][0], listOfTraceColorsDefault[0][1], listOfTraceColorsDefault[0][2]),
            StrokeThickness = 1.5,
        };

        LineSeries PLOT_LINE_STYLE_Y = new LineSeries
        {
            Title = "Y",
            Color = OxyColor.FromRgb(listOfTraceColorsDefault[5][0], listOfTraceColorsDefault[5][1], listOfTraceColorsDefault[5][2]),
            StrokeThickness = 1.5,
        };

        LineSeries PLOT_LINE_STYLE_Z = new LineSeries
        {
            Title = "Z",
            Color = OxyColor.FromRgb(listOfTraceColorsDefault[7][0], listOfTraceColorsDefault[7][1], listOfTraceColorsDefault[7][2]),
            StrokeThickness = 1.5,
        };

        public double xAxes { get; set; }
        public double yAxes { get; set; }
        public double zAxes { get; set; }
        public long timestampMilis { get; set; }
        public int dataPointsMax { get; set; }

        public PlotManager(PlotView plotView)
        {
            var plotModel = new PlotModel { Title = "Plot XYZ" };

            //Vertical Axes
            var linearAxis = new LinearAxis();
            linearAxis.Title = "Data Points";
            linearAxis.Position = AxisPosition.Left;
            linearAxis.IsZoomEnabled = false;
            plotModel.Axes.Add(linearAxis);

            //Horizontal Axes
            var secondLinearAxis = new LinearAxis();
            secondLinearAxis.Title = "Timestamp";
            secondLinearAxis.Position = AxisPosition.Bottom;
            secondLinearAxis.IsZoomEnabled = false;
            plotModel.Axes.Add(secondLinearAxis);

            plotView.Model = plotModel;
        }

        public void AddSignalGenerateRandomColor(string[] channelStringArray)
        {
            AddSignal(channelStringArray);
            ListOfTraceColorsCurrentlyUsed.Add(GenerateRandomColor());
        }

        public void FilterDataAndPlot(PlotView plotView)
        {
            //TODO fill in logic here, see BasicPlotManagerPC.java
            PlotModel plotViewModel = plotView.Model;

            DataPoint xPoint = new DataPoint(timestampMilis, xAxes);
            DataPoint yPoint = new DataPoint(timestampMilis, yAxes);
            DataPoint zPoint = new DataPoint(timestampMilis, zAxes);

            plotViewModel.Series.Clear();

            if (PLOT_LINE_STYLE_X.Points.Count <= dataPointsMax)
            {
                PLOT_LINE_STYLE_X.Points.Add(xPoint);

                PLOT_LINE_STYLE_Y.Points.Add(yPoint);

                PLOT_LINE_STYLE_Z.Points.Add(zPoint);
            }
            else if (PLOT_LINE_STYLE_X.Points.Count > dataPointsMax)
            {
                PLOT_LINE_STYLE_X.Points.RemoveAt(0);
                PLOT_LINE_STYLE_X.Points.Add(xPoint);

                PLOT_LINE_STYLE_Y.Points.RemoveAt(0);
                PLOT_LINE_STYLE_Y.Points.Add(yPoint);

                PLOT_LINE_STYLE_Z.Points.RemoveAt(0);
                PLOT_LINE_STYLE_Z.Points.Add(zPoint);
            }

            plotViewModel.Series.Add(PLOT_LINE_STYLE_X);
            plotViewModel.Series.Add(PLOT_LINE_STYLE_Y);
            plotViewModel.Series.Add(PLOT_LINE_STYLE_Z);

            plotViewModel.InvalidatePlot(true);
        }

        private void AddSignal(string[] channelStringArray)
        {
            //TODO fill in logic here, see AbstractPlotManager.java
        }

        private int[] GenerateRandomColor()
        {
            //TODO fill in logic here, see AbstractPlotManager.java
            Random rand = new Random();
            int min = 0;
            int max = 255;
            int[] rgb = new int[3];
            rgb[0] = rand.Next((max - min) + 1) + min;
            rgb[1] = rand.Next((max - min) + 1) + min;
            rgb[2] = rand.Next((max - min) + 1) + min;
            return rgb;
        }
    }
}
