using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace function.wpf.test
{
    /// <summary>
    /// DataBiding.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DataBiding : Window
    {
        DataTable dt;

        public DataBiding()
        {
            InitializeComponent();

            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dt = new DataTable();
            dt.Columns.Add("No", Type.GetType("System.Int32"));
            dt.Columns.Add("Text", Type.GetType("System.String"));

            string[] txt = new string[] { "1차 분류", "습점", "입상포장", "진공포장", "캔포장", "추출용 원료볶음", "삼정제조1", "삼정제조2", "삼정제조3", "삼정제조4" };


            for (int i = 1; i < 11; i++)
            {
                DataRow dr = dt.NewRow();

                dr["No"] = i;
                dr["Text"] = txt[i - 1];

                dt.Rows.Add(dr);
            }
                        
            dt.DefaultView.Sort = "No";
            dgLeft.ItemsSource = dt.DefaultView;


            //Binding bg = new Binding("No");
            //bg.Source = dt.DefaultView;
            //txtNo.SetBinding(TextBox.TextProperty, bg);

       
            
        }
    }
}
