using MAUILocalizerExtensions;

using System.Reflection;

namespace MAUILocalizationDEMO_XAMLFriendly
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureLocalizer(GetLocalizationAssembly(), LocalizationResources.General.ResourceManager)
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				});

			return builder.Build();
		}

		private static Assembly? GetLocalizationAssembly()
		{
			return Assembly.GetAssembly(typeof(LocalizationResources.General));
		}
	}
}