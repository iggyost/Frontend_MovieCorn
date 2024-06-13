using Frontend_MovieCorn.ApplicationData;
using Newtonsoft.Json;

namespace Frontend_MovieCorn.Views.ContentPages;

public partial class PasswordEnterPage : ContentPage
{
    public PasswordEnterPage()
    {
        InitializeComponent();
    }

    private async void passwordEntry_Completed(object sender, EventArgs e)
    {
        if (passwordEntry.Text != null)
        {
            if (passwordEntry.Text.Length > 3)
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync($"{App.conString}user/enter/{App.enteredPhone}/{passwordEntry.Text}");
                if (response.IsSuccessStatusCode)
                {
                    string dataContent = await response.Content.ReadAsStringAsync();
                    var dataUser = JsonConvert.DeserializeObject<User>(dataContent);
                    App.enteredUser = dataUser;
                    App.Current.MainPage = new AppShell();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    validPasswordLabel.Text = "Неверный пароль!";
                }
                else
                {
                    await DisplayAlert("Ошибка!", "Ошибка сервера!", "Закрыть!");
                }
            }
            else
            {
                validPasswordLabel.Text = "Не менее 4 символов!";
            }
        }
        else
        {
            await DisplayAlert("Ошибка!", "Поле для ввода не может быть пустым!", "Закрыть!");
        }
    }

    private void passwordEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        validPasswordLabel.Text = null;
    }
}