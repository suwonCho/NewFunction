using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Drawing;
using System.Collections.Generic;
using System.IO;

namespace Function.api
{

	//a9054ae7-5898-407e-b9f9-178a587c62ca

	/// <summary>
	/// 파일  ExplorerContextMenu.cs, ShellExtLib 파일을 참조해서 만들것
	/// 빌드를 anycpu로 해야 등록 처리 됨
	/// </summary>
	[ClassInterface(ClassInterfaceType.None)]
	[Guid("a9054ae7-5898-407e-b9f9-178a587c62ca"), ComVisible(true)]
	public class ExplorerContextMenu : IShellExtInit, IContextMenu
	{
		// The name of the selected file.
        private string selectedFile;
		private string[] selectedFiles;
        private string menuText = "Comic Commander v0.1";
        private IntPtr menuBmp = IntPtr.Zero;
        private string verb = "csdisplay";
        private string verbCanonicalName = "CSDisplayFileName";
        private string verbHelpText = "Display File Name (C#)";
        private uint IDM_DISPLAY = 0;

        private uint itemCount = 0;
        private Dictionary<int, string> pathMap = new Dictionary<int, string>();

		public string app;


        public ExplorerContextMenu()
        {
            // Load the bitmap for the menu item.
			//Bitmap bmp = Resources.OK;

			//bmp.MakeTransparent(bmp.GetPixel(0, 0));
			//this.menuBmp = bmp.GetHbitmap(); // TODO change.			

			Register(this.GetType());
        }

		~ExplorerContextMenu()
        {
            if (this.menuBmp != IntPtr.Zero)
            {
                NativeMethods.DeleteObject(this.menuBmp);
                this.menuBmp = IntPtr.Zero;
            }

			Unregister(this.GetType());
        }


        void OnVerbDisplayFileName(IntPtr hWnd)
        {
            // TODO implement a copy here.
            System.Windows.Forms.MessageBox.Show(
                "The selected file is \r\n\r\n" + this.selectedFile,
                "CSShellExtContextMenuHandler");
        }


        #region Shell Extension Registration

		/// <summary>
		/// dll com 등록시 실행되는 메서드
		/// </summary>
		/// <param name="t"></param>
        [ComRegisterFunction()]
        public static void Register(Type t)
        {
            try
            {
                ShellExtReg.RegisterShellExtContextMenuHandler(t.GUID, "*", 
                    "ComicCommander.FileContextMenuExt Class");

				ShellExtReg.RegisterShellExtContextMenuHandler(t.GUID, "Folder",
					"ComicCommander.FileContextMenuExt Class");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Log the error
                throw;  // Re-throw the exception
            }
        }

		/// <summary>
		/// dll com 등록 취소시 실행되는 메서드
		/// </summary>
		/// <param name="t"></param>
        [ComUnregisterFunction()]
        public static void Unregister(Type t)
        {
            try
            {
                ShellExtReg.UnregisterShellExtContextMenuHandler(t.GUID, "*");

				ShellExtReg.UnregisterShellExtContextMenuHandler(t.GUID, "Folder");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Log the error
                throw;  // Re-throw the exception
            }
        }

        #endregion


        #region IShellExtInit Members

