using System;
using System.Threading;
using System.Runtime.InteropServices;
using System.Data;
using System.Collections;

#if (ABPLC)
using RsiOPCAuto;

namespace PLCModule.PLCModules
{
	/// <summary>
	/// RSLinx�� �̿��� plc��� classs
	/// ������ Com�׸� 'Rockwell Software OPC Automation' �׸��� ����Ͽ� ����Ұ�..
	/// </summary>
	class clsABPLC : clsPLCModuleInterface, IDisposable
	{

#region ���� �����
		/// <summary>
		/// OpcClass
		/// </summary>
		OPCServer Opc;

		OPCGroup OpcGrp;

		/// <summary>
		/// GateWay���α׷� IP / local�̸� ��� �Ѱ�...
		/// </summary>
		string strIp = string.Empty;
		/// <summary>
		/// ProgramID / ���� 'RSLinx OPC Server'�� gateway����� 'RSLinx Remote OPC Server'
		/// </summary>
		string ProgId = "RSLinx OPC Server";
		/// <summary>
		/// Opc Group Name
		/// </summary>
		string strGrpName = "Group";
		/// <summary>
		/// Topic Name
		/// </summary>
		string strTopicName = string.Empty;
		/// <summary>
		/// Item Value scan rate(ms)
		/// </summary>
		int intUpdateRate = 500;
		/// <summary>
		/// Opc������� ���� ����..
		/// </summary>
		bool bolOpcStatus = false;
		/// <summary>
		/// Opc ������� üũ ������Ÿ�̸�
		/// </summary>
		Timer tmrOpcCheck;
		/// <summary>
		/// ����üũ �ߺ� ���� ���� ����
		/// </summary>
		bool bolChecking = false;

		/// <summary>
		/// Ŀ�ؼ� ������ ����� itemList
		/// </summary>
		System.Collections.ArrayList ItemList = new System.Collections.ArrayList();





#endregion



		/// <summary>
		/// RsLinx Opc Ŭ���� �����..
		/// </summary>
		/// <param name="IPAddress">GateWay���α׷� IP / local�̸� ��� �Ѱ�...</param>
		/// <param name="ProgramID">ProgramID / ���� 'RSLinx OPC Server'�� gateway����� 'RSLinx Remote OPC Server'</param>
		/// <param name="GroupName">�׷���� : empty�ϰ�� 'Group'</param>
		/// <param name="TopicName">�����̸�</param>
		/// <param name="UpdateRate">Item Value scan rate(ms)</param>
		public clsABPLC(string IPAddress, string ProgramID, string GroupName, string TopicName, int UpdateRate)
		{
			this.strIp = IPAddress.Trim();
			if (ProgramID.Trim() != string.Empty) this.ProgId = ProgramID.Trim();
			if (GroupName.Trim() != string.Empty) this.strGrpName = GroupName.Trim();
			if (TopicName.Trim() == string.Empty)
				throw new Exception("Topic�̸��� ���� �Ǿ� ���� �ʽ��ϴ�.");
			else
				this.strTopicName = string.Format("[{0}]", TopicName.Trim());

			if (UpdateRate > 0) this.intUpdateRate = UpdateRate;

		}



#region override ó����	


