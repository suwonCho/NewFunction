using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Function.form
{
	/// <summary>
	/// 프로퍼티의 Arry처리된 항목을 디자이너 모드에서 볼수 있게 하는 컨버터 클래스
	/// </summary>
	class TypeConverter_Properties : TypeConverter
	{

		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			return TypeDescriptor.GetProperties(value, attributes);
		}

		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;

			//return base.GetPropertiesSupported(context);
		}
	}
}
