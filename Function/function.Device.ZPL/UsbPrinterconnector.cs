

//-------------------------------
// UsbPrinterconnector.cs
//-------------------------------

#region License
/* ---------------------------------------------------------------------------
* Creative Commons License
* http://creativecommons.org/licenses/by/2.5/au/
*
* Attribution 2.5 Australia
*
* You are free:
*
* - to copy, distribute, display, and perform the work 
* - to make derivative works 
* - to make commercial use of the work 
*
* Under the following conditions:
*
* Attribution: You must attribute the work in the manner specified by the
* author or licensor. 
*
* For any reuse or distribution, you must make clear to others the license
* terms of this work. Any of these conditions can be waived if you get
* permission from the copyright holder. Your fair use and other rights
* are in no way affected by the above.
*
* This is a human-readable summary of the Legal Code (the full license). 
* http://creativecommons.org/licenses/by/2.5/au/legalcode
* ------------------------------------------------------------------------ */

/* ---------------------------------------------------------------------------
* Special Note
* 
* A special mention and thanks to the contributions of several parties in
* blogging and publishing this complex API. Converting to C# was not easy!
* 
* The "setupapi.h" file from the Microsoft DDK for Windows XP SP2
* 
* Peter Skarpetis for this blog "GETTING A HANDLE ON USBPRINT.SYS"
* http://blog.peter.skarpetis.com
* 
* The Code Project - http://www.codeproject.com
* 
* P/Invoke .NET - http://www.pinvoke.net
* 
* ------------------------------------------------------------------------ */
#endregion License

// $Header: /cvsroot/z-bar/msvs/zbar/USB.Printing/ConUsb.cs,v 1.7 2006/12/28 10:54:21 vinorodrigues Exp $

#if WindowsCE || PocketPC
#define WINCE
#endif

#if WINCE
#error This module is not intended for Mobile platform
#endif

using System;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Collections.Specialized;
using System.IO;
using Microsoft.Win32;
using System.Threading;

namespace function.Device.ZPL
{

    /// 
    /// Connector leveraging usbmon.dll/usbprint.sys.

    /// Note: This cannot be used in more than one process at a time, so if the spooler is running you cannot use it.
    /// 
    internal class UsbPrinterConnector : PrinterConnector
    {

        #region EnumDevices

        static Guid GUID_DEVICEINTERFACE_USBPRINT = new Guid(
        0x28d78fad, 0x5a12, 0x11D1,
        0xae, 0x5b, 0x00, 0x00, 0xf8, 0x03, 0xa8, 0xc2);
        // static IntPtr INVALID_HANDLE_VALUE = (IntPtr)(-1);

        public static NameValueCollection EnumDevices()
        {
            return EnumDevices(true);
        }

        public static NameValueCollection EnumDevices(bool presentOnly)
        {
            return EnumDevices(presentOnly, true);
        }

        public static NameValueCollection EnumDevices(bool presentOnly, bool zebraOnly)
        {
            return EnumDevices(presentOnly, zebraOnly, true);
        }

