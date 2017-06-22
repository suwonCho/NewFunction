using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using Function;
using Function.Util;
using Function.Db;

using System.Threading;
using System.IO;
using System.Xml;

namespace Function.uScm
{
    public partial class frmBaseForm : Form
    {
        #region 변수선언부
        /// <summary>
        /// Form에 현재시간 부분 업데이트
        /// </summary>
        System.Threading.Timer tmrDtNow;

		protected Function.EpcMW.clsJuno clsRfid;        

        protected Log clsLog;

        protected OracleDB.strConnect strDBConn;

        /// <summary>
        /// 시스템 id
        /// </summary>
		protected string SYSTEMID = string.Empty;
        /// <summary>
        /// 프로그램 id
        /// </summary>
		protected string PROGRAMID = string.Empty;
        /// <summary>
        /// 계획 이벤트
        /// </summary>
		protected string strPlanEvent = string.Empty;
        /// <summary>
        /// 현재 이벤트
        /// </summary>
		protected string strProdEvent = string.Empty;

		protected string strTitle = string.Empty;

		/// <summary>
		/// StationID - Chasys사용 않함
		/// </summary>
		protected string strStationID = string.Empty;
        /// <summary>
        /// epcis class
        /// </summary>
		protected clsEPCIS clsEpc;
        /// <summary>
        /// 테그 리딩 시간(ms)
        /// </summary>
		protected int intTagReadingDuration = 3000;

		
        /// <summary>
        /// 초기화 에러..
        /// </summary>
		string strerrMsg = string.Empty;
        		
        /// <summary>
        /// 초기화 작업중.. 완료 하면서 false로..
        /// </summary>
		protected bool isInitilalize = true;

        /// <summary>
        /// 중복체크 뮤텍스
        /// </summary>
		protected Mutex gM;


        /// <summary>
        /// 에러 메세지 처리..
        /// </summary>
		protected string strErrMsg
		{
			get
			{
				return strerrMsg;
			}
			set
			{
				if (strerrMsg != string.Empty) strerrMsg += " / ";

				strerrMsg += value;
			}
		}


        #endregion

		

        public frmBaseForm()
        {
            InitializeComponent();
        }

        private void frmBaseForm_Load(object sender, EventArgs e)
        {
            //title에 시간 관련 처리..
			lblStartTime.Text = Fnc.Date2String(DateTime.Now, Fnc.enDateType.DateTime);
            tmrDtNow = new System.Threading.Timer(new TimerCallback(UpdateDtNow), null, 0, 1000);

            this.Text += "  v" + Application.ProductVersion;

#if(DEBUG && TEST)
            this.Text += " --- Debug/TEST Mode";
#elif (DEBUG)
            this.Text += " --- Debug Mode";
#else
			//this.WindowState = FormWindowState.Maximized;
#endif

            //Logo파일..
            if (File.Exists(@".\logo.bmp"))
            {
                picLogo.Load(@".\logo.bmp");
            }
            
			Thread thInitForm = new Thread(new ThreadStart(initForm));

            thInitForm.IsBackground = true;
            thInitForm.Start();

        }

        private void UpdateDtNow(object obj)
        {
			form.control.Invoke_Control_Text(lblNowTime, Fnc.Date2String(DateTime.Now, Fnc.enDateType.DateTime));
        }

        protected virtual void initForm () 
        {
        }

        /// <summary>
        /// 폼 초기화 시작 기본 작업을 한다.
        /// </summary>
        protected XML initStart()
        {
            chFormEnabled(false);
            chpnlVisible(true);

            //xml로 부터 설정을 읽는다.
            XML xml = new XML(XML.enXmlType.File, @"./config.xml");
			            
			PROGRAMID = xml.GetSingleNodeValue("SystemInfo/PROGRAMID");            
            strDBConn.strTNS = xml.GetSingleNodeValue("DataBase/TNS");
            strDBConn.strID = xml.GetSingleNodeValue("DataBase/ID");
            strDBConn.strPass = "zaq1";
			


			xml.chSingleNode(@"SETTING/" + PROGRAMID);

            SYSTEMID = xml.GetSingleNodeValue("SYSTEMID");
			strPlanEvent = xml.GetSingleNodeValue("PLANEVENT");
			strProdEvent = xml.GetSingleNodeValue("PRODEVENT");
			strStationID = xml.GetSingleNodeValue("STATIONID");

			strTitle = xml.GetSingleNodeValue("TITLE");
            form.control.Invoke_Control_Text(this.lblTitle, strTitle);
                        
            clsLog = new Log(Application.StartupPath + "\\LOG", xml.GetSingleNodeValue("LogFileName"), 30, true);    

            
			clsEpc = new clsEPCIS(xml);

			xml.chNode2Root();
			


			//프로그램 중복 체크를 한다.

			bool isNew = false;

			gM = form.control.ProgramRunCheck(PROGRAMID, out isNew);


			if (!isNew)
			{
				Fnc.ShowMsg("이미 프로그램이 실행중입니다.", "기존 프로그램을 종료 후 다시 실행 하여 주십시요.", frmMessage.enMessageType.OK);
				form.control.Invoke_Form_Close(this);
				return null;
			}
			else
			{
				gM.ReleaseMutex();
			}
			

            return xml;
            
        }

