namespace TLSharp.Test
{
	partial class frmMassenger
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
			this.btnConnect = new System.Windows.Forms.Button();
			this.btnPCAuth = new System.Windows.Forms.Button();
			this.btnGetContact = new System.Windows.Forms.Button();
			this.grdContacts = new System.Windows.Forms.DataGridView();
			this.btnChatList = new System.Windows.Forms.Button();
			this.grdChat = new System.Windows.Forms.DataGridView();
			this.txtMsgContacts = new System.Windows.Forms.TextBox();
			this.btnSendMsgDataRow = new System.Windows.Forms.Button();
			this.btnSendMsgPhone = new System.Windows.Forms.Button();
			this.btnSendChatMsgPhone = new System.Windows.Forms.Button();
			this.btnSendChatMsgDataRow = new System.Windows.Forms.Button();
			this.txtMsgChat = new System.Windows.Forms.TextBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.grdContacts)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.grdChat)).BeginInit();
			this.SuspendLayout();
			// 
			// btnConnect
			// 
			this.btnConnect.Location = new System.Drawing.Point(337, 1);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(111, 21);
			this.btnConnect.TabIndex = 0;
			this.btnConnect.Text = "연결";
			this.btnConnect.UseVisualStyleBackColor = true;
			this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
			// 
			// btnPCAuth
			// 
			this.btnPCAuth.Location = new System.Drawing.Point(454, 1);
			this.btnPCAuth.Name = "btnPCAuth";
			this.btnPCAuth.Size = new System.Drawing.Size(111, 21);
			this.btnPCAuth.TabIndex = 1;
			this.btnPCAuth.Text = "PC 인증";
			this.btnPCAuth.UseVisualStyleBackColor = true;
			this.btnPCAuth.Click += new System.EventHandler(this.btnPCAuth_Click);
			// 
			// btnGetContact
			// 
			this.btnGetContact.Location = new System.Drawing.Point(3, 25);
			this.btnGetContact.Name = "btnGetContact";
			this.btnGetContact.Size = new System.Drawing.Size(111, 21);
			this.btnGetContact.TabIndex = 2;
			this.btnGetContact.Text = "연락처 조회";
			this.btnGetContact.UseVisualStyleBackColor = true;
			this.btnGetContact.Click += new System.EventHandler(this.btnGetContact_Click);
			// 
			// grdContacts
			// 
			this.grdContacts.AllowUserToAddRows = false;
			this.grdContacts.AllowUserToDeleteRows = false;
			this.grdContacts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grdContacts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.grdContacts.Location = new System.Drawing.Point(3, 47);
			this.grdContacts.Name = "grdContacts";
			this.grdContacts.ReadOnly = true;
			this.grdContacts.RowTemplate.Height = 23;
			this.grdContacts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.grdContacts.Size = new System.Drawing.Size(918, 247);
			this.grdContacts.TabIndex = 3;
			// 
			// btnChatList
			// 
			this.btnChatList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnChatList.Location = new System.Drawing.Point(3, 300);
			this.btnChatList.Name = "btnChatList";
			this.btnChatList.Size = new System.Drawing.Size(111, 21);
			this.btnChatList.TabIndex = 4;
			this.btnChatList.Text = "Chat List 조회";
			this.btnChatList.UseVisualStyleBackColor = true;
			this.btnChatList.Click += new System.EventHandler(this.btnChatList_Click);
			// 
			// grdChat
			// 
			this.grdChat.AllowUserToAddRows = false;
			this.grdChat.AllowUserToDeleteRows = false;
			this.grdChat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grdChat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.grdChat.Location = new System.Drawing.Point(3, 322);
			this.grdChat.Name = "grdChat";
			this.grdChat.ReadOnly = true;
			this.grdChat.RowTemplate.Height = 23;
			this.grdChat.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.grdChat.Size = new System.Drawing.Size(918, 247);
			this.grdChat.TabIndex = 5;
			// 
			// txtMsgContacts
			// 
			this.txtMsgContacts.Location = new System.Drawing.Point(120, 25);
			this.txtMsgContacts.Name = "txtMsgContacts";
			this.txtMsgContacts.Size = new System.Drawing.Size(445, 21);
			this.txtMsgContacts.TabIndex = 6;
			// 
			// btnSendMsgDataRow
			// 
			this.btnSendMsgDataRow.Location = new System.Drawing.Point(571, 25);
			this.btnSendMsgDataRow.Name = "btnSendMsgDataRow";
			this.btnSendMsgDataRow.Size = new System.Drawing.Size(131, 21);
			this.btnSendMsgDataRow.TabIndex = 7;
			this.btnSendMsgDataRow.Text = "메시지전송(DataRow)";
			this.btnSendMsgDataRow.UseVisualStyleBackColor = true;
			this.btnSendMsgDataRow.Click += new System.EventHandler(this.btnSendMsgDataRow_Click);
			// 
			// btnSendMsgPhone
			// 
			this.btnSendMsgPhone.Location = new System.Drawing.Point(708, 24);
			this.btnSendMsgPhone.Name = "btnSendMsgPhone";
			this.btnSendMsgPhone.Size = new System.Drawing.Size(131, 21);
			this.btnSendMsgPhone.TabIndex = 8;
			this.btnSendMsgPhone.Text = "메시지전송(전번)";
			this.btnSendMsgPhone.UseVisualStyleBackColor = true;
			this.btnSendMsgPhone.Click += new System.EventHandler(this.btnSendMsgPhone_Click);
			// 
			// btnSendChatMsgPhone
			// 
			this.btnSendChatMsgPhone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSendChatMsgPhone.Location = new System.Drawing.Point(708, 299);
			this.btnSendChatMsgPhone.Name = "btnSendChatMsgPhone";
			this.btnSendChatMsgPhone.Size = new System.Drawing.Size(131, 21);
			this.btnSendChatMsgPhone.TabIndex = 11;
			this.btnSendChatMsgPhone.Text = "메시지전송(ID)";
			this.btnSendChatMsgPhone.UseVisualStyleBackColor = true;
			this.btnSendChatMsgPhone.Click += new System.EventHandler(this.btnSendChatMsgPhone_Click);
			// 
			// btnSendChatMsgDataRow
			// 
			this.btnSendChatMsgDataRow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSendChatMsgDataRow.Location = new System.Drawing.Point(571, 300);
			this.btnSendChatMsgDataRow.Name = "btnSendChatMsgDataRow";
			this.btnSendChatMsgDataRow.Size = new System.Drawing.Size(131, 21);
			this.btnSendChatMsgDataRow.TabIndex = 10;
			this.btnSendChatMsgDataRow.Text = "메시지전송(DataRow)";
			this.btnSendChatMsgDataRow.UseVisualStyleBackColor = true;
			this.btnSendChatMsgDataRow.Click += new System.EventHandler(this.btnSendChatMsgDataRow_Click);
			// 
			// txtMsgChat
			// 
			this.txtMsgChat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.txtMsgChat.Location = new System.Drawing.Point(120, 300);
			this.txtMsgChat.Name = "txtMsgChat";
			this.txtMsgChat.Size = new System.Drawing.Size(445, 21);
			this.txtMsgChat.TabIndex = 9;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(3, 2);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(328, 21);
			this.textBox1.TabIndex = 12;
			this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
			// 
			// frmMassenger
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(925, 610);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.btnSendChatMsgPhone);
			this.Controls.Add(this.btnSendChatMsgDataRow);
			this.Controls.Add(this.txtMsgChat);
			this.Controls.Add(this.btnSendMsgPhone);
			this.Controls.Add(this.btnSendMsgDataRow);
			this.Controls.Add(this.txtMsgContacts);
			this.Controls.Add(this.grdChat);
			this.Controls.Add(this.btnChatList);
			this.Controls.Add(this.grdContacts);
			this.Controls.Add(this.btnGetContact);
			this.Controls.Add(this.btnPCAuth);
			this.Controls.Add(this.btnConnect);
			this.Name = "frmMassenger";
			this.Text = "frmMassenger";
			this.Controls.SetChildIndex(this.btnConnect, 0);
			this.Controls.SetChildIndex(this.btnPCAuth, 0);
			this.Controls.SetChildIndex(this.btnGetContact, 0);
			this.Controls.SetChildIndex(this.grdContacts, 0);
			this.Controls.SetChildIndex(this.btnChatList, 0);
			this.Controls.SetChildIndex(this.grdChat, 0);
			this.Controls.SetChildIndex(this.txtMsgContacts, 0);
			this.Controls.SetChildIndex(this.btnSendMsgDataRow, 0);
			this.Controls.SetChildIndex(this.btnSendMsgPhone, 0);
			this.Controls.SetChildIndex(this.txtMsgChat, 0);
			this.Controls.SetChildIndex(this.btnSendChatMsgDataRow, 0);
			this.Controls.SetChildIndex(this.btnSendChatMsgPhone, 0);
			this.Controls.SetChildIndex(this.textBox1, 0);
			((System.ComponentModel.ISupportInitialize)(this.grdContacts)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.grdChat)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnConnect;
		private System.Windows.Forms.Button btnPCAuth;
		private System.Windows.Forms.Button btnGetContact;
		private System.Windows.Forms.DataGridView grdContacts;
		private System.Windows.Forms.Button btnChatList;
		private System.Windows.Forms.DataGridView grdChat;
		private System.Windows.Forms.TextBox txtMsgContacts;
		private System.Windows.Forms.Button btnSendMsgDataRow;
		private System.Windows.Forms.Button btnSendMsgPhone;
		private System.Windows.Forms.Button btnSendChatMsgPhone;
		private System.Windows.Forms.Button btnSendChatMsgDataRow;
		private System.Windows.Forms.TextBox txtMsgChat;
		private System.Windows.Forms.TextBox textBox1;
	}
}