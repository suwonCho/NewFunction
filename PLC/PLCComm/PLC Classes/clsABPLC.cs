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
using Opc;

namespace PLCComm
{
	/// <summary>
	/// RSLinx�� �̿��� plc��� classs	
	/// </summary>
	class clsABPLC : iPLCCommInterface, IDisposable
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
		///System.Collections.ArrayList ItemList = new System.Collections.ArrayList();





#endregion



		/// <summary>
		/// RsLinx Opc Ŭ���� �����..
		/// </summary>
		/// <param name="IPAddress">GateWay���α׷� IP / local�̸� ��� �Ѱ�...</param>
		/// <param name="ProgramID">ProgramID / ���� 'RSLinx OPC Server'�� gateway����� 'RSLinx Remote OPC Server'</param>
		/// <param name="GroupName">�׷���� : empty�ϰ�� 'Group'</param>
		/// <param name="TopicName">�����̸�</param>
		/// <param name="UpdateRate">Item Value scan rate(ms)</param>
		public clsABPLC(string IPAddress, string ProgramID, string GroupName, string TopicName, int UpdateRate):base(true)
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
				_ConnctionStatus = enStatus.OK;
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

				add = ProgId;


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
			catch (Exception ex)
			{
				strMsg = "Open ���� :" + ex.Message + strMsg + "\r\n" + ex.ToString();
				throw ex;
			}
			finally
			{
				LogWrite("Open", strMsg);
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
					_ConnctionStatus = enStatus.Error;
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
				LogWrite("Close", strMsg);
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

				//OpcGrp.Refresh();


				/* �� üũ ����....
				Opc.Da.Item item = OpcGrp.Items[0];
				
				//���� ���� ������ ���� ����..
				if (!item.Active) throw new Exception(string.Empty);
				
				if (opc.GetStatus().ServerState == Opc.Da.serverState.running && !this.bolOpcStatus)    //���� ����
				{   //���� ȸ��..
					this.bolOpcStatus = true;
					_ConnctionStatus = enStatus.OK;					
				}
				*/


				bool isConn = false;


				//������ ���� ���� ��� ���� 0�̵ȴ�.
				foreach(DataRow r in dtAddress.Rows)
				{
					if(r["Value"] != DBNull.Value)
					{
						int v = 0;

						if (int.TryParse(Fnc.obj2String(r["Value"]), out v))
						{
							if (v != 0) isConn = true;
						}
						else
							isConn = true;

					}
					//���� Ȯ��
					if (isConn) break;
					
				}

				if(isConn != this.bolOpcStatus)
				{
					bolOpcStatus = isConn;
					_ConnctionStatus = isConn ? enStatus.OK : enStatus.Error;
				}


				if (_ConnctionStatus == enStatus.OK && dtWriteOrder.Rows.Count > 0)
				{
					DataRow[] rows = new DataRow[dtWriteOrder.Rows.Count];

					dtWriteOrder.Rows.CopyTo(rows, 0);


					//�������� ó�� �Ѵ�.
					foreach (DataRow dr in rows)
					{
						//dtWriteOrder.Rows[j]["Address"], dtWriteOrder.Rows[j]["Value"]);

						Item_Write(Fnc.obj2String(dr["Address"]), Fnc.obj2int(dr["value"]));

						dtWriteOrder.Rows.Remove(dr);
					}
				}


			}
			catch (Exception ex) //(System.Runtime.InteropServices.ExternalException ex)
			{   //������ ������ ��� ���� üũ�� ���� �߻� �ϹǷ�...

				Console.WriteLine(ex.ToString());

				if (this.bolOpcStatus)
				{
					this.bolOpcStatus = false;
					_ConnctionStatus = enStatus.Error;
				}
				//������õ�..
				try
				{
					//this.close();
					//this.Open();
					//this.Item_ReAdd_All();
				}
				catch { }
			}
			finally
			{

				bolChecking = false;
			}

		}

		public override bool AddAddress(string Address, enPLCValueType type = enPLCValueType.INT)
		{

			Item_Add(Address);

			return base.AddAddress(Address, type);
		}

		

		/// <summary>
		/// �׷쿡 Item�� �߰� �Ͽ� �ش�.
		/// </summary>
		/// <param name="strItem"></param>
		private void Item_Add(string strItem)
		{
			string ItemName = string.Empty;

			try
			{
				int intOpcItemCnt = OpcGrp.Items.Length;
				Opc.Da.Item i = new Opc.Da.Item();
				i.ItemName = strItem;

				OpcGrp.AddItems(new Opc.Da.Item[] { i });				
				
			}			
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// ������ ���� Item�� �־� �ش�.- re Open�� ���
		/// </summary>
		private void Item_ReAdd_All()
		{
			string ItemName = string.Empty;
			try
			{
				int intOpcItemCnt = 0;

				Opc.Da.Item[] items = new Opc.Da.Item[dtAddress.Rows.Count];

				foreach (DataRow r in dtAddress.Rows)
				{
					ItemName = Fnc.obj2String(r["Address"]);

					items[intOpcItemCnt] = new Opc.Da.Item();
					items[intOpcItemCnt].ItemName = ItemName;					
					intOpcItemCnt++;					
				}

				OpcGrp.AddItems(items);

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
				

				foreach (Opc.Da.ItemValueResult value in values)
				{
					short d;
					short[] data;

					if (short.TryParse(value.Value.ToString(), out d))
					{
						data = new short[] { d };
					}
					else
					{
						//���� ��� �Ѿ� ������ Ȯ�� �ϰ� ���� �Ұ�
						data = (short[])value.Value;
					}

					
					ChangeddAddressValue(value.ItemName, data, false, null, null);
				
				}
			}
			catch(Exception ex)
			{
				LogWrite("OpcGrp_DataChanged", ex.ToString());
			}
		}


		
	}
}

