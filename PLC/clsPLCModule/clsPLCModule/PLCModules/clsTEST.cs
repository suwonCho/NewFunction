using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading;
using System.Collections;

using PLCModule;

namespace  PLCModule.PLCModules
{
	class clsTEST : clsPLCModuleInterface, IDisposable
	{
		

		


		#region Tcp/IP 통신 관련
		private PLCSocket Comm = new PLCSocket();


		public clsTEST(string strIPAddress, int intPort, string _strDeviceType)
		{
			
		}	

		public override bool Open()
		{			
			return open();
		}


		protected override bool open()
		{
			foreach (DataRow dr in dtAddress.Rows)
			{
				dr["Value"] = 0;
				dr["Value(Hex)"] = "0000";
			}
			this.ChConnection_Status(true);
			return true;			
		}

		public override bool Close()
		{	
			return close();
		}

		protected override bool close()
		{
			this.ChConnection_Status(false);
			return true;

		}		

		#endregion


			#region IDisposable 멤버

		public void Dispose()
		{
			this.Close();

		}

		#endregion




	}
}
