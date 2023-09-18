using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace function.wpf
{


    public class control_Dispatcher
    {
        class cdParam
        {
            public FrameworkElement Ctrl { get; set; }

            public string MethodName { get; set; }

            public object Value { get; set; }

            public cdParam(FrameworkElement _ctrl, string _MethodName, object _value)
            {
                Ctrl = _ctrl;
                MethodName = _MethodName;
                Value = _value;
            }

        }


        

        public static void InvokeProperty(Dispatcher dis, FrameworkElement ctrl, string PropertyName, object value)
        {
            cdParam p = new cdParam(ctrl, PropertyName, value);

            dis.Invoke(new Action<object>(PropertySet),
                DispatcherPriority.Normal, new object[] { p });
        }


        public static DispatcherOperation BeginInvokeProperty(Dispatcher dis, FrameworkElement ctrl, string PropertyName, object value)
        {
            cdParam p = new cdParam(ctrl, PropertyName, value);

            return dis.BeginInvoke(new Action<object>(PropertySet),
                DispatcherPriority.Normal, new object[] { p });
        }

        static void PropertySet(object o)
        {
            try
            {
                cdParam p = o as cdParam;

                if (p == null) return;

                var type = p.Ctrl.GetType();
                var pt = type.GetProperty(p.MethodName);

                pt.SetValue(p.Ctrl, p.Value, null);

                //Console.WriteLine($"PropertySet {DateTime.Now.ToString()} : {p.Value}");
            }
            catch(Exception e)
            {

            }
        }

        public static void InvokeMethod(Dispatcher dis, System.Windows.FrameworkElement ctrl, string MethodName, object value)
        {
            cdParam p = new cdParam(ctrl, MethodName, value);

            dis.Invoke(new Action<object>(Method),
                DispatcherPriority.Background, new object[] { p });
        }
        
        static void Method(object o)
        {
            cdParam p = o as cdParam;

            if (p == null) return;

            var type = p.Ctrl.GetType();
            var pt = type.GetMethod(p.MethodName);
            object[] obj = (Object[])p.Value;

            pt.Invoke(p.Ctrl, obj);
        }

        public static DispatcherOperation InvokeComboBox_SelectedItem(Dispatcher dis, ComboBox cmb, string strField, string strSelectText)
        {
            return dis.BeginInvoke(new Action(() => ComboBox_SelectedItem(cmb, strField, strSelectText)), DispatcherPriority.Background);
        }


        /// <summary>
		/// 바인딩된 콤보박스에 특정 필드에 값으로 아이템 선택
		/// </summary>
		/// <param name="cmb"></param>
		/// <param name="strField">필드명</param>
		/// <param name="strSelectText">찾는 값</param>
		public static void ComboBox_SelectedItem(ComboBox cmb, string strField, string strSelectText)
        {   
            try
            {
                foreach (object obj in cmb.Items)
                {
                    DataRowView dv = (DataRowView)obj;

                    if (dv[strField].ToString() == strSelectText)
                    {
                        cmb.SelectedItem = obj;
                        return;
                    }
                }

                cmb.SelectedIndex = -1;
            }
            catch
            {
                cmb.SelectedIndex = -1;
            }

        }



        /// <summary>
		/// 텍스트 박스 입력시 넘버만 입력 할 수 있도록 한다.
		/// </summary>
		public static void TextBox_Press_NumberOnly(TextBox tb)
        {
            tb.PreviewTextInput += Tb_PreviewTextInput_NumberOnly;
        }

        /// <summary>
        /// 텍스트 박스 입력시 넘버만 입력 할 수 있도록 한다.
        /// </summary>
        public static void TextBox_Press_EngOnly(TextBox tb)
        {
            tb.PreviewTextInput += Tb_PreviewTextInput_EngOnly;
        }

        public static void TextBox_Press_None(TextBox tb)
        {
            tb.PreviewTextInput += Tb_PreviewTextInput_None;
        }

        private static void Tb_PreviewTextInput_None(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = false;
        }

        private static void Tb_PreviewTextInput_NumberOnly(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = false;
        }

        private static void Tb_PreviewTextInput_EngOnly(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^a-zA-Z0-9\s]");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// 텍스트 박스 입력시 모든 이벤트를 제거 한다.
        /// </summary>
        /// <param name="tb"></param>
        public static void TextBox_Press_Event_Remove(TextBox tb)
        {            
            tb.PreviewTextInput -= Tb_PreviewTextInput_NumberOnly;
            tb.PreviewTextInput -= Tb_PreviewTextInput_EngOnly;
            tb.PreviewTextInput -= Tb_PreviewTextInput_None;
        }



        /// <summary>
        /// 시작 UIElement 아래의 모든 UIElement에서 작업(del)을 처리 한다.
        /// <para>ex) function.delObject del = new delObject(ChangeTextGet);</para>
        /// <para>parmClass para = new parmClass();</para>
        /// para.Ctrl = grdMain;<para/>
        /// para.value = string.Empty;<para/>
        /// function.wpf.control_Dispatcher.UIElementInUIElement(para, del);
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="del">작업할 delegate</param>
        public static void UIElementInUIElement(object parm, function.delObject del)
        {

            parmClass para = parm as parmClass;

            //작업 수행
            del(parm);


            //PANNEL / GRID 처리
            Panel p = para.Ctrl as Panel;

            if (p != null)
            {

                foreach (UIElement c in p.Children)
                {
                    para.Ctrl = c;
                    UIElementInUIElement(para, del);
                }
            }
            else
            {
                //BORDER 처리
                Decorator b = para.Ctrl as Decorator;

                if (b != null)
                {
                    para.Ctrl = b.Child;
                    UIElementInUIElement(para, del);
                }
            }
        }


    }   //end class
}
