using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SocketPLC
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnQ_Tag_Click(object sender, EventArgs e)
        {
            Q_TagPublish Q_Tag = new Q_TagPublish(txtQ_Tag.Text);
            Q_Tag.Show();
        }

		private void btnMelsec_A_Click(object sender, EventArgs e)
		{
			Melsec_A frm = new Melsec_A(txtMelsecA.Text);
			frm.Show();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			frmLSXGT frm = new frmLSXGT(txtLSXGT.Text);
			frm.Show();
		}
    }
}