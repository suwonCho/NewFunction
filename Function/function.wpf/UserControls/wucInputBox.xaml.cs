using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace function.wpf
{
    /// <summary>
    /// wucInputBox.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class wucInputBox : UserControl
    {
        public object Tag1 { get; set; }
        public object Tag2 { get; set; }

        /// <summary>
        /// MultiTextItem 항목 분할자 (char)3
        /// </summary>
        public char MultiTextItemSeperator = (char)3;

        public enum enInputType
        {
            LABEL,
            TEXTBOX,
            MultiLineTEXTBOX,
            Password,
            /// <summary>
            /// 텍스트 박스 선택 시 여러 줄을 입력 할 수 있는 콤보 박스 표시
            /// </summary>
            MultiTextItem,
            COMBO,
            FileSelect,
            FolderSelect,
            ColorSelect,
            BrushesSelect,
            DatePicker,
            DateTimePicker

        }

        public enum enBlinkType
        {
            None,
            Always,
            Focused
        }

        private event usrEventHander _text_Changed = null;

        /// <summary>
        /// 텍스트가 변경 되었을경우 발행 하는 이벤트
        /// </summary>
        public event usrEventHander Text_Changed
        {
            add
            {
                _text_Changed += value;
            }
            remove
            {
                _text_Changed -= value;
            }
        }


        public TextBox txtBox = new TextBox();
        public Label dLabel = new Label();
        public ComboBox cmbBox = new ComboBox();
        public ComboBoxX cmbMulti = new ComboBoxX();
        public PasswordBox passBox = new PasswordBox();
        public ComboBoxBrush cmbBrushes = null;

        DateTime DateValue = DateTime.Now;
        DateTime DateText = DateTime.Now;

        Popup popDate = null;
        function.wpf.DatePicker datePicker;

        /// <summary>
        /// 텍스트 변경 여부
        /// </summary>
        public bool isChange = false;

        
        Object _win = null;

        /// <summary>
        /// 부모 창 or 컨트롤 - MultiTextItem에서 사용
        /// </summary>
        public Object Win
        {
            get
            {
                if (_win == null) _win = wpfFnc.ParentWindowGet(this);

                return _win;
            }
            set
            {
                _win = value;
            }
        }


        /// <summary>
        /// 부모 창 or 컨트롤에서 메인 그리드를 찾는다 - MultiTextItem에서 사용
        /// </summary>
        /// <returns></returns>
        private Grid WinGetGrid()
        {
            Type ty = Win.GetType();

            PropertyInfo pi = ty.GetProperty("Content");

            if (pi == null) return null;

            Grid rtn = pi.GetValue(Win, null) as Grid;

            return rtn;
        }
        
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(wucInputBox),
            new FrameworkPropertyMetadata((object)null, ValueChangeCallback));

        /// <summary>
        /// 원본 값
        /// </summary>
        public string Value
        {
            get { return GetValue(ValueProperty) == null ? null  : (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private void valueChaged()
        {
            string value = Fnc.obj2String(Value);

            switch (InputType)
            {
                case enInputType.ColorSelect:
                    BrushSelect_Brush = wpfFnc.String2Brush(value);
                    dLabel.Background = BrushSelect_Brush;
                    
                    break;

                case enInputType.MultiLineTEXTBOX:
                    MultiTextItemSetItem(value);
                    isChange = false;
                    Check_TextChanged(value);
                    break;

                case enInputType.Password:
                    passBox.Password = value;
                    isChange = false;
                    break;

                case enInputType.DatePicker:
                    try
                    {
                        DateValue = Fnc.String2Date(value, Fnc.enDateType.Date);
                        DateText = DateValue;
                    }
                    catch
                    {
                        DateTime n = DateTime.Now;
                        DateValue = new DateTime(n.Year, n.Month, n.Day);
                        DateText = DateValue;
                        Value = Fnc.Date2String(DateValue, Fnc.enDateType.Date);
                    }
                    dLabel.Content = value;
                    break;

                case enInputType.DateTimePicker:
                    try
                    {
                        DateValue = Fnc.String2Date(value, Fnc.enDateType.DateTime);
                        DateText = DateValue;
                    }
                    catch
                    {
                        DateValue = DateTime.Now;
                        DateText = DateValue;
                        Value = Fnc.Date2String(DateValue, Fnc.enDateType.DateTime);
                    }
                    dLabel.Content = value;
                    break;

                case enInputType.BrushesSelect:
                    cmbBrushes.BrushName = value;

                    if (Value != cmbBrushes.BrushName)
                    {
                        Value = cmbBrushes.BrushName;
                        return;
                    }
                    break;

                default:
                    txtBox.Text = value;
                    dLabel.Content = value;

                    if (cmbBox.ItemsSource == null)
                    {
                        cmbBox.Text = value;
                        if (!cmbBox.Text.Equals(value)) cmbBox.SelectedIndex = -1;

                    }
                    else
                    {
                        //control.Invoke_ComboBox_SelectedItem(cmbBox, ComboBoxValueMember, v);
                        //control_Dispatcher.InvokeComboBox_SelectedItem(Dispatcher, cmbBox, ComboBoxValueMember, v);
                        control_Dispatcher.ComboBox_SelectedItem(cmbBox, cmbBox.SelectedValuePath, value);
                    }

                    Text = value;
                    //dLabel.Content = string.Empty;
                    isChange = false;
                    Check_TextChanged(value);
                    break;
            }
        }


        public new void Focus()
        {
            switch (InputType)
            {
                case enInputType.COMBO:
                    cmbBox.Focus();
                    break;

                case enInputType.ColorSelect:
                case enInputType.LABEL:
                case enInputType.DateTimePicker:
                case enInputType.DatePicker:
                    dLabel.Focus();
                    break;

                case enInputType.BrushesSelect:
                    cmbBrushes.Focus();
                    break;

                case enInputType.Password:
                    passBox.Focus();
                    break;

                default:
                    txtBox.Focus();
                    break;
            }
        }


        static void ValueChangeCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            wucInputBox w = o as wucInputBox;
            w.valueChaged();
        }


        /// <summary>
        /// 문자열을 이용하여 MultiTextItem 콤보 박스를 채운다.
        /// </summary>
        /// <param name="value"></param>
        private void MultiTextItemSetItem(string value)
        {
            string[] vv = value.Split(new char[] { MultiTextItemSeperator });

            cmbMulti.Items.Clear();

            foreach(string ss in vv)
            {
                MultiTextItemAddItem(ss);
            }
                        
        }

        private void MultiTextItemAddItem(string value)
        {
            if (cmbMulti.Items.Contains(value)) return;

            cmbMulti.Items.Add(value);
        }


        /// <summary>
        /// MultiTextItem 콤보 박스의 아이템을 스트링으로 반환한다. MultiTextItemSeperator를 구분자로 사용
        /// </summary>
        /// <returns></returns>
        private string MultiTextItemGetValue()
        {
            string rtn = string.Empty;

            foreach(object o in cmbMulti.Items)
            {
                rtn = Fnc.StringAdd(rtn, o.ToString(), MultiTextItemSeperator.ToString());
            }

            return rtn;
        }


        /// <summary>
        /// MultiTextItem의 값을 문자 배열로 리턴한다. 
        /// </summary>
        /// <returns></returns>
        public string[] MultiTextItemGetValues()
        {
            //if (InputType != enInputType.MultiTextItem) return null;

            string[] rtn = Text.Split(new char[] { MultiTextItemSeperator });

            return rtn;

        }
        

        /// <summary>
        /// 표시 텍스트
        /// </summary>
        [Description("표시 텍스트")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        public string Text
        {
            get
            {
                string rtn;
                switch (InputType)
                {
                    case enInputType.COMBO:
                        
                        if (cmbBox.ItemsSource == null)
                            rtn = cmbBox.Text;
                        else
                        {

                            if (cmbBox.SelectedIndex < 0)
                            {
                                rtn = string.Empty;
                            }
                            else
                            {
                                string tp = cmbBox.SelectedItem.GetType().ToString();



                                switch (tp)
                                {
                                    case "System.Data.DataRowView":
                                        rtn = Fnc.obj2String(((System.Data.DataRowView)cmbBox.SelectedItem)[cmbBox.SelectedValuePath]);
                                        break;

                                    default:
                                        rtn = cmbBox.Text;
                                        break;
                                }
                            }

                        }
                        
                        //else if (cmbBox.DataSource == null)
                        //    rtn = Fnc.obj2String(cmbBox.SelectedItem);
                        //else
                        //{
                        //    if (ComboBoxValueMember == null)
                        //    {
                        //        console.WriteLine("usrInputBox[Name]{0} [프로퍼티]ComboBoxValueMember가 설정 되어 있지 않아 TEXT 속성을 가져 올 수 없습니다.", this.Name);
                        //        rtn = null;

                        //    }
                        //    else
                        //        rtn = control.Invoke_ComboBox_GetSelectValue(cmbBox, ComboBoxValueMember);
                        //}
                        break;

                    case enInputType.Password:
                        rtn = passBox.Password;
                        break;

                    case enInputType.DatePicker:
                    case enInputType.DateTimePicker:
                    case enInputType.LABEL:                    
                        rtn = (string)dLabel.Content;
                        break;

                    case enInputType.MultiLineTEXTBOX:
                        rtn = txtBox.Text.Replace(vari.Seperator.ToString(), "\r\n");
                        break;

                    case enInputType.MultiTextItem:
                        rtn = MultiTextItemGetValue();
                        break;

                    case enInputType.ColorSelect:
                        rtn =  wpfFnc.Brush2String(BrushSelect_Brush);
                        break;

                    case enInputType.BrushesSelect:
                        rtn = cmbBrushes.BrushName;
                        break;     

                    default:
                        rtn = txtBox.Text;
                        break;
                }
                

                return rtn;
            }
            set
            {
                switch (InputType)
                {
                    case enInputType.COMBO:
                        
                        //control_Dispatcher.ComboBox_SelectedItem(cmbBox, cmbBox.te)
                        //if (cmbBox.DropDownStyle == ComboBoxStyle.DropDown)
                        //    cmbBox.Text = value;
                        //else if (cmbBox.DataSource == null)
                        //    cmbBox.SelectedItem = Text;
                        //else
                        //    control.Invoke_ComboBox_SelectedItem(cmbBox, ComboBoxValueMember, Fnc.obj2String(value));

                        if (cmbBox.ItemsSource == null)
                        {
                            cmbBox.Text = Fnc.obj2String(value);
                            if (!cmbBox.Text.Equals(value)) cmbBox.SelectedIndex = -1;
                        }
                        else
                        {                            
                            control_Dispatcher.ComboBox_SelectedItem(cmbBox, cmbBox.SelectedValuePath, value);
                        }


                        break;

                    case enInputType.LABEL:                    
                        dLabel.Content = Fnc.obj2String(value);
                        //databinding 이용시 textbox값을 이용하므로
                        //_isSetLabelValue = true;
                        //txtBox.Text = Fnc.obj2String(value);
                        //Application.DoEvents();
                        //_isSetLabelValue = false;
                        break;

                    case enInputType.MultiTextItem:
                        MultiTextItemSetItem(value);
                        break;

                    case enInputType.ColorSelect:
                        BrushSelect_Brush = wpfFnc.String2Brush(value);
                        dLabel.Background = BrushSelect_Brush;
                        break;

                    case enInputType.DatePicker:
                        try
                        {
                            DateText = Fnc.String2Date(value, Fnc.enDateType.Date);                            
                        }
                        catch
                        {
                            DateText = DateTime.Now;                                                        
                        }
                        dLabel.Content = Fnc.Date2String(DateText, Fnc.enDateType.Date); ;
                        break;

                    case enInputType.DateTimePicker:
                        try
                        {
                            DateText = Fnc.String2Date(value, Fnc.enDateType.DateTime);
                        }
                        catch
                        {
                            DateText = DateTime.Now;                                                        
                        }
                        dLabel.Content = Fnc.Date2String(DateText, Fnc.enDateType.DateTime); 
                        break;

                    case enInputType.BrushesSelect:
                        cmbBrushes.BrushName = value;
                        break;

                    default:
                        txtBox.Text = Fnc.obj2String(value);
                        break;
                }

                Check_TextChanged(value);
            }
        }


        private void Check_TextChanged(string NewValue = null)
        {
            switch (InputType)
            {
                case enInputType.COMBO:
                    break;                

                default:
                    NewValue = Text;
                    break;
            }


            if (Fnc.obj2String(NewValue).Equals(Value))
            {
                lblChangeMarker.Content = string.Empty;
                isChange = false;
            }
            else
            {
                lblChangeMarker.Content = "*";
                isChange = true;
            }

            usrEventArgs a = new usrEventArgs();
            a.EventKind = enEventKind.TEXT_CHANGED;
            a.NewValue = NewValue;


            _text_Changed?.Invoke(this, a);
        }

        /// <summary>
        /// userInputBox의 값을 Commit 한다. Text -> Value
        /// </summary>
        public void Commit()
        {
            Value = Text;
        }

        
        public wucInputBox()
        {
            InitializeComponent();            

            //DLabel_SetAnimation();


            dLabel.MouseDown += DLabel_MouseDown;
            dLabel.MouseLeftButtonUp += DLabel_MouseLeftButtonUp;
            txtBox.TextChanged += TxtBox_TextChanged;            
            cmbBox.SelectionChanged += CmbBox_SelectionChanged;
            //cmbMulti.SelectionChanged += CmbBox_SelectionChanged; //선택 항복이 변경되어도 의미가 없음
            cmbMulti.ItemChanged += CmbMulti_ItemChanged;
            

            dLabel.Padding = new Thickness(0);
            dLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
            dLabel.VerticalContentAlignment = VerticalAlignment.Center;

            passBox.PasswordChar = '*';
            passBox.PasswordChanged += PassBox_PasswordChanged;
            
        }

        private void PassBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Check_TextChanged();
        }


        /// <summary>
        /// 콤포 박스 아이템이 추가/삭제 되면 TextChanged 이벤트 발생처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbMulti_ItemChanged(object sender, TextChangedEventArgs e)
        {
            Check_TextChanged(null);
        }


        /// <summary>
        /// 콤보 박스 아이템 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //
            string NewValue = null;
            if (e.AddedItems.Count > 0)
            {
                object o = e.AddedItems[0];

                switch(o.GetType().ToString())
                {
                    case "System.String":
                        NewValue = o.ToString();
                        break;

                    case "System.Data.DataRowView":
                        System.Data.DataRowView r = (System.Data.DataRowView)o;
                        NewValue = Fnc.obj2String(r[cmbBox.SelectedValuePath]);
                        break;

                    case "function.wpf.NamedBrush":
                        NamedBrush bb = (NamedBrush)o;
                        NewValue = bb.Name;
                        break;

                    default:
                        ListBoxItem l = o as ListBoxItem;

                        if (l != null)
                            NewValue = function.Fnc.obj2String(l.Content);
                        break;
                }              
                
            }

            if (InputType == enInputType.COMBO || InputType == enInputType.BrushesSelect)
                Check_TextChanged(NewValue);
            else if(InputType == enInputType.MultiTextItem)
                Check_TextChanged(null);
        }


        private void TxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            switch (InputType)
            {
                case enInputType.TEXTBOX:
                case enInputType.MultiLineTEXTBOX:            
                    if(TextBox_InputType == enTextType.NumberOlny && txtBox.Text != "")
                    {
                        if (!Fnc.isNumeric(txtBox.Text)) txtBox.Text = "";
                    }

                    Check_TextChanged();
                    break;
            }
        }

        private void DLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (InputType)
            {   
                //날짜선택 popup 표시
                case enInputType.DatePicker:
                case enInputType.DateTimePicker:                    
                    //popDate.IsOpen = true;
                    //datePicker.Value = DateText;
                    //FocusManager.SetFocusedElement(this.Parent, datePicker);
                    break;

                default:
                    Button1.Focus();
                    break;
            }
        }

        private void DLabel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            switch (InputType)
            {
                case enInputType.ColorSelect:
                    //컬러 선택이면 컬러 선택 창을 뜨운다
                    System.Windows.Forms.ColorDialog c = new System.Windows.Forms.ColorDialog();
                    //string col = wpfFnc.Brush2String(BrushSelect_Brush);
                    c.Color = Fnc.SolidColorBrush2Color(BrushSelect_Brush); // Fnc.String2AColor(col);
                    if (c.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
                    Text = Fnc.WpfColor2String(c.Color);
                    break;


                //날짜선택 popup 표시
                case enInputType.DatePicker:
                case enInputType.DateTimePicker:
                    popDate.Width = dLabel.ActualWidth;
                    popDate.Height = dLabel.ActualWidth;
                    popDate.IsOpen = true;
                    datePicker.Value = DateText;
                    FocusManager.SetFocusedElement(this.Parent, datePicker);
                    break;
            }

            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Init_Control_All();            
        }

        /// <summary>
        /// 컨트롤 초기화(방향 포함)
        /// </summary>
        public void Init_Control_All()
        {
            grd.RowDefinitions.Clear();
            grd.ColumnDefinitions.Clear();

            if(Orientation == Orientation.Horizontal)
            {   //가로
                grd.ColumnDefinitions.Add(new ColumnDefinition());
                grd.ColumnDefinitions.Add(new ColumnDefinition());
                grd.ColumnDefinitions.Add(new ColumnDefinition());
                grd.ColumnDefinitions.Add(new ColumnDefinition());
                grd.ColumnDefinitions.Add(new ColumnDefinition());
            }
            else
            {
                grd.RowDefinitions.Add(new RowDefinition());
                grd.RowDefinitions.Add(new RowDefinition());
                grd.RowDefinitions.Add(new RowDefinition());
                grd.RowDefinitions.Add(new RowDefinition());
                grd.RowDefinitions.Add(new RowDefinition());
            }

            Init_Control();
        }

        /// <summary>
        /// grid에 set하고 컨트롤의 너비나 높이를 계산 하여 col나 row의 너비에 대응 하여 준다.
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="initValue"></param>
        /// <param name="GrdColidx"></param>
        /// <returns></returns>
        private void ActualCtrl_Set(ContentControl ctrl, double initValue, int GrdColidx)
        {   
            double rtn;

            if (initValue > 0)
            {
                rtn = initValue;
            }
            else
            {

                string txt;

                ImageButton ib = ctrl as ImageButton;

                if (ib != null)
                    txt = ib.Text;
                else
                    txt = (string)ctrl.Content;

                FormattedText ft = new FormattedText(txt, CultureInfo.CurrentCulture, FlowDirection, new Typeface(ctrl.FontFamily.Source), ctrl.FontSize, ctrl.Foreground);

                if (Orientation == Orientation.Horizontal)
                    rtn = (ft.Width * 1.0) + (this.Padding.Left * 3) + (this.Padding.Right * 3);
                else
                    rtn = (ft.Height * 1.0) + (this.Padding.Top * 3) + (this.Padding.Bottom * 3);
            }

            if (Orientation == Orientation.Horizontal)
            {
                Grid.SetColumn(ctrl, GrdColidx);
                grd.ColumnDefinitions[GrdColidx].Width = new GridLength(rtn, GridUnitType.Pixel);
            }
            else
            {
                Grid.SetRow(ctrl, GrdColidx);
                grd.RowDefinitions[GrdColidx].Height = new GridLength(rtn, GridUnitType.Pixel);
            }

            

        }


        /// <summary>
        /// 컨트롤 초기화
        /// </summary>
        public void Init_Control()
        {
            //change markr 표시 위치
            int idx = 2;

            if(Label_Visibility == Visibility.Visible)
                ActualCtrl_Set(label, Label_Width, 0);
                        
            //버튼2개가 모두 보임
            if (this.Button1_Visibility == Visibility.Visible && this.Button2_Visibility == Visibility.Visible)
            {
                ActualCtrl_Set(Button1, Button1_Width, 3);
                ActualCtrl_Set(Button2, Button2_Width, 4);
                idx = 2;
            }
            else if (this.Button1_Visibility == Visibility.Visible && this.Button2_Visibility != Visibility.Visible)
            {   //버튼1만 보입
                ActualCtrl_Set(Button1, Button1_Width, 4);
                idx = 3;
            }
            else if (this.Button1_Visibility != Visibility.Visible && this.Button2_Visibility == Visibility.Visible)
            {   //버튼2만 보입
                ActualCtrl_Set(Button2, Button2_Width, 4);
                idx = 3;
            }
            else
                idx = 4;        //버튼 모두 안보임
            

            if (this.ChageMarker_Visibility == Visibility.Visible)
            {                
                ActualCtrl_Set(lblChangeMarker, 15, idx);
            }            
           
            InputTypeChange();
        }

        public void InputTypeChange()
        {
            Control[] ctrl = new Control[] { txtBox, dLabel, cmbBox, cmbMulti, passBox, cmbBrushes };

            foreach(Control cc in ctrl)
            {
                if (cc != null && grd.Children.Contains(cc)) grd.Children.Remove(cc);
            }
                        
            Control c = txtBox;

            switch(InputType)
            {
                case enInputType.COMBO:
                    c = cmbBox;
                    break;

                case enInputType.LABEL:
                    c = dLabel;
                    break;

                case enInputType.DatePicker:
                case enInputType.DateTimePicker:
                    c = dLabel;

                    //날짜 선택 팝업 초기환
                    if(popDate == null)
                    {                       
                        popDate = new Popup();
                        datePicker = new function.wpf.DatePicker();
                        datePicker.ShowNull = false;
                        popDate.Child = datePicker;
                        popDate.PlacementTarget = c;                        
                        popDate.StaysOpen = false;
                        datePicker.OnDateSelected += Dt_OnDateSelected;                                          
                    }                    
                    
                    datePicker.ShowTime = (InputType == enInputType.DateTimePicker);
                    valueChaged();
                    break;

                case enInputType.MultiLineTEXTBOX:
                    txtBox.AcceptsReturn = true;
                    txtBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                    c = txtBox;
                    break;

                case enInputType.FileSelect:
                case enInputType.FolderSelect:
                    txtBox.AcceptsReturn = false;
                    txtBox.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;


                    if(Button1_Text == "Button")  Button1_Text = "···";
                    //Button1_Visibility = Visibility.Visible;
                    if (Button1_Text == "Button") Button2_Text = "Ⅹ";
                    //Button2_Visibility = Visibility.Visible;
                    break;

                case enInputType.ColorSelect:
                    c = dLabel;
                    break;

                case enInputType.MultiTextItem:
                    c = cmbMulti;
                    break;

                case enInputType.Password:
                    c = passBox;
                    break;

                case enInputType.BrushesSelect:
                    //브러쉬 선택박스 초기화
                    if (cmbBrushes == null)
                    {
                        cmbBrushes = new ComboBoxBrush();
                        cmbBrushes.SelectionChanged += CmbBox_SelectionChanged;

                    }

                    cmbBrushes.Columns = ComboBoxBrush_Columns;
                    c = cmbBrushes;
                    break;

                default:
                    txtBox.AcceptsReturn = false;
                    txtBox.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                    break;
            }

            int idx = 1;
            int span = 1;

            if (Label_Visibility != Visibility.Visible)
            {
                idx = 0;
                span++;
            }

            if (this.ChageMarker_Visibility != Visibility.Visible) span++;

            if (this.Button1_Visibility != Visibility.Visible) span++;
            
            if (this.Button2_Visibility != Visibility.Visible) span++;

            grd.Children.Add(c);

            if (Orientation == Orientation.Horizontal)
            {
                Grid.SetColumn(c, idx);
                Grid.SetColumnSpan(c, span);
            }
            else
            {
                Grid.SetRow(c, idx);
                Grid.SetRowSpan(c, span);
            }

        }

       

        /// <summary>
        /// 날짜 선택 완료
        /// </summary>
        /// <param name="o"></param>
        private void Dt_OnDateSelected(object o)
        {
            DatePicker dp = o as DatePicker;

            Text = Fnc.Date2String((DateTime)dp.Value, InputType == enInputType.DatePicker ? Fnc.enDateType.Date : Fnc.enDateType.DateTime);
            popDate.IsOpen = false;
        }

        public static readonly DependencyProperty InputTypeProperty = DependencyProperty.Register("InputType", typeof(enInputType), typeof(wucInputBox),
           new PropertyMetadata(enInputType.TEXTBOX, InputTypeChangeCallback));
        

        /// <summary>
        /// 입력 타입을 설정하거나 가져 옵니다.
        /// </summary>
        public enInputType InputType
        {
            get { return (enInputType)GetValue(InputTypeProperty); }
            set
            {
                SetValue(InputTypeProperty, value);                
            }
        }

        static void InputTypeChangeCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            wucInputBox w = o as wucInputBox;
            w.InputTypeChange(); 
        }


        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(wucInputBox),
           new PropertyMetadata(System.Windows.Controls.Orientation.Horizontal, OrientationChangeCallback));

        /// <summary>
        /// 컨트롤의 방향을 가져오간 설정 합니다.
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set
            {
                SetValue(OrientationProperty, value);
            }
        }

        static void OrientationChangeCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            wucInputBox w = o as wucInputBox;
            w.Init_Control_All();
        }      
        



        #region label속성
        public object Label_Content
        {
            get { return label.Content; }
            set
            {
                label.Content = value;
                Init_Control();
            }
        }

        public Brush Label_Background
        {
            get { return label.Background; }
            set { label.Background = value; }
        }

        public Brush Label_Foreground
        {
            get { return label.Foreground; }
            set { label.Foreground = value; }
        }

        public HorizontalAlignment Label_HorizontalContentAlignment
        {
            get { return label.HorizontalContentAlignment; }
            set { label.HorizontalContentAlignment = value; }
        }

        public VerticalAlignment Label_VerticalContentAlignment
        {
            get { return label.VerticalContentAlignment; }
            set { label.VerticalContentAlignment = value; }
        }

        public FontFamily Label_FontFamily
        {
            get { return label.FontFamily; }
            set { label.FontFamily = value; }
        }
        
        public double Label_FontSize
        {
            get { return label.FontSize; }
            set {
                if (value <= 0)
                    label.FontSize = this.FontSize;
                else
                    label.FontSize = value;
            }
        }

        public Brush Label_BorderBrush
        {
            get { return label.BorderBrush; }
            set { label.BorderBrush = value; }
        }

        public Thickness Label_BorderThickness
        {
            get { return label.BorderThickness; }
            set { label.BorderThickness = value; }
        }


        public Visibility Label_Visibility
        {
            get { return label.Visibility; }
            set
            {
                if (label.Visibility == value) return;

                label.Visibility = value;

                Init_Control();
            }
        }


        public Thickness Label_Padding
        {
            get { return label.Padding; }
            set { label.Padding = value; }
        }



        double _label_Width = 50;
        /// <summary>
        /// 라벨 너비를 가지고 오거나 설정합니다. -1이면 자동으로 너비를 설정한다.
        /// </summary>
        public double Label_Width
        {
            get { return _label_Width; }
            set
            {
                if (_label_Width == value) return;

                _label_Width = value;
                Init_Control();
            }           
            
        }


        #endregion


        #region ChageMarker, button1/2 속성

        /// <summary>
        /// ChangeMarker(변경표시자) 표시 여부를 가져오거나 설정합니다.
        /// </summary>
        public Visibility ChageMarker_Visibility
        {
            get { return lblChangeMarker.Visibility; }
            set
            {
                if (lblChangeMarker.Visibility == value) return;

                lblChangeMarker.Visibility = value;

                Init_Control();
            }
        }

        /// <summary>
        /// 버튼 표시 여부를 가져오거나 설정합니다.
        /// </summary>
        public Visibility Button1_Visibility
        {
            get { return Button1.Visibility; }
            set
            {
                if (Button1.Visibility == value) return;
                Button1.Visibility = value;
                Init_Control();
            }
        }


        double _btn1_Width = 0;
        /// <summary>
        /// 버튼 너비를 가지고 오거나 설정합니다. -1이면 자동으로 너비를 설정한다.
        /// </summary>
        public double Button1_Width
        {
            get { return _btn1_Width; }
            set
            {
                if (_btn1_Width == value) return;

                _btn1_Width = value;
                Init_Control();
            }
        }

        /// <summary>
        /// 버튼2 활성화 여부를 가져오거나 설정 합니다.
        /// </summary>
        public bool Button1_Enabled
        {
            get { return Button1.IsEnabled; }
            set { Button1.IsEnabled = value; }
        }


        /// <summary>
        /// 버튼 표시 여부를 가져오거나 설정합니다.
        /// </summary>
        public Visibility Button2_Visibility
        {
            get { return Button2.Visibility; }
            set
            {
                if (Button2.Visibility == value) return;
                Button2.Visibility = value;
                Init_Control();
            }
        }


        double _btn2_Width = 0;
        /// <summary>
        /// 버튼 너비를 가지고 오거나 설정합니다. -1이면 자동으로 너비를 설정한다.
        /// </summary>
        public double Button2_Width
        {
            get { return _btn2_Width; }
            set
            {
                if (_btn2_Width == value) return;

                _btn2_Width = value;
                Init_Control();
            }

        }

        /// <summary>
        /// 버튼2 활성화 여부를 가져오거나 설정 합니다.
        /// </summary>
        public bool Button2_Enabled
        {
            get { return Button2.IsEnabled; }
            set { Button2.IsEnabled = value; }
        }

        #endregion


        #region DataLabel속성

        enBlinkType _dLabel_Blink = enBlinkType.None;
        /// <summary>
        /// 데이터 라벨의 깜박이는 여부
        /// </summary>
        public enBlinkType DLabel_Blink
        {
            get { return _dLabel_Blink; }
            set
            {
                if (_dLabel_Blink == value) return;

                _dLabel_Blink = value;

                dLabel_Blink();
            }
        }

        Brush _dLabel_backColor = SystemColors.ControlBrush;
        Brush _dLabel_BlinkColor = Brushes.DarkGray;
        DoubleAnimation daLabelAnimation = null;
        Storyboard daLabelStoryboard = null;
        /// <summary>
        /// 테이터 라벨 깜박이는 색깔을 지정하거나 가져 온다.
        /// </summary>
        public Brush DLabel_BlinkColor
        {
            get { return _dLabel_BlinkColor; }
            set
            {
                _dLabel_BlinkColor = value;               
            }
        }

        private void DLabel_SetAnimation()
        {
            if (daLabelAnimation == null)
            {                
                daLabelAnimation = new DoubleAnimation();
                daLabelAnimation.From = 1;
                daLabelAnimation.To = 0.0;
                daLabelAnimation.RepeatBehavior = RepeatBehavior.Forever;
                daLabelAnimation.AutoReverse = true;
                daLabelAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(750));
                dLabel.Background = _dLabel_BlinkColor;
            }

            if (daLabelStoryboard == null)
            {
                daLabelStoryboard = new Storyboard();
                daLabelStoryboard.Children.Add(daLabelAnimation);
                Storyboard.SetTargetProperty(daLabelAnimation, new PropertyPath("(Label.Opacity)"));
                Storyboard.SetTarget(daLabelAnimation, dLabel);
            }
        }



        bool _focused = false;

        bool _dLabel_Blinked = false;

        DoubleAnimation daLabel;

        public bool DLabel_Blinked
        {
            get { return _dLabel_Blinked; }
        }

        private void dLabel_Blink()
        {
            if (InputType == enInputType.LABEL)
            {
                if (daLabelStoryboard == null) return;

                Console.WriteLine($"dLabel_Blink {this.IsFocused}");

                if (_dLabel_Blink == enBlinkType.Always)
                {
                    dLabel.Background = DLabel_BlinkColor;
                    daLabelStoryboard.Begin();
                    return;
                }
                else if(_dLabel_Blink == enBlinkType.Focused && _focused)
                {
                    dLabel.Background = DLabel_BlinkColor;
                    daLabelStoryboard.Begin();
                    return;
                }

                dLabel.Background = DLabel_BackColor;
                daLabelStoryboard.Stop();
            }           
            
            
        }

        private void UserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            _focused = true;
            dLabel_Blink();

        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            _focused = false;
            dLabel_Blink();
        }


        public Brush DLabel_BackColor
        {
            get { return dLabel.Background; }
            set { dLabel.Background = value; }
        }

        public Brush DLabel_ForeColor
        {
            get { return dLabel.Foreground; }
            set { dLabel.Foreground = value; }
        }

        public VerticalAlignment DLabel_VerticalContentAlignment
        {
            get { return dLabel.VerticalContentAlignment; }
            set { dLabel.VerticalContentAlignment = value; }
        }

        public HorizontalAlignment DLabel_HorizontalContentAlignment
        {
            get { return dLabel.HorizontalContentAlignment; }
            set { dLabel.HorizontalContentAlignment = value; }
        }

        public FontFamily DLabel_Font
        {
            get { return dLabel.FontFamily; }
            set { dLabel.FontFamily = value; }
        }

        public Brush DLabel_BorderStyle
        {
            get { return dLabel.BorderBrush; }
            set { dLabel.BorderBrush = value; }
        }

        public Thickness DLabel_BorderThickness
        {
            get { return dLabel.BorderThickness; }
            set { dLabel.BorderThickness = value; }
        }

        #endregion



        #region TextBox 속성
        /// <summary>
        /// 여러 줄을 입력할 수 있는 TextBox 컨트롤에서 Tab 키를 누를 때 탭 순서에 따라 다음 컨트롤로 포커스를 이동하는 대신 해당 컨트롤에
        /// 탭 문자를 입력할지 여부를 나타내는 값을 가져오거나 설정합니다.
        /// </summary>
        [Description("여러 줄을 입력할 수 있는 TextBox 컨트롤에서 Tab 키를 누를 때 탭 순서에 따라 다음 컨트롤로 포커스를 이동하는 대신 해당 컨트롤에 탭 문자를 입력할지 여부를 나타내는 값을 가져오거나 설정합니다.")]
        public bool TextBox_AcceptsTab
        {
            get { return txtBox.AcceptsTab; }
            set
            {
                txtBox.AcceptsTab = value;                
            }
        }

        [Description("텍스트 박스에 입력 할 수 있는 최대 문자수를 가져오거나 설정 합니다.")]
        public int TextBox_MaxLength
        {
            get
            {
                return txtBox.MaxLength;
            }
            set
            {
                txtBox.MaxLength = value;
            }
        }

        [Bindable(true)]
        [Category("Layout")]
        public HorizontalAlignment txtBox_HorizontalContentAlignment
        {
            get { return txtBox.HorizontalContentAlignment; }
            set { txtBox.HorizontalContentAlignment = value; }
        }

        [Bindable(true)]
        [Category("Layout")]
        public VerticalAlignment txtBox_VerticalContentAlignment
        {
            get { return txtBox.VerticalContentAlignment; }
            set { txtBox.VerticalContentAlignment = value; }
        }


        private enTextType _textBox_InputType = enTextType.All;

        /// <summary>
        /// textBox에서 입력가능한 타입설정
        /// </summary>
        public enTextType TextBox_InputType
        {
            get { return _textBox_InputType; }
            set
            {
                if (value == _textBox_InputType) return;

                _textBox_InputType = value;

                switch (value)
                {
                    case enTextType.NumberOlny:
                        control_Dispatcher.TextBox_Press_NumberOnly(txtBox);
                        break;

                    case enTextType.None:
                        control_Dispatcher.TextBox_Press_None(txtBox);
                        break;

                    case enTextType.EngOnly:
                        control_Dispatcher.TextBox_Press_EngOnly(txtBox);
                        break;

                    default:
                        control_Dispatcher.TextBox_Press_Event_Remove(txtBox);
                        break;

                }

            }
        }


        #endregion

        #region Button1,2 속성

        event RoutedEventHandler button1_Click;

        /// <summary>
        /// 버튼 클릭 이벤트
        /// </summary>
        public event RoutedEventHandler Button1_Click
        {
            add { button1_Click += value; }
            remove { button1_Click -= value; }
        }


        event RoutedEventHandler button2_Click;

        /// <summary>
        /// 버튼 클릭 이벤트
        /// </summary>
        public event RoutedEventHandler Button2_Click
        {
            add { button2_Click += value; }
            remove { button2_Click -= value; }
        }

        string _Button1_FileDialogFilter = "Image files(*.jpg, *.jpeg, *.jpe, *.jfif, *.png)| *.jpg; *.jpeg; *.jpe; *.jfif; *.png|All Files|*.*";

        public string Button1_FileDialogFilter
        {
            get { return _Button1_FileDialogFilter; }
            set
            {
                _Button1_FileDialogFilter = value;
            }
        }


        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            if (button1_Click == null)
            {
                //파일 or 폴더 선택 처리
                if (InputType == enInputType.FileSelect)
                {
                    OpenFileDialog f = new OpenFileDialog();
                    f.Filter = Button1_FileDialogFilter;
                    f.Multiselect = false;                    
                    if(File.Exists(Value)) f.InitialDirectory = Value;
                                       
                    if (f.ShowDialog() == false) return;
                    Text = f.FileName;
                }
                else if (InputType == enInputType.FolderSelect)
                {
                    string[] rst = wpfFnc.FolderSelect(false, Text);

                    if (rst == null) return;
                    Text = rst[0];
                }

                return;
            }

            button1_Click?.Invoke(sender, e);
        }

        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            if (button2_Click == null)
            {
                //X 버튼 처리
                if (InputType == enInputType.FileSelect || InputType == enInputType.FolderSelect)
                {
                    Text = "";  
                }                
                return;
            }

            button2_Click?.Invoke(sender, e);
        }

        /// <summary>
        /// 버튼1 컨텐츠를 가져오거나 설저합니다.
        /// </summary>
        public object Button1_Content
        {
            get { return Button1.Content; }
            set { Button1.Content = value;  }
        }


        /// <summary>
        /// 버튼2 컨텐츠를 가져오거나 설저합니다.
        /// </summary>
        public object Button2_Content
        {
            get { return Button2.Content; }
            set { Button2.Content = value; }
        }


        /// <summary>
        /// 버튼1 텍스트 가져오거나 설저합니다.
        /// </summary>
        public string Button1_Text
        {
            get { return Button1.Text; }
            set { Button1.Text = value; }
        }


        /// <summary>
        /// 버튼2 텍스트를 가져오거나 설저합니다.
        /// </summary>
        public string Button2_Text
        {
            get { return Button2.Text; }
            set { Button2.Text = value; }
        }


        /// <summary>
        /// 버튼1 아이콘이름을 가져오거나 설저합니다.
        /// </summary>
        public string Button1_ResIcon16Name
        {
            get { return Button1.ResIcon16Name; }
            set { Button1.ResIcon16Name = value; }
        }


        /// <summary>
        /// 버튼1 아이콘이름을 가져오거나 설저합니다.
        /// </summary>
        public string Button2_ResIcon16Name
        {
            get { return Button2.ResIcon16Name; }
            set { Button2.ResIcon16Name = value; }
        }


        #endregion

        #region ComboBox 속성

        public void intt()
        {
            //cmbBox.Items

            //cmbBox.text
        }

        [Bindable(true)]        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ItemCollection ComboBox_Items
        {
            get { return cmbBox.Items; }            
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Localizability(LocalizationCategory.NeverLocalize)]
        public int ComboBox_SelectedIndex
        {
            get { return cmbBox.SelectedIndex; }
            set
            {
                cmbBox.SelectedIndex = value;
            }
        }


        #endregion


        #region ComboBox/ComboBoxBrush 속성
        SolidColorBrush _BrushSelect_Brush = new SolidColorBrush(Colors.White);
        public SolidColorBrush BrushSelect_Brush
        {
            get { return _BrushSelect_Brush; }
            set
            {
                _BrushSelect_Brush = value;                
            }
        }

        int comboBoxBrush_Columns = 2;

        public int ComboBoxBrush_Columns
        {
            get { return comboBoxBrush_Columns; }
            set
            {
                comboBoxBrush_Columns = value;

                if (cmbBrushes != null) cmbBrushes.Columns = value;

            }
        }




        #endregion

    }   //end class
}
