using ScottPlot.MarkerShapes;
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
          
            switch (type)
            {
                case "none": return 0;
                case "study": return 1;
                case "waste": return 2;
                case "rest": return 3;
                case "work": return 4;
                case "play": return 5;
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
            switch (i)
            {
                case 0: return "none";
                case 1: return "study";
                case 2: return "waste";
                case 3: return "rest";
                case 4: return "work";
                case 5: return "play";
            }

            return "none";
        }
    }
}
