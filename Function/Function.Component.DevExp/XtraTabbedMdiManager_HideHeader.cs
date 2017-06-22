using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Function.Component.DevExp
{
	/// <summary>
	/// XtraTabbedMdiManager에 Header를 표시 하지 않는다.
	/// </summary>
	public class XtraTabbedMdiManager_HideHeader : DevExpress.XtraTabbedMdi.XtraTabbedMdiManager, DevExpress.XtraTab.IXtraTabProperties
	{
		DevExpress.Utils.DefaultBoolean DevExpress.XtraTab.IXtraTabProperties.ShowTabHeader { get { return DevExpress.Utils.DefaultBoolean.False; } }
	}
}
