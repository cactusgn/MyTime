using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Summary.Domain
{
    //继承一个ValidationRule,重写Validate方法
    public class NumberValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double myValue = 0;
            if (double.TryParse(value.ToString(), out myValue))
            {
               
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "请输入数字");
            }

        }
    }

}
