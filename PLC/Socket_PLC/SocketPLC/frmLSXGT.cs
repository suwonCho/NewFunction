using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using tcp_server;
using Function;

namespace SocketPLC
{
	public enum enCmdType { None, Read, Write, MNG }
	public enum enDataType
	{
		Unit,
		Block = 0x14
	}

	public enum enMng_Type { Button, Trigger, Fix };

	public enum enValueType { Int, Hex, String };


    public partial class frmLSXGT : Function.form.frmBaseForm
    {
		public enum enWorkType { KPGMS }

		readonly string _plctype = "LSXGT";

		/// <summary>
		/// 어드레스 타입
		/// </summary>
		string _addType = string.Empty;
			

		DataTable _dtAdd = new DataTable();
		DataTable _dtLog;
		DataTable _dtMng;

		string strLastCol = string.Empty;

		int _multiple = 1;
		/// <summary>
		/// PLC 배수
		/// </summary>
		int Multiple
		{
			get { return _multiple; }
			set
			{
				txtMutiple.Text = _multiple.ToString().PadLeft(2, ' ');
			}
		}

		/// <summary>
		/// 테이블의 순위 변경 처리중
		/// </summary>
		private bool _dtAdd_Changing = false;
		
		/// <summary>
		/// 주소와 관리 테이블을 저장한다.
		/// </summary>
		private void AddTable_Save()
		{
			_dtAdd.AcceptChanges();
			fncDb.Address_Set(_plctype, _addType, _dtAdd);
			if (_dtMng == null) return;
			_dtMng.AcceptChanges();
			fncDb.PLC_ValueMng_Save(_plctype, _addType, _dtMng);
		}

		/// <summary>
		/// 주소와 관리 테이블을 로드한다.
		/// </summary>
		private void addTable_Load()
		{
			_dtAdd = fncDb.Address_Get(_plctype, _addType);
			gcAdd.DataSource = _dtAdd.DefaultView;
			_dtAdd.ColumnChanging += new DataColumnChangeEventHandler(_dtAdd_ColumnChanging);
			_dtAdd.TableNewRow += new DataTableNewRowEventHandler(_dtAdd_TableNewRow);

			_dtMng = fncDb.PLC_ValueMng_Get(_plctype, _addType);
			_dtMng.TableNewRow += new DataTableNewRowEventHandler(_dtMng_TableNewRow);
			gcMng.DataSource = _dtMng;
			
		}

		/// <summary>
		/// 관리테이블의 새로운 로우 추가
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void _dtMng_TableNewRow(object sender, DataTableNewRowEventArgs e)
		{
			DataRow dr = e.Row;

            _dtMng.AcceptChanges();

			if (!Fnc.isNumeric(dr["Priority"]))
			{
				var max = _dtMng.AsEnumerable().Max(p => p["Priority"]);
				dr["Priority"] = Fnc.obj2int(max) + 1;
			}

			if (Fnc.obj2String(dr["Address"]).Equals(string.Empty))
			{
				dr["Address"] = "None";
			}

			if (!Fnc.isNumeric(dr["Address_Length"]) )
			{
				dr["Address_Length"] = 1;
			}

			if (Fnc.obj2String(dr["Mng_Type"]).Equals(string.Empty))
			{
				dr["Mng_Type"] = enMng_Type.Button;
			}
			
			if (Fnc.obj2String(dr["ValueType"]).Equals(string.Empty))
			{
				dr["ValueType"] = enValueType.Int;
			}
		}


