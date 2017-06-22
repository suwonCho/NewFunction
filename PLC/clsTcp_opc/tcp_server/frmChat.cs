using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;

namespace tcp_server
{
	/// <summary>
	/// frmChat에 대한 요약 설명입니다.
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
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmChat()
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent를 호출한 다음 생성자 코드를 추가합니다.
			//
		}

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
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

		#region Windows Form 디자이너에서 생성한 코드
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
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
			this.button1.Text = "서버시작";
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
			this.button2.Text = "문구전송";
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
			this.button3.Text = "강제종료";
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
		/// 해당 응용 프로그램의 주 진입점입니다.
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
				nn.콤보리프레쉬();
				
				this.button1.Text = "서버시작";
			}
			else
			{
				nn.tbox = this.textBox1;
				nn.cbox = this.comboBox1;
				nn.svr_start();
				this.button1.Text = "서버종료";
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
			nn.클라이언트접속해제(값());

		}

		private int 값()
		{
			string 임시 = this.comboBox1.SelectedItem.ToString();
			임시 = 임시.Substring(임시.IndexOf("[")+1,임시.IndexOf("]")-1);
			return Convert.ToInt16(임시,10);

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
