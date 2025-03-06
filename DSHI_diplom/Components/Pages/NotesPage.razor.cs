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
        [Inject] public required HttpClient HttpClient { get; set; }
        [Inject] public required NavigationManager NavigationManager { get; set; }

        private bool showAudioModal = false;
        private string audioUrl = string.Empty;
        private bool isSortOpen = false;
        private string searchText = string.Empty;
        private string? selectedInstrument = null;
        private string? selectedComposer = null;
        private string? selectedClass = null;
        private string? selectedMusicalForm = null;
        private bool isInstrumentFilterOpen = false;
        private bool isComposerFilterOpen = false;
        private bool isClassFilterOpen = false;
        private bool isMusicalFormFilterOpen = false;
        private bool isLoading = true;
        private string errorMessage = string.Empty;
        private string? currentSortBy = null;
        private List<Note> NotesList { get; set; } = new List<Note>();
        private List<Note> filteredNotes { get; set; } = new List<Note>();
        private List<Instrument> InstrumentList { get; set; } = new List<Instrument>();
        private List<Composer> ComposerList { get; set; } = new List<Composer>();
        private List<Class> ClassList { get; set; } = new List<Class>();
        private List<MusicalForm> MusicalFormList { get; set; } = new List<MusicalForm>();
        protected override bool ShouldRender()
        {
            return !isLoading; 
        }
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
        
        private void ToggleDropdown_ForSort()
        {
            isSortOpen = !isSortOpen;
        }
        
        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (NotesList == null || !NotesList.Any()) 
                {
                    NotesList = await NoteService.GetAllAsync();
                    InstrumentList = await InstrumentService.GetAllAsync();
                    ComposerList = await ComposerService.GetAllAsync();
                    ClassList = await ClassService.GetAllAsync();
                    MusicalFormList = await MusicalFormService.GetAllAsync();
                    filteredNotes = NotesList.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки данных: {ex.Message}");
            }
            finally
            {
                isLoading = false;
            }
        }
        private async Task OpenPdf(Note note)
        {
            if (note.File != null && !string.IsNullOrEmpty(note.File.Path))
            {
                var pdfUrl = $"{NavigationManager.BaseUri}pdf/{note.File.Path}";
                await JSRuntime.InvokeVoidAsync("window.open", pdfUrl, "_blank");
            }
            else
            {
                Console.WriteLine("Файл не найден");
            }
        }
        private string? GetDownloadUrl(Note note)
        {
            if (note.File != null && !string.IsNullOrEmpty(note.File.Path))
            {
                return $"{NavigationManager.BaseUri}pdf/{note.File.Path}";
            }
            return null;
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
                            filteredNotes = NotesList.OrderBy(n => n.DateOfCreate).ToList();
                            break;
                        case "alphabet":
                            filteredNotes = NotesList.OrderBy(n => n.Name).ToList();
                            break;
                        case "class":
                            filteredNotes = NotesList.OrderBy(n => n.Class != null ? n.Class.Name : string.Empty).ToList();
                            break;
                        default:
                            filteredNotes = NotesList.ToList();
                            break;
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сортировке нот: {ex.Message}");
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
            filteredNotes = NotesList
                .Where(n =>
                    (selectedInstrument == null || n.Instrument?.Name == selectedInstrument) &&
                    (selectedComposer == null || n.Composer?.Name == selectedComposer) &&
                    (selectedClass == null || n.Class?.Name == selectedClass) &&
                    (selectedMusicalForm == null || n.Musicalform?.Name == selectedMusicalForm))
                .ToList();
        }
        private void ToggleInstrumentFilter()
        {
            isInstrumentFilterOpen = !isInstrumentFilterOpen;
            if (isInstrumentFilterOpen)
            {
                isComposerFilterOpen = false;
                isClassFilterOpen = false;
                isMusicalFormFilterOpen = false;
            }
        }

        private void ToggleComposerFilter()
        {
            isComposerFilterOpen = !isComposerFilterOpen;
            if (isComposerFilterOpen)
            {
                isInstrumentFilterOpen = false;
                isClassFilterOpen = false;
                isMusicalFormFilterOpen = false;
            }
        }

        private void ToggleClassFilter()
        {
            isClassFilterOpen = !isClassFilterOpen;
            if (isClassFilterOpen)
            {
                isInstrumentFilterOpen = false;
                isComposerFilterOpen = false;
                isMusicalFormFilterOpen = false;
            }
        }

        private void ToggleMusicalFormFilter()
        {
            isMusicalFormFilterOpen = !isMusicalFormFilterOpen;
            if (isMusicalFormFilterOpen)
            {
                isInstrumentFilterOpen = false;
                isClassFilterOpen = false;
                isComposerFilterOpen = false;
            }
        }
        private void SelectInstrument(string instrument)
        {
            selectedInstrument = instrument;
            isInstrumentFilterOpen = false;
            ApplyFilters();
        }

        private void SelectComposer(string composer)
        {
            selectedComposer = composer;
            isComposerFilterOpen = false;
            ApplyFilters();
        }

        private void SelectClass(string class_)
        {
            selectedClass = class_;
            isClassFilterOpen = false;
            ApplyFilters();
        }

        private void SelectMusicalForm(string musicalForm)
        {
            selectedMusicalForm = musicalForm;
            isMusicalFormFilterOpen = false;
            ApplyFilters();
        }
    }
}
