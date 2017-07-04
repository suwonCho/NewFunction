using System;
using System.Data;
using System.Threading;

using Function;
using System.Collections.Generic;

namespace PLCComm
{

	/// <summary>
	/// iPLCCommInterface Ŭ����(2017�� ����)
	/// </summary>
	abstract class iPLCCommInterface
	{
		/// <summary>
		/// WriteOrderTable ��� ����
		/// </summary>
		bool isUseWriteOrderTable;

		/// <summary>
		/// iPLCCommInterface Ŭ����
		/// </summary>
		/// <param name="UseWriteOrderTable">WriteOrderTable ��� ���� 
		///		<para/>���:WriteOrder�� ������ ���̺� ���� ����� �׾��ָ� ó���� ������ �ؾ� �Ѵ�.(������Ž�)
		///		<para/>�̻��:WriteOrder Override�ؼ� ���� ó�� ���ϸ� ���� �� ���� �߻�
		///		</param>
		public iPLCCommInterface(bool UseWriteOrderTable)
		{
			dtAddress = new DataTable("PLC Address");		
						
			dtAddress.Columns.Add(new DataColumn("Address", System.Type.GetType("System.String")));
			dtAddress.Columns.Add(new DataColumn("PLCValueType", System.Type.GetType("PLCComm.enPLCValueType")));
			dtAddress.Columns.Add(new DataColumn("Value", System.Type.GetType("System.Object")));
			dtAddress.Columns.Add(new DataColumn("Value(INT)", System.Type.GetType("System.Int16")));
			dtAddress.Columns.Add(new DataColumn("Value(HEX)", System.Type.GetType("System.String")));
			dtAddress.Columns.Add(new DataColumn("Value(STRING)", System.Type.GetType("System.String")));
			dtAddress.PrimaryKey = new DataColumn[] { dtAddress.Columns["Address"] };
			
			dtWriteOrder.Columns.Add(new DataColumn("Address", System.Type.GetType("System.String")));
			dtWriteOrder.Columns.Add(new DataColumn("PLCValueType", System.Type.GetType("PLCComm.enPLCValueType")));
			dtWriteOrder.Columns.Add(new DataColumn("Value", System.Type.GetType("System.Object")));
		}



		/// <summary>
		/// Log��� Ŭ����
		/// </summary>
		internal static Function.Util.Log _log = null;
		
		/// <summary>
		/// ��ϵ� PLC �ּҸ� ���� �ϴ� Datatable
		/// </summary>
		public DataTable dtAddress = new DataTable("PLCAddress");

		/// <summary>
		/// plc �� write order ���̺�
		/// </summary>
		internal DataTable dtWriteOrder = new DataTable("WirteOrder");

		/// <summary>
		/// PLC �� ��ĵ ������..
		/// </summary>
		protected Thread thPLCScan;

		/// <summary>
		/// PLC ���� ������
		/// </summary>
		protected Thread thOpen;
		
		
		/// <summary>
		/// plc�� �� ��ȸ�� ���� �߻� �ϴ� �̺�Ʈ
		/// </summary>
		event delPLCScan _onPLCScan = null;

		/// <summary>
		/// plc�� �� ��ȸ�� ���� �߻� �ϴ� �̺�Ʈ
		/// </summary>
		public event delPLCScan OnPLCScan
		{
			add { _onPLCScan += value; }
			remove { _onPLCScan -= value; }
		}

		private enStatus _conectionStatus = enStatus.None;

		
		/// <summary>
		/// PLC ������¸� ������ ���ų� ���� �մϴ�.
		/// </summary>
		internal enStatus _ConnctionStatus
		{
			get { return _conectionStatus; }
			set
			{
				if (value == _conectionStatus) return;
				
				_conectionStatus = value;


				try     //������ ������ ���,.. ���� ����ش�.
				{
					if (_conectionStatus != enStatus.OK)
					{
						foreach (DataRow dr in dtAddress.Rows)
						{
							dr["Value"] = System.DBNull.Value;
							dr["Value(INT)"] = System.DBNull.Value;
							dr["Value(Hex)"] = System.DBNull.Value;
							dr["Value(STRING)"] = System.DBNull.Value;
						}
					}
					else
					{
						//������ �ݴ��� ���� ����
						_isClose = false;
					}
				}
				catch { }


				if (_onChConnectionStatus != null) _onChConnectionStatus(_conectionStatus);

			}
		}


