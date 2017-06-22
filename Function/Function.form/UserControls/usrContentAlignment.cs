using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Function.form.UserControls
{

	public delegate void delContentAlignment_Changed(object sender, ContentAlignment Alignment);

	public partial class usrContentAlignment : UserControl
	{
		ContentAlignment _align = ContentAlignment.MiddleCenter;

		public ContentAlignment Alignment
		{
			get { return _align; }
			set
			{
				foreach(Button b in btns)
				{
					if(b.Name.Equals(value.ToString()))
					{
						b.BackColor = BackColor_Selected;
					}
					else
					{
						b.BackColor = BackColor;
					}

					b.ForeColor = ForeColor;
				}


				if (value == _align) return;

				_align = value;

				if (_onContentAlignment_Changed != null) _onContentAlignment_Changed(this, _align);

			}
		}


		event delContentAlignment_Changed _onContentAlignment_Changed;

		public event delContentAlignment_Changed OnContentAlignment_Changed
		{
			add
			{
				_onContentAlignment_Changed += value;
			}

			remove
			{
				_onContentAlignment_Changed -= value;
			}
		}


		Color _backColor = SystemColors.Control;

		public new Color BackColor
		{
			get { return _backColor; }
			set
			{
				_backColor = value;
				Alignment = Alignment;
			}
		}


		Color _backColor_Selected = Color.LightSkyBlue;

		public Color BackColor_Selected
		{
			get { return _backColor_Selected; }
			set
			{
				_backColor_Selected = value;
				Alignment = Alignment;
			}
		}



		Color _foreColor = SystemColors.ControlText;

		public new Color ForeColor
		{
			get { return _foreColor; }
			set
			{
				_foreColor = value;
				Alignment = Alignment;
			}
		}
		

		Button[] btns;

		public usrContentAlignment()
		{
			InitializeComponent();

			btns = new Button[] { TopLeft, TopCenter, TopRight, MiddleLeft, MiddleCenter, MiddleRight, BottomLeft, BottomCenter, BottomRight  };
		}

		private void usrContentAlignment_Load(object sender, EventArgs e)
		{
			usrContentAlignment_SizeChanged(null, null);
			Alignment = Alignment;
		}

		private void usrContentAlignment_SizeChanged(object sender, EventArgs e)
		{
			int idx = 0;

			int w = this.Width / 3;
			int h = this.Height / 3;

			int t = 0;
			int l = 0;

			Button btn;

			Font fnt = null;

			for(int y = 0; y < 3;y++)
			{
				l = 0;
				for(int x =0;x<3;x++)
				{
					btn = btns[idx];

					btn.Left = l;
					btn.Top = t;

					if (x < 2)
						btn.Width = w;
					else
						btn.Width = this.Width - l;

					if (y < 2)
						btn.Height = h;
					else
						btn.Height = this.Height - t;
					
					//폰트크기변경
					if(idx == 0)
					{						
						Font fw = Function.form.control.Font_Control_Resize_Get(btn, control.enControl_Criteria.width, btn.Text, 0.6f);
						Font fh = Function.form.control.Font_Control_Resize_Get(btn, control.enControl_Criteria.height, btn.Text, 0.6f);

						fnt = fw.Size > fh.Size ? fh : fw;
					}

					btn.Font = fnt;


					l += w;
					idx++;
				}

				t += h;
			}


		}

		private void TopLeft_Click(object sender, EventArgs e)
		{
			Button btn = sender as Button;

			Alignment = (ContentAlignment)Fnc.String2Enum(new ContentAlignment(), btn.Name);
		}
	}
}
