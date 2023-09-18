using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.Runtime.InteropServices;

namespace function.Device.QLight
{
	/// <summary>
	/// 램프 상태
	/// </summary>
	public enum enLampStatus
	{
		Off = 0,
		On = 1,
		Blink = 2
	}

	/// <summary>
	/// 소리 종류
	/// </summary>
	public enum enSoundKind
	{
		off = 0,
		Type1 = 1,
		Type2 = 2,
		Type3 = 3,
		Type4 = 4,
		Type5 = 5
	}

	/// <summary>
	/// 램프 종류
	/// </summary>
	public enum enLampKind
	{
		RedLamp = 2,
		YellowLamp = 3,
		GreenLamp = 4,
		BlueLamp = 5,
		WhiteLame = 6	
	}

	/// <summary>
	/// 경광등 상태 
	/// </summary>
	public enum enQL_Status
	{
		None,
		Connected,
		Disconnected
	}

	public struct QL_Result
	{
		/// <summary>
		/// 성공 실패
		/// </summary>
		public bool Result;

		/// <summary>
		/// 성공시 결과 값
		/// </summary>
		public byte[] Data;
	}


	public delegate void delQL_StatusChanged(QL sender, enQL_Status status);

	public delegate void delQL_DataChanged(QL sender, byte[] data);


	/// <summary>
	/// (인증필요)QLight 경광등 i/f 클래스
	/// </summary>
	public class QL : AppAuth.aAuth, IDisposable
	{
		/// <summary>
		/// 알람 배열 index
		/// </summary>
		readonly int AlarmIdx = 7;

		/// <summary>
		/// ip 어드레스 배열
		/// </summary>
		byte[] _ipAdd = null;

		byte[] _data = new byte[] { 1, 0, 0, 0, 0, 0, 0, 0 };
		
		byte[] _data_lastStatus = new byte[] { 1, 0, 0, 0, 0, 0, 0, 0 };

		/// <summary>
		/// 경광등 마지막 상태 값을 가져 옵니다.
		/// </summary>
		public byte[] Data_lastStatus
		{
			get { return _data_lastStatus; }
		}


		/// <summary>
		/// 알람 연결 상태 체크 쓰레드
		/// </summary>
		Timer thConnChk = null;
		
		/// <summary>
		/// ip 어드레스를 설정하거나 가져 옵니다.
		/// </summary>
		public string IpAddress
		{
			get
			{
				if (_ipAdd == null || _ipAdd.Length != 4) return null;

				return string.Format("{0}.{1}.{2}.{3}", _ipAdd[0], _ipAdd[1], _ipAdd[2], _ipAdd[3]);

			}
			set
			{
				string[] ips = value.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

				if (ips.Length != 4)
				{
					throw new Exception(string.Format("Ip 자리 수 오류 : {0}", IpAddress));
				}

				int idx = 0;
				byte[] ipAdd = new byte[4];

				foreach(string s in ips)
				{
					if(!Byte.TryParse(s, out ipAdd[idx]))
					{
						throw new Exception(string.Format("Ip값 오류 : {0}", IpAddress));
					}

					idx++;
				}


				_ipAdd = ipAdd;

				if(thConnChk != null)	connChk(null);

			}
		}


		int _port;

		/// <summary>
		/// 포트번호를 가져오거나 설정 합니다.
		/// </summary>
		public int Port
		{
			get { return _port; }
			set
			{
				_port = value;

				if (thConnChk != null)  connChk(null);
			}
		}


		event delQL_StatusChanged _onQL_StatusChanged;

		/// <summary>
		/// 경광등 연결 상태가 변경되면 이벤트 발생, 쓰레드에서 발생하므로 컨트롤 invoke 처리 할것
		/// </summary>
		public event delQL_StatusChanged OnQL_StatusChanged
		{
			add
			{
				_onQL_StatusChanged += value;
			}
			remove
			{
				_onQL_StatusChanged -= value;
			}
		}


		event delQL_DataChanged _onQL_DataChanged;

		/// <summary>
		/// 경광등 상태 데이터가 변경 되면 발생합니다.
		/// </summary>
		public event delQL_DataChanged OnQL_DataChanged
		{
			add
			{
				_onQL_DataChanged += value;
			}
			remove
			{
				_onQL_DataChanged -= value;
			}
		}


		/// <summary>
		/// 신규/마지막 상태값이 변경되면 마지막상태를 변경하고, 이벤트를 발생한다.
		/// </summary>
		/// <param name="data"></param>
		private void dataChange(byte[] data)
		{
			bool changed = false;


			for(int i=0; i<8; i++)
			{
				if(_data_lastStatus[i] != data[i])
				{
					changed = true;
					break;
				}
			}


			if (!changed) return;

			_data_lastStatus = data;
			if (_onQL_DataChanged != null) _onQL_DataChanged(this, _data_lastStatus);



		}


