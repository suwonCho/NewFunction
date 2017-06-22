using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceProcess;
using System.Diagnostics;

namespace _Test
{
	class MyNewSVC : ServiceBase
	{
		private System.Diagnostics.EventLog evtLog;

		readonly string ver = "v0.1";

		public MyNewSVC(string[] args)
		{
			InitializeComponent();
			string eventSourceName = "MySource";
			string logName = "MyNewLog";


			if (args.Count() > 0)
			{
				eventSourceName = args[0];
			}
			if (args.Count() > 1)
			{
				logName = args[1];
			}
			evtLog = new System.Diagnostics.EventLog();
			if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
			{
				System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName);
			}
			evtLog.Source = eventSourceName;
			evtLog.Log = logName;
		}


		private void InitializeComponent()
		{
			// 
			// MyNewSVC
			// 
			this.ServiceName = "MyNewSVC";

		}


		protected override void OnStart(string[] args)
		{
			evtLog.WriteEntry("In OnStart" + ver );


			System.Timers.Timer timer = new System.Timers.Timer();
			timer.Interval = 10000; // 60 seconds
			timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
			timer.Start();
		}


		public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
		{
			// TODO: Insert monitoring activities here.
			//evtLog.WriteEntry("Monitoring the System_10Sec Timer" + ver, EventLogEntryType.Information, 210001);
			evtLog.WriteEntry("Monitoring the System_10Sec Timer" + ver);
		}


		protected override void OnContinue()
		{
			evtLog.WriteEntry("Monitoring the System_OnContinue" + ver, EventLogEntryType.Information);

			base.OnContinue();			
		}

		protected override void OnPause()
		{
			evtLog.WriteEntry("Monitoring the System_OnPause" + ver, EventLogEntryType.Information);

			base.OnPause();		

		}

		protected override void OnStop()
		{
			evtLog.WriteEntry("Monitoring the System_OnStop" + ver, EventLogEntryType.Error);

			base.OnPause();
		}

	}
}
