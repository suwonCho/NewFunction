using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using Function;
using Function.Device;

namespace Function.Device
{
	public partial class usrRS232_Setting : UserControl
	{
		/// <summary>
		/// 설정 데이터 셑
		/// </summary>
		DataSet dsSet;

		public string FileName = string.Empty;
		public string DevName = string.Empty;
		bool _isLoadingComplete = false;
		bool _isApplySet = false;



		public BaudRate sBaudRate
		{
			get
			{
				return (BaudRate)Fnc.String2Enum(new BaudRate(), cmbBaudRate.SelectedItem.ToString());
			}
			set
			{
				cmbBaudRate.SelectedItem = value.ToString();
			}
		}

		public DataBits sDataBits
		{
			get
			{
				return (DataBits)Fnc.String2Enum(new DataBits(), cmbDatabit.SelectedItem.ToString());
			}
			set
			{
				cmbDatabit.SelectedItem = value.ToString();
			}
		}

		public StopBits sStopBits
		{
			get
			{
				return (StopBits)Fnc.String2Enum(new StopBits(), cmbStopbit.SelectedItem.ToString());
			}
			set
			{
				cmbStopbit.SelectedItem = value.ToString();
			}
		}

		public Parity sParity
		{
			get
			{
				return (Parity)Fnc.String2Enum(new Parity(), cmbParity.SelectedItem.ToString());
			}
			set
			{
				cmbParity.SelectedItem = value.ToString();
			}
		}

		public Handshake sHandshake
		{
			get
			{
				return (Handshake)Fnc.String2Enum(new Handshake(), cmbHandshake.SelectedItem.ToString());
			}
			set
			{
				cmbHandshake.SelectedItem = value.ToString();
			}
		}


		public usrRS232_Setting()
		{
			InitializeComponent();

		}

		private void usrMK80_Setting_Load(object sender, EventArgs e)
		{
			cmbBaudRate.Items.Clear();
			Function.form.control.Invoke_ComboBox_AddItem(cmbBaudRate, new BaudRate());

			cmbDatabit.Items.Clear();
			Function.form.control.Invoke_ComboBox_AddItem(cmbDatabit, new DataBits());

			cmbStopbit.Items.Clear();
			Function.form.control.Invoke_ComboBox_AddItem(cmbStopbit, new System.IO.Ports.StopBits());

			cmbParity.Items.Clear();
			Function.form.control.Invoke_ComboBox_AddItem(cmbParity, new System.IO.Ports.Parity());

			cmbHandshake.Items.Clear();
			Function.form.control.Invoke_ComboBox_AddItem(cmbHandshake, new System.IO.Ports.Handshake());

			_isLoadingComplete = true;

			if (_isApplySet)
			{
				_isApplySet = false;
				ApplySetting();
			}
		}

		/// <summary>
		/// 설정파일로 부터 설정을 로드한다.
		/// </summary>
		/// <param name="fileName">파일명 (기본:RS232_SET.XML)</param>
		/// <param name="devName">장비명 (기본:default)</param>
		public void InitSettingFile(string fileName, string devName)
		{
			if (fileName == string.Empty) fileName = fnc._defalt_rs232_Setting_fileName;
			if (devName == string.Empty) devName = fnc._defalt_rs232_Setting_devName;

			dsSet = fnc.InitSettingFile(fileName, devName);

			FileName = fileName;
			DevName = devName;
		}

		/// <summary>
		/// 로드된 설정을 컨트롤에 적용한다.
		/// </summary>
		public void ApplySetting()
		{
			//컨트롤이 로딩이 되지 않는 상태의 설정은 적용 되지 않는다.
			if (!_isLoadingComplete)
			{
				_isApplySet = true;
				return;
			}

			if (dsSet == null) InitSettingFile(FileName, DevName);

			DataTable dt = dsSet.Tables[fnc.sPLCSetTable];
			DataRow d = dt.Select(string.Format("plcname = '{0}'", DevName))[0];

			txtComport.Value = Fnc.obj2int(d["COMPORT"]);
			cmbBaudRate.SelectedItem = Fnc.obj2String(d["BAUDRATE"]);
			cmbDatabit.SelectedItem = Fnc.obj2String(d["DATABIT"]);
			cmbStopbit.SelectedItem = Fnc.obj2String(d["STOPBIT"]);
			cmbHandshake.SelectedItem = Fnc.obj2String(d["PARITY"]);
			cmbParity.SelectedItem = Fnc.obj2String(d["FLOW"]);
			
		}

		public void SaveSetting()
		{
			if (dsSet == null) InitSettingFile(FileName, DevName);

			DataTable dt = dsSet.Tables[fnc.sPLCSetTable];
			DataRow d = dt.Select(string.Format("plcname = '{0}'", DevName))[0];

			d["COMPORT"] = txtComport.Value;
			d["BAUDRATE"] = Fnc.obj2String(cmbBaudRate.SelectedItem);
			d["DATABIT"] = Fnc.obj2String(cmbDatabit.SelectedItem);
			d["STOPBIT"] = Fnc.obj2String(cmbStopbit.SelectedItem);
			d["PARITY"] = Fnc.obj2String(cmbHandshake.SelectedItem);
			d["FLOW"] = Fnc.obj2String(cmbParity.SelectedItem);

			fnc.SaveSettingFile(FileName, DevName, dsSet);
		}



	}
}
