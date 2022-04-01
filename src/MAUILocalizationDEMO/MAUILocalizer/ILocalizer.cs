using System.Globalization;
using System.Resources;

namespace MAUILocalizer
{
	public interface ILocalizer
	{
		CultureInfo CurrentAppCulture { get; }
		CultureInfo CurrentDeviceCulture { get; }

		void SetAppCulture(string cultureCode = "en-us");

		string? GetString(string resourcePath);
	}
}
