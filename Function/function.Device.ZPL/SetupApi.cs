
//-------------------------------
// SetupApi.cs
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

// $Header: /cvsroot/z-bar/msvs/zbar/USB.Printing/setupapi.cs,v 1.4 2006/12/28 10:54:21 vinorodrigues Exp $

#if WindowsCE || PocketPC
#define WINCE
#endif

#if WINCE
#error This module is not intended for Mobile platform
#endif

using System;
using System.Text;
using System.Runtime.InteropServices;

namespace function.Device.ZPL
{

    internal class SetupApi
    {

        #region Consts

        internal const UInt32 DIGCF_DEFAULT = 0x00000001; // only valid with DIGCF_DEVICEINTERFACE
        internal const UInt32 DIGCF_PRESENT = 0x00000002;
        internal const UInt32 DIGCF_ALLCLASSES = 0x00000004;
        internal const UInt32 DIGCF_PROFILE = 0x00000008;
        internal const UInt32 DIGCF_DEVICEINTERFACE = 0x00000010;

        internal const UInt32 SPDRP_DEVICEDESC = 0x00000000; // DeviceDesc (R/W)
        internal const UInt32 SPDRP_HARDWAREID = 0x00000001; // HardwareID (R/W)
        internal const UInt32 SPDRP_COMPATIBLEIDS = 0x00000002; // CompatibleIDs (R/W)
        internal const UInt32 SPDRP_UNUSED0 = 0x00000003; // unused
        internal const UInt32 SPDRP_SERVICE = 0x00000004; // Service (R/W)
        // internal const UInt32 SPDRP_UNUSED1 = 0x00000005; // unused
        // internal const UInt32 SPDRP_UNUSED2 = 0x00000006; // unused
        internal const UInt32 SPDRP_CLASS = 0x00000007; // Class (R--tied to ClassGUID)
        internal const UInt32 SPDRP_CLASSGUID = 0x00000008; // ClassGUID (R/W)
        internal const UInt32 SPDRP_DRIVER = 0x00000009; // Driver (R/W)
        internal const UInt32 SPDRP_CONFIGFLAGS = 0x0000000A; // ConfigFlags (R/W)
        internal const UInt32 SPDRP_MFG = 0x0000000B; // Mfg (R/W)
        internal const UInt32 SPDRP_FRIENDLYNAME = 0x0000000C; // FriendlyName (R/W)
        internal const UInt32 SPDRP_LOCATION_INFORMATION = 0x0000000D; // LocationInformation (R/W)
        internal const UInt32 SPDRP_PHYSICAL_DEVICE_OBJECT_NAME = 0x0000000E; // PhysicalDeviceObjectName (R)
        internal const UInt32 SPDRP_CAPABILITIES = 0x0000000F; // Capabilities (R)
        internal const UInt32 SPDRP_UI_NUMBER = 0x00000010; // UiNumber (R)
        internal const UInt32 SPDRP_UPPERFILTERS = 0x00000011; // UpperFilters (R/W)
        internal const UInt32 SPDRP_LOWERFILTERS = 0x00000012; // LowerFilters (R/W)
        internal const UInt32 SPDRP_BUSTYPEGUID = 0x00000013; // BusTypeGUID (R)
        internal const UInt32 SPDRP_LEGACYBUSTYPE = 0x00000014; // LegacyBusType (R)
        internal const UInt32 SPDRP_BUSNUMBER = 0x00000015; // BusNumber (R)
        internal const UInt32 SPDRP_ENUMERATOR_NAME = 0x00000016; // Enumerator Name (R)
        internal const UInt32 SPDRP_SECURITY = 0x00000017; // Security (R/W, binary form)
        internal const UInt32 SPDRP_SECURITY_SDS = 0x00000018; // Security (W, SDS form)
        internal const UInt32 SPDRP_DEVTYPE = 0x00000019; // Device Type (R/W)
        internal const UInt32 SPDRP_EXCLUSIVE = 0x0000001A; // Device is exclusive-access (R/W)
        internal const UInt32 SPDRP_CHARACTERISTICS = 0x0000001B; // Device Characteristics (R/W)
        internal const UInt32 SPDRP_ADDRESS = 0x0000001C; // Device Address (R)
        internal const UInt32 SPDRP_UI_NUMBER_DESC_FORMAT = 0x0000001D; // UiNumberDescFormat (R/W)
        internal const UInt32 SPDRP_DEVICE_POWER_DATA = 0x0000001E; // Device Power Data (R)
        internal const UInt32 SPDRP_REMOVAL_POLICY = 0x0000001F; // Removal Policy (R)
        internal const UInt32 SPDRP_REMOVAL_POLICY_HW_DEFAULT = 0x00000020; // Hardware Removal Policy (R)
        internal const UInt32 SPDRP_REMOVAL_POLICY_OVERRIDE = 0x00000021; // Removal Policy Override (RW)
        internal const UInt32 SPDRP_INSTALL_STATE = 0x00000022; // Device Install State (R)
        internal const UInt32 SPDRP_MAXIMUM_PROPERTY = 0x00000023; // Upper bound on ordinals

