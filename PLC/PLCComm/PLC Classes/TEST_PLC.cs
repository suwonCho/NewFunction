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
		System.Threading.Timer tmrScan = null;
		bool isScan = false;
		
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

			if(tmrScan == null)
			{
				tmrScan = new Timer(new TimerCallback(thScan), null, 0, 1000);
			}

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

		public void thScan(object o)
		{
			if (isScan) return;
			try
			{
				isScan = true;


				if (ConnctionStatus == enStatus.OK)
				{
					DataRow[] rows = dtAddress.Select("Value is null");

					foreach(DataRow row in rows)
					{
						row["Value"] = 0;
						row["Value(Int)"] = 0;
						row["Value(Hex)"] = "0000";
						row["Value(String)"] = string.Empty;
					}

				}
				else
				{
					DataRow[] rows = dtAddress.Select("Value is not null");

					foreach (DataRow row in rows)
					{
						row["Value"] = DBNull.Value;
						row["Value(Int)"] = DBNull.Value;
						row["Value(Hex)"] = DBNull.Value;
						row["Value(String)"] = DBNull.Value;
					}

				}





			}
			catch
			{

			}
			finally
			{
				isScan = false;
			}

		}



		/// <summary>
		/// 주소 값에 문자열 값을 써준다.
		/// </summary>
		/// <param name="Address"></param>
		/// <param name="sValue"></param>
		/// <returns></returns>
		public override bool WriteOrder(string Address, string sValue)
		{
			if (ConnctionStatus != enStatus.OK) throw new Exception("PLC와 연결이 끊어 졌습니다.");
				
			object val = null;

			val = Encoding.Default.GetBytes(sValue.Trim());

			ChangeddAddressValue(Address, val, false, null, null);

			return true;
		
		}


		/// <summary>
		/// 주소 값에 int 값을 써준다.
		/// </summary>
		/// <param name="Address"></param>
		/// <param name="intValue"></param>
		/// <returns></returns>
		public override bool WriteOrder(string Address, int intValue)
		{
			if (ConnctionStatus != enStatus.OK) throw new Exception("PLC와 연결이 끊어 졌습니다.");

			ChangeddAddressValue(Address, intValue, false, null, null);

			return true;
		}


		/// <summary>
		/// 주소들에 값들을 써준다.
		/// </summary>
		/// <param name="Address"></param>
		/// <param name="intValue"></param>
		/// <returns></returns>
		public override bool WriteOrder(string[] Address, int[] intValue)
		{
			if (ConnctionStatus != enStatus.OK) throw new Exception("PLC와 연결이 끊어 졌습니다.");

			for (int i = 0; i < Address.Length; i++)
			{
				ChangeddAddressValue(Address[i], intValue[i], false, null, null);
			}

			return true;

		}




	}
}
