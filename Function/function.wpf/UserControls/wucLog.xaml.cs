using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace function.wpf
{
    /// <summary>
    /// wucLog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class wucLog : UserControl
    {

        class logItem
        {
            public string Log { get; set; }
            public enStatus Status { get; set; }
        }

        Queue<logItem> queLog = new Queue<logItem>();
        public DataTable dtLog = null;

        DispatcherTimer tmrLog;

        public wucLog()
        {
            InitializeComponent();

            dtLog = new DataTable();
            dtLog.Columns.Add("LogTime", typeof(string));
            dtLog.Columns.Add("Log", typeof(string));
            dtLog.Columns.Add("BackColor", typeof(string));
            dtLog.Columns.Add("ForeColor", typeof(string));

            grdLog.ItemsSource = dtLog.DefaultView;

            tmrLog = new DispatcherTimer();
            tmrLog.Interval = TimeSpan.FromMilliseconds(100);
            tmrLog.Tick += TmrLog_Tick;
            tmrLog.Start();
        }

        private void TmrLog_Tick(object sender, EventArgs e)
        {
            while (queLog.Count > 0)
            {

                logItem item = queLog.Dequeue();

                DataRow dr = dtLog.NewRow();

                dr["LogTime"] = DateTime.Now.ToString("HH:mm:ss");
                dr["Log"] = item.Log;
                dr["BackColor"] = wpfFnc.StatusBackColorGet(item.Status);
                dr["ForeColor"] = wpfFnc.StatusForeColorGet(item.Status);

                dtLog.Rows.InsertAt(dr, 0);
            }
        }

        public void LogAdd(string log, enStatus status)
        {
            logItem item = new logItem() { Log = log, Status = status };
            queLog.Enqueue(item);
        }

        private void grdLog_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Console.WriteLine($"grdLog_SizeChanged [width]{e.NewSize.Width} [height]{e.NewSize.Height}");

            int cols = grdLog.Columns.Count;
            double w = 0;

            for (int i = 0; i < cols;i++)
            {
                if (i != (cols - 1))
                {
                    w += grdLog.Columns[i].Width.Value;
                }
                else
                    grdLog.Columns[i].Width = e.NewSize.Width - w - 35;
            }
        }
    }   //end class
}
