using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace function.wpf.test
{
    /// <summary>
    /// DataGridDragDrop.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DataGridDragDrop : Window
    {
        DataTable dt;
        DataTable dtLeft;
        function.wpf.DataGridItemMove dm = new DataGridItemMove();

        public DataGridDragDrop()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            dgLeft.AllowDrop = true;
            dgLeft.PreviewMouseLeftButtonDown += DgLeft_PreviewMouseLeftButtonDown;            
            dgLeft.PreviewMouseMove += DgLeft_PreviewMouseMove;
            dgLeft.Drop += DgLeft_Drop;

            Init();




        }


        private void Init()
        {
            dt = new DataTable();
            dt.Columns.Add("No", Type.GetType("System.Int32"));
            dt.Columns.Add("Text", Type.GetType("System.String"));

            string[] txt = new string[] { "1차 분류", "습점", "입상포장", "진공포장", "캔포장", "추출용 원료볶음", "삼정제조1", "삼정제조2", "삼정제조3", "삼정제조4" };


            for (int i = 1; i < 11; i++)
            {
                DataRow dr = dt.NewRow();

                dr["No"] = i;
                dr["Text"] = txt[i - 1];

                dt.Rows.Add(dr);
            }


            dtLeft = dt.Copy();
            dtLeft.DefaultView.Sort = "No";
            dgLeft.ItemsSource = dtLeft.DefaultView;

            DataView dv = dt.Copy().DefaultView;
            dv.Sort = "No";
            dgRight.ItemsSource = dv;


            dv = dt.Copy().DefaultView;
            dv.Sort = "No";
            dgCenter.ItemsSource = dv;


            dm.DataGridAdd(dgRight, "No");
            dm.DataGridAdd(dgCenter, "No");

        }


        Point _sPt;
        bool _isDrag = false;

        
        
        private void DgLeft_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _sPt = e.GetPosition(null);
        }

        private void DgLeft_Drop(object sender, DragEventArgs e)
        {
            DataGrid dg = dgLeft;
            DataTable dt = dtLeft;
            string sIdxFld = "No";

            //drop한 위치에 item 찾음
            UIElement ui = dg.InputHitTest(e.GetPosition(dg)) as UIElement;

            DataGridRow d = dg.ContainerFromElement(ui) as DataGridRow;
            DataRowView to = d.Item as DataRowView;
            DataRowView from = dg.SelectedItem as DataRowView;
            SortedList<int, DataRowView> vss = new SortedList<int, DataRowView>();


            bool ovr = false;
            int idx = 1;
            int oldNo; // = Fnc.obj2int(from[sIdxFld]);
            int newNo = Fnc.obj2int(to[sIdxFld]);
            int no;


            oldNo = -1;

            foreach (object o in dg.SelectedItems)
            {
                from = o as DataRowView;
                no = Fnc.obj2int(from[sIdxFld]);

                vss.Add(no, from);

                if (oldNo == -1 || oldNo > no) oldNo = no;

            }
            
            int newIdx = newNo;

            if (oldNo < newNo)
                newIdx = newNo;
            else
                idx++;
            

            foreach (DataRowView v in dt.DefaultView)
            {
                no = Fnc.obj2int(v[sIdxFld]);

                if (vss.Values.Contains(v))
                {                    
                    continue;
                }
                else if (oldNo < newNo)
                {

                    if (newNo >= no)
                    {
                        v[sIdxFld] = idx;

                        if(newNo == no) idx = idx + (vss.Count - 1);
                    }
                    else
                        v[sIdxFld] = idx + 1;
                }
                else
                {
                    if (newNo == no) idx = idx + (vss.Count - 1);

                    if (newNo <= no) v[sIdxFld] = idx;
                }

                idx++;
            }

            if (oldNo < newNo) newIdx = newIdx - (vss.Count - 1);

            foreach (DataRowView o in vss.Values)
            {
                o[sIdxFld] = newIdx;

                newIdx++;
            }


            

            return;

            /*
            bool ovr = false;
            int idx = 1;
            int oldNo = Fnc.obj2int(from[sIdxFld]);
            int newNo = Fnc.obj2int(to[sIdxFld]);
            int no;
            int  newIdx = newNo;

            if (oldNo < newNo)
                newIdx = newNo;
            else
                idx++;
            
            foreach (DataRowView v in dt.DefaultView)
            {
                no = Fnc.obj2int(v["No"]);

                if (v == from)
                {
                    continue;                    
                }
                else if(oldNo < newNo)
                {
                    if (newNo >= no)
                        v["no"] = idx;
                    else
                        v["no"] = idx + 1;
                }
                else
                {
                    if (newNo <= no) v["no"] = idx;
                }

                idx++;
            }

            from["No"] = newIdx;

            */
            //TreeViewItem tItem = treeView.TreeViewItemFormChild(ui);

            //if (tItem != null)
            //{
            //    Console.WriteLine(tItem.Header.ToString());

            //    int idx = treeView.Items.IndexOf(tItem);

            //    TreeViewItem si = treeView.SelectedItem as TreeViewItem;

            //    int idx2 = treeView.Items.IndexOf(si);

            //    if (idx > idx2) idx--;

            //    treeView.Items.Remove(si);

            //    treeView.Items.Insert(idx, si);

            //    si.IsSelected = true;

            //}
        }

        private void DgLeft_PreviewMouseMove(object sender, MouseEventArgs e)
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


        private void StartDrag(MouseEventArgs e)
        {
            try
            {
                _isDrag = true;

                object temp = dgLeft.SelectedItem;
                DataObject data = null;

                data = new DataObject("inadt", temp);

                if (data != null)
                {
                    DragDropEffects dde = DragDropEffects.Move;

                    if (e.RightButton == MouseButtonState.Pressed)
                    {
                        dde = DragDropEffects.All;
                    }

                    DragDropEffects de = DragDrop.DoDragDrop(dgLeft, data, dde);
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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Init();
        }
    }   //end class
}
