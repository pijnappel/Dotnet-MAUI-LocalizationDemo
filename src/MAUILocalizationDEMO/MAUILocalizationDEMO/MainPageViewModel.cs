using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using MAUILocalizationDEMO.Services.Localization;

using System.ComponentModel;

namespace MAUILocalizationDEMO
{
	public partial class MainPageViewModel : ObservableObject
	{
		private ILocalizationService _localizationService;
		public MainPageViewModel(ILocalizationService localization)
		{
			_localizationService = localization;

			AvailableLanguages = _localizationService.GetAvailableCultures();
			SelectedLanguage = _localizationService.CurrentAppCulture;

			CounterMessage = String.Format(LocalizationResources.Strings_Pages.MainPage_CurrentCount, m_counter);
			_localizationService.CurrentAppCultureChanged += _localizationService_CurrentAppCultureChanged;
		}

		private void _localizationService_CurrentAppCultureChanged(object sender, EventArgs e)
		{
			CounterMessage = String.Format(LocalizationResources.Strings_Pages.MainPage_CurrentCount, m_counter);
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
						_localizationService.SetAndSaveAppCulture(SelectedLanguage);
					break;
			}
		}

		[ICommand]
		private void IncrementCounter()
		{
			CounterMessage = String.Format(LocalizationResources.Strings_Pages.MainPage_CurrentCount, ++m_counter);
		}
	}
}