        /// <summary>
        /// Initialize the context menu handler. 우클릭 했을때 파일처리
        /// </summary>
        /// <param name="pidlFolder">
        /// A pointer to an ITEMIDLIST structure that uniquely identifies a folder.
        /// </param>
        /// <param name="pDataObj">
        /// A pointer to an IDataObject interface object that can be used to retrieve 
        /// the objects being acted upon.
        /// </param>
        /// <param name="hKeyProgID">
        /// The registry key for the file object or folder type.
        /// </param>
        public void Initialize(IntPtr pidlFolder, IntPtr pDataObj, IntPtr hKeyProgID)
        {
            if (pDataObj == IntPtr.Zero)
            {
                throw new ArgumentException();
            }

            FORMATETC fe = new FORMATETC();
            fe.cfFormat = (short)CLIPFORMAT.CF_HDROP;
            fe.ptd = IntPtr.Zero;
            fe.dwAspect = DVASPECT.DVASPECT_CONTENT;
            fe.lindex = -1;
            fe.tymed = TYMED.TYMED_HGLOBAL;
            STGMEDIUM stm = new STGMEDIUM();

            // The pDataObj pointer contains the objects being acted upon. In this 
            // example, we get an HDROP handle for enumerating the selected files 
            // and folders.
            IDataObject dataObject = (IDataObject)Marshal.GetObjectForIUnknown(pDataObj);
            dataObject.GetData(ref fe, out stm);
			uint rst;

            try
            {
                // Get an HDROP handle.
                IntPtr hDrop = stm.unionmember;
                if (hDrop == IntPtr.Zero)
                {
                    throw new ArgumentException();
                }

                // Determine how many files are involved in this operation.
                uint nFiles = NativeMethods.DragQueryFile(hDrop, UInt32.MaxValue, null, 0);
				selectedFiles = new string[nFiles];
                // This code sample displays the custom context menu item when only 
                // one file is selected. 
                if (nFiles >= 1)
                {
					for (int i = 0; i < nFiles; i++)
					{
						// Get the path of the file.
						StringBuilder fileName = new StringBuilder(260);
						rst = NativeMethods.DragQueryFile(hDrop, (uint)i, fileName,
							fileName.Capacity);
						if(rst == 0)
						{
							selectedFiles[i] = string.Empty;
						}
						
						selectedFiles[i] = string.Format("{0} : {1}", fileName.ToString(), rst);
					}
                }
                else
                {
                    Marshal.ThrowExceptionForHR(WinError.E_FAIL);
                }

                // [-or-]

                // Enumerate the selected files and folders.
                //if (nFiles > 0)
                //{
                //    StringCollection selectedFiles = new StringCollection();
                //    StringBuilder fileName = new StringBuilder(260);
                //    for (uint i = 0; i < nFiles; i++)
                //    {
                //        // Get the next file name.
                //        if (0 != NativeMethods.DragQueryFile(hDrop, i, fileName,
                //            fileName.Capacity))
                //        {
                //            // Add the file name to the list.
                //            selectedFiles.Add(fileName.ToString());
                //        }
                //    }
                //
                //    // If we did not find any files we can work with, throw 
                //    // exception.
                //    if (selectedFiles.Count == 0)
                //    {
                //        Marshal.ThrowExceptionForHR(WinError.E_FAIL);
                //    }
                //}
                //else
                //{
                //    Marshal.ThrowExceptionForHR(WinError.E_FAIL);
                //}
            }
            finally
            {
                NativeMethods.ReleaseStgMedium(ref stm);
            }
        }

        #endregion


        #region IContextMenu Members

