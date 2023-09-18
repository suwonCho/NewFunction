using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace function.wpf
{

    /// <summary>
    /// 문자열 -> 날짜 2023-05-16 <para/>
    /// <local:ForeBrushConverter x:Key='FConv' />
    /// Foreground="{Binding ElementName=comboBoxBrush, Path=SelectedItem.Brush, Converter={StaticResource FConv}}"
    /// </summary>
    public class dateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string val;
            DateTime dt;

            if(value.GetType() == typeof(System.DateTime))
            {
                dt = (System.DateTime)value;
                val = function.Fnc.Date2String(dt, Fnc.enDateType.DBTypeLong);
            }
            else
                val = function.Fnc.obj2String(value);
            

            switch(val.Length)
            {
                case 8:
                    dt = function.Fnc.String2Date(val, Fnc.enDateType.DBType);
                    val = function.Fnc.Date2String(dt, Fnc.enDateType.Date);
                    break;

                case 14:
                    dt = function.Fnc.String2Date(val, Fnc.enDateType.DBTypeLong);
                    val = function.Fnc.Date2String(dt, Fnc.enDateType.Date);
                    break;
            }

            return val;
            
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

    }
}
