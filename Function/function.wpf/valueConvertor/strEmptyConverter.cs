using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace function.wpf
{

    /// <summary>
    /// 숫자를 소수점 3자리 까지변환 ex)12345.125 -> 12,345.125 / 12345 -> 12,345 <para/>    
    /// </summary>
    public class strEmptyConverter : IValueConverter
    {
        /// <summary>
        /// 빈문자열 대체 문자
        /// </summary>
        public static string sletter = "-";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string rtn = Fnc.obj2String(value);

            if (rtn == "") rtn = sletter;

            return rtn;
            
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

    }
}
