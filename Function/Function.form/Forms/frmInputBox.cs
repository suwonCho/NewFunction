using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Function.form
{
	public partial class frmInputBox : Form
	{
		public object Value
		{
			get; set;
		}

		public frmInputBox()
		{
			init(false, false, string.Empty);
		}

		public frmInputBox(bool isNumberOnly, bool isMultiLine, object default_value)
		{
			init(isNumberOnly, isMultiLine, default_value);
		}

		private void init(bool isNumberOnly, bool isMultiLine, object default_value)
		{
			InitializeComponent();

			if (isNumberOnly)
			{
				controlEvent.TextBox_Press_NumberOnly(textBox1);
				isMultiLine = false;
			}

			textBox1.Multiline = isMultiLine;

			textBox1.Text = Fnc.obj2String(default_value);

			if (isMultiLine)
			{
				float height = control.Font_Control_String_Size_Get(textBox1, control.enControl_Criteria.width, textBox1.Text);
				this.Height = Fnc.obj2int(height) + 67 + 1;
			}
			else
			{
				this.Height = textBox1.Height + 67;
			}

			if (this.Height < 88) this.Height = 89;

			textBox1.SelectAll();
			this.BringToFront();
			textBox1.Focus();
		}

		private void textBox1_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Escape:
					btnCancel_Click(null, null);
					break;

				case Keys.Enter:
					if (!textBox1.Multiline) btnSave_Click(null, null);
					break;
			}

		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			Value = textBox1.Text;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Close();
		}

		

	}
}
