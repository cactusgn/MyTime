using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            int i = 0;
            string type = value.ToString().Trim();
            switch(type)
            {
                case "rest": return 0;
                case "study": return 1;
                case "work": return 2;
                case "waste": return 3;
                case "none": return 4;
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
