using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;

namespace tcp_server
{
	/// <summary>
	/// frmChat�� ���� ��� �����Դϴ�.
	/// </summary>
	public class frmChat : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		/// <summary>
		/// �ʼ� �����̳� �����Դϴ�.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmChat()
		{
			//
			// Windows Form �����̳� ������ �ʿ��մϴ�.
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent�� ȣ���� ���� ������ �ڵ带 �߰��մϴ�.
			//
		}

		/// <summary>
		/// ��� ���� ��� ���ҽ��� �����մϴ�.
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

		#region Windows Form �����̳ʿ��� ������ �ڵ�
		/// <summary>
		/// �����̳� ������ �ʿ��� �޼����Դϴ�.
		/// �� �޼����� ������ �ڵ� ������� �������� ���ʽÿ�.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button2 = new System.Windows.Forms.Button();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(432, 384);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(72, 32);
			this.button1.TabIndex = 0;
			this.button1.Text = "��������";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(16, 8);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new System.Drawing.Size(488, 368);
			this.textBox1.TabIndex = 1;
			this.textBox1.Text = "";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(344, 384);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(72, 32);
			this.button2.TabIndex = 2;
			this.button2.Text = "��������";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(16, 384);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(312, 21);
			this.textBox2.TabIndex = 3;
			this.textBox2.Text = "";
			// 
			// comboBox1
			// 
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
			this.comboBox1.Location = new System.Drawing.Point(512, 8);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(192, 376);
			this.comboBox1.TabIndex = 4;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(632, 384);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(72, 32);
			this.button3.TabIndex = 5;
			this.button3.Text = "��������";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(536, 392);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(72, 24);
			this.button4.TabIndex = 6;
			this.button4.Text = "button4";
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// frmChat
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(712, 422);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Name = "frmChat";
			this.Text = "frmChat";
			this.Load += new System.EventHandler(this.frmChat_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// �ش� ���� ���α׷��� �� �������Դϴ�.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new frmChat());
		}
		
		Network nn = new Network();

		private void frmChat_Load(object sender, System.EventArgs e)
		{
		
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			
			if (nn.SererStatus())
			{
				nn.ServerStop();
				nn.�޺���������();
				
				this.button1.Text = "��������";
			}
			else
			{
				nn.tbox = this.textBox1;
				nn.cbox = this.comboBox1;
				nn.svr_start();
				this.button1.Text = "��������";
			}
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			nn.Send_all(this.textBox2.Text);

			/*
			byte [] by = new byte[1];
			by[0] = 0xff;
			 StringBuilder _stringBuilder = new StringBuilder();
				foreach(Byte _byte in by)
				    _stringBuilder.AppendFormat("{0:X2}", _byte);


			this.textBox1.AppendText(_stringBuilder.ToString());
			this.textBox1.AppendText("pp");
			*/

		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			nn.Ŭ���̾�Ʈ��������(��());

		}

		private int ��()
		{
			string �ӽ� = this.comboBox1.SelectedItem.ToString();
			�ӽ� = �ӽ�.Substring(�ӽ�.IndexOf("[")+1,�ӽ�.IndexOf("]")-1);
			return Convert.ToInt16(�ӽ�,10);

		}

		Form1 frm;
		
		private void button4_Click(object sender, System.EventArgs e)
		{
			frm = new Form1(this);
			frm.evt += new MyEvent(test);
			frm.Show();

		}
		private void test(int a)
		{
			Console.WriteLine(a);
		}

	}
}
