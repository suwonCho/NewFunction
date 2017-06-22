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
	/// RSLinx를 이용한 plc통신 classs
	/// 참조에 Com항목에 'Rockwell Software OPC Automation' 항목을 등록하여 사용할것..
	/// </summary>
	class clsABPLC : clsPLCModuleInterface, IDisposable
	{

#region 변수 선언부
		/// <summary>
		/// OpcClass
		/// </summary>
		OPCServer Opc;

		OPCGroup OpcGrp;

		/// <summary>
		/// GateWay프로그램 IP / local이면 비워 둘것...
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
				if (Opc != null) close();

				Opc = new OPCServer();
				Opc.Connect(ProgId, (object)strIp);

				//OpcGrp = new OPCGroup();			

				OpcGrp = Opc.OPCGroups.Add(strGrpName);

				OpcGrp.IsActive = true;
				OpcGrp.UpdateRate = intUpdateRate;
				OpcGrp.IsSubscribed = true;

				OpcGrp.DataChange += new DIOPCGroupEvent_DataChangeEventHandler(OpcGrp_DataChange);

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


		/*
		public override bool AddAddress(int intAddress)
		{
			throw new Exception("구현 되지 않는 기능 입니다.");
		}

		public override bool AddAddress(int intAddress, int intLength)
		{
			throw new Exception("구현 되지 않는 기능 입니다.");
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
			throw new Exception("구현 되지 않는 기능 입니다.");
		}
		public override bool DelAddress(string[] strAddress)
		{
			throw new Exception("구현 되지 않는 기능 입니다.");
		}



		public override int GetValue(int intAddress)
		{
			throw new Exception("구현 되지 않는 기능 입니다.");
		}
		public override int GetValue(int intAddress, int intLength)
		{
			throw new Exception("구현 되지 않는 기능 입니다.");
		}
		public override string GetValue(int intAddress, int intLength, int intRetrunLength)
		{
			throw new Exception("구현 되지 않는 기능 입니다.");
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
			throw new Exception("구현 되지 않는 기능 입니다.");
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
		/// opc연결 상태 체크 쓰레드..
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

				//값을 정상 적으로 받지 못함..
				if (item.Quality == 0) throw new Exception(string.Empty);


				if (this.Opc.ServerState == 1 && !this.bolOpcStatus)    //접속 정상
				{   //연결 회복..
					this.bolOpcStatus = true;
					this.ChConnection_Status(bolOpcStatus);
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
				ItemName = strTopicName + strItem;
				int intOpcItemCnt = OpcGrp.OPCItems.Count;
				OpcGrp.OPCItems.AddItem(ItemName, intOpcItemCnt);
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
				foreach (object objItem in ItemList)
				{
					ItemName = objItem.ToString();
					OpcGrp.OPCItems.AddItem(ItemName, intOpcItemCnt);
					intOpcItemCnt++;
					dtAddress_Add(ItemName);
				}
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
		/// PLC Item 값을 읽는다.
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
			{   //activex이기 때문에 에러 발생이 제대로 되지 않아 몇가지만...
				throw ComException(ex, ItemName.ToString());
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}



		/// <summary>
		/// Plc Item에 값을 쓴다..
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
			{   //activex이기 때문에 에러 발생이 제대로 되지 않아 몇가지만...
				throw ComException(ex, ItemName.ToString());
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