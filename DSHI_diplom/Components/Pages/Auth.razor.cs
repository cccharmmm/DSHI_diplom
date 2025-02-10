using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DSHI_diplom.Components.Pages
{
    public partial class Auth
    {
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private HttpClient Http { get; set; } = null!;
        [Inject] private IJSRuntime JS { get; set; } = null!; 

        private string login = "";
        private string password = "";
        private string? errorMessage;

        private async Task Login()
        {
            errorMessage = null;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                errorMessage = "Заполните все поля";
                await HideErrorAfterDelay();
                return;
            }

            var request = new { Login = login, Password = password };

            try
            {
                var response = await Http.PostAsJsonAsync("api/auth/login", request);

                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();
                    await JS.InvokeVoidAsync("localStorage.setItem", "authToken", token);

                    NavigationManager.NavigateTo("/profile", true);
                }
                else
                {
                    errorMessage = "Неверный логин или пароль";
                    await HideErrorAfterDelay();
                }
            }
            catch
            {
                errorMessage = "Ошибка соединения с сервером";
                await HideErrorAfterDelay();
            }
        }

        private async Task HideErrorAfterDelay()
        {
            StateHasChanged();  
            await Task.Delay(3000); 
            errorMessage = null; 
            StateHasChanged(); 
        }
    }
}
