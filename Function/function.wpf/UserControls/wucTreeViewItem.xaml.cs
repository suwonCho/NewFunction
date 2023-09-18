using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace function.wpf
{
    /// <summary>
    /// TreeView에 입력 할 항목 - image와 textblock로 되어 있음
    /// </summary>
    public partial class wucTreeViewItem : UserControl
    {

        /// <summary>
        /// wucTreeViewItem 클래스를 이용해 TreeViewItem의 헤더를 만들고 TreeViewItem를 반환한다.
        /// </summary>
        /// <param name="_resIcon16_Name"></param>
        /// <param name="_text"></param>
        /// <returns></returns>
        public static TreeViewItem TreeViewItemGet(string _resIcon16_Name, string _text, object Tag = null)
        {
            TreeViewItem t = new TreeViewItem();
            wucTreeViewItem wt = new wucTreeViewItem(_resIcon16_Name, _text);            
            t.Header = wt;            
            t.Tag = Tag;
            

            return t;
        }


        /// <summary>
        /// wucTreeViewItem 클래스를 이용해 생성된 TreeViewItem의 헤더를 수정한다. 각 파라메터의 값을 null로 하면 변경 하지 않는다.
        /// </summary>
        /// <param name="treeViewItem"></param>
        /// <param name="_resIcon16_Name"></param>
        /// <param name="_text"></param>
        /// <param name="Tag"></param>
        /// <returns>성공 여부</returns>
        public static bool TreeViewItemChange(object treeViewItem, string _resIcon16_Name, string _text, object Tag)
        {
            TreeViewItem t = treeViewItem as TreeViewItem;

            if (t == null) return false;

            wucTreeViewItem wt = t.Header as wucTreeViewItem;

            if (wt == null) return false;

            if (_resIcon16_Name != null) wt.ResIcon16_Name = _resIcon16_Name;
            if (_text != null) wt.TextString = _text;
            if (Tag != null) t.Tag = Tag;

            return true;

        }



        string resIcon16_Name = "";

        public string ResIcon16_Name
        {
            get { return resIcon16_Name; }
            set
            {
                var val = function.resIcon16.ResourceManager.GetObject(value);

                if (val == null)
                {
                    resIcon16_Name = "";
                    img.Source = null;
                }
                else
                {
                    img.Source = wpfFnc.ImageSourceFormBitmap((System.Drawing.Bitmap)val);
                    resIcon16_Name = value;
                }
            }
        }
        
        string textString = "";


        public string TextString
        {
            get { return textString; }
            set
            {
                textString = value;
                tb.Text = textString;
            }
        }

        public wucTreeViewItem()
        {
            InitializeComponent();
        }

        public wucTreeViewItem(string _resIcon16_Name, string _text)
        {
            InitializeComponent();

            ResIcon16_Name = _resIcon16_Name;
            TextString = _text;

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }




    }   //end class
}
