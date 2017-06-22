using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Function.form
{
	/// <summary>
	/// 숫자 입력 사용자 컨트롤 - 정해진 정수 자리수를 입력 받고 표시 한다.
	/// </summary>
	public partial class usrNumberInput2 : UserControl
	{


		int _numberLength = 5;

		/// <summary>
		/// 입력 받을 숫자 자리 수
		/// </summary>
		[Description("입력 받을 숫자 자리 수")]
		public int NumberLength
		{
			get { return _numberLength; }
			set
			{

				if(_numberLength < 1 || _numberLength > 10)
				{
					throw new Exception("NumberLength 자리수를 1~10 사이로 입력 하여 주십시요");
				}

				_numberLength = value;

				InitNumber();
			}
		}


		bool _showNumber = false;

		/// <summary>
		/// 입력받은 숫자 표시 여부
		/// </summary>
		[Description("입력받은 숫자 표시 여부")]
		public bool ShowNumber
		{
			get { return _showNumber; }
			set
			{
				_showNumber = value;

				InitNumber();
			}
		}

		/// <summary>
		/// 입력된 값을 가지고 옵니다. null이면 입력 값이 없는것
		/// </summary>
		public string Value = null;

		int _btns_cnt = 14;
		Button[] _btns = null;


		event EventHandler _OK_Click;

		/// <summary>
		/// 확인 버튼 클릭
		/// </summary>
		public event EventHandler OK_Click
		{
			add { _OK_Click += value; }
			remove { _OK_Click -= value; }
		}


		event EventHandler _Cancel_Click;

		/// <summary>
		/// 취소 버튼 클릭
		/// </summary>
		public event EventHandler Cancel_Click
		{
			add { _Cancel_Click += value; }
			remove { _Cancel_Click -= value; }
		}



		/// <summary>
		/// 숫자 입력 사용자 컨트롤 - 정해진 정수 자리수를 입력 받고 표시 한다.
		/// </summary>
		public usrNumberInput2()
		{
			InitializeComponent();
		}


		public void InitForm()
		{
			int idx = 0;

			float s = 0;

			//버튼들을 생성한다.
			if (_btns == null)
			{
				Button btn;
				_btns = new Button[_btns_cnt];

				//숫자 0~9
				for (int n = 0; n < 10; n++)
				{
					btn = new Button();
					btn.TextAlign = ContentAlignment.MiddleCenter;
					btn.Text = n.ToString();
					btn.Tag = n;
					pnlBody.Controls.Add(btn);
					_btns[idx] = btn;

					idx++;
				}

				//백스페이스
				btn = new Button();
				btn.Image = Function.resIcon16.back_alt;
				btn.ImageAlign = ContentAlignment.MiddleCenter;
				btn.Tag = "B";
				pnlBody.Controls.Add(btn);
				_btns[idx] = btn;
				idx++;

				//clear
				btn = new Button();
				btn.TextAlign = ContentAlignment.MiddleCenter;
				btn.Text = "Clr";
				btn.Tag = "C";
				pnlBody.Controls.Add(btn);
				_btns[idx] = btn;
				idx++;

				//확인
				btn = new Button();
				btn.TextAlign = ContentAlignment.MiddleCenter;
				btn.Text = "확인";
				btn.Tag = "O";
				pnlBody.Controls.Add(btn);
				_btns[idx] = btn;
				idx++;

				//취소
				btn = new Button();
				btn.TextAlign = ContentAlignment.MiddleCenter;
				btn.Text = "취소";
				btn.Tag = "X";
				pnlBody.Controls.Add(btn);
				_btns[idx] = btn;
				idx++;


				for (int n = 0; n < 14; n++)
				{
					_btns[n].Click += Buttons_Click;
					_btns[n].BackColor = SystemColors.Control;
				}
			}
			
			//위치를 계산한다.

			//판넬 높이
			pnlTop.Height = Height * 3 / 10;

			//입력 받은 숫자의 폰트 크기
			lblNumber.Font = Function.form.control.Font_Control_Resize_Get(lblNumber, control.enControl_Criteria.height, vari.circle_white, 0.75f);	



			//위치를 재배열한다.
			//버튼사이 폭
			int gap = 3;
			//버튼 너비
			int bw = (pnlBody.Width - (gap * 8)) / 7;
			int bh = (pnlBody.Height - (gap * 3)) / 2;
			int l = gap;
			int t = gap;
			idx = 0;

			for(int y = 0; y < 2;y++)
			{
				for(int x = 0;x <7;x++)
				{
					_btns[idx].Height = bh;
					_btns[idx].Width = bw;

					_btns[idx].Left = l;
					_btns[idx].Top = t;

					l += bw + gap;
					idx++;
				}

				l = gap;
				t += bh + gap;
			}



			//숫자 버튼 폰트
			Font fnt = Function.form.control.Font_Control_Resize_Get(_btns[0], control.enControl_Criteria.height, "9", 0.75f);
			for (int i = 0; i < 10; i++)
			{
				_btns[i].Font = fnt;
			}

			fnt = Function.form.control.Font_Control_Resize_Get(_btns[0], control.enControl_Criteria.width, "확인", 0.75f);

			for (int i = 10; i < 14; i++)
			{
				_btns[i].Font = fnt;
			}
		}

		/// <summary>
		/// 버튼 클릭 처리
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Buttons_Click(object sender, EventArgs e)
		{
			Button btn = sender as Button;

			if (btn == null) return;

			//string vv = Value == -1 ? string.Empty : Value.ToString();
			int v = 0;

			if(!int.TryParse(btn.Tag.ToString(), out v))
			{
				switch(btn.Tag.ToString())
				{
					case "B":   //백스페이스

						if(Value != null && Value.Length == 1)
						{
							Value = null;
							InitNumber();
						}
						if(Value != null)
						{
							Value = Value.Substring(0, Value.Length - 1);
							InitNumber();
						}
						break;

					case "C":   //clear
						Value = null;
						InitNumber();
						break;

					case "O":   //OK
						if (_OK_Click != null) _OK_Click(null, new EventArgs());
						break;

					case "X":   //cancel
						if (_Cancel_Click != null) _Cancel_Click(null, new EventArgs());
						break;
				}

				Console.WriteLine("Value:{0}", Value);
				return;
			}
			
			if (Value != null && Value.Length >= NumberLength) return;

			Value += btn.Tag.ToString();			
			InitNumber();

			Console.WriteLine("Value:{0}", Value);
		}


		/// <summary>
		/// 숫자 표시를 처리 한다.
		/// </summary>
		public void InitNumber()
		{
			string v = string.Empty;
			int i;

			if (Value != null)
			{
				foreach(char c in Value)
				{
					if (_showNumber)
					{
						i = int.Parse(c.ToString());
						v += vari.circle_numbers[i];
					}
					else
						v += vari.circle_black;
					
				}
			}

			v = v.PadRight(NumberLength, Char.Parse(vari.circle_white)).Substring(0, NumberLength);

			lblNumber.Text = v;

		}



		private void usrNumberInput2_Load(object sender, EventArgs e)
		{
			InitForm();
		}

		private void usrNumberInput2_SizeChanged(object sender, EventArgs e)
		{
			InitForm();
		}
	}
}
