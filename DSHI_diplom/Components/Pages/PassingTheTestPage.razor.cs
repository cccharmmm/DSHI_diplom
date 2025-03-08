using Blazored.LocalStorage;
using DSHI_diplom.Model;
using DSHI_diplom.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DSHI_diplom.Components.Pages
{
    public partial class PassingTheTestPage 
    {
        [Inject] public required HttpClient HttpClient { get; set; }
        [Inject] public required NavigationManager NavigationManager { get; set; }
        [Inject] public required ILocalStorageService LocalStorage { get; set; }

        [Parameter]
        public int TestId { get; set; }

        private bool isLoading = true;
        private bool showCompleteModal = false;
        private bool showExitModal = false;
        private bool showIncompleteModal = false;
        private string errorMessage = string.Empty;
        private Test? currentTest;
        private Dictionary<int, int?> selectedAnswers = new Dictionary<int, int?>();
        private string? currentUserName;
        private TaskCompletionSource<bool>? _userConfirmationTaskCompletionSource;

        private async Task OnBeforeNavigation(LocationChangingContext context)
        {
            if (!AreAllQuestionsAnswered())
            {
                ShowExitConfirmation();
                var isConfirmed = await WaitForUserConfirmation();

                if (!isConfirmed)
                {
                    context.PreventNavigation();
                }
            }
        }

        private async Task<bool> WaitForUserConfirmation()
        {
            _userConfirmationTaskCompletionSource = new TaskCompletionSource<bool>();
            return await _userConfirmationTaskCompletionSource.Task;
        }

       
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                currentUserName = await GetCurrentUserNameAsync();
                StateHasChanged();
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                currentTest = await TestService.GetByIdAsync(TestId);
                if (currentTest != null)
                {
                    currentTest.Questions = await TestService.GetQuestionsByTestIdAsync(TestId);
                    foreach (var question in currentTest.Questions)
                    {
                        question.Answers = await TestService.GetAnswersByQuestionIdAsync(question.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Произошла ошибка: {ex.Message}";
                Console.Error.WriteLine(ex);
            }
            finally
            {
                isLoading = false;
            }
        }

        private bool AreAllQuestionsAnswered()
        {
            if (currentTest == null || currentTest.Questions == null)
            {
                return false;
            }

            return currentTest.Questions.All(question => selectedAnswers.ContainsKey(question.Id));
        }

        private void SelectAnswer(int questionId, int answerId)
        {
            selectedAnswers[questionId] = answerId;
        }

        private async Task<string> GetCurrentUserNameAsync()
        {
            var token = await LocalStorage.GetItemAsStringAsync("authToken");
            if (string.IsNullOrEmpty(token))
            {
                return "Гость";
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userNameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            return userNameClaim?.Value ?? "Гость";
        }

        private async Task<int> GetCurrentUserIdFromTokenAsync()
        {
            var token = await LocalStorage.GetItemAsStringAsync("authToken");
            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("Пользователь не аутентифицирован.");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }

            throw new UnauthorizedAccessException("Не удалось получить ID пользователя.");
        }

        private async Task SubmitTest()
        {
            if (currentTest != null)
            {
                if (!AreAllQuestionsAnswered())
                {
                    showIncompleteModal = true;
                    return;
                }

                showCompleteModal = true;
            }

            await Task.CompletedTask;
        }

        private async Task ConfirmTestCompletion()
        {
            showCompleteModal = false;

            if (currentTest == null)
            {
                errorMessage = "Тест не найден.";
                return;
            }

            try
            {
                var userId = await GetCurrentUserIdFromTokenAsync();

                var result = new TestResult
                {
                    TestId = currentTest.Id,
                    UserId = userId,
                    CorrectAnswers = CalculateCorrectAnswers(),
                    Date = DateOnly.FromDateTime(DateTime.Now)
                };

                await TestResultService.AddAsync(result);

                Navigation.NavigateTo("/testresults");
            }
            catch (UnauthorizedAccessException ex)
            {
                errorMessage = $"Ошибка аутентификации: {ex.Message}";
            }
            catch (Exception ex)
            {
                errorMessage = $"Произошла ошибка: {ex.Message}";
            }
        }

        private int CalculateCorrectAnswers()
        {
            if (currentTest == null || currentTest.Questions == null)
            {
                return 0;
            }

            int correctAnswers = 0;
            foreach (var question in currentTest.Questions)
            {
                if (selectedAnswers.TryGetValue(question.Id, out int? selectedAnswerId))
                {
                    var selectedAnswer = question.Answers.FirstOrDefault(a => a.Id == selectedAnswerId);
                    if (selectedAnswer != null && selectedAnswer.Rightt)
                    {
                        correctAnswers++;
                    }
                }
            }
            return correctAnswers;
        }

        private void ShowExitConfirmation()
        {
            showExitModal = true;
            StateHasChanged();
        }

        private void ConfirmExit()
        {
            showExitModal = false;
            _userConfirmationTaskCompletionSource?.TrySetResult(true); 
            NavigationManager.NavigateTo("/tests");
        }

        private void CancelExit()
        {
            showExitModal = false;
            _userConfirmationTaskCompletionSource?.TrySetResult(false); 
            StateHasChanged();
        }
    }
}