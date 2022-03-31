using LocalizationResources;

using System.ComponentModel;
using System.Resources;

namespace MAUILocalizationDEMO.Services.Localization
{
	/// <summary>
	/// Translator, used to support XAML Databinding
	/// in combination with live language switching
	/// 
	/// Wraps the resource manager instance of the corresponding resx file. 
	/// 
	/// When the culture is changed, call InvalidateAll(). This changes all strings in the app (using DataBinding) 
	/// </summary>
	public class Translator : INotifyPropertyChanged
	{
		private ResourceManager m_resourceManager;

		private Translator(ResourceManager resourceManger)
		{
			m_resourceManager = resourceManger;
		}

		public string this[string text]
		{
			get
			{
				return m_resourceManager.GetString(text, Thread.CurrentThread.CurrentUICulture);
			}
		}

		// For easier binding, we define the translators for our resx files here (static) 
		public static Translator General { get; } = new Translator(Strings_General.ResourceManager);
		public static Translator Pages { get; } = new Translator(Strings_Pages.ResourceManager);
		public static Translator SomeOtherPages { get; } = new Translator(Strings_SomeOtherPages.ResourceManager);
	

		public static void InvalidateAll()
		{
			General.Invalidate();
			Pages.Invalidate();
			SomeOtherPages.Invalidate();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void Invalidate()
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
		}
	}
}
