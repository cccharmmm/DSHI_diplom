using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text;
using DSHI_diplom.Model;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Web;
using System.Net;

namespace DSHI_diplom.Components.Pages
{
    public partial class NotesPage
    {
        
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        private bool showAudioModal = false;
        private string audioUrl = string.Empty;
        private bool isDropdownOpen = false;
        private string searchText = string.Empty;
        private string selectedInstrument = null;
        private string selectedComposer = null;
        private string selectedClass = null;
        private string selectedMusicalForm = null;
        private bool isLoading = true;
        private string errorMessage = string.Empty;
        private string currentSortBy = null;
        private bool isSortedByDate = false;
        private bool isSortedByAlphabet = false;
        private bool isSortedByClass = false;

        private List<Note> NotesList { get; set; } = new List<Note>();
        private List<Note> filteredNotes { get; set; } = new List<Note>();
        private List<Instrument> InstrumentList { get; set; } = new List<Instrument>();
        private List<Composer> ComposerList { get; set; } = new List<Composer>();
        private List<Class> ClassList { get; set; } = new List<Class>();

        private List<MusicalForm> MusicalFormList { get; set; } = new List<MusicalForm>();
        private void OpenAudioModal(AudioFile audioFile)
        {
            if (audioFile != null && !string.IsNullOrEmpty(audioFile.Path))
            {
                audioUrl = $"{NavigationManager.BaseUri}audio/{audioFile.Path}";
                showAudioModal = true;
                StateHasChanged();
            }
            else
            {
                Console.WriteLine("Аудиофайл не найден");
            }
        }

        private void CloseAudioModal()
        {
            showAudioModal = false;
            audioUrl = string.Empty;
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
                filteredNotes = await NoteService.GetFilteredNotesBySearchAsync(searchText);

                Console.WriteLine($"Найдено нот по запросу '{searchText}': {filteredNotes.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при поиске нот: {ex.Message}");
            }
            finally
            {
                isLoading = false;
            }
        }

        private async Task SortByDate()
        {
            Console.WriteLine("Сортировка по дате");
            filteredNotes = await NoteService.GetSortedNotesAsync(filteredNotes, "date");
            currentSortBy = "date";
            isSortedByDate = true;
            isSortedByAlphabet = false;
            isSortedByClass = false;
            CloseDropdown();
            StateHasChanged();
        }

        private async Task SortByAlphabet()
        {
            Console.WriteLine("Сортировка по алфавиту");
            filteredNotes = await NoteService.GetSortedNotesAsync(filteredNotes, "alphabet");
            currentSortBy = "alphabet";
            isSortedByDate = false;
            isSortedByAlphabet = true;
            isSortedByClass = false;
            CloseDropdown();
            StateHasChanged();
        }

        private async Task SortByClass()
        {
            Console.WriteLine("Сортировка по классу");
            filteredNotes = await NoteService.GetSortedNotesAsync(filteredNotes, "class");
            currentSortBy = "class";
            isSortedByDate = false;
            isSortedByAlphabet = false;
            isSortedByClass = true;
            CloseDropdown();
            StateHasChanged();
        }

        private void CloseDropdown()
        {
            isDropdownOpen = false;
            StateHasChanged();
        }

        private void ToggleDropdown()
        {
            isDropdownOpen = !isDropdownOpen;
            StateHasChanged();
        }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                NotesList = await NoteService.GetAllAsync();
                Console.WriteLine($"Загружено нот: {NotesList.Count}");
                InstrumentList = await InstrumentService.GetAllAsync();
                ComposerList = await ComposerService.GetAllAsync();
                ClassList = await ClassService.GetAllAsync();
                MusicalFormList = await MusicalFormService.GetAllAsync();
                filteredNotes = NotesList;
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
        private async Task SelectInstrument(string instrumentName)
        {
            Console.WriteLine("Метод SelectInstrument вызван");
            selectedInstrument = instrumentName;
            Console.WriteLine($"Выбран инструмент: {selectedInstrument}");
            await ApplyFilters();
        }
        private async Task SelectComposer(string composerName)
        {
            selectedComposer = composerName;
            Console.WriteLine($"Выбран композитор: {selectedComposer}");
            await ApplyFilters();
        }

        private async Task SelectClass(string className)
        {
            selectedClass = className;
            Console.WriteLine($"Выбран класс: {selectedClass}");
            await ApplyFilters();
        }

        private async Task SelectMusicalForm(string musicalFormName)
        {
            selectedMusicalForm = musicalFormName;
            Console.WriteLine($"Выбрана музыкальная форма: {selectedMusicalForm}");
            await ApplyFilters();
        }
        private async Task OpenPdf(Note note)
        {
            if (note.File != null && !string.IsNullOrEmpty(note.File.Path))
            {
                var pdfUrl = $"{NavigationManager.BaseUri}pdf/{note.File.Path}";
                Console.WriteLine($"URL: {pdfUrl}"); 
                await JSRuntime.InvokeVoidAsync("window.open", pdfUrl, "_blank");
            }
            else
            {
                Console.WriteLine("Файл не найден");
            }
        }
        private string GetDownloadUrl(Note note)
        {
            if (note.File != null && !string.IsNullOrEmpty(note.File.Path))
            {
                return $"{NavigationManager.BaseUri}pdf/{note.File.Path}";
            }
            return null;
        }
        private async Task ApplyFilters()
        {
            try
            {
                isLoading = true;
                StateHasChanged();

                filteredNotes = await NoteService.GetFilteredNotesAsync(selectedInstrument, selectedComposer, selectedClass, selectedMusicalForm);

                filteredNotes = await NoteService.GetSortedNotesAsync(filteredNotes, currentSortBy);

                Console.WriteLine($"Отфильтровано и отсортировано нот: {filteredNotes.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при фильтрации и сортировке нот: {ex.Message}");
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
