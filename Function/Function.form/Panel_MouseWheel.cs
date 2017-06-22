using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Function.form
{


	


	/// <summary>
	/// 판넬에 마우스 휠을 처리 하여 준다.
	/// 사용법: 
	/// public partial class Form1 : Form
	///	{
	///      private ScrollPanelMessageFilter filter;

	///       public Form1()
	///       {
	///           InitializeComponent();
	///       }

	///       private void Form1_Activated(object sender, EventArgs e)
	///       {
	///           filter = new ScrollPanelMessageFilter(panel);
	///           Application.AddMessageFilter(filter);
	///       }

	///       private void Form1_Deactivate(object sender, EventArgs e)
	///       {
	///           Application.RemoveMessageFilter(filter);
	///       }
	///	}
	/// </summary>
	public class ScrollPanelMessageFilter : IMessageFilter
	{
	   int WM_MOUSEWHEEL = 0x20A;
	   Panel panel;
	   bool panelHasFocus = false;
	   [DllImport("user32.dll")]
	   static extern bool GetCursorPos(ref Point lpPoint);
	   [DllImport("User32.dll")]
	   static extern Int32 SendMessage(int hWnd, int Msg, int wParam, int lParam);

	   public ScrollPanelMessageFilter(Panel panel)
	   {
		   this.panel = panel;
		   //Go through each control on the panel and add an event handler.
		   //We need to know if a control on the panel has focus to prevent sending
		   //the scroll message a second time
		   AddFocusEvent(panel);
	   }

	   private void AddFocusEvent(Control parentControl)
	   {
		   foreach (Control control in parentControl.Controls)
		   {
			   if (control.Controls.Count == 0)
			   {
				   control.GotFocus += new EventHandler(control_GotFocus);
				   control.LostFocus += new EventHandler(control_LostFocus);
			   }
			   else
			   {
				   AddFocusEvent(control);
			   }
		   }
	   }

	   void control_GotFocus(object sender, EventArgs e)
	   {
		   panelHasFocus = true;
	   }

	   void control_LostFocus(object sender, EventArgs e)
	   {
		   panelHasFocus = false;
	   }

	   #region IMessageFilter Members
	   public bool PreFilterMessage(ref Message m)
	   {
		   //filter out all other messages except than mousewheel
		   //also only proceed with processing if the panel is focusable,            //no controls on the panel have focus 
		   //and the vertical scroll bar is visible
		   if (m.Msg == WM_MOUSEWHEEL && panel.CanFocus &&                !panelHasFocus && panel.VerticalScroll.Visible)
		   {
			   //is mouse cordinates over the panel display rectangle?
			   Rectangle rect = panel.RectangleToScreen(panel.ClientRectangle);
			   Point cursorPoint = new Point();
			   GetCursorPos(ref cursorPoint);
			   if ((cursorPoint.X > rect.X && cursorPoint.X < rect.X + rect.Width) &&
				   (cursorPoint.Y > rect.Y && cursorPoint.Y < rect.Y + rect.Height))
			   {
				   //send the mouse wheel message to the panel.
				   SendMessage((int)panel.Handle, m.Msg, (Int32)m.WParam, (Int32)m.LParam);
				   return true;
			   }
		   }
		   return false;
	   }
	   #endregion



	}
}
