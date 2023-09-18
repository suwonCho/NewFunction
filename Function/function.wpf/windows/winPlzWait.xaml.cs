using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace function.wpf
{
    /// <summary>
    /// Window1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class winPlzWait : baseWindow
    {

        /// <summary>
        /// 창에 wucPlzWait 표시 한다. 창의 메인 Content는 Grid이어야 한다.
        /// </summary>
        /// <param name="w"></param>
        /// <param name="TextString">메시지를 설정한다. null이면 기본 메시지</param>
        /// <returns></returns>
        public static winPlzWait Load(Window w, string TextString = null)
        {
            Grid g = w.Content as Grid;
            winPlzWait c = null;

            if (g != null)
            {
                c = new winPlzWait(TextString);               

                g.IsEnabled = false;

                c.Show(w);
            }
            

            return c;
        }

        /// <summary>
        /// 창에 wucPlzWait 표시를 해제 한다.
        /// </summary>
        /// <param name="c"></param>
        public static void UnLoad(winPlzWait c)
        {
            Window w = c.parent as Window;

            if (w != null)
            {
                Grid g = w.Content as Grid;

                if (g != null)
                {
                    g.IsEnabled = true;
                }
            }

            c.Close();
        }

        string textString = null;
        public bool isClose = false;
        
        
        int cnt = 0;

        public winPlzWait()
        {
            InitializeComponent();
            this.Loaded += WinPlzWait_Loaded;
            this.Closed += WinPlzWait_Closed;
            
        }

        public winPlzWait(string TextString = null) : this()
        {
            textString = TextString;
        }


        private void WinPlzWait_Closed(object sender, EventArgs e)
        {
            
        }

        private void WinPlzWait_Loaded(object sender, RoutedEventArgs e)
        {
            this.Topmost = true;
            Init();
        }


        public void Init()
        {

            if (textString != null)
            {
                txt.Content = textString;
            }


            //string strPathGif = string.Format("file:\\{0}\\please_wait.GIF", Environment.CurrentDirectory);
            //Uri uri = new Uri(strPathGif, UriKind.Absolute);

            ////string strPathGif = "pack://application:,,,/please_wait.gif";            
            ////Uri uri = new Uri(strPathGif);

            //img.Source = uri;

            //BitmapImage bitmap = new BitmapImage();
            //bitmap.BeginInit();
            //bitmap.CacheOption = BitmapCacheOption.Default;
            //bitmap.UriSource = uri;            
            //Img_GifAnimation.ImageSource = bitmap;
            //bitmap.EndInit();

            //System.Windows.Forms.Application.DoEvents();

            //img.MediaEnded += Img_MediaEnded;            
        }



        private void Img_MediaEnded(object sender, RoutedEventArgs e)
        {

            //img.Position = new TimeSpan(0, 0, 1);
            //img.Play();
        }


    }
}