		/// <summary>
		/// PLC ������¸� ������ �ȴϴ�.
		/// </summary>
		public enStatus ConnctionStatus
		{
			get { return _conectionStatus; }		
		}


		/// <summary>
		/// plc socket ��� ���� ����� ���� �߻� �ϴ� �̺�Ʈ
		/// </summary>
		event delChConnectionStatus _onChConnectionStatus;

		/// <summary>
		/// plc socket ��� ���� ����� ���� �߻� �ϴ� �̺�Ʈ
		/// </summary>
		public event delChConnectionStatus OnChConnectionStatus
		{
			add { _onChConnectionStatus += value; }
			remove { _onChConnectionStatus -= value; }
		}
		
		Dictionary<string, List<delChAddressValue>> dicEvtAdd_ch = new Dictionary<string, List<delChAddressValue>>();

		/// <summary>
		/// �ش� �ּ��� ���� ���� �� ��� �߻� �� �̺�Ʋ�� ��� �Ѵ�.(���� �� �񵿱� ó���� �ϹǷ� ������ ó���� �Ұ�)
		/// </summary>
		/// <param name="address">������ �ּ�</param>
		/// <param name="onChAddressValue">�Ͼ �̺�Ʈ</param>
		public void ChangeEvtAddress_Add(string address, delChAddressValue onChAddressValue)
		{
			List<delChAddressValue> lst;

			if (dicEvtAdd_ch.ContainsKey(address))
			{
				lst = dicEvtAdd_ch[address];				
			}
			else
			{
				lst = new List<delChAddressValue>();
				dicEvtAdd_ch.Add(address, lst);
			}

			lst.Add(onChAddressValue);
		}
		

		
		/// <summary>
		/// ���� ��õ� ������..
		/// </summary>
		protected bool _isTryOpen = false;

		/// <summary>
		/// ��������� �ܺο��� ������ ���� ���
		/// </summary>
		protected bool _isClose = true;

		protected void RetryOpen(bool isStart)
		{
			if (thOpen != null && thOpen.IsAlive)
			{
				thOpen.Abort();
				thOpen.Join();
			}
			thOpen = null;

			if (isStart)
			{
				thOpen = new Thread(new ThreadStart(thRetryOpen));
				thOpen.IsBackground = true;
				thOpen.Name = "Retry Open";
				thOpen.Start();
			}

		}

		protected void thRetryOpen()
		{
			_isTryOpen = true;
			
			try
			{
				close();
			}
			catch { }

			while (true)
			{
				try
				{
					if (_isClose) return;

					Thread.Sleep(2000);

					if (_ConnctionStatus == enStatus.OK) return;

					if (open())
					{
						_isTryOpen = false;
						return;
					}
				}
				catch
				{
					Thread.Sleep(5000);
				}
			}
		}
		


		/// <summary>
		/// ������ �õ��Ѵ�.
		/// </summary>
		/// <returns></returns>
		public abstract bool Open();

		/// <summary>
		/// ������ �õ��Ѵ�. �����峪 �ٸ��� ���� ���� �ʰ� ���� ���Ḹ �õ�
		/// </summary>
		/// <returns></returns>
		protected abstract bool open();

		/// <summary>
		/// ������ ���´�.
		/// </summary>
		/// <returns>��������</returns>
		public abstract bool Close();


		/// <summary>
		/// ������ ���´�. �����峪 �ٸ��� ���� ���� �ʰ� ���� ���Ḹ ����
		/// </summary>
		/// <returns>��������</returns>
		protected abstract bool close();

		

