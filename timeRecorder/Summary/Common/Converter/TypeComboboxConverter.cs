using ScottPlot.MarkerShapes;
using Summary.Common.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Summary.Common.Converter
{
    public class TypeComboboxConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Type is null");
            }
            string type = value.ToString().Trim().ToLower();
            if (Helper.SummaryCategoryDic.ContainsKey(type))
            {
                return Helper.SummaryCategoryDic[type];
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Type is null");
            }
            int i =  int.Parse(value.ToString().Trim());
            return Helper.SummaryCategoryDic.First(x=>x.Value==i).Key;
        }
    }
}
