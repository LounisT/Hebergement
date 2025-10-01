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
            var jsonDoc = JsonDocument.Parse(json);
            var result = new List<HebergementViewModel>();

            foreach (var element in jsonDoc.RootElement.EnumerateArray())
            {
                var model = new HebergementViewModel
                {
                    Id = element.GetProperty("id").GetInt32(),
                    Nom = element.GetProperty("nom").GetString() ?? "",
                    Description = element.GetProperty("description").GetString() ?? "",
                    Adresse = element.GetProperty("adresse").GetString() ?? "",
                    Ville = element.GetProperty("ville").GetString() ?? "",
                    CodePostal = element.GetProperty("codePostal").GetString() ?? "",
                    CapaciteMax = element.GetProperty("capaciteMax").GetInt32(),
                    PrixParNuit = element.GetProperty("prixParNuit").GetDecimal(),
                    EstActif = element.GetProperty("estActif").GetBoolean(),
                    TypeHebergementId = element.GetProperty("typeHebergementId").GetInt32(),
                    DateCreation = element.GetProperty("dateCreation").GetDateTime()
                };

                // Charger les équipements pour chaque hébergement
                if (element.TryGetProperty("hebergementEquipements", out var equipements))
                {
                    foreach (var equip in equipements.EnumerateArray())
                    {
                        model.EquipementIds.Add(equip.GetProperty("equipementId").GetInt32());
                    }
                }

                result.Add(model);
            }

            return result;
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
            var jsonDoc = JsonDocument.Parse(json);
            var root = jsonDoc.RootElement;

            var model = new HebergementViewModel
            {
                Id = root.GetProperty("id").GetInt32(),
                Nom = root.GetProperty("nom").GetString() ?? "",
                Description = root.GetProperty("description").GetString() ?? "",
                Adresse = root.GetProperty("adresse").GetString() ?? "",
                Ville = root.GetProperty("ville").GetString() ?? "",
                CodePostal = root.GetProperty("codePostal").GetString() ?? "",
                CapaciteMax = root.GetProperty("capaciteMax").GetInt32(),
                PrixParNuit = root.GetProperty("prixParNuit").GetDecimal(),
                EstActif = root.GetProperty("estActif").GetBoolean(),
                TypeHebergementId = root.GetProperty("typeHebergementId").GetInt32(),
                DateCreation = root.GetProperty("dateCreation").GetDateTime()
            };

            // Extraire les IDs des équipements
            if (root.TryGetProperty("hebergementEquipements", out var equipements))
            {
                foreach (var equip in equipements.EnumerateArray())
                {
                    model.EquipementIds.Add(equip.GetProperty("equipementId").GetInt32());
                }
            }

            return model;
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
            var hebergement = new
            {
                model.Nom,
                model.Description,
                model.Adresse,
                model.Ville,
                model.CodePostal,
                model.CapaciteMax,
                model.PrixParNuit,
                model.EstActif,
                model.TypeHebergementId,
                model.DateCreation,
                HebergementEquipements = model.EquipementIds.Select(id => new
                {
                    EquipementId = id
                }).ToList()
            };

            var json = JsonSerializer.Serialize(hebergement);
            Console.WriteLine($"JSON envoyé: {json}");
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/api/Hebergements", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Erreur {response.StatusCode}: {errorContent}");
            }

            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateAsync(int id, HebergementViewModel model)
    {
        try
        {
            var hebergement = new
            {
                model.Id,
                model.Nom,
                model.Description,
                model.Adresse,
                model.Ville,
                model.CodePostal,
                model.CapaciteMax,
                model.PrixParNuit,
                model.EstActif,
                model.TypeHebergementId,
                model.DateCreation,
                HebergementEquipements = model.EquipementIds.Select(equipId => new
                {
                    HebergementId = id,
                    EquipementId = equipId
                }).ToList()
            };

            var json = JsonSerializer.Serialize(hebergement);
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

    public async Task<List<TypeHebergementDto>> GetTypesHebergementAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/Hebergements/types");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var types = JsonSerializer.Deserialize<List<TypeHebergementDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return types ?? new List<TypeHebergementDto>();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Erreur de connexion à l'API: {ex.Message}");
            return new List<TypeHebergementDto>();
        }
    }

    public async Task<List<CategorieEquipementDto>> GetEquipementsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/Equipements");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var equipements = JsonSerializer.Deserialize<List<CategorieEquipementDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return equipements ?? new List<CategorieEquipementDto>();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Erreur de connexion à l'API: {ex.Message}");
            return new List<CategorieEquipementDto>();
        }
    }
}

public class TypeHebergementDto
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
}

public class CategorieEquipementDto
{
    public int CategorieId { get; set; }
    public string CategorieNom { get; set; } = string.Empty;
    public List<EquipementDto> Equipements { get; set; } = new();
}

public class EquipementDto
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
}