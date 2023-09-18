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
    /// ComboBoxXTextItem.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ComboBoxXTextItem : UserControl
    {
        event RoutedEventHandler clickDel;

        public event RoutedEventHandler ClickDel
        {
            add { clickDel += value; }
            remove { clickDel -= value; }
        }


        /// <summary>
        /// CellPadding 의존 속성
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ComboBoxXTextItem),
            new FrameworkPropertyMetadata(" ", FrameworkPropertyMetadataOptions.AffectsArrange,
                OnTextChanged));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value);  }
        }

        static void OnTextChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            ComboBoxXTextItem c = (dependencyObject as ComboBoxXTextItem);
            c.TextApply();
        }


        public ComboBoxXTextItem()
        {
            InitializeComponent();
        }

        public ComboBoxXTextItem(string text)
        {
            InitializeComponent();
            Text = text;            
        }

        private void TextApply()
        {
            txt.Text = Text;
        }


        public override string ToString()
        {
            return Text;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement f = this.Parent as FrameworkElement;
            if (f == null) return;

            F_SizeChanged(null, null);

            f.SizeChanged += F_SizeChanged;
        }

        private void F_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            FrameworkElement f = this.Parent as FrameworkElement;
            if (f == null) return;

            this.Width = f.ActualWidth * 0.95;
            this.Height = f.ActualHeight * 0.95;

            double height = f.ActualHeight * 0.85;
            btn.Height = height;
            btn.Width = height;
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            if (clickDel != null) clickDel(this, e);
        }


    }
}
