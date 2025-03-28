using Blazored.LocalStorage;
using CustomerUI.Models;
using CustomerUI.Models.Dtos;
using System.Net.Http.Json;
using System.Text.Json;

namespace CustomerUI.Services
{
    public class ProjectService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public ProjectService(IHttpClientFactory clientFactory, ILocalStorageService localStorage)
        {
            _httpClient = clientFactory.CreateClient("ProjectService");
            _localStorage = localStorage;
        }

        public async Task<List<ProjectDto>> GetProjectsAsync()
        {
            var response = await _httpClient.GetAsync("api/project");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ProjectDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<ProjectDto>();
        }

        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Project>($"api/Project/{id}");
        }

        //public async Task CreateProjectAsync(Project project)
        //{
        //    await _httpClient.PostAsJsonAsync("api/Project", project);
        //}

        public async Task<Project?> CreateProjectAsync(Project project)
        {
          
            var json = JsonSerializer.Serialize(project);
            Console.WriteLine($"JSON inviato: {json}"); // Debug

            var response = await _httpClient.PostAsJsonAsync("api/Project", project);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Errore API: {response.StatusCode} - {errorContent}");
                throw new HttpRequestException($"Errore API: {response.StatusCode} - {errorContent}");
            }

            return null;
        }

        public async Task UpdateProjectAsync(int id, Project project)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Project/{project.ProjectId}", project);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Errore API: {response.StatusCode} - {errorContent}");
                throw new HttpRequestException($"Errore API: {response.StatusCode} - {errorContent}");
            }

        }

        public async Task DeleteProjectAsync(int id)
        {
            await _httpClient.DeleteAsync($"api/Project/{id}");
        }
    }
}
