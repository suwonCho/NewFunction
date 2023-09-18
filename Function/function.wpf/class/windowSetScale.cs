using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace function.wpf
{
    public class windowSetScale
    {

        double orginalWidth, originalHeight;

        Window win = null;
        FrameworkElement fe = null;
        public ScaleTransform scale = new ScaleTransform();

        public windowSetScale(Window _win)
        {
            win = _win;
            orginalWidth = win.Width;
            originalHeight = win.Height;

            if (win.WindowState == WindowState.Maximized)
            {
                ChangeSize(win.ActualWidth, win.ActualHeight);
            }

            win.SizeChanged += new SizeChangedEventHandler(Window1_SizeChanged);
        }


        public windowSetScale(FrameworkElement _fe)
        {
            fe = _fe;
            orginalWidth = fe.Width;
            originalHeight = fe.Height;

            fe.SizeChanged += Fe_SizeChanged;
        }

        private void Fe_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ChangeSize(e.NewSize.Width, e.NewSize.Height);
        }

        void Window1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ChangeSize(e.NewSize.Width, e.NewSize.Height);
        }


        private void ChangeSize(double width, double height)
        {

            scale.ScaleX = width / orginalWidth;
            scale.ScaleY = height / originalHeight;

            FrameworkElement rootElement = null;

            if (win != null)
                rootElement = win.Content as FrameworkElement;
            else if (fe != null)
                rootElement = fe;


            rootElement.LayoutTransform = scale;
        }

        public Point ChangeSizeGet(Point pt)
        {
            Point rtn = new Point();

            rtn.X = pt.X * scale.ScaleX;
            rtn.Y = pt.Y * scale.ScaleY;

            return rtn;
        }



    }   //end class
}
