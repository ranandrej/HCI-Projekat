using System;
using System.Windows;
using System.Windows.Media;

namespace NetworkService.Model
{
    public class CircleMarker : BindableBase
    {
        private string cmType;
        private double cmValue;
        private string cmDate;
        private string cmTime;
        private Thickness cmMargin;
        private Brush cmColor;
        private int cmWidthHeight;

        public CircleMarker()
        {
        }

        public CircleMarker(string cmType, double cmValue, string cmDate, string cmTime)
        {
            CmType = cmType;
            CmValue = cmValue;
            CmDate = cmDate;
            CmTime = cmTime;
        }

        public string CmType
        {
            get { return cmType; }
            set
            {
                cmType = value;
                OnPropertyChanged("CmType");
            }
        }

        public double CmValue
        {
            get { return cmValue; }
            set
            {
                cmValue = value;
                CmMargin = new Thickness(0, 0, 0, cmValue);
                CmWidthHeight = (int)Math.Round(cmValue * 10, 0);
                UpdateCmColor();
                OnPropertyChanged("CmValue");
            }
        }

        public string CmDate
        {
            get { return cmDate; }
            set
            {
                cmDate = value;
                OnPropertyChanged("CmDate");
            }
        }

        public string CmTime
        {
            get { return cmTime; }
            set
            {
                cmTime = value;
                OnPropertyChanged("CmTime");
            }
        }

        public Thickness CmMargin
        {
            get { return cmMargin; }
            set
            {
                cmMargin = value;
                OnPropertyChanged("CmMargin");
            }
        }

        public Brush CmColor
        {
            get { return cmColor; }
            set
            {
                cmColor = value;
                OnPropertyChanged("CmColor");
            }
        }

        public int CmWidthHeight
        {
            get { return cmWidthHeight; }
            set
            {
                cmWidthHeight = value;
                OnPropertyChanged("CmWidthHeight");
            }
        }

        private void UpdateCmColor()
        {
            switch (cmType)
            {
                case "SmartMeter":
                    CmColor = (cmValue > 2.73) ? Brushes.Red : Brushes.CadetBlue;
                    break;
                case "IntervalMeter":
                    CmColor = (cmValue > 2.73) ? Brushes.Red : Brushes.CadetBlue;
                    break;
                default:
                    CmColor = Brushes.CadetBlue;
                    break;
            }
        }
    }
}
