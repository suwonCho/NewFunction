using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Function.api
{
	/// <summary>
	/// 파일의 등록된 아이콘 정보를 관리 하는 class
	/// ImageList를 제공하고 해당 정보를 관리 하는 클래스
	/// </summary>
	public static class GetFileSystemImage
	{

		private static System.Windows.Forms.ImageList _imgList = null;

		/// <summary>
		/// File Image List를 가져 옵니다.
		/// </summary>
		public static System.Windows.Forms.ImageList ImgList
		{
			get {
				init();
				return _imgList; 
			}
			set
			{
				ImgList = value;
			}
		}


		private static Dictionary<Bitmap, int> _imageStore = null;

		/// <summary>
		/// 파일 아이콘 정보를 반환 합니다.
		/// </summary>
		/// <param name="path"></param>
		/// <param name="img_idx">imageList의 index</param>
		/// <returns></returns>
		public static Bitmap GetFileImage(string path, out int img_idx)
		{
			init();

			try
			{
				Bitmap img = (Bitmap)Function.api.API_Windows.GetFileSystemImage(path);

				//Console.WriteLine("이미지 셑:" + path + " : " + e.NodeImageIndex);



				if (!_imageStore.ContainsKey(img))
				{
					_imgList.Images.Add(img);
					_imageStore.Add(img, _imgList.Images.Count - 1);
					img_idx = _imgList.Images.Count - 1;
				}
				else
					img_idx = _imageStore[img];

				return img;
			}
			catch
			{
				img_idx = -1;
				return null;
			}
		}

		/// <summary>
		/// 파일 아이콘 정보를 반환 합니다.
		/// </summary>
		/// <param name="path"></param>		
		/// <returns></returns>
		public static Bitmap GetFileImage(string path)
		{
			int idx = 0;

			return GetFileImage(path, out idx);
		}




		private static void init()
		{
			if (_imgList == null) _imgList = new System.Windows.Forms.ImageList();
			if (_imageStore == null) _imageStore  = new Dictionary<Bitmap, int>(new Function.api.ImageComparer());
		}



	}


	[StructLayout(LayoutKind.Sequential)]
	internal struct SHFILEINFO
	{
		public IntPtr hIcon;
		public IntPtr iIcon;
		public uint dwAttributes;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string szDisplayName;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
		public string szTypeName;
	};

	internal class Win32
	{
		public const uint SHGFI_ICON = 0x100;
		public const uint SHGFI_LARGEICON = 0x0;
		public const uint SHGFI_SMALLICON = 0x1;

		[DllImport("shell32.dll")]
		public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
	}

	public class ImageComparer : IEqualityComparer<Bitmap>
	{
		public ImageComparer()
		{

		}

		public bool Equals(Bitmap x, Bitmap y)
		{
			if (x.Size.Height == y.Size.Height
				&& x.Size.Width == y.Size.Width)
				return GetHashCode(x) == GetHashCode(y);
			else
				return false;
		}

		public int GetHashCode(Bitmap obj)
		{
			int hash = 0;
			int x;
			int y;
			int pixelCount = obj.Width * obj.Height;

			for (int i = 0; i < pixelCount; i++)
			{
				x = i % obj.Width;
				y = i / obj.Width;
				hash ^= obj.GetPixel(x, y).ToArgb();
			}

			return hash;
		}
	}


}
