using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace function.wpf
{

    /// <summary>
    /// 마우스 포인트
    /// </summary>
    public struct MousePoint
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Field
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 키 - Key

        /// <summary>
        /// 키
        /// </summary>
        public int Key;

        #endregion
        #region 포인트 - Point

        /// <summary>
        /// 포인트
        /// </summary>
        public Point Point;

        #endregion
    }

    /// <summary>
    /// 마우스 어도너
    /// </summary>
    public class MouseAdorner : Adorner
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Field
        ////////////////////////////////////////////////////////////////////////////////////////// Private

        #region Field

        /// <summary>
        /// 마우스 포인트 딕셔너리
        /// </summary>
        private Dictionary<int, MousePoint> mousePointDictionary;

        /// <summary>
        /// 스트로크 펜
        /// </summary>
        private Pen strokePen = new Pen(Brushes.Gray, 1);

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Constructor
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 생성자 - MouseAdorner(element)

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="element">엘리먼트</param>
        public MouseAdorner(UIElement element) : base(element)
        {
            this.mousePointDictionary = new Dictionary<int, MousePoint>();

            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(element);

            adornerLayer.Add(this);
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 객체 추가하기 - AddObject(mousePoint)

        /// <summary>
        /// 객체 추가하기
        /// </summary>
        /// <param name="mousePoint">마우스 포인트</param>
        public void AddObject(MousePoint mousePoint)
        {
            this.mousePointDictionary[mousePoint.Key] = mousePoint;

            InvalidateVisual();
        }

        #endregion
        #region 객체 제거하기 - RemoveObject(key)

        /// <summary>
        /// 객체 제거하기
        /// </summary>
        /// <param name="key">키</param>
        public void RemoveObject(int key)
        {
            if (this.mousePointDictionary.ContainsKey(key))
            {
                this.mousePointDictionary.Remove(key);

                InvalidateVisual();
            }
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////// Protected

        #region 렌더링시 처리하기 - OnRender(drawingContext)

        /// <summary>
        /// 렌더링시 처리하기
        /// </summary>
        /// <param name="drawingContext">드로잉 컨텍스트</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            foreach (MousePoint mousePoint in this.mousePointDictionary.Values)
            {
                if (mousePoint.Key == 0)
                {
                    drawingContext.DrawEllipse
                    (
                        Brushes.Tomato,
                        this.strokePen,
                        new Point(mousePoint.Point.X, mousePoint.Point.Y),
                        10.0,
                        10.0
                    );
                }
                else
                {
                    drawingContext.DrawEllipse
                    (
                        Brushes.Turquoise,
                        this.strokePen,
                        new Point(mousePoint.Point.X, mousePoint.Point.Y),
                        10.0,
                        10.0
                    );
                }
            }

            IsHitTestVisible = false;

            base.OnRender(drawingContext);
        }

        #endregion
    }
}