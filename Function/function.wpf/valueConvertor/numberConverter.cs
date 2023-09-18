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
    /// <local:ForeBrushConverter x:Key='FConv' />
    /// Foreground="{Binding ElementName=comboBoxBrush, Path=SelectedItem.Brush, Converter={StaticResource FConv}}"
    /// </summary>
    public class numberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Double val;

            if (!Double.TryParse(value.ToString(), out val)) return 0;

            return String.Format("{0:#,##0.###}", val); 
            
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

    }
}