        public static NameValueCollection EnumDevices(bool PresentOnly, bool ZebraOnly, bool fullDetail)
        {
            NameValueCollection res = new NameValueCollection();
            String name, path, desc, port;

            Guid intfce;
            IntPtr devs;
            SetupApi.SP_DEVINFO_DATA devinfo = new SetupApi.SP_DEVINFO_DATA();
            SetupApi.SP_DEVICE_INTERFACE_DATA devinterface = new SetupApi.SP_DEVICE_INTERFACE_DATA();
            SetupApi.SP_DEVICE_INTERFACE_DETAIL_DATA interface_detail;
            UInt32 devcount;
            UInt32 size;

            RegistryKey regKey, subKey;

            intfce = GUID_DEVICEINTERFACE_USBPRINT;

            UInt32 flags = SetupApi.DIGCF_DEVICEINTERFACE;
            if (PresentOnly)
                flags |= SetupApi.DIGCF_PRESENT;

            devs = SetupApi.SetupDiGetClassDevs(ref intfce,
            null,
            IntPtr.Zero,
            flags);
            if (devs == (IntPtr)FileIO.INVALID_HANDLE_VALUE)
                return null;

            devcount = 0;
            devinterface.cbSize = Marshal.SizeOf(typeof(SetupApi.SP_DEVICE_INTERFACE_DATA));

            while (SetupApi.SetupDiEnumDeviceInterfaces(
            devs,
            0,
            ref intfce,
            devcount,
            ref devinterface))
            {
                devcount++;

                SetupApi.SetupDiGetDeviceInterfaceDetail0(
                devs,
                ref devinterface,
                null,
                0,
                out size,
                null);

                if ((size > 0) &&
                (size <= (Marshal.SizeOf(typeof(SetupApi.SP_DEVICE_INTERFACE_DETAIL_DATA))) - sizeof(UInt32)))
                {

                    interface_detail = new SetupApi.SP_DEVICE_INTERFACE_DETAIL_DATA();
                    interface_detail.cbSize = (UInt32)(sizeof(UInt32) + sizeof(Char)); // Wow! This is a gotcha!

                    devinfo = new SetupApi.SP_DEVINFO_DATA();
                    devinfo.cbSize = Marshal.SizeOf(typeof(SetupApi.SP_DEVINFO_DATA));

                    if (SetupApi.SetupDiGetDeviceInterfaceDetail0(
                    devs,
                    ref devinterface,
                    ref interface_detail,
                    size,
                    null,
                    ref devinfo))
                    {
                        path = interface_detail.devicePath.ToString();
                        name = GetDeviceRegistryProperty(devs, ref devinfo, SetupApi.SPDRP_LOCATION_INFORMATION);
                        if (fullDetail)
                        {
                            desc = "";
                            port = "";

                            if (path.StartsWith("\\\\?\\"))
                            {
                                string key = "##?#" + path.Substring(4);

                                try
                                {
                                    regKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\DeviceClasses\\" +
                                    GUID_DEVICEINTERFACE_USBPRINT.ToString("B"));
                                    try
                                    {
                                        subKey = regKey.OpenSubKey(key);
                                        if (subKey != null)
                                        {
                                            subKey = subKey.OpenSubKey("#\\Device Parameters");
                                            desc = subKey.GetValue("Port Description").ToString();
                                            port = subKey.GetValue("Port Number").ToString();
                                            subKey.Close();
                                        }
                                    }
                                    finally
                                    {
                                        regKey.Close();
                                    }
                                }
                                catch
                                {
                                    // do nothing
                                }
                            }

                            if (ZebraOnly && (!desc.StartsWith("Zebra")))
                                continue;

                            res.Add(name, path);
                            res.Add(name, desc);
                            res.Add(name, port);
                        }
                        else
                            res.Add(name, path);
                    }
                    else
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                }

            }
            SetupApi.SetupDiDestroyDeviceInfoList(devs);

            return res;
        }

