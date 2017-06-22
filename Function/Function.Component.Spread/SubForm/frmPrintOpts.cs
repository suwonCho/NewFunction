using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Function.Component
{
	/// <summary>
	/// Summary description for PrintOpts.
	/// </summary>
	class frmPrintOpts : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.RadioButton radioButton3;
		private System.Windows.Forms.RadioButton radioButton4;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.CheckBox checkBox3;
		private System.Windows.Forms.CheckBox checkBox4;
		private System.Windows.Forms.CheckBox checkBox5;
		private System.Windows.Forms.CheckBox checkBox6;
		private System.Windows.Forms.CheckBox checkBox7;
		private System.Windows.Forms.CheckBox checkBox8;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;

		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        FarPoint.Win.Spread.FpSpread fp;
        FarPoint.Win.Spread.PrintInfo pi;

		public frmPrintOpts(FarPoint.Win.Spread.FpSpread _fp)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            fp = _fp;
            pi = fp.ActiveSheet.PrintInfo;
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		//public SpreadWinDemo.Printing printingfrm;
		//private SpreadWinDemo.PageSetup pagesetup  = new PageSetup();
	
	    /*
		public void StartPage(Printing printform)

	{
		printingfrm = printform;
        this.Show();
        this.BringToFront();
	}
         */

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.radioButton4 = new System.Windows.Forms.RadioButton();
			this.radioButton3 = new System.Windows.Forms.RadioButton();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.checkBox8 = new System.Windows.Forms.CheckBox();
			this.checkBox7 = new System.Windows.Forms.CheckBox();
			this.checkBox6 = new System.Windows.Forms.CheckBox();
			this.checkBox5 = new System.Windows.Forms.CheckBox();
			this.checkBox4 = new System.Windows.Forms.CheckBox();
			this.checkBox3 = new System.Windows.Forms.CheckBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.textBox2);
			this.groupBox1.Controls.Add(this.textBox1);
			this.groupBox1.Controls.Add(this.radioButton4);
			this.groupBox1.Controls.Add(this.radioButton3);
			this.groupBox1.Controls.Add(this.radioButton2);
			this.groupBox1.Controls.Add(this.radioButton1);
			this.groupBox1.Location = new System.Drawing.Point(10, 26);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(220, 172);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "¿Œº‚ π¸¿ß";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(143, 149);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(29, 18);
			this.label1.TabIndex = 6;
			this.label1.Text = "to";
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(173, 146);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(38, 21);
			this.textBox2.TabIndex = 5;
			this.textBox2.Text = "1";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(91, 146);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(39, 21);
			this.textBox1.TabIndex = 4;
			this.textBox1.Text = "1";
			// 
			// radioButton4
			// 
			this.radioButton4.Location = new System.Drawing.Point(10, 146);
			this.radioButton4.Name = "radioButton4";
			this.radioButton4.Size = new System.Drawing.Size(120, 18);
			this.radioButton4.TabIndex = 3;
			this.radioButton4.Text = "∆‰¿Ã¡ˆπ¸¿ß";
			// 
			// radioButton3
			// 
			this.radioButton3.Location = new System.Drawing.Point(10, 103);
			this.radioButton3.Name = "radioButton3";
			this.radioButton3.Size = new System.Drawing.Size(201, 26);
			this.radioButton3.TabIndex = 2;
			this.radioButton3.Text = "«ˆ¿Á ∆‰¿Ã¡ˆ ¿Œº‚";
			// 
			// radioButton2
			// 
			this.radioButton2.Location = new System.Drawing.Point(10, 69);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(201, 17);
			this.radioButton2.TabIndex = 1;
			this.radioButton2.Text = "º±≈√µ» ºø∏∏ ¿Œº‚";
			// 
			// radioButton1
			// 
			this.radioButton1.Location = new System.Drawing.Point(10, 34);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(171, 18);
			this.radioButton1.TabIndex = 0;
			this.radioButton1.Text = "All(¿¸√º ∆‰¿Ã¡ˆ)";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.checkBox8);
			this.groupBox2.Controls.Add(this.checkBox7);
			this.groupBox2.Controls.Add(this.checkBox6);
			this.groupBox2.Controls.Add(this.checkBox5);
			this.groupBox2.Controls.Add(this.checkBox4);
			this.groupBox2.Controls.Add(this.checkBox3);
			this.groupBox2.Controls.Add(this.checkBox2);
			this.groupBox2.Controls.Add(this.checkBox1);
			this.groupBox2.Location = new System.Drawing.Point(240, 26);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(336, 172);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Print Options";
			// 
			// checkBox8
			// 
			this.checkBox8.Location = new System.Drawing.Point(173, 129);
			this.checkBox8.Name = "checkBox8";
			this.checkBox8.Size = new System.Drawing.Size(144, 26);
			this.checkBox8.TabIndex = 7;
			this.checkBox8.Text = "Best Fit Rows/Cols";
			// 
			// checkBox7
			// 
			this.checkBox7.Location = new System.Drawing.Point(173, 95);
			this.checkBox7.Name = "checkBox7";
			this.checkBox7.Size = new System.Drawing.Size(144, 26);
			this.checkBox7.TabIndex = 6;
			this.checkBox7.Text = "¿‘∑¬µ» ºø∏∏ ¿Œº‚";
			// 
			// checkBox6
			// 
			this.checkBox6.Location = new System.Drawing.Point(173, 60);
			this.checkBox6.Name = "checkBox6";
			this.checkBox6.Size = new System.Drawing.Size(144, 26);
			this.checkBox6.TabIndex = 5;
			this.checkBox6.Text = "Color ¿Œº‚";
			// 
			// checkBox5
			// 
			this.checkBox5.Location = new System.Drawing.Point(173, 26);
			this.checkBox5.Name = "checkBox5";
			this.checkBox5.Size = new System.Drawing.Size(144, 26);
			this.checkBox5.TabIndex = 4;
			this.checkBox5.Text = "¿Ωøµ ¿Œº‚";
			// 
			// checkBox4
			// 
			this.checkBox4.Location = new System.Drawing.Point(19, 129);
			this.checkBox4.Name = "checkBox4";
			this.checkBox4.Size = new System.Drawing.Size(135, 26);
			this.checkBox4.TabIndex = 3;
			this.checkBox4.Text = "Border ¿Œº‚";
			// 
			// checkBox3
			// 
			this.checkBox3.Location = new System.Drawing.Point(19, 95);
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = new System.Drawing.Size(159, 26);
			this.checkBox3.TabIndex = 2;
			this.checkBox3.Text = "Grid Lines(±∏∫–º±)¿Œº‚";
			// 
			// checkBox2
			// 
			this.checkBox2.Location = new System.Drawing.Point(19, 60);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(135, 26);
			this.checkBox2.TabIndex = 1;
			this.checkBox2.Text = "Row Headers ¿Œº‚";
			this.checkBox2.Validating += new System.ComponentModel.CancelEventHandler(this.checkBox2_Validating);
			// 
			// checkBox1
			// 
			this.checkBox1.Location = new System.Drawing.Point(19, 26);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(148, 26);
			this.checkBox1.TabIndex = 0;
			this.checkBox1.Text = "Column Headers ¿Œº‚";
			this.checkBox1.Validating += new System.ComponentModel.CancelEventHandler(this.checkBox1_Validating);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(10, 215);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(144, 35);
			this.button2.TabIndex = 2;
			this.button2.Text = "¿Œ  º‚";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(221, 215);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(144, 35);
			this.button3.TabIndex = 3;
			this.button3.Text = "√Î   º“";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(432, 215);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(144, 35);
			this.button4.TabIndex = 4;
			this.button4.Text = "πÃ ∏Æ ∫∏ ±‚";
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// frmPrintOpts
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(593, 269);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "frmPrintOpts";
			this.Text = "¿Œº‚ ø…º«";
			this.Load += new System.EventHandler(this.PrintOpts_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

    }
		#endregion

		private void PrintOpts_Load(object sender, System.EventArgs e)
		{
			switch (pi.PrintType)
			{
				case (FarPoint.Win.Spread.PrintType.All):
					radioButton1.Checked = true;
					break;
				case (FarPoint.Win.Spread.PrintType.CellRange):
					radioButton2.Checked = true;
					break;
				case (FarPoint.Win.Spread.PrintType.CurrentPage):
					radioButton3.Checked = true;
					break;
				case (FarPoint.Win.Spread.PrintType.PageRange):
					radioButton4.Checked = true;
					break;
			}
			checkBox1.Checked = pi.ShowColumnHeaders;
            checkBox2.Checked = pi.ShowRowHeaders;
            checkBox3.Checked = pi.ShowGrid;
            checkBox4.Checked = pi.ShowBorder;
            checkBox5.Checked = pi.ShowShadows;
            checkBox6.Checked = pi.ShowColor;
            checkBox7.Checked = pi.UseMax;
            checkBox8.Checked = pi.BestFitCols;

			if (pi.PageStart != -1) 
			{
				textBox2.Text = pi.PageStart.ToString();
				textBox1.Text = pi.PageEnd.ToString();
			}
			else
			{
				textBox2.Text = "";
				textBox1.Text = "";
			}
			
		}

		public bool IsNumeric(object o)
		{
			if( o is short ||
				o is int ||
				o is long ||
				o is float ||
				o is double ||
				o is decimal )
				return true;
			if( o is string )
			{
				try
				{
					double d = double.Parse((string)o, System.Globalization.CultureInfo.InvariantCulture);
					return true;
				}
				catch( FormatException )
				{
				}
				catch( OverflowException )
				{
				}
			}
			return false;
		}


		private void button1_Click(object sender, System.EventArgs e)
		{
            /*
			if (pagesetup.IsDisposed)
			{
				pagesetup = new PageSetup();
			}
        pagesetup.Show();
        pagesetup.BringToFront();
             */

		}

		private void checkBox1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			radioButton4.Checked = true;
			if (textBox2.Text != "")
			{
				if (!IsNumeric(textBox1.Text))
				{
					if (!IsNumeric(textBox1.Text) | (Int32.Parse(textBox1.Text) < Int32.Parse(textBox2.Text)))
					MessageBox.Show("You must enter a number not less then the page start.");
					e.Cancel = true;
					
				}
			}
		}

		private void checkBox2_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			radioButton4.Checked = true;
			if (textBox1.Text != "")
			{
				if (!IsNumeric(textBox2.Text) | (Int32.Parse(textBox1.Text) < Int32.Parse(textBox2.Text)))
				{
					MessageBox.Show("You must enter a number not greater then the page end.");
					e.Cancel = true;
				}
			}
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			printsheet(false);
		}
		public void printsheet(bool print)
		{
			if (radioButton1.Checked == true)
			{
				pi.PrintType = FarPoint.Win.Spread.PrintType.All;
			}
			if (radioButton2.Checked == true)
			{
				pi.PrintType = FarPoint.Win.Spread.PrintType.CellRange;
			}
			if (radioButton3.Checked == true)
			{
				pi.PrintType = FarPoint.Win.Spread.PrintType.CurrentPage;
			}
			if (radioButton4.Checked == true)
			{
				pi.PrintType = FarPoint.Win.Spread.PrintType.PageRange;
				pi.PageStart = Int32.Parse((textBox2.Text));
				pi.PageEnd = Int32.Parse(textBox1.Text);
			}

			pi.UseSmartPrint = true;

			pi.ShowColumnHeaders = checkBox1.Checked;
            pi.ShowRowHeaders = checkBox2.Checked;
            pi.ShowGrid = checkBox3.Checked;
            pi.ShowBorder = checkBox4.Checked;
            pi.ShowShadows = checkBox5.Checked;
            pi.ShowColor = checkBox6.Checked;
            pi.UseMax = checkBox7.Checked;
            pi.BestFitCols = checkBox8.Checked;
            pi.Preview = print;

		
			//printingfrm.fpSpread1.Sheets[0].PrintInfo = pi;
            fp.PrintSheet(fp.ActiveSheet);

            this.Close();
		}

		private void button4_Click(object sender, System.EventArgs e)
		{
			 printsheet(true);
		}
	}
}
