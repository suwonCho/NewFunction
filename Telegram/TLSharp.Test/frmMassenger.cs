using Function;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Telegram.Massenger;

namespace TLSharp.Test
{
	public partial class frmMassenger : Function.form.frmBaseForm
	{
		Client _client = null;
		Function.Setting config;

		public frmMassenger()
		{
			InitializeComponent();


			config = new Setting("config.xml", true);

			this.SaveConfigFileType = Function.form.enConfigFileType.ConfigXml;
			this.SavePosition = true;		

			this.SavePosition_Setting = config;

			textBox1.Text = config.Value_Get("PgmSetting", "PhoneNumber", enSettingValueType.Value);

		}

		private async void btnConnect_Click(object sender, EventArgs e)
		{
			try
			{

				string phone = textBox1.Text.Trim();

				config.Group_Select("PgmSetting", true);

				config.Value_Set("PhoneNumber", phone, null, null, null);
				config.Setting_Save();


				_client = new Client(phone, fnc.apiId, fnc.apiHash, this);

				await _client.Connect();

				if(_client.isAuth())
				{
					SetMessage(false, "연결이 되었습니다.", false);
				}
				else
					SetMessage(false, "연결이 되었습니다. 인증이 필요 합니다.", false);

			}
			catch(Exception ex)
			{
				ProcException(ex, "btnConnect_Click", false);
			}
		}

		private void btnPCAuth_Click(object sender, EventArgs e)
		{
			try
			{
				if(_client.AuthFormOpen())
				{
					SetMessage(false, "인증이 완료 되었습니다.", true);
				}
				else
					SetMessage(true, "인증이 취소/실패 되었습니다.", true);

			}
			catch(Exception ex)
			{
				ProcException(ex, "btnPCAuth_Click", false);
			}
		}

		/// <summary>
		/// 연락처 조회
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void btnGetContact_Click(object sender, EventArgs e)
		{

			try
			{
				DataTable dt = await _client.ContactsGet();

				grdContacts.DataSource = dt;
			}
			catch(Exception ex)
			{
				ProcException(ex, "btnGetContact_Click");
			}


		}

		/// <summary>
		/// 대화 목록 조회
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void btnChatList_Click(object sender, EventArgs e)
		{
			try
			{
				DataTable dt = await _client.ChatListGet();

				grdChat.DataSource = dt;
			}
			catch(Exception ex)
			{
				ProcException(ex, "btnChatList_Click");
			}
		}

		/// <summary>
		/// 선택된 datarow로 메시지 전송
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void btnSendMsgDataRow_Click(object sender, EventArgs e)
		{
			try
			{
				if (grdContacts.SelectedRows.Count < 1)
				{
					SetMessage(true, "연락처를 선택하여 주십시요.", false);
					return;
				}

				DataRow dr;

				foreach(DataGridViewRow vr in grdContacts.SelectedRows)
				{

					dr = ((DataRowView)vr.DataBoundItem).Row;

					await _client.SendMessageByContacts(dr, txtMsgContacts.Text);
				}


			}
			catch(Exception ex)
			{
				ProcException(ex, "btnSendMsgDataRow_Click");
			}
		}

		/// <summary>
		/// 선택된 datarow로 메시지 전송 by 전화번호
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSendMsgPhone_Click(object sender, EventArgs e)
		{
			try
			{
				if (grdContacts.SelectedRows.Count < 1)
				{
					SetMessage(true, "연락처를 선택하여 주십시요.", false);
					return;
				}
				

				foreach (DataGridViewRow vr in grdContacts.SelectedRows)
				{
					string phone = Fnc.obj2String(vr.Cells["phone"].Value);

					_client.SendMessageByContacts(enContactInfoType.phoneNumber, phone, txtMsgContacts.Text.Trim());
				}
			}
			catch(Exception ex)
			{
				ProcException(ex, "btnSendMsgPhone_Click");
			}
		}

		private async void btnSendChatMsgDataRow_Click(object sender, EventArgs e)
		{
			if (grdChat.SelectedRows.Count < 1)
			{
				SetMessage(true, "채팅 방을 선택하여 주십시요.", false);
				return;
			}

			DataRow dr;

			foreach (DataGridViewRow vr in grdChat.SelectedRows)
			{

				dr = ((DataRowView)vr.DataBoundItem).Row;

				await _client.SendMessageToChat(dr, txtMsgChat.Text.Trim());
			}

		}

		private async void btnSendChatMsgPhone_Click(object sender, EventArgs e)
		{
			if (grdChat.SelectedRows.Count < 1)
			{
				SetMessage(true, "채팅 방을 선택하여 주십시요.", false);
				return;
			}
			
			foreach (DataGridViewRow vr in grdChat.SelectedRows)
			{
				string id = Fnc.obj2String(vr.Cells["id"].Value);
				

				await _client.SendMessageToChat(enChatsInfoType.id , id, txtMsgChat.Text.Trim());
			}
		}

		private void textBox1_KeyDown(object sender, KeyEventArgs e)
		{
			
		}



	}
}
