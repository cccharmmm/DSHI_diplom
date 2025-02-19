using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using DSHI_diplom.Model;
using Blazored.LocalStorage;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace DSHI_diplom.Components.Pages
{
    public partial class Profile
    {
        [Inject] private HttpClient Http { get; set; }
        [Inject] private ILocalStorageService LocalStorage { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        private User _userProfile = new User();
        private string token;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                token = await LocalStorage.GetItemAsync<string>("authToken");

                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("Токен не найден!");
                }
                else
                {
                    Console.WriteLine($"Токен: {token}");
                    await GetUserProfile();
                }

                StateHasChanged();
            }
        }

        private async Task GetUserProfile()
        {
            var token = await LocalStorage.GetItemAsync<string>("authToken");
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "api/user/me");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await Http.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _userProfile = JsonConvert.DeserializeObject<User>(content);
            }
            else
            {
                Console.WriteLine("Не удалось получить данные пользователя.");
            }
        }


    }
}