        #endregion Consts

        #region Structs

        [StructLayout(LayoutKind.Sequential)]
        internal struct SP_DEVINFO_DATA
        {
            /// 
            /// Size of structure in bytes
            /// 
            public Int32 cbSize;
            /// 
            /// GUID of the device interface class
            /// 
            public Guid classGuid;
            /// 
            /// Handle to this device instance
            /// 
            public Int32 devInst;
            /// 
            /// Reserved; do not use. 
            /// 
            public UIntPtr reserved;
        };

        [StructLayout(LayoutKind.Sequential)]
        internal struct SP_DEVICE_INTERFACE_DATA
        {
            /// 
            /// Size of the structure, in bytes
            /// 
            public Int32 cbSize;
            /// 
            /// GUID of the device interface class
            /// 
            public Guid interfaceClassGuid;
            /// 
            /// 
            /// 
            public UInt32 flags;
            /// 
            /// Reserved; do not use.
            /// 
            public UIntPtr reserved;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
        public struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public UInt32 cbSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)] // will never be more than 256 in length
            public string devicePath;
        }

        public class NullClass
        {
            public NullClass()
            {
                throw new Exception("Cannot create instance of NullClass");
            }
        }

        #endregion Structs

        #region DLLImports

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SetupDiGetClassDevs(
        ref Guid ClassGuid,
        [MarshalAs(UnmanagedType.LPTStr)]
String Enumerator,
        IntPtr hwndParent,
        UInt32 Flags);

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)] // from PInvoke.net
        internal static extern UInt16 SetupDiDestroyDeviceInfoList(IntPtr hDevInfo);

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)] // from PInvoke.net
        internal static extern Boolean SetupDiEnumDeviceInterfaces(
        IntPtr hDevInfo,
        ref SP_DEVINFO_DATA devInfo,
        ref Guid interfaceClassGuid,
        UInt32 memberIndex,
        ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern Boolean SetupDiEnumDeviceInterfaces(
        IntPtr hDevInfo,
        int zero_devInfo, // used for 0 (Zero)
        ref Guid interfaceClassGuid,
        UInt32 memberIndex,
        ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData);

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true, EntryPoint = "SetupDiGetDeviceInterfaceDetail")]
        internal static extern Boolean SetupDiGetDeviceInterfaceDetail0(
        IntPtr hDevInfo,
        ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
        ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData, // OPTIONAL
        UInt32 deviceInterfaceDetailDataSize,
            /* out UInt32 */ NullClass requiredSize, // OPTIONAL
        ref SP_DEVINFO_DATA deviceInfoData); // OPTIONAL

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true, EntryPoint = "SetupDiGetDeviceInterfaceDetail")]
        internal static extern Boolean SetupDiGetDeviceInterfaceDetail0(
        IntPtr hDevInfo,
        ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
        NullClass deviceInterfaceDetailData, // OPTIONAL
        UInt32 deviceInterfaceDetailDataSize,
        out UInt32 requiredSize, // OPTIONAL
        NullClass deviceInfoData); // OPTIONAL

        /* [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern Boolean SetupDiGetDeviceInterfaceDetail(
        IntPtr hDevInfo,
        ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
        Int32 zero_deviceInterfaceDetailData, // used for 0 (Zero)
        UInt32 zero_deviceInterfaceDetailDataSize,
        out UInt32 requiredSize,
        Int32 zero_deviceInfoData); // used for 0 (Zero) */
        // KEEP

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true, EntryPoint = "SetupDiGetDeviceRegistryProperty")]
        internal static extern Boolean SetupDiGetDeviceRegistryProperty0(
        IntPtr hDevInfo,
        ref SP_DEVINFO_DATA deviceInfoData,
        UInt32 property,
            /* out IntPtr */ NullClass propertyRegDataType, // OPTIONAL
        StringBuilder propertyBuffer,
        UInt32 propertyBufferSize,
            /* out IntPtr */ NullClass requiredSize); // OPTIONAL

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true, EntryPoint = "SetupDiGetDeviceRegistryProperty")] // overloaded for null refs
        internal static extern Boolean SetupDiGetDeviceRegistryProperty0(
        IntPtr hDevInfo,
        ref SP_DEVINFO_DATA deviceInfoData,
        UInt32 property,
        out IntPtr propertyRegDataType, // OPTIONAL
        NullClass propertyBuffer,
        UInt32 propertyBufferSize, // set to 0 (Zero)
        out UInt32 requiredSize); // OPTIONAL

        #endregion DLLImports

    }
}
