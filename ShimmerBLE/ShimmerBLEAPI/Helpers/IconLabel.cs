using System;
using Xamarin.Forms;

namespace shimmer.Helpers
{
    public class IconLabel : Label
    {
        const string Typeface = "shimmer-icons";

        public IconLabel()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    FontFamily = Typeface;
                    break;
                case Device.Android:
                    FontFamily = Typeface + ".ttf#" + Typeface;
                    break;
            }
        }

        public IconLabel(string LabelIcon = null)
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    FontFamily = Typeface;
                    break;
                case Device.Android:
                    FontFamily = Typeface + ".ttf#" + Typeface;
                    break;
            }

            Text = LabelIcon;
        }

        public static class Icon
        {
            public static readonly string CircleEmpty = "A";
            public static readonly string CircleWithPoint = "B";
            public static readonly string Logout = "C";
            public static readonly string Menu = "D";
        }
    }
}
