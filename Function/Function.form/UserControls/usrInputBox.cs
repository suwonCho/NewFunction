using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using System.Runtime.InteropServices;
using System.Drawing.Design;
namespace Function.form
{
	public partial class usrInputBox : UserControl
	{
		bool isSizeChanging = false;

		/// <summary>
		/// 라벨 값 변경 중 여부
		/// </summary>
		bool _isSetLabelValue = false;

	


		///// <summary>
		///// 컨트롤의 배경색을 가져오거나 설정합니다.
		///// </summary>
		//public Color BackColor
		//{
		//	get { return base.BackColor; }
		//	set
		//	{
		//		base.BackColor = value;
		//		_dLabel_backColor = value;
		//	}
		//}

		#region label속성
		public string Label_Text
		{
			get { return label.Text; }
			set
			{
				label.Text = value;
				labelwidth_resize();
			}
		}

		public Color Label_BackColor
		{
			get { return label.BackColor; }
			set { label.BackColor = value; }
		}

		public Color Label_ForeColor
		{
			get { return label.ForeColor; }
			set { label.ForeColor = value; }
		}

		public ContentAlignment Label_TextAlign
		{
			get { return label.TextAlign; }
			set { label.TextAlign = value; }
		}

		public Font Label_Font
		{
			get { return label.Font; }
			set { label.Font = value; }
		}

		public BorderStyle Label_BorderStyle
		{
			get { return label.BorderStyle; }
			set { label.BorderStyle = value; }
		}

		public bool Label_Visable
		{
			get { return !splitContainer1.Panel1Collapsed; }
			set
			{
				splitContainer1.Panel1Collapsed = !value;
			}
		}




		public Orientation Orientation
		{
			get { return splitContainer1.Orientation; }
			set 
			{
				if (splitContainer1.Orientation == value) return;

				splitContainer1.SplitterDistance = 1;
				
				splitContainer1.Orientation = value;

				LabelWidth = -1;			

				usrInputBox_SizeChanged(null, null);
			}
		}

		private int _labelwidth = 0;

		/// <summary>
		/// 라벨 크기를 설정한다. -1이면 자동으로 너비를 설정한다.
		/// </summary>		
		public int LabelWidth
		{
			get
			{
				return _labelwidth < 0 ? -1 : splitContainer1.SplitterDistance;
							
			}
			set
			{
				if (value < 0)
				{
					_labelwidth = -1;
					labelwidth_resize();

				}
				else
					splitContainer1.SplitterDistance = _labelwidth = value;
			}
			
		}

		private void labelwidth_resize()
		{
			if (_labelwidth >= 0 || splitContainer1.Panel1Collapsed) return;

			float f;

			if(splitContainer1.Orientation == System.Windows.Forms.Orientation.Vertical)
				f = Function.form.control.Font_Control_String_Size_Get(label, control.enControl_Criteria.width,
					label.Text) + Function.form.control.Font_Control_String_Size_Get(label, control.enControl_Criteria.width,
					"*");
			else
				f = Function.form.control.Font_Control_String_Size_Get(label, control.enControl_Criteria.height,
					label.Text) * Function.form.control.Font_Control_String_Size_Get(label, control.enControl_Criteria.height,
					"*");

			int i = int.Parse(Math.Round(Double.Parse(f.ToString()), 0).ToString());

			splitContainer1.SplitterDistance = i;

		}

		#endregion

		//int _width = 0;

		//private Control cur;

		///// <summary>
		///// 컨트롤의 사이즈를 조정 한다.
		///// </summary>
		//public new int Width
		//{
		//    get
		//    {
		//        return _width < 0 ? -1 : this.Width;
		//    }
		//    set
		//    {
		//        if (value < 0)
		//        {
		//            _width = -1;
		//            _labelwidth = -1;
		//            width_resize();
		//        }
		//        else
		//            this.Width = _width = value;
		//    }
		//}

		//private void width_resize()
		//{
		//    if(_width >= 0) return;
			
		//    labelwidth_resize();

		//    float f = Function.form.control.Font_Control_String_Size_Get(cur, control.enControl_Criteria.width,
		//        TEXT_String) * 1.2f + (ChangeMark_Visable ? label1.Width + 5 : 0);

