using System;
using System.Collections.Generic;
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
    /// MsgBox.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class wpfMsgBox : function.wpf.baseWindow
    {

        public static bool? show(Window parent, string title, string msg, bool showCancel)
        {
            wpfMsgBox f = new wpfMsgBox(title, msg, showCancel);

            if (parent == null)
                return f.ShowDialog();
            else
                return f.ShowDialog(parent);
        }

        string Msg;
        string title;
        bool ShowCancel = true;

        public wpfMsgBox()
        {
            InitializeComponent();
        }


        public wpfMsgBox(string _title, string msg, bool showCancel) : this()
        {
            Msg = msg;
            title = _title;
            ShowCancel = showCancel;
        }


        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = title;
            tBlock.Text = Msg;

            if(!ShowCancel)
            {
                btnOk.Margin = btnCacel.Margin;
                btnCacel.Visibility = Visibility.Hidden;
            }            
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void BtnCacel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