		/// <summary>
		/// PLC�� ����� PLC ADDRESS�� �߰��Ѵ�.
		/// </summary>
		/// <param name="Address">�߰��� �ּ�</param>	
		/// <returns></returns>
		public bool AddAddress(string Address)
		{
			try
			{
				DataRow row = dtAddress.NewRow();

				row["Address"] = Address;

				if (this.GetType() == Type.GetType("PLCModule.PLCModules.clsTEST") && ConnctionStatus == enStatus.OK)
					row["Value"] = 0;
				else
					row["Value"] = DBNull.Value;

				dtAddress.Rows.Add(row);

				return true;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}



		/// <summary>
		/// PLC�� ����� PLC ADDRESS�� �߰��Ѵ�.
		/// </summary>
		/// <param name="Address"></param>
		/// <returns></returns>
		public bool AddAddress(string[] Address)
		{
			try
			{
				foreach (string strAdd in Address)
				{
					AddAddress(strAdd);
				}

				return true;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}



		/// <summary>
		/// ��ϵ� Address�� �����Ѵ�.
		/// </summary>
		/// <param name="Address"></param>
		/// <returns></returns>
		public bool DelAddress(string Address)
		{
			try
			{
				DataRow[] rows = dtAddress.Select("Address = " + Address);

				foreach (DataRow row in rows)
				{
					dtAddress.Rows.Remove(row);
				}
				return true;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// ��ϵ� Address�� �����Ѵ�.
		/// </summary>
		/// <param name="Address"></param>
		/// <returns></returns>
		public bool DelAddress(string[] Address)
		{
			try
			{
				foreach (string strAdd in Address)
				{
					DelAddress(strAdd);
				}
				return true;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}




		/// <summary>
		/// ��ϵ� ��巹���� ���� ������ �´�.(��� �� Ÿ������ ������ �´�.)
		/// </summary>
		/// <param name="Address"></param>
		/// <returns></returns>
		public AddressValue GetValue(string Address)
		{
			try
			{
				if (ConnctionStatus != enStatus.OK ) throw new Exception("PLC�� ������ ���� �����ϴ�.");

				DataRow[] row = this.dtAddress.Select("Address = '" + Address + "'");

				if (row.Length > 0)
				{
					AddressValue rtn = new AddressValue();

					rtn.Value = row[0]["Value"];
					rtn.ValueType = (enPLCValueType)row[0]["ValueType"];

					return rtn;
				}
				else
					throw new Exception("Ȯ��(���) �Ǿ� ���� PLC Address�Դϴ�.[" + Address + "]");
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		/// <summary>
		/// ��ϵ� ��巹���� ���� ��û�� Ÿ������ ������ �´�.
		/// </summary>
		/// <param name="Address">��û �ּ�</param>
		/// <param name="valuetype">��û �� Ÿ��</param>
		/// <returns></returns>
		public AddressValue GetValue(string Address, enPLCValueType valuetype)
		{
			try
			{
				if (ConnctionStatus != enStatus.OK) throw new Exception("PLC�� ������ ���� �����ϴ�.");

				DataRow[] row = this.dtAddress.Select("Address = '" + Address + "'");

				if (row.Length > 0)
				{
					AddressValue rtn = new AddressValue();

					switch(valuetype)
					{
						case enPLCValueType.HEX:
							rtn.Value = row[0]["Value(HEX)"];
							break;

						case enPLCValueType.STRING:
							rtn.Value = row[0]["Value(STRING)"];
							break;

						default:
							rtn.Value = row[0]["Value(INT)"];
							break;
					}
					
					rtn.ValueType = valuetype;

					return rtn;
				}
				else
					throw new Exception("Ȯ��(���) �Ǿ� ���� PLC Address�Դϴ�.[" + Address + "]");
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}


		/// <summary>
		/// ��ϵ� ��巹���� ���� Hex ���·� ������ �´�.(��û�� ��巹�� ������ �ٿ���)
		/// </summary>
		/// <param name="strAddress"></param>
		/// <returns></returns>
		public string GetValueHex(string[] strAddress)
		{
			try
			{
				string strValue = string.Empty;
				foreach (string strAdd in strAddress)
				{
					strValue += GetValue(strAdd);
				}
				return strValue;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		/// <summary>
		/// ��ϵ� ��巹���� ���� Hex ���·� ������ �´�.
		/// </summary>
		/// <param name="strAddress"></param>
		/// <returns></returns>
		public string GetValueHex(string strAddress)
		{

			try
			{
				string rtn = GetValue(strAddress, enPLCValueType.HEX).Value.ToString();

				return rtn;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}



		/// <summary>
		/// ��ϵ� ��巹���� ���� int ���·� ������ �´�.
		/// </summary>
		/// <param name="strAddress"></param>
		/// <returns></returns>
		public int GetValueInt(string strAddress)
		{
			try
			{
				int rtn = (int)GetValue(strAddress, enPLCValueType.INT).Value;

				return rtn;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		/// <summary>
		/// ��ϵ� ��巹���� ���� string ���·� ������ �´�.
		/// </summary>
		/// <param name="strAddress"></param>
		/// <returns></returns>
		public string GetValueString(string strAddress)
		{

			try
			{
				string rtn = GetValue(strAddress, enPLCValueType.STRING).Value.ToString();

				return rtn;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}




		/// <summary>
		/// �ּ� ���� ���� ���ش�.
		/// </summary>
		/// <param name="Address"></param>
		/// <param name="Value"></param>
		/// <returns></returns>
		public virtual bool WriteOrder(string Address, int intValue)
		{
			if (isUseWriteOrderTable) throw new Exception("WriterOrder�� Override �Ͽ� �ֽʽÿ�.");

			try
			{
				lock (this)
				{
					if (ConnctionStatus != enStatus.OK ) throw new Exception("PLC�� ������ ���� �����ϴ�.");

					if (this.GetType() == Type.GetType("PLCModule.PLCModules.clsTEST"))
					{   //test�� write������ �ٷ� ������..
						DataRow[] d = dtAddress.Select(string.Format("Address = '{0}'", Address));

						if (d.Length > 0)
						{
							ChangeddAddressValue(Address, intValue, false, null, null);
							return true;
						}
						else
							return false;
					}

					
					DataRow row = dtWriteOrder.NewRow();

					row["Address"] = Address;
					row["Value"] = intValue;

					dtWriteOrder.Rows.Add(row);

					return true;
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public virtual bool WriteOrder(string[] strAddress, int[] intValue)
		{
			if (isUseWriteOrderTable) throw new Exception("WriterOrder�� Override �Ͽ� �ֽʽÿ�.");

			try
			{
				int i = 0;

				foreach (string strAdd in strAddress)
				{
					WriteOrder(strAdd, intValue[i]);
					i++;
				}


				return true;

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		/// <summary>
		/// �ּ� ���� ���� ���ش�.(TestPLC ����)
		/// </summary>
		/// <param name="Address"></param>
		/// <param name="intValue"></param>
		/// <returns></returns>
		public virtual bool WriteOrder(string Address, string sValue)
		{
			throw new Exception("WriterOrder�� Override �Ͽ� �ֽʽÿ�.");

			lock (this)
			{
				if (ConnctionStatus != enStatus.OK) throw new Exception("PLC�� ������ ���� �����ϴ�.");

				if (this.GetType() != Type.GetType("PLCModule.PLCModules.clsTEST")) return false;

				
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



		/// <summary>
		/// ��巡�� ��� ���θ� Ȯ���Ѵ�.
		/// </summary>
		/// <param name="Address"></param>
		/// <returns></returns>
		protected bool AddressIsExist(string Address)
		{
			DataRow[] drs = dtAddress.Select("Address = '" + Address.ToString() + "'");

			if (drs.Length > 0) return true;
			return false;
		}

		/// <summary>
		/// ��ϵ� ��巹���� Datarow�� ������ �´�
		/// </summary>
		/// <param name="Address"></param>
		/// <returns>null�̸� �̵��</returns>
		protected DataRow AddressGetRow(string Address)
		{
			DataRow[] drs = dtAddress.Select("Address = '" + Address.ToString() + "'");

			if (drs.Length > 0) return drs[0];
			return null;
		}

		/// <summary>
		/// �α� ����� �մϴ�.
		/// </summary>
		/// <param name="strModule"></param>
		/// <param name="strMsg"></param>

		internal static void LogWrite(string strModule, string strMsg)
		{
			try
			{
				if (_log == null) return;

				string strMessage = string.Format("[{1}] {2}", strModule, strMsg);
				_log.WLog(strMessage);
			}
			catch
			{ }

		}


		/// <summary>
		/// �� ��巹���� ���� ���� �Ǿ��� ���.. Log�� ����ϰ�, ��ϵ� �ּ� �� ���� �̺�Ʈ�� ������ �񵿱� ���� �Ͽ��ش�.
		/// </summary>
		/// <param name="address"></param>
		/// <param name="ReceiveValue">����� �� byte�迭�� �ѱ�ų� 3�� �̻� ���ڿ��� HEX�� ������ �������� ����(int)�� �ѱ��</param>
		/// <param name="isWirteByte"></param>
		/// <param name="SendByte"></param>
		/// <param name="ReceiveByte"></param>
		/// <returns></returns>
		protected void ChangeddAddressValue(string address, object ReceiveValue, bool isWirteByte, byte[] SendByte, byte[] ReceiveByte)
		{
			

			try
			{

				lock (this)
				{
					DataRow dr = AddressGetRow(address);

					if (dr == null)
					{
						LogWrite("ChangeddAddressValue", $"�̵�ϵ� �ּ� ������ ����(��ó��)[Address]{address} [ReceiveValue]{ReceiveValue}");
						return;
					}

					

					object oldValue = dr["Value"] != DBNull.Value ? dr["Value"] : "(null)";
					object newValue = null;
					enPLCValueType vType = (enPLCValueType)dr["PLCValueType"];

					enPLCValueType nType = enPLCValueType.INT;

					Int16 intReceiveValue = -1;
					bool isChage = false;
					string tmp;				

					byte[] bts = ReceiveValue as byte[];


					//�ű԰� ó�� �� �� ���� ���� Ȯ���Ѵ�.
					if(bts != null)
					{	//����Ʈ �迭 ó��
						//2�� ���ϸ� short �̹Ƿ�
						if(bts.Length < 3)
						{   //int16 ó��
							intReceiveValue = Int16.Parse(fnc.ByteToLong(bts).ToString());

							isChage = (Int16)dr["Value(INT)"] != intReceiveValue;
							newValue = intReceiveValue;
						}
						else
						{   //Hexó��
							nType = enPLCValueType.HEX;
							newValue = fnc.ByteToHex(bts);

							isChage = Fnc.obj2String(dr["Value(HEX)"]).Equals(newValue.ToString());
						}

					}
					else if (Int16.TryParse(ReceiveValue.ToString(), out intReceiveValue))
					{	//int16 ó��
						isChage = (Int16)dr["Value(INT)"] != intReceiveValue;
						newValue = intReceiveValue;
					}
					else
					{	//hex ó��
						isChage = !Fnc.obj2String(dr["Value(HEX)"]).Equals(ReceiveValue.ToString());
						nType = enPLCValueType.HEX;
					}
					
					
					//������ �Ǿ����� �α׸� ����� �̺�Ʈ�� ���� �Ѵ�.
					if (isChage)
					{
						if (isWirteByte)
						{
							if (SendByte != null) LogWrite("ChangeddAddressValue", "[Sent] " + fnc.ByteToHex(SendByte));
							if (ReceiveByte != null) LogWrite("ChangeddAddressValue", "[Received] " + fnc.ByteToHex(ReceiveByte));
						}

						LogWrite("ChangeddAddressValue", string.Format("[{0}] {1} --> {2}", dr["Address"], oldValue, intReceiveValue.ToString("X4")));

						if (nType == enPLCValueType.INT)
						{
							dr["Value"] = intReceiveValue;
							dr["Value(INT)"] = intReceiveValue;
							tmp = string.Format("{0:X4}", intReceiveValue);
							dr["Value(HEX)"] = tmp;
							dr["Value(STRING)"] = fnc.HexToString(tmp);
						}
						else
						{	//hex
							dr["Value"] = newValue;
							dr["Value(INT)"] = DBNull.Value;
							dr["Value(HEX)"] = newValue;
							dr["Value(STRING)"] = fnc.HexToString(newValue.ToString());
						}

						//�̺�Ʈ ��� ���� Ȯ��
						if (dicEvtAdd_ch.ContainsKey(address))
						{
							List<delChAddressValue> lst = dicEvtAdd_ch[address];

							foreach(delChAddressValue del in lst)
							{
								del.BeginInvoke(address, vType, newValue, null, null);								
							}

						}
						
					}

					
				}
			}
			catch
			{
				throw;
			}
			
		}	



		/// <summary>
		/// ��巹�� �� ���� �α׸� �����.
		/// </summary>
		/// <param name="dr"></param>
		/// <param name="newValue"></param>
		/// <param name="SendByte">��Ͽ� ���� ���� byte �迭(null �Է� �� �αױ��X)</param>
		/// <param name="ReceiveByte">��Ͽ� ���� ����(����) byte �迭(null �Է� �� �αױ��X)</param>
		/// <returns></returns>
		protected bool WroteAddressValue(string strChanged, byte[] SendByte, byte[] ReceiveByte)
		{
			if (SendByte != null) LogWrite("WroteAddressValue", "[Sent] " + fnc.ByteToHex(SendByte));
			if (ReceiveByte != null) LogWrite("WroteAddressValue", "[Received] " + fnc.ByteToHex(ReceiveByte));
			LogWrite("WroteAddressValue", strChanged);

			return true;
		}

		

		#region PLC Scan Theread �˻� ó�� �κ�..
		/// <summary>
		/// Opc ������� üũ ������Ÿ�̸�
		/// </summary>
		Timer tmrCheckSacnThread;
		/// <summary>
		/// ��ĵ ���� Delegate
		/// </summary>
		protected delegate void delScanThread(bool isStart);
		/// <summary>
		/// ��ĵ ���� ���� Method
		/// </summary>
		protected delScanThread delStart = null;

		/// <summary>
		/// ��ĵ �˻� Timer ����..
		/// </summary>
		public void StartScan()
		{
			tmrCheckSacnThread = new Timer(new TimerCallback(tmrCheckScan), null, 0, 10000);
		}

		/// <summary>
		/// ��ĵ �˻� Timer ����..
		/// </summary>
		protected void StopScan()
		{
			if (tmrCheckSacnThread != null)
			{
				tmrCheckSacnThread.Dispose();
				tmrCheckSacnThread = null;
			}
		}

		/// <summary>
		/// ��ĵ ������ �˻�..
		/// </summary>
		/// <param name="obj"></param>
		private void tmrCheckScan(object obj)
		{
			//������ ���� ������ �˻� �� �ʿ� ����.
			if (ConnctionStatus != enStatus.OK ) return;

			//���� �۵���..
			if (thPLCScan != null && thPLCScan.IsAlive) return;

			//��� ��� ����.
			if (delStart == null) return;

			LogWrite("tmrCheckScan", "Address ��ĵ �����尡 ���� �Ǿ����ϴ�. �ٽ� ���� �մϴ�.");

			delStart(true);

		}


		protected int _ReOpen_cnt = 0;
		protected void _ReOpen()
		{
			try
			{
				Close();
			}
			catch
			{
			}
			Thread.Sleep(3000);
			try
			{
				Open();
			}
			catch
			{
			}
			_ReOpen_cnt = 0;
		}

		#endregion


	}
}
