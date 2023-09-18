using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace function.wpf
{
    /// <summary>
    /// 사용자 컨트론 위치, 크기 조정 클래스(사용하지 말고 wuiSizable 사용 할것)
    /// </summary>
    public class Transform2
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Field
        ////////////////////////////////////////////////////////////////////////////////////////// Static
        //////////////////////////////////////////////////////////////////////////////// Private

        #region Field

        /// <summary>
        /// 마우스 어도너
        /// </summary>
        private static MouseAdorner _mouseAdorner = null;

        /// <summary>
        /// 첫번째 마우스 포인트
        /// </summary>
        private static MousePoint _firstMousePoint;

        /// <summary>
        /// 두번째 마우스 포인트
        /// </summary>
        private static MousePoint _secondMousePoint;

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////// Instance
        //////////////////////////////////////////////////////////////////////////////// Private

        #region Field

        /// <summary>
        /// 윈도우
        /// </summary>
        private Window window;

        /// <summary>
        /// 엘리먼트
        /// </summary>
        private FrameworkElement element1;

        /// <summary>
        /// 변환 그룹
        /// </summary>
        //private TransformGroup transformGroup = new TransformGroup();

        /// <summary>
        /// 회전 변환
        /// </summary>
        private RotateTransform rotateTransform = new RotateTransform();

        /// <summary>
        /// 스케일 변환
        /// </summary>
        private ScaleTransform scaleTransform = new ScaleTransform();

        /// <summary>
        /// 이동 변환
        /// </summary>
        private TranslateTransform translateTransform = new TranslateTransform();

        /// <summary>
        /// 드래그 여부
        /// </summary>
        private bool isDragging = false;

        /// <summary>
        /// 스케일 여부
        /// </summary>
        private bool isScaling = false;

        /// <summary>
        /// 회전 여부
        /// </summary>
        private bool isRotating = false;

        /// <summary>
        /// 첫번째 길이
        /// </summary>
        private double firstLength;

        /// <summary>
        /// 첫번째 각도
        /// </summary>
        private double firstAngle;

        /// <summary>
        /// 역변환 첫번째 위치
        /// </summary>
        private Point inverseFirstPoint;

        /// <summary>
        /// 첫번째 위치
        /// </summary>
        private Point firstPoint;

        /// <summary>
        /// 변환 여부
        /// </summary>
        private bool isTransformation = false;

        /// <summary>
        /// 이전 Z 인덱스
        /// </summary>
        private int previousZIndex;

        /// <summary>
        /// 드래그 가능 여부
        /// </summary>
        public bool canDrag = false;

        /// <summary>
        /// 스케일 가능 여부
        /// </summary>
        public bool canScale = false;

        /// <summary>
        /// 회전 가능 여부
        /// </summary>
        public bool canRotate = false;

        /// <summary>
        /// 위치 표시 여부
        /// </summary>
        public bool showPosition = false;

        public FrameworkElement Parent = null;

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Constructor
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 생성자 - Transform(window, element)


        public windowSetScale WinScale { get; set; } = null;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="window">윈도우</param>
        /// <param name="element">엘리먼트</param>
        public Transform2(Window win, FrameworkElement parent)
        {
            window = win;
            Parent = parent;
            //this.transformGroup.Children.Add(this.rotateTransform   );
            //this.transformGroup.Children.Add(this.scaleTransform    );
            //this.transformGroup.Children.Add(this.translateTransform);
        }

        public void Element_Add(FrameworkElement element)
        {
            element.PreviewMouseDown += element_PreviewMouseDown;
            element.PreviewMouseUp += element_PreviewMouseUp;
            element.MouseLeave += element_MouseLeave;

            TransformGroup tg = new TransformGroup();
            RotateTransform rotateTransform = new RotateTransform();
            ScaleTransform scaleTransform = new ScaleTransform();
            TranslateTransform translateTransform = new TranslateTransform();

            tg.Children.Add(rotateTransform);
            tg.Children.Add(scaleTransform);
            tg.Children.Add(translateTransform);

            element.RenderTransform = tg;
        }

        public TranslateTransform Element_GetTrans(FrameworkElement element)
        {
            TransformGroup tg;
            tg = element.RenderTransform as TransformGroup;

            if (tg == null) return null;

            TranslateTransform rtn = null;

            foreach(object o in tg.Children)
            {
                rtn = o as TranslateTransform;

                if (rtn != null) break;
            }

            return rtn;
        }



        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Private
        //////////////////////////////////////////////////////////////////////////////// Event

        #region 엘리먼트 PREVIEW 마우스 DOWN 처리하기 - element_PreviewMouseDown(sender, e)

        /// <summary>
        /// 엘리먼트 PREVIEW 마우스 DOWN 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void element_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            FrameworkElement element = (FrameworkElement)sender;
            this.element1 = element;

            TransformGroup tg = (TransformGroup)this.element1.RenderTransform;

            rotateTransform = (RotateTransform)tg.Children[0];
            scaleTransform = (ScaleTransform)tg.Children[1];
            translateTransform = (TranslateTransform)tg.Children[2];

            Point pt = e.GetPosition(this.window);
            Point pt3 = Mouse.GetPosition(Parent);

            Point pt2 = WinScale == null ? pt3 : WinScale.ChangeSizeGet(pt3);

            Debug.WriteLine($"[pt]{pt.ToString()} / [pt3]{pt3.ToString()} / [pt2]{pt2.ToString()}");


            if (e.LeftButton == MouseButtonState.Pressed && e.RightButton == MouseButtonState.Pressed && (this.canScale || this.canRotate))
            {
                this.isTransformation = !this.isTransformation;

                this.window.PreviewMouseMove -= window_DragPreviewMouseMove;

                if (this.isTransformation)
                {
                    this.firstPoint = pt2; // e.GetPosition(this.window);

                    this.window.PreviewMouseMove += window_TransformPreviewMouseMove;
                }
            }
            else
            {

                if (this.canDrag && e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed)
                {
                    if (!this.isTransformation)
                    {

                        this.firstPoint = pt2; // e.GetPosition(this.window);

                        this.window.PreviewMouseMove += window_DragPreviewMouseMove;

                        this.previousZIndex = Canvas.GetZIndex(element);

                        Canvas.SetZIndex(element, 20);
                    }
                }
            }

            if (this.showPosition)
            {
                if (!this.isTransformation)
                {
                    ChangePosition(_firstMousePoint, pt2); // e.GetPosition(this.window));
                }
            }
        }

        #endregion
        #region 엘리먼트 PREVIEW 마우스 UP 처리하기 - element_PreviewMouseUp(sender, e)

        /// <summary>
        /// 엘리먼트 PREVIEW 마우스 UP 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void element_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.element1 == null) return;


            if (e.LeftButton == MouseButtonState.Released && e.RightButton == MouseButtonState.Released)
            {
                if (!this.isTransformation)
                {
                    Canvas.SetZIndex(this.element1, this.previousZIndex);
                }

                if (this.canDrag)
                {
                    this.window.PreviewMouseMove -= window_DragPreviewMouseMove;

                    this.isDragging = false;
                }

                if (this.canScale || this.canRotate)
                {
                    if (!this.isTransformation)
                    {
                        this.window.PreviewMouseMove -= window_TransformPreviewMouseMove;
                    }

                    this.isRotating = this.isScaling = false;
                }

                if (this.showPosition)
                {
                    if (!this.isTransformation)
                    {
                        _mouseAdorner.RemoveObject(_firstMousePoint.Key);
                        _mouseAdorner.RemoveObject(_secondMousePoint.Key);
                    }
                    else
                    {
                        _mouseAdorner.RemoveObject(_secondMousePoint.Key);
                    }
                }


                //this.element1.RenderTransform = null;
                this.element1 = null;
            }
        }

        #endregion
        #region 엘리먼트 마우스 이탈시 처리하기 - element_MouseLeave(sender, e)

        /// <summary>
        /// 엘리먼트 마우스 이탈시 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void element_MouseLeave(object sender, MouseEventArgs e)
        {
            if (this.element1 == null) return;

            if (e.LeftButton == MouseButtonState.Released && e.RightButton == MouseButtonState.Released)
            {
                this.window.PreviewMouseMove -= window_DragPreviewMouseMove;
                this.window.PreviewMouseMove -= window_TransformPreviewMouseMove;

                Canvas.SetZIndex(this.element1, this.previousZIndex);

                this.isTransformation = this.isDragging = this.isRotating = this.isScaling = false;

                if (this.showPosition)
                {
                    if (_mouseAdorner != null)
                    {
                        _mouseAdorner.RemoveObject(_firstMousePoint.Key);
                        _mouseAdorner.RemoveObject(_secondMousePoint.Key);
                    }
                }
            }
        }

        #endregion

        #region 윈도우 드래그 PREVIEW 마우스 이동시 처리하기 - window_DragPreviewMouseMove(sender, e)

        public Point MousePointGet()
        {
            Point pt3 = Mouse.GetPosition(Parent);

            Point pt2 = WinScale == null ? pt3 : WinScale.ChangeSizeGet(pt3);

            return pt2;
        }


        /// <summary>
        /// 윈도우 드래그 PREVIEW 마우스 이동시 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void window_DragPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed)
            {
                Point currentPoint = MousePointGet(); // e.GetPosition(this.window);

                if (this.showPosition)
                {
                    ChangePosition(_firstMousePoint, currentPoint);
                }

                Drag(currentPoint);
            }
        }

        #endregion
        #region 윈도우 변환 PREVIEW 마우스 이동시 처리하기 - window_TransformPreviewMouseMove(sender, e)

        /// <summary>
        /// 윈도우 변환 PREVIEW 마우스 이동시 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void window_TransformPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (this.element1 == null) return;

            if (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed)
            {
                Point currentPoint = MousePointGet();  //e.GetPosition(this.window);

                Point distanceVector = new Point((currentPoint.X - this.firstPoint.X), (this.firstPoint.Y - currentPoint.Y));

                double currentLength = Math.Sqrt(distanceVector.X * distanceVector.X + distanceVector.Y * distanceVector.Y);
                double currentAngle = Math.Atan2(distanceVector.Y, distanceVector.X) * 180 / Math.PI;

                if (this.showPosition)
                {
                    ChangePosition(_secondMousePoint, currentPoint);
                }

                if (this.canScale)
                {
                    if (!this.isScaling)
                    {
                        this.firstLength = currentLength / this.scaleTransform.ScaleX;

                        this.isScaling = true;
                    }

                    if (this.firstLength > 0)
                    {
                        double scale = currentLength / this.firstLength;

                        this.scaleTransform.ScaleX = scale;
                        this.scaleTransform.ScaleY = scale;
                    }
                }

                if (this.canRotate)
                {
                    if (!this.isRotating)
                    {
                        this.firstAngle = currentAngle + this.rotateTransform.Angle;

                        this.isRotating = true;
                    }

                    double finalAngle = (this.firstAngle - currentAngle + 360) % 360;

                    this.rotateTransform.Angle = finalAngle;
                }

                Drag(this.firstPoint);
            }
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////// Function

        #region 드래그하기 - Drag(point)

        /// <summary>
        /// 드래그하기
        /// </summary>
        /// <param name="point">포인트</param>
        private void Drag(Point point)
        {
            if (this.element1 == null) return;


            if (!this.isDragging)
            {
                this.inverseFirstPoint = this.element1.TransformToVisual(this.Parent).Inverse.Transform(this.firstPoint);

                this.isDragging = true;
            }

            TransformGroup tg = (TransformGroup)element1.RenderTransform;


            Point translatePoint = this.element1.TranslatePoint(this.inverseFirstPoint, Parent);

            Point differencePoint = new Point(point.X - translatePoint.X, point.Y - translatePoint.Y);

            this.translateTransform.X += differencePoint.X;
            this.translateTransform.Y += differencePoint.Y;

            ///Console.WriteLine($"translateTransform {this.translateTransform.ToString()}");

            //this.element1.RenderTransform = this.transformGroup;
        }

        #endregion
        #region 위치 변경하기 - ChangePosition(mousePoint, point)

        /// <summary>
        /// 위치 변경하기
        /// </summary>
        /// <param name="mousePoint">마우스 포인트</param>
        /// <param name="point">포인트</param>
        private void ChangePosition(MousePoint mousePoint, Point point)
        {
            if (this.element1 == null) return;


            if (_mouseAdorner == null)
            {
                _mouseAdorner = new MouseAdorner((UIElement)this.window.Content);

                _firstMousePoint.Key = 0;
                _secondMousePoint.Key = 1;
            }

            mousePoint.Point = point;

            _mouseAdorner.AddObject(mousePoint);
        }

        #endregion
    }
}