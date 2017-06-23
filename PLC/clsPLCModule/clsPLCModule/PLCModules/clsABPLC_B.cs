using System;
using System.Threading;
using System.Runtime.InteropServices;
using System.Data;
using System.Collections;
using Function;


//RS-Linx 3.80 �������� ���
//RS-Linx ��ġ ������ OpcNetApi.dll, OpcNetApi.Com.dll, OpcNetApi.Xml.dll �߰� �Ǿ�� ��

/*������ �߰� Ȯ��
 // add items to the group    (in Rockwell names are identified like [Name of PLC in the server]Block of word:number of word,number of consecutive readed words)        
items[0] = new Opc.Da.Item();
items[0].ItemName = "[UNTITLED_1]N7:0,L2";//this reads 2 word (short - 16 bit)
items[1] = new Opc.Da.Item();
items[1].ItemName = "[UNTITLED_1]N11:0,L10";//this reads an array of 10 words (short[])
items[2] = new Opc.Da.Item();
items[2].ItemName = "[UNTITLED_1]B3:0,L2";//this read a 2 word array (but in the plc the are used as bits so you have to mask them)    
items = groupRead.AddItems(items);	
 */


#if (ABPLC_B)
using Opc;

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
		Opc.Da.Server opc;

		/// <summary>
		/// Opc Url
		/// </summary>
		Opc.URL url;

		/// <summary>
		/// Item Group
		/// </summary>
		Opc.Da.Subscription OpcGrp;

		/// <summary>
		/// Item Group State
		/// </summary>
		Opc.Da.SubscriptionState OpcGrp_State;


		OpcCom.Factory fact = new OpcCom.Factory();


		/// <summary>
		/// GateWay���α׷� IP / local�̸� ��� �Ѱ�..
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
				if (opc != null) close();

				string add;

				//�ּҸ� ����� �ش�.
				if(strIp.Equals(string.Empty))
				{
					add = $"opcda://localhost/{ProgId}";
				}
				else
				{
					add = $"opcda://{strIp}/{ProgId}";
				}


				opc = new Opc.Da.Server(fact, null);

				url = new Opc.URL(add);

				//������ ����
				opc.Connect(url, new Opc.ConnectData(new System.Net.NetworkCredential()));

				//���� �� - �׽�Ʈ �Ұ�
				//opc.ServerShutdown


				//�׷����
				OpcGrp_State = new Opc.Da.SubscriptionState();
				OpcGrp_State.Name = strGrpName;
				OpcGrp_State.UpdateRate = intUpdateRate;
				OpcGrp_State.Active = true;

				OpcGrp = (Opc.Da.Subscription)opc.CreateSubscription(OpcGrp_State);
				OpcGrp.DataChanged += OpcGrp_DataChanged;
				
				

								
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

				if (opc != null)
				{
					if(opc.IsConnected)
						opc.Disconnect();

					opc.Dispose();
					opc = null;
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
				
				if (OpcGrp == null)
				{
					if (bolOpcStatus)
						return;
					else
						throw new Exception(string.Empty);
				}

				if (OpcGrp.Items.Length < 1)
				{
					//if 	(bolOpcStatus) 
					return;
					//else
					//	throw new Exception(string.Empty);
				}

				OpcGrp.Refresh();

				Opc.Da.Item item = OpcGrp.Items[0];
				
				//���� ���� ������ ���� ����..
				if (!item.Active) throw new Exception(string.Empty);
								

				if (opc.GetStatus().ServerState != Opc.Da.serverState.running && !this.bolOpcStatus)    //���� ����
				{   //���� ȸ��..
					this.bolOpcStatus = true;
					this.ChConnection_Status(bolOpcStatus);
				}

				//�������� ó�� �Ѵ�.
				foreach(DataRow dr in dtWriteOrder.Rows)
				{
					//dtWriteOrder.Rows[j]["Address"], dtWriteOrder.Rows[j]["Value"]);

					Item_Write(Fnc.obj2String(dr["Address"]), Fnc.obj2int(dr["value"]));
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
				int intOpcItemCnt = OpcGrp.Items.Length;
				Opc.Da.Item i = new Opc.Da.Item();
				i.ItemName = strItem;

				OpcGrp.AddItems(new Opc.Da.Item[] { i });				
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

				Opc.Da.Item[] items = new Opc.Da.Item[ItemList.Count];

				foreach (object objItem in ItemList)
				{
					ItemName = objItem.ToString();

					items[intOpcItemCnt] = new Opc.Da.Item();
					items[intOpcItemCnt].ItemName = ItemName;
					
					intOpcItemCnt++;
					dtAddress_Add(ItemName);
				}

				OpcGrp.AddItems(items);

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



		///// <summary>
		///// PLC Item ���� �д´�.
		///// </summary>
		///// <param name="strItem">ItemName</param>
		///// <returns></returns>
		//public int Item_Read(string strItem)
		//{
			
		//	try
		//	{
		//		Opc.Da.Item item = OpcGrp.Items[strItem];

				
		//		OPCItem Item = OpcGrp.OPCItems.Item(ItemName);
		//		object objValue = Item.Value;
		//		return Convert.ToInt16(objValue.ToString(), 10);
		//	}
		//	catch (System.Runtime.InteropServices.ExternalException ex)
		//	{   //activex�̱� ������ ���� �߻��� ����� ���� �ʾ� �����...
		//		throw ComException(ex, ItemName.ToString());
		//	}
		//	catch (Exception ex)
		//	{
		//		throw ex;
		//	}
		//}



		/// <summary>
		/// Plc Item�� ���� ����..
		/// </summary>
		/// <param name="strItem">ItemName</param>
		/// <param name="lngValue">Writing Value</param>
		void Item_Write(string strItem, int intValue)
		{			
			try
			{
				bool isWork = false;
				Opc.Da.ItemValue iv = new Opc.Da.ItemValue();


				foreach(Opc.Da.Item i in OpcGrp.Items)
				{
					if(i.ItemName.Equals(strItem))
					{

						iv.ServerHandle = i.ServerHandle;
						isWork = true;
						break;
					}
				}

				if(!isWork)
				{
					//��� �ȵ� item�� ������ ����
					throw new Exception($"��Ͼȵ� ITEM[{strItem}�� �� ������ �õ� �Ͽ����ϴ�. Item��� �Ͽ� �ֽʽÿ�.");
				}

				iv.Value = intValue;

				OpcGrp.Write(new Opc.Da.ItemValue[] { iv });

				string strChanged = string.Format("[{0}] WorteValue : {1}", strItem, intValue);
				WroteAddressValue(strChanged, null, null);
			}
			catch (System.Runtime.InteropServices.ExternalException ex)
			{   //activex�̱� ������ ���� �߻��� ����� ���� �ʾ� �����...
				throw ComException(ex, strItem);
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



		private void OpcGrp_DataChanged(object subscriptionHandle, object requestHandle, Opc.Da.ItemValueResult[] values)
		{
			try
			{
				//�� ó�� �κ��� ���� ���� �ʿ� array �� short[] ó�� ��
				DataRow[] item;

				foreach (Opc.Da.ItemValueResult value in values)
				{
					item = this.dtAddress.Select("Address = '" + value.ItemName + "'");

					if (item.Length > 0)
					{
						short[] data = (short[])value.Value;

						ChangeddAddressValue(item[0], data[0], false, null, null);
					}
				}
			}
			catch(Exception ex)
			{
				PLCModule.clsPLCModule.LogWrite("OpcGrp_DataChanged", ex.ToString());
			}
		}


		
	}
}

#endif