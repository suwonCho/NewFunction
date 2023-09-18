using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// DatePicker.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DatePicker : UserControl
    {
        public static string[] DayNames = new string[] { "월", "화", "수", "목", "금", "토", "일" };

        UniformGrid unigridMonth;
        DateTime datetimeSaved = DateTime.Now;

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(DateTime?), typeof(DatePicker),
            new PropertyMetadata(DateTime.Now, ValueChangeCallback));

        public DateTime? Value
        {
            set { SetValue(ValueProperty, value); }
            get { return (DateTime?)GetValue(ValueProperty); }
        }

        static void ValueChangeCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            (o as DatePicker).OnValueChanged((DateTime?)e.OldValue, (DateTime?)e.NewValue);
        }

        /// <summary>
        /// 선택 안함 표시 여부
        /// </summary>
        public static readonly DependencyProperty ShowTimeProperty = DependencyProperty.Register("ShowTime", typeof(bool), typeof(DatePicker),
           new PropertyMetadata(true, ShowNullChangeCallback));

        public bool ShowTime
        {
            set { SetValue(ShowTimeProperty, value); }
            get { return (bool)GetValue(ShowTimeProperty); }
        }



        /// <summary>
        /// 선택 안함 표시 여부
        /// </summary>
        public static readonly DependencyProperty ShowNullProperty = DependencyProperty.Register("ShowNull", typeof(bool), typeof(DatePicker),
           new PropertyMetadata(true, ShowNullChangeCallback));

        public bool ShowNull
        {
            set { SetValue(ShowNullProperty, value); }
            get { return (bool)GetValue(ShowNullProperty); }
        }


        /// <summary>
        /// 선택 버튼 표시 여부
        /// </summary>
        public static readonly DependencyProperty ShowOkProperty = DependencyProperty.Register("ShowOk", typeof(bool), typeof(DatePicker),
           new PropertyMetadata(true, ShowNullChangeCallback));

        public bool ShowOk
        {
            set { SetValue(ShowOkProperty, value); }
            get { return (bool)GetValue(ShowOkProperty); }
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        static void ShowNullChangeCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            DatePicker d = o as DatePicker;



            d.row3.Height = new GridLength((d.ShowTime || d.ShowNull) ? 25 : 0);

            if (!d.ShowTime)
            {
                d.pnlTime.Visibility = Visibility.Hidden;
                d.txtHour.Text = "00";
                d.txtMinute.Text = "00";
                d.txtSecond.Text = "00";
            }
            else
                d.pnlTime.Visibility = Visibility.Visible;

            d.chkboxnull.Visibility = d.ShowNull ? Visibility.Visible : Visibility.Hidden;

            d.row4.Height = new GridLength(d.ShowOk ? 28 : 0);
        }



        public static readonly RoutedEvent DateChangeEvent = EventManager.RegisterRoutedEvent("DateChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<DateTime?>), typeof(DatePicker));

        public event RoutedPropertyChangedEventHandler<DateTime?> DateChanged
        {
            add { AddHandler(DateChangeEvent, value); }
            remove { RemoveHandler(DateChangeEvent, value); }
        }


        event delObject onDateSelected;

        /// <summary>
        /// 선택(날자 더블클릭 or 버튼 클릭) 하면 발생 한는 이벤트
        /// </summary>
        public event delObject OnDateSelected
        {
            add { onDateSelected += value; }
            remove { onDateSelected -= value; }
        }




        public DatePicker()
        {
            InitializeComponent();

            Loaded += DatePicker_Loaded;

            control_Dispatcher.TextBox_Press_NumberOnly(txtHour);
            control_Dispatcher.TextBox_Press_NumberOnly(txtMinute);
            control_Dispatcher.TextBox_Press_NumberOnly(txtSecond);

            txtHour.TextChanged += TxtHour_TextChanged;
            txtMinute.TextChanged += TxtHour_TextChanged;
            txtSecond.TextChanged += TxtHour_TextChanged;
        }

        private void TxtHour_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int max = Fnc.obj2int(tb.Tag);
            int value = Fnc.obj2int(tb.Text);

            if (value > max)
            {
                tb.Text = max.ToString();
                return;
            }
            else if (value < 0)
            {
                tb.Text = "0";
                return;
            }

            //변경처리
            if (Value == null) return;
            DateTime dt = Value.Value;
            Value = new DateTime(dt.Year, dt.Month, dt.Day, Fnc.obj2int(txtHour.Text), Fnc.obj2int(txtMinute.Text), Fnc.obj2int(txtSecond.Text));
            
        }

        

        private void DatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            unigridMonth = FindUniGrid(lstboxMonth);


            //listitem을 꽉체운다.
            lstboxMonth.BeginInit();
            lstboxMonth.Items.Clear();
            ListBoxDayItem li;        

            for (int i = 0; i < 42; i++)
            {
                li = new ListBoxDayItem();
                li.MouseDoubleClick += Li_MouseDoubleClick;
                lstboxMonth.Items.Add(li);

            }

            lstboxMonth.EndInit();


            OnValueChanged(Value, Value);
            

        }


        /// <summary>
        /// 아이템 더블 클릭 - 날짜 선택 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Li_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (onDateSelected != null) onDateSelected.Invoke(this);
        }

        UniformGrid FindUniGrid(DependencyObject vis)
        {
            if (vis is UniformGrid)
            {
                return vis as UniformGrid;
            }


            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(vis); i++)
            {
                Visual visRtn = FindUniGrid(VisualTreeHelper.GetChild(vis, i));

                if (visRtn != null) return visRtn as UniformGrid;
            }

            return null;
        }




        /// <summary>
        /// 이동 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;
            if (b == null) return;            
            int step = Fnc.obj2int(b.Tag);
            FillPage(step);
        }


        /// <summary>
        /// 날짜/시간 변경 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstboxMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Value == null || e == null || e.AddedItems.Count < 1) return;

            ListBoxDayItem li = e.AddedItems[0] as ListBoxDayItem;
            DateTime dt = (DateTime)li.Date;

            dt = new DateTime(dt.Year, dt.Month, dt.Day, Fnc.obj2int(txtHour.Text), Fnc.obj2int(txtMinute.Text), Fnc.obj2int(txtSecond.Text));
            Value = dt;
        }

        private void chkboxnull_Checked(object sender, RoutedEventArgs e)
        {
            if(Value != null)
            {
                datetimeSaved = (DateTime)Value;
                Value = null;
            }
        }

        private void chkboxnull_Unchecked(object sender, RoutedEventArgs e)
        {
            Value = datetimeSaved;

            TxtHour_TextChanged(txtHour, null);
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.PageDown)
            {
                FillPage(-1);
                e.Handled = true;
            }
            else if (e.Key == Key.PageUp)
            {
                FillPage(1);
                e.Handled = false;
            }

        }

        /// <summary>
        /// 다음 달,년, 10년 등으로 이동
        /// </summary>
        /// <param name="Month"></param>
        void FillPage(int Month)
        {
            if (Value == null) return;

            DateTime dt = (DateTime)Value;
            int numPages = Month;

            //+Shift Key 1년 단위
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                numPages *= 12;
            }

            //+ctrl key 10년 단위

            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                numPages = Math.Max(-1200, Math.Min(1200, 120 * numPages));
            }

            int year = dt.Year + numPages / 12;
            int month = dt.Month + numPages % 12;

            while(month < 1)
            {
                month += 12;
                year -= 1;
            }
            while(month > 12)
            {
                month -= 12;
                year += 1;
            }

            if (year < DateTime.MinValue.Year)
                Value = DateTime.MinValue.Date;
            else if (year > DateTime.MaxValue.Year)
                Value = DateTime.MaxValue.Date;
            else
                Value = new DateTime(year, month, Math.Min(dt.Day, DateTime.DaysInMonth(year, month)));

        }


        protected virtual void OnValueChanged(DateTime? dtOldValue, DateTime? dtNewValue)
        {
            chkboxnull.IsChecked = dtNewValue == null;


            if (dtNewValue != null)
            {

                ListBoxDayItem li;
                DateTime val = (DateTime)dtNewValue;
                val = new DateTime(val.Year, val.Month, val.Day);

                li = lstboxMonth.Items[0] as ListBoxDayItem;

                //년월이 변경 되면 달력을 다시 설정한다.
                if (dtOldValue == null || dtOldValue.Value.Year != dtNewValue.Value.Year || dtOldValue.Value.Month != dtNewValue.Value.Month || li.Date == null)
                {
                    txtblkMonthYear.Text = val.ToString("yyyy년 MM월");

                    DateTime mon = new DateTime(val.Year, val.Month, 1);


                    //달력을 꽉체우기 위해 전달표시 일자를 찾는다.
                    if (mon.DayOfWeek == DayOfWeek.Sunday)
                        mon = mon.AddDays(-6);
                    else
                        mon = mon.AddDays(((int)mon.DayOfWeek - 1) * -1);


                    

                    for (int i = 0; i < 42; i++)
                    {
                        li = lstboxMonth.Items[i] as ListBoxDayItem;
                        li.CalendarDate = val;
                        li.Date = mon;

                        if (mon == val) lstboxMonth.SelectedItem = li;

                        mon = mon.AddDays(1);
                    }                    
                }

                val = (DateTime)dtNewValue;
                txtHour.Text = val.Hour.ToString();
                txtMinute.Text = val.Minute.ToString();
                txtSecond.Text = val.Second.ToString();
            }
            else
            {

            }

            //이벤트 발생
            RoutedPropertyChangedEventArgs<DateTime?> e = new RoutedPropertyChangedEventArgs<DateTime?>(dtOldValue, dtNewValue, DatePicker.DateChangeEvent);
            e.Source = this;
            RaiseEvent(e);
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (onDateSelected != null) onDateSelected.Invoke(this);
        }
    }   // end class
}
