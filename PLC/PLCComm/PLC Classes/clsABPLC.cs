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
using Opc;

namespace PLCComm
{
	/// <summary>
	/// RSLinx를 이용한 plc통신 classs	
	/// </summary>
	class clsABPLC : iPLCCommInterface, IDisposable
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
		///System.Collections.ArrayList ItemList = new System.Collections.ArrayList();





#endregion



		/// <summary>
		/// RsLinx Opc 클래스 선어부..
		/// </summary>
		/// <param name="IPAddress">GateWay프로그램 IP / local이면 비워 둘것...</param>
		/// <param name="ProgramID">ProgramID / 보통 'RSLinx OPC Server'임 gateway쓸경우 'RSLinx Remote OPC Server'</param>
		/// <param name="GroupName">그룹네임 : empty일경우 'Group'</param>
		/// <param name="TopicName">토픽이름</param>
		/// <param name="UpdateRate">Item Value scan rate(ms)</param>
		public clsABPLC(string IPAddress, string ProgramID, string GroupName, string TopicName, int UpdateRate):base(true)
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
				_ConnctionStatus = enStatus.OK;
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

				add = ProgId;


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
			catch (Exception ex)
			{
				strMsg = "Open 실패 :" + ex.Message + strMsg + "\r\n" + ex.ToString();
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

				//OpcGrp.Refresh();


				/* 구 체크 로직....
				Opc.Da.Item item = OpcGrp.Items[0];
				
				//값을 정상 적으로 받지 못함..
				if (!item.Active) throw new Exception(string.Empty);
				
				if (opc.GetStatus().ServerState == Opc.Da.serverState.running && !this.bolOpcStatus)    //접속 정상
				{   //연결 회복..
					this.bolOpcStatus = true;
					_ConnctionStatus = enStatus.OK;					
				}
				*/


				bool isConn = false;


				//연결이 끊어 지면 모든 값이 0이된다.
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
					//연결 확인
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


					//쓰기명령을 처리 한다.
					foreach (DataRow dr in rows)
					{
						//dtWriteOrder.Rows[j]["Address"], dtWriteOrder.Rows[j]["Value"]);

						Item_Write(Fnc.obj2String(dr["Address"]), Fnc.obj2int(dr["value"]));

						dtWriteOrder.Rows.Remove(dr);
					}
				}


			}
			catch (Exception ex) //(System.Runtime.InteropServices.ExternalException ex)
			{   //접속이 끊겼을 경우 상태 체크도 에러 발생 하므로...

				Console.WriteLine(ex.ToString());

				if (this.bolOpcStatus)
				{
					this.bolOpcStatus = false;
					_ConnctionStatus = enStatus.Error;
				}
				//접속재시도..
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
		/// 그룹에 Item을 추가 하여 준다.
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
		/// 저장해 놓은 Item을 넣어 준다.- re Open시 사용
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
						//값이 어떻게 넘어 오는지 확인 하고 변경 할것
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

