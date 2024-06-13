using Frontend_MovieCorn.ApplicationData;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace Frontend_MovieCorn.Views.ContentPages;

public partial class MainPage : ContentPage
{
    public static List<MoviesView> moviesView = new List<MoviesView>();
    public static List<MoviesView> popularMoviesView = new List<MoviesView>();
    public static List<MoviesView> recMoviesView = new List<MoviesView>();

    public ObservableCollection<MoviesView> MovieList { get; set; } = new ObservableCollection<MoviesView>();

    public static List<Tag> tagView = new List<Tag>();
    public ObservableCollection<Tag> TagList { get; set; } = new ObservableCollection<Tag>();

    public MainPage()
    {
        InitializeComponent();
        LoadPopularMovies();
    }
    private async void LoadTag(string searchText)
    {
        if (searchText != null)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"{App.conString}tag/get/{searchText}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var tag = JsonConvert.DeserializeObject<Tag>(content);
                if (TagList.Contains(tag))
                {
                    await DisplayAlert("Ошибка!", "Такой тег уже есть в фильтре!", "Закрыть");
                }
                else
                {
                    TagList.Add(tag);
                    tagsCv.ItemsSource = TagList;
                    LoadMovies();
                }
            }
            else
            {
                await DisplayAlert("Ошибка!", "Подходящий тег не найден!", "Закрыть");
            }
        }
        else
        {
            await DisplayAlert("Ошибка!", "Введите тег для поиска!", "Закрыть");
        }
    }
    private async void LoadPopularMovies()
    {
        loadingPopular.IsRunning = true;
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"{App.conString}moviesview/get/popular");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            var moviesList = JsonConvert.DeserializeObject<MoviesView[]>(content);
            popularMoviesView = moviesList.ToList();
            popularMoviesCv.ItemsSource = moviesList.ToList();
        }
        loadingPopular.IsRunning = false;
    }
    private async Task LoadRecMovies()
    {
        loadingPopular.IsRunning = true;
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"{App.conString}moviesview/get/recomend");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            var moviesList = JsonConvert.DeserializeObject<MoviesView[]>(content);
            recMoviesView = moviesList.ToList();
            recMoviesCv.ItemsSource = moviesList.ToList();
        }
        loadingPopular.IsRunning = false;
    }
    private async void LoadMovies()
    {
        loadingSearch.IsRunning = true;
        if (TagList.Count == 0)
        {
            await DisplayAlert("Ошибка!", "Выберите 1 или несколько тегов для поиска!", "Закрыть");
            moviesCv.ItemsSource = null;
            headerSearchFilms.IsVisible = false;
        }
        else
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsJsonAsync($"{App.conString}moviesview/get", TagList.ToList());
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var moviesList = JsonConvert.DeserializeObject<MoviesView[]>(content);
                moviesCv.ItemsSource = moviesList.ToList();
                moviesView = moviesList.ToList();
                headerSearchFilms.IsVisible = true;
            }
        }
        loadingSearch.IsRunning = false;
    }
    public async Task UpdateFavorites()
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"{App.conString}favoritesview/get/{App.enteredUser.UserId}");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            App.favoritesUser = JsonConvert.DeserializeObject<FavoritesView[]>(content).ToList();
        }
    }
    private void searchTags_SearchButtonPressed(object sender, EventArgs e)
    {
        LoadTag(searchTags.Text);
    }

    private void searchTags_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void clearTags_Clicked(object sender, EventArgs e)
    {
        TagList.Clear();
        tagsCv.ItemsSource = null;
        moviesCv.ItemsSource = null;
        headerSearchFilms.IsVisible = false;
    }

    private void removeTag_Clicked(object sender, EventArgs e)
    {
        ImageButton imageButton = (ImageButton)sender;
        int tagId = int.Parse(imageButton.AutomationId.ToString());
        Tag selectedTag = new Tag();
        selectedTag = TagList.Where(x => x.TagId == tagId).FirstOrDefault();
        TagList.Remove(selectedTag);
        LoadMovies();
    }

    private void tagsCv_Loaded(object sender, EventArgs e)
    {

    }

    private void refreshPopular_Refreshing(object sender, EventArgs e)
    {
        LoadPopularMovies();
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        await UpdateFavorites();
        await LoadRecMovies();
    }

    private async void borderPopularMovieTap_Tapped(object sender, TappedEventArgs e)
    {
        Border border = sender as Border;
        var movieBorderId = int.Parse(border.AutomationId.ToString());
        var selectedMovie = popularMoviesView.Where(x => x.MovieId == movieBorderId).FirstOrDefault();
        await Navigation.PushModalAsync(new MoviePage(selectedMovie));
    }

    private async void borderMovieTap_Tapped(object sender, TappedEventArgs e)
    {
        Border border = sender as Border;
        var movieBorderId = int.Parse(border.AutomationId.ToString());
        var selectedMovie = moviesView.Where(x => x.MovieId == movieBorderId).FirstOrDefault();
        await Navigation.PushModalAsync(new MoviePage(selectedMovie));
    }

    private async void borderRecMovieTap_Tapped(object sender, TappedEventArgs e)
    {
        Border border = sender as Border;
        var movieBorderId = int.Parse(border.AutomationId.ToString());
        var selectedMovie = recMoviesView.Where(x => x.MovieId == movieBorderId).FirstOrDefault();
        await Navigation.PushModalAsync(new MoviePage(selectedMovie));
    }
}