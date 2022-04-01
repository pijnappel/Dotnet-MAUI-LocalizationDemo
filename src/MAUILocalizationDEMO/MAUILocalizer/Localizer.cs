using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace MAUILocalizer
{
	internal class Localizer : ILocalizer, INotifyPropertyChanged
	{
		public Localizer()
		{
			m_ressourceAssembly = Assembly.GetExecutingAssembly();
			m_CurrentAppCulture = CultureInfo.CurrentUICulture;
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

		public CultureInfo CurrentDeviceCulture => DeviceInfo.DeviceCulture();

		public string? GetString(string resourcePath)
		{
			var resourceInfo = FindResourceManager(resourcePath);

			if (resourceInfo.resourceManager is null)
				throw new LocalizationException($"No ResourceManager for {resourcePath}");

			return resourceInfo.resourceManager.GetString(resourceInfo.localizationKey);
		}

		public string? this[string resourcePath] => GetString(resourcePath);

		private (ResourceManager? resourceManager, string localizationKey)  FindResourceManager(string resourcePath)
		{
			if(string.IsNullOrWhiteSpace(resourcePath))
				throw new ArgumentNullException(nameof(resourcePath));

			// try extracting the resourceManagerName (the string before the first '.')
			var pathNodes = resourcePath.Split('.');
			
			if(pathNodes.Length == 1)
			{
				// no '.' in the path, just a single string?
				// Fallback to a default ressourcemanager
				return (m_defaultResourceManager, resourcePath);
			}

			var resourceManagerName = pathNodes[0];

			ResourceManager resourceManager = new ResourceManager(resourceManagerName, m_ressourceAssembly);
			return (resourceManager, resourcePath.Substring(resourceManagerName.Length + 1)); // remove name and '.' in front of the localization key
		}

		public void SetAppCulture(string cultureCode = "en-us")
		{
			var desiredCulture = new CultureInfo(cultureCode);
			Thread.CurrentThread.CurrentCulture = desiredCulture;
			Thread.CurrentThread.CurrentUICulture = desiredCulture;
			CultureInfo.DefaultThreadCurrentCulture = desiredCulture;
			CultureInfo.DefaultThreadCurrentUICulture = desiredCulture;

			RaisePropertyChanged(nameof(cultureCode));
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
		}

		public event PropertyChangedEventHandler? PropertyChanged;
		private void RaisePropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public static class LocalizerInitializationExtensions
	{
		public static MauiAppBuilder ConfigureLocalizer(this MauiAppBuilder builder, Assembly? ressourceAssembly = null)
		{
			Localizer.Default.Initialize(ressourceAssembly);
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
