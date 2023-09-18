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
    /// wucNumpad.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class wucNumpad : UserControl
    {

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(wucNumpad),
    new FrameworkPropertyMetadata(0d, ValueChangeCallback));

        /// <summary>
        /// 원본 값
        /// </summary>
        public double Value
        {
            get { return double.Parse(sText) ; }
            set
            {
                SetValue(ValueProperty, value);                
            }
        }


        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(double?), typeof(wucNumpad),
    new FrameworkPropertyMetadata(null, ValueChangeCallback));

        /// <summary>
        /// 최대 입력 가능 수
        /// </summary>
        public double? MaxValue
        {
            get { return (double?)GetValue(MaxValueProperty); }
            set
            {
                SetValue(MaxValueProperty, value);
            }
        }


        public static readonly DependencyProperty DigitProperty = DependencyProperty.Register("Digit", typeof(int), typeof(wucNumpad),
    new FrameworkPropertyMetadata(0, ValueChangeCallback));

        /// <summary>
        /// 입력 가능 소수점 자리수
        /// </summary>
        public int Digit
        {
            get { return (int)GetValue(DigitProperty); }
            set
            {
                SetValue(DigitProperty, value);
            }
        }

        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(int), typeof(wucNumpad),
    new FrameworkPropertyMetadata(10, ValueChangeCallback));

        /// <summary>
        /// 입력 가능 소수점 자리수
        /// </summary>
        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set
            {
                SetValue(MaxLengthProperty, value);
            }
        }

        public static readonly DependencyProperty ShowZeroProperty = DependencyProperty.Register("ShowZero", typeof(bool), typeof(wucNumpad),
new FrameworkPropertyMetadata(true, ValueChangeCallback));

        /// <summary>
        /// 입력 가능 소수점 자리수
        /// </summary>
        public bool ShowZero
        {
            get { return (bool)GetValue(ShowZeroProperty); }
            set
            {
                SetValue(ShowZeroProperty, value);
            }
        }


        /// <summary>
        /// Label 정렬 설정
        /// </summary>
        public HorizontalAlignment ValueLabel_HorizontalContentAlignment
        {
            get { return lblText.HorizontalContentAlignment; }
            set
            {
                lblText.HorizontalContentAlignment = value;
            }
        }

        event RoutedEventHandler onInputEnd;

        /// <summary>
        /// 입력이 끝나면 발생 하는 이벤트(길이 만족 or Key Enter)
        /// </summary>
        public event RoutedEventHandler OnInputEnd
        {
            add { onInputEnd += value; }
            remove { onInputEnd -= value; }
        }


        event TextChangedEventHandler onTextChanged;

        /// <summary>
        /// 값이 변경 되면 발생
        /// </summary>
        public event TextChangedEventHandler OnTextChanged
        {
            add { onTextChanged += value; }
            remove { onTextChanged -= value; }
        }


        static void ValueChangeCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            wucNumpad w = o as wucNumpad;
            w.Init();
        }


        private void Init()
        {

            if (Digit < 1)
            {
                btnDot.Visibility = Visibility.Hidden;
                Grid.SetColumnSpan(btn0, 2);
            }
            else
            {
                btnDot.Visibility = Visibility.Visible;
                Grid.SetColumnSpan(btn0, 1);
            }

            Double v = (double)GetValue(ValueProperty);
            sText = v.ToString();
        }


        string stext = "0";
        
        public string sText
        {
            get { return stext; }
            set
            {
                Double d = 0;

                if (!Double.TryParse(value, out d)) d = 0;

                string[] t = value.ToString().Split(new char[] { '.' });

                string txt = stext;

                if(value == ".")
                {
                    txt = Digit > 1 ? "0." : "0";
                }
                else if (t.Length < 1)
                    txt = "0";
                else if (Digit < 1 || t.Length == 1)
                {
                    txt = t[0];

                    if (txt.Length > 0 && Fnc.Right(value, 1) == ".") txt += ".";
                }
                else
                {
                    if (t[1].Length > Digit) t[1] = t[1].Substring(0, Digit);
                    txt = $"{t[0]}.{t[1]}";
                }

                if (txt != "0." && txt.Length > 1 && txt.StartsWith("0") && !txt.StartsWith("0.")) txt = txt.Substring(1);

                if (!ShowZero && txt == "0") txt = "";

                if (txt.Length > MaxLength) txt = txt.Substring(0, MaxLength);

                //최대 입력 가능 수 확인
                if (MaxValue != null && Fnc.obj2Double(txt) > MaxValue.Value) txt = MaxValue.Value.ToString();

                if (txt != stext)
                {
                    stext = txt;
                    if(onTextChanged != null) onTextChanged(this, null);
                }

                if (stext.Length >= MaxLength && onInputEnd != null) onInputEnd(this, null);

                lblText.Content = txt;
            }
        }

        public wucNumpad()
        {
            InitializeComponent();            
        }       


        private string formatGet()
        {
            string rtn = "{0:#,##0";

            if(Digit > 0)
            {
                rtn += ".";

                for (int i = 0; i < Digit; i++)
                    rtn += "#";
            }

            rtn += "}";

            return rtn;
        }


        //버튼 클릭
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;

            string t = b.Content.ToString();

            switch(t)
            {
                case "←":
                    if (sText.Length > 2)
                        sText = sText.Substring(0, sText.Length - 1);
                    else
                        sText = ShowZero ? "0" : "";

                    break;

                default:
                    sText += t;
                    break;
            }

        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {

        }
    }
}