		public frmLSXGT(string Title)
        {
            InitializeComponent();
            this.Title = Title;

			SavePosition = true;
			SavePosition_Setting = Properties.Settings.Default;

			PropertySaveContolsName = "txtAddType;pnlLeft;txtPort;txtMutiple;pnlLeftDown;gcAdd;gcLog;gcMng;chkAddGroup"; 
			
			
			//dtAdd.ColumnChanged += new DataColumnChangeEventHandler(dtAdd_ColumnChanged);
			Function.Component.DevExp.fnc.GridView_EditInit(gvAdd, false, true);
			Function.Component.DevExp.fnc.GridView_SetClipboard_Cell(gvAdd);
			gvAdd.OptionsSelection.MultiSelect = true;


			Function.Component.DevExp.fnc.GridView_EditInit(gvMng, false, false);
			Function.Component.DevExp.fnc.GridView_ViewInit(gvLog);

			txtAddType.ComboBoxDisplayMember = "Expression";
			txtAddType.ComboBoxValueMember = "ADDTYPE";
			txtAddType.ComboBoxDataSource = fncDb.AddType_Get(_plctype);

			//로그그리드 포맷 표시 설정
			DevExpress.XtraGrid.StyleFormatCondition con = new DevExpress.XtraGrid.StyleFormatCondition();
			con.Condition = DevExpress.XtraGrid.FormatConditionEnum.Expression;
			con.Expression = "Type = 'Read'";
			con.Appearance.BackColor = Color.LightGoldenrodYellow;
			con.Appearance.BackColor2 = Color.White;
			con.Appearance.Options.UseBackColor = true;
			con.ApplyToRow = true;


			gvLog.FormatConditions.Add(con);


			con = new DevExpress.XtraGrid.StyleFormatCondition();
			con.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
			con.ColumnName = "Type";
			con.Value1 = "Write";
			con.Appearance.BackColor = Color.LightSteelBlue;
			con.Appearance.BackColor2 = Color.White;
			con.Appearance.Options.UseBackColor = true;
			con.ApplyToRow = true;

			gvLog.FormatConditions.Add(con);


			con = new DevExpress.XtraGrid.StyleFormatCondition();
			con.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
			con.ColumnName = "Type";
			con.Value1 = "None";
			con.Appearance.BackColor = Color.Crimson;
			con.Appearance.BackColor2 = Color.White;
			con.Appearance.Options.UseBackColor = true;
			con.ApplyToRow = true;

			gvLog.FormatConditions.Add(con);

			//관리 그리드설정
			Function.Component.DevExp.fnc.GridView_Column_SetEnumAsComboBox(gvMng, gvMng.Columns["MNG_Type"], new enMng_Type());
			Function.Component.DevExp.fnc.GridView_Column_SetEnumAsComboBox(gvMng, gvMng.Columns["ValueType"], new enValueType());
			DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit btn = Function.Component.DevExp.fnc.GridView_Column_Button(gvMng, gvMng.Columns["Button"]);
			Function.Component.DevExp.fnc.GridView_Column_CheckEdit(gvMng, gvMng.Columns["isUse"], "Y", "N", "_"); 
			btn.Click += new EventHandler(btn_Click);
			
			//주소 그리드이 그룹설정
			//Function.Component.DevExp.fnc.GridView_GroupEditor_Set(gvAdd, colGroup);
			//colGroup.GroupIndex = 0;
			

        }

		void btn_Click(object sender, EventArgs e)
		{
			mng_Run(gvMng.GetFocusedDataRow());
		}

		void dtAdd_ColumnChanged(object sender, DataColumnChangeEventArgs e)
		{

			if ( strLastCol != string.Empty && e.Column.Caption != strLastCol )
			{
				strLastCol = string.Empty;
				return;
			}

			int intValue;
			string hexValue = string.Empty;

			try
			{
				switch (e.Column.Caption)
				{
					case "Value":
						//value 변경..
						intValue = (int)e.ProposedValue;
						strLastCol = e.Column.Caption;
						e.Row["HexValue"] = intValue.ToString("X4");				
						break;

					case "HexValue":
						hexValue = (string)e.ProposedValue;
						strLastCol = e.Column.Caption;
						e.Row["Value"] = int.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);						
						break;

					default:
						strLastCol = string.Empty;
						break;
						
						
				}
			}
			catch
			{
				strLastCol = string.Empty;
				e.Row["Value"] = 0;				
			}
		}

