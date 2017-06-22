using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Function.form
{
	public enum enUnit
	{
		Pixel = 0,
		Percent
		
	}
	
	/// <summary>
	/// 컨피그 파일 타입
	/// </summary>
	public enum enConfigFileType
	{
		/// <summary>
		/// 프레임 워크에서 제공하는 설정 파일 사용
		/// </summary>
		DefaultConfig,
		/// <summary>
		/// DataSet xml 설정 파일 사용
		/// </summary>
		ConfigXml
	}

	public enum enMsgType
	{
		/// <summary>
		/// 타입 없음, 하얀 동그라미
		/// </summary>
		None,
		/// <summary>
		/// 타입 정상, 녹색 동그라미
		/// </summary>
		OK,
		/// <summary>
		/// 타입 에러, 빨강 동그라미
		/// </summary>
		Error
	}


	
	[TypeConverter(typeof(TypeConverter_Properties))]
	public class ControlViewInfo
	{

		event EventHandler _onPropertiesChannged;

		[Description("프로퍼티 값이 변경 되었을 경우 발새하는 이벤트")]
		public event EventHandler OnPropertiesChannged
		{
			add { _onPropertiesChannged += value; }
			remove { _onPropertiesChannged -= value; }
		}

		private void properties_changed()
		{
			if (_onPropertiesChannged == null) return;

			_onPropertiesChannged(this, null);
		}



		Color _backColor = System.Drawing.SystemColors.Control;

		[MergableProperty(true)]
		[RefreshProperties(System.ComponentModel.RefreshProperties.All)]
		public Color BackColor
		{
			get { return _backColor; }
			set
			{
				_backColor = value;

				properties_changed();
			}
		}

		Color _foreColor = Color.Black;

		[MergableProperty(true)]
		[RefreshProperties(System.ComponentModel.RefreshProperties.All)]
		public Color ForeColor
		{
			get { return _foreColor; }
			set
			{
				_foreColor = value;

				properties_changed();
			}
		}


		Font _font = new Font("굴림", 9);

		[RefreshProperties(System.ComponentModel.RefreshProperties.All)]
		public Font Font
		{
			get { return _font; }
			set
			{
				_font = value;

				properties_changed();
			}
		}


	}


}
