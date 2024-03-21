using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;

namespace shimmer.Helpers
{
    /// <summary>
    /// This class contains project settings
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class ProjectSettings
    {
        public static ProjectSettings Data = new ProjectSettings();

        string _barBackgroundColor = "#3684C2";
        string _barTextColor = "#FFFFFF";
        double _scaleFontSize = 1;
        double _displayScaleFactor = 1;

        /// <summary>
        /// Property for bar background color
        /// </summary>
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

        /// <summary>
        /// Property for bar text color
        /// </summary>
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

        /// <summary>
        /// Property for scale font size
        /// </summary>
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

        /// <summary>
        /// Property for display scale factor
        /// </summary>
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

        /// <summary>
        /// Set bar background color to #3684C2
        /// </summary>
        public void SetStockBarBackgroundColor()
        {
            BarBackgroundColor = "#3684C2";
        }

        /// <summary>
        /// Set bar text color to #FFFFFF
        /// </summary>
        public void SetStockBarTextColor()
        {
            BarTextColor = "#FFFFFF";
        }
    }

}
