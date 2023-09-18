
//-------------------------------
//FileIO.cs
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
* blogging and publishing this complex API. 
* 
* P/Invoke .NET - http://www.pinvoke.net
* 
* MSDN Magazine
* 
* ------------------------------------------------------------------------ */
#endregion License

// $Header: /cvsroot/z-bar/msvs/zbar/USB.Printing/fileio.cs,v 1.4 2006/11/16 10:55:04 vinorodrigues Exp $

#if WindowsCE || PocketPC
#define WINCE
#endif

using System;
using System.Runtime.InteropServices;
using System.Threading;

#if WINCE
#error This module is not intended for Mobile platform
#endif

namespace function.Device.ZPL
{
    internal class FileIO
    {

        internal const int INVALID_HANDLE_VALUE = -1;

        internal const int ERROR_FILE_NOT_FOUND = 2;
        internal const int ERROR_INVALID_NAME = 123;
        internal const int ERROR_ACCESS_DENIED = 5;
        internal const int ERROR_IO_PENDING = 997;
        internal const int ERROR_IO_INCOMPLETE = 996;

        internal class NullClass
        {
            public NullClass()
            {
                throw new Exception("Cannot create instance of NullClass");
            }
        }

        #region CreateFile

        [Flags]
        internal enum FileAccess : uint // from winnt.h
        {
            GENERIC_READ = 0x80000000,
            GENERIC_WRITE = 0x40000000,
            GENERIC_EXECUTE = 0x20000000,
            GENERIC_ALL = 0x10000000
        }

        [Flags]
        internal enum FileShareMode : uint // from winnt.h
        {
            FILE_SHARE_READ = 0x00000001,
            FILE_SHARE_WRITE = 0x00000002,
            FILE_SHARE_DELETE = 0x00000004
        }

        internal enum FileCreationDisposition : uint // from winbase.h
        {
            CREATE_NEW = 1,
            CREATE_ALWAYS = 2,
            OPEN_EXISTING = 3,
            OPEN_ALWAYS = 4,
            TRUNCATE_EXISTING = 5
        }

        [Flags]
        internal enum FileAttributes : uint // from winnt.h
        {
            FILE_ATTRIBUTE_READONLY = 0x00000001,
            FILE_ATTRIBUTE_HIDDEN = 0x00000002,
            FILE_ATTRIBUTE_SYSTEM = 0x00000004,
            FILE_ATTRIBUTE_DIRECTORY = 0x00000010,
            FILE_ATTRIBUTE_ARCHIVE = 0x00000020,
            FILE_ATTRIBUTE_DEVICE = 0x00000040,
            FILE_ATTRIBUTE_NORMAL = 0x00000080,
            FILE_ATTRIBUTE_TEMPORARY = 0x00000100,
            FILE_ATTRIBUTE_SPARSE_FILE = 0x00000200,
            FILE_ATTRIBUTE_REPARSE_POINT = 0x00000400,
            FILE_ATTRIBUTE_COMPRESSED = 0x00000800,
            FILE_ATTRIBUTE_OFFLINE = 0x00001000,
            FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000,
            FILE_ATTRIBUTE_ENCRYPTED = 0x00004000,

            // from winbase.h
            FILE_FLAG_WRITE_THROUGH = 0x80000000,
            FILE_FLAG_OVERLAPPED = 0x40000000,
            FILE_FLAG_NO_BUFFERING = 0x20000000,
            FILE_FLAG_RANDOM_ACCESS = 0x10000000,
            FILE_FLAG_SEQUENTIAL_SCAN = 0x08000000,
            FILE_FLAG_DELETE_ON_CLOSE = 0x04000000,
            FILE_FLAG_BACKUP_SEMANTICS = 0x02000000,
            FILE_FLAG_POSIX_SEMANTICS = 0x01000000,
            FILE_FLAG_OPEN_REPARSE_POINT = 0x00200000,
            FILE_FLAG_OPEN_NO_RECALL = 0x00100000,
            FILE_FLAG_FIRST_PIPE_INSTANCE = 0x00080000
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr CreateFile(
        string lpFileName,
        FileAccess dwDesiredAccess,
        FileShareMode dwShareMode,
        IntPtr lpSecurityAttributes,
        FileCreationDisposition dwCreationDisposition,
        FileAttributes dwFlagsAndAttributes,
        IntPtr hTemplateFile);

        #endregion CreateFile

        #region CloseHandle

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseHandle(IntPtr hObject);

        #endregion CloseHandle

        #region GetOverlappedResult

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetOverlappedResult(
        IntPtr hFile,
            /* IntPtr */ ref System.Threading.NativeOverlapped lpOverlapped,
        out uint nNumberOfBytesTransferred,
        bool bWait);

        #endregion GetOverlappedResult

        #region WriteFile

        [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "WriteFile")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool WriteFile0(
        IntPtr hFile,
        [MarshalAs(UnmanagedType.LPArray)]
byte[] lpBuffer,
        uint nNumberOfBytesToWrite,
        out uint lpNumberOfBytesWritten,
        NullClass lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool WriteFile(
        IntPtr hFile,
        [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer,
        uint nNumberOfBytesToWrite,
        out uint lpNumberOfBytesWritten,
        [In] ref System.Threading.NativeOverlapped lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int WriteFileEx(
        IntPtr hFile,
        [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer,
        int nNumberOfBytesToWrite,
        [In] ref System.Threading.NativeOverlapped lpOverlapped,
        [MarshalAs(UnmanagedType.FunctionPtr)] IOCompletionCallback callback
        );

        #endregion WriteFile

        #region ReadFile

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ReadFile(
        IntPtr hFile,
        [MarshalAs(UnmanagedType.LPArray)] [Out] byte[] lpBuffer,
        uint nNumberOfBytesToRead,
        out uint lpNumberOfBytesRead,
        [In] ref System.Threading.NativeOverlapped lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "ReadFile")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ReadFile0(
        IntPtr hFile,
        [MarshalAs(UnmanagedType.LPArray)] [Out] byte[] lpBuffer,
        uint nNumberOfBytesToRead,
        out uint lpNumberOfBytesRead,
        NullClass lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int ReadFileEx(
        IntPtr hFile,
        [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer,
        int nNumberOfBytesToRead,
        [In] ref System.Threading.NativeOverlapped lpOverlapped,
        [MarshalAs(UnmanagedType.FunctionPtr)] IOCompletionCallback callback);

        #endregion ReadFile

    }
}