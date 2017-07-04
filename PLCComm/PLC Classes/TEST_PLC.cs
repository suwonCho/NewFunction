using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading;
using System.Collections;

using Function;

namespace PLCComm
{
	class TEST_PLC : iPLCCommInterface, IDisposable
	{
		
		#region Tcp/IP 통신 관련
		
		public TEST_PLC(string strIPAddress, int intPort, string _strDeviceType) : base(false)
		{
			
		}	

		public override bool Open()
		{			
			return open();
		}


		protected override bool open()
		{
			_ConnctionStatus = Function.enStatus.OK;
			return true;			
		}

		public override bool Close()
		{	
			return close();
		}

		protected override bool close()
		{
			_ConnctionStatus = Function.enStatus.Error;
			return true;
		}		

		#endregion


		#region IDisposable 멤버

		public void Dispose()
		{
			this.Close();

		}

		#endregion



		/// <summary>
		/// 주소 값에 값을 써준다.(TestPLC 전용)
		/// </summary>
		/// <param name="Address"></param>
		/// <param name="intValue"></param>
		/// <returns></returns>
		public override bool WriteOrder(string Address, string sValue)
		{
			lock (this)
			{
				if (ConnctionStatus != enStatus.OK) throw new Exception("PLC와 연결이 끊어 졌습니다.");
				

				DataRow[] d = dtAddress.Select(string.Format("Address = '{0}'", Address));

				if (d.Length > 0)
				{
					d[0]["Value(HEX)"] = sValue;
					return true;
				}
				else
					return false;


			}
		}




	}
}
