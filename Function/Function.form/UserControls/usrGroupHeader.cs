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
	public partial class usrGroupHeader : UserControl
	{
		/// <summary>
		/// 텍스트에 맞춤을 가져 오거나 설정 합니다.
		/// </summary>
		public ContentAlignment TextAlign
		{
			get { return label1.TextAlign; }
			set { label1.TextAlign = value; }
		}

		/// <summary>
		/// 텍스트의 글꼴을 가져오거나 설정합니다.
		/// </summary>
		public new Font Font
		{
			get { return label1.Font; }
			set { label1.Font = value; } 
		}

		/// <summary>
		/// 텍스트의 글꼴섹을 가져오거나 설정합니다.
		/// </summary>
		public new Color ForeColor
		{
			get { return label1.ForeColor; }
			set { label1.ForeColor = value; }
		}

		/// <summary>
		/// 텍스트를  가져오거나 설정합니다.
		/// </summary>
		public override string Text
		{
			get { return label1.Text; }
			set { label1.Text = value; }
		}

		/// <summary>
		/// 텍스트를  가져오거나 설정합니다.
		/// </summary>
		public string TEXT
		{
			get { return label1.Text; }
			set { label1.Text = value; }
		}


		public usrGroupHeader()
		{
			InitializeComponent();
		}		

	}
}
