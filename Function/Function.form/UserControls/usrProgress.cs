using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Function.form
{
	public partial class usrProgress : UserControl
	{
		public usrProgress()
		{
			InitializeComponent();
		}

		
		string _title = "타이틀";

		/// <summary>
		/// 타이틀 텍스트를 설정하거나 가져 옵니다.
		/// </summary>
		public string Title
		{
			get { return _title; }
			set
			{
				if (_title.Equals(value)) return;

				_title = value;

				Function.form.control.Invoke_Control_Text(lblTitle, _title);

				Application.DoEvents();
			}
		}

		string _detil = string.Empty;

		/// <summary>
		/// 세부항목 텍스트를 가져오거나 설정합니다.
		/// </summary>
		public string Detail
		{
			get { return _detil; }
			set
			{
				if (_detil.Equals(value)) return;

				_detil = value;

				Function.form.control.Invoke_Control_Text(lblDetail, _detil);

				Application.DoEvents();
			}
		}

		long _eachMaxValue = 0;

		/// <summary>
		/// 개별항목 진행상태 최대 값을 설정하거나 가져 옵니다.
		/// </summary>
		public long	EachMaxValue
		{
			get { return _eachMaxValue; }
			set
			{
				if (_eachMaxValue.Equals(value)) return;

				_eachMaxValue = value;

				eachValue_changed();
			}
		}



		long _eachValue = 0;

		/// <summary>
		/// 개별항목 진행상태 현재 값을 설정하거나 가져 옵니다.
		/// </summary>
		public long EachValue
		{
			get { return _eachValue; }
			set
			{
				if (_eachValue.Equals(value)) return;

				_eachValue = value > _eachMaxValue ? _eachMaxValue : value;

				eachValue_changed();
			}
		}


		long _allMaxValue = 0;

		/// <summary>
		/// 개별항목 진행상태 최대 값을 설정하거나 가져 옵니다.
		/// </summary>
		public long AllMaxValue
		{
			get { return _allMaxValue; }
			set
			{
				if (_allMaxValue.Equals(value)) return;

				_allMaxValue = value;

				allValue_changed();
			}
		}



		long _allValue = 0;

		/// <summary>
		/// 개별항목 진행상태 현재 값을 설정하거나 가져 옵니다.
		/// </summary>
		public long AllValue
		{
			get { return _allValue; }
			set
			{
				if (_allValue.Equals(value)) return;

				_allValue = value >_allMaxValue ? _allMaxValue : value;

				allValue_changed();
			}
		}

		private void eachValue_changed()
		{
			
			Function.form.control.Invoke_ProgressBar_Value(pgbEach, 1000, long2int(_eachValue,_eachMaxValue));

			string text;
			if (_eachMaxValue <= 0)
				text = "0%";
			else
				text = string.Format("{0:D}%", (_eachValue * 100) / _eachMaxValue );

			Function.form.control.Invoke_Control_Text(lblEachPer, text);

			text = string.Format("{0}/{1}", _eachValue.ToString("#,##0"), _eachMaxValue.ToString("#,##0"));

			Function.form.control.Invoke_Control_Text(lblEachCnt, text);

			Application.DoEvents();

		}


		private void allValue_changed()
		{
			Function.form.control.Invoke_ProgressBar_Value(pgbAll, 1000, long2int(_allValue, _allMaxValue));

			string text;
			if (_allMaxValue <= 0)
				text = "0%";
			else
				text = string.Format("{0:D}%", (_allValue * 100) / _allMaxValue);

			Function.form.control.Invoke_Control_Text(lblAllPer, text);

			text = string.Format("{0}/{1}", _allValue.ToString("#,##0"), _allMaxValue.ToString("#,##0"));

			Function.form.control.Invoke_Control_Text(lblAllCnt, text);

			Application.DoEvents();

		}

		private int long2int(long curr, long max)
		{
			if (max == 0) return 0;

			long value = (curr * 1000) / max;

			if (value > 1000) value = 1000;
						
			return int.Parse(value.ToString());
		}


		/// <summary>
		/// 취소 여부를 가져 옵니다.
		/// </summary>
		public bool isCancel
		{
			get;
			set;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			isCancel = true;
		}


		public void Show()
		{
			Control pt = this.Parent as Control;

			if (pt == null) return;

			foreach (Control c in pt.Controls)
			{
				if (c.Equals(this)) continue;

				Function.form.control.Invoke_Control_Enabled(c,false);
			}

			AllMaxValue = EachMaxValue = 100;				
			AllValue = EachValue = 0;
			isCancel = false;

			Function.form.control.Invoke_Control_BringToFront(this);

			Function.form.control.Invoke_Control_Top(this, (pt.Height / 2) - (this.Height / 2));
			Function.form.control.Invoke_Control_Left(this, (pt.Width / 2) - (this.Width / 2));

			Function.form.control.Invoke_Control_Visible(this, true);

			Application.DoEvents();
		}

		public void Hide()
		{
			Control pt = this.Parent as Control;

			if (pt == null) return;

			foreach (Control c in pt.Controls)
			{
				if (c.Equals(this)) continue;

				Function.form.control.Invoke_Control_Enabled(c, true);				
			}

			Function.form.control.Invoke_Control_Visible(this, false);
		}


	}
}
