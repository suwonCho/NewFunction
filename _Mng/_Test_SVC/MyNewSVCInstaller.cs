using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;

namespace _Test
{
	[RunInstaller(true)]
	public partial class MyNewSVCInstaller : System.Configuration.Install.Installer
	{
		public MyNewSVCInstaller()
		{
			InitializeComponent();
		}
	}
}
