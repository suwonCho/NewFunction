//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Data;
//using System.Text;
//using System.Windows.Forms;

//using Function;
//using Function.form;

//namespace Function.Device
//{
//	public partial class usrKR900_Monitoring : UserControl
//	{
//		KR900 kr;
		
//		public KR900 KR
//		{
//			get { return kr; }
//			set
//			{
//				kr = value;
//				if(kr != null)
//					kr.OnConnectStatusChange = new KR900.delConnectStatusChange(KR900_OnConnectStatusChange);
//			}
//		}


//		/// <summary>
//		/// 글자 폰트
//		/// </summary>
//		public Font Font
//		{
//			get { return lblStatus.Font; }
//			set
//			{
//				lblStatus.Font = value;
//			}
//		}

//		public usrKR900_Monitoring()
//		{
//			InitializeComponent();
//		}

//		public enum enStatusType { Waiting, Connenct, Connecting, Disconnect };

//		enStatusType Status_Current = enStatusType.Waiting;

//		/// <summary>
//		/// kr900 연결상태 변경..
//		/// </summary>
//		/// <param name="isStatus"></param>
//		public void KR900_OnConnectStatusChange(enStatusType Status)
//		{
//			switch(Status)
//			{
//				case enStatusType.Connenct:
//					//연결
//					Function.form.control.Invoke_Control_Text(lblStatus, "연결 됨..");
//					Function.form.control.Invoke_Control_Color(lblStatus, Color.Yellow, Color.RoyalBlue);
//					break;

//				case enStatusType.Disconnect:
//					//연결 안됨..
//					Function.form.control.Invoke_Control_Text(lblStatus, "연결 안됨");
//					Function.form.control.Invoke_Control_Color(lblStatus, Color.White, Color.Red);
//					break;

//				case enStatusType.Connecting:
//					Function.form.control.Invoke_Control_Text(lblStatus, "연결시도중");
//					Function.form.control.Invoke_Control_Color(lblStatus, Color.WhiteSmoke, Color.Black);
//					break;

//				default:
//					Function.form.control.Invoke_Control_Text(lblStatus, "연결 대기");
//					Function.form.control.Invoke_Control_Color(lblStatus, Color.WhiteSmoke, Color.Black);
//					break;
//			}

//			Status_Current = Status;
//		}


//		public void KR900_OnConnectStatusChange(bool isConnect)
//		{
//			if(isConnect)
//			{
//				//연결
//				Function.form.control.Invoke_Control_Text(lblStatus, "연결 됨..");
//				Function.form.control.Invoke_Control_Color(lblStatus, Color.Yellow, Color.RoyalBlue);

//				Status_Current = enStatusType.Connenct;
//			}
//			else
//			{	//연결 안됨..
//				Function.form.control.Invoke_Control_Text(lblStatus, "연결 안됨");
//				Function.form.control.Invoke_Control_Color(lblStatus, Color.White, Color.Red);

//				Status_Current = enStatusType.Disconnect;
//			}

//		}

//		public bool KR900isConnect()
//		{
//			if (Status_Current == enStatusType.Connenct)
//				return true;
//			else
//				return false;
//		}


//		private void lblStatus_DoubleClick(object sender, EventArgs e)
//		{
//			if (kr == null) return;

//			if (kr.IsConnected)
//			{
//				if (clsFunction.ShowMsg("H/H와 접속을 끊으 시겠습니까?", "", frmMessage.enMessageType.YesNo) == DialogResult.Yes)
//				{
//					kr.Close();
//				}
//			}
//			else
//			{
//				if (clsFunction.ShowMsg("H/H와 접속 하겠습니까?", "", frmMessage.enMessageType.YesNo) == DialogResult.Yes)
//				{
//					kr.Open();
//				}
//			}
//		}


//		public event EventHandler DoubleClick
//		{
//			add
//			{
//				lblStatus.DoubleClick += value;
//			}
//			remove
//			{
//				lblStatus.DoubleClick -= value;
//			}
//		}



//	}
//}
