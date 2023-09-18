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
    public enum RadilPanelOrientation
    {
        ByWidth,
        ByHeight
    }

    public class RadialPanel : Panel
    {
        public static readonly DependencyProperty OrientationProperty;

        bool showPieLine;
        double angleEach;
        Size sizeLagerst;
        double radius;
        double outerEdgeFromCenter;
        double innerEdgeFromCenter;


        static RadialPanel()
        {
            OrientationProperty = DependencyProperty.Register("Orientation", typeof(RadilPanelOrientation), typeof(RadialPanel),
                    new FrameworkPropertyMetadata(RadilPanelOrientation.ByWidth, FrameworkPropertyMetadataOptions.AffectsMeasure));
        }

        public RadilPanelOrientation Orientation
        {
            set { SetValue(OrientationProperty, value); }
            get
            {
                return (RadilPanelOrientation)GetValue(OrientationProperty);
            }
        }

        public bool ShowPieLines
        {
            set
            {
                if (value != showPieLine) InvalidateVisual();

                showPieLine = value;
            }
            get { return showPieLine; }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (InternalChildren.Count < 1) return new Size(0, 0);

            angleEach = 360.0 / InternalChildren.Count;
            sizeLagerst = new Size(0, 0);

            foreach (UIElement c in InternalChildren)
            {
                c.Measure(new Size(Double.PositiveInfinity, double.PositiveInfinity));

                sizeLagerst.Width = Math.Max(sizeLagerst.Width, c.DesiredSize.Width);
                sizeLagerst.Height = Math.Max(sizeLagerst.Height, c.DesiredSize.Height);
            }

            if (Orientation == RadilPanelOrientation.ByWidth)
            {
                //중심에서 엘리번트 변까리 거리를 계산
                innerEdgeFromCenter = sizeLagerst.Width / 2 / Math.Tan(Math.PI * angleEach / 360);
                outerEdgeFromCenter = innerEdgeFromCenter + sizeLagerst.Height;

                //가장큰자식을 기눙으로 원의 반지름을 계산
                radius = Math.Sqrt(Math.Pow(sizeLagerst.Width / 2, 2) + Math.Pow(outerEdgeFromCenter, 2));
            }
            else
            {
                //중심에서 엘리번트 변까리 거리를 계산
                innerEdgeFromCenter = sizeLagerst.Height / 2 / Math.Tan(Math.PI * angleEach / 360);
                outerEdgeFromCenter = innerEdgeFromCenter + sizeLagerst.Width;

                //가장큰자식을 기눙으로 원의 반지름을 계산
                radius = Math.Sqrt(Math.Pow(sizeLagerst.Height / 2, 2) + Math.Pow(outerEdgeFromCenter, 2));
            }



            //원의 크기를 반환
            return new Size(2 * radius, 2 * radius);

        }


        protected override Size ArrangeOverride(Size sizeFinal)
        {
            //Grid등 다른 판넬에 child로 추가 시 *(auto)로 높이 설정시 화면에 꽉 안차는 문제 있음 - 추후 확인


            double angleChild = 0;
            Point ptCenter = new Point(sizeFinal.Width / 2, sizeFinal.Height / 2);
            double multiplier = Math.Min(sizeFinal.Width / (2 * radius), sizeFinal.Height / (2 * radius));

            foreach (UIElement c in InternalChildren)
            {
                //각 자식의 RenderTransform 리셋
                c.RenderTransform = Transform.Identity;

                if (Orientation == RadilPanelOrientation.ByWidth)
                {
                    //상단에 자식을 위치
                    c.Arrange(new Rect(ptCenter.X - multiplier * sizeLagerst.Width / 2, ptCenter.Y - multiplier * outerEdgeFromCenter, multiplier * sizeLagerst.Width, multiplier * sizeLagerst.Height));
                }
                else
                {
                    //오른쪽에 자식을 위치
                    c.Arrange(new Rect(ptCenter.X + multiplier * innerEdgeFromCenter, ptCenter.Y - multiplier * sizeLagerst.Height / 2, multiplier * sizeLagerst.Width, multiplier * sizeLagerst.Height));
                }

                //원 주위로 자식을 회전(자식에 대해 상대적)
                Point pt = TranslatePoint(ptCenter, c);
                c.RenderTransform = new RotateTransform(angleChild, pt.X, pt.Y);

                //각도 증가
                angleChild += angleEach;
            }

            return sizeFinal;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (ShowPieLines)
            {
                Point ptCenter = new Point(RenderSize.Width / 2, RenderSize.Height / 2);
                double multiplier = Math.Min(RenderSize.Width / (2 * radius), RenderSize.Height / (2 * radius));
                Pen pen = new Pen(SystemColors.WindowTextBrush, 1);
                pen.DashStyle = DashStyles.Dash;

                //원을 출력
                dc.DrawEllipse(null, pen, ptCenter, multiplier * radius, multiplier * radius);

                //각도를 초기화
                double angleChild = -angleEach / 2;

                if (Orientation == RadilPanelOrientation.ByWidth) angleChild += 90;

                //각 자식에 대해 방사하는 선 표시
                foreach (UIElement c in InternalChildren)
                {
                    dc.DrawLine(pen, ptCenter,
                        new Point(ptCenter.X + multiplier * radius * Math.Cos(2 * Math.PI * angleChild / 360),
                        ptCenter.Y + multiplier * radius * Math.Sin(2 * Math.PI * angleChild / 360)));

                    angleChild += angleEach;
                }

            }
        }



    }   //end class
}
