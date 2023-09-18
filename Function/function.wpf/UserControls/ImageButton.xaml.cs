using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace function.wpf
{
    /// <summary>
    /// ImageButton.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ImageButton : Button
    {
        public static readonly DependencyProperty ResIcon16NameProperty = DependencyProperty.Register("ResIcon16Name", typeof(string), typeof(ImageButton),
    new FrameworkPropertyMetadata((object)null, ResIcon16NameChangeCallback));

        /// <summary>
        /// function ResIcon16 이름으로 아이콘을 설정 한다.
        /// </summary>
        [Description("function ResIcon16 이름으로 아이콘을 설정 한다.")]
        public string ResIcon16Name
        {
            get { return (string)GetValue(ResIcon16NameProperty); }
            set { SetValue(ResIcon16NameProperty, value); }
        }

        static void ResIcon16NameChangeCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ImageButton w = o as ImageButton;
            w.ResIcon16NameChange();
        }
        
        public void ResIcon16NameChange()
        {          
            try
            {
                var val = function.resIcon16.ResourceManager.GetObject(ResIcon16Name);

                if (val == null)
                {                    
                    im.Source = null;
                }
                else
                {
                    im.Source = wpfFnc.ImageSourceFormBitmap((System.Drawing.Bitmap)val);                    
                }
            }
            catch { }
          
        }


        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(ImageButton),
           new PropertyMetadata(System.Windows.Controls.Orientation.Horizontal, OrientationChangeCallback));

        /// <summary>
        /// 컨트롤의 방향을 가져오거나 설정 합니다.
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set
            {
                SetValue(OrientationProperty, value);
            }
        }

        static void OrientationChangeCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ImageButton w = o as ImageButton;
            w.sp.Orientation = w.Orientation;
        }



        public double Image_Width
        {
            get { return im.Width; }
            set
            {
                im.Width = value;
            }
        }

        public double Image_Height
        {
            get { return im.Height; }
            set
            {
                im.Height = value;
            }
        }

        public HorizontalAlignment TextBlock_HorizontalAlignment
        {
            get { return txt.HorizontalAlignment; }
            set
            {
                txt.HorizontalAlignment = value;
            }
        }

        public VerticalAlignment TextBlock_VerticalAlignment
        {
            get { return txt.VerticalAlignment; }
            set
            {
                txt.VerticalAlignment = value;
            }
        }


        public void ImageSet(System.Drawing.Bitmap img)
        {
            if (img == null) return;

            im.Source = wpfFnc.ImageSourceFormBitmap((System.Drawing.Bitmap)img);
        }

        public string Text
        {
            get { return txt.Text; }
            set { txt.Text = value; }
        }

        public Object Content
        {
            get { return base.Content; }
            set
            {
                Object o = value;
            }
        }


        public ImageButton()
        {
            InitializeComponent();

            LinearGradientBrush gb = new LinearGradientBrush();
            
        }
    }
}