		//    this.Width = int.Parse(f.ToString());


		//}

#region DataLabel속성

		bool _dLabel_FontAutoSize = false;
		/// <summary>
		/// 데이터 라벨의 폰트 사이즈를 자동 저장 합니다.
		/// </summary>
		public bool DLabel_FontAutoSize
		{
			get { return _dLabel_FontAutoSize; }
			set
			{
				_dLabel_FontAutoSize = value;
			}
		}


		enBlinkType _dLabel_Blink = enBlinkType.None;
		/// <summary>
		/// 데이터 라벨의 깜박이는 여부
		/// </summary>
		public enBlinkType DLabel_Blink
		{
			get { return _dLabel_Blink; }
			set
			{
				if (_dLabel_Blink == value) return;

				_dLabel_Blink = value;

				dLabel_Blink_Start();
			}
		}

		Color _dLabel_backColor = System.Drawing.SystemColors.Control;
		Color _dLabel_BlinkColor = Color.White;
		/// <summary>
		/// 테이터 라벨 깜박이는 색깔을 지정하거나 가져 온다.
		/// </summary>
		public Color DLabel_BlinkColor
		{
			get { return _dLabel_BlinkColor; }
			set
			{
				_dLabel_BlinkColor = value;
			}
		}

		System.Threading.Timer _tmrDlabelBlink;


		bool _focused = false;


		private void dLabel_Blink_Start()
		{
			if (_dLabel_Blink != enBlinkType.None)
			{
				_tmrDlabelBlink = new System.Threading.Timer(new TimerCallback(dLabel_Blink_tmr), null, 0, 1000);
			}
			

		}

		private void dLabel_Blink_tmr(object obj)
		{
			if (_dLabel_Blink == enBlinkType.None) return;

			if (_dLabel_Blink == enBlinkType.Focused && !_focused)
			{
				Function.form.control.Invoke_Control_Color(this, null, _dLabel_backColor);
			}
			else
			{
				Function.form.control.Invoke_Control_Color(this, null, _dLabel_BlinkColor);
				Thread.Sleep(350);
				Function.form.control.Invoke_Control_Color(this, null, _dLabel_backColor);
			}
			
		}



		public Color DLabel_BackColor
		{
			get { return lblText.BackColor; }
			set { lblText.BackColor = value; }
		}

		public Color DLabel_ForeColor
		{
			get { return lblText.ForeColor; }
			set { lblText.ForeColor = value; }
		}

		public ContentAlignment DLabel_TextAlign
		{
			get { return lblText.TextAlign; }
			set { lblText.TextAlign = value; }
		}

		public Font DLabel_Font
		{
			get { return lblText.Font; }
			set { lblText.Font = value; }
		}

		public BorderStyle DLabel_BorderStyle
		{
			get { return lblText.BorderStyle; }
			set { lblText.BorderStyle = value; }
		}


		#endregion


		#region TextBox 속성
		/// <summary>
		/// 여러 줄을 입력할 수 있는 TextBox 컨트롤에서 Tab 키를 누를 때 탭 순서에 따라 다음 컨트롤로 포커스를 이동하는 대신 해당 컨트롤에
		/// 탭 문자를 입력할지 여부를 나타내는 값을 가져오거나 설정합니다.
		/// </summary>
		[Description("여러 줄을 입력할 수 있는 TextBox 컨트롤에서 Tab 키를 누를 때 탭 순서에 따라 다음 컨트롤로 포커스를 이동하는 대신 해당 컨트롤에 탭 문자를 입력할지 여부를 나타내는 값을 가져오거나 설정합니다.")]
		public bool TextBox_AcceptsTab
		{
			get { return txtBox.AcceptsTab; }
			set
			{
				txtBox.AcceptsTab = value;
			}
		}


		private int _tabStopsLength = 8;

		/// <summary>
		/// 텍스트 박스에 탭문자 길이를 설정 한다.
		/// </summary>
		[Description("텍스트 박스에 탭문자 길이를 설정 한다.")]
		public int TextBox_TabStopsLength
		{
			get { return _tabStopsLength; }
			set
			{
				_tabStopsLength = value;

				try
				{
					control.SetTabStopsLength(txtBox, _tabStopsLength);
				}
				catch(Exception ex)
				{

				}
			}
		}



