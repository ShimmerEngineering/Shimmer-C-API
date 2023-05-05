using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using shimmer.Communications;
using shimmer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using static shimmer.Models.ShimmerBLEEventData;

namespace JointCorpWatch
{
    public partial class MainPage : ContentPage
    {
        LineSeries lineSeries;
        PlotModel model;
        JCWatch watch;
        public MainPage()
        {
            InitializeComponent();
            plotView.Model = BuildPlotModel();
            Connect();
        }

        public PlotModel BuildPlotModel()
        {
            model = new PlotModel { Title = "ECG" };

            //Vertical Axes
            var linearAxis = new LinearAxis
            {
                Title = "ECG",
                Position = AxisPosition.Left,
                IsZoomEnabled = false,
                Minimum = -15000,
                Maximum = 15000
            };
            model.Axes.Add(linearAxis);

            //Horizontal Axes
            //var secondLinearAxis = new DateTimeAxis
            //{
            //    Title = "time",
            //    Position = AxisPosition.Bottom,
            //    IsZoomEnabled = false
            //};
            //model.Axes.Add(secondLinearAxis);


            //model.Series.Points.Add(new DataPoint(0, 0));
            lineSeries = new LineSeries
            {
                Title = "ECG",
                Color = OxyColor.FromRgb(100, 100, 100),
            };
            model.Series.Add(lineSeries);

            model.InvalidatePlot(true);
            return model;
        }

        public async void Connect()
        {
            watch = new JCWatch("00000000-0000-0000-0000-03020205feaa", true);
            //watch = new JCWatch("00000000-0000-0000-0000-03020205fbb0", true);
            //watch = new JCWatch("00000000-0000-0000-0000-04020205bb0f", true);
            //JCWatch watch = new JCWatch("00000000-0000-0000-0000-daa619f04ad7");
            watch.ShimmerBLEEvent += ShimmerDevice_BLEEvent;
            await watch.Connect();
        }

        protected void ShimmerDevice_BLEEvent(object sender, ShimmerBLEEventData e)
        {
            if (e.CurrentEvent == VerisenseBLEEvent.StateChange)
            {
            }
            else if (e.CurrentEvent == VerisenseBLEEvent.NewDataPacket)
            {
                JCWatchEvent ojc = (JCWatchEvent)e.ObjMsg;
                if (ojc.Identifier == JCWatchDeviceConstant.CMD_Get_Address)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        macAddress.Text = "Connect success + " + ojc.Message;
                    });
                }
                else if (ojc.Identifier == JCWatchDeviceConstant.GetECGwaveform)
                {
                    if (ojc.End)
                    {
                        Debug.WriteLine("End ECG Data");
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DateTime now = DateTime.Now;
                            temp2.Text = now.ToString("F") + " " + ojc.Message;
                        });
                        if (RT)
                        {
                            Thread.Sleep(int.Parse(ojc.Data["Duration"]));

                        }
                        byte[] readHistory = { 0x71, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x71 };
                        watch.WriteBytes(readHistory);
                    }

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        temp3.Text = JCWatch.MyDictionaryToJson(ojc.Data);
                    });
                }
                else if (ojc.Identifier == JCWatchDeviceConstant.CMD_ECGDATA)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DateTime foo = DateTime.Now;
                        long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();
                        temp2.Text = JCWatch.MyDictionaryToJson(ojc.Data);
                        int[] data = ojc.Data["ECGValue"].Split(',').Select(Int32.Parse).ToArray();
                        for (int i = 0; i < data.Length; i++)
                        {
                            if (lineSeries.Points.Count <= 60)
                            {
                                //lineSeries.Points.Add(new DataPoint(unixTime + 10 * i, data[i]));
                                lineSeries.Points.Add(new DataPoint(i, data[i]));
                            }
                            else if (lineSeries.Points.Count > 60)
                            {
                                lineSeries.Points.RemoveAt(0);
                                lineSeries.Points.Add(new DataPoint(data.Length + i, data[i]));
                            }
                            //lineSeries.Points.Add(new DataPoint(unixTime + 10 * i, data[i]));
                        }
                        model.InvalidatePlot(true);
                        Thread.Sleep(25);
                    });
                }
                else if (ojc.Identifier == JCWatchDeviceConstant.CMD_ECGQuality)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        temp1.Text = JCWatch.MyDictionaryToJson(ojc.Data);
                    });
                }
            }
        }

        private void streamECGButton_Clicked(object sender, EventArgs e)
        {
            watch.WriteBytes(JCWatch.openecg);
        }

        private void getDeviceInfoButton_Clicked(object sender, EventArgs e)
        {
            watch.WriteBytes(JCWatch.getDeviceInfo);
        }
        bool RT = false;
        private void readECGHistoryRTButton_Clicked(object sender, EventArgs e)
        {
            RT = true;
            watch.WriteBytes(JCWatch.readHistory);
        }

        private void readECGHistoryButton_Clicked(object sender, EventArgs e)
        {
            RT = false;
            watch.WriteBytes(JCWatch.readHistory);
        }

        private void sportsMode_Clicked(object sender, EventArgs e)
        {
            //walking 5 mins sports mode
            //byte[] readHistory = { 0x19, 0x03, 0x09, 0x00, 0x5, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2A };
            //walk
            //byte[] readHistory = { 0x19, 0x01, 0x09, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x28 };
            //meditation
            byte[] readHistory = { 0x19, 0x01, 0x06, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x25 };
            //byte[] readHistory = { 0x19, 0x01, 0x06, 0x02, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x27 };

            watch.WriteBytes(readHistory);
        }

        private void sportsModeStop_Clicked(object sender, EventArgs e)
        {
            //walking 5 mins sports mode
            //byte[] readHistory = { 0x19, 0x03, 0x09, 0x00, 0x5, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2A };
            //walk
            //byte[] readHistory = { 0x19, 0x01, 0x09, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x28 };
            //meditation stop
            byte[] readHistory = { 0x19, 0x04, 0x06, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x28 };
            //byte[] readHistory = { 0x19, 0x01, 0x06, 0x02, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x27 };

            watch.WriteBytes(readHistory);
        }

        private void ecgppgRealTime_Clicked(object sender, EventArgs e)
        {
            watch.WriteBytes(JCWatch.ecgppgRealTime);
        }

        private void ppgRealTime_Clicked(object sender, EventArgs e)
        {
            watch.WriteBytes(JCWatch.ppgRealTime);
        }
    }
}
