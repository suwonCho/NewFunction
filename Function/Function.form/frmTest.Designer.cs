namespace Function.form
{
	partial class frmTest
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			Function.form.SimpleGridColumn simpleGridColumn1 = new Function.form.SimpleGridColumn();
			Function.form.SimpleGridColumn simpleGridColumn2 = new Function.form.SimpleGridColumn();
			Function.form.SimpleGridColumn simpleGridColumn3 = new Function.form.SimpleGridColumn();
			Function.form.SimpleGridColumn simpleGridColumn4 = new Function.form.SimpleGridColumn();
			Function.form.ControlViewInfo controlViewInfo1 = new Function.form.ControlViewInfo();
			Function.form.ControlViewInfo controlViewInfo2 = new Function.form.ControlViewInfo();
			this.button1 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.usrSimpleGrid1 = new Function.form.usrSimpleGrid();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(640, 252);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(129, 31);
			this.button1.TabIndex = 1;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(54, 228);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(163, 37);
			this.textBox1.TabIndex = 3;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(52, 271);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 12);
			this.label1.TabIndex = 4;
			this.label1.Text = "label1";
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(255, 238);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(117, 20);
			this.comboBox1.TabIndex = 5;
			// 
			// usrSimpleGrid1
			// 
			this.usrSimpleGrid1.Column_Count = 4;
			simpleGridColumn1.Column_Width = 25;
			simpleGridColumn1.Column_Width_Unit = Function.form.enUnit.Percent;
			simpleGridColumn1.ColumnName = "Column\r\ntest";
			simpleGridColumn1.DataFieldName = "";
			simpleGridColumn1.Grid_Alignment = System.Drawing.ContentAlignment.BottomCenter;
			simpleGridColumn1.Header_Alignment = System.Drawing.ContentAlignment.MiddleRight;
			simpleGridColumn1.Index = 0;
			simpleGridColumn2.Column_Width = 80;
			simpleGridColumn2.ColumnName = "Column1";
			simpleGridColumn2.DataFieldName = "";
			simpleGridColumn2.Grid_Alignment = System.Drawing.ContentAlignment.MiddleCenter;
			simpleGridColumn2.Header_Alignment = System.Drawing.ContentAlignment.MiddleCenter;
			simpleGridColumn2.Index = 1;
			simpleGridColumn3.Column_Width = 100;
			simpleGridColumn3.ColumnName = "Column3";
			simpleGridColumn3.DataFieldName = "";
			simpleGridColumn3.Grid_Alignment = System.Drawing.ContentAlignment.TopLeft;
			simpleGridColumn3.Header_Alignment = System.Drawing.ContentAlignment.BottomRight;
			simpleGridColumn3.Index = 2;
			simpleGridColumn4.Column_Width = 258;
			simpleGridColumn4.ColumnName = "Column3";
			simpleGridColumn4.DataFieldName = "";
			simpleGridColumn4.Grid_Alignment = System.Drawing.ContentAlignment.MiddleCenter;
			simpleGridColumn4.Header_Alignment = System.Drawing.ContentAlignment.MiddleCenter;
			simpleGridColumn4.Index = 3;
			this.usrSimpleGrid1.Columns = new Function.form.SimpleGridColumn[] {
        simpleGridColumn1,
        simpleGridColumn2,
        simpleGridColumn3,
        simpleGridColumn4};
			this.usrSimpleGrid1.Grid_Header_Height = 50;
			controlViewInfo1.BackColor = System.Drawing.SystemColors.ActiveCaption;
			controlViewInfo1.Font = new System.Drawing.Font("굴림", 12F);
			controlViewInfo1.ForeColor = System.Drawing.Color.Black;
			this.usrSimpleGrid1.Grid_Header_View = controlViewInfo1;
			this.usrSimpleGrid1.Grid_Line_Color = System.Drawing.Color.Black;
			this.usrSimpleGrid1.Grid_Line_Width = 4;
			controlViewInfo2.BackColor = System.Drawing.Color.Transparent;
			controlViewInfo2.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			controlViewInfo2.ForeColor = System.Drawing.Color.DimGray;
			this.usrSimpleGrid1.Grid_View = controlViewInfo2;
			this.usrSimpleGrid1.Location = new System.Drawing.Point(26, 12);
			this.usrSimpleGrid1.Name = "usrSimpleGrid1";
			this.usrSimpleGrid1.Rows_Count = 1;
			this.usrSimpleGrid1.Size = new System.Drawing.Size(583, 186);
			this.usrSimpleGrid1.TabIndex = 2;
			this.usrSimpleGrid1.Values = "123,456,123,789,456\r\n";
			// 
			// frmTest
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(787, 309);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.usrSimpleGrid1);
			this.Controls.Add(this.button1);
			this.Name = "frmTest";
			this.Text = "frmTest";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		
		private System.Windows.Forms.Button button1;
		private usrSimpleGrid usrSimpleGrid1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBox1;
	}
}