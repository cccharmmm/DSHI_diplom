using DSHI_diplom.Model;
using DSHI_diplom.Services.Implementations;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DSHI_diplom.Components.Pages
{
    public partial class TheoryPage
    {
        [Inject] public required HttpClient HttpClient { get; set; }
        [Inject] public required NavigationManager NavigationManager { get; set; }

        private bool isLoading = true;
        private string searchText = string.Empty;
        private string errorMessage = string.Empty;
        private bool isSortOpen = false;
        private string? selectedAuthor = null;
        private string? selectedSubject = null;
        private string? selectedClass = null;
        private bool isSubjectFilterOpen = false;
        private bool isAuthorFilterOpen = false;
        private bool isClassFilterOpen = false;
        private string? currentSortBy = null;
        private List<Class> ClassList { get; set; } = new List<Class>();
        private List<TheoreticalMaterial> TheoryList { get; set; } = new List<TheoreticalMaterial>();
        private List<Subject> SubjectList { get; set; } = new List<Subject>();
        private List<Author> AuthorList { get; set; } = new List<Author>();
        private List<TheoreticalMaterial> filteredTheory { get; set; } = new List<TheoreticalMaterial>();
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
                filteredTheory = await TheoreticalMaterialService.GetFilteredTheoryBySearchAsync(searchText);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при поиске теории: {ex.Message}");
            }
            finally
            {
                isLoading = false;
            }
        }
        
        private async Task OpenPdf(TheoreticalMaterial tm)
        {
            if (tm.File != null && !string.IsNullOrEmpty(tm.File.Path))
            {
                var pdfUrl = $"{NavigationManager.BaseUri}pdf/{tm.File.Path}";
                await JSRuntime.InvokeVoidAsync("window.open", pdfUrl, "_blank");
            }
            else
            {
                Console.WriteLine("Файл не найден");
                Console.WriteLine($"File: {tm.File}, Path: {tm.File?.Path}");
            }
        }
        private string? GetDownloadUrl(TheoreticalMaterial tm)
        {
            if (tm.File != null && !string.IsNullOrEmpty(tm.File.Path))
            {
                return $"{NavigationManager.BaseUri}pdf/{tm.File.Path}";
            }
            return null;
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                TheoryList = await TheoreticalMaterialService.GetAllAsync();
                ClassList = await ClassService.GetAllAsync();
                SubjectList = await SubjectService.GetAllAsync();
                AuthorList = await AuthorService.GetAllAsync();
                filteredTheory = TheoryList;
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
                        case "date":
                            filteredTheory = TheoryList.OrderBy(n => n.DateOfCreate).ToList();
                            break;
                        case "alphabet":
                            filteredTheory = TheoryList.OrderBy(n => n.Name).ToList();
                            break;
                        case "class":
                            filteredTheory = TheoryList
                                .OrderBy(n => n.ClassId)
                                .ToList();
                            break;
                        default:
                            filteredTheory = TheoryList.ToList();
                            break;
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сортировке теории: {ex.Message}");
                errorMessage = "Не удалось загрузить данные. Пожалуйста, попробуйте позже.";
            }
            finally
            {
                isLoading = false;
            }
        }
        private async Task SortByDate()
        {
            currentSortBy = "date";
            await ApplySorting();
            await Task.Delay(200);
            isSortOpen = false;
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
            if (!string.IsNullOrEmpty(selectedClass))
            {
                filteredTheory = TheoryList
                    .Where(n => n.Class?.Name == selectedClass)
                    .ToList();
            }
            else if (!string.IsNullOrEmpty(selectedSubject))
            {
                filteredTheory = TheoryList
                    .Where(n => n.Subject?.Name == selectedSubject)
                    .ToList();
            }
            else if (!string.IsNullOrEmpty(selectedAuthor))
            {
                filteredTheory = TheoryList
                    .Where(n => n.Author?.Name == selectedAuthor)
                    .ToList();
            }
            else
            {
                filteredTheory = TheoryList.ToList();
            }
        }
        private void ResetFilters()
        {
            selectedAuthor = null;
            selectedSubject = null;
            selectedClass = null;
        }
        private void ToggleClassFilter()
        {
            isClassFilterOpen = !isClassFilterOpen;
            if (isClassFilterOpen)
            {
                isSubjectFilterOpen = false;
                isAuthorFilterOpen = false;
            }
        }

        private void ToggleSubjectFilter()
        {
            isSubjectFilterOpen = !isSubjectFilterOpen;
            if (isSubjectFilterOpen)
            {
                isClassFilterOpen = false;
                isAuthorFilterOpen = false;
            }
        }

        private void ToggleAuthorFilter()
        {
            isAuthorFilterOpen = !isAuthorFilterOpen;
            if (isAuthorFilterOpen)
            {
                isSubjectFilterOpen = false;
                isClassFilterOpen = false;
            }
        }
        
        private void SelectSubject(string subject)
        {
            ResetFilters();
            selectedSubject = subject;
            isSubjectFilterOpen = false;
            ApplyFilters();
        }

        private void SelectAuthor(string author)
        {
            ResetFilters();
            selectedAuthor = author;
            isAuthorFilterOpen = false;
            ApplyFilters();
        }

        private void SelectClass(string class_)
        {
            ResetFilters();
            selectedClass = class_;
            isClassFilterOpen = false;
            ApplyFilters();
        }

    }
}
