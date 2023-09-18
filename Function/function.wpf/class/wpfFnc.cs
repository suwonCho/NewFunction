using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace function.wpf
{
    public class wpfFnc
    {
        private static Mutex gM;

        /// <summary>
        /// 중복 실행 체크 한다. 중복:false
        /// </summary>
        /// <param name="strProgramName"></param>		
        /// <returns>false이면 이미 실행, true이면 미실행</returns>
        public static bool ProgramRunCheck(string strProgramName)
        {
            bool isCreateNew;

            gM = new Mutex(true, strProgramName, out isCreateNew);

            return isCreateNew;
        }

        /// <summary>
        /// 폴더 선택 다이럴로그를 연다.
        /// </summary>
        /// <param name="Multiselect"></param>
        /// <param name="DefaultFolder"></param>
        /// <returns></returns>
        public static string[] FolderSelect(bool Multiselect = false, string DefaultFolder = null)
		{
			CommonOpenFileDialog f = new CommonOpenFileDialog();
			f.IsFolderPicker = true;
			f.Multiselect = Multiselect;
			if (DefaultFolder != null)
			{
				f.DefaultDirectory = DefaultFolder;
				f.InitialDirectory = DefaultFolder;
			}

			if (f.ShowDialog() != CommonFileDialogResult.Ok) return null;

			string[] rtn = new string[f.FileNames.Count<string>()];

			for (int i = 0; i < rtn.Length; i++)
			{
				rtn[i] = f.FileNames.ToArray<string>()[i];
			}

			return rtn;
		}


        /// <summary>
        /// string 값을 Solid brush 로 변환 16진수 - #AARRGGBB or #RRGGBB
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static SolidColorBrush String2Brush(string str)
        {
            try
            {
                if(!str.StartsWith("#")) return new SolidColorBrush(Colors.White);

                Byte[] col = new byte[4];

                int idx = 1;
                int length = str.Length;
                
                for(int i =0; i < 4;i++)
                {
                    if(i ==0 && length < 9)
                    {
                        col[i] = 255;
                    }
                    else if(idx + 1 > length)
                    {
                        col[i] = 0;
                    }
                    else
                    {
                        col[i] = byte.Parse(str.Substring(idx, 2), System.Globalization.NumberStyles.HexNumber);
                        idx = idx + 2;
                    }
                }

                System.Windows.Media.Color cc = System.Windows.Media.Color.FromArgb(col[0], col[1], col[2], col[3]);

                return new SolidColorBrush(cc);
            }
            catch
            {
                return new SolidColorBrush(Colors.White);
            }
        }

        /// <summary>
        /// Solid brush를 string으로 변환
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string Brush2String(SolidColorBrush b)
        {
            System.Windows.Media.Color col = b.Color;
            return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", col.A, col.R, col.G, col.B);
        }



        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public static ImageSource ImageSourceFormBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                ImageSource newSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                DeleteObject(handle);
                return newSource;
            }
            catch (Exception ex)
            {
                DeleteObject(handle);
                return null;
            }
        }


        /// <summary>
        /// 그리드 초기와, AutoColumnSize 설정, FullRowSelect, 컬럼 숨김 처리
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="HideColumns">숨길 컬럼</param>
        /// <param name="ColumnsName">숨겨진 컬럼이외에 컬럼 헤더 표시(순서대로)</param>
        public static void DGV_Init(DataGrid dgv, string[] HideColumns, string[] ColumnsName = null)
        {
            dgv.SelectionMode = DataGridSelectionMode.Extended;
            dgv.SelectionUnit = DataGridSelectionUnit.FullRow;

            if (HideColumns == null) return;

            //컬럼 수정 못하게
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                dgv.Columns[i].IsReadOnly = true;
            }


            foreach (string c in HideColumns)
            {
                for(int i = 0; i < dgv.Columns.Count;i++)
                {
                    if(Fnc.obj2String(dgv.Columns[i].Header).ToUpper() == c.ToUpper())
                    {
                        dgv.Columns[i].Visibility = Visibility.Hidden;
                        break;
                    }
                }                
            }

            if (ColumnsName == null) return;

            int n = 0;

            foreach(var c in dgv.Columns)
            {
                if (c.Visibility == Visibility.Hidden) continue;

                if (n >= ColumnsName.Length) break;

                

                Style st = new Style();
                Setter s = new Setter(DataGridColumnHeader.HorizontalContentAlignmentProperty, HorizontalAlignment.Center);
                st.Setters.Add(s);
                c.HeaderStyle = st;
                c.Header = ColumnsName[n];
                

                n++;
            }


        }



        /// <summary>
        /// 그리드 초기와, AutoColumnSize 설정, FullRowSelect, 컬럼 표시 처리
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="ShowColumns">표시 할 컬럼</param>
        /// <param name="ColumnsName">표시 컬럼이외에 컬럼 헤더 표시(순서대로)</param>
        public static void DGV_Init2(DataGrid dgv, string[] ShowColumns, string[] ColumnsName = null)
        {
            dgv.SelectionMode = DataGridSelectionMode.Extended;
            dgv.SelectionUnit = DataGridSelectionUnit.FullRow;

            if (ShowColumns == null) return;

            //컬럼 수정 못하게
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                dgv.Columns[i].IsReadOnly = true;
            }

            //입력된 컬럼만 보이게
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                dgv.Columns[i].Visibility = Visibility.Hidden;

                foreach (string c in ShowColumns)
                {
                    if (Fnc.obj2String(dgv.Columns[i].Header).ToUpper() == c.ToUpper())
                    {
                        dgv.Columns[i].Visibility = Visibility.Visible;
                        break;
                    }
                }
            }
                       
            if (ColumnsName == null) return;

            int n = 0;

            foreach (var c in dgv.Columns)
            {
                if (c.Visibility == Visibility.Hidden) continue;

                if (n >= ColumnsName.Length) break;

                Style st = new Style();
                Setter s = new Setter(DataGridColumnHeader.HorizontalContentAlignmentProperty, HorizontalAlignment.Center);
                st.Setters.Add(s);
                c.HeaderStyle = st;
                c.Header = ColumnsName[n];
                n++;
            }


        }





        /// <summary>
        /// 컨트롤의 부모 윈도우를 가지고 온다.
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public static Window ParentWindowGet(FrameworkElement ctrl)
        {
            return Window.GetWindow(ctrl);
        }



        public static WpfFont Font2WpfFont(Font fnt)
        {
            function.wpf.WpfFont rtn = new function.wpf.WpfFont();

            rtn.FontFamily = new System.Windows.Media.FontFamily(fnt.Name);
            rtn.FontSize = fnt.Size;

            if (fnt.Italic)
                rtn.FontStyle = FontStyles.Italic;
            else
                rtn.FontStyle = FontStyles.Normal;

            if (fnt.Bold)
                rtn.FontWeight = FontWeights.Bold;
            else
                rtn.FontWeight = FontWeights.Normal;


            rtn.FontSize = fnt.Size;

            return rtn;
        }

        public static Font WpfFont2Font(WpfFont fnt)
        {

            string fntName = fnt.FontFamily.Source;

            System.Drawing.FontStyle st = System.Drawing.FontStyle.Regular;

            if (fnt.FontStyle == FontStyles.Italic && fnt.FontWeight == FontWeights.Bold)
                st = System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Bold;
            else if (fnt.FontStyle == FontStyles.Italic)
                st = System.Drawing.FontStyle.Italic;
            else if (fnt.FontWeight == FontWeights.Bold)
                st = System.Drawing.FontStyle.Bold;

            Font rtn = new Font(fntName, Fnc.obj2Float(fnt.FontSize), st);

            return rtn;
        }

        /// <summary>
        /// 상태 값에 따른 배경색을 반환 한다.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static System.Windows.Media.Color StatusBackColorGet(enStatus status)
        {
            System.Windows.Media.Color rtn = System.Windows.Media.Colors.White;

            switch (status)
            {
                case enStatus.None:
                    rtn = System.Windows.SystemColors.ControlColor;
                    break;

                case enStatus.Pause:
                    rtn = System.Windows.SystemColors.ControlDarkColor;
                    break;

                case enStatus.Done:
                    rtn = System.Windows.Media.Colors.YellowGreen;
                    break;

                case enStatus.OK:
                    rtn = System.Windows.Media.Colors.LightGreen;
                    break;

                case enStatus.Critical:
                    rtn = System.Windows.Media.Colors.Crimson;
                    break;

                case enStatus.Error:
                    rtn = System.Windows.Media.Colors.Crimson;
                    break;

                case enStatus.Warnning:
                    rtn = System.Windows.Media.Colors.Orange;
                    break;

            }

            return rtn;
        }

        /// <summary>
        /// 상태 값에 따른 글자색을 반환 한다.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static System.Windows.Media.Color StatusForeColorGet(enStatus status)
        {
            System.Windows.Media.Color rtn = System.Windows.Media.Colors.Black;

            switch (status)
            {
                case enStatus.None:
                    //rtn = System.Windows.SystemColors.ControlColor;
                    break;

                case enStatus.Pause:
                    rtn = System.Windows.Media.Colors.White;
                    break;

                case enStatus.Done:
                case enStatus.OK:
                    //rtn = System.Windows.Media.Colors.LightGreen;
                    break;

                case enStatus.Critical:
                case enStatus.Error:
                    rtn = System.Windows.Media.Colors.Yellow;
                    break;
            }

            return rtn;
        }

    }   //end class
}
