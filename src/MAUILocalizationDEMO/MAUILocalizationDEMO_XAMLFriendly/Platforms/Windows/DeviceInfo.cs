using System.Globalization;

namespace MAUILocalizerExtensions
{
	public static partial class DeviceCultureInfo
	{
		public static partial CultureInfo DeviceCulture()
		{
			return CultureInfo.InstalledUICulture;
		}
	}
}
