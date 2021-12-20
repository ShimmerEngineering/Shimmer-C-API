using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;

namespace shimmer.Helpers
{
    [AddINotifyPropertyChangedInterface]
    public class ProjectSettings
    {
        public static ProjectSettings Data = new ProjectSettings();

        string _barBackgroundColor = "#3684C2";
        string _barTextColor = "#FFFFFF";
        double _scaleFontSize = 1;
        double _displayScaleFactor = 1;

        public string BarBackgroundColor
        {
            get
            {
                return _barBackgroundColor;
            }
            set
            {
                _barBackgroundColor = value;
            }
        }

        public string BarTextColor
        {
            get
            {
                return _barTextColor;
            }
            set 
            {
                _barTextColor = value;
            }
        }

        public double ScaleFontSize
        {
            get
            {
                return _scaleFontSize;
            }
            set
            {
                _scaleFontSize = value;
            }
        }

        public double DisplayScaleFactor
        {
            get
            {
                return _displayScaleFactor;
            }
            set
            {
                _displayScaleFactor = value;
            }
        }

        public void SetStockBarBackgroundColor()
        {
            BarBackgroundColor = "#3684C2";
        }

        public void SetStockBarTextColor()
        {
            BarTextColor = "#FFFFFF";
        }
    }

}
