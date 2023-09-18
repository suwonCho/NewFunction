using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Input;
using System.Reflection;
using System.Windows.Controls;
//using System.Reflection;

namespace function.wpf
{
    public class baseWindow : Window
    {
        Window _parent = null;
        internal Window parent
        {
            get { return _parent; }
            set
            {
                _parent = value;

                baseWindow b = _parent as baseWindow;

                if (b == null) return;

                b.childWindow = this;


            }
        }

        baseWindow childWindow = null;

        /// <summary>
        /// 프로그램 중복 실행 체크를 한다.
        /// </summary>
        public bool isPgmRunCheck { get; set; } = false;


        static function.Setting _setting = null;
        /// <summary>
        /// 셑팅설정  클라스
        /// </summary>
        public static function.Setting Setting
        {
            get { return _setting; }
            set
            {
                _setting = value;
            }
        }


        /// <summary>
        /// 프로퍼티를 저장할 컨트롤 이름(대소문자 구분, ';'로 컨트롤별 구분)<para/>
        /// 지원 컨트롤:DataGrid(컬럼너비),Rowdefinition(높이),ColumnDefinition,ListView(컬럼들 너비)<para/>        
        /// </summary>
        [Description(@"컨트롤 설정을 저장할 이름들을 가져오거나 설정한다.")]
        public string SaveSettingControlName
        {
            get; set;
        } = "";

        /// <summary>
		/// 프로그램 종료 확인창 해더 텍스트
		/// </summary>
		[Description("프로그램 종료 확인창 해더 텍스트")]
        public string QuitPrompt_Head
        { get; set; } = "종료확인";


        /// <summary>
        /// 프로그램 종료 확인창 텍스트
        /// </summary>
        [Description("프로그램 종료 확인창 텍스트")]
        public string QuitPrompt_Text
        { get; set; } = "프로그램을 종료 하시겠습니까?";

        public wucStatusBar _statusBar = null;

        /// <summary>
        /// 상태 표시 할 창을 설정한다.
        /// </summary>
        public wucStatusBar StatusBar
        {
            get { return _statusBar; }
            set { _statusBar = value; }
        }

        /// <summary>
		/// 로그 클래스 -> 사용 폼에서 객체 생성을 해주어야 한다.
		/// </summary>
		public function.Util.Log clsLog = null;


        enCloseWindowToType _closeWindowToType = enCloseWindowToType.None;

        /// <summary>
        /// 폼 닫을 경우 처리 방법을 가져오거나 설정합니다.
        /// </summary>
        [Description("윈도우 닫을 경우 처리 방법을 가져오거나 설정합니다.")]
        public enCloseWindowToType CloseWindowToType
        {
            get { return _closeWindowToType; }
            set
            {
                _closeWindowToType = value;
            }
        }


        [Description("윈도우 타이틀에 버전 표시 여부를 가져오거나 설정합니다.")]
        public bool ShowVersionInTitle
        {
            get; set;
        }

        /// <summary>
        /// 윈도우 전체 화면일때 처리 하는 방법
        /// </summary>
        [Description("윈도우 전체 화면일때 처리 하는 방법")]
        public enWindowStateMaximizedMode WindowStateMaximizedMode { get; set; } = enWindowStateMaximizedMode.Normal;
        


        /// <summary>
        /// 윈도우 닫임 여부 처리여부.(윈도우 닫을 때 확인 창을 안띄움)
        /// </summary>
        public bool CloseWindow = false;


        /// <summary>
        /// 프로그램 이름
        /// </summary>
        string pgmName = string.Empty;


        /// <summary>
        /// 프로그램 이름을 설정 하거나 가저 옵니다.
        /// </summary>
        [Description("프로그램 이름을 설정 하거나 가저 옵니다.")]
        public string PgmName
        {
            get
            {
                return pgmName;
            }
            set
            {
                pgmName = value;
            }
        }


