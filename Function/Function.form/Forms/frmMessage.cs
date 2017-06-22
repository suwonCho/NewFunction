using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Function.form
{
	public partial class frmMessage : Form
	{
		public enum enMessageType { YesNo, OK, None, YesNoCancel };

		enMessageType _enType;

		System.Threading.Timer tmrSec;

		int _closetime = 0;

		int _intSecCnt = 0;

		DialogResult _defaltResult;

		/// <summary>
		/// 모든 항목 적용여부
		/// </summary>
		public bool ApplyAll = false;


		public frmMessage()
		{
			InitializeComponent();
		}

		public frmMessage(string title, string msg, enMessageType enType, int closetime = 0, DialogResult defaltResult = System.Windows.Forms.DialogResult.No, bool ShowApplyAll = false)
		{
			InitializeComponent();

			_defaltResult = defaltResult;

			int line_qty = msg.Split(new string[] { "\r\n" }, StringSplitOptions.None).Length;
			int line_Height = Fnc.floatToInt(Function.form.control.Font_Control_String_Size_Get(lblMsg, control.enControl_Criteria.height, "A"));

			if(line_qty > 2)
			{	//2줄 이상
				this.Height += ((line_qty - 1) * line_Height);
			}
			else
			{	//한줄일 경우
				float txtWidth = Function.form.control.Font_Control_String_Size_Get(lblMsg, control.enControl_Criteria.width, msg);

				float line_f = txtWidth / (lblMsg.Width);

				float half = (txtWidth % (lblMsg.Width)) / lblMsg.Width * 0.9f * 100;

				line_qty = Fnc.floatToInt(line_f) +  (half >= 50f ? 0 : 1);



				this.Height += ((line_qty - 1) * line_Height);
			}

			

			_closetime = closetime;

			int he_01 = lblTitle.Height;
			lblTitle.Text = title;
			lblMsg.Text = msg;

			_enType = enType;

			float f = Function.form.control.Font_Control_String_Size_Get(lblTitle, control.enControl_Criteria.height, msg);
			f =  f - he_01;

			if (f > 3)
			{
				this.Height += int.Parse(f.ToString("F0"));
			}


			switch (enType)
			{
				case enMessageType.YesNo:
					btnOk.Visible = false;
					btnYes.Image = Function.resIcon16.ok;
					btnNo.Image = Function.resIcon16.delete;
					break;

				case enMessageType.YesNoCancel:
					btnOk.Visible = false;
					btnYes.Image = Function.resIcon16.ok;
					btnNo.Image = Function.resIcon16.delete;
					btnCancel.Visible = true;
					btnCancel.Image = Function.resIcon16.redo;
					break;

				case enMessageType.None:		//none으로 설정시 10초후에 사라 진다.
					btnYes.Visible = false;
					btnNo.Visible = false;
					btnOk.Image = Function.resIcon16.ok;
					_closetime = _closetime == 0 ? 10 : _closetime;
					break;

				case enMessageType.OK:
					btnYes.Visible = false;
					btnNo.Visible = false;
					btnOk.Image = Function.resIcon16.ok;
					break;

				default:
					btnYes.Visible = false;
					btnNo.Visible = false;
					btnOk.Image = Function.resIcon16.ok;
					break;
			}

			chkApplyAll.Visible = ShowApplyAll;

			if(_closetime > 0)
				tmrSec = new System.Threading.Timer(new System.Threading.TimerCallback(SecCount), null, 1000, 1000);


		}

		/// <summary>
		/// 자동 닫힘 쓰레드...
		/// </summary>
		/// <param name="obj"></param>
		private void SecCount(object obj)
		{
			if (_closetime < 1) return;

			_intSecCnt++;

			if (_intSecCnt >= _closetime)
			{
				switch (_enType)
				{
					case enMessageType.YesNo:
						this.DialogResult = _defaltResult;
						break;

					default:
						this.DialogResult = DialogResult.OK;
						break;
				}

				//CloseForm(this);
			}

		}



		private void picClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			ApplyAll = chkApplyAll.Checked;
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

		private void btnNo_Click(object sender, EventArgs e)
		{
			ApplyAll = chkApplyAll.Checked;
			this.DialogResult = System.Windows.Forms.DialogResult.No;
			this.Close();
		}

		private void btnYes_Click(object sender, EventArgs e)
		{
			ApplyAll = chkApplyAll.Checked;
			this.DialogResult = System.Windows.Forms.DialogResult.Yes;
			this.Close();
		}

		private void frmMessage_Load(object sender, EventArgs e)
		{
			this.Select(true, false);
			this.BringToFront();
		}

		private void frmMessage_FormClosed(object sender, FormClosedEventArgs e)
		{
			if(tmrSec != null) tmrSec.Dispose();
		}

		private void lblMsg_DoubleClick(object sender, EventArgs e)
		{
			Clipboard.SetText(lblMsg.Text);

			if (!btnYes.Visible) this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Close();
		}

		
	}
}
