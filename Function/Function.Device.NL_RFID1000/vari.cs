using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Function.Device.NL_RFID1000
{
	/// <summary>
	/// 테그 수신시 사용 델리게이드
	/// </summary>
	/// <param name="rowdata"></param>
	/// <param name="textdata"></param>
	public delegate void delTagReceive(object sender, string rowdata, string textdata);

	class vari
	{
	}
}