        private bool _isUseFormInit = false;
        /// <summary>
        /// 폼 로드 시 타이머로 Form_Init 사용 여부를 가져오거나 설정한다. Form_Init를 override하여 사용할것
        /// </summary>
        [Description("폼 로드 시 타이머로 Form_Init 사용 여부를 가져오거나 설정한다. Form_Init를 override하여 사용할것")]
        public bool isUseFormInit
        {
            get { return _isUseFormInit; }
            set
            {
                _isUseFormInit = value;
            }
        }

        DispatcherTimer tmrInit;



        public baseWindow()
        {
            this.Loaded += BaseWindow_Loaded;
            this.Closing += BaseWindow_Closing;
            this.Closed += BaseWindow_Closed;
            this.PreviewKeyDown += BaseWindow_PreviewKeyDown;
            this.Activated += BaseWindow_Activated;
            this.StateChanged += BaseWindow_StateChanged;
        }

        bool? stTopMost = null;
        ResizeMode? stResizeMode = null;

        private void BaseWindow_StateChanged(object sender, EventArgs e)
        {
            if(WindowState == WindowState.Maximized)
            {
                switch(WindowStateMaximizedMode)
                {
                    case enWindowStateMaximizedMode.NoAllow:
                        this.WindowState = WindowState.Normal;
                        break;

                    case enWindowStateMaximizedMode.OverTaskbar:
                        Visibility = Visibility.Collapsed;
                        stTopMost = Topmost;
                        stResizeMode = ResizeMode;

                        Topmost = true;
                        ResizeMode = ResizeMode.NoResize;
                        Visibility = Visibility.Visible;

                        //ResizeMode = (ResizeMode)stResizeMode;
                        break;
                }
            }
            else
            {
                if (stTopMost != null) Topmost = (bool)stTopMost;
                if (stResizeMode != null) ResizeMode = (ResizeMode)stResizeMode;
            }
        }

        private void BaseWindow_Activated(object sender, EventArgs e)
        {
            if (childWindow == null) return;

            //자식 폼이 있으면 자식폼에 포커스를 해준다.
            if (childWindow.WindowState == WindowState.Minimized)
            {
                childWindow.WindowState = WindowState.Normal;                
            }

            childWindow.Focus();
        }

