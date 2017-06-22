using System;
using System.Collections.Generic;
using System.Text;

using Function;
using Function.Util;

namespace Function.Device
{
	/// <summary>
	/// 장비 연결 상태
	/// </summary>
	public enum enConnectionStatus
	{
		/// <summary>
		/// 초기화
		/// </summary>
		None,
		/// <summary>
		/// 연결 대기
		/// </summary>
		Wait,
		/// <summary>
		/// 연결 됨
		/// </summary>
		Connected,
		/// <summary>
		/// 연결 끊어짐
		/// </summary>
		Disconnected
	}


	public delegate void delLogUpdate(object sender, string log);


	/// <summary>
	/// 연결 상태 변경 델리 게이트 (기존이름 : delDevStatusChange)
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="status"></param>
	public delegate void delConnectionChanged(object sender, enConnectionStatus status);


	/// <summary>
	/// device class base class
	/// </summary>
	public abstract class _DeviceBaseClass
	{
		/// <summary>
		/// log 기록
		/// </summary>
		public Log Log;

		/// <summary>
		/// 에러 처리를 한다. 로그기록 및 상태표시 창...
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="strMethodName"></param>
		protected void ProcException(Exception ex, string strMethodName)
		{
			if (Log != null) Log.WLog_Exception(strMethodName, ex);			
		}

		internal event delLogUpdate _onLogUpdate;

		/// <summary>
		/// 로그가 변경 되었을 경우 발생합니다.
		/// </summary>
		public event delLogUpdate OnLogUpdate
		{
			add { _onLogUpdate += value; }
			remove { _onLogUpdate += value; }
		}

		string _lastLog = string.Empty;

		/// <summary>
		/// 로그가 기록 된다.
		/// </summary>
		public string LastLog
		{
			get { return _lastLog; }
			set
			{
				if (value == string.Empty)
				{
					_lastLog = string.Empty;
					return;
				}

				_lastLog = string.Format("[{0}]{1}", DateTime.Now.ToString("HH:mm:ss"), value);

				if (_onLogUpdate != null) _onLogUpdate(this, _lastLog);

			}
		}


		internal event delConnectionChanged _onConnectionChanged;

		/// <summary>
		/// 장비 상태 변경시 발생합니다.
		/// </summary>
		public event delConnectionChanged OnConnectionChanged
		{
			add { _onConnectionChanged += value; }
			remove { _onConnectionChanged -= value; }
		}


		enConnectionStatus _isConnected = enConnectionStatus.None;

		/// <summary>
		/// 연결상태를 가지고 옵니다.
		/// </summary>
		public enConnectionStatus IsConnected
		{
			get { return _isConnected; }
			set
			{
				if (_isConnected == value) return;

				_isConnected = value;

				if (_onConnectionChanged != null) _onConnectionChanged(this, _isConnected);
			}
		}


	}
}

