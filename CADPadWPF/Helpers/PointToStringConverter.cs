using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CADPadWPF.Helpers
{

    internal class PointToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strResult = string.Empty;

            if (value is Point)
            {
                Point pnt = (Point)value;
                string strX = pnt.X.ToString("0.00");
                strX = strX.Replace(',', '.');
                string strY = pnt.Y.ToString("0.00");
                strY = strY.Replace(',', '.');

                strResult = strX + ", " + strY;
            }

            return strResult;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}