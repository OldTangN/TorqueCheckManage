using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace LongTie.Nlbs.Common
{


    [ValueConversion(typeof(string ), typeof(bool))]
    public class Converter : IValueConverter
    {
   
        public object Convert(object value,Type targetType,object parameter,System .Globalization .CultureInfo culture)     
        {

            if (value is bool ){
            if ((bool)value)
                return "是";
            else
                return "否";
            }
            return "";
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException();
            return "";
        }

    }

 
}
