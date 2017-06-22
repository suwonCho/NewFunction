using System;
using System.Collections.Generic;
using System.Text;
using C1.Win.C1Input;
using C1.Win.C1InputPanel;
using C1.Win.C1List;
using C1.Win.C1Chart;
using System.Windows.Forms;
using System.Drawing;
using Function.form;
using System.Data;

namespace Function.Component
{
	public class c1
	{

		public delegate void delControlInReset(Control ctl);
		/// <summary>
		/// 그룹 않에 있는 컨트롤을 초기화 한다.
		/// </summary>
		/// <param name="ctl"></param>
		public static void ControlInReset(Control ctl)
		{
			if (ctl.InvokeRequired)
			{
				ctl.Invoke(new delControlInReset(ControlInReset), new object[] { ctl });
				return;
			}


			TextBox txt;
			ComboBox cmb;

			C1TextBox c1txt;
			C1Combo c1cmb;
			
			foreach (Control c in ctl.Controls)
			{
				txt = c as TextBox;

				if (txt != null)
				{
					txt.Enabled = true;
					Function.form.control.Invoke_Control_TextAdd(txt, string.Empty, true, true);
					continue;
				}

				cmb = c as ComboBox;
				if (cmb != null)
				{
					cmb.Enabled = true;
					control.Invoke_ComboBox_SelectedIndex(cmb, -1);
					continue;
				}

				c1txt = c as C1TextBox;

				if (c1txt != null)
				{
					c1txt.Enabled = true;
					c1txt.Text = string.Empty;
					continue;
				}

				c1cmb = c as C1Combo;
				if (c1cmb != null)
				{
					c1cmb.Enabled = true;
					c1cmb.SelectedIndex = -1;
					continue;
				}
				
				ControlInReset(c);
			}

		}

		/// <summary>
		/// 텍스트 박스에 ime모드를 영어로 고정시킨다.
		/// </summary>
		/// <param name="txt"></param>
		public static void TextBox_Set_ImeModeOnlyAlpah(C1TextBox txt)
		{
			txt.ImeMode = System.Windows.Forms.ImeMode.Alpha;
			txt.ImeModeChanged += new EventHandler(TextBox_ImeModeChanged);
		}

		static void TextBox_ImeModeChanged(object sender, EventArgs e)
		{
			C1TextBox txt = (C1TextBox)sender;
			txt.ImeMode = System.Windows.Forms.ImeMode.Alpha;
		}



		delegate void delInvoke_ComboBox_DataSource(C1Combo cmb, DataTable dt, string strDisplayMember);
		public static void Invoke_ComboBox_DataSource(C1Combo cmb, DataTable dt, string strDisplayMember)
		{
			if (cmb.InvokeRequired)
			{
				cmb.Invoke(new delInvoke_ComboBox_DataSource(Invoke_ComboBox_DataSource), new object[] { cmb, dt, strDisplayMember });
				return;
			}

			cmb.DisplayMember = strDisplayMember;
			cmb.DataSource = dt;
			
			//cmb.SelectedIndex = -1;
		}


		delegate void delInvoke_InputPanel_Reset(C1InputPanel inp);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="inp"></param>		
		/// <returns></returns>
		public static void Invoke_InputPanel_Reset(C1InputPanel inp)
		{
			if (inp.InvokeRequired)
			{
				inp.Invoke(new delInvoke_InputPanel_Reset(Invoke_InputPanel_Reset), new object[] { inp });
				return;
			}

			foreach(InputComponent ctl in inp.Items)
			{
				InputDatePicker dt = ctl as InputDatePicker;
				if (dt != null)
				{
					dt.ValueIsNull = true;
					continue;
				}
				
				InputTimePicker tm = ctl as InputTimePicker;
				if (tm != null)
				{
					tm.ValueIsNull = true;
					continue;
				}

				InputTextBox txt = ctl as InputTextBox;
				if (txt != null)
				{
					txt.Text = string.Empty;
					continue;
				}

				InputComboBox cmb = ctl as InputComboBox;
				if (cmb != null)
				{
					cmb.SelectedIndex = -1;
					continue;
				}





			}


		}