        /// <summary>
        /// Add commands to a shortcut menu. 메뉴추가
        /// </summary>
        /// <param name="hMenu">A handle to the shortcut menu.</param>
        /// <param name="iMenu">
        /// The zero-based position at which to insert the first new menu item.
        /// </param>
        /// <param name="idCmdFirst">
        /// The minimum value that the handler can specify for a menu item ID.
        /// </param>
        /// <param name="idCmdLast">
        /// The maximum value that the handler can specify for a menu item ID.
        /// </param>
        /// <param name="uFlags">
        /// Optional flags that specify how the shortcut menu can be changed.
        /// </param>
        /// <returns>
        /// If successful, returns an HRESULT value that has its severity value set 
        /// to SEVERITY_SUCCESS and its code value set to the offset of the largest 
        /// command identifier that was assigned, plus one.
        /// </returns>
        public int QueryContextMenu(
            IntPtr hMenu,
            uint iMenu,
            uint idCmdFirst,
            uint idCmdLast,
            uint uFlags)
        {
            // If uFlags include CMF_DEFAULTONLY then we should not do anything.
            if (((uint)CMF.CMF_DEFAULTONLY & uFlags) != 0)
            {
                return WinError.MAKE_HRESULT(WinError.SEVERITY_SUCCESS, 0, 0);
            }


            // ORIGINAL
           // Use either InsertMenu or InsertMenuItem to add menu items.
            /*
            MENUITEMINFO mii = new MENUITEMINFO();
            mii.cbSize = (uint)Marshal.SizeOf(mii);
            mii.fMask = MIIM.MIIM_BITMAP | MIIM.MIIM_STRING | MIIM.MIIM_FTYPE | MIIM.MIIM_ID | MIIM.MIIM_STATE;
            mii.wID = idCmdFirst + itemCount++;
            mii.fType = MFT.MFT_STRING;
            mii.dwTypeData = this.menuText;
            mii.fState = MFS.MFS_ENABLED;
            mii.hbmpItem = this.menuBmp;
            if (!NativeMethods.InsertMenuItem(hMenu, iMenu, true, ref mii))
            {
                return Marshal.GetHRForLastWin32Error();
            }
            */

            IntPtr hSubMenu = NativeMethods.CreatePopupMenu();

            // 메인 메뉴를 만든다.
            MENUITEMINFO mii2 = new MENUITEMINFO();
            mii2.cbSize = (uint)Marshal.SizeOf(mii2);
            mii2.fMask = MIIM.MIIM_BITMAP | MIIM.MIIM_SUBMENU | MIIM.MIIM_STRING | MIIM.MIIM_FTYPE | MIIM.MIIM_ID | MIIM.MIIM_STATE;
            mii2.hSubMenu = hSubMenu;
            mii2.wID = idCmdFirst + itemCount;
            mii2.fType = MFT.MFT_STRING;
            mii2.dwTypeData = this.menuText;
            mii2.fState = MFS.MFS_ENABLED;
            //mii2.hbmpItem = this.menuBmp;


            uint subItemCount = 0;
            
            //SHDocVw.ShellWindows shellWindows = new SHDocVw.ShellWindows(); 
            //string filename;



			foreach (string file in selectedFiles)
			{

				MENUITEMINFO mii3 = new MENUITEMINFO();
				mii3.cbSize = (uint)Marshal.SizeOf(mii2);
				mii3.fMask = MIIM.MIIM_STRING | MIIM.MIIM_FTYPE | MIIM.MIIM_ID | MIIM.MIIM_STATE;
				mii3.wID = idCmdFirst + itemCount;
				mii3.fType = MFT.MFT_STRING;
				mii3.dwTypeData = file;
				mii3.fState = MFS.MFS_ENABLED;
				//mii3.hbmpItem = this.menuBmp;

				if (!NativeMethods.InsertMenuItem(hSubMenu, subItemCount++, true, ref mii3))
				{
					return Marshal.GetHRForLastWin32Error();
				}

				pathMap.Add((int)itemCount, this.selectedFile);
				itemCount++;
			}

			

			//foreach (SHDocVw.InternetExplorer ie in shellWindows)
			//{
            
			//	/*
			//string[] paths = {"path1", "path2", "path3"};
			//foreach(string s in paths ) {
			//	*/
                
			//	filename = Path.GetFileNameWithoutExtension(ie.FullName).ToLower();


			//	if (filename.Equals("explorer"))
			//	{
			//		string ExplorerWindowPath = ie.LocationURL;
			//		ExplorerWindowPath = ExplorerWindowPath.Replace("file:///", "");
					

			//		// Filters out the current explorer window

			//		// Had a file in c:\ and it matched c:\alertus-dev...
			//		if (/*Path.GetDirectoryName(selectedFile) != Path.GetDirectoryName(ExplorerWindowPath) && */
			//			!String.IsNullOrEmpty(ExplorerWindowPath) )
			//		{
			//			  // Subitem1
			//			MENUITEMINFO mii3 = new MENUITEMINFO();
			//			mii3.cbSize = (uint)Marshal.SizeOf(mii2);
			//			mii3.fMask = MIIM.MIIM_STRING | MIIM.MIIM_FTYPE | MIIM.MIIM_ID | MIIM.MIIM_STATE;
			//			mii3.wID = idCmdFirst + itemCount;
			//			mii3.fType = MFT.MFT_STRING;
			//			mii3.dwTypeData = ExplorerWindowPath;
			//			mii3.fState = MFS.MFS_ENABLED;
			//			//mii3.hbmpItem = this.menuBmp;

			//			if (!NativeMethods.InsertMenuItem(hSubMenu, subItemCount++, true, ref mii3))
			//			{
			//				return Marshal.GetHRForLastWin32Error();
			//			}
						
			//			pathMap.Add((int)itemCount, ExplorerWindowPath);
			//			itemCount++;

			//			mii3.dwTypeData += "22";

			//			if (!NativeMethods.InsertMenuItem(hSubMenu, subItemCount++, true, ref mii3))
			//			{
			//				return Marshal.GetHRForLastWin32Error();
			//			}

			//			ExplorerWindowPath += "2";
			//			pathMap.Add((int)itemCount, ExplorerWindowPath);

			//			itemCount++;
			//		}
			//	}
			//}






            // Adding the POPUP Menu
            if (!NativeMethods.InsertMenuItem(hMenu, iMenu + 0, true, ref mii2))
            {
                return Marshal.GetHRForLastWin32Error();
            }

            






            // Return an HRESULT value with the severity set to SEVERITY_SUCCESS. 
            // Set the code value to the offset of the largest command identifier 
            // that was assigned, plus one (1).
            return WinError.MAKE_HRESULT(WinError.SEVERITY_SUCCESS, 0,
                IDM_DISPLAY + itemCount);
        }

