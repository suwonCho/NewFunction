using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Function;

namespace PLCComm
{

	/// <summary>
	/// PLC 값을 조회 할때 일어나느 이벤트용 델리게이트
	/// </summary>
	public delegate void delPLCScan();

	/// <summary>
	/// PLC 상태가 변경 되는 경우 일어나느 이벤트용 델리 게이트
	/// </summary>
	/// <param name="status"></param>
	public delegate void delChConnectionStatus(enStatus status);


	/// <summary>
	/// plc 값 변경 시 일어나는 이벤트용 델리 게이트
	/// </summary>
	/// <param name="Address"></param>
	/// <param name="valueType"></param>
	/// <param name="value"></param>
	public delegate void delChAddressValue(string Address, enPLCValueType valueType, object value);

	/// <summary>
	/// PLC Address Value Struct
	/// </summary>
	public struct AddressValue
	{
		public object Value;
		public enPLCValueType ValueType;

		public AddressValue(object value, enPLCValueType valuetype )
		{
			Value = value;
			ValueType = valuetype;
		}
			
	}


	/// <summary>
	/// PLC 값 타입
	/// </summary>
	public enum enPLCValueType
	{		
		INT,
		HEX,
		STRING
	}




	class vari
	{
	}
}
