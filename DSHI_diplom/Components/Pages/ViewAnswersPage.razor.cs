using DSHI_diplom.Model;
using DSHI_diplom.Services.Implementations;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
namespace DSHI_diplom.Components.Pages
{
    public partial class ViewAnswersPage
    {
        [Parameter]
        public int TestResultId { get; set; }

        private TestResult? testResult;
        private bool isLoading = true;
        private string? errorMessage;
        protected override async Task OnParametersSetAsync()
        {
            try
            {
                isLoading = true;
                testResult = await TestResultService.GetTestResultByIdAsync(TestResultId);
            }
            catch (Exception ex)
            {
                errorMessage = "Ошибка при загрузке ответов: " + ex.Message;
            }
            finally
            {
                isLoading = false;
            }
        }

        private void ReturnToResults()
        {
            Navigation.NavigateTo("/testresults");
        }

    }
}