		#endregion


		public new int Width
		{
			get { return base.Width; }
			set
			{
				if (value >= 0) base.Width = value;
			}
		}


		public new int Height
		{
			get { return base.Height; }
			set
			{
				if (value >= 0) base.Height = value;
			}
		}
		


		/// <summary>
		/// 현재 컨트롤 실제 사이즈
		/// </summary>
		public int ActualWidth
		{
			get { return this.Width; }
		}


		public bool ChangeMark_Visable
		{
			get { return label1.Visible; }
			set
			{
				label1.Visible = value;			
			}
		}

		public new event usrEventHander Click;

		public new event usrEventHander DoubleClick;

		public enum enInputType 
		{ 
			LABEL, 
			TEXTBOX,			
			MultiLineTEXTBOX,
			COMBO
		};

		public enum enBlinkType
		{
			None,
			Always,
			Focused
		}



		public enum enTextType { All, NumberOlny, None };


		private event KeyEventHandler keydown;

		public new event KeyEventHandler KeyDown
		{
			add
			{
				keydown += value;
			}
			remove
			{
				keydown -= value;
			}

		}


		public bool Multiline
		{
			get { return txtBox.Multiline; }
			set
			{
				txtBox.Multiline = value;

				if (value)
				{
					txtBox.ScrollBars = ScrollBars.Vertical;					
				}
				else
					txtBox.ScrollBars = ScrollBars.None;

			}
		}

		public ContentAlignment TextAlign
		{
			get { return lblText.TextAlign; }
			set
			{
				

				lblText.TextAlign = value;				

				switch (value)
				{
					case ContentAlignment.BottomCenter:
					case ContentAlignment.MiddleCenter:
					case ContentAlignment.TopCenter:
						txtBox.TextAlign =  HorizontalAlignment.Center;
						break;

					case ContentAlignment.BottomLeft:
					case ContentAlignment.MiddleLeft:
					case ContentAlignment.TopLeft:
						txtBox.TextAlign =  HorizontalAlignment.Left;
						break;

					default:
						txtBox.TextAlign = HorizontalAlignment.Right;
						break;
				}

			}
		}

		string _value = string.Empty;

		/// <summary>
		/// 한 줄만 입력할 수 있는 System.Windows.Forms.TextBox 컨트롤에서 암호 문자를 마스킹하는 데 사용되는 문자를 가져오거나 설정합니다.
		/// </summary>
		[Description("한 줄만 입력할 수 있는 System.Windows.Forms.TextBox 컨트롤에서 암호 문자를 마스킹하는 데 사용되는 문자를 가져오거나 설정합니다.")]
		public char TEXTBOX_PasswordChar
		{
			get
			{
				return txtBox.PasswordChar;
			}
			set
			{
				txtBox.PasswordChar = value;
			}
		}


		/// <summary>
		/// 원본 값
		/// </summary>
		public string Value
		{
			get { return _value; }
			set
			{
				_value = value;
			
				string v = Fnc.obj2String(value);		

				if(txtBox.Visible) txtBox.Text = v;
				if(lblText.Visible) lblText.Text = v;
				
				if (cmbBox.DataSource == null)
				{
					cmbBox.Text = v;
					if (!cmbBox.Text.Equals(v)) cmbBox.SelectedIndex = -1;
					
				}
				else
					control.Invoke_ComboBox_SelectedItem(cmbBox, ComboBoxValueMember, v);

				TEXT = _value;
				label1.Text = string.Empty;
				isChange = false;

			}
		}

		private event usrEventHander _text_Changed = null;

		/// <summary>
		/// 텍스트가 변경 되었을경우 발행 하는 이벤트
		/// </summary>
		public event usrEventHander Text_Changed
		{
			add
			{
				_text_Changed += value;
			}
			remove
			{
				_text_Changed -= value;
			}
		}

		public new string Text
		{
			get { return TEXT; }
			set
			{
				TEXT = value;
			}
		}

