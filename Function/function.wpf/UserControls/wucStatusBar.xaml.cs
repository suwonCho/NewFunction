using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace function.wpf
{
    /// <summary>
    /// wucStatusBar.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class wucStatusBar : UserControl
    {
        static void PropertyChangeCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            wucStatusBar w = o as wucStatusBar;
            w.Init();
        }

        private void Init()
        {
            //헤더너비
            GrdMain.ColumnDefinitions[0].Width = new GridLength(HederWidth, GridUnitType.Star);
            //시계너비
            GrdMain.ColumnDefinitions[3].Width = new GridLength(TimerWidth, GridUnitType.Star);

            //시계 표시 여부
            if (isShowTimer)
            {
                Grid.SetColumnSpan(lblMsg, 1);
                lblTime.Visibility = Visibility.Visible;

                if (tmrTimer == null)
                {
                    tmrTimer = new DispatcherTimer();
                    tmrTimer.Interval = TimeSpan.FromSeconds(1);
                    tmrTimer.Tick += TmrTimer_Tick;
                }

                tmrTimer.Start();
            }
            else
            {
                Grid.SetColumnSpan(lblMsg, 2);
                lblTime.Visibility = Visibility.Hidden;

                if (tmrTimer != null)
                {
                    tmrTimer.Stop();
                }
            }


            //헤더타입
            if (HeaderType == enElementType.Label)
            {
                pgrBar.Visibility = Visibility.Hidden;
                lblHeader.Visibility = Visibility.Visible;
            }
            else
            {
                pgrBar.Visibility = Visibility.Visible;
                lblHeader.Visibility = Visibility.Hidden;
            }


            lblHeader.Content = HeaderText;



        }
        

        public static readonly DependencyProperty HederWidthProperty = DependencyProperty.Register("HederWidth", typeof(double), typeof(wucStatusBar),
    new FrameworkPropertyMetadata(0d, PropertyChangeCallback));

        /// <summary>
        /// 헤더 너비를 설정하거나 가져옵니다.
        /// </summary>
        public double HederWidth
        {
            get { return (double)GetValue(HederWidthProperty); }
            set
            {
                SetValue(HederWidthProperty, value);
            }
        }



        public static readonly DependencyProperty timerWidthProperty = DependencyProperty.Register("TimerWidth", typeof(double), typeof(wucStatusBar),
    new FrameworkPropertyMetadata(10d, PropertyChangeCallback));

        /// <summary>
        /// 시계 너비를 설정하거나 가져옵니다.
        /// </summary>
        public double TimerWidth
        {
            get { return (double)GetValue(timerWidthProperty); }
            set
            {
                SetValue(timerWidthProperty, value);
            }
        }





        public static readonly DependencyProperty isShowTimerProperty = DependencyProperty.Register("isShowTimer", typeof(bool), typeof(wucStatusBar),
    new FrameworkPropertyMetadata(false, PropertyChangeCallback));

        /// <summary>
        /// 시계 표시 여부
        /// </summary>
        public bool isShowTimer
        {
            get { return (bool)GetValue(isShowTimerProperty); }
            set
            {
                SetValue(isShowTimerProperty, value);
            }
        }



        public static readonly DependencyProperty HeaderTypeProperty = DependencyProperty.Register("HeaderType", typeof(enElementType), typeof(wucStatusBar),
new FrameworkPropertyMetadata(enElementType.Progress, PropertyChangeCallback));

        /// <summary>
        /// 상태 표시 창 헤더 타입 설정 Progress or Label
        /// </summary>
        public enElementType HeaderType
        {
            get { return (enElementType)GetValue(HeaderTypeProperty); }
            set
            {
                SetValue(HeaderTypeProperty, value);
            }
        }



        public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register("HeaderText", typeof(string), typeof(wucStatusBar),
new FrameworkPropertyMetadata("", PropertyChangeCallback));

        /// <summary>
        /// 상태 표시 창 헤더 타입 설정 Progress or Label
        /// </summary>
        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set
            {
                SetValue(HeaderTextProperty, value);
            }
        }








        public string LastMessage { get; set; } = "";
        public enMsgType LastMsgType { get; set; }

        DispatcherTimer tmrTimer = null;

        public wucStatusBar()
        {
            InitializeComponent();

            this.Loaded += WucStatusBar_Loaded;            
        }



        private void WucStatusBar_Loaded(object sender, RoutedEventArgs e)
        {
            //lblMsg.Content = "";
        }

        private void TmrTimer_Tick(object sender, EventArgs e)
        {
            lblTime.Content = Fnc.Date2String(DateTime.Now, Fnc.enDateType.DateTime);
        }

        /// <summary>
        /// 메시지 창에 아이콘을 표시 합니다.
        /// </summary>
        /// <param name="msgType"></param>
        protected void SetMessageIcon(enMsgType msgType)
        {
            ImageSourceConverter c = new ImageSourceConverter();

            ImageSource src;

            switch (msgType)
            {
                case enMsgType.Error:
                    //img.Source = (ImageSource)c.ConvertFrom(function.resIcon16.stop);              //function.Properties.Resources.stop));
                    src = wpfFnc.ImageSourceFormBitmap(function.resIcon16.stop);
                    break;

                case enMsgType.OK:
                    //img.Source = (ImageSource)c.ConvertFrom(function.resIcon16.button_green);      //(function.Properties.Resources.button_green));
                    src = wpfFnc.ImageSourceFormBitmap(function.resIcon16.button_green);
                    break;

                default:
                    //img.Source = (ImageSource)c.ConvertFrom(function.resIcon16.button_withe);      //(function.Properties.Resources.button_green));
                    src = wpfFnc.ImageSourceFormBitmap(function.resIcon16.button_withe);
                    break;

            }


            //img.Source = src;

            control_Dispatcher.InvokeProperty(this.Parent.Dispatcher, img, "", src);

        }



        /// <summary>
        /// 메시지 창에 내용을 보여 준다.(메시지 타입 사용)
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="strMessage"></param>
        /// <param name="isLog"></param>
        public void SetMessage(enMsgType msgType, string strMessage)
        {
            
            if (msgType != enMsgType.Error && strMessage.Equals(string.Empty))
            {
                img.Source = null;
                lblMsg.Content = string.Empty;
                return;
            }

            //아이콘 설정
            SetMessageIcon(msgType);

            LastMessage = string.Format("[{0}] {1}", Fnc.Date2String(DateTime.Now, Fnc.enDateType.Time), strMessage);
            LastMsgType = msgType;

            control_Dispatcher.InvokeProperty(Dispatcher, lblMsg, "Content", LastMessage);           

        }



        /// <summary>
        /// 상태바에 값을 변경한다.
        /// </summary>
        /// <param name="NowValue"></param>
        public void ProgressBar_Value(double NowValue)
        {
            try
            {
                pgrBar.Value = pgrBar.Maximum < NowValue ? pgrBar.Maximum : NowValue;
            }
            catch
            {

            }
        }

        /// <summary>
        /// 최대값을 설정한다.
        /// </summary>
        /// <param name="MaxValue"></param>
        public void ProgressBar_MaxValue(double MaxValue)
        {
            pgrBar.Maximum = MaxValue;           
        }

        /// <summary>
        /// 최소값을 설정한다.
        /// </summary>
        /// <param name="MinValue"></param>
        public void ProgressBar_MinValue(double MinValue)
        {
            pgrBar.Minimum = MinValue;
        }

        private void LblMsg_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LastMessage == "") return;

            MessageBox.Show(wpfFnc.ParentWindowGet(this), LastMessage, "메시지", MessageBoxButton.OK, MessageBoxImage.None);
        }
    }   //end class
}
