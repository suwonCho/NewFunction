using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Function.form
{
	public partial class subBaseForm : clsbaseForm
	{
		/// <summary>
		/// 메시지 처리 event
		/// </summary>
		public event frmBaseForm.delSetMessage OnSetMessage
		{
			add { _onSetMessage += value; }
			remove { _onSetMessage -= value; }
		}

		event frmBaseForm.delSetMessage _onSetMessage;




		/// <summary>
		/// 에러 처리 event
		/// </summary>
		public event frmBaseForm.delProcException OnProcException
		{
			add { _onProcException += value; }
			remove { _onProcException -= value; }
		}
		
		event frmBaseForm.delProcException _onProcException;



		/// <summary>
		/// 메시지 처리 event
		/// </summary>
		public event frmBaseForm.delSetMessage OnSetLastMessage
		{
			add { _onSetLastMessage += value; }
			remove { _onSetLastMessage -= value; }
		}


		event frmBaseForm.delSetMessage _onSetLastMessage;

		bool LastMessageError = false;
		string LastMessage = null;

		
		/// <summary>
		/// 메시지 처리 한다.
		/// </summary>
		/// <param name="isError"></param>
		/// <param name="strMessage"></param>
		/// <param name="isLog"></param>
		public void SetMessage(bool isError, string strMessage, bool isLog)
		{
			if (_onSetMessage != null)
			{
				_onSetMessage(isError ? enMsgType.Error : enMsgType.OK, strMessage, isLog);
				LastMessageSet();
			}
		}

		/// <summary>
		/// 에러 처리를 한다.
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="strModuleName"></param>
		public void ProcException(Exception ex, string strModuleName, bool isLog = false)
		{
			if (_onProcException != null)
			{
				_onProcException(ex, strModuleName, isLog);
				LastMessageSet();
			}
		}

		public void LastMessageSet()
		{
			frmBaseForm parent = this.ParentForm as frmBaseForm;
			if (parent == null) return;

			LastMessageError = parent.LastMessageError;
			LastMessage = parent.LastMessage;

		}


		public subBaseForm()
		{
			InitializeComponent();

			this.ParentChanged += new EventHandler(subBaseForm_ParentChanged);
			
		}

		private void subBaseForm_Load(object sender, EventArgs e)
		{
			
		}

		void subBaseForm_ParentChanged(object sender, EventArgs e)
		{
			try
			{
				frmBaseForm frm = ((MdiClient)Parent).TopLevelControl as frmBaseForm;

				if (frm == null) return;

				frm.frmBaseForm_ControlAdded(this, null);
			}
			catch { }
		}

		private void subBaseForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			
		}

		private void subBaseForm_Activated(object sender, EventArgs e)
		{
			if (LastMessage == null) return;
			if (_onSetLastMessage != null) _onSetLastMessage(LastMessageError ? enMsgType.Error : enMsgType.OK, LastMessage, false);
		}

		
	}
}