		/// <summary>
		/// 표시 텍스트
		/// </summary>
		public string TEXT
		{
			get 
			{
				string rtn;
				switch (InputType)
				{
					case enInputType.COMBO:
						if (cmbBox.DropDownStyle == ComboBoxStyle.DropDown)
							rtn = cmbBox.Text;
						else if (cmbBox.DataSource == null)
							rtn = Fnc.obj2String(cmbBox.SelectedItem);						
						else
						{
							if (ComboBoxValueMember == null)
							{
								console.WriteLine("usrInputBox[Name]{0} [프로퍼티]ComboBoxValueMember가 설정 되어 있지 않아 TEXT 속성을 가져 올 수 없습니다.", this.Name);
								rtn = null;

							}
							else
								rtn = control.Invoke_ComboBox_GetSelectValue(cmbBox, ComboBoxValueMember);
						}
						break;

					case enInputType.LABEL:
						rtn = lblText.Text;
						break;

					case enInputType.MultiLineTEXTBOX:
						rtn = txtBox.Text.Replace(usrMultiTextItem.Seperator.ToString(), "\r\n");
						break;

					default:
						rtn = txtBox.Text;
						break;
				}

				return rtn;
			}
			set
			{
				switch (InputType)
				{
					case enInputType.COMBO:
						if (cmbBox.DropDownStyle == ComboBoxStyle.DropDown)
							cmbBox.Text = value;
						else if (cmbBox.DataSource == null)
							cmbBox.SelectedItem = TEXT;						
						else
							control.Invoke_ComboBox_SelectedItem(cmbBox, ComboBoxValueMember, Fnc.obj2String(value));
						break;				

					case enInputType.LABEL:
						lblText.Text = Fnc.obj2String(value);
						//databinding 이용시 textbox값을 이용하므로
						_isSetLabelValue = true;
						txtBox.Text = Fnc.obj2String(value);
						Application.DoEvents();
						_isSetLabelValue = false;

						break;

					default:
						txtBox.Text = Fnc.obj2String(value);
						break;
				}

				Check_TextChanged();
			}
		}

		/// <summary>
		/// userInputBox의 값을 Commit 한다. Text -> Value
		/// </summary>
		public void Commit()
		{
			Value = Text;
		}



		private enInputType _inputType =  enInputType.TEXTBOX;


		public enInputType InputType
		{
			get { return _inputType; }
			set
			{
				if (value == _inputType) return;

				_inputType = value;

				lblText.Visible = false;
				txtBox.Visible = false;				
				cmbBox.Visible = false;

				usrMultiTextItem.Remove(txtBox);
				
				switch(_inputType)
				{
					case enInputType.LABEL:
						txtBox.Visible = true;
						lblText.Visible = true;
						lblText.BringToFront();
						lblText.Text = txtBox.Text;
						break;

					case enInputType.COMBO:						
						cmbBox.Visible = true;						
						break;

					case enInputType.TEXTBOX:						
						txtBox.Visible = true;						
						break;

					case enInputType.MultiLineTEXTBOX:
						txtBox.Visible = true;
						usrMultiTextItem.Set(txtBox);
						break;

				}
			}
		}


		//bool _multiLineTEXTBOX_isUseMultiTextItem = false;

		///// <summary>
		///// 
		///// </summary>
		//public bool MultiLineTEXTBOX_isUseMultiTextItem
		//{
		//	get { return _multiLineTEXTBOX_isUseMultiTextItem; }
		//	set
		//	{
		//		_multiLineTEXTBOX_isUseMultiTextItem = value;

		//		if (_inputType != enInputType.MultiLineTEXTBOX) return;

		//		if(_multiLineTEXTBOX_isUseMultiTextItem)
		//			usrMultiTextItem.Set(txtBox);
		//		else
		//			usrMultiTextItem.Remove(txtBox);

		//	}
		//}


		/// <summary>
		/// ComboBox의 드롭다운 부분이 표시될 때 발생합니다.
		/// </summary>
		public event EventHandler ComboBoxDropDown
		{
			add
			{
				cmbBox.DropDown += value;
			}
			remove
			{
				cmbBox.DropDown -= value;
			}
		}