		delegate void delInvoke_InputComboBox_Items_AddItems(C1InputPanel inp,
								C1.Win.C1InputPanel.InputComboBox cmb, string[] strItems);
		/// <summary>
		/// InputComboBox에 string 배열의 값을 item으로 추가한다.
		/// </summary>
		/// <param name="inp"></param>
		/// <param name="cmb"></param>
		/// <param name="strItems"></param>
		/// <returns></returns>
		public static void Invoke_InputComboBox_Items_AddItems(C1InputPanel inp, 
		                        C1.Win.C1InputPanel.InputComboBox cmb, string[] strItems)
		{
		    if (inp.InvokeRequired)
		    {
		        inp.Invoke(new delInvoke_InputComboBox_Items_AddItems(Invoke_InputComboBox_Items_AddItems), new object[] { inp, cmb, strItems });
		        return;
		    }

			InputOption io;

			foreach (string str in strItems)
			{
				io = new InputOption(str);
				cmb.Items.Add(io);
			}

		}

		
		/// <summary>
		/// InputComboBox에 string 배열의 값을 Group으로 추가한다.
		/// </summary>
		/// <param name="inp"></param>
		/// <param name="cmb"></param>
		/// <param name="strItems"></param>
		/// <returns></returns>
		public static void Invoke_InputComboBox_Items_AddGroups(C1InputPanel inp,
								C1.Win.C1InputPanel.InputComboBox cmb, string[] strGroups)
		{
			if (inp.InvokeRequired)
			{
				inp.Invoke(new delInvoke_InputComboBox_Items_AddItems(Invoke_InputComboBox_Items_AddGroups), new object[] { inp, cmb, strGroups });
				return;
			}

			InputGroupHeader ig;

			foreach (string str in strGroups)
			{
				ig = new InputGroupHeader();
				ig.Text = str;
				cmb.Items.Add(ig);
			}

		}

		delegate void delInvoke_InputComboBox_SetSelectedValue(C1InputPanel inp,
								C1.Win.C1InputPanel.InputComboBox cmb, string strValue);
		/// <summary>
		/// InputComboBox에 값을 선택한다.
		/// </summary>
		/// <param name="cmb"></param>
		/// <param name="strField"></param>
		public static void Invoke_InputComboBox_SetSelectedValue(C1InputPanel inp,
								C1.Win.C1InputPanel.InputComboBox cmb, string strValue)
		{

			if (inp.InvokeRequired)
			{
				inp.Invoke(new delInvoke_InputComboBox_SetSelectedValue(Invoke_InputComboBox_SetSelectedValue), new object[] { inp, cmb, strValue });
				return;
			}

			if (cmb.DataSource != null)
			{
				foreach (DataRowView dr in ((DataTable)cmb.DataSource).DefaultView)
				{
					if (dr[cmb.DisplayMember].ToString() == strValue)
					{
						cmb.SelectedValue = dr;
						return;
					}
				}

				cmb.SelectedIndex = -1;
			}
			else
			{
				InputOption io;
				foreach (InputComponent i in cmb.Items)
				{
					io = i as InputOption;
					if (io != null)
					{
						if (io.Text == strValue)
						{
							cmb.SelectedOption = io;
							return;
						}
					}

				}
				cmb.SelectedIndex = -1;
			}

		}

		delegate void delInvoke_InputComboBox_SetSelectedValue2(C1InputPanel inp,
								C1.Win.C1InputPanel.InputComboBox cmb, string strField, string strValue);
		/// <summary>
		/// InputComboBox에 값을 선택한다.
		/// </summary>
		/// <param name="cmb"></param>
		/// <param name="strField"></param>
		public static void Invoke_InputComboBox_SetSelectedValue(C1InputPanel inp,
								C1.Win.C1InputPanel.InputComboBox cmb, string strField, string strValue)
		{

			if (inp.InvokeRequired)
			{
				inp.Invoke(new delInvoke_InputComboBox_SetSelectedValue2(Invoke_InputComboBox_SetSelectedValue), new object[] { inp, cmb, strField, strValue });
				return;
			}

			if (cmb.DataSource != null)
			{
				DataView dv;
								
				DataTable dt = cmb.DataSource as DataTable;
							

				if (dt != null)
					dv = dt.DefaultView;
				else
					dv = cmb.DataSource as DataView;


				foreach (DataRowView dr in dv)
				{
					if (dr[strField].ToString().Trim() == strValue)
					{
						Application.DoEvents();
                        cmb.SelectedValue = dr;
						return;
					}
				}
			
				cmb.SelectedIndex = -1;

			}

		}



		/// <summary>
		/// InputComboBox에 선택된 값을 가져온다.(Invoke 아님)
		/// </summary>
		/// <param name="cmb"></param>
		/// <param name="strField"></param>
		public static string Invoke_InputComboBox_GetSelectValue(C1.Win.C1InputPanel.InputComboBox cmb, string strField)
		{
			if (cmb.SelectedIndex < 0) return string.Empty;

			if (cmb.DataSource != null)
			{
				//DataRow dr = ((DataTable)cmb.DataSource).Select(string.Format("{0} = '{1}'", cmb.DisplayMember, cmb.Text))[0];
                return Fnc.obj2String(((DataRowView)cmb.SelectedValue)[strField]);				//Function.Fnc.obj2String(dr[strField]);
			}
			else
			{
				return cmb.Text;
			}
			       
		}




