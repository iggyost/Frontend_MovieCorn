namespace Frontend_MovieCorn.Views.ContentPages;

public partial class WelcomePage : ContentPage
{
	public WelcomePage()
	{
		InitializeComponent();
	}

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
		loadingView.IsRunning = true;
		await Task.Delay(3000);
		loadingView.IsRunning = false;
		Application.Current.MainPage = new PhoneEnterPage();
    }
}