		public ComboBoxStyle ComboBoxDropDownStyle
		{
			get { return cmbBox.DropDownStyle; }
			set
			{
				cmbBox.DropDownStyle = value; 
			}
		}


		public object ComboBoxDataSource
		{
			get { return cmbBox.DataSource; }
			set
			{
				cmbBox.DataSource = value;
			}
		}

		public string ComboBoxDisplayMember
		{
			get { return cmbBox.DisplayMember; }
			set
			{
				cmbBox.DisplayMember = value;
			}
		}


		public object ComboBoxSelectedValue
		{
			get { return cmbBox.SelectedValue; }
			set
			{
				cmbBox.SelectedValue = value;
			}
		}


		public object ComboBoxSelectItem
		{
			get { return cmbBox.SelectedItem; }
			set { cmbBox.SelectedItem = value; }
		}

		public int ComboBoxSelectIndex
		{
			get { return cmbBox.SelectedIndex; }
			set { cmbBox.SelectedIndex = value; }
		}

		/// <summary>
		/// inputtype에 해당하는 DataBindings을 리턴한다.
		/// </summary>
		public new ControlBindingsCollection DataBindings
		{
			get
			{
				ControlBindingsCollection rtn = null;

				switch (_inputType)
				{
					case enInputType.LABEL:
						//label의 DataBindings 이용시 값이 변경 되어도 datasource에 적용 되지 않으므로 textbox의 값을 이용한다.
						//rtn = lblText.DataBindings;

						rtn = txtBox.DataBindings;
						break;

					case enInputType.COMBO:
						rtn = cmbBox.DataBindings;
						break;

					case enInputType.MultiLineTEXTBOX:
					case enInputType.TEXTBOX:
						rtn = txtBox.DataBindings;
						break;
				}

				return rtn;
			}
			//set { txtBox.DataBindings = value; }
		}


		//[SRDescription("ComboBoxItemsDescr")]
		[MergableProperty(false)]
		[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(true)]
		//[SRCategory("CatData")]
		public ComboBox.ObjectCollection ComboBoxItems
		{
			get { return cmbBox.Items; }
			set 
			{ 
				cmbBox.Items.Clear();

				foreach (object obj in value)
				{
					cmbBox.Items.Add(obj);
				}
			}
		}

		public string ComboBoxValueMember
		{
			get { return cmbBox.ValueMember; }
			set
			{
				cmbBox.ValueMember = value;
			}
		}

		private enTextType _textType = enTextType.All;

		/// <summary>
		/// textBox에서 입력가능한 타입설정
		/// </summary>
		public enTextType TextType
		{
			get { return _textType; }
			set
			{
				if (value == _textType) return;

				_textType = value;

				switch (value)
				{
					case enTextType.NumberOlny:
						controlEvent.TextBox_Press_NumberOnly(txtBox);
						break;

					case enTextType.None:
						controlEvent.TextBox_Press_None(txtBox);
						break;

					default:
						controlEvent.TextBox_Press_NumberOnly_Remove(txtBox);
						controlEvent.TextBox_Press_None_Remove(txtBox);
						break;

				}

			}
		}


		/// <summary>
		/// 텍스트 변경 여부
		/// </summary>
		public bool isChange = false;


		public usrInputBox()
		{
			InitializeComponent();

			//cur = txtBox;
			//_width = this.Width;
			_labelwidth = splitContainer1.SplitterDistance;


			foreach (Control c in splitContainer1.Panel2.Controls)
			{
				c.Click += new EventHandler(c_Click);
				c.DoubleClick += new EventHandler(c_DoubleClick);
				c.KeyDown += new KeyEventHandler(usrInputBox_KeyDown);
				c.GotFocus += new EventHandler(c_GotFocus);
				c.LostFocus += new EventHandler(c_LostFocus);
			}

			base.Click += new EventHandler(c_Click);
			base.DoubleClick += new EventHandler(c_DoubleClick);
			//base.KeyDown += new KeyEventHandler(usrInputBox_KeyDown);

			lblText.TextChanged += new EventHandler(lblText_TextChanged);

			labelwidth_resize();

			txtBox.Tag = this;

		}


