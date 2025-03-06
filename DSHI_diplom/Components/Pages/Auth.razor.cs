using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text;
using DSHI_diplom.Model;
using System.Security.Claims;

namespace DSHI_diplom.Components.Pages
{
    public partial class Auth
    {
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private HttpClient Http { get; set; } = null!;
        [Inject] private IJSRuntime JS { get; set; } = null!;
        [Inject] private ILocalStorageService LocalStorage { get; set; } = null!;


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
                    var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>(); 

                    if (loginResponse?.Token != null)
                    {
                        Console.WriteLine($"Token: {loginResponse.Token}");
                        await JS.InvokeVoidAsync("localStorage.setItem", "authToken", loginResponse.Token);

                        if (await ValidateRoleFromToken(loginResponse.Token))
                        {
                            NavigationManager.NavigateTo("/profile", true);
                        }
                        else
                        {
                            errorMessage = "У вас нет прав доступа. Пожалуйста, обратитесь к администратору.";
                            await HideErrorAfterDelay();
                        }
                    }
                    else
                    {
                        errorMessage = "Ошибка получения токена";
                        await HideErrorAfterDelay();
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    errorMessage = "Неверный логин или пароль";
                    await HideErrorAfterDelay();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    errorMessage = "Доступ запрещен: неподходящая роль пользователя.";
                    await HideErrorAfterDelay();
                }
                else
                {
                    errorMessage = "Произошла ошибка при авторизации.";
                    await HideErrorAfterDelay();
                }
            }
            catch (HttpRequestException)
            {
                errorMessage = "Ошибка соединения с сервером. Проверьте подключение.";
                await HideErrorAfterDelay();
            }
            catch (Exception ex)
            {
                errorMessage = $"Произошла непредвиденная ошибка: {ex.Message}";
                await HideErrorAfterDelay();
            }
        }

        private async Task<bool> ValidateRoleFromToken(string token)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token);

                    var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

                    if (roleClaim == null)
                    {
                        Console.WriteLine("Роль не найдена в токене.");
                        return false;
                    }

                    Console.WriteLine($"Роль пользователя: {roleClaim.Value}");

                    return roleClaim.Value == "user";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка в валидации токена: {ex.Message}");
                    return false;
                }
            });
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
