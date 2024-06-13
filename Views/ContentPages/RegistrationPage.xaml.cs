using Frontend_MovieCorn.ApplicationData;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Frontend_MovieCorn.Views.ContentPages;

public partial class RegistrationPage : ContentPage
{
    Regex phoneRegex = new Regex("^((\\+7|7|8)+([0-9]){10})$");
    public RegistrationPage()
    {
        InitializeComponent();
        phoneEntry.Text = App.enteredPhone;
    }

    private void passwordEntry_Completed(object sender, EventArgs e)
    {

    }

    private void phoneEntry_Completed(object sender, EventArgs e)
    {

    }

    private void nameEntry_Completed(object sender, EventArgs e)
    {

    }

    private async void regBtn_Clicked(object sender, EventArgs e)
    {
        string userName;
        if (phoneEntry.Text != null)
        {
            if (passwordEntry.Text != null)
            {
                if (passwordEntry.Text.Length > 3)
                {
                    if (phoneRegex.IsMatch(phoneEntry.Text))
                    {
                        if (nameEntry.Text == null)
                        {
                            userName = "Пользователь";
                        }
                        else
                        {
                            userName = nameEntry.Text;
                        }
                        HttpClient client = new HttpClient();
                        var response = await client.GetAsync($"{App.conString}user/reg/{userName}/{phoneEntry.Text}/{passwordEntry.Text}");
                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            App.enteredUser = JsonConvert.DeserializeObject<User>(content);
                            Application.Current.MainPage = new AppShell();
                        }
                        else
                        {
                            await DisplayAlert("Ошибка!", "Неправильные данные пользователя!", "Закрыть");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Ошибка!", "Номер телефона не соответствует формату!", "Закрыть");
                    }
                }
                else
                {
                    await DisplayAlert("Ошибка!", "Минимальная длина пароля - 4 символа!", "Закрыть");
                }
            }
            else
            {
                await DisplayAlert("Ошибка!", "Поле для ввода пароля не может быть пустым", "Закрыть");
            }
        }
        else
        {
            await DisplayAlert("Ошибка!", "Поле для ввода номера телефона не может быть пустым", "Закрыть");
        }
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new PhoneEnterPage();
    }
}