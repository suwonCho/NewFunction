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
    /// wucPlzWait.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class wucPlzWait : UserControl
    {


        /// <summary>
        /// 창에 wucPlzWait 표시 한다. 창의 메인 Content는 Grid이어야 한다.
        /// </summary>
        /// <param name="w"></param>
        /// <param name="TextString">메시지를 설정한다. null이면 기본 메시지</param>
        /// <returns></returns>
        public static wucPlzWait Show(Window w, string TextString = null)
        {
            Grid g = w.Content as Grid;
            wucPlzWait c = null;

            if (g != null)
            {
                c = new wucPlzWait(TextString);
                g.Children.Add(c);
                c.HorizontalAlignment = HorizontalAlignment.Center;
                c.VerticalAlignment = VerticalAlignment.Center;
                c.Width = 800;
                c.Height = 70;

                g.IsEnabled = false;
            }

            return c;
        }

        /// <summary>
        /// 창에 wucPlzWait 표시를 해제 한다.
        /// </summary>
        /// <param name="c"></param>
        public static void UnLoad(wucPlzWait c)
        {
            Grid g = c.Parent as Grid;

            if(g != null)
            {
                g.Children.Remove(c);

                g.IsEnabled = true;
            }
        }


        string textString = null;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="TextString">메시지를 설정한다. null이면 기본 메시지</param>
        public wucPlzWait(string TextString = null)
        {
            InitializeComponent();

            BringIntoView();
            textString = TextString;
            Init();            
        }


        public wucPlzWait()
        {
            InitializeComponent();

            BringIntoView();            
            Init();
        }


        public void Init()
        {

            if(textString != null)
            {
                txt.Text = textString;
            }


            string strPathGif = string.Format("file:\\{0}\\please_wait.GIF", Environment.CurrentDirectory);
            //string strPathGif = "pack://application:,,,/please_wait.gif";
            //Uri uri = new Uri(strPathGif, UriKind.Absolute);            
            //img.Source = uri;

            //BitmapImage bitmap = new BitmapImage();
            //bitmap.BeginInit();
            //bitmap.CacheOption = BitmapCacheOption.None;
            //bitmap.UriSource = uri;
            //bitmap.EndInit();
            //Img_GifAnimation.ImageSource = bitmap;

            //img.MediaEnded += Img_MediaEnded;            
        }

        private void Img_MediaEnded(object sender, RoutedEventArgs e)
        {
            //img.Position = new TimeSpan(0, 0, 1);
            //img.Play();
        }

    }
}
