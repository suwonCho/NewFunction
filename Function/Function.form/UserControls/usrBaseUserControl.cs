using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Function.form
{
	public partial class usrBaseUserControl : UserControl
	{
		/// <summary>
		/// 메시지 처리 event
		/// </summary>
		public frmBaseForm.delSetMessage evtSetMessage = null;
		/// <summary>
		/// 에러 처리 event
		/// </summary>
		public frmBaseForm.delProcException evtProcException = null;


		public usrBaseUserControl()
		{
			InitializeComponent();
		}



		/// <summary>
		/// 메시지 처리 한다.
		/// </summary>
		/// <param name="isError"></param>
		/// <param name="strMessage"></param>
		/// <param name="isLog"></param>
		protected void SetMessage(bool isError, string strMessage, bool isLog)
		{
			if (evtSetMessage != null) evtSetMessage(isError ? enMsgType.Error : enMsgType.OK, strMessage, isLog);
		}

		/// <summary>
		/// 에러 처리를 한다.
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="strModuleName"></param>
		protected void ProcException(Exception ex, string strModuleName, bool isLog)
		{
			if (evtProcException != null) evtProcException(ex, strModuleName, isLog);
		}


	}
}
