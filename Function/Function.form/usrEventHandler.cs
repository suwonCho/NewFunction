using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Function.form
{
	public enum enEventKind
	{
		LOAD,
		TEXT_CHANGED,
		CLICK,
		DOUBLECLICK
	}

	public delegate void usrEventHander(object sender, usrEventArgs e);

	public class usrEventArgs : EventArgs
	{
		public enEventKind EventKind { get; set; }
	}



}
