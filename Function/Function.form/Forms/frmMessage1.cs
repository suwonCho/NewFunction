using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Function
{
	/// <summary>
	/// 메시지 창 생성 클래스
	/// </summary>
    public partial class frmMessage_del : Form
    {
		int intSecCnt = 0;
		System.Threading.Timer tmrSec;
		//일정 시간후 닫힘..
		int intCloseSec = 0;

		enMessageType enType;

        public enum enMessageType { YesNo, OK, None };
        public frmMessage_del(string strMsg1, string strMsg2, enMessageType enType)
        {
            InitializeComponent();

			FormInitialize(strMsg1, strMsg2, enType, 0);
        }

		public frmMessage_del(string strMsg1, string strMsg2, enMessageType enType, int intclosesec)
        {
            InitializeComponent();

			FormInitialize(strMsg1, strMsg2, enType, intclosesec);
        }


		private void FormInitialize(string strMsg1, string strMsg2, enMessageType _enType, int intclosesec)
		{
			lblMsg1.Text = strMsg1;
			lblMsg2.Text = strMsg2;
			this.TopMost = true;

			enType = _enType;

			tmrSec = new System.Threading.Timer(new System.Threading.TimerCallback(SecCount), null, 1000, 1000);

			intCloseSec = intclosesec;

			switch (enType)
			{
				case enMessageType.YesNo:
					btnYes.Visible = true;
					btnNO.Visible = true;
					break;

				case enMessageType.None:		//none으로 설정시 10초후에 사라 진다.
					btnOK.Visible = true;
					intCloseSec = 10;
					break;

				case enMessageType.OK:
					btnOK.Visible = true;
					break;

				default:
					btnOK.Visible = true;
					break;
			}

			this.Select(true, false);
			this.BringToFront();
		}



		/// <summary>
		/// 자동 닫힘 쓰레드...
		/// </summary>
		/// <param name="obj"></param>
		private void SecCount(object obj)
		{
			if (intCloseSec < 1) return;

			intSecCnt++;

			if (intSecCnt >= intCloseSec)
			{
				switch (enType)
				{
					case enMessageType.YesNo:
						this.DialogResult = DialogResult.Yes;
						break;				

					default:
						this.DialogResult = DialogResult.OK;
						break;
				}

				//CloseForm(this);
			}

		}

		delegate void delCloseForm(Form frm);

		private void CloseForm(Form frm)
		{
			if (frm.InvokeRequired)
			{
				frm.Invoke(new delCloseForm(CloseForm), new object[] { frm });
				return;
			}

		
			frm.Close();
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
            finally
            {
                this.Close();
            }

        }

		private void frmMessage_Load(object sender, EventArgs e)
		{
			this.WindowState = FormWindowState.Normal;
			this.Select();
			this.BringToFront();			
		}

		private void frmMessage_StyleChanged(object sender, EventArgs e)
		{			
			this.WindowState = FormWindowState.Normal;
		}

		private void frmMessage_Deactivate(object sender, EventArgs e)
		{
			
			this.Activate();
			this.WindowState = FormWindowState.Normal;
			this.Select();
			this.BringToFront();
		}

		private void frmMessage_SizeChanged(object sender, EventArgs e)
		{
			this.Activate();
			this.WindowState = FormWindowState.Normal;
			this.Select();
			this.BringToFront();
		}

		private void frmMessage_FormClosed(object sender, FormClosedEventArgs e)
		{
			tmrSec.Dispose();
		}
    }
}