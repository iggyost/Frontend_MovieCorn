using Frontend_MovieCorn.ApplicationData;

namespace Frontend_MovieCorn.Views.ContentPages;

public partial class MoviePage : ContentPage
{
    public static MoviesView currentMovie = new MoviesView();
    public MoviePage()
    {
        InitializeComponent();
    }
    public static bool isCurrentFavorite;
    public MoviePage(MoviesView movie)
    {
        InitializeComponent();

        currentMovie = movie;
        nameMovieLabel.Text = currentMovie.Name;
        lengthLabel.Text = currentMovie.LengthMin.ToString() + " мин.";
        countryLabel.Text = currentMovie.Country;
        ageLabel.Text = currentMovie.Age.ToString() + "+";
        movieCoverImage.Source = currentMovie.CoverPath;
        yearLabel.Text = currentMovie.ProduceYear.ToString();
        directorLabel.Text = currentMovie.Director;
        producerLabel.Text = currentMovie.Producer;
        if (App.favoritesUser.Where(x => x.MovieId == currentMovie.MovieId).FirstOrDefault() != null)
        {
            favoriteButton.Source = "favorite_icon_filled.png";
            isCurrentFavorite = true;
        }
        else
        {
            favoriteButton.Source = "favorite_icon.png";
            isCurrentFavorite = false;
        }
    }

    public async Task SetFavorite()
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"{App.conString}favoritesview/set/{App.enteredUser.UserId}/{currentMovie.MovieId}");
        if (response.IsSuccessStatusCode)
        {
            favoriteButton.Source = "favorite_icon_filled.png";
            isCurrentFavorite = true;
            App.isFavoriteUpdated = true;
        }
    }
    public async Task RemoveFavorite()
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"{App.conString}favoritesview/remove/{App.enteredUser.UserId}/{currentMovie.MovieId}");
        if (response.IsSuccessStatusCode)
        {
            favoriteButton.Source = "favorite_icon.png";
            isCurrentFavorite = false;
            App.isFavoriteUpdated = true;
        }
    }

    private async void favoriteButton_Clicked(object sender, EventArgs e)
    {
        loadingIndicator.IsRunning = true;
        favoriteButton.IsEnabled = false;

        if (isCurrentFavorite == true)
        {
            await RemoveFavorite();
        }
        else
        {
            await SetFavorite();
        }

        await Task.Delay(1000);
        loadingIndicator.IsRunning = false;
        favoriteButton.IsEnabled = true;
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {

    }
}