		void lblText_TextChanged(object sender, EventArgs e)
		{

			if (!_dLabel_FontAutoSize) return;

			lblText.Font = this.Font;

			//폰트자동크기 조절.. 폭이 넘어갈경우 폰트 사이즈를 조절 하여 준다.
			float f = Function.form.control.Font_Control_String_Size_Get(lblText, control.enControl_Criteria.width, lblText.Text);

			if ((f * 1.1f) <= lblText.Width) return;

			lblText.Font = Function.form.control.Font_Control_Resize_Get(lblText, control.enControl_Criteria.width, lblText.Text, 0.9f);


		}

		void c_GotFocus(object sender, EventArgs e)
		{
			Control c = sender as Control;
			_focused = true;
			//Console.WriteLine("InputBox Got Focus:{0}", c.Name);
		}

		void c_LostFocus(object sender, EventArgs e)
		{			
			_focused = false;
			//Console.WriteLine("InputBox Lost Focus:{0}", c.Name);
		}

		protected void c_Click(object sender, EventArgs e)
		{
			usrEventArgs a = new usrEventArgs();
			a.EventKind = enEventKind.CLICK;
			
			if(Click != null) this.Click(this, a);
		}

		void c_DoubleClick(object sender, EventArgs e)
		{
			usrEventArgs a = new usrEventArgs();
			a.EventKind = enEventKind.DOUBLECLICK;

			if (DoubleClick != null) this.DoubleClick(this, a);
		}

		private void Check_TextChanged()
		{

			if (Fnc.obj2String(TEXT).Equals(Value))
			{
				label1.Text = string.Empty;
				isChange = false;
			}
			else
			{
				label1.Text = "*";
				isChange = true;
			}

			usrEventArgs a = new usrEventArgs();
			a.EventKind = enEventKind.TEXT_CHANGED;
					

			if (_text_Changed != null) _text_Changed(this, a);
		}

		private void txtBox_TextChanged(object sender, EventArgs e)
		{
			//label databindings이용 대응 처리
			if (!_isSetLabelValue &&_inputType == enInputType.LABEL)
			{
				lblText.Text = txtBox.Text;
				Console.WriteLine("txtBox_TextChanged");
			}
			else if(_isSetLabelValue)
			{
				txtBox.Text = lblText.Text;
			}

			Check_TextChanged();
		}

		private void cmbBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			Check_TextChanged();
		}

		private void usrInputBox_SizeChanged(object sender, EventArgs e)
		{
			if (isSizeChanging) return;

			int y = -1;
			try
			{
				isSizeChanging = true;

				switch (_inputType)
				{
					case enInputType.COMBO:
						y = cmbBox.Height;
						break;

					case enInputType.TEXTBOX:
						y = txtBox.Height;
						break;

					//case enInputType.LABEL:
					//	y = lblText.Height;
					//	break;
				}

				if (splitContainer1.Orientation == System.Windows.Forms.Orientation.Horizontal && Label_Visable)
				{
					y = y + label.Height + 4;
					this.Height = y;
					splitContainer1.SplitterDistance = label.Height;
				}
				else
				{

					if (y < 0) return;
					this.Height = y + 2;
				}

				System.Threading.Thread.Sleep(1);
				Application.DoEvents();
				
			}
			catch { }
			finally
			{
				isSizeChanging = false;
			}


			
			
		}

		public void Init()
		{
			Value = string.Empty;
			
//			if(InputType == enInputType.ComboBox
			//if (cmbBox.DataSource != null) cmbBox.DataSource = null;
			if (cmbBox.Items.Count > 0) cmbBox.SelectedIndex = -1;

		}

		private void usrInputBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				TEXT = Value;
			}

			if (keydown != null) keydown(this, e);
			
		}

		private void label_Click(object sender, EventArgs e)
		{
			//Console.WriteLine("[Name]{0}[Panel1Width]{1}", Label_Text, splitContainer1.SplitterDistance);

			//LabelWidth = -1;
		}

		private void usrInputBox_Load(object sender, EventArgs e)
		{
			labelwidth_resize();
		}

		private void lblText_Click(object sender, EventArgs e)
		{
			this.Focus();
		}

	}
}
