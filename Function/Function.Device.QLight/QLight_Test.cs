using Function;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using System.Threading;

namespace Function.Device.QLight
{
	/// <summary>
	/// QLight 경광등 테스트 폼
	/// </summary>
	public partial class QLight_Test : Function.form.subBaseForm
	{

		QL _ql = null;


		Label[] lblCStatus;
		Color[] colCColor = new Color[] { Color.Red, Color.Yellow, Color.Green, Color.Blue, Color.White, Color.CadetBlue };
		int[] tags = new int[] { 0, 0, 0, 0, 0, 0 };

		System.Threading.Timer thBlink;
		bool isBlink = false;

		/// <summary>
		/// 경광등 설정프로그램 경로
		/// </summary>
		string setting_pgm = "IP_Setting.exe";

		public QLight_Test()
		{
			InitializeComponent();
			
		}

		public QLight_Test(QL ql, bool readOnly)
		{
			InitializeComponent();
			_ql = ql;

			setting_pgm = Application.StartupPath + "\\" + setting_pgm;

			if (readOnly)
			{
				inpIpAddress.InputType = Function.form.usrInputBox.enInputType.LABEL;
				inpPort.InputType = Function.form.usrInputBox.enInputType.LABEL;
			}		

		}
		

		/// <summary>
		/// 경광등 상태 변경
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="status"></param>
		private void _ql_OnQL_StatusChanged(QL sender, enQL_Status status)
		{
			if(picStatus.InvokeRequired)
			{
				picStatus.Invoke(new delQL_StatusChanged(_ql_OnQL_StatusChanged), sender, status);
				return;
			}

			if (status == enQL_Status.Connected)
			{
				picStatus.Image = Function.resIcon16.button_green;
			}
			else
				picStatus.Image = Function.resIcon16.button_red;


			lblStatus.Text = status.ToString();
		}

		private void QLight_Test_Load(object sender, EventArgs e)
		{

			//알람 환결 설정 프로그래 
			if (System.IO.File.Exists(setting_pgm))
			{
				//있음
				btnSettingPgm.Enabled = true;
				btnSettingPgm.Image = Function.resIcon16.status_on;

			}
			else
			{
				//없음
				btnSettingPgm.Enabled = false;
				btnSettingPgm.Image = Function.resIcon16.status_off;

			}

			lblCStatus = new Label[] { label5, label6, label7, label8, label9, label10 };

			thBlink = new System.Threading.Timer(new TimerCallback(lblBlink), null, 1000, 800);

			inpAlarm.ComboBoxItems.AddRange(Fnc.EnumItems2Strings(new enSoundKind()));
			inpAlarm.ComboBoxSelectIndex = 0;

			if (_ql == null)
			{
				_ql = new QL("210.100.103.85", 20000);				
			}

			inpIpAddress.Value = _ql.IpAddress;
			inpPort.Value = _ql.Port.ToString();

			_ql.OnQL_StatusChanged += _ql_OnQL_StatusChanged;
			_ql_OnQL_StatusChanged(_ql, _ql.QL_Status);

			_ql.OnQL_DataChanged += _ql_OnQL_DataChanged;
			_ql_OnQL_DataChanged(_ql, _ql.Data_lastStatus);


		}

		/// <summary>
		/// 시스템 타이머로 블링크 처리
		/// </summary>
		/// <param name="obj"></param>
		private void lblBlink(object obj)
		{
			try
			{
				Color col;

				isBlink = !isBlink;

				for(int i=2; i < 7; i++)
				{

					if (tags[i - 2] < 2) continue;

					if(isBlink)
						col = colCColor[i - 2];
					else
						col = Color.Transparent;

					Function.form.control.Invoke_Control_Color(lblCStatus[i - 2], null, col);

				}
				

			}
			catch
			{

			}

		}


		private void _ql_OnQL_DataChanged(QL sender, byte[] data)
		{
			try
			{

				Color col;
				int tag = 0;

				for(int i=2; i< 8; i++)
				{
					if (data[i] != 0)
					{
						col = colCColor[i - 2];
						tag = data[i];
					}
					else
					{
						col = Color.Transparent;
						tag = 0;
					}
					
					Function.form.control.Invoke_Control_Color(lblCStatus[i-2], null, col);
					//Function.form.control.Invoke_Control_Tag(lblCStatus[i - 2], tag);

					tags[i - 2] = tag;

					//알람은 텍스트 변경
					if (i == 7)
					{
						Function.form.control.Invoke_Control_Text(lblCStatus[i - 2], tag == 0 ? "A" : tag.ToString() );
					}

				}
			}
			catch
			{ }
		}


		/// <summary>
		/// 폼에서 아이피를 변경한다.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void inpIpAddress_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
			{
				try
				{
					_ql.IpAddress = inpIpAddress.Text;
					inpIpAddress.Value = inpIpAddress.Text;
				}
				catch(Exception ex)
				{
					ProcException(ex, "IP변경", false);

					inpIpAddress.Value = _ql.IpAddress;
				}
			}
		}


		/// <summary>
		/// 폼에서 포트를 변경한다
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void inpPort_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				try
				{
					_ql.Port = int.Parse(inpPort.Text);

					inpPort.Value = inpPort.Text;
				}
				catch (Exception ex)
				{
					ProcException(ex, "포트 변경", false);

					inpPort.Value = _ql.Port.ToString();
				}
			}
		}

		//버튼 클릭
		private void button13_Click(object sender, EventArgs e)
		{
			Button btn = sender as Button;

			if (btn == null) return;

			int idx = Fnc.obj2int(btn.Tag);
			if (idx < 2 || idx > 6) return;

			enLampStatus st;
			enLampKind lamp = (enLampKind)idx;

			switch(btn.Text)
			{
				case "ON":
					st = enLampStatus.On;
					break;

				case "Blink":
					st = enLampStatus.Blink;
					break;

				default:
					st = enLampStatus.Off;
					break;
			}

			_ql.Order_Lamp(lamp, st);
			
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			_ql.Clear();
		}

		private void btnAlarm_Click(object sender, EventArgs e)
		{
			enSoundKind k = (enSoundKind)inpAlarm.ComboBoxSelectIndex;

			_ql.Order_Alarm(k);
		}

		private void QLight_Test_FormClosed(object sender, FormClosedEventArgs e)
		{
			_ql.OnQL_StatusChanged -= _ql_OnQL_StatusChanged;		

			_ql.OnQL_DataChanged -= _ql_OnQL_DataChanged;
		}

		private void btnSettingPgm_Click(object sender, EventArgs e)
		{
			if (!System.IO.File.Exists(setting_pgm)) return;

			try
			{
				System.Diagnostics.Process pro = new System.Diagnostics.Process();

				pro.StartInfo.FileName = setting_pgm;
				pro.Start();
			}
			catch(Exception ex)
			{
				ProcException(ex, ex.Message, false);
			}

		}
	}//end class
}
