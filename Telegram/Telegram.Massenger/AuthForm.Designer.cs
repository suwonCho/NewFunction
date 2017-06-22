namespace Telegram.Massenger
{
	partial class AuthForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuthForm));
			this.btnReqNumber = new System.Windows.Forms.Button();
			this.usrNumberInput = new Function.form.usrNumberInput2();
			this.SuspendLayout();
			// 
			// btnReqNumber
			// 
			this.btnReqNumber.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnReqNumber.Font = new System.Drawing.Font("굴림체", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnReqNumber.Location = new System.Drawing.Point(0, 0);
			this.btnReqNumber.Name = "btnReqNumber";
			this.btnReqNumber.Size = new System.Drawing.Size(503, 35);
			this.btnReqNumber.TabIndex = 0;
			this.btnReqNumber.Text = "인증 번호 요청";
			this.btnReqNumber.UseVisualStyleBackColor = true;
			this.btnReqNumber.Click += new System.EventHandler(this.btnReqNumber_Click);
			// 
			// usrNumberInput
			// 
			this.usrNumberInput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.usrNumberInput.Location = new System.Drawing.Point(0, 35);
			this.usrNumberInput.Name = "usrNumberInput";
			this.usrNumberInput.NumberLength = 5;
			this.usrNumberInput.ShowNumber = false;
			this.usrNumberInput.Size = new System.Drawing.Size(503, 205);
			this.usrNumberInput.TabIndex = 1;
			this.usrNumberInput.OK_Click += new System.EventHandler(this.usrNumberInput_OK_Click);
			this.usrNumberInput.Cancel_Click += new System.EventHandler(this.usrNumberInput_Cancel_Click);
			// 
			// AuthForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(503, 262);
			this.Controls.Add(this.usrNumberInput);
			this.Controls.Add(this.btnReqNumber);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "AuthForm";
			this.Text = "Telegram 인증";
			this.Controls.SetChildIndex(this.btnReqNumber, 0);
			this.Controls.SetChildIndex(this.usrNumberInput, 0);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnReqNumber;
		private Function.form.usrNumberInput2 usrNumberInput;
	}
}