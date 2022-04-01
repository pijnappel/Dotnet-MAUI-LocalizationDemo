using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using MAUILocalizerExtensions;

using System.ComponentModel;

namespace MAUILocalizationDEMO_XAMLFriendly
{
	public partial class MainPageViewModel : ObservableObject
	{
		public MainPageViewModel()
		{
			AvailableLanguages = Localizer.Default.GetAvailableCultures();
			SelectedLanguage = Localizer.Default.CurrentAppCulture.Name;

			CounterMessage = String.Format(LocalizationResources.Pages.MainPage_CurrentCount, m_counter);
			Localizer.Default.CurrentAppCultureChanged += _localizationService_CurrentAppCultureChanged;
		}

		private void _localizationService_CurrentAppCultureChanged(object sender, EventArgs e)
		{
			CounterMessage = String.Format(LocalizationResources.Pages.MainPage_CurrentCount, m_counter);
		}

		private int m_counter = 0;

		[ObservableProperty]
		string counterMessage;

		[ObservableProperty]
		List<string> availableLanguages;

		[ObservableProperty]
		string selectedLanguage;

		protected override void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);

			switch(e.PropertyName)
			{
				case nameof(SelectedLanguage):
					if(!string.IsNullOrWhiteSpace(SelectedLanguage))
						Localizer.Default.SetAppCulture(SelectedLanguage);
					break;
			}
		}

		[ICommand]
		private void IncrementCounter()
		{
			CounterMessage = String.Format(LocalizationResources.Pages.MainPage_CurrentCount, ++m_counter);
		}
	}
}
