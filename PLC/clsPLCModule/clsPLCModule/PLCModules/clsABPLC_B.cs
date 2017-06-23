using System;
using System.Threading;
using System.Runtime.InteropServices;
using System.Data;
using System.Collections;
using Function;


//RS-Linx 3.80 버전에서 사용
//RS-Linx 설치 폴더에 OpcNetApi.dll, OpcNetApi.Com.dll, OpcNetApi.Xml.dll 추가 되어야 함

/*아이템 추가 확인
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
	/// RSLinx를 이용한 plc통신 classs
	/// 참조에 Com항목에 'Rockwell Software OPC Automation' 항목을 등록하여 사용할것..
	/// </summary>
	class clsABPLC : clsPLCModuleInterface, IDisposable
	{

#region 변수 선언부
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
		/// GateWay프로그램 IP / local이면 비워 둘것..
		/// </summary>
		string strIp = string.Empty;
		/// <summary>
		/// ProgramID / 보통 'RSLinx OPC Server'임 gateway쓸경우 'RSLinx Remote OPC Server'
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
		/// Opc연결상태 최종 상태..
		/// </summary>
		bool bolOpcStatus = false;
		/// <summary>
		/// Opc 연결상태 체크 쓰레드타이머
		/// </summary>
		Timer tmrOpcCheck;
		/// <summary>
		/// 상태체크 중복 실행 방지 변수
		/// </summary>
		bool bolChecking = false;

		/// <summary>
		/// 커넥션 복구시 사용할 itemList
		/// </summary>
		System.Collections.ArrayList ItemList = new System.Collections.ArrayList();





#endregion



		/// <summary>
		/// RsLinx Opc 클래스 선어부..
		/// </summary>
		/// <param name="IPAddress">GateWay프로그램 IP / local이면 비워 둘것...</param>
		/// <param name="ProgramID">ProgramID / 보통 'RSLinx OPC Server'임 gateway쓸경우 'RSLinx Remote OPC Server'</param>
		/// <param name="GroupName">그룹네임 : empty일경우 'Group'</param>
		/// <param name="TopicName">토픽이름</param>
		/// <param name="UpdateRate">Item Value scan rate(ms)</param>
		public clsABPLC(string IPAddress, string ProgramID, string GroupName, string TopicName, int UpdateRate)
		{
			this.strIp = IPAddress.Trim();
			if (ProgramID.Trim() != string.Empty) this.ProgId = ProgramID.Trim();
			if (GroupName.Trim() != string.Empty) this.strGrpName = GroupName.Trim();
			if (TopicName.Trim() == string.Empty)
				throw new Exception("Topic이름이 설정 되어 있지 않습니다.");
			else
				this.strTopicName = string.Format("[{0}]", TopicName.Trim());

			if (UpdateRate > 0) this.intUpdateRate = UpdateRate;

		}



#region override 처리부	


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
		/// Opc와 연결을 합니다.
		/// </summary>
		/// <returns></returns>
		public override bool Open()
		{
			string strMsg = string.Format("(Node:{0} / ProgID : {1} )", strIp, ProgId);

			try
			{
				if (opc != null) close();

				string add;

				//주소를 만들어 준다.
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

				//서버에 연결
				opc.Connect(url, new Opc.ConnectData(new System.Net.NetworkCredential()));

				//연결 후 - 테스트 할것
				//opc.ServerShutdown


				//그룹생성
				OpcGrp_State = new Opc.Da.SubscriptionState();
				OpcGrp_State.Name = strGrpName;
				OpcGrp_State.UpdateRate = intUpdateRate;
				OpcGrp_State.Active = true;

				OpcGrp = (Opc.Da.Subscription)opc.CreateSubscription(OpcGrp_State);
				OpcGrp.DataChanged += OpcGrp_DataChanged;
				
				

								
				//opc 연결상태 체크 Timer를 시작한다.
				if (this.tmrOpcCheck != null)
				{
					tmrOpcCheck.Dispose();
					tmrOpcCheck = null;
				}

				//			this.bolOpcStatus = true;
				//			this.ChConnection_Status(bolOpcStatus);

				tmrOpcCheck = new Timer(new TimerCallback(tmrCheckOpcStatus), null, 0, intUpdateRate);

				//this.ChConnection_Status(true);

				strMsg = "Open 성공" + strMsg;

				return true;
			}
			catch (System.Runtime.InteropServices.ExternalException ex)
			{   //activex이기 때문에 에러 발생이 제대로 되지 않아 몇가지만...
				strMsg = "Open 실패 :" + ex.Message + strMsg + "\r\n" + ex.ToString();
				throw ComException(ex, string.Empty);
			}
			catch (Exception ex)
			{
				strMsg = "Open 실패 :" + ex.Message + strMsg + "\r\n" + ex.ToString();
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
			catch (Exception ex)        //opc 클라이언트가 종료 되면.. 에러 발생..
			{
				throw ex;
			}

			return true;
		}

		/// <summary>
		/// Opc 서버와 연결은 끊는다.
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
		/// opc연결 상태 체크 쓰레드..
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
				
				//값을 정상 적으로 받지 못함..
				if (!item.Active) throw new Exception(string.Empty);
								

				if (opc.GetStatus().ServerState != Opc.Da.serverState.running && !this.bolOpcStatus)    //접속 정상
				{   //연결 회복..
					this.bolOpcStatus = true;
					this.ChConnection_Status(bolOpcStatus);
				}

				//쓰기명령을 처리 한다.
				foreach(DataRow dr in dtWriteOrder.Rows)
				{
					//dtWriteOrder.Rows[j]["Address"], dtWriteOrder.Rows[j]["Value"]);

					Item_Write(Fnc.obj2String(dr["Address"]), Fnc.obj2int(dr["value"]));
				}



			}
			catch (Exception ex) //(System.Runtime.InteropServices.ExternalException ex)
			{   //접속이 끊겼을 경우 상태 체크도 에러 발생 하므로...

				Console.WriteLine(ex.ToString());

				if (this.bolOpcStatus)
				{
					this.bolOpcStatus = false;
					this.ChConnection_Status(bolOpcStatus);
				}
				//접속재시도..
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
		/// activex에서 넘어오는 에러 처리.
		/// </summary>
		/// <param name="ex"></param>
		private Exception ComException(System.Runtime.InteropServices.ExternalException ex, string Info)
		{
			Exception Ex;
			switch (ex.ErrorCode)
			{
				case -2147467259:   //ProgramID가 잘못 지적 되어있거나, ItemId가 그룹에 등록 않되어 있을 경우
					if (Info == string.Empty)
						Ex = new Exception("Opc Open Error : ProramID를 찾지 못하였습니다. 설정 값을 확인 하여 주십시요", ex);
					else
						if (this.bolOpcStatus)
						Ex = new Exception("Item Error : 해당 Item이 등록되어 있지 않습니다.(" + Info + ")", ex);
					else
						Ex = new Exception("Opc와 연결이 끊어 졌습니다.", ex);

					break;
				case -1073479673:
					if (this.bolOpcStatus)
						Ex = new Exception("Item Error : 해당 Item이 등록되어 있지 않습니다.(" + Info + ")", ex);
					else
						Ex = new Exception("Opc와 연결이 끊어 졌습니다.", ex);
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
		/// 그룹에 Item을 추가 하여 준다.
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
			{   //activex이기 때문에 에러 발생이 제대로 되지 않아 몇가지만...
				throw ComException(ex, ItemName);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// 저장해 놓은 Item을 넣어 준다.- re Open시 사용
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
			{   //activex이기 때문에 에러 발생이 제대로 되지 않아 몇가지만...
				throw ComException(ex, ItemName);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}



		///// <summary>
		///// PLC Item 값을 읽는다.
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
		//	{   //activex이기 때문에 에러 발생이 제대로 되지 않아 몇가지만...
		//		throw ComException(ex, ItemName.ToString());
		//	}
		//	catch (Exception ex)
		//	{
		//		throw ex;
		//	}
		//}



		/// <summary>
		/// Plc Item에 값을 쓴다..
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
					//등록 안된 item을 쓸려고 했음
					throw new Exception($"등록안된 ITEM[{strItem}에 값 변경을 시도 하였습니다. Item등록 하여 주십시요.");
				}

				iv.Value = intValue;

				OpcGrp.Write(new Opc.Da.ItemValue[] { iv });

				string strChanged = string.Format("[{0}] WorteValue : {1}", strItem, intValue);
				WroteAddressValue(strChanged, null, null);
			}
			catch (System.Runtime.InteropServices.ExternalException ex)
			{   //activex이기 때문에 에러 발생이 제대로 되지 않아 몇가지만...
				throw ComException(ex, strItem);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}




#region IDisposable 멤버

		public void Dispose()
		{
			this.Close();

		}

		#endregion



		private void OpcGrp_DataChanged(object subscriptionHandle, object requestHandle, Opc.Da.ItemValueResult[] values)
		{
			try
			{
				//값 처리 부분은 추후 변경 필요 array 및 short[] 처리 부
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