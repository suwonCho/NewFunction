using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace function.wpf
{
    public enum enEventKind
    {
        LOAD,
        TEXT_CHANGED,
        CLICK,
        DOUBLECLICK
    }

    public delegate void usrEventHander(object sender, usrEventArgs e);

    public delegate void delProcException(Exception ex, string strMethodName, bool showMessageBox);

    public class usrEventArgs : EventArgs
    {
        public enEventKind EventKind { get; set; }

        public string NewValue { get; set; }
    }

    public class parmClass
    {
        public object Ctrl;
        public string value;
    }



    class vari
    {
        static char _seperator = (char)3;

        /// <summary>
        /// 항목간 구분자를 가져온다.
        /// </summary>
        public static char Seperator
        {
            get { return _seperator; }
        }

    }
}
