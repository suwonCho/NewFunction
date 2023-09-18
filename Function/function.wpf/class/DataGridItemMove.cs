using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace function.wpf
{
    class DataGridMovePram
    {
        public string sIdxFld { get; set; }
        public Point sPt { get; set; }

        public bool isDrag { get; set; } = false;

    }

    public class DataGridItemMove
    {
        Dictionary<DataGrid, DataGridMovePram> dicDataGrid = new Dictionary<DataGrid, DataGridMovePram>();


        public DataGridItemMove()
        {

        }

        public DataGridItemMove(DataGrid grd, string sIdxFld)
        {
            DataGridAdd(grd, sIdxFld);
        }


        public void DataGridAdd(DataGrid grd, string sIdxFld)
        {
            if (dicDataGrid.Keys.Contains(grd)) return;

            grd.AllowDrop = true;
            grd.PreviewMouseLeftButtonDown += Grd_PreviewMouseLeftButtonDown;
            grd.PreviewKeyDown += Grd_PreviewKeyDown;
            grd.PreviewMouseMove += Grd_PreviewMouseMove;
            grd.Drop += Grd_Drop;

            DataGridMovePram p = new DataGridMovePram();
            p.sIdxFld = sIdxFld;

            dicDataGrid.Add(grd, p);
        }

        private void Grd_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            return;

            DataGrid dg = DataGridGet(sender);

            if (dg == null) return;

            DataGridMovePram prm = dicDataGrid[dg];
            DataView dt = dg.ItemsSource as DataView;
            string sIdxFld = prm.sIdxFld;

            int min = -1, max = -1, SelMin = -1, SelMax = -1, no;
            SortedList<int, DataRowView> vss = new SortedList<int, DataRowView>();
            DataRowView from;

            //선택된 항목 처리
            foreach (object o in dg.SelectedItems)
            {
                from = o as DataRowView;
                no = Fnc.obj2int(from[sIdxFld]);

                vss.Add(no, from);

                if (SelMin == -1 || SelMin > no) SelMin = no;
                if (SelMax == -1 || SelMax < no) SelMax = no;

            }

            if (vss.Count < 1) return;

            foreach (DataRowView o in dt)
            {                
                no = Fnc.obj2int(o[sIdxFld]);                

                if (min == -1 || min > no) min = no;
                if (max == -1 || max < no) max = no;

            }

            var ss = dg.SelectedCells;

            bool isUp = true;

            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key != Key.LeftCtrl)
            {
                int newIdx = -1;

                switch (e.Key)
                {
                    case Key.Up:
                        if (SelMin > min) newIdx = SelMin - 1;
                        break;

                    case Key.Down:
                        if (SelMax < max)
                        {
                            newIdx = SelMin + 1;
                            isUp = false;
                        }
                        break;

                    case Key.PageUp:
                        if (SelMin > min)  newIdx = min;
                        break;

                    case Key.PageDown:
                        if (SelMax < max)
                        {
                            newIdx = max;
                            isUp = false;
                        }
                        break;
                }

                if (newIdx == -1) return;

                foreach(DataRowView v in dt)
                {
                    no = Fnc.obj2int(v[sIdxFld]);

                    if(vss.Values.Contains(v))
                    {
                        continue;
                    }
                    else if(isUp)
                    {
                        if (no >= newIdx && no < SelMax)
                        {
                            //if(no == newIdx) 
                            no = no + (vss.Count);
                            v[sIdxFld] = no;
                        }
                    }
                    else
                    {
                        if(no <= newIdx && no > SelMin)
                        {
                            //if (no == newIdx) 
                            no = no - (vss.Count);
                            v[sIdxFld] = no;
                        }
                    }
                }


                //if(!isUp) newIdx = newIdx - (vss.Count - 1);

                foreach (DataRowView o in vss.Values)
                {
                    o[sIdxFld] = newIdx;

                    newIdx++;
                }
                               

                dg.Focus();

                dg.CurrentCell = ss[0];
                
            }
        }


        private DataGrid DataGridGet(object sender)
        {
            DataGrid rtn = sender as DataGrid;

            if (rtn == null) return null;

            if (dicDataGrid.Keys.Contains(rtn))
                return rtn;
            else
                return null;

        }

        private void Grd_Drop(object sender, System.Windows.DragEventArgs e)
        {
            DataGrid dg = DataGridGet(sender);

            if (dg == null) return;

            DataGridMovePram prm = dicDataGrid[dg];
            DataView dt = dg.ItemsSource as DataView;

            string sIdxFld = prm.sIdxFld;

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


            foreach (DataRowView v in dt)
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

                        if (newNo == no) idx = idx + (vss.Count - 1);
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
        }

        private void Grd_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DataGrid dg = DataGridGet(sender);

            if (dg == null) return;

            DataGridMovePram prm = dicDataGrid[dg];


            if (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed && !prm.isDrag)
            {
                Point pt = e.GetPosition(null);
                if (Math.Abs(pt.X - prm.sPt.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(pt.Y - prm.sPt.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    StartDrag(dg, prm, e);
                }
            }
        }

        private void Grd_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DataGrid dg = DataGridGet(sender);

            if (dg == null) return;

            DataGridMovePram prm = dicDataGrid[dg];

            prm.sPt = e.GetPosition(null);
        }

        private void StartDrag(DataGrid dg, DataGridMovePram prm, MouseEventArgs e)
        {
            try
            {
                prm.isDrag = true;

                object temp = dg.SelectedItem;
                DataObject data = null;

                data = new DataObject("inadt", temp);

                if (data != null)
                {
                    DragDropEffects dde = DragDropEffects.Move;

                    if (e.RightButton == MouseButtonState.Pressed)
                    {
                        dde = DragDropEffects.All;
                    }

                    DragDropEffects de = DragDrop.DoDragDrop(dg, data, dde);
                }
            }
            catch
            {

            }
            finally
            {
                prm.isDrag = false;
            }
        }

    }
}
