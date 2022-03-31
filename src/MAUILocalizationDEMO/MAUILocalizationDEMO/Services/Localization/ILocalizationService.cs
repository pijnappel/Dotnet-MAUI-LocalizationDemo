using System.Globalization;

namespace MAUILocalizationDEMO.Services.Localization
{
	public interface ILocalizationService
	{
		string CurrentAppCulture { get; }

		List<string> GetAvailableCultures();
		void SetAndSaveAppCulture(string culture);

		CultureInfo GetCurrentAppCultureInfo();

		event EventHandler CurrentAppCultureChanged;
	}
}
