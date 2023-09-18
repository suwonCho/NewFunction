using System;
using System.Collections.Generic;
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

namespace function.wpf.test
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TreeViewDragDrop : Window
    {
        public TreeViewDragDrop()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TreeInit(treeView);
            TreeInit(treeView2);

            function.wpf.TreeViewItemMove tm = new function.wpf.TreeViewItemMove(treeView2);
        }

        private void TreeInit(TreeView tv)
        {
            for (int i = 0; i < 11; i++)
            {
                TreeViewItem ti = new TreeViewItem();
                ti.Header = $"Itme{i}";


                for(int j = 1; j < 4;j++)
                {
                    TreeViewItem t2 = new TreeViewItem();
                    t2.Header = $"SubItme{j}";

                    ti.Items.Add(t2);
                }
                tv.Items.Add(ti);               

            }
        }



        Point _sPt;
        bool _isDrag = false;

        private void treeView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed && !_isDrag)
            {
                Point pt = e.GetPosition(null);
                if(Math.Abs(pt.X - _sPt.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(pt.Y - _sPt.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    StartDrag(e);
                }
            }            
        }




        private void treeView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _sPt = e.GetPosition(null);
        }


        private void StartDrag(MouseEventArgs e)
        {
            _isDrag = true;

            object temp = this.treeView.SelectedItem;
            DataObject data = null;

            data = new DataObject("inadt", temp);

            if(data != null)
            {
                DragDropEffects dde = DragDropEffects.Move;

                Console.WriteLine("dde");

                if(e.RightButton == MouseButtonState.Pressed)
                {
                    dde = DragDropEffects.All;

                    Console.WriteLine("dde all");
                }

                DragDropEffects de = DragDrop.DoDragDrop(treeView, data, dde);

                Console.WriteLine("dde drop");
            }

            _isDrag = false;


        }

        private void treeView_Drop(object sender, DragEventArgs e)
        {            
            UIElement ui = treeView.InputHitTest(e.GetPosition(treeView)) as UIElement;

            TreeViewItem tItem = treeView.TreeViewItemFormChild(ui);

            if (tItem != null)
            {
                Console.WriteLine(tItem.Header.ToString());

                int idx = treeView.Items.IndexOf(tItem);

                TreeViewItem si = treeView.SelectedItem as TreeViewItem;

                int idx2 = treeView.Items.IndexOf(si);

                if (idx > idx2) idx--;

                treeView.Items.Remove(si);

                treeView.Items.Insert(idx, si);

            }
            else
                Console.WriteLine("Null.....");

        }

        private void treeView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var ui = sender as UIElement;
            //var tItem = ui.FindCommonVisualAncestor( ui);

            TreeViewItem tItem = treeView.TreeViewItemFormChild(ui);

            if (tItem != null)
                Console.WriteLine(tItem.Header.ToString());
            else
                Console.WriteLine("Null.....");
        }


    }
}
