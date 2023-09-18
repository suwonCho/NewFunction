using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;



namespace function.wpf
{
    public static class cStatic
    {
        public static TreeViewItem TreeViewItemFormChild(this TreeView treeView, UIElement child)
        //public TreeViewItem TreeViewItemFormChild(UIElement child)
        {
            UIElement chd = child;

            while ((chd != null) && !(chd is TreeViewItem))
                chd = VisualTreeHelper.GetParent(chd) as UIElement;

            return chd as TreeViewItem;
        }


        public static bool TreeViewItemMoveCheck(this TreeView treeView, object SourceItem, object TargetItem)
        {
            return true;
        }
    }
}
