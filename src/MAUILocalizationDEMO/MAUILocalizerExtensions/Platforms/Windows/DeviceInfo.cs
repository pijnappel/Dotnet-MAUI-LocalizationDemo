using System.Globalization;

namespace MAUILocalizerExtensions
{
	public static partial class DeviceCultureInfo
	{
		static partial CultureInfo DeviceCulture()
		{
			return CultureInfo.InstalledUICulture;
		}
	}
}
