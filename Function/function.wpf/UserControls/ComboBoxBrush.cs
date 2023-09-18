using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace function.wpf
{
    public class ComboBoxBrush : ComboBox
    {
        FrameworkElementFactory fUniGrid;

        public ComboBoxBrush()
        {
            ItemsSource = NamedBrush.All;

            //템플릿 생성
            DataTemplate dt = new DataTemplate();
            dt.DataType = typeof(NamedBrush);

            FrameworkElementFactory fStack = new FrameworkElementFactory(typeof(StackPanel));
            fStack.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            dt.VisualTree = fStack;

            FrameworkElementFactory fRect = new FrameworkElementFactory(typeof(Label));
            fRect.SetValue(Label.BorderBrushProperty, SystemColors.WindowTextBrush);
            fRect.SetValue(Label.ContentProperty, " ");
            fRect.SetValue(Label.BackgroundProperty, new Binding("Brush"));
            fStack.AppendChild(fRect);

            FrameworkElementFactory fTb = new FrameworkElementFactory(typeof(TextBlock));
            fTb.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            fTb.SetValue(TextBlock.TextProperty, new Binding("Name"));
            fStack.AppendChild(fTb);

            ItemTemplate = dt;

            fUniGrid = new FrameworkElementFactory(typeof(UniformGrid));
            fUniGrid.SetValue(UniformGrid.ColumnsProperty, columns);            
            ItemsPanel = new ItemsPanelTemplate(fUniGrid);
        }


        private int columns = 3;

        public int Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                fUniGrid.SetValue(UniformGrid.ColumnsProperty, columns);
                ItemsPanel = new ItemsPanelTemplate(fUniGrid);
            }
        }

        /// <summary>
        /// 현재 선택된 브러쉬의 이름을 가져오거나 이름으로 브러쉬를 선택 한다.
        /// </summary>
        public string BrushName
        {
            get
            {
                NamedBrush n = this.SelectedItem as NamedBrush;

                return n == null ? string.Empty : n.Name;
            }
            set
            {
                var rtn = from br in NamedBrush.All
                          where br.Name == value
                          select br;

                if(rtn.Count() > 0)
                {
                    this.SelectedItem = rtn.First();
                }
                else
                {
                    this.SelectedIndex = -1;
                }

            }
        }


    }


    /// <summary>
    /// ComboBoxBrush에서 사용 하는 Brushes 처리 클래스
    /// </summary>
    public class NamedBrush
    {
        static NamedBrush[] nbrushes;

        string name;
        Brush brush;

        static NamedBrush()
        {
            PropertyInfo[] ps = typeof(Brushes).GetProperties();
            nbrushes = new NamedBrush[ps.Length];

            for(int i=0;i<ps.Length;i++)
            {
                nbrushes[i] = new NamedBrush(ps[i].Name, (Brush)ps[i].GetValue(null, null));
            }
        }

        public NamedBrush(string str, Brush br)
        {
            this.name = str;
            this.brush = br;
        }

        public static NamedBrush[] All
        {
            get { return nbrushes; }
        }

        public Brush Brush
        {
            get { return brush; }
        }

        public string Name { get { return name; } }

        public override string ToString()
        {
            return name;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

    }
}
