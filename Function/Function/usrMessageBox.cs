using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;
namespace Function
{
	public partial class usrMessageBox : UserControl
	{
		public usrMessageBox()
		{
			InitializeComponent();

			this.Visible = false;
		}


		System.Threading.Timer tmrSec;
		int intSecCnt = 0;
		public DialogResult DialogResult = DialogResult.None;
		

		public enum enMessageType { YesNo, OK, None };


		private void usrMessageBox_Load(object sender, EventArgs e)
		{

		}

		/// <summary>
		/// 메시지를 보여준다.
		/// </summary>
		/// <param name="strMsg1"></param>
		/// <param name="strMsg2"></param>
		/// <param name="enType"></param>
		/// <returns></returns>
		public DialogResult ShowMessage(string strMsg1, string strMsg2, enMessageType enType)
        {
            
            lblMsg1.Text = strMsg1;
            lblMsg2.Text = strMsg2;

			try
			{
				this.Visible = true;
				this.BringToFront();
				this.Select();

				DialogResult = DialogResult.None;
				
				switch (enType)
				{
					case enMessageType.YesNo:
						btnYes.Visible = true;
						btnNO.Visible = true;
						break;

					case enMessageType.None:		//none으로 설정시 10초후에 사라 진다.
						btnOK.Visible = true;
						tmrSec = new System.Threading.Timer(new System.Threading.TimerCallback(SecCount), null, 1000, 1000);
						break;

					case enMessageType.OK:
						btnOK.Visible = true;
						break;

					default:
						btnOK.Visible = true;
						break;
				}

				Application.DoEvents();

				while (true)
				{
					if (DialogResult != DialogResult.None) break;

					Thread.Sleep(200);
				}

				return DialogResult;
			}
			catch
			{
				return DialogResult;
			}
			finally
			{
				this.Visible = false;

				if (tmrSec != null)
				{
					tmrSec.Dispose();
					Application.DoEvents();
					tmrSec = null;
				}
			}

        }

		private void btn_Click(object sender, EventArgs e)
		{
			try
			{
				Button btn = (Button)sender;

				switch (btn.Name)
				{
					case "btnYes":
						this.DialogResult = DialogResult.Yes;
						break;

					case "btnNO":
						this.DialogResult = DialogResult.No;
						break;

					case "btnOK":
						this.DialogResult = DialogResult.OK;
						break;

					default:
						this.DialogResult = DialogResult.No;
						break;
				}
			}
			catch
			{
				this.DialogResult = DialogResult.No;
			}
		

		}


		private void SecCount(object obj)
		{
			intSecCnt++;

			if (intSecCnt > 9)
			{
				this.DialogResult = DialogResult.OK;
			}

		}


	}
}
