using DSHI_diplom.Model;
using DSHI_diplom.Services.Implementations;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DSHI_diplom.Components.Pages
{
    public partial class TestsPage
    {
        [Inject] public required HttpClient HttpClient { get; set; }
        [Inject] public required NavigationManager NavigationManager { get; set; }

        private bool isLoading = true;
        private string searchText = string.Empty;
        private string errorMessage = string.Empty;
        private bool isSortOpen = false;
        private string? selectedClass = null;
        private string? selectedSubject = null;
        private bool isClassFilterOpen = false;
        private bool isSubjectFilterOpen = false;
        private string? currentSortBy = null;
        private List<Class> ClassList { get; set; } = new List<Class>();
        private List<Test> TestList { get; set; } = new List<Test>();
        private List<Subject> SubjectList { get; set; } = new List<Subject>();
        private List<Test> filteredTest { get; set; } = new List<Test>();

        protected override bool ShouldRender()
        {
            return !isLoading;
        }

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

        protected override async Task OnInitializedAsync()
        {
            try
            {
                TestList = await TestService.GetAllAsync();
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

        private void ToggleDropdown_ForSort()
        {
            isSortOpen = !isSortOpen;
        }

        private async Task ApplySorting()
        {
            try
            {
                isLoading = true;

                await Task.Run(() =>
                {
                    switch (currentSortBy)
                    {
                        case "alphabet":
                            filteredTest = TestList.OrderBy(n => n.Name).ToList();
                            break;
                        case "class":
                            filteredTest = TestList.OrderBy(n => n.Class != null ? n.Class.Name : string.Empty).ToList();
                            break;
                        default:
                            filteredTest = TestList.ToList();
                            break;
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сортировке тестов: {ex.Message}");
                errorMessage = "Не удалось загрузить данные. Пожалуйста, попробуйте позже.";
            }
            finally
            {
                isLoading = false;
            }
        }

        private async Task SortByAlphabet()
        {
            currentSortBy = "alphabet";
            await ApplySorting();
            await Task.Delay(200);
            isSortOpen = false;
        }

        private async Task SortByClass()
        {
            currentSortBy = "class";
            await ApplySorting();
            await Task.Delay(200);
            isSortOpen = false;
        }

        private void ApplyFilters()
        {
            filteredTest = TestList
                .Where(n =>
                    (selectedClass == null || n.Class?.Name == selectedClass) &&
                    (selectedSubject == null || n.Subject?.Name == selectedSubject))
                .ToList();
        }

        private void ToggleClassFilter()
        {
            isClassFilterOpen = !isClassFilterOpen;
            if (isClassFilterOpen)
            {
                isSubjectFilterOpen = false;
            }
        }

        private void ToggleSubjectFilter()
        {
            isSubjectFilterOpen = !isSubjectFilterOpen;
            if (isSubjectFilterOpen)
            {
                isClassFilterOpen = false;
            }
        }

        private void SelectClass(string class_)
        {
            selectedClass = class_;
            isClassFilterOpen = false;
            ApplyFilters();
        }

        private void SelectSubject(string subject)
        {
            selectedSubject = subject;
            isSubjectFilterOpen = false;
            ApplyFilters();
        }

        private void StartTest(Test test)
        {
            Console.WriteLine($"Начат тест: {test.Name}");
        }
    }
}