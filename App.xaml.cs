using Frontend_MovieCorn.ApplicationData;
using Frontend_MovieCorn.Views.ContentPages;

namespace Frontend_MovieCorn;

public partial class App : Application
{
    public static string conString = "http://192.168.0.10:45455/api/";
    public static User enteredUser;
    public static string enteredPhone;
    public static bool isFavoriteUpdated = false;
    public static List<FavoritesView> favoritesUser = new List<FavoritesView>();
    public App()
	{
		InitializeComponent();

		MainPage = new WelcomePage();
	}
}