        /// <summary>
        /// Carry out the command associated with a shortcut menu item.
        /// </summary>
        /// <param name="pici">
        /// A pointer to a CMINVOKECOMMANDINFO or CMINVOKECOMMANDINFOEX structure 
        /// containing information about the command. 
        /// </param>
        public void InvokeCommand(IntPtr pici)
        {
            bool isUnicode = false;

            // Determine which structure is being passed in, CMINVOKECOMMANDINFO or 
            // CMINVOKECOMMANDINFOEX based on the cbSize member of lpcmi. Although 
            // the lpcmi parameter is declared in Shlobj.h as a CMINVOKECOMMANDINFO 
            // structure, in practice it often points to a CMINVOKECOMMANDINFOEX 
            // structure. This struct is an extended version of CMINVOKECOMMANDINFO 
            // and has additional members that allow Unicode strings to be passed.
            CMINVOKECOMMANDINFO ici = (CMINVOKECOMMANDINFO)Marshal.PtrToStructure(
                pici, typeof(CMINVOKECOMMANDINFO));
            CMINVOKECOMMANDINFOEX iciex = new CMINVOKECOMMANDINFOEX();
            if (ici.cbSize == Marshal.SizeOf(typeof(CMINVOKECOMMANDINFOEX)))
            {
                if ((ici.fMask & CMIC.CMIC_MASK_UNICODE) != 0)
                {
                    isUnicode = true;
                    iciex = (CMINVOKECOMMANDINFOEX)Marshal.PtrToStructure(pici,
                        typeof(CMINVOKECOMMANDINFOEX));
                }
            }

            // Determines whether the command is identified by its offset or verb.
            // There are two ways to identify commands:
            // 
            //   1) The command's verb string 
            //   2) The command's identifier offset
            // 
            // If the high-order word of lpcmi->lpVerb (for the ANSI case) or 
            // lpcmi->lpVerbW (for the Unicode case) is nonzero, lpVerb or lpVerbW 
            // holds a verb string. If the high-order word is zero, the command 
            // offset is in the low-order word of lpcmi->lpVerb.

            // For the ANSI case, if the high-order word is not zero, the command's 
            // verb string is in lpcmi->lpVerb. 
            if (!isUnicode && NativeMethods.HighWord(ici.verb.ToInt32()) != 0)
            {
                // Is the verb supported by this context menu extension?
                if (Marshal.PtrToStringAnsi(ici.verb) == this.verb)
                {
                    OnVerbDisplayFileName(ici.hwnd);
                }
                else
                {
                    string verb = Marshal.PtrToStringAnsi(ici.verb);
                    System.Windows.Forms.MessageBox.Show("Ansi - " + verb);

                    // If the verb is not recognized by the context menu handler, it 
                    // must return E_FAIL to allow it to be passed on to the other 
                    // context menu handlers that might implement that verb.
                    Marshal.ThrowExceptionForHR(WinError.E_FAIL);
                }
            }

            // For the Unicode case, if the high-order word is not zero, the 
            // command's verb string is in lpcmi->lpVerbW. 
            else if (isUnicode && NativeMethods.HighWord(iciex.verbW.ToInt32()) != 0)
            {
                // Is the verb supported by this context menu extension?
                if (Marshal.PtrToStringUni(iciex.verbW) == this.verb)
                {
                    OnVerbDisplayFileName(ici.hwnd);
                }
                else
                {
                    string verb = Marshal.PtrToStringAnsi(iciex.verbW);
                    System.Windows.Forms.MessageBox.Show("Unicode - " + verb);

                    // If the verb is not recognized by the context menu handler, it 
                    // must return E_FAIL to allow it to be passed on to the other 
                    // context menu handlers that might implement that verb.
                    Marshal.ThrowExceptionForHR(WinError.E_FAIL);
                }
            }

            // If the command cannot be identified through the verb string, then 
            // check the identifier offset.
            else
            {
                int verb = NativeMethods.LowWord(ici.verb.ToInt32());

                string temp = "start "; 
                    
                // Loop over pairs with foreach
                foreach (KeyValuePair<int, string> pair in pathMap)
                {
                    temp += pair.Key + " " + pair.Value + "     ";
                }



                //System.Windows.Forms.MessageBox.Show(verb.ToString() + temp);
                if (pathMap.ContainsKey(verb))
                {
                    //System.Windows.Forms.MessageBox.Show(pathMap[verb]);

                    if (System.Windows.Forms.DialogResult.OK ==
                        System.Windows.Forms.MessageBox.Show("Copy " + selectedFile + " to " + pathMap[verb] + "?", "Confirm Copy", System.Windows.Forms.MessageBoxButtons.OKCancel))
                    {
                        //System.Windows.Forms.MessageBox.Show("copying...");
                        try
                        {
                            File.Copy(this.selectedFile, Path.Combine(pathMap[verb], Path.GetFileName(this.selectedFile)) );
                        }
                        catch (Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show("Unable to Copy: " + ex.Message);
                        }
                    }

                    //System.Windows.Forms.MessageBox.Show("done...");
                }
                else
                {
                    // If the verb is not recognized by the context menu handler, it 
                    // must return E_FAIL to allow it to be passed on to the other 
                    // context menu handlers that might implement that verb.
                    Marshal.ThrowExceptionForHR(WinError.E_FAIL);
                }
            }
        }

