using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Function.Advenced.IF
{
	/// <summary>
	/// 기본 IF 폼..
	/// </summary>
	public partial class usrIF_Base : UserControl
	{
		FarPoint.Win.Spread.FpSpread fplog;


		/// <summary>
		/// 로그 처리용..
		/// </summary>
		public Function.Util.Log Log;

		/// <summary>
		/// Log용 Spread
		/// </summary>
		public FarPoint.Win.Spread.FpSpread fpLog
		{
			get { return fplog; }
			set
			{
				//if (fplog != null)
				//{	//기존 컨트롤을 해제 한다..
				//    pnlBody.Controls.Remove(fplog);
				//}

				fplog = value;

				if (fplog != null)
				{	//컨트롤을 팬널에 추가후 처리..

					//if(fplog.Parent != null)
					//    fplog.Parent.Controls.Remove(fplog);

					pnlBody.Controls.Add(fplog);

					fplog.Dock = DockStyle.Fill;

					fplog.BringToFront();

				}

				//등록이 되어 있지 않으면 fpLog에 해더가 필드가 된다..
				if (strBindingField == string.Empty)
				{
					foreach (FarPoint.Win.Spread.Column c in fplog.ActiveSheet.Columns)
					{
						strBindingField += (strBindingField != string.Empty ? "\r\n" : string.Empty) + c.Label;
					}
				}
				

			}
		}

		/// <summary>
		/// 주기적 작업용 타이머..
		/// </summary>
		public System.Threading.Timer tmr;


		enLogAdd_Type defaltLogAdd_Type = enLogAdd_Type.Add_Top;

		/// <summary>
		/// 로그 추가 시 위치..
		/// </summary>
		public enLogAdd_Type DefaltLogAdd_Type 
		{
			get
			{
				return defaltLogAdd_Type;
			}
			set
			{
				defaltLogAdd_Type = value;
			}
		}

		/// <summary>
		/// 로그 추가 타입
		/// </summary>
		public enum enLogAdd_Type 
			{ 
				/// <summary>
				/// 신규 추가
				/// </summary>
				New, 
				/// <summary>
				/// 추가 위
				/// </summary>
				Add_Top,
				/// <summary>
				/// 추가 아래
				/// </summary>
				Add_Bottom 
			};

		/// <summary>
		/// 테이틀을 설정하거나 가져옵니다..
		/// </summary>
		public string TITLE
		{
			get
			{
				return lblTitle.Text;
			}
			set
			{
				lblTitle.Text = value;
			}
		}

		DateTime dtLasttime;

		/// <summary>
		/// 마지막 작업 시간..
		/// </summary>
		public DateTime LastWorkTime
		{
			get
			{
				return dtLasttime;
			}
			set
			{
				dtLasttime = value;

				lblShippingLasttime.Text = Fnc.Date2String(dtLasttime, Fnc.enDateType.DateTime);

			}
		}

		private string strBindingField = string.Empty;

		/// <summary>
		/// 바인딩 시 연결하는 필드명, 한줄에 하나 씩 입력..
		/// </summary>
		public string BindingField
		{
			get
			{
				return strBindingField;
			}
			set
			{
				strBindingField = value;
			}
		}


		private string strMsgheader = "작업을 시작 합니다.";

		/// <summary>
		/// 작업 시작시 로그에 표시되는 로그..
		/// </summary>
		public string MsgHeader
		{
			get { return strMsgheader; }
			set
			{
				strMsgheader = value;
			}
		}

		
		/// <summary>
		/// 인터페이스 기본 폼..
		/// </summary>
		public usrIF_Base()
		{
			InitializeComponent();			
		}




		/// <summary>
		/// 로그를 바인딩한다.
		/// </summary>
		/// <param name="dt"></param>
		public void LogBinding(DataTable dt)
		{
			if (fpLog == null) return;

			string [] strFields = strBindingField.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
			
			Function.Component.Spread.Invoke_DataSource(fpLog, fpLog.ActiveSheet, dt, strFields);

		}


		/// <summary>
		/// 로그 추기 delegate
		/// </summary>
		/// <param name="dr"></param>
		/// <param name="eLogAdd_Type"></param>
		/// <param name="isTextLog"></param>
		/// <param name="strMsgHeader"></param>
		public delegate void delLogAdd(DataRow dr, enLogAdd_Type eLogAdd_Type, bool isTextLog, string strMsgHeader);

		/// <summary>
		/// 로그를 추가 한다..
		/// </summary>
		/// <param name="dr"></param>
		/// <param name="eLogAdd_Type"></param>
		/// <param name="isTextLog">텍스트 로그 남김 여부..</param>
		/// <param name="strMsgHeader"></param>
		public void LogAdd(DataRow dr, enLogAdd_Type eLogAdd_Type, bool isTextLog, string strMsgHeader)
		{
			if (fpLog == null) return;

			if (fpLog.InvokeRequired)
			{
				fpLog.Invoke(new delLogAdd(LogAdd), new object[] { dr, eLogAdd_Type, isTextLog, strMsgHeader });
				return;
			}


			string [] strFields = strBindingField.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

			object[] obj = new object[strFields.Length];
					

			for (int i = 0; i < strFields.Length; i++)
			{
				try
				{
					obj[i] = dr[strFields[i]];
				}
				catch
				{
					obj[i] = string.Empty;
				}

				if (Log != null && isTextLog)
					strMsgHeader += string.Format("\r\n\t\t\t{0} : {1}", strFields[i], obj[i]);

			}

			

			int intAddRowindex = AddType_Proc(eLogAdd_Type);
			
			Function.Component.Spread.Invoke_AddRowData(fpLog, fpLog.ActiveSheet, 0, 500, obj);

			isLastLogHead = true;

			if (Log != null && isTextLog)
			{
				Log.WLog(strMsgHeader);
			}

		}


		/// <summary>
		/// 로그 추기 delegate
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="eLogAdd_Type"></param>
		/// <param name="isTextLog"></param>
		/// <param name="strMsgHeader"></param>
		public delegate void delLogAdd2(object[] obj, enLogAdd_Type eLogAdd_Type, bool isTextLog, string strMsgHeader);

		/// <summary>
		/// 로그를 추가 한다..
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="eLogAdd_Type"></param>
		/// <param name="isTextLog">텍스트 로그 남김 여부..</param>
		/// <param name="strMsgHeader"></param>
		public void LogAdd(object[] obj, enLogAdd_Type eLogAdd_Type, bool isTextLog, string strMsgHeader)
		{
			if (fpLog == null) return;

			if (fpLog.InvokeRequired)
			{
				fpLog.Invoke(new delLogAdd2(LogAdd), new object[] { obj, eLogAdd_Type, isTextLog, strMsgHeader });
				return;
			}

			string [] strFields = strBindingField.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
				

			int intAddRowindex = AddType_Proc(eLogAdd_Type);

			Function.Component.Spread.Invoke_AddRowData(fpLog, fpLog.ActiveSheet, 0, 500, obj);

			isLastLogHead = false;

			if (Log == null || !isTextLog) return;




			for (int i = 0; i < strFields.Length; i++)
			{			
				strMsgHeader += string.Format("\r\n\t\t\t{0} : {1}", strFields[i], obj[i]);
			}
			
			Log.WLog(strMsgHeader);
			
		}

		/// <summary>
		/// 로그 헤더 추가 delegate
		/// </summary>
		/// <param name="eLogAdd_Type"></param>
		/// <param name="strMsg"></param>
		/// <param name="colRowBackcolor"></param>
		/// <param name="isHeader"></param>
		/// <param name="isLastDelete"></param>
		public delegate void delHeadLogAdd(enLogAdd_Type eLogAdd_Type, string strMsg, Color colRowBackcolor, bool isHeader, bool isLastDelete);

		bool isLastLogHead = false;

		/// <summary>
		/// 해드로그를 남긴다..
		/// </summary>
		/// <param name="eLogAdd_Type"></param>
		/// <param name="strMsg">추가할 메세지..</param>
		/// <param name="colRowBackcolor">로그 컬러</param>
		/// <param name="isHeader">해더 여부..</param>
		/// <param name="isLastDelete">최종 데이터가 헤더일 경우 삭제 여부...</param>
		public void HeadLogAdd(enLogAdd_Type eLogAdd_Type, string strMsg, Color colRowBackcolor, bool isHeader,  bool isLastDelete)
		{

			if (fpLog.InvokeRequired)
			{
				fpLog.Invoke(new delHeadLogAdd(HeadLogAdd), new object[] { eLogAdd_Type, strMsg, colRowBackcolor, isHeader, isLastDelete });
				return;
			}


			int intAddRowindex = AddType_Proc(eLogAdd_Type);

			if (isLastDelete && isLastLogHead)
			{
				Function.Component.Spread.Invoke_DeleteRow(fpLog, fpLog.ActiveSheet, intAddRowindex, 1);
			}

			Function.Component.Spread.Invoke_AddRow(fpLog, fpLog.ActiveSheet, intAddRowindex, 1);

			if (intAddRowindex < 0)
				intAddRowindex = fpLog.ActiveSheet.RowCount - 1;

			fpLog.ActiveSheet.Cells[intAddRowindex, 0].Value = DateTime.Now;

			int intColCnt = fpLog.ActiveSheet.ColumnCount - 1;

			//컬럼 없음..
			if (intColCnt < 1) return;

			Function.Component.Spread.Invoke_CellSpan(fpLog, fpLog.ActiveSheet, intAddRowindex, 1, 1, intColCnt);

			fpLog.ActiveSheet.Cells[intAddRowindex, 1].Value = strMsg;

			fpLog.ActiveSheet.Rows[intAddRowindex].BackColor = colRowBackcolor;


			isLastLogHead = isHeader;

		}


		private int AddType_Proc(enLogAdd_Type eLogAdd_Type)
		{
			if (fpLog == null) return 0;

			int intAddRowindex = 0;

			switch (eLogAdd_Type)
			{
				case enLogAdd_Type.New:
					fpLog.ActiveSheet.Rows.Clear();
					fpLog.ActiveSheet.RowCount = 0;
					break;

				case enLogAdd_Type.Add_Top:
					intAddRowindex = 0;
					break;

				default:
					intAddRowindex = -1;
					break;
			}

			return intAddRowindex;
		}


		private int intWorkInterval = 5000;

		/// <summary>
		/// 작업 간격(ms)
		/// </summary>
		public int WorkInterval
		{
			get
			{
				return intWorkInterval;
			}

			set
			{
				intWorkInterval = value;

				if (tmr != null )
				{
					tmr.Change(intWorkInterval, intWorkInterval);
				}

                string str = string.Empty;
                int intMod = 0;
                int intValue = 0;

                //ms -> sec
                intValue = Math.DivRem(intWorkInterval, 1000, out intMod);

                if (intMod > 0)
                    str = string.Format("{0}ms", intMod);

                //sec -> min
                intValue = Math.DivRem(intValue, 60, out intMod);

                if (intMod > 0 || str != string.Empty)
                    str = string.Format("{0}초{1}", intMod, str);

                //min -> hour
                intValue = Math.DivRem(intValue, 60, out intMod);

                if (intMod > 0 || str != string.Empty)
                    str = string.Format("{0}분{1}", intMod, str);

                //hour -> day
                intValue = Math.DivRem(intValue, 24, out intMod);

                if (intMod > 0 || str != string.Empty)
                    str = string.Format("{0}시간{1}", intMod, str);

                if(intValue > 0)
                    str = string.Format("{0}일{1}", intValue, str);


                Function.form.control.Invoke_Control_Text(lblInterval, string.Format("(간격: {0})", str));

			}
		}





		/// <summary>
		/// 작업을 시작 한다..
		/// </summary>
		public void WorkStart()
		{
			//로그 초기회..
			Function.Component.Spread.Invoke_RowCount(fplog, fplog.ActiveSheet, 0);

			//타이머 시작..
			tmr = new System.Threading.Timer(new TimerCallback(Work), null, 5000, WorkInterval);
		}

		/// <summary>
		/// 작업 세부내역 delegate
		/// </summary>
		public delegate void delWorkDetail();

		/// <summary>
		/// 작업 세부 내역 
		/// </summary>
		public delWorkDetail WorkDetail = null;


		/// <summary>
		/// 주기 적으로 처리한 내역을 정의 한다.
		/// </summary>
		/// <param name="obj"></param>
		public virtual void Work(object obj)
		{
			if (WorkDetail != null)
				WorkDetail();


			Function.form.control.Invoke_Control_Text(lblShippingLasttime, Fnc.Date2String(DateTime.Now, Fnc.enDateType.DateTime));
		}









	}
}
