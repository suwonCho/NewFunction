using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace function.wpf
{
    /// <summary>
    /// 런타입에서 사용자가 크기를 조절 할 수 있는 기능을 구현한다.(202205 AndonLCD 프로젝트 참조)
    /// </summary>
    public class wuiSizable
    {
        FrameworkElement el;
        FrameworkElement parent;
        Window mWin;

        #region Fields
        private bool isSelect = false;
        private bool isDragging = false;
        private bool isStretching = false;
        private bool stretchLeft = false;
        private bool stretchRight = false;
        private IInputElement inputElement = null;
        private double x, y = 0;
        private int zIndex = 0;

        private TranslateTransform tForm;        

        public double Padding = 5;
        public windowSetScale scale = null;

        enStrachtType StType = enStrachtType.None;
        #endregion

        public double X
        {
            get
            {
                double rtn = 0;

                var transformGroup = ((UIElement)InputElement).RenderTransform as TransformGroup;
                if (transformGroup == null)
                    return 0;

                var translateTransforms = from transform in transformGroup.Children
                                          where transform.GetType().Name == "TranslateTransform"
                                          select transform;

                foreach (TranslateTransform tt in translateTransforms)
                {
                    rtn = tt.X;
                    break;
                }

                return rtn;
            }
        }

        public double Y
        {
            get
            {
                double rtn = 0;

                var transformGroup = ((UIElement)InputElement).RenderTransform as TransformGroup;
                if (transformGroup == null)
                    return 0;

                var translateTransforms = from transform in transformGroup.Children
                                          where transform.GetType().Name == "TranslateTransform"
                                          select transform;

                foreach (TranslateTransform tt in translateTransforms)
                {
                    rtn = tt.Y;
                    break;
                }

                return rtn;
            }
        }


        /// <summary>
        /// 사용 여부
        /// </summary>
        public bool Enable
        {
            get; set;
        } = true;

        public wuiSizable(FrameworkElement element)
        {
            FrameworkElement p = (FrameworkElement)el.Parent;            
            init(element, p);
        }

        public wuiSizable(FrameworkElement element, FrameworkElement parent) 
        {
            init(element, parent);
        }

        private void init(FrameworkElement element, FrameworkElement parent)
        {
            el = element;

            TransformGroup tg = new TransformGroup();
            tg.Children.Add(new ScaleTransform());

            tForm = new TranslateTransform();
            tg.Children.Add(tForm);

            el.RenderTransform = tg;

            el.MouseLeftButtonDown += El_MouseLeftButtonDown;
            el.MouseLeftButtonUp += El_MouseLeftButtonUp;
            el.MouseLeave += El_MouseLeave;
            el.MouseEnter += El_MouseEnter;
            el.MouseMove += El_MouseMove;
                        
            parent.MouseMove += Parent_MouseMove;
            InputElement = (IInputElement)el;

            mWin = wpfFnc.ParentWindowGet(parent);
                        
        }


        public void SizeSet(double width, double height)
        {
            el.Width = width;
            el.Height = height;
        }

        public void PositionSet(double x, double y)
        {

            var transformGroup = ((UIElement)InputElement).RenderTransform as TransformGroup;
            if (transformGroup == null)
                return;

            var translateTransforms = from transform in transformGroup.Children
                                      where transform.GetType().Name == "TranslateTransform"
                                      select transform;

            foreach (TranslateTransform tt in translateTransforms)
            {
                tt.X = x;
                tt.Y = y;
            }
        }

        private void El_MouseMove(object sender, MouseEventArgs e)
        {
            if (!Enable || InputElement.IsMouseCaptured) return;
            //increment z-Order and pass it to the current element, 
            //so that it stays on top of all other elements
            //((Border)InputElement).SetValue(Canvas.ZIndexProperty, ZIndex++);

            el.SetValue(Canvas.ZIndexProperty, ZIndex++);

            // get coordinates
            //Border border = (Border)sender;
                       
            var rightLimit = el.ActualWidth - Padding; // border.Padding.Right;
            var bottomLimit = el.ActualHeight - Padding; // border.Padding.Bottom;
            var topLimit = Padding;
            var LeftLimit = Padding;

            var x = Mouse.GetPosition((IInputElement)sender).X;
            var y = Mouse.GetPosition((IInputElement)sender).Y;

            // figure out stretching directions - only to Right, Bottom 
            bool stretchRight = (x >= rightLimit && x < el.ActualWidth) ? true : false;
            bool stretchBottom = (y >= bottomLimit && y < el.ActualHeight) ? true : false;
            //왼쪽, 위쪽 추가
            bool stretchLeft = (x <= LeftLimit && x > 0) ? true : false;
            bool stretchTop = (y <= topLimit && y > 0) ? true : false;



            Console.WriteLine($"x:{x},y:{y} / X:{X},Y:{Y} / stretchTop:{stretchTop} stretchBottom:{stretchBottom} stretchLeft:{stretchLeft} stretchRight:{stretchRight}");

            //왼쪽, 위쪽 추가
            if(stretchLeft && stretchTop)
            {
                el.Cursor = Cursors.SizeNWSE;
                StType = enStrachtType.LeftTop;
            }
            else if (stretchLeft && stretchBottom)
            {
                el.Cursor = Cursors.SizeNESW;
                StType = enStrachtType.LeftBottom;
            }
            else if (stretchRight && stretchTop)
            {
                el.Cursor = Cursors.SizeNESW;
                StType = enStrachtType.RigthTop;
            }
            //set cursor to show stretch direction 
            else if (stretchRight && stretchBottom)
            {
                el.Cursor = Cursors.SizeNWSE;
                StType = enStrachtType.RightBottom;
            }
            else if (stretchRight && !stretchBottom)
            {
                el.Cursor = Cursors.SizeWE;
                StType = enStrachtType.Right;
            }
            else if (stretchBottom && !stretchRight)
            {
                el.Cursor = Cursors.SizeNS;
                StType = enStrachtType.Bottom;
            }
            else if(stretchLeft)
            {
                el.Cursor = Cursors.SizeWE;
                StType = enStrachtType.Left;
            }
            else if (stretchTop)
            {
                el.Cursor = Cursors.SizeNS;
                StType = enStrachtType.Top;
            }
            else //no stretch
            {
                el.Cursor = Cursors.Arrow;
                StType = enStrachtType.None;
            }

            //Console.WriteLine($"X:{x} , Y:{y} / rightLimit:{rightLimit} , bottomLimit:{bottomLimit} / stretchRight:{stretchRight} , stretchBottom:{stretchBottom} / IsStretching:{IsStretching} IsDragging:{IsDragging}");

        }

        private void El_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (InputElement.IsMouseCaptured)
            {
                InputElement.ReleaseMouseCapture();
                el.Cursor = Cursors.Arrow;
            }
        }

        private void Parent_MouseMove(object sender, MouseEventArgs e)
        {
            //if (IsDragging && e.LeftButton == MouseButtonState.Pressed) IsDragging = true;

            //Console.WriteLine("Parent_MouseMove");
            if (isSelect && e.LeftButton == MouseButtonState.Pressed && InputElement != null)
            {
                if (IsDragging)
                    Drag(sender);

                if (IsStretching)
                    Stretch(sender);
            }
        }

        private void El_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var rightLimit = el.ActualWidth - Padding; // el.Padding.Right;
            var bottomLimit = el.ActualHeight - Padding; //border.Padding.Bottom;

            var x = Mouse.GetPosition((IInputElement)el).X;
            var y = Mouse.GetPosition((IInputElement)el).Y;

            if (x < rightLimit && y < bottomLimit)
                el.Cursor = Cursors.Arrow;

            isSelect = true;
        }

        private void El_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            isSelect = false;

            el.Cursor = Cursors.Arrow;

        }

        private void El_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!Enable) return;

            x = Mouse.GetPosition(parent).X;
            y = Mouse.GetPosition(parent).Y;

            if (InputElement != null)
            {
                InputElement.CaptureMouse();

                isDragging = false;
                IsStretching = false;

                if(el.Cursor == Cursors.Arrow)
                    isDragging = true;
                else
                    IsStretching = true;
            }


            //if (!IsStretching)
                //IsDragging = true;

            //capture the new current element
            
        }

        private void Drag(object sender)
        {

            if (!Enable) return;
            //Console.WriteLine("Drag");

            el.Cursor = Cursors.Hand;

            // Retrieve the current position of the mouse.
            var newX = Mouse.GetPosition((IInputElement)parent).X;
            var newY = Mouse.GetPosition((IInputElement)parent).Y;

            // Reset the location of the object (add to sender's renderTransform newPosition minus currentElement's position
            var transformGroup = ((UIElement)InputElement).RenderTransform as TransformGroup;
            if (transformGroup == null)
                return;

            var translateTransforms = from transform in transformGroup.Children
                                      where transform.GetType().Name == "TranslateTransform"
                                      select transform;

            foreach (TranslateTransform tt in translateTransforms)
            {
                if (scale != null)
                {
                    tt.X += (newX - x) / scale.scale.ScaleX;
                    tt.Y += (newY - y) / scale.scale.ScaleY;
                }
                else
                {
                    tt.X += newX - x;
                    tt.Y += newY - y;
                }
            }
            

            //tForm.X = newX - X;
            //tForm.Y += newY - Y;

            // Update the beginning position of the mouse
            x = newX;
            y = newY;
        }
        private void Stretch(object sender)
        {
            Console.WriteLine("Stretch");


            // Retrieve the current position of the mouse.
            var mousePosX = Mouse.GetPosition((IInputElement)parent).X;
            var mousePosY = Mouse.GetPosition((IInputElement)parent).Y;


            //get coordinates
            //Border border = (Border)InputElement;
            var xDiff = mousePosX - x;
            var yDiff = mousePosY - y;
            var width = el.Width;
            var heigth = el.Height;


            //make sure not to resize to negative width or heigth
            xDiff = (el.Width + xDiff) > el.MinWidth ? xDiff : 30;
            yDiff = (el.Height + yDiff) > el.MinHeight ? yDiff : 30;

            if (scale != null)
            {
                xDiff = xDiff / scale.scale.ScaleX;
                yDiff = yDiff / scale.scale.ScaleY;
            }

            double top = 0;
            double left = 0;
            double t1;
            
            switch(StType)
            {
                case enStrachtType.Top:
                    t1 = DiffCalc(el.Height, yDiff);
                    el.Height -= t1;
                    top = t1;
                    break;

                case enStrachtType.Left:
                    t1 = DiffCalc(el.Width, xDiff);
                    el.Width -= t1;
                    left = t1;
                    break;

                case enStrachtType.LeftTop:
                    t1 = DiffCalc(el.Width, xDiff);
                    el.Width -= t1;
                    left = t1;

                    t1 = DiffCalc(el.Height, yDiff);
                    el.Height -= t1;
                    top = t1;
                    break;

                case enStrachtType.LeftBottom:
                    t1 = DiffCalc(el.Width, xDiff);
                    el.Width -= t1;
                    left = t1;

                    el.Height += yDiff;
                    break;

                case enStrachtType.Right:
                    el.Width += xDiff;
                    break;

                case enStrachtType.RigthTop:
                    el.Width += xDiff;

                    t1 = DiffCalc(el.Height, yDiff);
                    el.Height -= t1;
                    top = t1;
                    break;

                case enStrachtType.RightBottom:
                    el.Width += xDiff;
                    el.Height += yDiff;
                    break;               


                case enStrachtType.Bottom:
                    el.Height += yDiff;
                    break;
            }


            if(top != 0 || left != 0)
            {
                // Reset the location of the object (add to sender's renderTransform newPosition minus currentElement's position
                var transformGroup = ((UIElement)InputElement).RenderTransform as TransformGroup;
                if (transformGroup == null)
                    return;

                var translateTransforms = from transform in transformGroup.Children
                                          where transform.GetType().Name == "TranslateTransform"
                                          select transform;

                foreach (TranslateTransform tt in translateTransforms)
                {
                    tt.X += left;
                    tt.Y += top;
                }
            }

            // update current coordinates with the latest postion of the mouse
            x = mousePosX;
            y = mousePosY;
        }


        /// <summary>
        /// 기준값 이하기 되지 않도를 diff를 계산 하여 준다.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="diff"></param>
        /// <returns></returns>
        private double DiffCalc(Double value, double diff)
        {
            //기준값(최하 유지값)
            double ss = 30;
            double rtn = diff;

            if ((value - diff) < ss) rtn = value - ss;

            return rtn;
        }


        #region Properties
        public IInputElement InputElement
        {
            get { return this.inputElement; }
            set
            {
                this.inputElement = value;
                this.isDragging = false;
                this.isStretching = false;
            }
        }
     
     
        public int ZIndex
        {
            get { return this.zIndex; }
            set { this.zIndex = value; }
        }
        public bool IsDragging
        {
            get { return this.isDragging; }
            set
            {
                this.isDragging = value;
                this.isStretching = !this.isDragging;
            }
        }
        public bool IsStretching
        {
            get { return this.isStretching; }
            set
            {
                this.isStretching = value;
                this.IsDragging = !this.isStretching;
            }
        }
        public bool StretchLeft
        {
            get { return this.stretchLeft; }
            set { this.stretchLeft = value; this.stretchRight = !this.stretchLeft; }
        }
        public bool StretchRight
        {
            get { return this.stretchRight; }
            set { this.stretchRight = value; this.stretchLeft = !this.stretchRight; }
        }
        #endregion
    }
}
