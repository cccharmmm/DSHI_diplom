using DSHI_diplom.Model;
using DSHI_diplom.Services.Implementations;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DSHI_diplom.Components.Pages
{
    public partial class TheoryPage
    {
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        private bool isLoading = true;
        private string searchText = string.Empty;
        private string errorMessage = string.Empty;
        private string selectedAuthor = null;
        private string selectedSubject = null;
        private string selectedClass = null;
        private List<Class> ClassList { get; set; } = new List<Class>();
        private List<TheoreticalMaterial> TheoryList { get; set; } = new List<TheoreticalMaterial>();
        private List<Subject> SubjectList { get; set; } = new List<Subject>();
        private List<Author> AuthorList { get; set; } = new List<Author>();
        private List<TheoreticalMaterial> filteredTheory { get; set; } = new List<TheoreticalMaterial>();
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
                Console.WriteLine($"Найдено теории по запросу '{searchText}': {filteredTheory.Count}");
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
        private async Task SelectAuthor(string authorName)
        {
            Console.WriteLine("Метод SelectAuthor вызван");
            selectedAuthor = authorName;
            Console.WriteLine($"Выбран автор: {selectedAuthor}");
            await ApplyFilters();
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
        private async Task OpenPdf(TheoreticalMaterial tm)
        {
            if (tm.File != null && !string.IsNullOrEmpty(tm.File.Path))
            {
                var pdfUrl = $"{NavigationManager.BaseUri}pdf/{tm.File.Path}";
                Console.WriteLine($"URL: {pdfUrl}");
                await JSRuntime.InvokeVoidAsync("window.open", pdfUrl, "_blank");
            }
            else
            {
                Console.WriteLine("Файл не найден");
                Console.WriteLine($"File: {tm.File}, Path: {tm.File?.Path}");
            }
        }
        private string GetDownloadUrl(TheoreticalMaterial tm)
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
                Console.WriteLine($"Загружено теории: {TheoryList.Count}");
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
        private async Task ApplyFilters()
        {
            try
            {
                isLoading = true;
                StateHasChanged();

                // Применение фильтров
                filteredTheory = await TheoreticalMaterialService.GetFilteredTheoryAsync(selectedAuthor, selectedSubject, selectedClass);

                //// Применение сортировки (если выбрана)
                //if (isSortedByDate)
                //    filteredNotes = filteredNotes.OrderBy(n => n.DateOfCreate).ToList();
                //else if (isSortedByAlphabet)
                //    filteredNotes = filteredNotes.OrderBy(n => n.Name).ToList();
                //else if (isSortedByClass)
                //    filteredNotes = filteredNotes.OrderBy(n => n.Class?.Name).ToList();

                Console.WriteLine($"Отфильтровано и отсортировано теории: {filteredTheory.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при фильтрации и сортировке теории: {ex.Message}");
                errorMessage = "Не удалось загрузить данные. Пожалуйста, попробуйте позже.";
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }
    }
}
