using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Function.Util;
namespace Fnc_Utility
{
    public partial class Frmcryptography : Form
    {
		private cryptography cryp = new cryptography();

        public Frmcryptography()
        {
            InitializeComponent();
        }

		private void setKey()
		{
			
			cryp.Key = txtEnKey.Text;
			cryp.IV = txtEnIV.Text;
			
		}

		private void btnEcKeyGen_Click(object sender, EventArgs e)
		{
			cryp.GenKey();

			txtEnKey.Text = cryp.Key;
		}

		private void btnEnIVGen_Click(object sender, EventArgs e)
		{
			cryp.GenIV();
			txtEnIV.Text = cryp.IV;
		}

		private void btnEnc_Click(object sender, EventArgs e)
		{
			setKey();
			txtEnResult.Text = cryp.Encrypting(txtEnSource.Text.Trim());
		}

		private void btnDec_Click(object sender, EventArgs e)
		{
			setKey();
			txtEnResult.Text = cryp.Decrypting(txtEnSource.Text.Trim());
		}


		
    }
}
