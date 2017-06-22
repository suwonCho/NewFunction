using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime;
using System.Runtime.InteropServices;

namespace Function.form
{
	public partial class usrInputDatePicker : usrInputBox
	{
		
		public DateTimePicker _dtp = new DateTimePicker();
		public DateTimePicker dtp
		{
			get { return _dtp; }
			set { _dtp = value; }
		}

		private event usrEventHander _text_Changed = null;

		private enInputType _inputType = enInputType.DateTimePicker;
		public new enInputType InputType
		{
			get { return _inputType; }
		}

		/// <summary>
		/// 텍스트가 변경 되었을경우 발행 하는 이벤트
		/// </summary>
		public new event usrEventHander Text_Changed
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

		/// <summary>
		/// 표시되는 날짜 및 시간 서식을 가져오거나 설정합니다.
		/// </summary>
		public DateTimePickerFormat Format
		{
			get { return dtp.Format; }
			set { dtp.Format = value; }
		}

		/// <summary>
		/// 사용자가 설정한 날짜 및 시간 서식을 가져오거나 설정합니다.
		/// </summary>
		public string CustomFormat
		{
			get { return dtp.CustomFormat; }
			set
			{
				dtp.CustomFormat = value;
			}
		}


		public usrInputDatePicker() : base()
		{	

			InitializeComponent();

			foreach (Control c in new Control[] { txtBox, cmbBox, lblText })
			{
				splitContainer1.Panel2.Controls.Remove(c);
				c.Dispose();
			}

			dtp.Click += new EventHandler(c_Click);
			
			
			splitContainer1.Panel2.Controls.Add(dtp);
			dtp.Dock = DockStyle.Fill;
			Value =  DateTime.Now;
			dtp.BringToFront();
		}

		public new enum enInputType
		{
			DateTimePicker
		};

		private new ContentAlignment TextAlign
		{
			get { return ContentAlignment.MiddleRight; }
		}


		private DateTime _value;

		//[SRDescription("DateTimePickerValueDescr")]
		[Bindable(true)]
		[RefreshProperties(RefreshProperties.All)]		
		public new DateTime Value
		{
			get { return _value; }
			set
			{
				TEXT = _value = value;
			}
		}

		//[SRDescription("DateTimePickerValueDescr")]
		[Bindable(true)]
		[RefreshProperties(RefreshProperties.All)]
		//[SRCategory("CatBehavior")]
		public new DateTime TEXT
		{
			get { return dtp.Value; }
			set
			{
				dtp.Value = value;
				Check_TextChanged();
			}
		}


		private void Check_TextChanged()
		{
			if (TEXT.Equals(Value))
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

		private void usrInputBox_SizeChanged(object sender, EventArgs e)
		{
			int y = dtp.Height;
			
			if (y < 0) return;
			this.Height = y + 2;
		}

		

	}
}
