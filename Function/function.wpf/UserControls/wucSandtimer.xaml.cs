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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace function.wpf
{
    /// <summary>
    /// wucSandtimer.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class wucSandtimer : UserControl
    {
        Storyboard sb;
        DoubleAnimation ani;

        public static readonly DependencyProperty StrechProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(wucSandtimer),
            new PropertyMetadata(Stretch.Fill, StrachChangeCallback));

        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StrechProperty); }
            set
            {
                SetValue(StrechProperty, value);
            }
        }



        static void StrachChangeCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            wucSandtimer w = o as wucSandtimer;            
            w.sizeChanged(new Size(w.ActualWidth, w.ActualHeight));
        }

        public wucSandtimer()
        {
            InitializeComponent();           

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {            

            //sb = new Storyboard();

            //ani = new DoubleAnimation();
            //ani.Duration = new Duration(TimeSpan.FromSeconds(2));
            //ani.From = 0;
            //ani.To = 360;
            //ani.By = 180;
            //ani.AutoReverse = false;     //리버스
            //ani.RepeatBehavior = RepeatBehavior.Forever;  //반복횟수


            //sb.Children.Add(ani);

            ////ani.BeginTime = TimeSpan.FromMilliseconds(2000);     //시작 지연시간            

            //Storyboard.SetTargetName(ani, "rTrans");
            //Storyboard.SetTargetProperty(ani, new PropertyPath(RotateTransform.AngleProperty));
            //sb.Begin(this);



            //using keyframe
            sb = new Storyboard();
            sb.BeginTime = TimeSpan.FromMilliseconds(0);
            sb.RepeatBehavior = RepeatBehavior.Forever;
            Storyboard.SetTargetName(sb, "rTrans");
            Storyboard.SetTargetProperty(sb, new PropertyPath(RotateTransform.AngleProperty));

            DoubleAnimationUsingKeyFrames frames = new DoubleAnimationUsingKeyFrames();
            frames.Duration = new Duration(TimeSpan.FromSeconds(2));

            double dg = 0;
            double ts = 0;
            int cnt = 12;
            for (int i = 0; i < cnt; i++)
            {
                dg = i * (360 / cnt);
                ts = i * (2000 / cnt);
                frames.KeyFrames.Add(new DiscreteDoubleKeyFrame(dg, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(ts))));
            }

            //frames.KeyFrames.Add(new DiscreteDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0))));
            //frames.KeyFrames.Add(new DiscreteDoubleKeyFrame(45, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(250))));
            //frames.KeyFrames.Add(new DiscreteDoubleKeyFrame(90, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(500))));
            //frames.KeyFrames.Add(new DiscreteDoubleKeyFrame(135, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(750))));
            //frames.KeyFrames.Add(new DiscreteDoubleKeyFrame(180, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(1000))));
            //frames.KeyFrames.Add(new DiscreteDoubleKeyFrame(225, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(1250))));
            //frames.KeyFrames.Add(new DiscreteDoubleKeyFrame(270, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(1500))));
            //frames.KeyFrames.Add(new DiscreteDoubleKeyFrame(315, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(1750))));
            //frames.KeyFrames.Add(new DiscreteDoubleKeyFrame(360, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(2000))));

            sb.Children.Add(frames);
            sb.Begin(this);
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            sizeChanged(e.NewSize);
        }


        private void sizeChanged(Size NewSize)
        {
            ln1.Points.Clear();
            ln2.Points.Clear();

            double w = NewSize.Width;
            double h = NewSize.Height;

            el.Stretch = Stretch;

            switch(Stretch)
            {
                case Stretch.Uniform:
                case Stretch.UniformToFill:
                    if (w < h)
                        h = w;
                    else
                        w = h;
                    break;
            }

            grd.Width = w;
            grd.Height = h;

            ln1.Points.Add(new Point(w * 0.5, h * 0.2));
            ln1.Points.Add(new Point(w * 0.75, h * 0.2));
            ln1.Points.Add(new Point(w * 0.6, h * 0.5));
            ln1.Points.Add(new Point(w * 0.65, h * 0.6));
            ln1.Points.Add(new Point(w * 0.35, h * 0.6));
            ln1.Points.Add(new Point(w * 0.4, h * 0.5));
            ln1.Points.Add(new Point(w * 0.25, h * 0.2));
            ln1.Points.Add(new Point(w * 0.5, h * 0.2));


            ln2.Points.Add(new Point(w * 0.5, h * 0.6));
            ln2.Points.Add(new Point(w * 0.65, h * 0.6));
            ln2.Points.Add(new Point(w * 0.75, h * 0.8));
            ln2.Points.Add(new Point(w * 0.25, h * 0.8));
            ln2.Points.Add(new Point(w * 0.35, h * 0.6));
            ln2.Points.Add(new Point(w * 0.5, h * 0.6));

            
        }


    }   //end class
}
