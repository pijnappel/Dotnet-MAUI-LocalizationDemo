using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUILocalizationDEMO
{
	public static partial class DeviceInfoService
	{
		public static partial CultureInfo DeviceCulture()
		{
			return CultureInfo.InstalledUICulture;
		}
	}
}
