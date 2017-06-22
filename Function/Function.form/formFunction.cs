using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;


namespace Function.form
{
	/// <summary>
	/// 서브폼 로드시 시작 위치 enum
	/// </summary>
	public enum enSubFormLocation
	{
		Top,
		Buttom,
		Left,
		Rigth
	}


	public class formFunction
	{
		/// <summary>
		/// Panel의 마우스 스크롤 이벤트를 추가 한다.
		/// </summary>
		/// <param name="frmParent"></param>
		/// <param name="pnl"></param>
		public static void PanelMouseScroll_Set(Form frmParent, Panel pnl)
		{
			if (frmParent == null) return;

			if (dicScrollPanelMessageFilter.ContainsKey(frmParent))
			{
				if (dicScrollPanelMessageFilter[frmParent].ContainsKey(pnl)) return;

				dicScrollPanelMessageFilter[frmParent].Add(pnl, new ScrollPanelMessageFilter(pnl));
				
			}
			else
			{
				Dictionary<Panel, ScrollPanelMessageFilter> dic = new Dictionary<Panel, ScrollPanelMessageFilter>();

				dic.Add(pnl, new ScrollPanelMessageFilter(pnl));

				dicScrollPanelMessageFilter.Add(frmParent, dic);

				frmParent.Activated += new EventHandler(frmParent_Activated);
				frmParent.Deactivate += new EventHandler(frmParent_Deactivate);
			}

			
		}

		static Dictionary<Form, Dictionary<Panel, ScrollPanelMessageFilter>> dicScrollPanelMessageFilter = new Dictionary<Form, Dictionary<Panel, ScrollPanelMessageFilter>>();

		static void frmParent_Deactivate(object sender, EventArgs e)
		{
			Form frm = sender as Form;

			if(frm == null) return;

			foreach (ScrollPanelMessageFilter filter in dicScrollPanelMessageFilter[frm].Values)
			{
				Application.RemoveMessageFilter(filter);
			}
		}

		static void frmParent_Activated(object sender, EventArgs e)
		{
			Form frm = sender as Form;

			if (frm == null) return;

			foreach (ScrollPanelMessageFilter filter in dicScrollPanelMessageFilter[frm].Values)
			{
				Application.AddMessageFilter(filter);				
			}
		}


		/// <summary>
		/// 숫자 입력판을 보여 준다.
		/// </summary>
		/// <param name="frmParent"></param>
		/// <param name="intMaxLength">입력 받을 숫자 길이 1~10</param>
		/// <returns>입력 받은 값</returns>
		public static string NumberInput_Show(Form frmParent, int intMaxLength)
		{
			using (frmNumberInput frm = new frmNumberInput())
			{
				frm.StartPosition = FormStartPosition.CenterParent;
				frm.intMaxLength = intMaxLength;
				frm.ShowDialog(frmParent);

				if (frm.DialogResult != DialogResult.OK)
					return string.Empty;
				else
					return frm.strNumber;
			}

		}

		/// <summary>
		/// 서브폼 로드시 시작 위치 조정하여 준다
		/// </summary>
		/// <param name="frmMain"></param>
		/// <param name="frmSub"></param>
		public static void SubForm_SetLocation(Form frmMain, Form frmSub, enSubFormLocation loc)
		{
			int left = frmMain.Left;
			int top = frmMain.Top;

			switch (loc)
			{
				case enSubFormLocation.Top:
					top -= frmSub.Height;
					break;

				case enSubFormLocation.Buttom:
					top += frmMain.Height;
					break;

				case enSubFormLocation.Left:
					left -= frmSub.Width;
					break;

				case enSubFormLocation.Rigth:
					left += frmMain.Width;
					break;
			}

			frmSub.Left = left;
			frmSub.Top = top;

		}