        /// <summary>
        /// Get information about a shortcut menu command, including the help string 
        /// and the language-independent, or canonical, name for the command.
        /// </summary>
        /// <param name="idCmd">Menu command identifier offset.</param>
        /// <param name="uFlags">
        /// Flags specifying the information to return. This parameter can have one 
        /// of the following values: GCS_HELPTEXTA, GCS_HELPTEXTW, GCS_VALIDATEA, 
        /// GCS_VALIDATEW, GCS_VERBA, GCS_VERBW.
        /// </param>
        /// <param name="pReserved">Reserved. Must be IntPtr.Zero</param>
        /// <param name="pszName">
        /// The address of the buffer to receive the null-terminated string being 
        /// retrieved.
        /// </param>
        /// <param name="cchMax">
        /// Size of the buffer, in characters, to receive the null-terminated string.
        /// </param>
        public void GetCommandString(
            UIntPtr idCmd,
            uint uFlags,
            IntPtr pReserved,
            StringBuilder pszName,
            uint cchMax)
        {
            if (idCmd.ToUInt32() == IDM_DISPLAY)
            {
                switch ((GCS)uFlags)
                {
                    case GCS.GCS_VERBW:
                        if (this.verbCanonicalName.Length > cchMax - 1)
                        {
                            Marshal.ThrowExceptionForHR(WinError.STRSAFE_E_INSUFFICIENT_BUFFER);
                        }
                        else
                        {
                            pszName.Clear();
                            pszName.Append(this.verbCanonicalName);
                        }
                        break;

                    case GCS.GCS_HELPTEXTW:
                        if (this.verbHelpText.Length > cchMax - 1)
                        {
                            Marshal.ThrowExceptionForHR(WinError.STRSAFE_E_INSUFFICIENT_BUFFER);
                        }
                        else
                        {
                            pszName.Clear();
                            pszName.Append(this.verbHelpText);
                        }
                        break;
                }
            }
        }

        #endregion
	}
}
