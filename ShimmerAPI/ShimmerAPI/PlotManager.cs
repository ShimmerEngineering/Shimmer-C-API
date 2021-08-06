using System;
using System.Collections.Generic;
using System.Text;
using OxyPlot;
using OxyPlot.Xamarin.Forms;
using OxyPlot.Axes;
using OxyPlot.Reporting;
using OxyPlot.Series;
using OxyPlot.Annotations;

namespace ShimmerAPI
{
    class PlotManager
    {
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

        LineSeries PLOT_LINE_STYLE_X = new LineSeries
        {
            Title = "X",
            Color = OxyColors.Red,
            StrokeThickness = 1.5,
        };

        LineSeries PLOT_LINE_STYLE_Y = new LineSeries
        {
            Title = "Y",
            Color = OxyColors.Green,
            StrokeThickness = 1.5,
        };

        LineSeries PLOT_LINE_STYLE_Z = new LineSeries
        {
            Title = "Z",
            Color = OxyColors.Blue,
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

            PlotModel plotViewModel = plotView.Model;
        }

        public void AddSignalGenerateRandomColor(string[] channelStringArray)
        {
            AddSignal(channelStringArray);
            ListOfTraceColorsCurrentlyUsed.Add(GenerateRandomColor());
        }

        public void FilterDataAndPlot(PlotView plotView)
        {
            //TODO fill in logic here, see BasicPlotManagerPC.java

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

            //PLOT_LINE_STYLE_X.Points.Add(xPoint);
            //PLOT_LINE_STYLE_Y.Points.Add(yPoint);
            //PLOT_LINE_STYLE_Z.Points.Add(zPoint);

            //plotViewModel.Series.Add(PLOT_LINE_STYLE_X);
            //plotViewModel.Series.Add(PLOT_LINE_STYLE_Y);
            //plotViewModel.Series.Add(PLOT_LINE_STYLE_Z);

            //plotViewModel.InvalidatePlot(true);
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
