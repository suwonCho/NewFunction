using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

namespace Function.api
{
	public class ProfileInI
	{
		//Public Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" 
		//(ByVal lpApplicationName As String, ByVal lpKeyName As Any, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Long, ByVal lpFileName As String) As Long
		
		[DllImport("kernel32.dll" ,CharSet = CharSet.Auto)]
		static extern uint GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSizem, string lpFileName);
			
			
		//Public Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" 
		//(ByVal lpApplicationName As String, ByVal lpKeyName As Any, ByVal lpString As Any, ByVal lpFileName As String) As Long

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		static extern uint WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString, string lpFileName);


		/// <summary>
		/// INI파일의 설정 정보를 가져온다
		/// </summary>
		/// <param name="section"></param>
		/// <param name="Item"></param>
		/// <param name="File_Name"></param>
		/// <param name="Default">값이 없거나 비어있을경우 사용값</param>
		/// <returns></returns>
		public static string GetINInString(string section, string Item, string File_Name , string Default = "")
		{
			try
			{
				StringBuilder stb = new StringBuilder(255);
				string tmp = string.Empty;
				long x;

				x = GetPrivateProfileString(section, Item, "", stb, stb.Capacity, File_Name);
				tmp = stb.ToString().Trim();

				return tmp.Length > 0 ? tmp : Default;
			}
			catch (Exception ex)
			{
				return Default;
			}
		}


		/// <summary>
		/// INI파일의 설정 정보를 설정한다
		/// </summary>
		/// <param name="section"></param>
		/// <param name="Key"></param>
		/// <param name="Writestr"></param>
		/// <param name="File_Name"></param>
		/// <returns></returns>
		public static bool WriteInIString(string section, string Key, string Writestr, string File_Name)
		{
			long worked = WritePrivateProfileString(section, Key, Writestr, File_Name);
    
			return worked > 0;
		}


	}
}