        string Title;
        TcpServer Server = new TcpServer();
        byte[] bytBodySerial = new byte[] { 0x81, 0x00, 0x03, 0x01, 0x66, 0x88 };
		/// <summary>
		/// header 'LSIS-XGT'
		/// </summary>
		byte[] _bytHeader = new byte[] { 0x4C, 0x53, 0x49, 0x53, 0x2D, 0x58, 0X47, 0X54, 0X00, 0X00 };

        
        

        private void frmServer_Load(object sender, EventArgs e)
        {
            //Server.PortNo = 6040;
            Server.evtRecieveData = new TcpServer.delRecieveData(this.RecieveData);
            Server.evtDisconnect = new TcpServer.delDisconnect(this.ServerDisconnect);

			DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
			            
            //this.txtPort.Text = Server.PortNo.ToString();

			_dtLog = fnc.LogDataTable_Create();
			gcLog.DataSource = _dtLog;

			Application.DoEvents();
			btnOnOff_Click(null, null);
        }

        private void btnOnOff_Click(object sender, EventArgs e)
        {
            if (Server.InOpened)
            {
                Server.Server_Stop();
                lblServerStats.Text = "SeverStop";
                lblServerStats.ForeColor = Color.White;
                lblServerStats.BackColor = Color.Red;
                this.Text = Title;
                this.txtPort.Enabled = true;
				txtAddType.Enabled = true;
            }
            else
            {
                if (!Server.Server_start()) return;
                lblServerStats.Text = "SeverStart";
                lblServerStats.ForeColor = Color.Red;
                lblServerStats.BackColor = Color.AliceBlue;
                this.Text = string.Format("{0} (Port : {1})",this.Title,Server.PortNo);
                this.txtPort.Enabled = false;
				txtAddType.Enabled = false;
            }
        }


        private void RecieveData(byte[] data)
        {           
            
            Data_Proc(data);
        }

        delegate void del_Fp_InsertRow(string data, bool isSend, bool Error);

		private void Log_Write(string data, enCmdType type, bool Error)
        {
			if (_dtLog.Rows.Count > 0)
			{
				if (Fnc.obj2String(_dtLog.Rows[0]["Log"]).Equals(data.Trim()))
				{
					_dtLog.Rows[0]["Time"] = DateTime.Now.ToString("HH:mm:ss");
					return;
				}
			}

			DataRow dr = _dtLog.NewRow();

			dr["Time"] = DateTime.Now.ToString("HH:mm:ss");
			dr["Log"] = data;
			dr["Type"] = type.ToString();
			dr["Err"] = Error;


			_dtLog.Rows.InsertAt(dr, 0);

        }
        

