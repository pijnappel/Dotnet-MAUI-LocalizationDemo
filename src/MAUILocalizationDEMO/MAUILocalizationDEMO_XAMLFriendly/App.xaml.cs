namespace MAUILocalizationDEMO_XAMLFriendly
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new MainPage(new MainPageViewModel());
		}
	}
}