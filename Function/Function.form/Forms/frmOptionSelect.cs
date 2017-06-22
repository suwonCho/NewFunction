using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Function.form
{
	public partial class frmOptionSelect : Form
	{
		/// <summary>
		/// 로드시 기본 선택 항목
		/// </summary>
		object[] _defaltKey;

		/// <summary>
		/// 폴 로드 여부
		/// </summary>
		bool _isFormLoad = false;

		public enum enSelectType { Radio, CheckBox}
		
		private enSelectType _selectType = enSelectType.Radio;

		/// <summary>
		/// 메시지
		/// </summary>
		private string _msg = string.Empty;

		/// <summary>
		/// 항목 선택 타입을 가져오거나 설정 합니다.
		/// </summary>
		public enSelectType SelectType
		{
			get { return _selectType; }
			set
			{
				_selectType = value;
				
			}
		}

		
		Dictionary<object, string> dicItems = new Dictionary<object, string>();

		List<Control> lstBtns = new List<Control>();

		public object[] SelectedKeys;


		/// <summary>
		/// 타이틀을 가져오거나 설정 합니다.
		/// </summary>
		public string Title
		{
			get; set;
		}


		/// <summary>
		/// 선택 아이템을 추가 합니다.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="Text"></param>
		public void Items_Add(object key, string Text)
		{
			dicItems.Add(key, Text);
		}

		
		/// <summary>
		/// 옵션 선택 폼을 생성합니다.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="msg"></param>
		/// <param name="defealtKey"></param>
		public frmOptionSelect(string title, string msg, object[] defealtKey)
		{
			InitializeComponent();

			_defaltKey = defealtKey;
			Title = title;
			_msg = msg;		

		}

		public void Init()
		{
			

			//메시지 크기 조정
			int line_qty = _msg.Split(new string[] { "\r\n" }, StringSplitOptions.None).Length;
			int line_Height = Fnc.floatToInt(Function.form.control.Font_Control_String_Size_Get(lblMsg, control.enControl_Criteria.height, "A"));
			int line_Width = Fnc.floatToInt(Function.form.control.Font_Control_String_Size_Get(lblMsg, control.enControl_Criteria.width, "A"));

			lblTitle.Text = Title;
			lblMsg.Text = _msg;

			if (line_qty > 2)
			{   //2줄 이상
				lblMsg.Height = Fnc.obj2int(((line_qty - 1) * line_Height) + (line_Height * 0.5));
			}
			else
			{   //한줄일 경우
				//float txtWidth = Function.form.control.Font_Control_String_Size_Get(lblMsg, control.enControl_Criteria.width, _msg);
				float txtHeight = Function.form.control.Font_Control_String_Size_Get(lblMsg, control.enControl_Criteria.height, _msg);
				lblMsg.Height = Fnc.obj2int(txtHeight + (line_Height * 0.5));
			}


			//기존 컨트롤은 삭제한다.
			foreach (Control c in lstBtns)
			{
				panel1.Controls.Remove(c);
			}

			lstBtns.Clear();


			//라디오나 체크 박스 추가
			int h = lblMsg.Top + lblMsg.Height + Fnc.obj2int(line_Height * 0.1);
			Control btn;
			bool chk;

			foreach(object o in dicItems.Keys)
			{

				chk = false;

				if (_defaltKey != null)
				{
					foreach (object d in _defaltKey)
					{
						if (o.Equals(d))
						{
							chk = true;
							break;
						}
					}
				}



				switch (SelectType)
				{
					case enSelectType.CheckBox:
						CheckBox cb = new CheckBox();
						cb.Checked = chk;
						btn = cb;												
						break;

					default:
						RadioButton rb = new RadioButton();
						rb.Checked = chk;
						btn = rb;
						break;

				}

				btn.Tag = o;
				btn.Text = dicItems[o];
				panel1.Controls.Add(btn);
				btn.Left = lblMsg.Left + Fnc.obj2int(line_Width);
				btn.Width = lblMsg.Width - +Fnc.obj2int(line_Width);
				btn.Top = h;
				h = btn.Top + btn.Height + Fnc.obj2int(line_Height * 0);

				lstBtns.Add(btn);
			}
			
			//panel height가 49일때 form은 120 
			this.Height = 120 + (h - 49);

		}
		

		private void picClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnNo_Click(object sender, EventArgs e)
		{			
			this.DialogResult = System.Windows.Forms.DialogResult.No;
			this.Close();
		}

		private void btnYes_Click(object sender, EventArgs e)
		{	
			List<object> keys = new List<object>();
			bool rst;

			foreach(Control c in lstBtns )
			{
				rst = false;

				RadioButton r = c as RadioButton;

				if(r != null)
				{
					if (r.Checked) rst = true;
				}
				else
				{
					CheckBox ck = c as CheckBox;
					if(ck != null && ck.Checked)
					{
						rst = true;
					}
				}

				if (rst) keys.Add(c.Tag);

			}

			SelectedKeys = keys.ToArray();

			this.DialogResult = System.Windows.Forms.DialogResult.OK;

			this.Close();
		}

		private void frmMessage_Load(object sender, EventArgs e)
		{
			Init();
			this.Select(true, false);
			this.BringToFront();

			_isFormLoad = true;
		}

		private void frmMessage_FormClosed(object sender, FormClosedEventArgs e)
		{
			
		}

		private void lblMsg_DoubleClick(object sender, EventArgs e)
		{
			
		}
		
	}
}