		protected override bool open()
		{
			if (Open())
			{
				this.ChConnection_Status(true);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Opc�� ������ �մϴ�.
		/// </summary>
		/// <returns></returns>
		public override bool Open()
		{
			string strMsg = string.Format("(Node:{0} / ProgID : {1} )", strIp, ProgId);

			try
			{
				if (Opc != null) close();

				Opc = new OPCServer();
				Opc.Connect(ProgId, (object)strIp);

				//OpcGrp = new OPCGroup();			

				OpcGrp = Opc.OPCGroups.Add(strGrpName);

				OpcGrp.IsActive = true;
				OpcGrp.UpdateRate = intUpdateRate;
				OpcGrp.IsSubscribed = true;

				OpcGrp.DataChange += new DIOPCGroupEvent_DataChangeEventHandler(OpcGrp_DataChange);

				//opc ������� üũ Timer�� �����Ѵ�.
				if (this.tmrOpcCheck != null)
				{
					tmrOpcCheck.Dispose();
					tmrOpcCheck = null;
				}

				//			this.bolOpcStatus = true;
				//			this.ChConnection_Status(bolOpcStatus);

				tmrOpcCheck = new Timer(new TimerCallback(tmrCheckOpcStatus), null, 0, intUpdateRate);

				//this.ChConnection_Status(true);

				strMsg = "Open ����" + strMsg;

				return true;
			}
			catch (System.Runtime.InteropServices.ExternalException ex)
			{   //activex�̱� ������ ���� �߻��� ����� ���� �ʾ� �����...
				strMsg = "Open ���� :" + ex.Message + strMsg + "\r\n" + ex.ToString();
				throw ComException(ex, string.Empty);
			}
			catch (Exception ex)
			{
				strMsg = "Open ���� :" + ex.Message + strMsg + "\r\n" + ex.ToString();
				throw ex;
			}
			finally
			{
				PLCModule.clsPLCModule.LogWrite("Open", strMsg);
			}

		}


		protected override bool close()
		{
			try
			{
				if (OpcGrp != null)
					OpcGrp = null;

				if (Opc != null)
				{
					if (Opc.ServerState == 1)
						Opc.Disconnect();

					Opc = null;
				}

				if (bolOpcStatus)
				{
					bolOpcStatus = false;
					this.ChConnection_Status(bolOpcStatus);
				}

				this.dtAddress.Clear();
			}
			catch (Exception ex)        //opc Ŭ���̾�Ʈ�� ���� �Ǹ�.. ���� �߻�..
			{
				throw ex;
			}

			return true;
		}

		/// <summary>
		/// Opc ������ ������ ���´�.
		/// </summary>
		/// <returns></returns>
		public override bool Close()
		{
			string strMsg = string.Empty;
			try
			{
				this.ItemList.Clear();
				//this.dtAddress.Clear();

				if (tmrOpcCheck != null)
				{
					tmrOpcCheck.Dispose();
					tmrOpcCheck = null;
				}

				strMsg = "Sucess closing connection";
				return this.close();
			}
			catch (Exception ex)
			{
				strMsg = "Fail closing connection   : " + ex.Message + "\r\n" + ex.ToString();
				throw ex;
			}
			finally
			{
				PLCModule.clsPLCModule.LogWrite("Close", strMsg);
			}
		}


		/*
		public override bool AddAddress(int intAddress)
		{
			throw new Exception("���� ���� �ʴ� ��� �Դϴ�.");
		}

		public override bool AddAddress(int intAddress, int intLength)
		{
			throw new Exception("���� ���� �ʴ� ��� �Դϴ�.");
		}

		public override bool AddAddress(string[] strAddress)
		{
			try
			{
				foreach (string strAdd in strAddress)
				{
					this.Item_Add(strAdd);
				}
				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}



		public override bool DelAddress(int intAddress)
		{
			throw new Exception("���� ���� �ʴ� ��� �Դϴ�.");
		}
		public override bool DelAddress(string[] strAddress)
		{
			throw new Exception("���� ���� �ʴ� ��� �Դϴ�.");
		}



		public override int GetValue(int intAddress)
		{
			throw new Exception("���� ���� �ʴ� ��� �Դϴ�.");
		}
		public override int GetValue(int intAddress, int intLength)
		{
			throw new Exception("���� ���� �ʴ� ��� �Դϴ�.");
		}
		public override string GetValue(int intAddress, int intLength, int intRetrunLength)
		{
			throw new Exception("���� ���� �ʴ� ��� �Դϴ�.");
		}

		public override int GetValue(string[] strAddress)
		{
			try
			{
				string strValue = string.Empty;
				foreach (string strAdd in strAddress)
				{
					strValue += IntToHex(this.Item_Read(strAdd));
				}

				return int.Parse(strValue);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public override string GetValue(string[] strAddress, int intRetrunLength)
		{
			try
			{
				return string.Format("{0:D" + intRetrunLength.ToString() + "}", GetValue(strAddress));
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public override int GetValueInt(string strAddress)
		{
			try
			{
				return this.Item_Read(strAddress);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public override bool WriteOrder(int Address, int Value)
		{
			throw new Exception("���� ���� �ʴ� ��� �Դϴ�.");
		}

		public override bool WriteOrder(string[] Address, int[] Value)
		{
			try
			{
				int i = 0;
				string strValue = string.Empty;
				foreach (string strAdd in Address)
				{
					this.Item_Write(strAdd, Value[i]);
					i++;
				}

				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		*/
#endregion

		/// <summary>
		/// opc���� ���� üũ ������..
		/// </summary>
		/// <param name="obj"></param>
		private void tmrCheckOpcStatus(object obj)
		{
			if (bolChecking) return;



			try
			{
				bolChecking = true;

				int intCancelId = 0;
				if (OpcGrp == null)
				{
					if (bolOpcStatus)
						return;
					else
						throw new Exception(string.Empty);
				}

				if (OpcGrp.OPCItems.Count < 1)
				{
					//if 	(bolOpcStatus) 
					return;
					//else
					//	throw new Exception(string.Empty);
				}

				OpcGrp.AsyncRefresh(1, 2, out intCancelId);

				RsiOPCAuto.OPCItem item = OpcGrp.OPCItems.Item(1);

				//���� ���� ������ ���� ����..
				if (item.Quality == 0) throw new Exception(string.Empty);


				if (this.Opc.ServerState == 1 && !this.bolOpcStatus)    //���� ����
				{   //���� ȸ��..
					this.bolOpcStatus = true;
					this.ChConnection_Status(bolOpcStatus);
				}

			}
			catch (Exception ex) //(System.Runtime.InteropServices.ExternalException ex)
			{   //������ ������ ��� ���� üũ�� ���� �߻� �ϹǷ�...

				Console.WriteLine(ex.ToString());

				if (this.bolOpcStatus)
				{
					this.bolOpcStatus = false;
					this.ChConnection_Status(bolOpcStatus);
				}
				//������õ�..
				try
				{
					this.close();
					this.Open();
					this.Item_Add();
				}
				catch { }
			}
			finally
			{

				bolChecking = false;
			}

		}

		/// <summary>
		/// activex���� �Ѿ���� ���� ó��.
		/// </summary>
		/// <param name="ex"></param>
		private Exception ComException(System.Runtime.InteropServices.ExternalException ex, string Info)
		{
			Exception Ex;
			switch (ex.ErrorCode)
			{
				case -2147467259:   //ProgramID�� �߸� ���� �Ǿ��ְų�, ItemId�� �׷쿡 ��� �ʵǾ� ���� ���
					if (Info == string.Empty)
						Ex = new Exception("Opc Open Error : ProramID�� ã�� ���Ͽ����ϴ�. ���� ���� Ȯ�� �Ͽ� �ֽʽÿ�", ex);
					else
						if (this.bolOpcStatus)
						Ex = new Exception("Item Error : �ش� Item�� ��ϵǾ� ���� �ʽ��ϴ�.(" + Info + ")", ex);
					else
						Ex = new Exception("Opc�� ������ ���� �����ϴ�.", ex);

					break;
				case -1073479673:
					if (this.bolOpcStatus)
						Ex = new Exception("Item Error : �ش� Item�� ��ϵǾ� ���� �ʽ��ϴ�.(" + Info + ")", ex);
					else
						Ex = new Exception("Opc�� ������ ���� �����ϴ�.", ex);
					break;
				default:
					Ex = new Exception("Opc Open Error : " + ex.Message, ex);
					break;
			}

			return Ex;
		}


		private void dtAddress_Add(string strItemName)
		{
			try
			{
				this.dtAddress.Rows.Add(new object[] { strItemName, DBNull.Value });
			}
			catch
			{
			}
		}

		/// <summary>
		/// �׷쿡 Item�� �߰� �Ͽ� �ش�.
		/// </summary>
		/// <param name="strItem"></param>
		public void Item_Add(string strItem)
		{
			string ItemName = string.Empty;

			try
			{
				ItemName = strTopicName + strItem;
				int intOpcItemCnt = OpcGrp.OPCItems.Count;
				OpcGrp.OPCItems.AddItem(ItemName, intOpcItemCnt);
				ItemList.Add((object)ItemName);

				dtAddress_Add(ItemName);

			}
			catch (System.Runtime.InteropServices.ExternalException ex)
			{   //activex�̱� ������ ���� �߻��� ����� ���� �ʾ� �����...
				throw ComException(ex, ItemName);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// ������ ���� Item�� �־� �ش�.- re Open�� ���
		/// </summary>
		private void Item_Add()
		{
			string ItemName = string.Empty;
			try
			{
				int intOpcItemCnt = 0;
				foreach (object objItem in ItemList)
				{
					ItemName = objItem.ToString();
					OpcGrp.OPCItems.AddItem(ItemName, intOpcItemCnt);
					intOpcItemCnt++;
					dtAddress_Add(ItemName);
				}
			}
			catch (System.Runtime.InteropServices.ExternalException ex)
			{   //activex�̱� ������ ���� �߻��� ����� ���� �ʾ� �����...
				throw ComException(ex, ItemName);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}



		/// <summary>
		/// PLC Item ���� �д´�.
		/// </summary>
		/// <param name="strItem">ItemName</param>
		/// <returns></returns>
		public int Item_Read(string strItem)
		{
			object ItemName = string.Empty;
			try
			{
				ItemName = strTopicName + strItem;
				OPCItem Item = OpcGrp.OPCItems.Item(ItemName);
				object objValue = Item.Value;
				return Convert.ToInt16(objValue.ToString(), 10);
			}
			catch (System.Runtime.InteropServices.ExternalException ex)
			{   //activex�̱� ������ ���� �߻��� ����� ���� �ʾ� �����...
				throw ComException(ex, ItemName.ToString());
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}



		/// <summary>
		/// Plc Item�� ���� ����..
		/// </summary>
		/// <param name="strItem">ItemName</param>
		/// <param name="lngValue">Writing Value</param>
		public void Item_Write(string strItem, int intValue)
		{
			object ItemName = string.Empty;
			try
			{
				ItemName = strTopicName + strItem;
				OPCItem Item = OpcGrp.OPCItems.Item(ItemName);
				object objValue = intValue;
				Item.Write(objValue);

				string strChanged = string.Format("[{0}] WorteValue : {1}", strItem, intValue);
				WroteAddressValue(strChanged, null, null);
			}
			catch (System.Runtime.InteropServices.ExternalException ex)
			{   //activex�̱� ������ ���� �߻��� ����� ���� �ʾ� �����...
				throw ComException(ex, ItemName.ToString());
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}




#region IDisposable ���

		public void Dispose()
		{
			this.Close();

		}

#endregion

		private void OpcGrp_DataChange(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
		{
			try
			{
				foreach (OPCItem it in OpcGrp.OPCItems)
				{
					//Console.WriteLine("[AB] {0} = {1}", item.ItemID, item.Value);

					string strAddress = it.ItemID; //.Replace(this.strTopicName, string.Empty);

					DataRow[] row = this.dtAddress.Select("Address = '" + strAddress + "'");

					//row[0]["Value"] = Convert.ToInt32(it.Value);

					if (row.Length > 0)
						ChangeddAddressValue(row[0], Convert.ToInt32(it.Value), false, null, null);

				}

				if (this.evtPLCScan != null) ThevtPLCScan();

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}

		}
	}
}

#endif