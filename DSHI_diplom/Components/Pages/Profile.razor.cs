using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using DSHI_diplom.Model;
using Blazored.LocalStorage;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Forms;

namespace DSHI_diplom.Components.Pages
{
    public partial class Profile
    {
        [Inject] public required HttpClient Http { get; set; }
        [Inject] public required ILocalStorageService LocalStorage { get; set; }
        [Inject] public required NavigationManager NavigationManager { get; set; }

        private User _userProfile = new User();
        private string? token;
        private string? successMessage;
        private string messageColor = "forestgreen";
        private bool showConfirmationModal = false;
        private bool _isFormValid = false;

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
                _userProfile = JsonConvert.DeserializeObject<User>(content)!;
            }
            else
            {
                Console.WriteLine("Не удалось получить данные пользователя.");
            }
        }

        private void ValidateAndOpenModal()
        {
            var editContext = new EditContext(_userProfile);
            _isFormValid = editContext.Validate();

            if (_isFormValid)
            {
                showConfirmationModal = true;
            }
            else
            {
                Console.WriteLine("Форма невалидна, модальное окно не открыто.");
            }
        }

        private void CancelSave()
        {
            showConfirmationModal = false;
            _isFormValid = false;
            InvokeAsync(() => StateHasChanged());
            ResetForm();
            Console.WriteLine("Модальное окно закрыто");
        }

        private async Task SaveChanges()
        {
            showConfirmationModal = false;
            _isFormValid = false;

            await InvokeAsync(StateHasChanged);

            Console.WriteLine($"showConfirmationModal: {showConfirmationModal}, _isFormValid: {_isFormValid}");

            var token = await LocalStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrEmpty(token))
            {
                successMessage = "Ошибка: токен не найден";
                messageColor = "red";
                await HideErrorAfterDelay();
                return;
            }

            var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"api/user/{_userProfile.Id}");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(_userProfile));
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await Http.SendAsync(requestMessage);
            if (response.IsSuccessStatusCode)
            {
                successMessage = "Данные успешно сохранены";
                messageColor = "forestgreen";
                await InvokeAsync(StateHasChanged);
                await HideErrorAfterDelay();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                successMessage = "Ошибка сохранения данных";
                messageColor = "red";
                await HideErrorAfterDelay();
                await InvokeAsync(StateHasChanged);
                Console.WriteLine($"Ошибка сохранения: {errorContent}");
            }

            ResetForm();
        }
        private async Task HideErrorAfterDelay()
        {
            await Task.Delay(3000);
            successMessage = null;
            await InvokeAsync(StateHasChanged);
        }
     
        private void HandleInvalidSubmit()
        {
            _isFormValid = false;
            showConfirmationModal = false;
        }
        private void ResetForm()
        {
            _isFormValid = false;
            showConfirmationModal = false;

        }
    }
}
