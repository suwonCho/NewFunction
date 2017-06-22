using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PLCModule
{
	public partial class frmPLCValueChange : Form
	{
		bool _isbit_click = false;
		string _add = string.Empty;
		clsPLCModule _plc;
		System.Int16 value = 0;
		System.Int16 value_new = 0;

		Form _basefrom;
		
		public frmPLCValueChange(Form baseform, clsPLCModule plc, string add)
		{
			InitializeComponent();

			_basefrom = baseform;

			_plc = plc;			
			_add = add;

			txtValue.Minimum = System.Int16.MinValue;
			txtValue.Maximum = System.Int16.MaxValue; 

		}

		void dtAddress_ColumnChanged(object sender, DataColumnChangeEventArgs e)
		{
			DataRow [] dr = _plc.dtAddress.Select(string.Format("Address = '{0}'", _add));

			if (dr.Length < 1)
			{
				this.Close();
				return;
			}

			control_text(lblAdd, dr[0]["address"]);
			if (dr[0]["Value"] == DBNull.Value)
			{
				_isbit_click = false;
				value = 0;
				control_text(lblValue, "(null)");
				control_text(lblHex, "(null)");
				control_enable(txtHex, false);
				control_enable(txtValue, false);
				
			}
			else
			{
				control_enable(txtHex, true);
				control_enable(txtValue, true);
				control_text(lblValue, dr[0]["Value"]);
				value = System.Int16.Parse(dr[0]["Value"].ToString());
				control_text(lblHex, dr[0]["Value(Hex)"]);
			}

			if (!_isbit_click) set_bit(value);

		}

		private void set_bit(int vv)
		{
			Control c;

			for (int i = 0; i < 16; i++)
			{
				c = this.Controls[string.Format("lbit{0}", i)];

				int v = 1 << i; //int.Parse(Math.Pow(2, i).ToString());

				if ((v & vv) != 0)
					if(_isbit_click)
						control_backcolor(c, Color.LightSalmon);
					else
						control_backcolor(c, Color.LightSeaGreen);				

				else
					if (_isbit_click)
						control_backcolor(c, Color.DarkGray);
					else
						control_backcolor(c, Color.LightGray);

			}
		}




		delegate void delcontrol_Text(Control ctl, object text);

		private void control_text(Control ctl, object text)
		{
			if (ctl.InvokeRequired)
			{
				ctl.BeginInvoke(new delcontrol_Text(control_text), new object[] { ctl, text });
				return;
			}

			if (text == null)
				ctl.Text = "(null)";
			else
				ctl.Text = text.ToString();

		}

		delegate void delcontrol_backcolor(Control ctl, Color c);

		private void control_backcolor(Control ctl, Color c)
		{
			if (ctl == null) return;

			if (ctl.InvokeRequired)
			{
				ctl.BeginInvoke(new delcontrol_backcolor(control_backcolor), new object[] { ctl, c });
				return;
			}

			ctl.BackColor = c;
		}


		delegate void delcontrol_enable(Control ctl, bool enable);

		private void control_enable(Control ctl, bool enable)
		{
			if (ctl == null) return;

			if (ctl.InvokeRequired)
			{
				ctl.BeginInvoke(new delcontrol_enable(control_enable), new object[] { ctl, enable });
				return;
			}

			ctl.Enabled = enable;
		}
		
		

		private void lbit_Click(object sender, EventArgs e)
		{
			if (!txtValue.Enabled) return;

			Control ctl = sender as Control;

			if (ctl == null) return;

			try
			{
				System.Int16 idx = System.Int16.Parse(ctl.Name.Substring(4));

				System.Int16 v = _isbit_click ? value_new : value;

				System.Int16 i = Convert.ToInt16(string.Format("{0:X4}", 1 << idx), 16);

				value_new = Convert.ToInt16(v ^ i);	//toggle

				input_value();

			}
			catch
			{
			}

		}

		private bool isInput = false;
		private void input_value()
		{
			isInput = true;

			try
			{
				_isbit_click = true;

				set_bit(value_new);

				txtValue.Value = value_new;
				txtHex.Text = string.Format("{0:X4}", value_new);
			}
			catch
			{
			}
			finally
			{
				System.Threading.Thread.Sleep(100);
				isInput = false;
			}
		}

		private void frmPLCValueChange_Load(object sender, EventArgs e)
		{
			dtAddress_ColumnChanged(null, null);
			_plc.dtAddress.ColumnChanged += new DataColumnChangeEventHandler(dtAddress_ColumnChanged);

			if(_basefrom != null)
			{
				Top = _basefrom.Top + _basefrom.Height + 5;
				Left = _basefrom.Left;
			}

			txtHex.SelectAll();

		}

		private void button1_Click(object sender, EventArgs e)
		{
			_isbit_click = false;
			isInput = true;

			try
			{
				txtHex.Text = "0000";
				txtValue.Value = 0;

				dtAddress_ColumnChanged(null, null);
			}
			catch
			{

			}
			finally
			{
				System.Threading.Thread.Sleep(100);
				isInput = false;
			}
		}

		private void txtValue_ValueChanged(object sender, EventArgs e)
		{
			if (isInput) return;

			value_new = System.Int16.Parse(txtValue.Value.ToString());
			input_value();			
		}

		

		private void btnOk_Click(object sender, EventArgs e)
		{
			if (!txtValue.Enabled) return;
			if (!_isbit_click) return;

			_plc.WriteOrder(_add, value_new);

			button1_Click(null, null);
		}

		private void txtHex_Changed()
		{
			value_new = Convert.ToInt16(txtHex.Text, 16);
			input_value();
		}

		private void txtHex_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				if (!_isbit_click)
				{
					txtHex_Changed();
					txtHex.SelectAll();
				}
				else
				{
					txtHex_Changed();
					btnOk_Click(null, null);
					txtHex.SelectAll();
				}
			}
		}

		private void txtHex_Leave(object sender, EventArgs e)
		{			
			txtHex_Changed();
		}

		private void frmPLCValueChange_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape) this.Close();
		}




	}
}
