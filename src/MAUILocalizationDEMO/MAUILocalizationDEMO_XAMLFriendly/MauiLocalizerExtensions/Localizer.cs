using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MAUILocalizerExtensions
{
	internal class Localizer : ILocalizer, INotifyPropertyChanged
	{
		public Localizer()
		{
			m_ressourceAssembly = Assembly.GetExecutingAssembly();

			m_CurrentAppCulture = GetInitialCulture();

			if (m_CurrentAppCulture.Name != CurrentDeviceCulture.Name)
				SetAppCulture_internal(m_CurrentAppCulture.Name);
		}

		private const string m_defaultAppLocale = "en";

		private CultureInfo GetInitialCulture()
		{
			string? appLocale = Preferences.Get("AppLocale", string.Empty);

			if (!string.IsNullOrWhiteSpace(appLocale))
				return new CultureInfo(appLocale);

			// Try to use the app culture of the device
			var deviceLocale = DeviceCultureInfo.GetDeviceCultureInfo();
			if (GetAvailableCultures().Any(x => x == deviceLocale.Name))
				return deviceLocale;

			return new CultureInfo(m_defaultAppLocale);
		}

		public event EventHandler? CurrentAppCultureChanged;

		public List<string> GetAvailableCultures()
		{
			return new List<string>()
			{
			"en-US",
			"de-DE"
			};
		}


		public static Localizer Default { get; } = new Localizer();


		private CultureInfo m_CurrentAppCulture;
		public CultureInfo CurrentAppCulture
		{
			get
			{
				return m_CurrentAppCulture;
			}
			private set
			{
				if (m_CurrentAppCulture == value) return;
				m_CurrentAppCulture = value;
				SetAppCulture(value.Name);
				RaisePropertyChanged(nameof(CurrentAppCulture));
			}
		}

		public CultureInfo CurrentDeviceCulture => DeviceCultureInfo.GetDeviceCultureInfo();

		public string? GetString(string resourcePath)
		{
			var resourceInfo = FindResourceManager(resourcePath);

			if (resourceInfo.resourceManager is null)
				throw new LocalizationException($"No ResourceManager for {resourcePath}");

			return resourceInfo.resourceManager.GetString(resourceInfo.localizationKey);
		}

		[IndexerName("Item")]
		public string? this[string resourcePath] => GetString(resourcePath);

		private Dictionary<string, ResourceManager> m_resourceManagerBuffer = new Dictionary<string, ResourceManager>();

		private (ResourceManager? resourceManager, string localizationKey) FindResourceManager(string resourcePath)
		{
			if (string.IsNullOrWhiteSpace(resourcePath))
				throw new ArgumentNullException(nameof(resourcePath));


			// try extracting the resourceManagerName (the string before the first '.')
			var pathNodes = resourcePath.Split(':');

			if (pathNodes.Length == 1)
			{
				// no '.' in the path, just a single string?
				// Fallback to a default ressourcemanager
				return (m_defaultResourceManager, resourcePath);
			}
			var resourceManagerName = pathNodes[0];
			var resourceManagerFullName = $"{m_ressourceAssembly.GetName().Name}.{resourceManagerName}";
			var key = resourcePath.Substring(resourceManagerName.Length + 1);

			ResourceManager? resourceManager = null;
			if (m_resourceManagerBuffer.TryGetValue(resourceManagerFullName, out resourceManager) && resourceManager is not null)
				return (resourceManager, key);

			resourceManager = new ResourceManager(resourceManagerFullName, m_ressourceAssembly); 		
			m_resourceManagerBuffer.Add(resourceManagerFullName, resourceManager);
			return (resourceManager, key); // remove name and '.' in front of the localization key
		}

		public void SetAppCulture(string cultureCode = "en-us")
		{
			if (string.IsNullOrWhiteSpace(cultureCode))
				throw new ArgumentNullException(nameof(cultureCode));

			if (cultureCode == m_CurrentAppCulture.Name)
				return;

			SetAppCulture_internal(cultureCode);
		}

		private void SetAppCulture_internal(string cultureCode)
		{
			m_resourceManagerBuffer.Clear();

			var desiredCulture = new CultureInfo(cultureCode);
			m_CurrentAppCulture = desiredCulture;
			Thread.CurrentThread.CurrentCulture = desiredCulture;
			Thread.CurrentThread.CurrentUICulture = desiredCulture;
			CultureInfo.DefaultThreadCurrentCulture = desiredCulture;
			CultureInfo.DefaultThreadCurrentUICulture = desiredCulture;

			RaisePropertyChanged("Item");
			Preferences.Set("AppLocale", cultureCode);
			CurrentAppCultureChanged?.Invoke(this, new EventArgs());
		}

		Assembly m_ressourceAssembly;
		ResourceManager? m_defaultResourceManager;
		internal void Initialize(Assembly? ressourceAssembly = null, ResourceManager? defaultResourceManager = null)
		{
			m_defaultResourceManager = defaultResourceManager;
			
			if (ressourceAssembly is null)
			{
				m_ressourceAssembly = Assembly.GetExecutingAssembly();
				return;
			}
			else
				m_ressourceAssembly = ressourceAssembly;
		}

		public event PropertyChangedEventHandler? PropertyChanged;
		private void RaisePropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public static class LocalizerInitializationExtensions
	{
		public static MauiAppBuilder ConfigureLocalizer(this MauiAppBuilder builder, Assembly? ressourceAssembly = null, ResourceManager? resourceManager = null)
		{
			Localizer.Default.Initialize(ressourceAssembly, resourceManager);
			return builder;
		}
	}

	public class LocalizationException : Exception
	{
		public LocalizationException() { }
		public LocalizationException(string message) : base(message) { }
		public LocalizationException(string message, Exception innerException) : base(message, innerException) { }
	}

}
