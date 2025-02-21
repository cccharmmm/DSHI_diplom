﻿using Microsoft.AspNetCore.Components;
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
        [Inject] private HttpClient Http { get; set; }
        [Inject] private ILocalStorageService LocalStorage { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        private User _userProfile = new User();
        private string token;
        private string? successMessage;
        private string messageColor = "forestgreen";
        private bool showConfirmationModal = false;
        private bool _isFormValid = false;
        private bool isFormSubmitting = false;
        private bool isFormHandled = false;

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
        
         private void ConfirmSaveChanges()
        {
            showConfirmationModal = true;
            _isFormValid = true; 
            Console.WriteLine("Модальное окно открыто");
        }

        private void CancelSave()
        {
            showConfirmationModal = false;
            _isFormValid = false;
            isFormSubmitting = false;
            isFormHandled = false;
            Console.WriteLine("CancelSave вызван:");
            InvokeAsync(() => StateHasChanged());
            ResetForm();
            Console.WriteLine("Модальное окно закрыто");
        }

        private async Task SaveChanges()
        {
            showConfirmationModal = false;
            _isFormValid = false;
            isFormSubmitting = false;
            isFormHandled = false; 

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
                Console.WriteLine("Данные изменены");
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
            isFormSubmitting = false;
            isFormHandled = false;
        }
        private void ResetForm()
        {
            _isFormValid = false;
            showConfirmationModal = false;
            isFormSubmitting = false;
             isFormHandled = false;

        }
    }
}