		public delegate void delControlInSetDatasoruce(Control ctl, object objDataSource);
		/// <summary>
		/// 그룹 않에 있는 컨트롤을 초기화 한다.
		/// </summary>
		/// <param name="ctl"></param>
		public static void ControlInSetDatasoruce(Control ctl, object objDataSource)
		{
			if (ctl.InvokeRequired)
			{
				ctl.Invoke(new delControlInSetDatasoruce(ControlInSetDatasoruce), new object[] { ctl, objDataSource });
				return;
			}

			C1TextBox c1txt;
			C1Label c1lbl;		


			foreach (Control c in ctl.Controls)
			{
				c1txt = c as C1TextBox;
				if (c1txt != null)
				{
					c1txt.DataSource = objDataSource;					
					continue;
				}

				c1lbl = c as C1Label;
				if (c1lbl != null)
				{
					if (c1lbl.Name.Substring(0, 3) == "lbl")
					{
						c1lbl.DataSource = objDataSource;
					}					
					continue;
				}

				ControlInSetDatasoruce(c, objDataSource);
			}

		}


		delegate void delInvoke_InputPanel_Ctl_Enabled(C1InputPanel inp, bool enabled);

		public static void Invoke_InputPanel_Ctl_Enabled(C1InputPanel inp, bool enabled)
		{
			if (inp.InvokeRequired)
			{
				inp.Invoke(new delInvoke_InputPanel_Ctl_Enabled(Invoke_InputPanel_Ctl_Enabled), new object[] { inp, enabled });
				return;
			}



			foreach (InputComponent c in inp.Items)
			{
				c.Enabled = enabled;
			}

		}


		delegate void delInvoke_InputPanel_Group_Collapsed(C1InputPanel inp, string[] strGrpName);

		/// <summary>
		/// inputpannel 그룹에 선택된 그룹만 보이게 한다.
		/// </summary>
		/// <param name="inp"></param>
		/// <param name="strGrpNames"></param>
		public static void Invoke_InputPanel_Group_Collapsed(C1InputPanel inp, string [] strGrpNames)
		{
			if (inp.InvokeRequired)
			{
				inp.Invoke(new delInvoke_InputPanel_Group_Collapsed(Invoke_InputPanel_Group_Collapsed), new object[] { inp, strGrpNames });
				return;
			}



			InputGroupHeader grp = null;

			//일단 전부 않보이게..
			foreach (InputComponent c in inp.Items)
			{
				grp = c as InputGroupHeader;

				if (grp != null)
				{
					grp.Height = 0;
					grp.Collapsed = true;
					grp.Visibility = C1.Win.C1InputPanel.Visibility.Hidden;					
				}
			}


			//대상만 보이게..
			foreach (InputComponent c in inp.Items)
			{
				grp = c as InputGroupHeader;

				if (grp != null)
				{
					foreach (string strGrpName in strGrpNames)
					{
						if (grp.Name == strGrpName)
						{
							//grp.Height = -1;
							grp.Collapsed = false;
							grp.Visibility = C1.Win.C1InputPanel.Visibility.Visible;
							break;
						}
						
					}
				}

			}






		}


		/// <summary>
		/// 챠트를 이미지로 저장한다.
		/// </summary>
		/// <param name="chart"></param>
		public static void Chart_SaveAsImage(C1Chart chart)
		{
			SaveFileDialog sfd = new SaveFileDialog();

			sfd.Filter = "Bitmap (*.bmp)|*.bmp|"
				+ "EMF Enhanced Metafile Format (*.emf)|*.emf|"
				+ "Graphics Interchange Format (*.gif)|*.gif|"
				+ "Joint Photographic Experts Group (*.jpg)|*.jpg|"
				+ "W3C Portable Network Graphics (*.png)|*.png";
			sfd.DefaultExt = "bmp";
			sfd.FileName = "image1";
			sfd.OverwritePrompt = true;
			sfd.CheckPathExists = true;
			sfd.RestoreDirectory = false;
			sfd.ValidateNames = true;

			if (sfd.ShowDialog() == DialogResult.OK)
			{
				chart.SaveImage(sfd.FileName, getImageFormatFromDlg(sfd.FilterIndex), getSize());				
			}
		}


		private static System.Drawing.Imaging.ImageFormat getImageFormatFromDlg(int index)
		{
			switch (index)
			{
				case 1: return System.Drawing.Imaging.ImageFormat.Bmp;
				case 2: return System.Drawing.Imaging.ImageFormat.Emf;
				case 3: return System.Drawing.Imaging.ImageFormat.Gif;
				case 4: return System.Drawing.Imaging.ImageFormat.Jpeg;
				case 5: return System.Drawing.Imaging.ImageFormat.Png;
				default: return System.Drawing.Imaging.ImageFormat.Bmp;
			}
		}

		private static System.Drawing.Size getSize()
		{
			System.Drawing.Size sz = Size.Empty;
			try
			{
				sz.Width = 1024; // int.Parse(1024);
				sz.Height = 768; //int.Parse(768);
			}
			catch
			{
				sz = Size.Empty;
			}
			return sz;
		}

	}
}
