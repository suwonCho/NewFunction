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
        #region ���������
        /// <summary>
        /// Form�� ����ð� �κ� ������Ʈ
        /// </summary>
        System.Threading.Timer tmrDtNow;

		protected Function.EpcMW.clsJuno clsRfid;        

        protected Log clsLog;

        protected OracleDB.strConnect strDBConn;

        /// <summary>
        /// �ý��� id
        /// </summary>
		protected string SYSTEMID = string.Empty;
        /// <summary>
        /// ���α׷� id
        /// </summary>
		protected string PROGRAMID = string.Empty;
        /// <summary>
        /// ��ȹ �̺�Ʈ
        /// </summary>
		protected string strPlanEvent = string.Empty;
        /// <summary>
        /// ���� �̺�Ʈ
        /// </summary>
		protected string strProdEvent = string.Empty;

		protected string strTitle = string.Empty;

		/// <summary>
		/// StationID - Chasys��� ����
		/// </summary>
		protected string strStationID = string.Empty;
        /// <summary>
        /// epcis class
        /// </summary>
		protected clsEPCIS clsEpc;
        /// <summary>
        /// �ױ� ���� �ð�(ms)
        /// </summary>
		protected int intTagReadingDuration = 3000;

		
        /// <summary>
        /// �ʱ�ȭ ����..
        /// </summary>
		string strerrMsg = string.Empty;
        		
        /// <summary>
        /// �ʱ�ȭ �۾���.. �Ϸ� �ϸ鼭 false��..
        /// </summary>
		protected bool isInitilalize = true;

        /// <summary>
        /// �ߺ�üũ ���ؽ�
        /// </summary>
		protected Mutex gM;


        /// <summary>
        /// ���� �޼��� ó��..
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
            //title�� �ð� ���� ó��..
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

            //Logo����..
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
        /// �� �ʱ�ȭ ���� �⺻ �۾��� �Ѵ�.
        /// </summary>
        protected XML initStart()
        {
            chFormEnabled(false);
            chpnlVisible(true);

            //xml�� ���� ������ �д´�.
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
			


			//���α׷� �ߺ� üũ�� �Ѵ�.

			bool isNew = false;

			gM = form.control.ProgramRunCheck(PROGRAMID, out isNew);


			if (!isNew)
			{
				Fnc.ShowMsg("�̹� ���α׷��� �������Դϴ�.", "���� ���α׷��� ���� �� �ٽ� ���� �Ͽ� �ֽʽÿ�.", frmMessage.enMessageType.OK);
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
				Fnc.ShowMsg("���α׷� ȯ�� ���� ���� ������ �߻� �Ͽ� ���α׷��� ���������� �۵� ���� ���� �� �ֽ��ϴ�.", 
					strErrMsg, frmMessage.enMessageType.OK);

				if (clsLog != null)
					clsLog.WLog("���α׷� ȯ�� ���� ���� ������ �߻� �Ͽ� ���α׷��� ���������� �۵� ���� ���� �� �ֽ��ϴ�.\r\n" +
						strerrMsg);

			}
            else
                SetMessage(false, "���α׷��� ���� �մϴ�.", true); 

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
		/// �ǳ�ǥ�� ����
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
		/// �ǳ� label text����...
		/// </summary>
		/// <param name="strText"></param>
		protected void lblPnl_Text(string strText)
		{
			form.control.Invoke_Control_Text(lblPnl, strText);
		}


		/// <summary>
		/// form ���� ó��
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
        /// �޽��� â�� ������ ���� �ش�.
        /// </summary>
        /// <param name="isError">���� ����</param>
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
			if (!isInitilalize && Fnc.ShowMsg(this, "���α׷��� ���� �Ͻðڽ��ϱ�?", string.Empty, frmMessage.enMessageType.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

			if (clsRfid != null)
			{
				//clsRfid.UnSubScribe();
				clsRfid.Dispose();
			}

			
			if (!isInitilalize) SetMessage(false, "���α׷��� ���� �մϴ�.", true);
        }

		private void frmBaseForm_SizeChanged(object sender, EventArgs e)
		{
#if(!DEBUG)          
			//this.WindowState = FormWindowState.Maximized;
#endif
		}

	
		
    }
}