using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUILocalizationDEMO.Services.Localization
{
	internal class LocalizationService : ILocalizationService
	{
		private const string m_defaultAppLocale = "en";

		public event EventHandler CurrentAppCultureChanged;

		public List<string> GetAvailableCultures()
		{
			return new List<string>()
			{
			"en-US",
			"de-DE"
			};
		}

		public string GetSystemCulture()
		{
			return DeviceInfoService.DeviceCulture().Name;

			//if (PlatformCulture == null)
			//	return Thread.CurrentThread.CurrentUICulture.Name;
			//return PlatformCulture.GetCurrentCulture().Name;
		}

		public void SetAndSaveAppCulture(string culture)
		{
			if (string.IsNullOrWhiteSpace(culture))
				throw new ArgumentException(nameof(culture));


			Preferences.Set("AppLocale", culture);
			ApplyAppCulture(culture);
		}

		private void ApplyAppCulture(string culture)
		{
			var desiredCulture = new CultureInfo(culture);
			Thread.CurrentThread.CurrentCulture = desiredCulture;
			Thread.CurrentThread.CurrentUICulture = desiredCulture;
			CultureInfo.DefaultThreadCurrentCulture = desiredCulture;
			CultureInfo.DefaultThreadCurrentUICulture = desiredCulture;

			// Sends a notification to ALL texts from resources
			Translator.InvalidateAll();

			CurrentAppCultureChanged?.Invoke(this, new EventArgs());
		}

		public string CurrentAppCulture
		{
			get
			{
				string appLocale = Preferences.Get("AppLocale", string.Empty);

				if (!string.IsNullOrWhiteSpace(appLocale))
					return appLocale;

				// Try to use the app culture of the device
				var deviceLocale = DeviceInfoService.DeviceCulture();
				if (GetAvailableCultures().Any(x => x == deviceLocale.Name))
					return deviceLocale.Name;

				return m_defaultAppLocale;
			}
		}

		public CultureInfo GetCurrentAppCultureInfo()
		{
			try
			{
				return CultureInfo.CreateSpecificCulture(CurrentAppCulture);
			}
			catch (Exception)
			{
				return CultureInfo.InvariantCulture;
			}
		}
	}
}

