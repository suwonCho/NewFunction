using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace function.apilite
{
    public class api
    {
        public const int VK_LSHIFT = 0xA0;
        public const int VK_RMENU = 0xA5;
        public const int VK_LMENU = 0xA4;
        public const int VK_LCONTROL = 0xA2;
        public const int VK_RCONTROL = 0xA3;
        public const int VK_LBUTTON = 0x01;
        public const int VK_RBUTTON = 0x02;
        public const int VK_MBUTTON = 0x04;
        public const int VK_XBUTTON1 = 0x05;
        public const int VK_XBUTTON2 = 0x06;

        [DllImport("user32.dll")]
        public static extern uint keybd_event(byte bvk, byte bscan, uint dwflags, ref int dwExtrainfo);

        /// <summary>
		/// 키입력을 보낸다. 특수 키등을 안되니 필요하면 구현 할것
		/// </summary>
		/// <param name="key"></param>
		/// <param name="Control"></param>
		/// <param name="alt"></param>
		/// <param name="shift"></param>
		/// <param name="interval"></param>
		public static void hookProk_KeySend(Keys[] key, bool Control, bool alt, bool shift, int interval = 50)
        {
            List<byte> lstKeys = new List<byte>();

            if (Control) lstKeys.Add((byte)VK_LCONTROL);
            if (alt) lstKeys.Add((byte)VK_LMENU);
            if (shift) lstKeys.Add((byte)VK_LSHIFT);

            int info = 0;
            uint iflag = 0;

            foreach (Keys k in key)
            {
                lstKeys.Add((byte)k);
            }

            for (int i = 0; i < 2; i++)
            {
                foreach (byte b in lstKeys)
                {
                    keybd_event(b, 0, iflag, ref info);
                }

                if (i == 0)
                {
                    iflag = 0x02;
                    System.Threading.Thread.Sleep(interval);
                }

            }

        }


    }   //end key
}
