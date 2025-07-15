using HebergementManager.Web.Models;
using System.Text.Json;
using System.Text;

namespace HebergementManager.Web.Services;

public class HebergementService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public HebergementService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        
        // Lire l'URL de l'API depuis la configuration
        _baseUrl = configuration["ApiSettings:BaseUrl"];
        
        // Ou utiliser directement l'URL
        // _baseUrl = "https://localhost:7000";
    }

    public async Task<List<HebergementViewModel>> GetAllAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/Hebergements");
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();
            var hebergements = JsonSerializer.Deserialize<List<HebergementViewModel>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            return hebergements ?? new List<HebergementViewModel>();
        }
        catch (HttpRequestException ex)
        {
            // Log l'erreur
            Console.WriteLine($"Erreur de connexion à l'API: {ex.Message}");
            
            // Retourner des données de test si l'API n'est pas disponible
            return null;
        }
    }

    public async Task<HebergementViewModel?> GetByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/Hebergements/{id}");
            if (!response.IsSuccessStatusCode)
                return null;
            
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<HebergementViewModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<bool> CreateAsync(HebergementViewModel model)
    {
        try
        {
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/Hebergements", content);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
            return false;
        }
    }

    public async Task<bool> UpdateAsync(int id, HebergementViewModel model)
    {
        try
        {
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"{_baseUrl}/api/Hebergements/{id}", content);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/Hebergements/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
            return false;
        }
    }
    
}