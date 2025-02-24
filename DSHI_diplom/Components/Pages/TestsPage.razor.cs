using DSHI_diplom.Model;
using DSHI_diplom.Services.Implementations;
using Microsoft.AspNetCore.Components;

namespace DSHI_diplom.Components.Pages
{
    public partial class TestsPage
    {
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        private bool isLoading = true;
        private string searchText = string.Empty;
        private string errorMessage = string.Empty;
        private string selectedSubject = null;
        private string selectedClass = null;
        private List<Class> ClassList { get; set; } = new List<Class>();
        private List<Test> TestList { get; set; } = new List<Test>();
        private List<Subject> SubjectList { get; set; } = new List<Subject>();
        private List<Test> filteredTest { get; set; } = new List<Test>();
        private async Task HandleInput(ChangeEventArgs e)
        {
            searchText = e.Value?.ToString() ?? string.Empty;
            await DebounceSearch();
        }
        private async Task DebounceSearch()
        {
            await Task.Delay(300);
            await HandleSearch();
        }

        private async Task HandleSearch()
        {
            try
            {
                isLoading = true;
                filteredTest = await TestService.GetFilteredTestBySearchAsync(searchText);
                Console.WriteLine($"Найдено тестов по запросу '{searchText}': {filteredTest.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при поиске тестов: {ex.Message}");
            }
            finally
            {
                isLoading = false;
            }
        }
        private async Task SelectSubject(string subjectName)
        {
            Console.WriteLine("Метод SelectSubject вызван");
            selectedSubject = subjectName;
            Console.WriteLine($"Выбран предмет: {selectedSubject}");
            await ApplyFilters();
        }

        private async Task SelectClass(string className)
        {
            selectedClass = className;
            Console.WriteLine($"Выбран класс: {selectedClass}");
            await ApplyFilters();
        }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                TestList = await TestService.GetAllAsync();
                Console.WriteLine($"Загружено тестов: {TestList.Count}");
                ClassList = await ClassService.GetAllAsync();
                SubjectList = await SubjectService.GetAllAsync();
                filteredTest = TestList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки данных: {ex.Message}");
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }
        private async Task ApplyFilters()
        {
            try
            {
                isLoading = true;
                StateHasChanged();

                // Применение фильтров
                filteredTest = await TestService.GetFilteredTestAsync(selectedSubject, selectedClass);

                //// Применение сортировки (если выбрана)
                //if (isSortedByDate)
                //    filteredNotes = filteredNotes.OrderBy(n => n.DateOfCreate).ToList();
                //else if (isSortedByAlphabet)
                //    filteredNotes = filteredNotes.OrderBy(n => n.Name).ToList();
                //else if (isSortedByClass)
                //    filteredNotes = filteredNotes.OrderBy(n => n.Class?.Name).ToList();

                Console.WriteLine($"Отфильтровано и отсортировано теcтов: {filteredTest.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при фильтрации и сортировке тестов: {ex.Message}");
                errorMessage = "Не удалось загрузить данные. Пожалуйста, попробуйте позже.";
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }
        private void StartTest(Test test)
        {
            Console.WriteLine($"Начат тест: {test.Name}"); 
        }
    }
}
