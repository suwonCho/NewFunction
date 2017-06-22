using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Telegram.Massenger
{
	public partial class AuthForm : Function.form.frmBaseForm
	{
		Client _client = null;

		string _hash = null;

		public AuthForm(Client client)
		{
			InitializeComponent();

			_client = client;
		}

		private async void usrNumberInput_OK_Click(object sender, EventArgs e)
		{
			await MakeAuth();
		}

		private async Task MakeAuth()
		{
			try
			{
				if (_hash == null)
				{
					Function.clsFunction.ShowMsg(this, "인증번호요청", "인증번호 요청을 먼저 하여 주십시요", Function.form.frmMessage.enMessageType.OK);
					return;
				}

				await _client.MakeAuthAsync(_hash, usrNumberInput.Value);

				DialogResult = DialogResult.OK;
				this.Close();
			}
			catch (Exception ex)
			{
				ProcException(ex, "MakeAuth");
			}
		}


		private void usrNumberInput_Cancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		}

		/// <summary>
		/// 인증번호 요청
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void btnReqNumber_Click(object sender, EventArgs e)
		{
			try
			{
				await ReqNumber();
			}
			catch(Exception ex)
			{
				ProcException(ex, "인증번호 요청", false);
				Function.clsFunction.ShowMsg(this, "인증번호 전송 실패", "인증번호 전송 실패 시 재 연결 후 시도 하여 주십시요", Function.form.frmMessage.enMessageType.OK);
			}
		}

		private async Task ReqNumber()
		{
			Task<string> rst = _client.SendCodeRequest();

			SetMessage(false, "인증 번호 요청을 하였습니다.", false);
			
			_hash = await rst;

		}


	}
}