        private void BaseWindow_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if ( (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && (Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt && e.Key == Key.D0)
            {
                this.WindowState = WindowState.Normal;
                this.Left = 0;
                this.Top = 0;
            }
        }

        private void BaseWindow_Closing(object sender, CancelEventArgs e)
        {
            if (CloseWindow)  return;

            switch (CloseWindowToType)
            {
                case enCloseWindowToType.Prompt:
                case enCloseWindowToType.PromptYN:
                    //종료 확인을 한다.
                    if(MessageBox.Show(this, QuitPrompt_Text, QuitPrompt_Head,  MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) e.Cancel = true;
                    break;
            }
        }

        private void BaseWindow_Closed(object sender, EventArgs e)
        {
            baseWindow b = _parent as baseWindow;

            if (b != null)
            {
                b.childWindow = null;
            }

            if (clsLog != null) clsLog.WLog(pgmName + "을(를) 종료 합니다.");


            if(Notifyicon != null)
            {
                Notifyicon.Dispose();
            }

            formPostion_Save();

            SaveSettingControl_Save();
        }

        /// <summary>
        /// 윈도우 시작 절차 [0]시작 [10]form_init [99]form_init_Fin
        /// </summary>
        int iWinProc = 0;

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //포지션이나 컨트롤 저장 하는 경우에 xml이 설정 안되면 만들어 준다.
            if (SavePosition || SaveSettingControlName != string.Empty)
            {
                if (Setting == null)
                    _setting = new Setting(AppDomain.CurrentDomain.BaseDirectory + "\\WindowSetting.xml");

                _savePosition_PropertyName = this.GetType().Name + "_WINDOWPOSITION";
            }

            formPositon_Load();

            SaveSettingControl_Load();

            PostionSet();

            if (isUseFormInit)
            {
                wp = winPlzWait.Load(this, "프로그램 초기화 중입니다.\r\n잠시만 기다려 주세요.");
                //0.1초 후 폼 초기화를 처리 한다.
                tmrInit = new DispatcherTimer();
                tmrInit.Interval = TimeSpan.FromMilliseconds(100);
                tmrInit.Tick += TmrInit_Tick;
                tmrInit.Start();
            }

            if(ShowVersionInTitle)
            {
#if (DEBUG)
                this.Title = $"[DEBUG]{this.Title}"; //  v.{System.Windows.Forms.Application.ProductVersion}";                        
#endif

                this.Title += "  v." + System.Windows.Forms.Application.ProductVersion;
            }



        }


        /// <summary>
        /// 자식 윈도우일 경우 부모창의 센터에 위치 하여 준다.
        /// </summary>
        public void PostionSet()
        {

            //ShowDialog일경우 부모의 가운데로 이동
            if (parent != null)
            {
                if (parent.WindowState == WindowState.Maximized)
                {
                    Left = (parent.ActualWidth / 2) - (this.ActualWidth / 2);
                    Top = (parent.ActualHeight / 2) - (this.ActualHeight / 2);
                }
                else
                {
                    Left = parent.Left + (parent.ActualWidth / 2) - (this.ActualWidth / 2);
                    Top = parent.Top + (parent.ActualHeight / 2) - (this.ActualHeight / 2);
                }
            }

        }


        winPlzWait wp = null;

        Thread thFormInit;

        private void TmrInit_Tick(object sender, EventArgs e)
        {            
            try
            {

                //윈도우 시작 절차 [0]시작 [10]form_init [99]form_init_Fin
                switch (iWinProc)
                {
                    case 0:
#if (!DEBUG)
                        //프로그램 중복 체크
                        if (isPgmRunCheck && !function.wpf.wpfFnc.ProgramRunCheck(PgmName))
                        {
                            winPlzWait.UnLoad(wp);

                            function.wpf.wpfMsgBox f = new wpfMsgBox("프로그램 종료", "이미 프로그램이 실행 중입니다. 프로그램을 종료 합니다.", false);
                            f.ShowDialog(this);

                            CloseWindow = true;
                            this.Close();
                        }
#endif

                        thFormInit = new Thread(new ThreadStart(Form_Init));
                        thFormInit.IsBackground = true;
                        thFormInit.Start();

                        iWinProc = 10;
                        break;

                    case 10:
                        //작업이 종료되면 완료 처리할 수 있도록
                        if (!thFormInit.IsAlive) iWinProc = 99;
                        break;

                    case 99:
                        winPlzWait.UnLoad(wp);
                        tmrInit.Stop();
                        Form_InitFin();
                        break;
                }
                
            }
            catch
            {

            }
            finally
            {                
                //wucPlzWait.UnLoad(wp);
                
            }

        }

        /// <summary>
        /// 폼 초기화 작업을 한다. - 폼로드후 처리 함으로 메인화면 표시 후 작업을 처리 한다. 쓰레드 처리 함으로 여기서는 화면작업은 하지 말것, 화면 작업은 Form_InitFin override 하여 구현
        /// </summary>
        protected virtual void Form_Init()
        {

        }

        /// <summary>
        /// 폼 초기화 작업 후 실행 한다.
        /// </summary>
        protected virtual void Form_InitFin()
        {

        }


        public bool? ShowDialog(Window parent)
        {
            _parent = parent;
            return base.ShowDialog();
        }

        public void Show(Window parent)
        {
            _parent = parent;
            Show();
        }



        #region 윈도우 위치/크기 저장 처리부

        readonly static string _xmlSave_GroupName = "WINDOW_SAVE";
        string _savePosition_PropertyName = "BF_WINDOWPOSITION";

        bool _saveposition = false;

        /// <summary>
        /// 윈도우 위치 저장 여부
        /// 각 프로그램에서 Setting.setting 파일을 만든다<para/>
        /// 아래와 같은 항목을 추가 한다 <para/>
        /// 프로퍼티 클래스에 BF_FORMPOSITION항목 추가한다(형식은 STRING) 
        /// </summary>
        [Description(@"윈도우 위치 저장 여부
각 프로그램에서 Setting.setting 파일을 만든다. 아래와 같은 항목을 추가 한다. 프로퍼티 사용시 프로퍼티 클래스에 BF_FORMPOSITION항목 추가한다(형식은 STRING)")]
        public bool SavePosition
        {
            get { return _saveposition; }
            set
            {
                _saveposition = value;
            }
        }

        private void formPositon_Load()
        {
            if (!_saveposition) return;


            try
            {
                string value;

                //폼내용은 '/*폼이름:Top, Left, Width, Height, WindowState*/' 구성
                function.Setting s = Setting;

                if (s == null)
                    return;
                else
                {
                    s.Group_Select(_xmlSave_GroupName);
                    value = s.Value_Get(_savePosition_PropertyName.ToUpper(), string.Empty);
                }


                value = fncWin.getSettingString(value, Name);

                //컨트롤정보는 '컨트롤이름:top:left:너비:높이;' 로구분
                string[] cValue = value.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                double Top = Fnc.obj2Double(cValue[0]);
                double Left = Fnc.obj2Double(cValue[1]);
                double Width = Fnc.obj2Double(cValue[2]);
                double Height = Fnc.obj2Double(cValue[3]);
                

                WindowState WindowState = (WindowState)Fnc.obj2int(cValue[4]);

                if (Width > 100 && Height > 100)
                {
                    this.Width = Width < 10 ? this.Width : Width;
                    this.Height = Height < 10 ? this.Height : Height;
                }

                this.Top = Top < 10 ? 500 : Top;
                this.Left = Left < 10 ? 500 : Left;

                this.WindowState = WindowState;
            }
            catch
            {
            }

        }

        /// <summary>
        /// 폼 위치 정보를 저장한다.
        /// </summary>
        protected void formPostion_Save()
        {
            //마지막 창정보를 저장한다. 
            if (!_saveposition) return;

            try
            {
                string value;
                function.Setting xml = Setting;

                //폼내용은 '/*폼이름:Top, Left, Width, Height, WindowState*/' 구성
                if (xml == null)
                    return;
                else
                {
                    xml.Group_Select(_xmlSave_GroupName);
                    value = xml.Value_Get(_savePosition_PropertyName, string.Empty);
                }
                                

                string winNM = string.Format("/*{0}:", Name.ToUpper());

                value = fncWin.recvSettingString(value, Name);

                value += winNM;

                WindowState state = WindowState;
                if (this.WindowState != WindowState.Normal)
                {
                    this.WindowState = WindowState.Normal;                                      
                }

                value += string.Format("{0}:{1}:{2}:{3}:{4}", Top, Left, Width, Height, (int)state);

                value += "*/";

                if (xml != null)
                {
                    xml.Group_Select(_xmlSave_GroupName);
                    xml.Value_Set(_xmlSave_GroupName, _savePosition_PropertyName, value);
                    xml.Setting_Save();
                }

            }
            catch
            {
            }

        }


        readonly static string _xmlSaveControl_GroupName = "CONTROLSETTING_SAVE";


        /// <summary>
        /// 컨트롤 설정을 저장한다.
        /// </summary>
        protected void SaveSettingControl_Save()
        {
            if (SaveSettingControlName == string.Empty) return;

            try
            {   
                function.Setting xml = Setting;

                //폼내용은 '/*폼이름:Top, Left, Width, Height, WindowState*/' 구성
                if (xml == null)
                    return;
                else
                {
                    xml.Group_Select(_xmlSaveControl_GroupName);                    
                }

                string[] pp = SaveSettingControlName.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                object obj;
                string nm;
                string valueName;
                string value;

                foreach(string p in pp)
                {
                    try
                    {
                        obj = this.FindName(p);

                        if (obj == null) continue;
                        nm = obj.GetType().ToString();
                        valueName = $"{_savePosition_PropertyName}.{p}";
                        value = "";

                        switch (nm)
                        {
                            case "System.Windows.Controls.RowDefinition":
                                GridLength gl = (GridLength)function.DFnc.Property_Get_Value(obj, "Height");
                                if (gl != null)
                                {
                                    //GridUnitType;Value
                                    value = $"{gl.GridUnitType};{gl.Value}";
                                }
                                break;


                            case "System.Windows.Controls.ColumnDefinition":
                                GridLength gw = (GridLength)function.DFnc.Property_Get_Value(obj, "Width");
                                if (gw != null)
                                {
                                    //GridUnitType;Value
                                    value = $"{gw.GridUnitType};{gw.Value}";
                                }                                
                                break;

                            case "System.Windows.Controls.DataGrid":
                                DataGrid dg = (DataGrid)obj;
                                if (dg == null) return;

                                foreach(DataGridColumn c in dg.Columns)
                                {
                                    value = Fnc.StringAdd(value, $"{c.Width.UnitType}:{c.Width.Value}", ";");
                                }
                                break;
                        }

                        xml.Value_Set(_xmlSaveControl_GroupName, valueName, value, "", "", "");
                    }
                    catch { }
                }


                if (xml != null)
                {                 
                    xml.Setting_Save();
                }


            }
            catch
            {

            }
        }

        /// <summary>
        /// 컨트롤 설정을 반영 한다.
        /// </summary>
        protected void SaveSettingControl_Load()
        {
            if (SaveSettingControlName == string.Empty) return;

            try
            {
                function.Setting xml = Setting;

                //폼내용은 '/*폼이름:Top, Left, Width, Height, WindowState*/' 구성
                if (xml == null)
                    return;
                else
                {
                    xml.Group_Select(_xmlSaveControl_GroupName);
                }

                string[] pp = SaveSettingControlName.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                object obj;
                string nm;
                string valueName;
                string value;
                string[] vals;
                string[] vs;
                int idx;
                double dv;
                GridUnitType gt;

                foreach (string p in pp)
                {
                    try
                    {
                        obj = this.FindName(p);

                        if (obj == null) continue;
                        nm = obj.GetType().ToString();
                        valueName = $"{_savePosition_PropertyName}.{p}";
                        value = xml.Value_Get(_xmlSaveControl_GroupName, valueName, enSettingValueType.Value);                        

                        if (value == "") continue;
                        vals = value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                        switch (nm)
                        {
                            case "System.Windows.Controls.RowDefinition":
                                if (vals.Length != 2) continue;
                                
                                RowDefinition rd = (RowDefinition)obj;
                                if (rd != null)
                                {
                                    gt = (GridUnitType)Fnc.String2Enum(new GridUnitType(), vals[0]);
                                    dv = Fnc.obj2Double(vals[1]);

                                    rd.Height = new GridLength(dv, gt);
                                }
                                break;

                            case "System.Windows.Controls.ColumnDefinition":
                                if (vals.Length != 2) continue;

                                ColumnDefinition cd = (ColumnDefinition)obj;
                                if (cd != null)
                                {
                                    gt = (GridUnitType)Fnc.String2Enum(new GridUnitType(), vals[0]);
                                    dv = Fnc.obj2Double(vals[1]);

                                    cd.Width = new GridLength(dv, gt);
                                }
                                break;

                            case "System.Windows.Controls.DataGrid":
                                DataGrid dg = (DataGrid)obj;
                                if (dg == null) return;
                                idx = 0;
                                DataGridLength dgl;
                                DataGridLengthUnitType dlt;

                                foreach (DataGridColumn c in dg.Columns)
                                {
                                    if (vals.Length <= idx) break;

                                    vs = vals[idx].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                                    if (vs.Length == 2)
                                    {
                                        dlt = (DataGridLengthUnitType)Fnc.String2Enum(new DataGridLengthUnitType(), vs[0]);
                                        dv = Fnc.obj2Double(vs[1]);

                                        dgl = new DataGridLength(dv, dlt);
                                        c.Width = dgl;
                                    }
                                    //c.Width = 
                                    idx++;
                                }
                                break;
                        }
                        
                    }
                    catch { }
                }
                
            }
            catch
            {

            }
        }





        #endregion



        /// <summary>
        /// 에러 처리를 한다. 로그기록 및 상태표시 창...
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="strMethodName"></param>
        /// <param name="showMessageBox">메세지 PopUp 표시 여부</param>
        public virtual void ProcException(Exception ex, string strMethodName, bool showMessageBox = false)
        {
            if (clsLog != null) clsLog.WLog_Exception(strMethodName, ex);

            string msg;

            if (ex.Message.Equals(string.Empty))
            {
                msg = ex.InnerException.Message;
            }
            else
            {
                msg = ex.Message;
            }


            if (showMessageBox)
            {
                MessageBox.Show(this, msg, "오류발생", MessageBoxButton.OK, MessageBoxImage.Error);                
            }

            SetMessage(true, msg, false);

            Console.WriteLine(string.Format("[{0}]{1}", msg, ex.ToString()));
        }



        /// <summary>
        /// 메시지 창을 클리어 한다.
        /// </summary>
        public virtual void SetMessage_Clear()
        {
            SetMessage(false, string.Empty, false);
        }


        /// <summary>
        /// 메시지 창에 내용을 보여 준다.
        /// </summary>
        /// <param name="isError">에러 여부</param>
        /// <param name="strMessage"></param>
        /// <param name="isLog"></param>
        public virtual void SetMessage(bool isError, string strMessage, bool isLog)
        {
            SetMessage(isError ? enMsgType.Error : enMsgType.OK, strMessage, isLog);
        }
       

        /// <summary>
        /// 메시지 창에 내용을 보여 준다.(메시지 타입 사용)
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="strMessage"></param>
        /// <param name="isLog"></param>
        protected virtual void SetMessage(enMsgType msgType, string strMessage, bool isLog)
        {
            if (this.StatusBar != null)
            {
                this.StatusBar.SetMessage(msgType, strMessage);
            }

            if (clsLog != null && isLog)
                clsLog.WLog(strMessage);
        }



        /// <summary>
        /// 상태바에 값을 변경한다.
        /// </summary>
        /// <param name="NowValue"></param>
        public void ProgressBar_Value(double NowValue)
        {
            this.StatusBar.ProgressBar_Value(NowValue);
        }

        /// <summary>
        /// 최대값을 설정한다.
        /// </summary>
        /// <param name="MaxValue"></param>
        public void ProgressBar_MaxValue(double MaxValue)
        {
            this.StatusBar.ProgressBar_MaxValue(MaxValue);
        }

        /// <summary>
        /// 최소값을 설정한다.
        /// </summary>
        /// <param name="MinValue"></param>
        protected void ProgressBar_MinValue(double MinValue)
        {
            this.StatusBar.ProgressBar_MinValue(MinValue);
        }


        System.Windows.Forms.NotifyIcon notifyIcon;

        /// <summary>
        /// NotifyIcon 설정 하가나 가져 옵니다.사용 하려면 System.Windows.Forms 참조 추가 해야 함<para/>
        /// Notifyicon = new System.Windows.Forms.NotifyIcon();                <para/>
        /// Notifyicon.Icon = Properties.Resources.FillingStamp;        //잘됨 <para/>               
        /// Notifyicon.Text = "Test";   <para/>
        /// Notifyicon.Visible = true;  <para/>
		/// </summary>
		[Description("NotifyIcon 설정 하가나 가져 옵니다. 사용 하려면 System.Windows.Forms 참조 추가 해야 함")]
        public System.Windows.Forms.NotifyIcon Notifyicon
        {
            get
            {
                return notifyIcon;
            }
            set
            {
                notifyIcon = value;
            }

        }



    }   //end class
}
