using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Function.uScm
{
    public partial class usrStatusGrp : UserControl
    {
        
        
        int intStatus_cnt = 0;
        usrStatus[] usrSt;

        public int intCtrlWidth = 148;
        public Font defaultFont = new Font("����ü", 14, FontStyle.Bold);

        public usrStatusGrp()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ��Ʈ�ѿ� ǥ�õ� ����ĭ ����
        /// </summary>
        public void SetControl(string [] strHeader)
        {
            intStatus_cnt = strHeader.Length;

            //ȭ��ǥ�ÿ� ���� �⺻ ������ ����Ѵ�.
            int[] intSize = new int[] { intCtrlWidth, grpUsr.Height - 31 };

            int x = ( (grpUsr.Width - 6) - (intSize[0] * intStatus_cnt) + 38) / 2;
			
            int[] intPos = new int[] { x, 25 };

            usrSt = new usrStatus[intStatus_cnt];
			
            for(int i = 0; i < usrSt.Length; i++)
            {
                usrSt[i] = new usrStatus();

                grpUsr.Controls.Add(usrSt[i]);

                usrSt[i].Left = intPos[0];
                usrSt[i].Top = intPos[1];

                usrSt[i].Width = intSize[0];
                usrSt[i].Height = intSize[1];

                intPos[0] += usrSt[i].Width;

                usrSt[i].SetHeader(strHeader[i]);

                usrSt[i].lblInfo.Font = defaultFont;
				
                if (i + 1 == usrSt.Length)
                    usrSt[i].isShowArrow = false;

            }

			grpUsr.BringToFront();

			Application.DoEvents();
            
        }


        /// <summary>
        /// ���¸� �����Ѵ�.
        /// </summary>
        /// <param name="intIndex"></param>
        /// <param name="strInfo"></param>
        /// <param name="uStatus"></param>
        public void SetStatus(int intIndex, string strInfo, usrStatus.wError uStatus)
        {
            usrSt[intIndex].SetStatus(strInfo, uStatus);
            Application.DoEvents();
            
        }

        /// <summary>
        /// ���¸� �ʱ�ȭ �Ѵ�.
        /// </summary>
        public void ClearStatus()
        {
            for (int i = 0; i < usrSt.Length; i++)
            {
                usrSt[i].SetStatus(string.Empty, usrStatus.wError.None);

            }

        }

    }
}
