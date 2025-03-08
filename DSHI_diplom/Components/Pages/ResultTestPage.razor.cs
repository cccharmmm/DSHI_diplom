using DSHI_diplom.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;

namespace DSHI_diplom.Components.Pages
{
    public partial class ResultTestPage
    {
        private bool isLoading = true;
        private int attemptNumber = 0; 
        private TestResult? testResult;
        private string? currentUserName;
        private string errorMessage = string.Empty;
        private int correctAnswersCount = 0;
        private double correctAnswersPercentage = 0; 

        protected override void OnInitialized()
        {
            isLoading = true;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    Console.WriteLine("Загрузка результатов...");

                    var userId = await GetCurrentUserIdFromTokenAsync();
                    Console.WriteLine($"UserId из токена: {userId}");

                    testResult = await TestResultService.GetLatestResultByUserIdAsync(userId);
                    Console.WriteLine($"Результат из базы данных: {testResult?.Id}, Правильных ответов: {testResult?.CorrectAnswers}, Дата: {testResult?.Date}");

                    if (testResult == null)
                    {
                        errorMessage = "Результаты тестирования не найдены.";
                        Console.WriteLine("Результаты не найдены.");
                    }
                    else
                    {
                        currentUserName = await GetCurrentUserNameAsync();
                        Console.WriteLine($"Имя пользователя: {currentUserName}");

                        var questionCount = await TestResultService.GetQuestionCountByTestIdAsync(testResult.TestId);
                        Console.WriteLine($"Количество вопросов в тесте: {questionCount}");

                        correctAnswersCount = testResult.CorrectAnswers;
                        Console.WriteLine($"Правильных ответов: {correctAnswersCount}");

                        correctAnswersPercentage = CalculatePercentage(correctAnswersCount, questionCount);
                        Console.WriteLine($"Процент правильных ответов: {correctAnswersPercentage}%");

                        attemptNumber = await TestResultService.GetAttemptNumberAsync(testResult.TestId, userId);
                        Console.WriteLine($"Номер попытки: {attemptNumber}");
                    }
                }
                catch (Exception ex)
                {
                    errorMessage = $"Ошибка при загрузке результатов: {ex.Message}. Подробности: {ex.StackTrace}";
                    Console.Error.WriteLine(ex);
                }
                finally
                {
                    isLoading = false;
                    StateHasChanged();
                }
            }
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

        private double CalculatePercentage(int correctAnswers, int totalQuestions)
        {
            if (totalQuestions == 0)
            {
                return 0;
            }
            return Math.Round((double)correctAnswers / totalQuestions * 100, 2);
        }

        private void ReturnToTests()
        {
            Navigation.NavigateTo("/tests");
        }
        private void ViewAnswers(int testResultId)
        {
            if (testResultId > 0)
            {
                Navigation.NavigateTo($"/viewanswers/{testResultId}");
            }
            else
            {
                Console.WriteLine("ID результата теста некорректен.");
            }
        }
    }
}