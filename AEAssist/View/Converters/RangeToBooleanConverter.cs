using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using AEAssist.Helper;

namespace AEAssist.View
{
    public class RangeToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,object parameter, CultureInfo culture)
        {
            LogHelper.Info("比较值{value}");
            return ((double)value) < 300;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("Nope.");
        }

    }
}