        private void Data_Proc(byte[] data)
        {
            try
            {
                Console.WriteLine("[Data수신]" + Fnc.ByteArray2HexString(data, " "));
                //헤더 체크
                if (!Fnc.bytesEqual(data, 0, _bytHeader, 0, _bytHeader.Length))
                {
                    Log_Write("헤더 불일치", enCmdType.None, true);
                    return;
                }

                string log = string.Empty;

                enCmdType cmd = enCmdType.None;

                //20~21 명령 타입
                switch (data[20])
                {
                    case 0x54:       //read command
                        cmd = enCmdType.Read;
                        break;

                    case 0x58:       //write command
                        cmd = enCmdType.Write;
                        break;

                    case 0xBD:
                        byte[] bytData = new byte[] { 0x01, 0xff };
                        Server.Send(bytData);
                        break;

                    default:
                        break;

                }

                if (cmd == enCmdType.None) return;

                //22-23데이터타입
                enDataType dType = enDataType.Block;

                if (data[22] != 0x14) dType = enDataType.Unit;


                //24-25 예약

                //26-27 변수 개수
                int cnt = fnc.ByteToInt(data, 26, 2);

                int idx = 28;
                int ridx = 30;
                DataRow dr;
                string add = string.Empty;
                byte[] send;
                int value;
                int vLen;

                //log = string.Format("[{0}]", cmd);

                if (dType == enDataType.Block)
                {
                    vLen = fnc.ByteToInt(data, idx, 2);
                    cnt = fnc.ByteToInt(data, idx + 2 + vLen, 2);

                    cnt = Multiple > 1 ? cnt / Multiple : cnt;
                }

                if (cmd == enCmdType.Read)
                {
                    if (dType == enDataType.Unit)
                        send = new byte[32 + cnt * 4];
                    else
                        send = new byte[32 + cnt * 2];
                }
                else
                    send = new byte[30];


                for (int i = 0; i < cnt; i++)
                {


                    //처음이거나 처리단위가 유닛이면
                    if (i == 0 || dType == enDataType.Unit)
                    {
                        //28-29 변수명 길이
                        vLen = fnc.ByteToInt(data, idx, 2);
                        idx += 2;

                        //30~   변수명
                        add = fnc.BytesToAscii(data, idx, vLen);
                        add = fnc.Address_SetAddType(add, Multiple);
                        idx += vLen;

                    }
                    else
                    {	//블록단위(연속처리)
                        add = fnc.Address_NetGet(add);
                    }

                    if (_dtAdd.Select(string.Format("Address = '{0}'", add)).Length > 0)
                        dr = _dtAdd.Select(string.Format("Address = '{0}'", add))[0];
                    else
                    {
                        dr = _dtAdd.NewRow();
                        dr["Address"] = add;
                        dr["Value"] = 0;
                        _dtAdd.Rows.Add(dr);
                    }


                    if (cmd == enCmdType.Read)
                    {

                        if (dType == enDataType.Unit)
                        {	
							//data size
                            fnc.ByteSetIntValue(send, ridx, 2, 2);
                            ridx += 2;
                        }

						//datavalue
                        fnc.ByteSetIntValue(send, ridx, 2, Fnc.obj2int(dr["Value"]));
                        ridx += 2;

                        //if (i == 0 && dType == enDataType.Block & cnt > 1)
                        //    log += string.Format("[{0}", add);
                        //else if (i == 0 || dType == enDataType.Unit) 
                        //    log += string.Format("[{0}] ", add);

                        log += string.Format("[{0}]{1} ", add, dr["Value"]);

                    }
                    else if (cmd == enCmdType.Write)
                    {
						//값 길이
						vLen = fnc.ByteToInt(data, idx, 2);
						idx += 2;

						//값
						value = fnc.ByteToInt(data, idx, vLen);
                        value = Multiple > 1 ? value / Multiple : value;

                        log += string.Format("[{0}]{1}=>{2} ", add, dr["Value"], value);

                        dr["Value"] = value;
                        idx += 2;


                    }
                }

                //if (cmd == enCmdType.Read && dType == enDataType.Block && cnt > 1)
                //{
                //    log += string.Format("~{0}] ", add);
                //}


                _bytHeader.CopyTo(send, 0);

                //plc info 10~11
                send[11] = 1;

                //cpu info [A0]XGK [A4]XGI [A8]XGR
                send[12] = data[12];

                //source of frame
                send[13] = 0x11;

                //invoke id
                send[14] = data[14];
                send[15] = data[15];

                //length
                fnc.ByteSetIntValue(send, 16, 2, send.Length);

                //response type
                if (cmd == enCmdType.Read)
                {
                    send[20] = 0x55;
                    //data type
                    send[22] = data[22];

                    //data cnt
                    fnc.ByteSetIntValue(send, 28, 2, cnt);

                    //data size
                    //fnc.ByteSetIntValue(send, 30, 2, 2);
                }
                else if (cmd == enCmdType.Write)
                {

                    send[20] = (byte)(dType == enDataType.Block ? 0x59 : 0x58);
					send[20] = 0x59;
                    //data type
                    send[22] = data[22];

                    //data cnt
                    fnc.ByteSetIntValue(send, 28, 2, cnt);
                }




                Log_Write(log, cmd, false);

                Server.Send(send);
            }
            catch (Exception ex)
			{
				ProcException(ex, "Data_Proc");
			}

        }

