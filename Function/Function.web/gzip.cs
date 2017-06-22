using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.GZip;
using System.IO;
using System.Runtime.InteropServices;
using System.Net;

namespace Function.web
{
	public class Browser
	{
		public static Stream GZip_Decoding(Stream httpStream)
		{
			return new GZipInputStream(httpStream);
		}


		[DllImport("wininet.dll", SetLastError = true)]
		public static extern bool InternetGetCookieEx(
			string url,
			string cookieName,
			StringBuilder cookieData,
			ref int size,
			Int32 dwFlags,
			IntPtr lpReserved);

		private const Int32 InternetCookieHttponly = 0x2000;

		/// <summary>  
		/// Gets the URI cookie container.  
		/// </summary>  
		/// <param name="uri">The URI.  
		/// <returns></returns>  
		public static CookieContainer GetUriCookieContainer(Uri uri)
		{
			CookieContainer cookies = null;
			// Determine the size of the cookie  
			int datasize = 8192 * 16;
			StringBuilder cookieData = new StringBuilder(datasize);
			if (!InternetGetCookieEx(uri.ToString(), null, cookieData, ref datasize, InternetCookieHttponly, IntPtr.Zero))
			{
				if (datasize < 0)
					return null;
				// Allocate stringbuilder large enough to hold the cookie  
				cookieData = new StringBuilder(datasize);
				if (!InternetGetCookieEx(
				uri.ToString(),
				null, cookieData,
				ref datasize,
				InternetCookieHttponly,
				IntPtr.Zero))
					return null;
			}

			if (cookieData.Length > 0)
			{
				cookies = new CookieContainer();
				cookies.SetCookies(uri, cookieData.ToString().Replace(';', ','));
				//MessageBox.Show(cookieData.ToString());  
			}
			return cookies;
		}

	}




}
