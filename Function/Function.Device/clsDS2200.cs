using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Function.Device
{
	/// <summary>
	/// 데이터로직 DS2200 고정형 바코드스캐너 컨트롤 클래스
	/// 부여,원주 이물 스캐너
	/// </summary>
	public class clsDS2200 : _SerialDeviceBase
	{

		bool isTriigered = false;
		bool isConnected = false;

		public string Barode_Last { get; set; }

		private event delReceiveStrData _onReceiveBarcode;

		public event delReceiveStrData OnReceiveBarcode
		{
			add { _onReceiveBarcode += value; }
			remove { _onReceiveBarcode -= value; }
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="setting">PortNo;boudrate;parity;databits;stopbits</param>
		public clsDS2200(string setting) : base(setting)
		{
			initClass();
		}

		private  void initClass()
		{
			Barode_Last = string.Empty;
			OnReceiveData += clsDS2200_OnReceiveBarcode;
		}

		void clsDS2200_OnReceiveBarcode(byte[] data)
		{
			isConnected = true;

			LastLog = string.Format("[Data_OnReceived]{0} / {1}", Fnc.Bytes2String(data), Encoding.Default.GetString(data));

			if(isTriigered)
			{
				int len = data.Length;
				//바코드양식 <stx>바코드<cr><lf>
				if(data[0] == 0x2 && data[len-2] == 0x0d & data[len-1] == 0x0a)
				{
					Barode_Last = Encoding.Default.GetString(data, 1, len - 3);

					LastLog = string.Format("[바코드 수신]" + Barode_Last);

					if (_onReceiveBarcode != null) _onReceiveBarcode(this, Barode_Last);
				}
			}
			
		}


		public new bool Open()
		{
			

			base.Open();

			if (!IsOpen) return false;

			isConnected = false;

			//초기 설정값을 보낸다
			HostMode(true);
			

			Send_Cmd("GA!");	//OP MODE SERIAL-ON-LINE
			Send_Cmd("CAB");	//CODE1 TYPE IS CODE39 STANDARD
			Send_Cmd("DAG");	//CODE2 TYPE IS CODE 128
			Send_Cmd("GQ!");	//BEAM SHETTER IS TRIGGERED

			HostMode(false);

			Barode_Last = string.Empty;

			Thread.Sleep(300);

			if (!isConnected)
			{
				Close();
				LastLog = "스캐너와 연결에 실패하였습니다. 응답이 없습니다.";
				throw new Exception(LastLog);
			}


			return true;

		}

		public new void Close()
		{
			if (isTriigered) Trigger_End();

			base.Close();
		}


		private void HostMode(bool isEnter)
		{
			if(isEnter)
			{
				Send_Cmd("[C");	//ENTER HOST MODE
				Send_Cmd("[B");	//ENTER PGMING MODE
			}
			else
			{
				Send_Cmd("IA!");	//
				Send_Cmd("[A");		//EXIT HOST MODE
			}
		}

		/// <summary>
		/// 트리거를 보낸다.
		/// </summary>
		/// <param name="time">트리거 유지시간 ms</param>
		public void Trigger_Start(int time)
		{
			if (isTriigered) return;

			isConnected = false;

			Barode_Last = string.Empty;

			isTriigered = true;

			HostMode(true);

			Send_Cmd("GA" + '"'.ToString());

			HostMode(false);

			if (time < 1)
			{
				LastLog = "트리거 [시작] 전송";
				return;
			}

			if (!isConnected)
			{				
				LastLog = "스캐너가 응답이 없습니다.";
				IsConnected = enConnectionStatus.Disconnected;
			}
			else
				IsConnected = enConnectionStatus.Connected;


		}

		public void Trigger_End()
		{
			if (!isTriigered) return;

			isTriigered = false;
			isConnected = false;

			HostMode(true);

			Send_Cmd("GA!");	//OP MODE SERIAL-ON-LINE

			HostMode(false);

			LastLog = "트리거 [종료] 전송";

			if (!isConnected)
			{
				LastLog = "스캐너가 응답이 없습니다.";
				IsConnected =  enConnectionStatus.Disconnected;
			}
			else
				IsConnected = enConnectionStatus.Connected;
		}



		public void Send_Cmd(string cmd)
		{
			byte[] sent = Encoding.Default.GetBytes(" " + cmd);
			sent[0] = 0x1b;

			SendData(sent);

			Thread.Sleep(150);

		}
	}
}