		/// <summary>
		/// 컨트롤을 부모컨트롤에 정렬을 한다.
		/// </summary>
		/// <param name="ctrl"></param>
		/// <param name="ali"></param>
		public static void ControlAlign(Control ctrl, System.Drawing.ContentAlignment ali)
		{
			//부모가 없으면 안된다.
			if (ctrl.Parent == null) return;

			System.Drawing.StringAlignment H;			
            System.Drawing.StringAlignment V;
			int al = (int)ali;
			

			//세로 정리
			switch(ali)
			{
				case System.Drawing.ContentAlignment.TopCenter:
				case System.Drawing.ContentAlignment.TopLeft:
				case System.Drawing.ContentAlignment.TopRight:
					V = System.Drawing.StringAlignment.Near;
					break;

				case System.Drawing.ContentAlignment.MiddleRight:
				case System.Drawing.ContentAlignment.MiddleLeft:
				case System.Drawing.ContentAlignment.MiddleCenter:
					V = System.Drawing.StringAlignment.Center;
					break;

				default:
					V = System.Drawing.StringAlignment.Far;
					break;
			}


			//가로 정리
			switch (ali)
			{
				case System.Drawing.ContentAlignment.TopLeft:
				case System.Drawing.ContentAlignment.MiddleLeft:
				case System.Drawing.ContentAlignment.BottomLeft:
					H = System.Drawing.StringAlignment.Near;
					break;

				case System.Drawing.ContentAlignment.TopCenter:
				case System.Drawing.ContentAlignment.MiddleCenter:
				case System.Drawing.ContentAlignment.BottomCenter:
					H = System.Drawing.StringAlignment.Center;
					break;

				default:
					H = System.Drawing.StringAlignment.Far;
					break;
			}
								

			Control p = ctrl.Parent;

			ctrl.Left = ControlAlign_GetValue(p.Width, ctrl.Width, H);
			ctrl.Top = ControlAlign_GetValue(p.Height, ctrl.Height, V);

		}


		private static int ControlAlign_GetValue(int ParentValue, int CtrlValue, System.Drawing.StringAlignment align)
		{
			int rtn;
			switch(align)
			{
				case System.Drawing.StringAlignment.Near:
					rtn = 0;
					break;

				case System.Drawing.StringAlignment.Center:
					rtn = (ParentValue / 2) - (CtrlValue / 2);
					break;

				default:
					rtn = ParentValue - CtrlValue;
					break;

			}

			if (rtn < 0) rtn = 0;

			return rtn;

		}



		public static Control LoadingImage_Set(Control Parent_Control, int size = 60, System.Drawing.Image img = null)
		{
			PictureBox pic = null;

			if (Parent_Control.Parent == null) return null;

			foreach (Control c in Parent_Control.Parent.Controls)
			{
				if (Fnc.obj2String(c.Tag).Equals("LoadingImage"))
				{
					pic = c as PictureBox;
					break;
				}
			}

			if (pic == null)
			{
				pic = new PictureBox();
				pic.SizeMode = PictureBoxSizeMode.StretchImage;
				if (img == null)
					pic.Image = Function.resIcon16.loading_004;
				else
					pic.Image = img;
			}

			pic.Width = size;
			pic.Height = size;
			pic.Tag = "LoadingImage";
			pic.Visible = true;

			Parent_Control.Enabled = false;

			Parent_Control.Parent.Controls.Add(pic);

			pic.Left = Parent_Control.Left + (Parent_Control.Width / 2) - (pic.Width / 2);
			pic.Top = Parent_Control.Top + (Parent_Control.Height / 2) - (pic.Height / 2);

			pic.BringToFront();

			return pic;

		}

		public static void LoadingImage_Release(Control Parent_Control, Control LoadingBox)
		{
			Parent_Control.Enabled = true;

			if (LoadingBox != null)
			{
				LoadingBox.Parent.Controls.Remove(LoadingBox);
			}
			else if(Parent_Control.Parent != null)
			{
				foreach (Control c in Parent_Control.Parent.Controls)
				{
					if (c.Tag != null && c.Tag.ToString().Equals("LoadingImage"))
					{
						c.Visible = false;
					}
				}
			}
						
		}


		/// <summary>
		/// 사용자 입력 창을 띄운다
		/// </summary>
		/// <param name="mainform">표시될 부모창</param>
		/// <param name="isNumberOnly">숫자만 입력</param>
		/// <param name="isMultiLine">다중로우 여부</param>
		/// <param name="default_value">기본값</param>
		/// <returns>취소 돼면 null로 리턴</returns>
		public static object InputBoxShow(Form mainform, bool isNumberOnly = false, bool isMultiLine = false, object default_value = null)
		{
			frmInputBox frm = new frmInputBox(isNumberOnly, isMultiLine, default_value);

			frm.ShowDialog(mainform);

			if (frm.DialogResult == DialogResult.OK)
			{
				return frm.Value;
			}
			else
			{
				return null;
			}

		}



	}
}
