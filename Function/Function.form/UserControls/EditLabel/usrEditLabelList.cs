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
	public partial class usrEditLabelList : usrUserControlListBase
	{
		public usrEditLabelList()
		{
			InitializeComponent();
		}

		private void usrEditLabelList_Load(object sender, EventArgs e)
		{
			Function.form.formFunction.PanelMouseScroll_Set(this.ParentForm, pnlBody);
			DataSourceChanged += new EventHandler(usrEditLabelList_DataSourceChanged);
		}

		void usrEditLabelList_DataSourceChanged(object sender, EventArgs e)
		{

			ItemsClear(pnlBody);

			int idx = 0;

			if (DataSource == null && TextValues != null)
			{
				foreach (string val in TextValues)
				{
					usrEditLabel c = new usrEditLabel();

					c.Index = idx;
					c.Value = val;

					pnlBody.Controls.Add(c);

					idx++;

				}
			}
			else if (DataSource != null)
			{
			}
		}

		

	}
}
