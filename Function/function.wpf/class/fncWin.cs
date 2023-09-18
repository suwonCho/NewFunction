using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace function.wpf
{
    public class fncWin
    {
        /// <summary>
        /// 셑팅 문자열을 가지고 온다. 
        /// 포맷) /*윈도우이름:셑팅문자열*/
        /// </summary>
        /// <param name="value"></param>
        /// <param name="winName"></param>
        /// <returns></returns>
        public static string getSettingString(string value, string winName)
        {
            string rtn = string.Empty;

            string winNM = string.Format("/*{0}:", winName.ToUpper());
            int iSt = value.IndexOf(winNM);
            if (iSt < 0) return rtn;
            int ied = value.IndexOf("*/", iSt + 1);

            rtn = value.Substring(iSt + winNM.Length, ied - (iSt + winNM.Length));

            



            return rtn;

        }

        /// <summary>
        /// 설정값에서 해당 윈도우의 설정값을 제거하고 가져온다.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="winName"></param>
        /// <returns></returns>
        public static string recvSettingString(string value, string winName)
        {
            string winNM = string.Format("/*{0}:", winName.ToUpper());

            //폼내용은 전부 삭제
            while (true)
            {
                int iSt = value.IndexOf(winNM);
                if (iSt < 0) break;
                int ied = value.IndexOf("*/", iSt + 1);

                if (ied < 0)
                    value = value.Substring(0, iSt);
                else
                    value = value.Substring(0, iSt) + (ied + 2 >= value.Length ? string.Empty : value.Substring(ied + 2));
            }

            return value;
        }


    }   //end class
}
