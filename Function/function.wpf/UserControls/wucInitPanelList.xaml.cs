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
using System.Windows.Threading;

namespace function.wpf
{
    /// <summary>
    /// wucInitPanelList.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class wucInitPanelList : UserControl
    {
        class item
        {
            public string Text { get; set; }

            public string BackColor { get; set; }

            public string ForeColor { get; set; }

            public item(string text)
            {
                Text = text;

                BackColor = Fnc.WpfColor2String(System.Drawing.Color.White);
                ForeColor = Fnc.WpfColor2String(System.Drawing.Color.Black);
            }

            public item(string text, SolidColorBrush forecolor, SolidColorBrush backcolor)
            {
                Text = text;                
                
                BackColor = Fnc.SolidBrush2String(forecolor);
                ForeColor = Fnc.SolidBrush2String(backcolor);
            }

        }


		/// <summary>
		/// 타이틀을 설정 하거나 가져 옵니다.
		/// </summary>
		public string Title
        {
            get
            {
                return Fnc.obj2String(lblTitle.Content);
            }
            set
            {
                lblTitle.Content = value;
            }
        }


        /// <summary>
        /// 생성 후 InitInGrid 실행
        /// </summary>
        public wucInitPanelList()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 그리드에 추가 후 센터에 표시
        /// </summary>
        /// <param name="MainGrd"></param>
        /// <param name="title"></param>
		public void InitInGrid(Grid MainGrd, string title)
		{
			Title = title;
			MainGrd.Children.Add(this);

			Grid.SetColumnSpan(this, MainGrd.ColumnDefinitions.Count);
			Grid.SetRowSpan(this, MainGrd.RowDefinitions.Count);

			double l = (MainGrd.ActualWidth / 2) - (this.Width / 2);
			double h = (MainGrd.ActualHeight / 2) - (this.Height / 2);

			this.Margin = new Thickness(0, 0, 0, 0);
		}
        
        public void BtnOkEnable()
        {
            btnOk.IsEnabled = true;
        }


        /// <summary>
        /// 추가된 그리드를 제거 한다.
        /// </summary>
        /// <param name="MainGrd"></param>
        public void InitEndGrid(Grid MainGrd)
        {
            MainGrd.Children.Remove(this);
        }


		private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
			grdList.Columns[0].Width = grdList.ActualWidth -3;
		}


        public void TextAdd(string text, bool isError = false)
        {
            item i = new item(text);

            if (isError)
                TextAdd(text, Brushes.Gold, Brushes.Red);
            else
                grdList.Items.Insert(0, i);

            System.Windows.Forms.Application.DoEvents();

        }


        public void TextAdd(string text, SolidColorBrush forecolor, SolidColorBrush backcolor)
        {
            item i = new item(text, forecolor, backcolor);

            grdList.Items.Insert(0, i);

            System.Windows.Forms.Application.DoEvents();

        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            InitEndGrid((Grid)this.Parent);
        }
    }	//end calss
}
