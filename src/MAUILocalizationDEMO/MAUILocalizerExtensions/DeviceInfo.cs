using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUILocalizerExtensions
{
	public static partial class DeviceCultureInfo
	{
		public static CultureInfo GetDeviceCultureInfo()
		{
			return DeviceCulture();
		}
		static partial CultureInfo DeviceCulture();
	}
}