		enQL_Status _QL_Status = enQL_Status.None;

		/// <summary>
		/// 경광등 연결 상태를 가져옵니다.
		/// </summary>
		public enQL_Status QL_Status
		{
			get
			{
				return _QL_Status;
			}
		}

		private void QL_Status_Change(enQL_Status staus)
		{
			if (_QL_Status == staus) return;

			_QL_Status = staus;

			if (_onQL_StatusChanged != null) _onQL_StatusChanged(this, _QL_Status);
		}



		/// <summary>
		/// (인증필요)클래스 생성자
		/// </summary>
		/// <param name="ipAddress"></param>
		/// <param name="port"></param>
		public QL(string ipAddress, int port)
		{
			//인증 확인을 한다.
			//AppAuth.Auth.Check(this);
			
			IpAddress = ipAddress;
			Port = port;

			thConnChk = new Timer(new TimerCallback(connChk), null, 2500, 5000);

		}

		/// <summary>
		/// 클래스 정리
		/// </summary>
		public void Dispose()
		{
			try
			{
				thConnChk.Dispose();
			}
			catch
			{

			}
			
		}


		/// <summary>
		/// 알람 연결 상태 체크
		/// </summary>
		/// <param name="obj"></param>
		private void connChk(object obj)
		{
			if (IpAddress == null) return;

			Read_Status();
		}


		private byte[] GetData(bool useLastStatus)
		{
			if (useLastStatus)
			{
				byte[] b = _data_lastStatus.ToArray<byte>();
				b[0] = 1;
				return b;
			}
			else
				return _data.ToArray<byte>();
		}



		/// <summary>
		/// 경광등의 결과값으로 상태를 적용 한다.
		/// </summary>
		/// <param name="rst"></param>
		private void setResult(QL_Result rst)
		{
			//연결이 끊어짐
			if (!rst.Result)
			{
				QL_Status_Change(enQL_Status.Disconnected);
				dataChange(GetData(false));
			}
			else
			{
				QL_Status_Change(enQL_Status.Connected);
				dataChange(rst.Data);
			}

		}


		/// <summary>
		/// 경광등에 byte[] 데이터를 보낸다
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public bool Send_Data(byte[] data)
		{
			QL_Result rst = Send(data);
			return rst.Result;
		}

		/// <summary>
		/// 경광등에 byte[] 데이터를 보낸다
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public bool Send_Data(string data)
		{
			byte[] b = new byte[_data.Length];


			for(int i=0;i < b.Length;i++)
			{
				if(data.Length > i && data[i] == '1' )
				{
					b[i] = 1;
				}
			}

			return Send_Data(b);

		}


		QL_Result Send(byte[] data)
		{
			QL_Result rst = new QL_Result();

			rst.Data = data;

			rst.Result = Tcp_Qu_RW(Port, _ipAdd, rst.Data);

			setResult(rst);

			return rst;
		}


		[DllImport("Qtvc_dll.dll", CallingConvention = CallingConvention.Cdecl)]
		static extern bool Tcp_Qu_RW(int iPort, byte[] pbip, byte[] pbData);

		
		/// <summary>
		/// 알람 전체를 초기화 한다.
		/// </summary>
		/// <returns></returns>
		public QL_Result Clear()
		{
			byte[] d = GetData(false);

			return Send(d);
		}

		/// <summary>
		/// 램프 처리를 명령한다.
		/// </summary>
		/// <param name="lamp"></param>
		/// <param name="status"></param>
		/// <returns></returns>
		public QL_Result Order_Lamp(enLampKind lamp, enLampStatus status, bool useLastStatus = true)
		{
			byte[] d = GetData(useLastStatus);

			d[(int)lamp] = (byte)status;

			return Send(d);
		}

		/// <summary>
		/// 알람 처리를 명령 한다
		/// </summary>
		/// <param name="kind"></param>
		/// <returns></returns>
		public QL_Result Order_Alarm(enSoundKind kind, bool useLastStatus = true)
		{
			byte[] d = GetData(useLastStatus);
			d[AlarmIdx] = (byte)kind;

			return Send(d);
		}


		/// <summary>
		/// 경광등 상태값을 읽는다
		/// </summary>
		/// <returns></returns>
		public QL_Result Read_Status()
		{
			byte[] d = GetData(false);
			d[0] = 0;
			return Send(d);
		}


	}// end class
}
