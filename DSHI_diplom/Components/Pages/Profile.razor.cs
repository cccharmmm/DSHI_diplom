//using Microsoft.AspNetCore.Components;
//using Microsoft.JSInterop;
//using DSHI_diplom.Model;
//using Blazored.LocalStorage;

//namespace DSHI_diplom.Components.Pages
//{
//    public partial class Profile
//    {
//        [Inject] private HttpClient Http { get; set; }
//        [Inject] private ILocalStorageService LocalStorage { get; set; }
//        [Inject] private NavigationManager NavigationManager { get; set; }

//        private User _userProfile;
//        private string _token;
//        private bool _isLoading = true;

//        protected override async Task OnAfterRenderAsync(bool firstRender)
//        {
//            if (firstRender)
//            {
//                _token = await GetTokenAsync();

//                if (_token == null)
//                {
//                    NavigationManager.NavigateTo("/login");
//                    return;
//                }

//                var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7202/api/user/profile");
//                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

//                var response = await Http.SendAsync(requestMessage);

//                if (response.IsSuccessStatusCode)
//                {
//                    _userProfile = await response.Content.ReadFromJsonAsync<User>();
//                }
//                else
//                {
//                    var errorMessage = await response.Content.ReadAsStringAsync();
//                    Console.WriteLine($"Ошибка при получении профиля: {errorMessage}");
//                    NavigationManager.NavigateTo("/login");
//                }

//                _isLoading = false;
//                StateHasChanged();  // Обновляем UI
//            }
//        }

//        private async Task<string> GetTokenAsync()
//        {
//            return await LocalStorage.GetItemAsync<string>("authToken");
//        }
//    }
//}