        private void ServerDisconnect()
        {
        }

        private void txtPort_TextChanged(object sender, EventArgs e)
        {
            Server.PortNo = Convert.ToInt32(txtPort.Text, 10);
        }

        private void Q_TagPublish_FormClosing(object sender, FormClosingEventArgs e)
        {
			if (Server == null) return;

            Server.Dispose();
            Server = null;

        }

        private void button1_Click(object sender, EventArgs e)
        {
			_dtLog.Rows.Clear();
		}

		

		private void txtAdd_Type_TextChanged(object sender, EventArgs e)
		{
			try
			{
				_multiple = int.Parse(txtMutiple.Text);
			}
			catch { }
		}

		/// <summary>
		/// 어드레스 타입이 변경 되면 주소를 조회환다.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void txtAddType_Text_Changed(object sender, Function.form.usrEventArgs e)
		{
			if (_addType == txtAddType.Text) return;
			AddTable_Save();
			_addType = txtAddType.Text;
			addTable_Load();
			
		}

		/// <summary>
		/// 어드레스 테이블의 컬럼값이 변경
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void _dtAdd_ColumnChanging(object sender, DataColumnChangeEventArgs e)
		{
			if (_dtAdd_Changing) return;

			bool chaged_value = false;
			try
			{
				int value;
				_dtAdd_Changing = true;

				switch (e.Column.ColumnName)
				{
					case "Value":
						try
						{
							value = Fnc.obj2int(e.ProposedValue);
							//관리테이블확인
							e.ProposedValue = mng_Check(e.Row, value);

							string hexValue = Fnc.obj2int(e.ProposedValue).ToString("X4");
							if (hexValue.Equals(Fnc.obj2String(e.Row["HexValue"]))) return;
							e.Row["HexValue"] = hexValue;
						}
						catch
						{
							e.ProposedValue = e.Row["Value"];
						}

						chaged_value = true;
						break;

					case "HexValue":
						try
						{
							value = int.Parse(Fnc.obj2String(e.ProposedValue), System.Globalization.NumberStyles.HexNumber);
							//관리테이블확인
							e.ProposedValue = mng_Check(e.Row, value).ToString("X4");
							if (value.Equals(Fnc.obj2int(e.Row["Value"]))) return;

							e.Row["Value"] = value;
						}
						catch
						{
							e.ProposedValue = e.Row["HexValue"];
						}

						chaged_value = true;
						break;

					case "Priority":

						int priority = Fnc.obj2int(e.ProposedValue);

						foreach (DataRow dr in _dtAdd.Rows)
						{
							if (dr.Equals(e.Row)) continue;

							if (Fnc.obj2int(dr["Priority"]) >= priority)
							{
								dr["Priority"] = Fnc.obj2int(dr["Priority"]) + 1;
							}
						}

						_dtAdd.DefaultView.Sort = "Priority";

						break;


				}
			}
			catch
			{
			}
			finally
			{
				_dtAdd_Changing = false;				
				if(!gcAdd.InvokeRequired) gcAdd.RefreshDataSource();
			}

		}

		/// <summary>
		/// 로우가 추가 됨
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void _dtAdd_TableNewRow(object sender, DataTableNewRowEventArgs e)
		{

            _dtAdd.AcceptChanges();

			if (Fnc.isNumeric(Fnc.obj2String(e.Row["Priority"]))) return;

			var max = _dtAdd.AsEnumerable().Max(p => p["priority"]);

			e.Row["Priority"] = Fnc.obj2int(max) + 1;		


		}

