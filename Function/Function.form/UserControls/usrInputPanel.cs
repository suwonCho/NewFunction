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
	public partial class usrInputPanel : Panel
	{
		internal static readonly int _padding = 10;

		Dictionary<int, stCtrls> _dicCtrl = new Dictionary<int, stCtrls>();

		class stCtrls
		{
			public int Row;
			public Panel pnl;
			public bool RowBreaking;

			int _width;			

			public stCtrls(int row)
			{
				Row = row;				
				pnl = new Panel();
				pnl.Height = 0;
				RowBreaking = false;
				_width = 5;
			}
			
			public void Add(Control ctrl, bool rowbreaking)
			{
				pnl.Controls.Add(ctrl);
				ctrl.Left = _width;
				_width = _width + ctrl.Width + usrInputPanel._padding;

				if (pnl.Height < ctrl.Height + 4)
				{
					pnl.Height = ctrl.Height + 4;
					ctrl.Top = 2;
				}
				else
				{
					ctrl.Top = (pnl.Height - ctrl.Height) / 2;
				}


				RowBreaking = rowbreaking;
			}
		}

		public void Controls_Add(Control ctrl, bool rowbreaking)
		{
			int crow = _dicCtrl.Count - 1;
			stCtrls pnl = null;

			if (crow > -1)
			{
				pnl = _dicCtrl[crow];
			}

			if (pnl == null || pnl.RowBreaking)
			{
				crow++;
				pnl = new stCtrls(crow);
				this.Controls.Add(pnl.pnl);
				pnl.pnl.Dock = DockStyle.Top;
				pnl.pnl.BringToFront();

				_dicCtrl.Add(crow, pnl);
			}

			pnl.Add(ctrl, rowbreaking);
		}

		public usrInputPanel()
		{
			//this.BackColor = Color.Black;
		}

		

	}
}
