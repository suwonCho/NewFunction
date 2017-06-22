using Function;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TeleSharp.TL;
using TeleSharp.TL.Messages;

namespace Telegram.Massenger
{
	/// <summary>
	/// 텔레그램 메신져 관리 클라이언트
	/// </summary>
    public class Client
	{
		/// <summary>
		/// 메신져 전화번호를 입력합니다. 국가번호 포함 </para>ex) +821012345678
		/// </summary>
		string _phoneNumber;
		/// <summary>
		/// 텔레그램 App설정의 api_id
		/// </summary>
		int _apiId;
		/// <summary>
		/// 텔레그램 App설정의 api_hash
		/// </summary>
		string _apiHash;
		/// <summary>
		/// 인증번호 입력을 위해 창을 띄울경우 부모폼, 없을경우 화면 가운데 띄운다.
		/// </summary>
		Form _parentForm;

		/// <summary>
		/// 연락처 데이터 테이블
		/// </summary>
		DataTable _contacts = null;

		/// <summary>
		/// 연락처 데이터 테이블
		/// </summary>
		public DataTable Contacts
		{
			get { return _contacts; }
		}

		DataTable _chatList = null;

		/// <summary>
		/// 채팅 목록을 가지고 온다
		/// </summary>
		public DataTable ChatList
		{
			get { return _chatList; }
		}


		TLSharp.Core.TelegramClient client;

		/// <summary>
		/// 클라이언트를 생성합니다.
		/// </summary>
		/// <param name="PhoneNumber">메신져 전화번호를 입력합니다. 국가번호 포함 </para>ex) +821012345678</param>
		/// <param name="ApiId">텔레그램 App설정의 api_id</param>
		/// <param name="ApiHash">텔레그램 App설정의 api_hash</param>
		/// <param name="ParentForm">인증번호 입력을 위해 창을 띄울경우 부모폼, 없을경우 화면 가운데 띄운다.</param>
		public Client(string PhoneNumber,int ApiId, string ApiHash, Form ParentForm = null)
		{
			_phoneNumber = PhoneNumber;
			_apiId = ApiId;
			_apiHash = ApiHash;
			_parentForm = ParentForm;
		}

		/// <summary>
		/// 서버와 연결을 시작합니다.
		/// </summary>
		public async Task Connect()
		{
			client = new TLSharp.Core.TelegramClient(_apiId, _apiHash);
			await client.ConnectAsync();			
		}

		/// <summary>
		/// 인증여부를 확인합니다.
		/// </summary>
		/// <returns></returns>
		public bool isAuth()
		{
			return client.IsUserAuthorized();
		}

		/// <summary>
		/// 인증여부를 확인합니다.
		/// </summary>
		/// <param name="isThrowException">미인증일 경우 Exception 발생 여부</param>
		/// <returns></returns>
		public bool isAuth(bool isThrowException)
		{
			bool rst = isAuth();

			if (isThrowException && !rst)
				throw new Exception("Telegram Client 인증이 되어 있지 않습니다.");

			return rst;
		}

		/// <summary>
		/// 핸드폰 텔레그램으로 인증번호를 요청 합니다.
		/// </summary>
		/// <returns>hash code</returns>
		public async Task<string> SendCodeRequest()
		{
			//핸드폰 텔레그램으로 인증번호를 요청 합니다.
			var hash = await client.SendCodeRequestAsync(_phoneNumber);

			return hash;
		}


		/// <summary>
		/// 인증 요청 처리를 한다.
		/// </summary>
		/// <param name="hash">인증 요청(SendCodeRequest) 시 생성된 코드</param>
		/// <param name="code">핸드폰 텔래그램으로 받은 코드</param>
		/// <returns></returns>
		public async Task<TLUser> MakeAuthAsync(string hash, string code)
		{
			//인증을 요청한다.
			var user = await client.MakeAuthAsync(_phoneNumber, hash, code);

			return user;
		}
		

		/// <summary>
		/// 인증창을 띄웁니다.
		/// </summary>
		/// <param name="PhoneNumber"></param>
		public bool AuthFormOpen()
		{
			
			AuthForm pass = new AuthForm(this);

			if (_parentForm == null || _parentForm.WindowState == FormWindowState.Minimized || !_parentForm.Visible)
			{   //부모폼에 표시 못함
				pass.StartPosition = FormStartPosition.CenterScreen;
				pass.TopMost = true;
				pass.ShowDialog();
			}
			else
			{   //부모폼에 표시
				pass.StartPosition = FormStartPosition.CenterParent;
				pass.ShowDialog(_parentForm);
			}

			return pass.DialogResult == DialogResult.OK;

		}


		/// <summary>
		/// 연락처를 가져 온다.
		/// </summary>
		/// <returns></returns>
		public async Task<DataTable> ContactsGet()
		{
			isAuth(true);

			//연락처를 받아 온다
			TeleSharp.TL.Contacts.TLContacts rst = await client.GetContactsAsync();

			if(_contacts == null)
			{
				_contacts = Fnc.DataTable_SchemaByObject("TelegramContacts", new TLUser());
			}

			_contacts.Rows.Clear();
			//DataRow dr;

			Fnc.DataTable_InsDataFromObject(_contacts, rst.users.lists);


			//foreach (TLUser u in rst.users.lists)
			//{
			//	dr = _contacts.NewRow();
			//	Fnc.DataRow_InsDataFromObject(dr, u);

			//	_contacts.Rows.Add(dr);
			//}


			return _contacts;
		}
		

