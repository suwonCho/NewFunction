using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace function.wpf
{
    class ListBoxDayItem : ListBoxItem
    {
        public static readonly DependencyProperty DateProperty = DependencyProperty.Register("Date", typeof(DateTime?), typeof(ListBoxDayItem),
           new PropertyMetadata(null, DataChangeCallback));

        /// <summary>
        /// 자신의 날짜
        /// </summary>
        public DateTime? Date
        {
            set { SetValue(DateProperty, value); }
            get { return (DateTime?)GetValue(DateProperty); }
        }

        public static readonly DependencyProperty CalendarDateProperty = DependencyProperty.Register("CalendarDate", typeof(DateTime?), typeof(ListBoxDayItem),
           new PropertyMetadata(null));

        /// <summary>
        /// 달력 날짜
        /// </summary>
        public DateTime? CalendarDate
        {
            set { SetValue(CalendarDateProperty, value); }
            get { return (DateTime?)GetValue(CalendarDateProperty); }
        }



        static void DataChangeCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ListBoxDayItem li = (ListBoxDayItem)o;
            li.OnDateChanged((DateTime?)e.OldValue, (DateTime?)e.NewValue);
        }


        public ListBoxDayItem()
        {

        }

        public ListBoxDayItem(DateTime calendarDate, DateTime dt)
        {
            CalendarDate = calendarDate;
            Date = dt;
        }

        /// <summary>
        /// 달력 날짜 / 자신의 날짜를 년/월 비교 하여 표시를 변경 한다.
        /// </summary>
        /// <param name="dtOldValue"></param>
        /// <param name="dtNewValue"></param>
        protected virtual void OnDateChanged(DateTime? dtOldValue, DateTime? dtNewValue)
        {
            TextBlock tb = this.Content as TextBlock;

            if (tb == null)
            {
                tb = new TextBlock();
                this.Content = tb;
            }

            if (dtNewValue == null)
            {
                tb.Text = " ";
                return;
            }
            
            
            tb.Text = dtNewValue.Value.Day.ToString();
            
            ListBox calendar = this.Parent as ListBox;

            if (calendar == null || CalendarDate == null) return;

            //달력달과 아닌달의 일 표시 방법 변경
            if (dtNewValue.Value.Year == CalendarDate.Value.Year && dtNewValue.Value.Month == CalendarDate.Value.Month)
            {
                if (dtNewValue.Value.DayOfWeek == DayOfWeek.Sunday)
                    tb.Foreground = Brushes.Red;        //일
                else if (dtNewValue.Value.DayOfWeek == DayOfWeek.Saturday)
                    tb.Foreground = Brushes.Blue;        //토
                else
                    tb.Foreground = Brushes.Black;        //평일


                this.FontSize = calendar.FontSize;
                this.HorizontalContentAlignment = HorizontalAlignment.Right;
                this.VerticalAlignment = VerticalAlignment.Center;                
            }
            else
            {

                if (dtNewValue.Value.DayOfWeek == DayOfWeek.Sunday)
                    tb.Foreground = Brushes.Orange;        //일
                else if (dtNewValue.Value.DayOfWeek == DayOfWeek.Saturday)
                    tb.Foreground = Brushes.SkyBlue;        //토
                else
                    tb.Foreground = Brushes.Gray;        //평일


                this.FontSize = calendar.FontSize * 0.5;
                this.HorizontalContentAlignment = HorizontalAlignment.Right;
                this.VerticalAlignment = VerticalAlignment.Bottom;                
            }

        }


    }
}
