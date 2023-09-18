using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Window1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DataGridColMultiRow : Window
    {
        DataTable dt;

        public DataGridColMultiRow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dt = new DataTable();

            dt.Columns.Add("Col1", typeof(int));
            dt.Columns.Add("Col2", typeof(string));


            for(int i = 0; i < 100; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Col1"] = i;
                dr["Col2"] = $"Row{i.ToString("D3")}";

                dt.Rows.Add(dr);
            }

            dg.ItemsSource = dt.DefaultView;

        }


    }   //end class
}
