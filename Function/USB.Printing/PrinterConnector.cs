
//-------------------------------
// PrinterConnector.cs
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
#endregion License

// $Header: /cvsroot/z-bar/msvs/zbar/USB.Printing/Connect.cs,v 1.7 2006/11/16 10:55:04 vinorodrigues Exp $

using System;
using System.Collections.Generic;
using System.Text;

namespace USB.Printing
{

    internal abstract class PrinterConnector
    {

        protected abstract void SetConnected(bool value);

        protected abstract bool GetConnected();

        public bool IsConnected
        {
            get { return GetConnected(); }
            set { SetConnected(value); }
        }

        public static readonly int DefaultReadTimeout = 200;

        private int readTimeout = DefaultReadTimeout;

        public int ReadTimeout
        {
            get { return readTimeout; }
            set { readTimeout = value; }
        }

        public static readonly int DefaultWriteTimeout = 200;

        private int writeTimeout = DefaultWriteTimeout;

        public int WriteTimeout
        {
            get { return writeTimeout; }
            set { writeTimeout = value; }
        }

        /* public bool Connect()
        {
        SetConnected(true);
        return GetConnected();
        } */

        /* public void Disconnect()
        {
        SetConnected(false);
        } */

        public int Send(byte[] buffer)
        {
            return Send(buffer, 0, buffer.Length);
        }

        public abstract bool BeginSend();

        public abstract void EndSend();

        public abstract int Send(byte[] buffer, int offset, int count);

        public virtual bool CanRead()
        {
            return false;
        }

        /// 
        /// Reads data from the incomming connection.
        /// 
        /// populated data buffer or null if empty/unsuccessful
        /// Number of bytes read or -1 if unsuccessful
        public abstract int Read(byte[] buffer, int offset, int count);

    }

}