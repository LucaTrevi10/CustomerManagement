using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public AuthService(HttpClient http, ILocalStorageService localStorage)
    {
        _http = http;
        _localStorage = localStorage;
    }

    public async Task<bool> Login(string username, string password)
    {
        var response = await _http.PostAsJsonAsync("http://localhost:5257/api/auth/login", new { Username = username, Password = password });

        if (!response.IsSuccessStatusCode)
            return false;

        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        await _localStorage.SetItemAsync("token", result.Token);

        // Aggiorna le richieste HTTP con il nuovo token
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);
        return true;
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("token");
        _http.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<string> GetToken()
    {
        return await _localStorage.GetItemAsStringAsync("token");
    }
}

public class AuthResponse
{
    public string Token { get; set; }
}
