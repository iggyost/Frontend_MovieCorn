using Frontend_MovieCorn.ApplicationData;
using Newtonsoft.Json;

namespace Frontend_MovieCorn.Views.ContentPages;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();

    }

    private void exitBtn_Clicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new WelcomePage();
        App.enteredUser = null;
        App.enteredPhone = null;
    }
    public async Task UpdateFavorites()
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"{App.conString}favoritesview/get/{App.enteredUser.UserId}");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<FavoritesView[]>(content).ToList();
            favoriteCollectionView.ItemsSource = data;
            App.favoritesUser = data;
            if (data.Count == 0)
            {
                noMoviesLabel.IsVisible = true;
            }
            else
            {
                noMoviesLabel.IsVisible = false;
            }
        }
    }
    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        await UpdateFavorites();
        userNameLabel.Text = App.enteredUser.Name.ToString();
        userPhoneLabel.Text = App.enteredUser.Phone.ToString();
        while (true)
        {
            if (App.isFavoriteUpdated == true)
            {
                await UpdateFavorites();
                App.isFavoriteUpdated = false;
            }
            await Task.Delay(1500);
        }
    }

    private async void borderMovieTap_Tapped(object sender, TappedEventArgs e)
    {
        Border border = sender as Border;
        var movieBorderId = int.Parse(border.AutomationId.ToString());
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"{App.conString}moviesview/select/{movieBorderId}");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            var selectedMovie = JsonConvert.DeserializeObject<MoviesView>(content);
            await Navigation.PushModalAsync(new MoviePage(selectedMovie));
        }
    }
}