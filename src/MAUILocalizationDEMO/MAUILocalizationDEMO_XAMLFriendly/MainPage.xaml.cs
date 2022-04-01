namespace MAUILocalizationDEMO_XAMLFriendly;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}