        private static String GetDeviceRegistryProperty(IntPtr hDevInfo,
        ref SetupApi.SP_DEVINFO_DATA deviceInfoData,
        UInt32 property)
        {
            IntPtr dataType = IntPtr.Zero;
            uint size;
            StringBuilder buffer;

            SetupApi.SetupDiGetDeviceRegistryProperty0(
            hDevInfo,
            ref deviceInfoData,
            property,
            out dataType,
            null,
            0,
            out size);

            if (size > 0)
            {
                buffer = new StringBuilder((int)size);

                if (SetupApi.SetupDiGetDeviceRegistryProperty0(
                hDevInfo,
                ref deviceInfoData,
                property,
                null,
                buffer,
                size,
                null))
                {
                    return buffer.ToString();
                }
                else
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            else
                return String.Empty;
        }

        #endregion EnumDevices

        private string interfaceName;

        private IntPtr usbHandle = IntPtr.Zero;

        public static readonly uint ReadBufferSize = 512;

        private byte[] readBuffer;

        /// 
        /// Constructor
        /// 
        /// 
        public UsbPrinterConnector(string InterfaceName)
            : base()
        {
            if (!InterfaceName.StartsWith("\\"))
            {
                NameValueCollection plist = EnumDevices(true, false, false);
                if (plist.GetValues(InterfaceName) != null)
                    InterfaceName = plist.GetValues(InterfaceName)[0];
                else
                    throw new Exception("Cannot locate USB device");
            }
            this.interfaceName = InterfaceName;
        }

        /// 
        /// Destructor
        /// 
        ~UsbPrinterConnector()
        {
            SetConnected(false);
        }

        protected override void SetConnected(bool value)
        {
            if (value)
            {
                if ((int)usbHandle > 0)
                    SetConnected(false);

                /* C++ Decl.
                usbHandle = CreateFile(
                interfacename, 
                GENERIC_WRITE, 
                FILE_SHARE_READ,
                NULL, 
                OPEN_ALWAYS, 
                FILE_ATTRIBUTE_NORMAL | FILE_FLAG_SEQUENTIAL_SCAN, 
                NULL);
                */

                usbHandle = FileIO.CreateFile(
                interfaceName,
                FileIO.FileAccess.GENERIC_WRITE | FileIO.FileAccess.GENERIC_READ,
                FileIO.FileShareMode.FILE_SHARE_READ,
                IntPtr.Zero,
                FileIO.FileCreationDisposition.OPEN_ALWAYS,
                FileIO.FileAttributes.FILE_ATTRIBUTE_NORMAL |
                FileIO.FileAttributes.FILE_FLAG_SEQUENTIAL_SCAN |
                FileIO.FileAttributes.FILE_FLAG_OVERLAPPED,
                IntPtr.Zero);
                if ((int)usbHandle <= 0)
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            else
                if ((int)usbHandle > 0)
                {
                    FileIO.CloseHandle(usbHandle);
                    usbHandle = IntPtr.Zero;
                }
        }

        protected override bool GetConnected()
        {
            return ((int)usbHandle > 0);
        }

        public override bool BeginSend()
        {
            return GetConnected();
        }

        public override void EndSend()
        {
            // do nothing
        }

		/// <summary>
		/// 프린터 포트에 string 명령을 날린다.
		/// </summary>
		/// <param name="strData"></param>
		public void Write(string strData)
		{
			string[] s = strData.Split(new string[] { "\r\n" }, StringSplitOptions.None);

			foreach (string ss in s)
			{
				//string s1 = " " + ss + " ";
				//byte[] byt = Encoding.Default.GetBytes(s1);
				//byt[0] = 0x1B;
				//byt[byt.Length - 1] = 0x0d;


				
				byte[] byt = Encoding.ASCII.GetBytes(ss);

				Send(byt, 0, byt.Length);
			}
		}



        public override int Send(byte[] buffer, int offset, int count)
        {
            // USB 1.1 WriteFile maximum block size is 4096
            uint size;
            byte[] bytes;

            if (!GetConnected())
                throw new Exception("Not connected");

            if (count > 4096)
            {
                throw new NotImplementedException(); // TODO: Copy byte array loop
            }
            else
            {
                bytes = new byte[count];
                Array.Copy(buffer, offset, bytes, 0, count);
                ManualResetEvent wo = new ManualResetEvent(false);
                NativeOverlapped ov = new NativeOverlapped();
                // ov.OffsetLow = 0; ov.OffsetHigh = 0;
                ov.EventHandle = wo.Handle;
                if (!FileIO.WriteFile(usbHandle, bytes, (uint)count, out size, ref ov))
                {
                    if (Marshal.GetLastWin32Error() == FileIO.ERROR_IO_PENDING)
                        wo.WaitOne(WriteTimeout, false);
                    else
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                FileIO.GetOverlappedResult(usbHandle, ref ov, out size, true);
                return (int)size;
            }
        }

        public override bool CanRead()
        {
            return true;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            // USB 1.1 ReadFile in block chunks of 64 bytes
            // USB 2.0 ReadFile in block chunks of 512 bytes
            uint read;

            if (readBuffer == null)
                readBuffer = new byte[ReadBufferSize];

            AutoResetEvent sg = new AutoResetEvent(false);
            NativeOverlapped ov = new NativeOverlapped();
            ov.OffsetLow = 0;
            ov.OffsetHigh = 0;
            ov.EventHandle = sg.Handle;

            if (!FileIO.ReadFile(usbHandle, readBuffer, ReadBufferSize, out read, ref ov))
            {
                if (Marshal.GetLastWin32Error() == FileIO.ERROR_IO_PENDING)
                    sg.WaitOne(ReadTimeout, false);
                else
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            FileIO.GetOverlappedResult(usbHandle, ref ov, out read, true);

            Array.Copy(readBuffer, 0, buffer, offset, read);
            return (int)read;
        }

    }

}