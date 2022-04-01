using System.Globalization;

namespace MAUILocalizer
{
	public static partial class DeviceInfo
	{
		public static partial CultureInfo DeviceCulture()
		{
			return CultureInfo.InstalledUICulture;
		}
	}
}
