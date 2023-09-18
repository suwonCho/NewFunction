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
using function;
using System.Windows.Media.Animation;

namespace function.wpf
{
    /// <summary>
    /// wucTriangle.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class wucTriangle : UserControl
    {
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(wucTriangle),
    new PropertyMetadata(Brushes.Transparent, FillChangeCallback));

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set
            {
                SetValue(FillProperty, value);
            }
        }


        static void FillChangeCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            wucTriangle w = o as wucTriangle;
            Size s = new Size(w.ActualWidth, w.ActualHeight);

            w.lastBrush = w.Fill;
            w.sizeChanged(s);
        }


        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(enStatus), typeof(wucTriangle),
    new PropertyMetadata(enStatus.None, StatusChangeCallback));

        public enStatus Status
        {
            get { return (enStatus)GetValue(StatusProperty); }
            set
            {
                SetValue(StatusProperty, value);
            }
        }


        static void StatusChangeCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            wucTriangle w = o as wucTriangle;

            enStatus oldSt = (enStatus)e.OldValue;
            enStatus newSt = (enStatus)e.NewValue;

            w.statusChanged(oldSt, newSt);
        }





        ColorAnimation ani;
        Storyboard stb;
        private Brush lastBrush = Brushes.Transparent;

        public wucTriangle()
        {
            InitializeComponent();


            ani = new ColorAnimation();
            ani.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            ani.From = Colors.Transparent;
            ani.To = Colors.Transparent;
            ani.RepeatBehavior = new RepeatBehavior(8);
            ani.AutoReverse = true;

            Storyboard.SetTargetProperty(ani, new PropertyPath("Fill.Color", null));
            stb = new Storyboard();
            stb.Children.Add(ani);
        }
     
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            sizeChanged(e.NewSize);
        }

        private void sizeChanged(Size NewSize)
        {
            ln1.Points.Clear();

            double w = NewSize.Width;
            double h = NewSize.Height;

            //Points="0 0,300 150, 0 300"

            ln1.Points.Add(new Point(0, 0));
            ln1.Points.Add(new Point(w, h * 0.5));
            ln1.Points.Add(new Point(0, h));
            ln1.Points.Add(new Point(0, 0));

            ln1.Fill = lastBrush;
        }

        private void statusChanged(enStatus oldStaus, enStatus newStatus)
        {

            try
            {
                if (newStatus != enStatus.Done && newStatus != enStatus.OK)
                {
                    lastBrush = new SolidColorBrush(function.wpf.wpfFnc.StatusBackColorGet(enStatus.None));
                    ln1.Fill = lastBrush;
                    return;
                }

                ani.From = function.wpf.wpfFnc.StatusBackColorGet(newStatus);

                //stb.Completed            
                stb.Begin(ln1);

                lastBrush = new SolidColorBrush(function.wpf.wpfFnc.StatusBackColorGet(newStatus));
            }
            catch { }

        }


    }
}
