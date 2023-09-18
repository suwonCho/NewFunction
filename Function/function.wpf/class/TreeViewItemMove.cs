using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace function.wpf
{
    /// <summary>
    /// TreeView 아이템 이동 처리를 해준다. 마우스 드래그 드롭 및 키보드 이동(컨트롤버튼 + 위,아래,페이지업/다운 버튼)
    /// </summary>
    public class TreeViewItemMove
    {

        TreeView treeView;

        Point _sPt;
        bool _isDrag = false;


        public TreeViewItemMove(TreeView tv)
        {
            treeView = tv;

            treeView.AllowDrop = true;
            treeView.PreviewMouseMove += treeView_PreviewMouseMove;
            treeView.PreviewMouseLeftButtonDown += treeView_PreviewMouseLeftButtonDown;
            treeView.Drop += treeView_Drop;

            
            treeView.PreviewKeyDown += TreeView_PreviewKeyDown;
        }
        

        private void TreeView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TreeViewItem ti = treeView.SelectedItem as TreeViewItem;

            if (ti == null) return;

            if((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key != Key.LeftCtrl)
            {
                int idx = treeView.Items.IndexOf(ti);

                int newIdx = -1;
                switch(e.Key)
                {
                    case Key.Up:
                        if (idx > 0) newIdx = idx - 1;
                        break;

                    case Key.Down:
                        if (idx < treeView.Items.Count) newIdx = idx + 1;
                        break;

                    case Key.PageUp:
                        newIdx = 0;
                        break;

                    case Key.PageDown:
                        newIdx = treeView.Items.Count - 1;
                        break;
                }


                if (newIdx == -1) return;

                if (newIdx >= treeView.Items.Count) return;

                treeView.Items.Remove(ti);

                treeView.Items.Insert(newIdx, ti);

                ti.IsSelected = true;
            }
        }

        
        private void treeView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed && !_isDrag)
            {
                Point pt = e.GetPosition(null);
                if (Math.Abs(pt.X - _sPt.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(pt.Y - _sPt.Y) > SystemParameters.MinimumVerticalDragDistance)
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
            try
            {
                _isDrag = true;

                object temp = this.treeView.SelectedItem;
                DataObject data = null;

                data = new DataObject("inadt", temp);

                if (data != null)
                {
                    DragDropEffects dde = DragDropEffects.Move;

                    if (e.RightButton == MouseButtonState.Pressed)
                    {
                        dde = DragDropEffects.All;
                    }

                    DragDropEffects de = DragDrop.DoDragDrop(treeView, data, dde);
                }
            }
            catch
            {

            }
            finally
            {
                _isDrag = false;
            }
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

                si.IsSelected = true;

            }           

        }

    }
}
