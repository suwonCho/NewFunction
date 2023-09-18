using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace function.wpf
{
    /// <summary>
    /// Gridline 처리 개선 클래스
    /// </summary>
    public class GridX : Grid
    {
        /// <summary>
        /// GetCellPadding
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Thickness CellPadding
        {
            get
            {
                return (Thickness)this.GetValue(CellPaddingProperty);
            }
            set
            {
                this.SetValue(CellPaddingProperty, value);
            }
        }

        /// <summary>
        /// CellPadding 의존 속성
        /// </summary>
        public static readonly DependencyProperty CellPaddingProperty =
            DependencyProperty.Register("CellPadding", typeof(Thickness), typeof(GridX),
            new FrameworkPropertyMetadata(new Thickness(0.0, 0.0, 0.0, 0.0), FrameworkPropertyMetadataOptions.AffectsArrange,
                OnCellPaddingChanged));

        static void OnCellPaddingChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            GridX grid = (dependencyObject as GridX);
            foreach (UIElement uiElement in grid.Children)
            {
                ApplyMargin(grid, uiElement);
            }
        }

        // UIElementCollection 을 거칠 것 없이 곧바로 여기서 처리해도 됨.
        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            FrameworkElement childElement = visualAdded as FrameworkElement;
            ApplyMargin(this, childElement);

            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        }

        public static void ApplyMargin(GridX PaddingGrid, UIElement element)
        {
            FrameworkElement childElement = element as FrameworkElement;
            Thickness cellPadding = PaddingGrid.CellPadding;

            GridX childPaddingGrid = element as GridX;
            if (childPaddingGrid != null)
            {
                // 자식 노드가 PaddingGrid인 경우에는,
                // Margin 이 아닌 CellPadding을 설정한다.
                childPaddingGrid.CellPadding = cellPadding;
            }
            else
            {
                if (childElement != null)
                {
                    // 일반 자식 노드는 Margin을 설정
                    childElement.Margin = cellPadding;
                }
            }
        }


        /// <summary>
        /// DataGrid CellTemplate에서 사용 시 웃라인을 그리지 않는다.
        /// </summary>
        public static readonly DependencyProperty IsCellTemplateProperty =
            DependencyProperty.Register("IsCellTemplate", typeof(bool), typeof(GridX),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsArrange));
        
        public bool IsCellTemplate
        {
            get { return (bool)GetValue(IsCellTemplateProperty); }
            set { SetValue(IsCellTemplateProperty, value); }
        }





        #region OnRender
        Pen line = new Pen(Brushes.Black, 1);
        /// <summary>
        /// Border를 그리기 위한 OnRender 재정의
        /// </summary>
        /// <param name="dc"></param>
        protected override void OnRender(System.Windows.Media.DrawingContext dc)
        {

            base.OnRender(dc);

            GridX customGrid = this.Parent as GridX;

            

            if (customGrid == null)
            {

                if (!IsCellTemplate)
                {
                    //사각형을 그리고 배경색을 채운다.
                    dc.DrawRectangle(Background, line, new Rect(0, 0, this.ActualWidth, this.ActualHeight));
                }
                else
                {
                    Pen line2 = new Pen(Brushes.Black, 0);
                    //뱌경색을 채운다.
                    dc.DrawRectangle(Background, line2, new Rect(0, 0, this.ActualWidth, this.ActualHeight));

                    //라인 중 웻라인은 그리지 않는다.
                    dc.DrawLine(line, new Point(0, 0), new Point(0, this.ActualHeight));  //좌
                    dc.DrawLine(line, new Point(0, this.ActualHeight), new Point(this.ActualWidth, this.ActualHeight));  //아래
                    dc.DrawLine(line, new Point(this.ActualWidth, 0), new Point(this.ActualWidth, this.ActualHeight));  //우
                }
            }

            double linePoint = 0;
            double posFrom = 0.0;
            double posTo = 0.0;

            int rowCount = Math.Max(this.RowDefinitions.Count, 1);
            int columnCount = Math.Max(this.ColumnDefinitions.Count, 1);

            bool[,] rowCellStatus;
            bool[,] columnCellStatus;

            GetRowLineCellStatus(rowCount, columnCount, out rowCellStatus, out columnCellStatus);

            if (this.ColumnDefinitions.Count != 0)
            {
                for (int row = 0; row < rowCount - 1; row++)
                {
                    var r = this.RowDefinitions[row];

                    linePoint += r.ActualHeight;
                    for (int column = 0; column < columnCount; column++)
                    {
                        bool drawLine = rowCellStatus[row + 1, column];
                        posTo += this.ColumnDefinitions[column].ActualWidth;

                        if (drawLine == true)
                        {
                            dc.DrawLine(line, new Point(posFrom, linePoint), new Point(posTo, linePoint));
                        }

                        posFrom = posTo;
                    }

                    posFrom = 0.0;
                    posTo = 0.0;
                }
            }

            linePoint = 0;
            posFrom = 0.0;
            posTo = 0.0;

            if (this.RowDefinitions.Count != 0)
            {
                for (int column = 0; column < columnCount - 1; column++)
                {
                    var r = this.ColumnDefinitions[column];

                    linePoint += r.ActualWidth;
                    for (int row = 0; row < rowCount; row++)
                    {
                        bool drawLine = columnCellStatus[row, column + 1];
                        posTo += this.RowDefinitions[row].ActualHeight;

                        if (drawLine == true)
                        {
                            dc.DrawLine(line, new Point(linePoint, posFrom), new Point(linePoint, posTo));
                        }

                        posFrom = posTo;
                    }

                    posTo = 0.0;
                    posFrom = 0.0;
                }
            }
        }

        private void GetRowLineCellStatus(int rowCount, int columnCount, out bool[,] rowCellStatus, out bool[,] columnCellStatus)
        {
            rowCellStatus = new bool[rowCount, columnCount];
            columnCellStatus = new bool[rowCount, columnCount];

            foreach (UIElement element in this.Children)
            {
                int row = Grid.GetRow(element);
                int column = Grid.GetColumn(element);

                int spanCount = Grid.GetColumnSpan(element);

                for (int span = 0; span < spanCount; span++)
                {
                    try
                    {
                        rowCellStatus[row, column + span] = true;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }

                spanCount = Grid.GetRowSpan(element);
                for (int span = 0; span < spanCount; span++)
                {
                    columnCellStatus[row + span, column] = true;
                }
            }
        }
        #endregion OnRender


    }
}