		protected virtual void initEnd()
        {
			if (strErrMsg != string.Empty)
			{
				Fnc.ShowMsg("프로그램 환경 설정 도중 에러가 발생 하여 프로그램이 정상적으로 작동 하지 않을 수 있습니다.", 
					strErrMsg, frmMessage.enMessageType.OK);

				if (clsLog != null)
					clsLog.WLog("프로그램 환경 설정 도중 에러가 발생 하여 프로그램이 정상적으로 작동 하지 않을 수 있습니다.\r\n" +
						strerrMsg);

			}
            else
                SetMessage(false, "프로그램을 시작 합니다.", true); 

            chFormEnabled(true);
            
            chpnlVisible(false);

			isInitilalize = false;
        }


        delegate void delchEnabled(bool isValue);

        private void chFormEnabled(bool isValue)
        {
            if (this.InvokeRequired)
                this.Invoke(new delchEnabled(chFormEnabled), new object[] { isValue });
            else
                this.Enabled = isValue;
        }

		/// <summary>
		/// 판넬표시 변경
		/// </summary>
		/// <param name="isValue"></param>
        protected void chpnlVisible(bool isValue)
        {
            if (pnlInit.InvokeRequired)
                pnlInit.Invoke(new delchEnabled(chpnlVisible), new object[] { isValue });
            else
            {
                pnlInit.BringToFront();
                pnlInit.Visible = isValue;
            }
        }

		/// <summary>
		/// 판넬 label text변경...
		/// </summary>
		/// <param name="strText"></param>
		protected void lblPnl_Text(string strText)
		{
			form.control.Invoke_Control_Text(lblPnl, strText);
		}


		/// <summary>
		/// form 예외 처리
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="strModule"></param>
        protected void ProcException(Exception ex, string strModule)
        {
			if (clsLog != null)  clsLog.WLog_Exception(strModule, ex);
            SetMessage(true, ex.Message, false);
        }

        delegate void delSetMessage(bool isError, string strMessage, bool isLog);
        /// <summary>
        /// 메시지 창에 내용을 보여 준다.
        /// </summary>
        /// <param name="isError">에러 여부</param>
        /// <param name="strMessage"></param>
        /// <param name="isLog"></param>
        protected virtual void SetMessage(bool isError, string strMessage, bool isLog)
        {
            if (lblMessage.InvokeRequired)
            {
                lblMessage.Invoke(new delSetMessage(SetMessage), new object[] { isError, strMessage, isLog });
                return;
            }

            if (isError)
                lblMessage.BackColor = Color.Tomato;
            else
                lblMessage.BackColor = Color.Black;

            lblMessage.Text = string.Format("[{0}] {1}", Fnc.Date2String(DateTime.Now, Fnc.enDateType.Time), strMessage);

			if (clsLog != null && isLog)
                clsLog.WLog(strMessage);

        }





        protected virtual void frmBaseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
			if (!isInitilalize && Fnc.ShowMsg(this, "프로그램을 종료 하시겠습니까?", string.Empty, frmMessage.enMessageType.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

			if (clsRfid != null)
			{
				//clsRfid.UnSubScribe();
				clsRfid.Dispose();
			}

			
			if (!isInitilalize) SetMessage(false, "프로그램을 종료 합니다.", true);
        }

		private void frmBaseForm_SizeChanged(object sender, EventArgs e)
		{
#if(!DEBUG)          
			//this.WindowState = FormWindowState.Maximized;
#endif
		}

	
		
    }
}