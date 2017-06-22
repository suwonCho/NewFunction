using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Function;

namespace Function.uScm
{
    public partial class usrProduct : UserControl
    {
		Color colBackColor;


		private void usrProductInit(bool isBorder, Color colbackColor)
		{
			InitializeComponent();

			if (!isBorder)
			{
				lblNo.BorderStyle = BorderStyle.None;
				lblInfo.BorderStyle = BorderStyle.None;
				lblType.BorderStyle = BorderStyle.None;
			}

			colBackColor = colbackColor;

			usrSetClear();
		}

        public usrProduct(bool isBorder, Color colbackColor)
        {
			usrProductInit(isBorder, colBackColor);
        }


		public usrProduct(bool isBorder, Color colbackColor, int SizeW, int SizeH)
		{
			usrProductInit(isBorder, colBackColor);

			this.Width = SizeW;
			this.Height = SizeH;
		}



        public void usrSetClear()
        {
			usrSetlblNo(string.Empty, Color.Black, colBackColor);
			usrSetlblInfo(string.Empty, Color.Black, colBackColor);
			usrSetlblType(string.Empty, Color.Black, colBackColor);
        }

        public void usrSetlblNo(string strText)
        {
            form.control.Invoke_Control_Text(lblNo, strText);
        }


        public void usrSetlblType(string strText)
        {
            form.control.Invoke_Control_Text(lblType, strText);
        }

        public void usrSetlblInfo(string strText)
        {
            form.control.Invoke_Control_Text(lblInfo, strText);
        }

		public void usrSetlblNo(string strText, Color colFore, Color colBack)
        {
			form.control.Invoke_Control_Text(lblNo, strText);
			form.control.Invoke_Control_Color(lblNo, colFore, colBack);
        }


        public void usrSetlblType(string strText, Color colFore, Color colBack)
        {
			form.control.Invoke_Control_Text(lblType, strText);
			form.control.Invoke_Control_Color(lblType, colFore, colBack);
        }

		public void usrSetlblInfo(string strText, Color colFore, Color colBack)
        {
			form.control.Invoke_Control_Text(lblInfo, strText);
			form.control.Invoke_Control_Color(lblInfo, colFore, colBack);
        }

        public void usrSetlblAll(string [] strText, Color colFore, Color colBack)
        {
			usrSetlblNo(strText[0], colFore, colBack);
			usrSetlblType(strText[1], colFore, colBack);
			usrSetlblInfo(strText[2], colFore, colBack);
        }


		public void usrSetSizeFont(Font usrFont, int [] intWidth)
		{
			int intLeft = 10;
			form.control.Invoke_Control_Font(lblNo, usrFont);
			form.control.Invoke_Control_Top(lblNo, intLeft);
			form.control.Invoke_Control_Width(lblNo, intWidth[0]);

			
			intLeft += intWidth[0] + 10;
			form.control.Invoke_Control_Font(lblNo, usrFont);
			form.control.Invoke_Control_Top(lblNo, intLeft);
			form.control.Invoke_Control_Width(lblNo, intWidth[1]);
			
			
			intLeft += intWidth[1] + 10;
			form.control.Invoke_Control_Font(lblNo, usrFont);
			form.control.Invoke_Control_Top(lblNo, intLeft);
			form.control.Invoke_Control_Width(lblNo, intWidth[2]);


		}
    }
}
