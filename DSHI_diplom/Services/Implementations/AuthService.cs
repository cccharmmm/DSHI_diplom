using System.Net.Http.Json;
using System.Threading.Tasks;
using DSHI_diplom.Model;
using Blazored.LocalStorage;
using System.Text.Json;
using System.Net.Http.Headers;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public AuthService(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        Console.WriteLine($"Отправка запроса: Login={request.Login}, Password={request.Password}");

        var response = await _httpClient.PostAsJsonAsync("api/auth/login", request, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Invalid login attempt.");
        }

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

        if (result != null && !string.IsNullOrEmpty(result.Token))
        {
            await _localStorage.SetItemAsStringAsync("authToken", result.Token);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);

            Console.WriteLine($"Token: {result.Token}");
        }

        return result ?? throw new Exception("Не удалось получить токен.");
    }


    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<string?> GetToken()
    {
        return await _localStorage.GetItemAsStringAsync("authToken");
    }
}