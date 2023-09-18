using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace function.wpf
{
    /// <summary>
    /// wucInputCheckValue.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class wucInputCheckValue : UserControl
    {
        public string Title
        {
            get { return (string)lblTitle.Content; }
            set { lblTitle.Content = value; }
        }

        string _value = "";

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                lblValue.Content = value;
                txtValue_TextChanged(null, null);
            }
        }

        public GridLength TitleWidth
        {
            get { return (GridLength)this.Resources["col0"]; }
            set
            {
                this.Resources["col0"] = value;
            }
        }

        public GridLength ValueWidth
        {
            get { return (GridLength)this.Resources["col1"]; }
            set
            {
                this.Resources["col1"] = value;
            }
        }

        public GridLength InputWidth
        {
            get { return (GridLength)this.Resources["col2"]; }
            set
            {
                this.Resources["col2"] = value;
            }
        }

        public GridLength RstWidth
        {
            get { return (GridLength)this.Resources["col3"]; }
            set
            {
                this.Resources["col3"] = value;
            }
        }

        bool showValue = false;

        public bool ShowValue
        {
            get { return showValue; }
            set
            {
                showValue = value;
                lblValue.Foreground = showValue ? Brushes.White : Brushes.Black;
            }
        }


        event PropertyChangedCallback onRstChanged;

        public event PropertyChangedCallback OnRstChanged
        {
            add { onRstChanged += value; }
            remove { onRstChanged -= value; }
        }


        public static readonly DependencyProperty RstProperty = DependencyProperty.Register("Rst", typeof(bool), typeof(wucInputCheckValue), new PropertyMetadata(false));
        

        public bool Rst
        {
            get { return (bool)GetValue(RstProperty); }
            set
            {

                if (Rst == value) return;

                bool old = Rst;
                SetValue(RstProperty, value);

                if(onRstChanged != null)
                {                    
                    DependencyPropertyChangedEventArgs e = new DependencyPropertyChangedEventArgs(RstProperty, old, Rst);
                    onRstChanged.Invoke(this, e);
                }
            }
        }


        public TextBox ValueTextBox { get { return txtValue; } }

        public wucInputCheckValue()
        {
            InitializeComponent();     
            
                   
        }

        private void txtValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(txtValue.Text.Length < 1)
            {
                lblRst.Content = "입력  대기";
                lblRst.Background = Brushes.LightGray;
                Rst = false;
            }
            else if(txtValue.Text == _value)
            {
                lblRst.Content = "입력  일치";
                lblRst.Background = Brushes.RoyalBlue;
                Rst = true;
            }
            else
            {
                lblRst.Content = "입력불일치";
                lblRst.Background = Brushes.Red;
                Rst = false;
            }
        }

        private void UserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            //txtValue.Focus();
        }

        public void txtValue_Focus()
        {
            this.Focus();
            txtValue.Focus();

            
        }

    }
}
