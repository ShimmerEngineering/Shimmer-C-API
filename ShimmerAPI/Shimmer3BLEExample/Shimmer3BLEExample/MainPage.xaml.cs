using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Shimmer3BLEExample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            ShimmerAPI.ShimmerLogAndStreamBLE device = new ShimmerAPI.ShimmerLogAndStreamBLE("e8eb1b9767ad", "e8eb1b9767ad");
            device.Connect();
        }
    }
}
