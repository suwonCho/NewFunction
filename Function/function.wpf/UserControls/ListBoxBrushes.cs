using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;

namespace function.wpf
{
    public class ListBoxBrushes : ListBox
    {
        FrameworkElementFactory ftUnigrid;

        int columns = 3;

        public int Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                ftUnigrid = new FrameworkElementFactory(typeof(UniformGrid));
                ftUnigrid.SetValue(UniformGrid.ColumnsProperty, columns);
                ItemsPanel = new ItemsPanelTemplate(ftUnigrid);
            }
        }


        public ListBoxBrushes()
        {
            //그리드 모양
            //ftUnigrid = new FrameworkElementFactory(typeof(UniformGrid));
            //ftUnigrid.SetValue(UniformGrid.ColumnsProperty, 3);
            Columns = columns;

            //라디얼판넬
            //FrameworkElementFactory ftUnigrid = new FrameworkElementFactory(typeof(RadialPanel));

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
            fTb.SetValue(TextBlock.PaddingProperty, new Thickness(5, 0, 0, 0));
            fTb.SetValue(TextBlock.TextProperty, new Binding("Name"));
            fStack.AppendChild(fTb);

            ItemTemplate = dt;           
            ItemsSource = NamedBrush.All;

            ////항목 추가
            //foreach (NamedBrush s in NamedBrush.All)
            //{
                
            //    Rectangle rect = new Rectangle();
            //    rect.Width = 12;
            //    rect.Height = 12;
            //    rect.Margin = new Thickness(4);
            //    rect.Fill = s.Brush;


            //    Items.Add(rect);

            //    ToolTip tip = new System.Windows.Controls.ToolTip();
            //    tip.Content = s;
            //    rect.ToolTip = tip;
            //}

            //SelectedValuePath = "Name";
            

        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            ClipboardSet();
        }


        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if(e.Key == Key.C && Keyboard.Modifiers == ModifierKeys.Control)
            {
                ClipboardSet();
            }
        }

       private void ClipboardSet()
        {
            if (SelectedItem == null) return;

            NamedBrush n = (NamedBrush)SelectedItem;
            Clipboard.SetText(n.Name);
        }

    }
}