		/// <summary>
		/// 연락처에서 사용자 정보를 조회 한다.
		/// </summary>
		/// <param name="ctype">조회 할 필드</param>
		/// <param name="info">조회 할 정보</param>
		/// <returns></returns>
		public async Task<DataRow[]> UserInfoGetFromContact(enContactInfoType ctype, string info)
		{
			DataRow[] rtn;

			//연락처 조회전이면 조회 한다.
			if (_contacts == null) await ContactsGet();

			string con = "deleted = false and ";

			switch(ctype)
			{
				case enContactInfoType.id:
					con += "id = {0}";
					break;

				case enContactInfoType.name:
					con += "( first_name like '%{0}%' or last_name like '%{0}%' )";
					break;

				case enContactInfoType.phoneNumber:
					con += string.Format("phone = '{0}'", fnc.PhoneNumberInit(info));
					break;

				case enContactInfoType.username:
					con += "username = '{0}'";
					break;
			}

			con = string.Format(con, info);

			rtn = _contacts.Select(con);

			return rtn;

		}


		/// <summary>
		/// 사용자에게 메시지를 보낸다. by contact datarow
		/// </summary>
		/// <param name="info"></param>
		/// <param name="msg">보낼 메시지</param>
		/// <returns></returns>
		public async Task SendMessageByContacts(DataRow info, string msg)
		{
			string id = Fnc.obj2String(info["id"]);

			await SendMessageByContacts(enContactInfoType.id, id, msg);
		}


		/// <summary>
		/// 사용자에게 메시지를 보낸다.
		/// </summary>
		/// <param name="ctype">조회 할 필드</param>
		/// <param name="info">조회 할 정보</param>
		/// <param name="msg">보낼 메시지</param>
		/// <returns></returns>
		public async Task SendMessageByContacts(enContactInfoType ctype, string info, string msg)
		{
			//인증 확인
			isAuth(true);

			//연락처 조회전이면 조회 한다.
			if (_contacts == null) await ContactsGet();

			DataRow[] rows = await UserInfoGetFromContact(ctype, info);

			if (rows.Length < 1 || info == null || info.Trim().Equals(string.Empty)) throw new Exception(string.Format("[Type]{0} [Info]{1} 로 연락처 정보를 찾을 수 없습니다.", ctype, info));

			foreach (DataRow dr in rows)
			{
				await client.SendMessageAsync(new TLInputPeerUser() { user_id = Fnc.obj2int(dr["id"]) }, msg);
			}

		}


		/// <summary>
		/// 채팅 리스트를 가지고 온다.(개인 채팅은 제외)
		/// </summary>
		/// <returns></returns>
		public async Task<DataTable> ChatListGet()
		{

			
			//TeleSharp.TL.Messages.TLAbsDialogs dial = await client.GetUserDialogsAsync();

			TLDialogs dialogs = (TLDialogs)await client.GetUserDialogsAsync();


			if (_chatList == null)
			{
				_chatList = Fnc.DataTable_SchemaByObject("TelegramChatList", dialogs.chats.lists[0]);

				//_chatList.Columns.Add("access_hash", typeof(System.String));
			}

			_chatList.Rows.Clear();
			//DataRow dr;




			Fnc.DataTable_InsDataFromObject(_chatList, dialogs.chats.lists);

			return _chatList;
			

		}


		/// <summary>
		/// 채팅방에 메시지를 보낸다.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public async Task SendMessageToChat(DataRow info, string msg)
		{
			string id = Fnc.obj2String(info["id"]);

			await SendMessageToChat(enChatsInfoType.id, id, msg);
		}

		/// <summary>
		/// 채팅방을 조건으로 조회 한다.
		/// </summary>
		/// <param name="ctype"></param>
		/// <param name="info"></param>
		/// <returns></returns>
		public async Task<DataRow> ChatGetFromChats(enChatsInfoType ctype, string info)
		{
			DataRow[] rtn;

			//연락처 조회전이면 조회 한다.
			if (_contacts == null) await ContactsGet();

			string con = "";

			switch (ctype)
			{
				case enChatsInfoType.id:
					con += "id = {0}";
					break;

				case enChatsInfoType.title:
					con += "title = '{0}'";
					break;
			}

			con = string.Format(con, info);

			rtn = _chatList.Select(con);

			return rtn.Length > 0 ? rtn[0] : null;

		}


		/// <summary>
		/// 채팅방에 메시지를 보낸다.
		/// </summary>
		/// <param name="ctype"></param>
		/// <param name="info"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public async Task SendMessageToChat(enChatsInfoType ctype, string info, string msg)
		{
			//인증 확인
			isAuth(true);


			//채팅방 리스트 조회전이면 조회 한다.
			if (_chatList == null) await ChatListGet();

			DataRow row = await ChatGetFromChats(ctype, info);

			if (row == null || info == null || info.Trim().Equals(string.Empty)) throw new Exception(string.Format("[Type]{0} [Info]{1} 로 채팅방 정보를 찾을 수 없습니다.", ctype, info));
						
			await client.SendMessageAsync(new TLInputPeerChannel() { channel_id = Fnc.obj2int(row["id"]), access_hash = Fnc.obj2Long(row["access_hash"]) }, msg);			

		}


	}	//END CLASS
}