		/// <summary>
		/// 관리테이블에 해당 하는지 확인하다.
		/// </summary>
		/// <param name="dr"></param>
		int mng_Check(DataRow add, int newValue, bool chkIsUse = true)
		{			
			try
			{
				string a = Fnc.obj2String(add["Address"]);

				foreach (DataRow r in _dtMng.Rows)
				{
					if (chkIsUse && !Fnc.obj2String(r["isUse"]).Equals("Y")) continue;

					

					switch(Fnc.obj2String(r["MNG_Type"]))
					{
						case "Fix":
							if (fnc.Address_InRange(a, Fnc.obj2String(r["Address"]), Fnc.obj2int(r["Address_Length"])))
							{
								if(chkConditon(Fnc.obj2String(r["Condition"])))	newValue = Fnc.obj2int(r["Value"]);
							}
							break;

						case "Trigger":
							if (chkConditon(Fnc.obj2String(r["Condition"]), a, newValue))
							{
								mng_Run(r);
							}
							break;
					}
				}

				return newValue;
			}
			catch
			{
				return newValue;
			}	

		}

		bool chkConditon(string condition, string add = null, object addValue = null)
		{
			if (condition.Trim().Length < 1) return true;

			string[] con = condition.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
			
			if (con.Length != 2) return false;

			if (add != null && !add.Equals(con[0].Trim())) return false;

			try
			{
				if (addValue == null) addValue = Address_GetValue(con[0].Trim());
				return Fnc.obj2int(addValue) == Fnc.obj2int(con[1]);
			}
			catch
			{
				return false;
			}

		}

		object Address_GetValue(string add)
		{
			object rtn = null;

			if (_dtAdd.Select(string.Format("Address = '{0}'", add)).Length > 0)
				rtn = _dtAdd.Select(string.Format("Address = '{0}'", add))[0]["Value"];

			return rtn;
		}

		/// <summary>
		/// 관리테이블을 실행 한다.
		/// </summary>
		/// <param name="mng"></param>
		void mng_Run(DataRow mng)
		{
			string log = "[";
			string a = Fnc.obj2String(mng["Address"]);
			fnc.stPLCAddress add = new fnc.stPLCAddress(a);
			int add_len = Fnc.obj2int(mng["Address_Length"]);
			DataRow[] d;
            string asc = string.Empty;
            bool isString = false;
            log += add.ToString();

            if (Fnc.obj2String(mng["ValueType"]).Equals("String"))
            {
                //int x = char.ConvertToUtf32("A", 0);
                isString = true;
                asc = Fnc.obj2String(mng["Value"]);
            }



			for(int i = 0; i < add_len; i++)
			{
				d = _dtAdd.Select(string.Format("Address = '{0}'", add.ToString()));
				if (d.Length > 0)
				{
                    if (isString)
                    {   //문자열을 주소에 넣어 준다.
                        d[0]["Value"] = fnc.String2AddValue(asc, i * 2, char.ConvertFromUtf32(0).ToString());
                    }
                    else
                    {
                        d[0]["Value"] = Fnc.obj2int(mng["Value"]);
                        if (_dtAdd_Changing) d[0]["HexValue"] = Fnc.obj2int(mng["Value"]).ToString("X4");
                    }
				}
				add.Address++;
			}

			if (add_len > 1)
				log += string.Format("~{0}", add.ToString());

			log += string.Format("]=>{0}", mng["Value"]);

			Log_Write(log, enCmdType.MNG, false);

		}


		void _dtAdd_ColumnChanged(object sender, DataColumnChangeEventArgs e)
		{
			
		}

		private void frmLSXGT_FormClosed(object sender, FormClosedEventArgs e)
		{
			AddTable_Save();
		}

		private void gcMng_MouseClick(object sender, MouseEventArgs e)
		{
			if(gvMng.FocusedColumn.Name.Equals("mngButton")) mng_Run(gvMng.GetFocusedDataRow());
		}

		private void chkAddGroup_CheckedChanged(object sender, EventArgs e)
		{
			if(chkAddGroup.Checked)
			{
				colGroup.GroupIndex = 0;
				gvAdd.ExpandAllGroups();
			}
			else
			{
				colGroup.GroupIndex = -1;
			}
		}

	


    }// end class
}