using function.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace function.wpf
{
    /// <summary>
    /// DBSetting.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DBSetting : baseWindow
    {
        public MsSQL.strConnect sql = new MsSQL.strConnect();
#if (!NO_ORACLE)
        public OracleDB.strConnect ora = new OracleDB.strConnect();
#endif 
        public function.Db.enDBType dbType = enDBType.None;

        bool isInit = false;

        string tIP_Inp = "IP를 입력 하여 주세요";
        string tTns_Inp = "TNS를 입력 하여 주세요";

        string tAuth_Sql = "Sql Server 인증";
        string tAuth_Win = "Windows 인증";
        string tID_Inp = "ID를 입력 하여 주세요";
        string tID_Pass = "암호를 입력 하여 주세요";
        string tDBType_Sel = "DB타입을 선택 하여 주세요.";

        string tDB_Searching = "DB 목록 조회중...";

        #region properties

        /// <summary>
        /// DB타입 변경 가능여부, 폼 로드전에만 변경 가능
        /// </summary>
        [Description("DB타입 변경 가능여부")]
        public bool DbType_Changable { get; set; } = true;



        #endregion


        public DBSetting()
        {
            InitializeComponent();            
        }

        

        public DBSetting(MsSQL.strConnect _sql, bool isEng = false) : this()
        {
            if (isEng) EngSet();

            dbType = enDBType.MsSQL;
            sql = _sql;
            
        }

#if (!NO_ORACLE)
        public DBSetting(OracleDB.strConnect _orc, bool isEng = false) : this()
        {
            if (isEng) EngSet();

            dbType = enDBType.Oracle;
            ora = _orc;            
        }
#endif

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.StatusBar = stBar;
            lblBDSelect.cmbBox.DropDownOpened += CmbBox_DropDownOpened;

            Form_Init();
        }


        /// <summary>
        /// 영문으로 변경
        /// </summary>
        public void EngSet()
        {
            this.Title = "Database Setting";

            
            lblBDType.Label_Content = "DB Type";
            lblAuthType.Label_Content = "Sql Auth.";
            lblPass.Label_Content = "Pass.";
            lblBDSelect.Label_Content = "DB";
            btnSave.Text = "  Save";
            btnCancel.Text = "  Cancel";


            tIP_Inp = "Input IP.";
            tTns_Inp = "Input TNS.";

            tAuth_Sql = "Sql Server Auth.";
            tAuth_Win = "Windows Auth.";
            tID_Inp = "Input ID.";
            tID_Pass = "Input Passwords.";
            tDBType_Sel = "Select DBType.";

            //inpAuthType.ComboBoxItems.Clear();
            //inpAuthType.ComboBoxItems.Add(tAuth_Sql);
            //inpAuthType.ComboBoxItems.Add(tAuth_Win);

            tDB_Searching = "Searching DB List...";
        }


        public void Form_Init()
        {
            try
            {
                isInit = true;


                lblAuthType.ComboBox_Items.Clear();

                lblAuthType.ComboBox_Items.Add(tAuth_Sql);
                lblAuthType.ComboBox_Items.Add(tAuth_Win);
                
                

                switch (dbType)
                {
                    case enDBType.Oracle:
#if (!NO_ORACLE)
                        lblBDType.Value = "ORACLE";
                        lblIP.Value = ora.strTNS;
                        lblID.Value = ora.strID;
                        lblPass.Value = ora.strPass;
#endif
                        break;

                    case enDBType.MsSQL:
                        lblBDType.Value = "MS-SQL";
                        lblIP.Value = sql.strIP;
                        lblID.Value = sql.strID;
                        lblPass.Value = sql.strPass;
                        string db = sql.strDataBase;
                        lblAuthType.Value = (sql.strID == string.Empty) ?  tAuth_Win : tAuth_Sql;
                        CmbBox_DropDownOpened(null, null);
                        lblBDSelect.Value = db;
                        break;
                }
            }
            catch(Exception ex)
            {
            }
            finally
            {
                isInit = false;
            }

        }

        private void CmbBox_DropDownOpened(object sender, EventArgs e)
        {
            lblBDSelect.cmbBox.ItemsSource = null;

            if (dbType != enDBType.MsSQL)
            {
                return;
            }

            set_ConnString();

            if (lblIP.Text.Trim() == string.Empty) return;

            try
            {
                SetMessage(false, tDB_Searching, false);
                

                DataTable dt = MsSQL.DBListGet(sql);

                lblBDSelect.cmbBox.DisplayMemberPath = "NAME";
                lblBDSelect.cmbBox.SelectedValuePath = "NAME";
                lblBDSelect.cmbBox.ItemsSource = dt.DefaultView;

                SetMessage(false, "", false);
            }
            catch (Exception ex)
            {
                if (ex.Message.Trim().Equals(string.Empty))
                    SetMessage(true, ex.InnerException.Message, false);
                else
                    SetMessage(true, ex.Message, false);
            }
        }


        private void set_ConnString()
        {

            if (lblIP.Text.Trim().Equals(string.Empty))
            {
                if (dbType == enDBType.Oracle)
                    SetMessage(true, tIP_Inp, false);
                else
                    SetMessage(true, tTns_Inp, false);
            }

            bool auth = lblAuthType.Text.Equals(tAuth_Sql);

            if (auth)
            {
                if (lblIP.Text.Trim().Equals(string.Empty)) SetMessage(true, tID_Inp, false);

                if (lblPass.Text.Trim().Equals(string.Empty)) SetMessage(true, tID_Pass, false);
            }

            switch (dbType)
            {
                case enDBType.Oracle:
#if (!NO_ORACLE)
                    ora.strTNS = lblIP.Text.Trim();
                    ora.strID = lblID.Text.Trim();
                    ora.strPass = lblPass.Text.Trim();
#endif
                    break;

                case enDBType.MsSQL:
                    sql.strIP = lblIP.Text.Trim();
                    if (auth)
                    {
                        sql.strID = lblID.Text.Trim();
                        sql.strPass = lblPass.Text.Trim();
                    }
                    else
                    {
                        sql.strID = string.Empty;
                        sql.strPass = string.Empty;
                    }


                    sql.strDataBase = lblBDSelect.Text.Trim();
                    break;

                default:
                    SetMessage(true, tDBType_Sel, false);
                    break;

            }

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {            
            set_ConnString();           

            this.DialogResult = true;
            Close();
        }

        private void DBSetting_Load(object sender, EventArgs e)
        {
            //Form_Init();
            lblBDSelect.IsEnabled = DbType_Changable;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {           
            Close();
        }

        private void LblAuthType_Text_Changed(object sender, usrEventArgs e)
        {
            bool value;
            if (e.NewValue == null)
            {
                value = lblAuthType.Text.Equals(tAuth_Sql);
            }
            else
            {
                value = e.NewValue.Equals(tAuth_Sql);
            }

            lblID.IsEnabled = value;
            lblPass.IsEnabled = value;
        }

       
    }   //end class
}
