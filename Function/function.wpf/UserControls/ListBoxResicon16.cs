using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace function.wpf
{
    public class ListBoxResicon16 : ListBox
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


        public ListBoxResicon16()
        {
            //그리드 모양
            //ftUnigrid = new FrameworkElementFactory(typeof(UniformGrid));
            //ftUnigrid.SetValue(UniformGrid.ColumnsProperty, 3);
            Columns = columns;

            //라디얼판넬
            //FrameworkElementFactory ftUnigrid = new FrameworkElementFactory(typeof(RadialPanel));

            //템플릿 생성
            DataTemplate dt = new DataTemplate();
            dt.DataType = typeof(ResIcon16);

            FrameworkElementFactory fStack = new FrameworkElementFactory(typeof(StackPanel));
            fStack.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            fStack.SetValue(StackPanel.HeightProperty, 20.0);
            dt.VisualTree = fStack;


            
            FrameworkElementFactory fRect = new FrameworkElementFactory(typeof(Image));                    
            fRect.SetValue(Image.SourceProperty, new Binding("ImageSource"));            
            fStack.AppendChild(fRect);

            FrameworkElementFactory fTb = new FrameworkElementFactory(typeof(TextBlock));
            fTb.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            fTb.SetValue(TextBlock.PaddingProperty, new Thickness(5, 0, 0, 0));
            fTb.SetValue(TextBlock.TextProperty, new Binding("ResIcon16Name"));
            fStack.AppendChild(fTb);

            ItemTemplate = dt;           
            ItemsSource = ResIcon16.All;
            

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

            ResIcon16 n = (ResIcon16)SelectedItem;
            Clipboard.SetText(n.ResIcon16Name);
        }

    }


    class ResIcon16 : DependencyObject
    {
        static ResIcon16[] ResIcon16s;
                
        static ResIcon16()
        {

            var pros = (from a in typeof(function.resIcon16).GetProperties()
                        where a.PropertyType.ToString() == "System.Drawing.Bitmap"
                        select a.Name).ToArray();
                       ;
            
            ResIcon16s = new ResIcon16[pros.Count()];

            for (int i = 0; i < ResIcon16s.Length; i++)
            {
                ResIcon16s[i] = new ResIcon16(pros[i]);
            }
        }

        public static ResIcon16[] All
        {
            get { return ResIcon16s; }
        }




        public static readonly DependencyProperty ResIcon16NameProperty = DependencyProperty.Register("ResIcon16Name", typeof(string), typeof(ResIcon16),
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
            ResIcon16 w = o as ResIcon16;            
        }


        public ImageSource ImageSource
        {
            get
            {
                var val = function.resIcon16.ResourceManager.GetObject(ResIcon16Name);

                if (val == null)
                {
                    return null;
                }
                else
                {
                    return wpfFnc.ImageSourceFormBitmap((System.Drawing.Bitmap)val);
                }
            }
        }

        public ResIcon16(string resIcon16Name)
        {
            ResIcon16Name = resIcon16Name;
        }
    }



}
