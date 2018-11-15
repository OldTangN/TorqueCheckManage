using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;


namespace LongTie.Nlbs.Common
{
    [ValueConversion(typeof(string), typeof(string))]
    public class ConverterImage :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
           {
               string imgPath = "";
               if (value is string)
               {
                   if ((string)value == "√")
                   {
                       imgPath = "/LongTie.Nlbs;component/Images/success.png";
                   }
                   if ((string)value == "×")
                   {
                       imgPath = "/LongTie.Nlbs;component/Images/fail.png";
                   }
               }
               return imgPath;
           }
           public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
           {
               if (targetType != typeof(string))
                   throw new InvalidOperationException();
               return "";
           }
       }    
}
