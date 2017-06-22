using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mshtml;
using SHDocVw;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;


namespace Function.web
{
	public class InternetExplorer : IDisposable
    {
		public SHDocVw.InternetExplorer ie;

		public HTMLDocument HTMLDoc
		{
			get
			{
				return ie.Document as HTMLDocument;
			}
		}

		public IHTMLElement HTMLElement = null;

		public InternetExplorer()
		{
			init_ie();
		}

		void ie_WindowStateChanged(uint dwWindowStateFlags, uint dwValidFlagsMask)
		{
			
		}


		private void init_ie()
		{
			ie = new SHDocVw.InternetExplorer(); 
		}



		/// <summary>
		/// 
		/// </summary>
		public bool Visable
		{
			get { return ie.Visible; }
			set
			{
				ie.Visible = value;
			}
		}
		

		/// <summary>
		/// 웹페이지를 이동한다.
		/// </summary>
		/// <param name="uri"></param>
		/// <param name="isWait">완료 할 때까지 대기 여부</param>
		public void Navigate(string uri, bool isWait = false)
		{
			try
			{
				bool v =ie.Visible;
			}
			catch
			{
				init_ie();
			}
			

			ie.Navigate(uri);

			if (!isWait) return;

			//페이지 로딩시간이 필요하다
			while (ie.Busy)
			{
				Thread.Sleep(100);
			}

		}


		/// <summary>
		/// 페이지를 갱신한다
		/// </summary>
		/// <param name="isWait"></param>
		public void Refresh(bool isWait = false)
		{
			ie.Refresh();

			//페이지 로딩시간이 필요하다
			while (ie.Busy)
			{
				Thread.Sleep(100);
			}
		}


		public IHTMLElementCollection FindElementsByName(string name)
		{
			return HTMLDoc.getElementsByName(name);
        }


		public string Node_InnerText(int idx)
		{
			try
			{
				int i = 0;
				string rtn = null;

				foreach (var v in HTMLDoc.childNodes)
				{
					if (i == idx)
					{
						rtn = v.innerHTML;
						break;
					}

					i++;
				}


				return rtn;
			}
			catch
			{
				return null;
			}
        }



		/// <summary>
		/// 웹에 이미지를 폴더에 저장한다.
		/// </summary>
		/// <param name="FolderPath"></param>
		public void AllImage_Save(string FolderPath)
		{
			string strElName;
			mshtml.IHTMLElement2 body2 = (mshtml.IHTMLElement2)HTMLDoc.body;
			mshtml.IHTMLControlRange controlRange = (mshtml.IHTMLControlRange)body2.createControlRange();			
			IHTMLElementCollection imgs = HTMLDoc.images;
			

			foreach (mshtml.HTMLImg objImg in imgs)
			{
				controlRange.add((mshtml.IHTMLControlElement)objImg);
				controlRange.execCommand("Copy", false, System.Reflection.Missing.Value);
				controlRange.remove(0);
				strElName = objImg.nameProp;

				if (Clipboard.GetDataObject() != null)
				{
					IDataObject data = Clipboard.GetDataObject();

					if (data.GetDataPresent(DataFormats.Bitmap))
					{
						Image image = (Image)data.GetData(DataFormats.Bitmap, true);
						if (strElName.Substring(strElName.IndexOf(".") + 1).ToLower() == "jpg")
							image.Save(FolderPath + strElName, System.Drawing.Imaging.ImageFormat.Jpeg);
						else if (strElName.Substring(strElName.IndexOf(".") + 1).ToLower() == "gif")
							image.Save(FolderPath + strElName, System.Drawing.Imaging.ImageFormat.Gif);
						else if (strElName.Substring(strElName.IndexOf(".") + 1).ToLower() == "png")
							image.Save(FolderPath + strElName, System.Drawing.Imaging.ImageFormat.Png);
						
					}
					
				}
			}
		}



		/// <summary>
		/// 이미지 이름으로 찾는다.
		/// </summary>
		/// <param name="imgName">이미지파일명이나 herf 경로</param>
		/// <returns></returns>
		public Image Image_Get(string imgName)
		{
			string strElName;
			mshtml.IHTMLElement2 body2 = (mshtml.IHTMLElement2)HTMLDoc.body;
			mshtml.IHTMLControlRange controlRange = (mshtml.IHTMLControlRange)body2.createControlRange();			
			IHTMLElementCollection imgs = HTMLDoc.images;

			Image image = null;
			
			foreach (mshtml.HTMLImg objImg in imgs)
			{
				if (objImg.nameProp != imgName && objImg.href != imgName) continue;


				controlRange.add((mshtml.IHTMLControlElement)objImg);
				controlRange.execCommand("Copy", false, System.Reflection.Missing.Value);
				controlRange.remove(0);
				strElName = objImg.nameProp;

				if (Clipboard.GetDataObject() != null)
				{
					IDataObject data = Clipboard.GetDataObject();

					if (data.GetDataPresent(DataFormats.Bitmap))
					{
						image = (Image)data.GetData(DataFormats.Bitmap, true);						

					}

				}
			}

			return image;
		}



		public void Dispose()
		{
			ie.Quit();
		}


    }
}
