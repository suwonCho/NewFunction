using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace function.wpf
{
    /// <summary>
    /// winSettingView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class winSettingView : Window
    {
        object o = null;
        public winSettingView(object obj)
        {
            InitializeComponent();

            o = obj;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (o == null) return;

            string t = string.Empty;
            int i = 0;
            Type type = o.GetType();
            PropertyInfo[] pi = type.GetProperties();

            FieldInfo[] fi = type.GetFields();

            foreach (FieldInfo f in fi)
            {
                if (i > 0) t += "\r\n";

                t += $"[{f.Name}]{f.GetValue(o)}";

                i++;
            }


            foreach (PropertyInfo p in pi)
            {
                if (i > 0) t += "\r\n";

                t += $"[{p.Name}]{p.GetValue(o, null)}";

                i++;
            }

            txtView.Text = t;

        }


    }   //end class
}
